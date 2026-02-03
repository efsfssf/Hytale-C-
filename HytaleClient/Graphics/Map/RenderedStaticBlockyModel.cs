using System;
using HytaleClient.Data.BlockyModels;
using HytaleClient.Graphics.BlockyModels;
using HytaleClient.Math;

namespace HytaleClient.Graphics.Map
{
	// Token: 0x02000A93 RID: 2707
	internal class RenderedStaticBlockyModel
	{
		// Token: 0x0600553D RID: 21821 RVA: 0x0018DB90 File Offset: 0x0018BD90
		public RenderedStaticBlockyModel(BlockyModel model)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			bool flag = true;
			int num5 = 0;
			for (int i = 0; i < model.NodeCount; i++)
			{
				BlockyModelNode blockyModelNode = model.AllNodes[i];
				bool flag2 = blockyModelNode.Type == BlockyModelNode.ShapeType.None;
				if (!flag2)
				{
					foreach (BlockyModelFaceTextureLayout blockyModelFaceTextureLayout in blockyModelNode.TextureLayout)
					{
						bool flag3 = !blockyModelFaceTextureLayout.Hidden;
						if (flag3)
						{
							bool visible = blockyModelNode.Visible;
							if (visible)
							{
								num += 4;
								num2 += 2;
							}
							num3 += 4;
							num4 += 2;
						}
					}
					bool flag4 = blockyModelNode.Type == BlockyModelNode.ShapeType.Box;
					if (flag4)
					{
						flag = false;
						bool flag5 = num5 == 0 && num2 > 0 && model.Lod == LodMode.Auto;
						if (flag5)
						{
							num5 = (int)((ushort)(num2 * 3));
						}
					}
				}
			}
			switch (model.Lod)
			{
			case LodMode.Off:
				this.LowLODIndicesCount = (ushort)(num2 * 3);
				break;
			case LodMode.Auto:
			{
				int num6 = num2 / 2;
				this.LowLODIndicesCount = (flag ? ((ushort)(num6 / 2 * 6)) : ((ushort)num5));
				break;
			}
			case LodMode.Billboard:
				this.LowLODIndicesCount = 6;
				break;
			case LodMode.Disappear:
				this.LowLODIndicesCount = 0;
				break;
			default:
				throw new Exception("Unreachable");
			}
			this.HasOnlyQuads = flag;
			this.UsesBillboardLOD = (model.Lod == LodMode.Billboard);
			this.StaticVertices = new StaticBlockyModelVertex[num];
			this.StaticIndices = new ushort[num2 * 3];
			int num7 = 0;
			int num8 = 0;
			this.AnimatedVertices = new StaticBlockyModelVertex[num3];
			this.AnimatedIndices = new ushort[num4 * 3];
			int num9 = 0;
			int num10 = 0;
			this.NodeParentTransforms = new AnimatedRenderer.NodeTransform[model.NodeCount];
			byte b = 0;
			while ((int)b < model.NodeCount)
			{
				ref BlockyModelNode ptr = ref model.AllNodes[(int)b];
				bool flag6 = ptr.Type == BlockyModelNode.ShapeType.Quad;
				float num11;
				float num12;
				float num13;
				if (flag6)
				{
					switch (ptr.QuadNormalDirection)
					{
					case BlockyModelNode.QuadNormal.PlusZ:
					case BlockyModelNode.QuadNormal.MinusZ:
						goto IL_298;
					case BlockyModelNode.QuadNormal.PlusX:
					case BlockyModelNode.QuadNormal.MinusX:
						num11 = 0f;
						num12 = ptr.Size.Y * ptr.Stretch.Y;
						num13 = ptr.Size.X * ptr.Stretch.Z;
						break;
					case BlockyModelNode.QuadNormal.PlusY:
					case BlockyModelNode.QuadNormal.MinusY:
						num11 = ptr.Size.X * ptr.Stretch.X;
						num12 = 0f;
						num13 = ptr.Size.Y * ptr.Stretch.Z;
						break;
					default:
						goto IL_298;
					}
					goto IL_32D;
					IL_298:
					num11 = ptr.Size.X * ptr.Stretch.X;
					num12 = ptr.Size.Y * ptr.Stretch.Y;
					num13 = 0f;
				}
				else
				{
					num11 = ptr.Size.X * ptr.Stretch.X;
					num12 = ptr.Size.Y * ptr.Stretch.Y;
					num13 = ptr.Size.Z * ptr.Stretch.Z;
				}
				IL_32D:
				Matrix matrix;
				Matrix.CreateScale(num11, num12, num13, out matrix);
				RenderedStaticBlockyModel.SetupVertices(b, ref ptr, ref matrix, ref num9, this.AnimatedVertices, ref num10, this.AnimatedIndices);
				bool flag7 = !ptr.Visible;
				if (!flag7)
				{
					this.NodeParentTransforms[(int)b].Position = Vector3.Transform(ptr.Offset, ptr.Orientation) + Vector3.Transform(ptr.ProceduralOffset, Quaternion.Identity) + ptr.Position;
					Quaternion quaternion = Quaternion.CreateFromYawPitchRoll(ptr.ProceduralRotation.Yaw, ptr.ProceduralRotation.Pitch, ptr.ProceduralRotation.Roll);
					this.NodeParentTransforms[(int)b].Orientation = quaternion * ptr.Orientation;
					int num14 = model.ParentNodes[(int)b];
					bool flag8 = num14 >= 0;
					if (flag8)
					{
						this.NodeParentTransforms[(int)b].Position = Vector3.Transform(this.NodeParentTransforms[(int)b].Position, this.NodeParentTransforms[num14].Orientation) + this.NodeParentTransforms[num14].Position;
						this.NodeParentTransforms[(int)b].Orientation = this.NodeParentTransforms[num14].Orientation * this.NodeParentTransforms[(int)b].Orientation;
					}
					Matrix.Compose(num11, num12, num13, this.NodeParentTransforms[(int)b].Orientation, this.NodeParentTransforms[(int)b].Position, out matrix);
					RenderedStaticBlockyModel.SetupVertices(b, ref ptr, ref matrix, ref num7, this.StaticVertices, ref num8, this.StaticIndices);
				}
				b += 1;
			}
		}

		// Token: 0x0600553E RID: 21822 RVA: 0x0018E0B0 File Offset: 0x0018C2B0
		public void PrepareUVs(BlockyModel model, Point textureSize, Point atlasSize)
		{
			int num = 0;
			int num2 = 0;
			byte b = 0;
			while ((int)b < model.NodeCount)
			{
				ref BlockyModelNode ptr = ref model.AllNodes[(int)b];
				RenderedStaticBlockyModel.SetupUVs(ref ptr, ref num, this.AnimatedVertices, textureSize, atlasSize);
				bool flag = !ptr.Visible;
				if (!flag)
				{
					RenderedStaticBlockyModel.SetupUVs(ref ptr, ref num2, this.StaticVertices, textureSize, atlasSize);
				}
				b += 1;
			}
		}

		// Token: 0x0600553F RID: 21823 RVA: 0x0018E11C File Offset: 0x0018C31C
		private static void SetupVertices(byte nodeIndex, ref BlockyModelNode node, ref Matrix nodeMatrix, ref int verticesOffset, StaticBlockyModelVertex[] vertices, ref int indicesOffset, ushort[] indices)
		{
			bool useDefaultVertexOrder = node.Stretch.X > 0f ^ node.Stretch.Y > 0f ^ node.Stretch.Z > 0f;
			int num = 0;
			BlockyModelNode.ShapeType type = node.Type;
			BlockyModelNode.ShapeType shapeType = type;
			if (shapeType != BlockyModelNode.ShapeType.Box)
			{
				if (shapeType == BlockyModelNode.ShapeType.Quad)
				{
					bool flag = !node.TextureLayout[0].Hidden;
					if (flag)
					{
						Vector3[] sourceArray = (node.QuadNormalDirection == BlockyModelNode.QuadNormal.PlusZ) ? RenderedStaticBlockyModel.QuadCornersPlusZ : ((node.QuadNormalDirection == BlockyModelNode.QuadNormal.MinusZ) ? RenderedStaticBlockyModel.QuadCornersMinusZ : ((node.QuadNormalDirection == BlockyModelNode.QuadNormal.PlusX) ? RenderedStaticBlockyModel.QuadCornersPlusX : ((node.QuadNormalDirection == BlockyModelNode.QuadNormal.MinusX) ? RenderedStaticBlockyModel.QuadCornersMinusX : ((node.QuadNormalDirection == BlockyModelNode.QuadNormal.PlusY) ? RenderedStaticBlockyModel.QuadCornersPlusY : RenderedStaticBlockyModel.QuadCornersMinusY))));
						Vector3.Transform(sourceArray, ref nodeMatrix, RenderedStaticBlockyModel.TempCorners);
						RenderedStaticBlockyModel.SetupQuad(vertices, nodeIndex, node.ShadingMode, node.DoubleSided, verticesOffset, 1, 0, 3, 2, useDefaultVertexOrder);
						RenderedStaticBlockyModel.SetupQuadIndices(indices, indicesOffset, verticesOffset + num * 4, useDefaultVertexOrder);
						num++;
						indicesOffset += 6;
						verticesOffset += num * 4;
					}
				}
			}
			else
			{
				Vector3.Transform(ModelRenderer.BoxCorners, ref nodeMatrix, RenderedStaticBlockyModel.TempCorners);
				bool flag2 = !node.TextureLayout[0].Hidden;
				if (flag2)
				{
					RenderedStaticBlockyModel.SetupQuad(vertices, nodeIndex, node.ShadingMode, node.DoubleSided, verticesOffset + num * 4, 4, 5, 6, 7, useDefaultVertexOrder);
					RenderedStaticBlockyModel.SetupQuadIndices(indices, indicesOffset, verticesOffset + num * 4, useDefaultVertexOrder);
					num++;
					indicesOffset += 6;
				}
				bool flag3 = !node.TextureLayout[1].Hidden;
				if (flag3)
				{
					RenderedStaticBlockyModel.SetupQuad(vertices, nodeIndex, node.ShadingMode, node.DoubleSided, verticesOffset + num * 4, 0, 1, 2, 3, useDefaultVertexOrder);
					RenderedStaticBlockyModel.SetupQuadIndices(indices, indicesOffset, verticesOffset + num * 4, useDefaultVertexOrder);
					num++;
					indicesOffset += 6;
				}
				bool flag4 = !node.TextureLayout[2].Hidden;
				if (flag4)
				{
					RenderedStaticBlockyModel.SetupQuad(vertices, nodeIndex, node.ShadingMode, node.DoubleSided, verticesOffset + num * 4, 1, 4, 7, 2, useDefaultVertexOrder);
					RenderedStaticBlockyModel.SetupQuadIndices(indices, indicesOffset, verticesOffset + num * 4, useDefaultVertexOrder);
					num++;
					indicesOffset += 6;
				}
				bool flag5 = !node.TextureLayout[3].Hidden;
				if (flag5)
				{
					RenderedStaticBlockyModel.SetupQuad(vertices, nodeIndex, node.ShadingMode, node.DoubleSided, verticesOffset + num * 4, 7, 6, 3, 2, useDefaultVertexOrder);
					RenderedStaticBlockyModel.SetupQuadIndices(indices, indicesOffset, verticesOffset + num * 4, useDefaultVertexOrder);
					num++;
					indicesOffset += 6;
				}
				bool flag6 = !node.TextureLayout[4].Hidden;
				if (flag6)
				{
					RenderedStaticBlockyModel.SetupQuad(vertices, nodeIndex, node.ShadingMode, node.DoubleSided, verticesOffset + num * 4, 5, 0, 3, 6, useDefaultVertexOrder);
					RenderedStaticBlockyModel.SetupQuadIndices(indices, indicesOffset, verticesOffset + num * 4, useDefaultVertexOrder);
					num++;
					indicesOffset += 6;
				}
				bool flag7 = !node.TextureLayout[5].Hidden;
				if (flag7)
				{
					RenderedStaticBlockyModel.SetupQuad(vertices, nodeIndex, node.ShadingMode, node.DoubleSided, verticesOffset + num * 4, 1, 0, 5, 4, useDefaultVertexOrder);
					RenderedStaticBlockyModel.SetupQuadIndices(indices, indicesOffset, verticesOffset + num * 4, useDefaultVertexOrder);
					num++;
					indicesOffset += 6;
				}
				verticesOffset += num * 4;
			}
		}

		// Token: 0x06005540 RID: 21824 RVA: 0x0018E484 File Offset: 0x0018C684
		private static void SetupUVs(ref BlockyModelNode node, ref int verticesOffset, StaticBlockyModelVertex[] vertices, Point textureSize, Point atlasSize)
		{
			int num = 0;
			BlockyModelNode.ShapeType type = node.Type;
			BlockyModelNode.ShapeType shapeType = type;
			if (shapeType != BlockyModelNode.ShapeType.Box)
			{
				if (shapeType == BlockyModelNode.ShapeType.Quad)
				{
					bool flag = !node.TextureLayout[0].Hidden;
					if (flag)
					{
						RenderedStaticBlockyModel.SetupQuadUV(vertices, verticesOffset + num * 4, ref node.TextureLayout[0], RenderedStaticBlockyModel.GetShapeTextureFaceSize(ref node, "front"), textureSize, atlasSize);
						num++;
						verticesOffset += num * 4;
					}
				}
			}
			else
			{
				bool flag2 = !node.TextureLayout[0].Hidden;
				if (flag2)
				{
					RenderedStaticBlockyModel.SetupQuadUV(vertices, verticesOffset + num * 4, ref node.TextureLayout[0], RenderedStaticBlockyModel.GetShapeTextureFaceSize(ref node, "front"), textureSize, atlasSize);
					num++;
				}
				bool flag3 = !node.TextureLayout[1].Hidden;
				if (flag3)
				{
					RenderedStaticBlockyModel.SetupQuadUV(vertices, verticesOffset + num * 4, ref node.TextureLayout[1], RenderedStaticBlockyModel.GetShapeTextureFaceSize(ref node, "back"), textureSize, atlasSize);
					num++;
				}
				bool flag4 = !node.TextureLayout[2].Hidden;
				if (flag4)
				{
					RenderedStaticBlockyModel.SetupQuadUV(vertices, verticesOffset + num * 4, ref node.TextureLayout[2], RenderedStaticBlockyModel.GetShapeTextureFaceSize(ref node, "right"), textureSize, atlasSize);
					num++;
				}
				bool flag5 = !node.TextureLayout[3].Hidden;
				if (flag5)
				{
					RenderedStaticBlockyModel.SetupQuadUV(vertices, verticesOffset + num * 4, ref node.TextureLayout[3], RenderedStaticBlockyModel.GetShapeTextureFaceSize(ref node, "bottom"), textureSize, atlasSize);
					num++;
				}
				bool flag6 = !node.TextureLayout[4].Hidden;
				if (flag6)
				{
					RenderedStaticBlockyModel.SetupQuadUV(vertices, verticesOffset + num * 4, ref node.TextureLayout[4], RenderedStaticBlockyModel.GetShapeTextureFaceSize(ref node, "left"), textureSize, atlasSize);
					num++;
				}
				bool flag7 = !node.TextureLayout[5].Hidden;
				if (flag7)
				{
					RenderedStaticBlockyModel.SetupQuadUV(vertices, verticesOffset + num * 4, ref node.TextureLayout[5], RenderedStaticBlockyModel.GetShapeTextureFaceSize(ref node, "top"), textureSize, atlasSize);
					num++;
				}
				verticesOffset += num * 4;
			}
		}

		// Token: 0x06005541 RID: 21825 RVA: 0x0018E6B4 File Offset: 0x0018C8B4
		private static void SetupQuad(StaticBlockyModelVertex[] vertices, byte nodeIndex, ShadingMode shadingMode, bool doubleSided, int offset, int a, int b, int c, int d, bool useDefaultVertexOrder)
		{
			int num = offset + 1;
			int num2 = offset + 2;
			vertices[offset + 3].NodeIndex = nodeIndex;
			vertices[num2].NodeIndex = nodeIndex;
			vertices[num].NodeIndex = nodeIndex;
			vertices[offset].NodeIndex = nodeIndex;
			vertices[offset].Position = RenderedStaticBlockyModel.TempCorners[a];
			vertices[offset + 1].Position = RenderedStaticBlockyModel.TempCorners[b];
			vertices[offset + 2].Position = RenderedStaticBlockyModel.TempCorners[c];
			vertices[offset + 3].Position = RenderedStaticBlockyModel.TempCorners[d];
			uint doubleSided2 = doubleSided ? 1U : 0U;
			vertices[offset].DoubleSided = doubleSided2;
			vertices[offset + 1].DoubleSided = doubleSided2;
			vertices[offset + 2].DoubleSided = doubleSided2;
			vertices[offset + 3].DoubleSided = doubleSided2;
			Vector3 normal = useDefaultVertexOrder ? Vector3.Cross(vertices[offset].Position - vertices[offset + 1].Position, vertices[offset].Position - vertices[offset + 2].Position) : Vector3.Cross(vertices[offset + 1].Position - vertices[offset].Position, vertices[offset + 1].Position - vertices[offset + 3].Position);
			normal.Normalize();
			vertices[offset].Normal = (vertices[offset + 1].Normal = (vertices[offset + 2].Normal = (vertices[offset + 3].Normal = normal)));
			int num3 = offset + 1;
			int num4 = offset + 2;
			vertices[offset + 3].ShadingMode = shadingMode;
			vertices[num4].ShadingMode = shadingMode;
			vertices[num3].ShadingMode = shadingMode;
			vertices[offset].ShadingMode = shadingMode;
		}

		// Token: 0x06005542 RID: 21826 RVA: 0x0018E8E4 File Offset: 0x0018CAE4
		private static void SetupQuadUV(StaticBlockyModelVertex[] vertices, int offset, ref BlockyModelFaceTextureLayout faceData, Point size, Point textureSize, Point atlasSize)
		{
			int x = faceData.Offset.X;
			int x2 = faceData.Offset.X + (faceData.MirrorX ? -1 : 1) * size.X;
			int num = textureSize.Y - faceData.Offset.Y;
			int y = textureSize.Y - (faceData.Offset.Y + (faceData.MirrorY ? -1 : 1) * size.Y);
			RenderedStaticBlockyModel.TempPoints[0] = new Point(x2, num);
			RenderedStaticBlockyModel.TempPoints[1] = new Point(x, num);
			RenderedStaticBlockyModel.TempPoints[2] = new Point(x, y);
			RenderedStaticBlockyModel.TempPoints[3] = new Point(x2, y);
			int num2 = 1;
			int num3 = 0;
			int angle = faceData.Angle;
			int num4 = angle;
			if (num4 != 90)
			{
				if (num4 != 180)
				{
					if (num4 == 270)
					{
						num2 = 0;
						num3 = 1;
					}
				}
				else
				{
					num2 = -1;
					num3 = 0;
				}
			}
			else
			{
				num2 = 0;
				num3 = -1;
			}
			int num5 = int.MaxValue;
			int num6 = int.MaxValue;
			for (int i = 0; i < 4; i++)
			{
				Point point = RenderedStaticBlockyModel.TempPoints[i];
				int x3 = x + (point.X - x) * num2 - (point.Y - num) * num3;
				int num7 = num + (point.X - x) * num3 + (point.Y - num) * num2;
				RenderedStaticBlockyModel.TempPoints[i].X = x3;
				RenderedStaticBlockyModel.TempPoints[i].Y = textureSize.Y - num7;
				num5 = Math.Min(num5, RenderedStaticBlockyModel.TempPoints[i].X);
				num6 = Math.Min(num6, RenderedStaticBlockyModel.TempPoints[i].Y);
			}
			for (int j = 0; j < 4; j++)
			{
				Point point2 = RenderedStaticBlockyModel.TempPoints[j];
				float num8 = (point2.X == num5) ? 0.04f : -0.04f;
				float num9 = (point2.Y == num6) ? 0.04f : -0.04f;
				vertices[offset + j].TextureCoordinates.X = ((float)point2.X + num8) / (float)atlasSize.X;
				vertices[offset + j].TextureCoordinates.Y = ((float)point2.Y + num9) / (float)atlasSize.Y;
			}
		}

		// Token: 0x06005543 RID: 21827 RVA: 0x0018EB70 File Offset: 0x0018CD70
		private static void SetupQuadIndices(ushort[] indices, int indicesOffset, int targetOffset, bool useDefaultVertexOrder)
		{
			if (useDefaultVertexOrder)
			{
				indices[indicesOffset] = (ushort)targetOffset;
				indices[indicesOffset + 1] = (ushort)(targetOffset + 1);
				indices[indicesOffset + 2] = (ushort)(targetOffset + 2);
				indices[indicesOffset + 3] = (ushort)targetOffset;
				indices[indicesOffset + 4] = (ushort)(targetOffset + 2);
				indices[indicesOffset + 5] = (ushort)(targetOffset + 3);
			}
			else
			{
				indices[indicesOffset] = (ushort)(targetOffset + 1);
				indices[indicesOffset + 1] = (ushort)targetOffset;
				indices[indicesOffset + 2] = (ushort)(targetOffset + 3);
				indices[indicesOffset + 3] = (ushort)(targetOffset + 1);
				indices[indicesOffset + 4] = (ushort)(targetOffset + 3);
				indices[indicesOffset + 5] = (ushort)(targetOffset + 2);
			}
		}

		// Token: 0x06005544 RID: 21828 RVA: 0x0018EBEC File Offset: 0x0018CDEC
		private static Point GetShapeTextureFaceSize(ref BlockyModelNode node, string faceName)
		{
			BlockyModelNode.ShapeType type = node.Type;
			BlockyModelNode.ShapeType shapeType = type;
			if (shapeType != BlockyModelNode.ShapeType.Box)
			{
				if (shapeType == BlockyModelNode.ShapeType.Quad)
				{
					if (faceName == "front")
					{
						return new Point((int)node.Size.X, (int)node.Size.Y);
					}
				}
			}
			else
			{
				if (faceName == "front" || faceName == "back")
				{
					return new Point((int)node.Size.X, (int)node.Size.Y);
				}
				if (faceName == "left" || faceName == "right")
				{
					return new Point((int)node.Size.Z, (int)node.Size.Y);
				}
				if (faceName == "top" || faceName == "bottom")
				{
					return new Point((int)node.Size.X, (int)node.Size.Z);
				}
			}
			throw new Exception("Unreachable");
		}

		// Token: 0x04003205 RID: 12805
		public readonly StaticBlockyModelVertex[] StaticVertices;

		// Token: 0x04003206 RID: 12806
		public readonly ushort[] StaticIndices;

		// Token: 0x04003207 RID: 12807
		public readonly StaticBlockyModelVertex[] AnimatedVertices;

		// Token: 0x04003208 RID: 12808
		public readonly ushort[] AnimatedIndices;

		// Token: 0x04003209 RID: 12809
		public readonly ushort LowLODIndicesCount;

		// Token: 0x0400320A RID: 12810
		public readonly bool HasOnlyQuads;

		// Token: 0x0400320B RID: 12811
		public readonly bool UsesBillboardLOD;

		// Token: 0x0400320C RID: 12812
		public AnimatedRenderer.NodeTransform[] NodeParentTransforms;

		// Token: 0x0400320D RID: 12813
		private static readonly Vector3[] QuadCornersPlusZ = new Vector3[]
		{
			new Vector3(-0.5f, 0.5f, 0f),
			new Vector3(0.5f, 0.5f, 0f),
			new Vector3(0.5f, -0.5f, 0f),
			new Vector3(-0.5f, -0.5f, 0f)
		};

		// Token: 0x0400320E RID: 12814
		private static readonly Vector3[] QuadCornersMinusZ = new Vector3[]
		{
			new Vector3(0.5f, 0.5f, 0f),
			new Vector3(-0.5f, 0.5f, 0f),
			new Vector3(-0.5f, -0.5f, 0f),
			new Vector3(0.5f, -0.5f, 0f)
		};

		// Token: 0x0400320F RID: 12815
		private static readonly Vector3[] QuadCornersPlusX = new Vector3[]
		{
			new Vector3(0f, 0.5f, 0.5f),
			new Vector3(0f, 0.5f, -0.5f),
			new Vector3(0f, -0.5f, -0.5f),
			new Vector3(0f, -0.5f, 0.5f)
		};

		// Token: 0x04003210 RID: 12816
		private static readonly Vector3[] QuadCornersMinusX = new Vector3[]
		{
			new Vector3(0f, 0.5f, -0.5f),
			new Vector3(0f, 0.5f, 0.5f),
			new Vector3(0f, -0.5f, 0.5f),
			new Vector3(0f, -0.5f, -0.5f)
		};

		// Token: 0x04003211 RID: 12817
		private static readonly Vector3[] QuadCornersPlusY = new Vector3[]
		{
			new Vector3(-0.5f, 0f, -0.5f),
			new Vector3(0.5f, 0f, -0.5f),
			new Vector3(0.5f, 0f, 0.5f),
			new Vector3(-0.5f, 0f, 0.5f)
		};

		// Token: 0x04003212 RID: 12818
		private static readonly Vector3[] QuadCornersMinusY = new Vector3[]
		{
			new Vector3(-0.5f, 0f, 0.5f),
			new Vector3(0.5f, 0f, 0.5f),
			new Vector3(0.5f, 0f, -0.5f),
			new Vector3(-0.5f, 0f, -0.5f)
		};

		// Token: 0x04003213 RID: 12819
		private static readonly Vector3[] TempCorners = new Vector3[8];

		// Token: 0x04003214 RID: 12820
		private static readonly Point[] TempPoints = new Point[4];
	}
}
