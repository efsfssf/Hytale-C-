using System;
using System.Runtime.CompilerServices;
using HytaleClient.Data.BlockyModels;
using HytaleClient.Graphics.Programs;
using HytaleClient.Math;

namespace HytaleClient.Graphics.BlockyModels
{
	// Token: 0x02000AC2 RID: 2754
	internal class ModelRenderer : AnimatedRenderer
	{
		// Token: 0x17001346 RID: 4934
		// (get) Token: 0x060056D3 RID: 22227 RVA: 0x001A1F20 File Offset: 0x001A0120
		public uint Timestamp
		{
			get
			{
				return this._timestamp;
			}
		}

		// Token: 0x17001347 RID: 4935
		// (get) Token: 0x060056D4 RID: 22228 RVA: 0x001A1F28 File Offset: 0x001A0128
		// (set) Token: 0x060056D5 RID: 22229 RVA: 0x001A1F30 File Offset: 0x001A0130
		public GLVertexArray VertexArray { get; private set; }

		// Token: 0x060056D6 RID: 22230 RVA: 0x001A1F3C File Offset: 0x001A013C
		public ModelRenderer(BlockyModel model, Point[] atlasSizes, GraphicsDevice graphics, uint timestamp, bool selfManageNodeBuffer = false) : base(model, atlasSizes, selfManageNodeBuffer)
		{
			this._timestamp = timestamp;
			ModelRenderer.MakeGeometry(model, atlasSizes, out this._vertices, out this._indices);
			this.IndicesCount = this._indices.Length;
			bool flag = graphics != null;
			if (flag)
			{
				this.CreateGPUData(graphics);
			}
		}

		// Token: 0x060056D7 RID: 22231 RVA: 0x001A1F90 File Offset: 0x001A0190
		public unsafe override void CreateGPUData(GraphicsDevice graphics)
		{
			base.CreateGPUData(graphics);
			GLFunctions gl = graphics.GL;
			this.VertexArray = gl.GenVertexArray();
			gl.BindVertexArray(this.VertexArray);
			this._verticesBuffer = gl.GenBuffer();
			gl.BindBuffer(this.VertexArray, GL.ARRAY_BUFFER, this._verticesBuffer);
			ModelVertex[] array;
			ModelVertex* value;
			if ((array = this._vertices) == null || array.Length == 0)
			{
				value = null;
			}
			else
			{
				value = &array[0];
			}
			gl.BufferData(GL.ARRAY_BUFFER, (IntPtr)(this._vertices.Length * ModelVertex.Size), (IntPtr)((void*)value), GL.STATIC_DRAW);
			array = null;
			this._vertices = null;
			this._indicesBuffer = gl.GenBuffer();
			gl.BindBuffer(this.VertexArray, GL.ELEMENT_ARRAY_BUFFER, this._indicesBuffer);
			ushort[] array2;
			ushort* value2;
			if ((array2 = this._indices) == null || array2.Length == 0)
			{
				value2 = null;
			}
			else
			{
				value2 = &array2[0];
			}
			gl.BufferData(GL.ELEMENT_ARRAY_BUFFER, (IntPtr)(this.IndicesCount * 2), (IntPtr)((void*)value2), GL.STATIC_DRAW);
			array2 = null;
			this._indices = null;
			BlockyModelProgram blockyModelProgram = graphics.GPUProgramStore.BlockyModelProgram;
			IntPtr pointer = IntPtr.Zero;
			gl.EnableVertexAttribArray(blockyModelProgram.AttribNodeIndex.Index);
			gl.VertexAttribIPointer(blockyModelProgram.AttribNodeIndex.Index, 1, GL.UNSIGNED_INT, ModelVertex.Size, pointer);
			pointer += 4;
			gl.EnableVertexAttribArray(blockyModelProgram.AttribAtlasIndexAndShadingModeAndGradientId.Index);
			gl.VertexAttribIPointer(blockyModelProgram.AttribAtlasIndexAndShadingModeAndGradientId.Index, 1, GL.UNSIGNED_INT, ModelVertex.Size, pointer);
			pointer += 4;
			gl.EnableVertexAttribArray(blockyModelProgram.AttribPosition.Index);
			gl.VertexAttribPointer(blockyModelProgram.AttribPosition.Index, 3, GL.FLOAT, false, ModelVertex.Size, pointer);
			pointer += 12;
			gl.EnableVertexAttribArray(blockyModelProgram.AttribTexCoords.Index);
			gl.VertexAttribPointer(blockyModelProgram.AttribTexCoords.Index, 2, GL.FLOAT, false, ModelVertex.Size, pointer);
			pointer += 8;
		}

		// Token: 0x060056D8 RID: 22232 RVA: 0x001A21CC File Offset: 0x001A03CC
		protected override void DoDispose()
		{
			base.DoDispose();
			bool flag = this._graphics != null;
			if (flag)
			{
				GLFunctions gl = this._graphics.GL;
				gl.DeleteBuffer(this._verticesBuffer);
				gl.DeleteBuffer(this._indicesBuffer);
				gl.DeleteVertexArray(this.VertexArray);
			}
		}

		// Token: 0x060056D9 RID: 22233 RVA: 0x001A2224 File Offset: 0x001A0424
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Draw()
		{
			bool disposed = base.Disposed;
			if (disposed)
			{
				throw new ObjectDisposedException(typeof(ModelRenderer).FullName);
			}
			GLFunctions gl = this._graphics.GL;
			gl.BindVertexArray(this.VertexArray);
			gl.DrawElements(GL.TRIANGLES, this.IndicesCount, GL.UNSIGNED_SHORT, (IntPtr)0);
		}

		// Token: 0x060056DA RID: 22234 RVA: 0x001A2284 File Offset: 0x001A0484
		private static void MakeGeometry(BlockyModel model, Point[] atlasSizes, out ModelVertex[] vertices, out ushort[] indices)
		{
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < model.NodeCount; i++)
			{
				bool flag = model.AllNodes[i].Type == BlockyModelNode.ShapeType.None;
				if (!flag)
				{
					foreach (BlockyModelFaceTextureLayout blockyModelFaceTextureLayout in model.AllNodes[i].TextureLayout)
					{
						bool flag2 = !blockyModelFaceTextureLayout.Hidden;
						if (flag2)
						{
							num += 4;
							num2 += (model.AllNodes[i].DoubleSided ? 4 : 2);
						}
					}
				}
			}
			vertices = new ModelVertex[num];
			indices = new ushort[num2 * 3];
			int num3 = 0;
			int num4 = 0;
			uint num5 = 0U;
			while ((ulong)num5 < (ulong)((long)model.NodeCount))
			{
				ModelRenderer.SetupVertices(ref model.AllNodes[(int)num5], num5, ref num3, vertices, ref num4, indices, atlasSizes);
				num5 += 1U;
			}
		}

		// Token: 0x060056DB RID: 22235 RVA: 0x001A2390 File Offset: 0x001A0590
		private static void SetupVertices(ref BlockyModelNode node, uint nodeIndex, ref int verticesOffset, ModelVertex[] vertices, ref int indicesOffset, ushort[] indices, Point[] atlasSizes)
		{
			bool flag = node.Stretch.X > 0f ^ node.Stretch.Y > 0f ^ node.Stretch.Z > 0f;
			byte gradientId = node.GradientId;
			BlockyModelNode.ShapeType type = node.Type;
			BlockyModelNode.ShapeType shapeType = type;
			if (shapeType != BlockyModelNode.ShapeType.Box)
			{
				if (shapeType == BlockyModelNode.ShapeType.Quad)
				{
					Vector3 vector = node.Size / 2f;
					bool flag2 = !node.TextureLayout[0].Hidden;
					if (flag2)
					{
						bool flag3 = node.QuadNormalDirection == BlockyModelNode.QuadNormal.PlusZ;
						if (flag3)
						{
							vector.X *= node.Stretch.X;
							vector.Y *= node.Stretch.Y;
							ModelRenderer.TempCorners[0] = new Vector3(-vector.X, vector.Y, 0f);
							ModelRenderer.TempCorners[1] = new Vector3(vector.X, vector.Y, 0f);
							ModelRenderer.TempCorners[2] = new Vector3(vector.X, -vector.Y, 0f);
							ModelRenderer.TempCorners[3] = new Vector3(-vector.X, -vector.Y, 0f);
						}
						else
						{
							bool flag4 = node.QuadNormalDirection == BlockyModelNode.QuadNormal.MinusZ;
							if (flag4)
							{
								vector.X *= node.Stretch.X;
								vector.Y *= node.Stretch.Y;
								ModelRenderer.TempCorners[0] = new Vector3(vector.X, vector.Y, 0f);
								ModelRenderer.TempCorners[1] = new Vector3(-vector.X, vector.Y, 0f);
								ModelRenderer.TempCorners[2] = new Vector3(-vector.X, -vector.Y, 0f);
								ModelRenderer.TempCorners[3] = new Vector3(vector.X, -vector.Y, 0f);
							}
							else
							{
								bool flag5 = node.QuadNormalDirection == BlockyModelNode.QuadNormal.PlusX;
								if (flag5)
								{
									vector.X *= node.Stretch.Z;
									vector.Y *= node.Stretch.Y;
									ModelRenderer.TempCorners[0] = new Vector3(0f, vector.Y, vector.X);
									ModelRenderer.TempCorners[1] = new Vector3(0f, vector.Y, -vector.X);
									ModelRenderer.TempCorners[2] = new Vector3(0f, -vector.Y, -vector.X);
									ModelRenderer.TempCorners[3] = new Vector3(0f, -vector.Y, vector.X);
								}
								else
								{
									bool flag6 = node.QuadNormalDirection == BlockyModelNode.QuadNormal.MinusX;
									if (flag6)
									{
										vector.X *= node.Stretch.Z;
										vector.Y *= node.Stretch.Y;
										ModelRenderer.TempCorners[0] = new Vector3(0f, vector.Y, -vector.X);
										ModelRenderer.TempCorners[1] = new Vector3(0f, vector.Y, vector.X);
										ModelRenderer.TempCorners[2] = new Vector3(0f, -vector.Y, vector.X);
										ModelRenderer.TempCorners[3] = new Vector3(0f, -vector.Y, -vector.X);
									}
									else
									{
										bool flag7 = node.QuadNormalDirection == BlockyModelNode.QuadNormal.PlusY;
										if (flag7)
										{
											vector.X *= node.Stretch.X;
											vector.Y *= node.Stretch.Z;
											ModelRenderer.TempCorners[0] = new Vector3(-vector.X, 0f, -vector.Y);
											ModelRenderer.TempCorners[1] = new Vector3(vector.X, 0f, -vector.Y);
											ModelRenderer.TempCorners[2] = new Vector3(vector.X, 0f, vector.Y);
											ModelRenderer.TempCorners[3] = new Vector3(-vector.X, 0f, vector.Y);
										}
										else
										{
											bool flag8 = node.QuadNormalDirection == BlockyModelNode.QuadNormal.MinusY;
											if (flag8)
											{
												vector.X *= node.Stretch.X;
												vector.Y *= node.Stretch.Z;
												ModelRenderer.TempCorners[0] = new Vector3(-vector.X, 0f, vector.Y);
												ModelRenderer.TempCorners[1] = new Vector3(vector.X, 0f, vector.Y);
												ModelRenderer.TempCorners[2] = new Vector3(vector.X, 0f, -vector.Y);
												ModelRenderer.TempCorners[3] = new Vector3(-vector.X, 0f, -vector.Y);
											}
										}
									}
								}
							}
						}
						ModelRenderer.SetupQuad(nodeIndex, node.AtlasIndex, (byte)node.ShadingMode, gradientId, vertices, verticesOffset, 1, 0, 3, 2);
						ModelRenderer.SetupQuadUV(vertices, verticesOffset, ref node.TextureLayout[0], ModelRenderer.GetShapeTextureFaceSize(ref node, "front"), atlasSizes[(int)node.AtlasIndex]);
						ModelRenderer.SetupQuadIndices(indices, indicesOffset, verticesOffset, flag);
						indicesOffset += 6;
						bool doubleSided = node.DoubleSided;
						if (doubleSided)
						{
							ModelRenderer.SetupQuadIndices(indices, indicesOffset, verticesOffset, !flag);
							indicesOffset += 6;
						}
						verticesOffset += 4;
					}
				}
			}
			else
			{
				Matrix matrix = Matrix.CreateScale(node.Size * node.Stretch);
				Vector3.Transform(ModelRenderer.BoxCorners, ref matrix, ModelRenderer.TempCorners);
				int num = 0;
				bool flag9 = !node.TextureLayout[0].Hidden;
				if (flag9)
				{
					ModelRenderer.SetupQuad(nodeIndex, node.AtlasIndex, (byte)node.ShadingMode, gradientId, vertices, verticesOffset + num * 4, 4, 5, 6, 7);
					ModelRenderer.SetupQuadUV(vertices, verticesOffset + num * 4, ref node.TextureLayout[0], ModelRenderer.GetShapeTextureFaceSize(ref node, "front"), atlasSizes[(int)node.AtlasIndex]);
					ModelRenderer.SetupQuadIndices(indices, indicesOffset, verticesOffset + num * 4, flag);
					indicesOffset += 6;
					bool doubleSided2 = node.DoubleSided;
					if (doubleSided2)
					{
						ModelRenderer.SetupQuadIndices(indices, indicesOffset, verticesOffset + num * 4, !flag);
						indicesOffset += 6;
					}
					num++;
				}
				bool flag10 = !node.TextureLayout[1].Hidden;
				if (flag10)
				{
					ModelRenderer.SetupQuad(nodeIndex, node.AtlasIndex, (byte)node.ShadingMode, gradientId, vertices, verticesOffset + num * 4, 0, 1, 2, 3);
					ModelRenderer.SetupQuadUV(vertices, verticesOffset + num * 4, ref node.TextureLayout[1], ModelRenderer.GetShapeTextureFaceSize(ref node, "back"), atlasSizes[(int)node.AtlasIndex]);
					ModelRenderer.SetupQuadIndices(indices, indicesOffset, verticesOffset + num * 4, flag);
					indicesOffset += 6;
					bool doubleSided3 = node.DoubleSided;
					if (doubleSided3)
					{
						ModelRenderer.SetupQuadIndices(indices, indicesOffset, verticesOffset + num * 4, !flag);
						indicesOffset += 6;
					}
					num++;
				}
				bool flag11 = !node.TextureLayout[2].Hidden;
				if (flag11)
				{
					ModelRenderer.SetupQuad(nodeIndex, node.AtlasIndex, (byte)node.ShadingMode, gradientId, vertices, verticesOffset + num * 4, 1, 4, 7, 2);
					ModelRenderer.SetupQuadUV(vertices, verticesOffset + num * 4, ref node.TextureLayout[2], ModelRenderer.GetShapeTextureFaceSize(ref node, "right"), atlasSizes[(int)node.AtlasIndex]);
					ModelRenderer.SetupQuadIndices(indices, indicesOffset, verticesOffset + num * 4, flag);
					indicesOffset += 6;
					bool doubleSided4 = node.DoubleSided;
					if (doubleSided4)
					{
						ModelRenderer.SetupQuadIndices(indices, indicesOffset, verticesOffset + num * 4, !flag);
						indicesOffset += 6;
					}
					num++;
				}
				bool flag12 = !node.TextureLayout[3].Hidden;
				if (flag12)
				{
					ModelRenderer.SetupQuad(nodeIndex, node.AtlasIndex, (byte)node.ShadingMode, gradientId, vertices, verticesOffset + num * 4, 7, 6, 3, 2);
					ModelRenderer.SetupQuadUV(vertices, verticesOffset + num * 4, ref node.TextureLayout[3], ModelRenderer.GetShapeTextureFaceSize(ref node, "bottom"), atlasSizes[(int)node.AtlasIndex]);
					ModelRenderer.SetupQuadIndices(indices, indicesOffset, verticesOffset + num * 4, flag);
					indicesOffset += 6;
					bool doubleSided5 = node.DoubleSided;
					if (doubleSided5)
					{
						ModelRenderer.SetupQuadIndices(indices, indicesOffset, verticesOffset + num * 4, !flag);
						indicesOffset += 6;
					}
					num++;
				}
				bool flag13 = !node.TextureLayout[4].Hidden;
				if (flag13)
				{
					ModelRenderer.SetupQuad(nodeIndex, node.AtlasIndex, (byte)node.ShadingMode, gradientId, vertices, verticesOffset + num * 4, 5, 0, 3, 6);
					ModelRenderer.SetupQuadUV(vertices, verticesOffset + num * 4, ref node.TextureLayout[4], ModelRenderer.GetShapeTextureFaceSize(ref node, "left"), atlasSizes[(int)node.AtlasIndex]);
					ModelRenderer.SetupQuadIndices(indices, indicesOffset, verticesOffset + num * 4, flag);
					indicesOffset += 6;
					bool doubleSided6 = node.DoubleSided;
					if (doubleSided6)
					{
						ModelRenderer.SetupQuadIndices(indices, indicesOffset, verticesOffset + num * 4, !flag);
						indicesOffset += 6;
					}
					num++;
				}
				bool flag14 = !node.TextureLayout[5].Hidden;
				if (flag14)
				{
					ModelRenderer.SetupQuad(nodeIndex, node.AtlasIndex, (byte)node.ShadingMode, gradientId, vertices, verticesOffset + num * 4, 1, 0, 5, 4);
					ModelRenderer.SetupQuadUV(vertices, verticesOffset + num * 4, ref node.TextureLayout[5], ModelRenderer.GetShapeTextureFaceSize(ref node, "top"), atlasSizes[(int)node.AtlasIndex]);
					ModelRenderer.SetupQuadIndices(indices, indicesOffset, verticesOffset + num * 4, flag);
					indicesOffset += 6;
					bool doubleSided7 = node.DoubleSided;
					if (doubleSided7)
					{
						ModelRenderer.SetupQuadIndices(indices, indicesOffset, verticesOffset + num * 4, !flag);
						indicesOffset += 6;
					}
					num++;
				}
				verticesOffset += num * 4;
			}
		}

		// Token: 0x060056DC RID: 22236 RVA: 0x001A2E44 File Offset: 0x001A1044
		private static void SetupQuad(uint nodeIndex, byte atlasIndex, byte shadingMode, byte gradientId, ModelVertex[] vertices, int offset, int a, int b, int c, int d)
		{
			int num = offset + 1;
			int num2 = offset + 2;
			vertices[offset + 3].NodeIndex = nodeIndex;
			vertices[num2].NodeIndex = nodeIndex;
			vertices[num].NodeIndex = nodeIndex;
			vertices[offset].NodeIndex = nodeIndex;
			uint atlasIndexAndShadingModeAndGradienId = (uint)((int)atlasIndex | (int)shadingMode << 8 | (int)gradientId << 10);
			vertices[offset].AtlasIndexAndShadingModeAndGradienId = (vertices[offset + 1].AtlasIndexAndShadingModeAndGradienId = (vertices[offset + 2].AtlasIndexAndShadingModeAndGradienId = (vertices[offset + 3].AtlasIndexAndShadingModeAndGradienId = atlasIndexAndShadingModeAndGradienId)));
			vertices[offset].Position = ModelRenderer.TempCorners[a];
			vertices[offset + 1].Position = ModelRenderer.TempCorners[b];
			vertices[offset + 2].Position = ModelRenderer.TempCorners[c];
			vertices[offset + 3].Position = ModelRenderer.TempCorners[d];
		}

		// Token: 0x060056DD RID: 22237 RVA: 0x001A2F5C File Offset: 0x001A115C
		private static void SetupQuadUV(ModelVertex[] vertices, int offset, ref BlockyModelFaceTextureLayout faceData, Vector2 size, Point textureSize)
		{
			int x = faceData.Offset.X;
			float x2 = (float)faceData.Offset.X + (float)(faceData.MirrorX ? -1 : 1) * size.X;
			int num = textureSize.Y - faceData.Offset.Y;
			float y = (float)textureSize.Y - ((float)faceData.Offset.Y + (float)(faceData.MirrorY ? -1 : 1) * size.Y);
			vertices[offset].TextureCoordinates = new Vector2(x2, (float)num);
			vertices[offset + 1].TextureCoordinates = new Vector2((float)x, (float)num);
			vertices[offset + 2].TextureCoordinates = new Vector2((float)x, y);
			vertices[offset + 3].TextureCoordinates = new Vector2(x2, y);
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
			for (int i = 0; i < 4; i++)
			{
				Vector2 textureCoordinates = vertices[offset + i].TextureCoordinates;
				float num5 = (float)x + (textureCoordinates.X - (float)x) * (float)num2 - (textureCoordinates.Y - (float)num) * (float)num3;
				float num6 = (float)num + (textureCoordinates.X - (float)x) * (float)num3 + (textureCoordinates.Y - (float)num) * (float)num2;
				vertices[offset + i].TextureCoordinates.X = num5 / (float)textureSize.X;
				vertices[offset + i].TextureCoordinates.Y = 1f - num6 / (float)textureSize.Y;
			}
		}

		// Token: 0x060056DE RID: 22238 RVA: 0x001A312C File Offset: 0x001A132C
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

		// Token: 0x060056DF RID: 22239 RVA: 0x001A31A8 File Offset: 0x001A13A8
		private static Vector2 GetShapeTextureFaceSize(ref BlockyModelNode node, string faceName)
		{
			BlockyModelNode.ShapeType type = node.Type;
			BlockyModelNode.ShapeType shapeType = type;
			if (shapeType != BlockyModelNode.ShapeType.Box)
			{
				if (shapeType == BlockyModelNode.ShapeType.Quad)
				{
					if (faceName == "front")
					{
						return new Vector2(node.Size.X, node.Size.Y);
					}
				}
			}
			else
			{
				if (faceName == "front" || faceName == "back")
				{
					return new Vector2(node.Size.X, node.Size.Y);
				}
				if (faceName == "left" || faceName == "right")
				{
					return new Vector2(node.Size.Z, node.Size.Y);
				}
				if (faceName == "top" || faceName == "bottom")
				{
					return new Vector2(node.Size.X, node.Size.Z);
				}
			}
			throw new Exception("Unreachable");
		}

		// Token: 0x04003458 RID: 13400
		public readonly int IndicesCount;

		// Token: 0x0400345A RID: 13402
		private ModelVertex[] _vertices;

		// Token: 0x0400345B RID: 13403
		private ushort[] _indices;

		// Token: 0x0400345C RID: 13404
		private GLBuffer _verticesBuffer;

		// Token: 0x0400345D RID: 13405
		private GLBuffer _indicesBuffer;

		// Token: 0x0400345E RID: 13406
		private uint _timestamp;

		// Token: 0x0400345F RID: 13407
		private static readonly Vector3[] TempCorners = new Vector3[8];

		// Token: 0x04003460 RID: 13408
		public static readonly Vector3[] BoxCorners = new Vector3[]
		{
			new Vector3(-0.5f, 0.5f, -0.5f),
			new Vector3(0.5f, 0.5f, -0.5f),
			new Vector3(0.5f, -0.5f, -0.5f),
			new Vector3(-0.5f, -0.5f, -0.5f),
			new Vector3(0.5f, 0.5f, 0.5f),
			new Vector3(-0.5f, 0.5f, 0.5f),
			new Vector3(-0.5f, -0.5f, 0.5f),
			new Vector3(0.5f, -0.5f, 0.5f)
		};

		// Token: 0x02000F0E RID: 3854
		public static class BoxCorner
		{
			// Token: 0x040049EA RID: 18922
			public const int BackTopLeft = 0;

			// Token: 0x040049EB RID: 18923
			public const int BackTopRight = 1;

			// Token: 0x040049EC RID: 18924
			public const int BackBottomRight = 2;

			// Token: 0x040049ED RID: 18925
			public const int BackBottomLeft = 3;

			// Token: 0x040049EE RID: 18926
			public const int FrontTopRight = 4;

			// Token: 0x040049EF RID: 18927
			public const int FrontTopLeft = 5;

			// Token: 0x040049F0 RID: 18928
			public const int FrontBottomLeft = 6;

			// Token: 0x040049F1 RID: 18929
			public const int FrontBottomRight = 7;
		}

		// Token: 0x02000F0F RID: 3855
		public static class QuadCorner
		{
			// Token: 0x040049F2 RID: 18930
			public const int TopLeft = 0;

			// Token: 0x040049F3 RID: 18931
			public const int TopRight = 1;

			// Token: 0x040049F4 RID: 18932
			public const int BottomRight = 2;

			// Token: 0x040049F5 RID: 18933
			public const int BottomLeft = 3;
		}
	}
}
