using System;
using System.Runtime.CompilerServices;
using HytaleClient.Core;
using HytaleClient.Graphics.Map;
using HytaleClient.Math;

namespace HytaleClient.Graphics.Gizmos
{
	// Token: 0x02000A96 RID: 2710
	internal class BlockShapeRenderer : Disposable
	{
		// Token: 0x0600554A RID: 21834 RVA: 0x0018F364 File Offset: 0x0018D564
		public BlockShapeRenderer(GraphicsDevice graphics, int vertPositionAttrib = -1, int vertTexCoordsAttrib = -1)
		{
			this._graphics = graphics;
			GLFunctions gl = graphics.GL;
			this._vertexArray = gl.GenVertexArray();
			gl.BindVertexArray(this._vertexArray);
			this._verticesBuffer = gl.GenBuffer();
			gl.BindBuffer(this._vertexArray, GL.ARRAY_BUFFER, this._verticesBuffer);
			this._indicesBuffer = gl.GenBuffer();
			gl.BindBuffer(this._vertexArray, GL.ELEMENT_ARRAY_BUFFER, this._indicesBuffer);
			bool flag = vertPositionAttrib != -1;
			if (flag)
			{
				gl.EnableVertexAttribArray((uint)vertPositionAttrib);
				gl.VertexAttribPointer((uint)vertPositionAttrib, 3, GL.FLOAT, false, 32, IntPtr.Zero);
			}
			bool flag2 = vertTexCoordsAttrib != -1;
			if (flag2)
			{
				gl.EnableVertexAttribArray((uint)vertTexCoordsAttrib);
				gl.VertexAttribPointer((uint)vertTexCoordsAttrib, 2, GL.FLOAT, false, 32, (IntPtr)12);
			}
		}

		// Token: 0x0600554B RID: 21835 RVA: 0x0018F44C File Offset: 0x0018D64C
		protected override void DoDispose()
		{
			this._graphics.GL.DeleteBuffer(this._verticesBuffer);
			this._graphics.GL.DeleteBuffer(this._indicesBuffer);
			this._graphics.GL.DeleteVertexArray(this._vertexArray);
		}

		// Token: 0x0600554C RID: 21836 RVA: 0x0018F4A0 File Offset: 0x0018D6A0
		public unsafe void UpdateModelData(bool[,,] blockData, int xOffset, int yOffset, int zOffset)
		{
			int length = blockData.GetLength(0);
			int length2 = blockData.GetLength(1);
			int length3 = blockData.GetLength(2);
			int num = length * length3;
			int num2 = 0;
			int num3 = 0;
			int num4 = length * length2 * length3 * 6;
			ushort[] array = new ushort[num4];
			int num5 = 0;
			bool[] array2 = new bool[3];
			short num6 = 0;
			while ((int)num6 < length)
			{
				short num7 = 0;
				while ((int)num7 < length2)
				{
					short num8 = 0;
					while ((int)num8 < length3)
					{
						bool flag = !blockData[(int)num6, (int)num7, (int)num8];
						if (!flag)
						{
							for (int i = 0; i < 6; i++)
							{
								Vector3 normal = ChunkGeometryBuilder.AdjacentBlockOffsetsBySide[i].Normal;
								int num9 = (int)num6 + (int)normal.X;
								int num10 = (int)num7 + (int)normal.Y;
								int num11 = (int)num8 + (int)normal.Z;
								bool flag2 = num9 >= 0 && num9 < length && num10 >= 0 && num10 < length2 && num11 >= 0 && num11 < length3 && blockData[num9, num10, num11];
								if (!flag2)
								{
									num2 += 6;
								}
							}
							for (int j = 0; j < BlockShapeRenderer.Edges.GetLength(0); j++)
							{
								for (int k = 0; k < 3; k++)
								{
									int num12 = k * 3;
									int num9 = (int)(num6 + (short)BlockShapeRenderer.Edges[j, num12]);
									int num10 = (int)(num7 + (short)BlockShapeRenderer.Edges[j, num12 + 1]);
									int num11 = (int)(num8 + (short)BlockShapeRenderer.Edges[j, num12 + 2]);
									array2[k] = (num9 >= 0 && num9 < length && num10 >= 0 && num10 < length2 && num11 >= 0 && num11 < length3 && blockData[num9, num10, num11]);
								}
								bool flag3 = (array2[0] && array2[1] && array2[2]) || (!array2[0] && !array2[1] && array2[2]) || (!array2[0] && array2[1] && !array2[2]) || ((array2[0] || array2[1] || array2[2]) && ((num6 > 0 && (j == 2 || j == 6 || j == 8 || j == 10)) || (num7 > 0 && (j == 0 || j == 4 || j == 10 || j == 11)) || (num8 > 0 && (j == 0 || j == 1 || j == 2 || j == 3))));
								if (!flag3)
								{
									num3 += 2;
									bool flag4 = num5 + 6 >= array.Length;
									if (flag4)
									{
										Array.Resize<ushort>(ref array, array.Length * 2);
									}
									array[num5++] = (ushort)(num6 + (short)BlockShapeRenderer.EdgeLines[j, 0]);
									array[num5++] = (ushort)(num7 + (short)BlockShapeRenderer.EdgeLines[j, 1]);
									array[num5++] = (ushort)(num8 + (short)BlockShapeRenderer.EdgeLines[j, 2]);
									array[num5++] = (ushort)(num6 + (short)BlockShapeRenderer.EdgeLines[j, 3]);
									array[num5++] = (ushort)(num7 + (short)BlockShapeRenderer.EdgeLines[j, 4]);
									array[num5++] = (ushort)(num8 + (short)BlockShapeRenderer.EdgeLines[j, 5]);
								}
							}
						}
						num8 += 1;
					}
					num7 += 1;
				}
				num6 += 1;
			}
			this._indices = new ushort[num2 + num3];
			this._quadIndicesTotal = num2;
			this._lineIndicesTotal = num3;
			BlockShapeRenderer.<>c__DisplayClass13_0 CS$<>8__locals1;
			CS$<>8__locals1.vertArray = new ushort[Math.Max(num2 / 4, 32)];
			CS$<>8__locals1.vertArrayOffset = 0;
			int num13 = 0;
			CS$<>8__locals1.vertCount = 1;
			CS$<>8__locals1.vertLookup = new ushort[length + 1, length2 + 1, length3 + 1];
			ushort num14 = 0;
			while ((int)num14 < length)
			{
				ushort num15 = 0;
				while ((int)num15 < length2)
				{
					ushort num16 = 0;
					while ((int)num16 < length3)
					{
						bool flag5 = !blockData[(int)num14, (int)num15, (int)num16];
						if (!flag5)
						{
							for (int l = 0; l < 6; l++)
							{
								Vector3 normal2 = ChunkGeometryBuilder.AdjacentBlockOffsetsBySide[l].Normal;
								int num17 = (int)num14 + (int)normal2.X;
								int num18 = (int)num15 + (int)normal2.Y;
								int num19 = (int)num16 + (int)normal2.Z;
								bool flag6 = num17 >= 0 && num17 < length && num18 >= 0 && num18 < length2 && num19 >= 0 && num19 < length3 && blockData[num17, num18, num19];
								if (!flag6)
								{
									ushort[] array3 = new ushort[4];
									for (int m = 0; m < 4; m++)
									{
										array3[m] = BlockShapeRenderer.<UpdateModelData>g__addVert|13_0((ushort)(ChunkGeometryBuilder.CornersPerSide[l][m].X + (float)num14), (ushort)(ChunkGeometryBuilder.CornersPerSide[l][m].Y + (float)num15), (ushort)(ChunkGeometryBuilder.CornersPerSide[l][m].Z + (float)num16), ref CS$<>8__locals1);
									}
									this._indices[num13++] = array3[0];
									this._indices[num13++] = array3[1];
									this._indices[num13++] = array3[2];
									this._indices[num13++] = array3[0];
									this._indices[num13++] = array3[2];
									this._indices[num13++] = array3[3];
								}
							}
						}
						num16 += 1;
					}
					num15 += 1;
				}
				num14 += 1;
			}
			for (int n = 0; n < num5; n += 6)
			{
				this._indices[num13++] = BlockShapeRenderer.<UpdateModelData>g__addVert|13_0(array[n], array[n + 1], array[n + 2], ref CS$<>8__locals1);
				this._indices[num13++] = BlockShapeRenderer.<UpdateModelData>g__addVert|13_0(array[n + 3], array[n + 4], array[n + 5], ref CS$<>8__locals1);
			}
			ushort vertCount = CS$<>8__locals1.vertCount;
			CS$<>8__locals1.vertCount = vertCount - 1;
			bool flag7 = CS$<>8__locals1.vertArray.Length == 0;
			if (!flag7)
			{
				this._vertices = new float[(int)(CS$<>8__locals1.vertCount * 8)];
				for (int num20 = 0; num20 < (int)CS$<>8__locals1.vertCount; num20++)
				{
					int num21 = num20 * 3;
					int num22 = num20 * 8;
					this._vertices[num22] = (float)((int)CS$<>8__locals1.vertArray[num21] + xOffset);
					this._vertices[num22 + 1] = (float)((int)CS$<>8__locals1.vertArray[num21 + 1] + yOffset);
					this._vertices[num22 + 2] = (float)((int)CS$<>8__locals1.vertArray[num21 + 2] + zOffset);
				}
				GLFunctions gl = this._graphics.GL;
				gl.BindVertexArray(this._vertexArray);
				gl.BindBuffer(this._vertexArray, GL.ARRAY_BUFFER, this._verticesBuffer);
				float[] array4;
				float* value;
				if ((array4 = this._vertices) == null || array4.Length == 0)
				{
					value = null;
				}
				else
				{
					value = &array4[0];
				}
				gl.BufferData(GL.ARRAY_BUFFER, (IntPtr)(this._vertices.Length * 4), (IntPtr)((void*)value), GL.STATIC_DRAW);
				array4 = null;
				gl.BindBuffer(this._vertexArray, GL.ELEMENT_ARRAY_BUFFER, this._indicesBuffer);
				ushort[] array5;
				ushort* value2;
				if ((array5 = this._indices) == null || array5.Length == 0)
				{
					value2 = null;
				}
				else
				{
					value2 = &array5[0];
				}
				gl.BufferData(GL.ELEMENT_ARRAY_BUFFER, (IntPtr)(this._indices.Length * 2), (IntPtr)((void*)value2), GL.STATIC_DRAW);
				array5 = null;
			}
		}

		// Token: 0x0600554D RID: 21837 RVA: 0x0018FC7C File Offset: 0x0018DE7C
		public void DrawBlockShapeOutline()
		{
			GLFunctions gl = this._graphics.GL;
			gl.BindVertexArray(this._vertexArray);
			gl.DrawElements(GL.ONE, this._lineIndicesTotal, GL.UNSIGNED_SHORT, (IntPtr)(this._quadIndicesTotal * 2));
		}

		// Token: 0x0600554E RID: 21838 RVA: 0x0018FCC4 File Offset: 0x0018DEC4
		public void DrawBlockShape()
		{
			GLFunctions gl = this._graphics.GL;
			gl.BindVertexArray(this._vertexArray);
			gl.DrawElements(GL.TRIANGLES, this._quadIndicesTotal, GL.UNSIGNED_SHORT, (IntPtr)0);
		}

		// Token: 0x06005550 RID: 21840 RVA: 0x0018FD38 File Offset: 0x0018DF38
		[CompilerGenerated]
		internal static ushort <UpdateModelData>g__addVert|13_0(ushort x, ushort y, ushort z, ref BlockShapeRenderer.<>c__DisplayClass13_0 A_3)
		{
			ushort num = A_3.vertLookup[(int)x, (int)y, (int)z];
			bool flag = num > 0;
			ushort result;
			if (flag)
			{
				result = num - 1;
			}
			else
			{
				bool flag2 = A_3.vertArrayOffset + 3 >= A_3.vertArray.Length;
				if (flag2)
				{
					Array.Resize<ushort>(ref A_3.vertArray, (int)((double)A_3.vertArray.Length * 1.2));
				}
				ushort[,,] vertLookup = A_3.vertLookup;
				ushort vertCount = A_3.vertCount;
				A_3.vertCount = vertCount + 1;
				vertLookup[(int)x, (int)y, (int)z] = vertCount;
				ushort[] vertArray = A_3.vertArray;
				int vertArrayOffset = A_3.vertArrayOffset;
				A_3.vertArrayOffset = vertArrayOffset + 1;
				vertArray[vertArrayOffset] = x;
				ushort[] vertArray2 = A_3.vertArray;
				vertArrayOffset = A_3.vertArrayOffset;
				A_3.vertArrayOffset = vertArrayOffset + 1;
				vertArray2[vertArrayOffset] = y;
				ushort[] vertArray3 = A_3.vertArray;
				vertArrayOffset = A_3.vertArrayOffset;
				A_3.vertArrayOffset = vertArrayOffset + 1;
				vertArray3[vertArrayOffset] = z;
				result = A_3.vertCount - 2;
			}
			return result;
		}

		// Token: 0x04003226 RID: 12838
		private readonly GraphicsDevice _graphics;

		// Token: 0x04003227 RID: 12839
		private GLVertexArray _vertexArray;

		// Token: 0x04003228 RID: 12840
		private GLBuffer _verticesBuffer;

		// Token: 0x04003229 RID: 12841
		private GLBuffer _indicesBuffer;

		// Token: 0x0400322A RID: 12842
		private Matrix _matrix;

		// Token: 0x0400322B RID: 12843
		private int _quadIndicesTotal;

		// Token: 0x0400322C RID: 12844
		private int _lineIndicesTotal;

		// Token: 0x0400322D RID: 12845
		private float[] _vertices;

		// Token: 0x0400322E RID: 12846
		private ushort[] _indices;

		// Token: 0x0400322F RID: 12847
		private static readonly sbyte[,] Edges = new sbyte[,]
		{
			{
				0,
				-1,
				-1,
				0,
				-1,
				0,
				0,
				0,
				-1
			},
			{
				0,
				1,
				-1,
				0,
				1,
				0,
				0,
				0,
				-1
			},
			{
				-1,
				0,
				-1,
				-1,
				0,
				0,
				0,
				0,
				-1
			},
			{
				1,
				0,
				-1,
				1,
				0,
				0,
				0,
				0,
				-1
			},
			{
				0,
				-1,
				1,
				0,
				-1,
				0,
				0,
				0,
				1
			},
			{
				0,
				1,
				1,
				0,
				1,
				0,
				0,
				0,
				1
			},
			{
				-1,
				0,
				1,
				-1,
				0,
				0,
				0,
				0,
				1
			},
			{
				1,
				0,
				1,
				1,
				0,
				0,
				0,
				0,
				1
			},
			{
				-1,
				1,
				0,
				-1,
				0,
				0,
				0,
				1,
				0
			},
			{
				1,
				1,
				0,
				1,
				0,
				0,
				0,
				1,
				0
			},
			{
				-1,
				-1,
				0,
				-1,
				0,
				0,
				0,
				-1,
				0
			},
			{
				1,
				-1,
				0,
				1,
				0,
				0,
				0,
				-1,
				0
			}
		};

		// Token: 0x04003230 RID: 12848
		private static readonly byte[,] EdgeLines = new byte[,]
		{
			{
				0,
				0,
				0,
				1,
				0,
				0
			},
			{
				0,
				1,
				0,
				1,
				1,
				0
			},
			{
				0,
				0,
				0,
				0,
				1,
				0
			},
			{
				1,
				0,
				0,
				1,
				1,
				0
			},
			{
				0,
				0,
				1,
				1,
				0,
				1
			},
			{
				0,
				1,
				1,
				1,
				1,
				1
			},
			{
				0,
				0,
				1,
				0,
				1,
				1
			},
			{
				1,
				0,
				1,
				1,
				1,
				1
			},
			{
				0,
				1,
				0,
				0,
				1,
				1
			},
			{
				1,
				1,
				0,
				1,
				1,
				1
			},
			{
				0,
				0,
				0,
				0,
				0,
				1
			},
			{
				1,
				0,
				0,
				1,
				0,
				1
			}
		};
	}
}
