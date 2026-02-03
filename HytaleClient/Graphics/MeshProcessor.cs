using System;
using HytaleClient.Math;

namespace HytaleClient.Graphics
{
	// Token: 0x02000A48 RID: 2632
	internal static class MeshProcessor
	{
		// Token: 0x060053C3 RID: 21443 RVA: 0x0017DBCA File Offset: 0x0017BDCA
		public static void InitializeGL(GLFunctions gl)
		{
			MeshProcessor._gl = gl;
		}

		// Token: 0x060053C4 RID: 21444 RVA: 0x0017DBD2 File Offset: 0x0017BDD2
		public static void ReleaseGL()
		{
			MeshProcessor._gl = null;
		}

		// Token: 0x060053C5 RID: 21445 RVA: 0x0017DBDC File Offset: 0x0017BDDC
		public unsafe static void CreateQuad(ref Mesh result, float size = 1f, int vertPositionAttrib = -1, int vertTexCoordsAttrib = -1, int vertNormalAttrib = -1)
		{
			result.Count = 6;
			float num = size * 0.5f;
			float[] array = new float[]
			{
				0f,
				0f,
				0f,
				1f,
				0f,
				0f,
				0f,
				1f,
				0f,
				0f,
				0f,
				0f,
				0f,
				0f,
				0f,
				1f,
				0f,
				0f,
				0f,
				0f,
				1f,
				0f,
				0f,
				1f,
				0f,
				0f,
				0f,
				1f,
				0f,
				0f,
				0f,
				1f,
				0f,
				0f,
				0f,
				0f,
				1f,
				0f,
				0f,
				1f,
				0f,
				0f,
				0f,
				1f,
				1f,
				0f,
				0f,
				1f
			};
			array[0] = num;
			array[1] = num;
			array[2] = -num;
			array[8] = -num;
			array[9] = num;
			array[10] = -num;
			array[16] = -num;
			array[17] = -num;
			array[18] = -num;
			array[24] = num;
			array[25] = num;
			array[26] = -num;
			array[32] = -num;
			array[33] = -num;
			array[34] = -num;
			array[40] = num;
			array[41] = -num;
			array[42] = -num;
			float[] array2 = array;
			bool flag = result.VertexArray == GLVertexArray.None;
			if (flag)
			{
				result.VertexArray = MeshProcessor._gl.GenVertexArray();
			}
			MeshProcessor._gl.BindVertexArray(result.VertexArray);
			bool flag2 = result.VerticesBuffer == GLBuffer.None;
			if (flag2)
			{
				result.VerticesBuffer = MeshProcessor._gl.GenBuffer();
			}
			MeshProcessor._gl.BindBuffer(GL.ARRAY_BUFFER, result.VerticesBuffer);
			float[] array3;
			float* value;
			if ((array3 = array2) == null || array3.Length == 0)
			{
				value = null;
			}
			else
			{
				value = &array3[0];
			}
			MeshProcessor._gl.BufferData(GL.ARRAY_BUFFER, (IntPtr)(array2.Length * 4), (IntPtr)((void*)value), GL.STATIC_DRAW);
			array3 = null;
			bool flag3 = vertPositionAttrib != -1;
			if (flag3)
			{
				MeshProcessor._gl.EnableVertexAttribArray((uint)vertPositionAttrib);
				MeshProcessor._gl.VertexAttribPointer((uint)vertPositionAttrib, 3, GL.FLOAT, false, 32, IntPtr.Zero);
			}
			bool flag4 = vertTexCoordsAttrib != -1;
			if (flag4)
			{
				MeshProcessor._gl.EnableVertexAttribArray((uint)vertTexCoordsAttrib);
				MeshProcessor._gl.VertexAttribPointer((uint)vertTexCoordsAttrib, 2, GL.FLOAT, false, 32, (IntPtr)12);
			}
			bool flag5 = vertNormalAttrib != -1;
			if (flag5)
			{
				MeshProcessor._gl.EnableVertexAttribArray((uint)vertNormalAttrib);
				MeshProcessor._gl.VertexAttribPointer((uint)vertNormalAttrib, 3, GL.FLOAT, false, 32, (IntPtr)20);
			}
		}

		// Token: 0x060053C6 RID: 21446 RVA: 0x0017DDE0 File Offset: 0x0017BFE0
		public unsafe static void CreateSimpleBox(ref Mesh result, float size = 1f)
		{
			float num = size * 0.5f;
			float[] array = new float[]
			{
				-num,
				-num,
				num,
				num,
				-num,
				num,
				num,
				num,
				num,
				-num,
				num,
				num,
				-num,
				-num,
				-num,
				num,
				-num,
				-num,
				num,
				num,
				-num,
				-num,
				num,
				-num
			};
			ushort[] array2 = new ushort[]
			{
				0,
				1,
				2,
				2,
				3,
				0,
				1,
				5,
				6,
				6,
				2,
				1,
				7,
				6,
				5,
				5,
				4,
				7,
				4,
				0,
				3,
				3,
				7,
				4,
				4,
				5,
				1,
				1,
				0,
				4,
				3,
				2,
				6,
				6,
				7,
				3
			};
			result.Count = (int)((ushort)array2.Length);
			bool flag = result.VertexArray == GLVertexArray.None;
			if (flag)
			{
				result.VertexArray = MeshProcessor._gl.GenVertexArray();
			}
			MeshProcessor._gl.BindVertexArray(result.VertexArray);
			bool flag2 = result.VerticesBuffer == GLBuffer.None;
			if (flag2)
			{
				result.VerticesBuffer = MeshProcessor._gl.GenBuffer();
			}
			MeshProcessor._gl.BindBuffer(GL.ARRAY_BUFFER, result.VerticesBuffer);
			float[] array3;
			float* value;
			if ((array3 = array) == null || array3.Length == 0)
			{
				value = null;
			}
			else
			{
				value = &array3[0];
			}
			MeshProcessor._gl.BufferData(GL.ARRAY_BUFFER, (IntPtr)(array.Length * 4), (IntPtr)((void*)value), GL.STATIC_DRAW);
			array3 = null;
			bool flag3 = result.IndicesBuffer == GLBuffer.None;
			if (flag3)
			{
				result.IndicesBuffer = MeshProcessor._gl.GenBuffer();
			}
			MeshProcessor._gl.BindBuffer(GL.ELEMENT_ARRAY_BUFFER, result.IndicesBuffer);
			ushort[] array4;
			ushort* value2;
			if ((array4 = array2) == null || array4.Length == 0)
			{
				value2 = null;
			}
			else
			{
				value2 = &array4[0];
			}
			MeshProcessor._gl.BufferData(GL.ELEMENT_ARRAY_BUFFER, (IntPtr)(array2.Length * 2), (IntPtr)((void*)value2), GL.STATIC_DRAW);
			array4 = null;
			MeshProcessor._gl.EnableVertexAttribArray(0U);
			MeshProcessor._gl.VertexAttribPointer(0U, 3, GL.FLOAT, false, 12, IntPtr.Zero);
		}

		// Token: 0x060053C7 RID: 21447 RVA: 0x0017E008 File Offset: 0x0017C208
		public unsafe static void CreateBox(ref Mesh result, float size = 1f, int vertPositionAttrib = -1, int vertTexCoordsAttrib = -1, int vertNormalAttrib = -1)
		{
			result.Count = 36;
			float num = size * 0.5f;
			float[] array = new float[]
			{
				-num,
				-num,
				-num,
				num,
				-num,
				-num,
				-num,
				-num,
				num,
				-num,
				-num,
				num,
				num,
				-num,
				-num,
				num,
				-num,
				num,
				num,
				-num,
				-num,
				num,
				num,
				-num,
				num,
				-num,
				num,
				num,
				-num,
				num,
				num,
				num,
				-num,
				num,
				num,
				num,
				num,
				num,
				-num,
				-num,
				num,
				num,
				num,
				num,
				num,
				-num,
				num,
				-num,
				-num,
				num,
				num,
				num,
				num,
				-num,
				-num,
				num,
				num,
				-num,
				num,
				-num,
				-num,
				-num,
				num,
				-num,
				num,
				-num,
				-num,
				-num,
				-num,
				-num,
				-num,
				num,
				-num,
				-num,
				num,
				num,
				-num,
				num,
				-num,
				num,
				num,
				num,
				-num,
				num,
				num,
				num,
				num,
				-num,
				num,
				num,
				-num,
				-num,
				-num,
				-num,
				num,
				-num,
				num,
				-num,
				-num,
				num,
				-num,
				-num,
				-num,
				num,
				-num,
				num,
				num,
				-num
			};
			float[] array2 = new float[]
			{
				0f,
				-1f,
				0f,
				0f,
				-1f,
				0f,
				0f,
				-1f,
				0f,
				0f,
				-1f,
				0f,
				0f,
				-1f,
				0f,
				0f,
				-1f,
				0f,
				1f,
				0f,
				0f,
				1f,
				0f,
				0f,
				1f,
				0f,
				0f,
				1f,
				0f,
				0f,
				1f,
				0f,
				0f,
				1f,
				0f,
				0f,
				0f,
				1f,
				0f,
				0f,
				1f,
				0f,
				0f,
				1f,
				0f,
				0f,
				1f,
				0f,
				0f,
				1f,
				0f,
				0f,
				1f,
				0f,
				-1f,
				0f,
				0f,
				-1f,
				0f,
				0f,
				-1f,
				0f,
				0f,
				-1f,
				0f,
				0f,
				-1f,
				0f,
				0f,
				-1f,
				0f,
				0f,
				0f,
				0f,
				1f,
				0f,
				0f,
				1f,
				0f,
				0f,
				1f,
				0f,
				0f,
				1f,
				0f,
				0f,
				1f,
				0f,
				0f,
				1f,
				0f,
				0f,
				-1f,
				0f,
				0f,
				-1f,
				0f,
				0f,
				-1f,
				0f,
				0f,
				-1f,
				0f,
				0f,
				-1f,
				0f,
				0f,
				-1f
			};
			float[] array3 = new float[]
			{
				0f,
				0f,
				1f,
				0f,
				0f,
				1f,
				0f,
				1f,
				1f,
				0f,
				1f,
				1f,
				0f,
				0f,
				0f,
				1f,
				1f,
				0f,
				1f,
				0f,
				0f,
				1f,
				1f,
				1f,
				0f,
				0f,
				1f,
				1f,
				1f,
				0f,
				0f,
				1f,
				1f,
				1f,
				0f,
				0f,
				1f,
				1f,
				0f,
				1f,
				1f,
				0f,
				0f,
				1f,
				0f,
				0f,
				1f,
				0f,
				0f,
				0f,
				1f,
				0f,
				0f,
				1f,
				1f,
				0f,
				1f,
				1f,
				0f,
				1f,
				0f,
				0f,
				0f,
				1f,
				1f,
				0f,
				1f,
				0f,
				0f,
				1f,
				1f,
				1f
			};
			bool flag = result.VertexArray == GLVertexArray.None;
			if (flag)
			{
				result.VertexArray = MeshProcessor._gl.GenVertexArray();
			}
			MeshProcessor._gl.BindVertexArray(result.VertexArray);
			bool flag2 = result.VerticesBuffer == GLBuffer.None;
			if (flag2)
			{
				result.VerticesBuffer = MeshProcessor._gl.GenBuffer();
			}
			MeshProcessor._gl.BindBuffer(GL.ARRAY_BUFFER, result.VerticesBuffer);
			int num2 = (vertPositionAttrib == -1) ? 0 : (array.Length * 4 * 3);
			int num3 = (vertTexCoordsAttrib == -1) ? 0 : (array3.Length * 4 * 2);
			int num4 = (vertNormalAttrib == -1) ? 0 : (array2.Length * 4 * 3);
			int value = num2 + num3 + num4;
			MeshProcessor._gl.BufferData(GL.ARRAY_BUFFER, (IntPtr)value, IntPtr.Zero, GL.STATIC_DRAW);
			int num5 = 0;
			bool flag3 = num2 != 0;
			if (flag3)
			{
				float[] array4;
				float* value2;
				if ((array4 = array) == null || array4.Length == 0)
				{
					value2 = null;
				}
				else
				{
					value2 = &array4[0];
				}
				MeshProcessor._gl.BufferSubData(GL.ARRAY_BUFFER, (IntPtr)num5, (IntPtr)num2, (IntPtr)((void*)value2));
				array4 = null;
				num5 += num2;
				MeshProcessor._gl.EnableVertexAttribArray((uint)vertPositionAttrib);
				MeshProcessor._gl.VertexAttribPointer((uint)vertPositionAttrib, 3, GL.FLOAT, false, 12, IntPtr.Zero);
			}
			bool flag4 = num3 != 0;
			if (flag4)
			{
				float[] array4;
				float* value3;
				if ((array4 = array3) == null || array4.Length == 0)
				{
					value3 = null;
				}
				else
				{
					value3 = &array4[0];
				}
				MeshProcessor._gl.BufferSubData(GL.ARRAY_BUFFER, (IntPtr)num5, (IntPtr)num3, (IntPtr)((void*)value3));
				array4 = null;
				num5 += num3;
				MeshProcessor._gl.EnableVertexAttribArray((uint)vertTexCoordsAttrib);
				MeshProcessor._gl.VertexAttribPointer((uint)vertTexCoordsAttrib, 2, GL.FLOAT, false, 8, (IntPtr)num2);
			}
			bool flag5 = num4 != 0;
			if (flag5)
			{
				float[] array4;
				float* value4;
				if ((array4 = array2) == null || array4.Length == 0)
				{
					value4 = null;
				}
				else
				{
					value4 = &array4[0];
				}
				MeshProcessor._gl.BufferSubData(GL.ARRAY_BUFFER, (IntPtr)num5, (IntPtr)num4, (IntPtr)((void*)value4));
				array4 = null;
				MeshProcessor._gl.EnableVertexAttribArray((uint)vertNormalAttrib);
				MeshProcessor._gl.VertexAttribPointer((uint)vertNormalAttrib, 3, GL.FLOAT, false, 12, (IntPtr)(num2 + num3));
			}
		}

		// Token: 0x060053C8 RID: 21448 RVA: 0x0017E514 File Offset: 0x0017C714
		public unsafe static void CreateCylinder(ref Mesh result, ushort meridianCount = 8, float radius = 1f, int vertPositionAttrib = -1, int vertTexCoordsAttrib = -1, int vertNormalAttrib = -1)
		{
			ushort num = 2;
			uint num2 = (uint)(num * meridianCount + 2);
			Vector3[] array = new Vector3[num2];
			Vector2[] array2 = new Vector2[num2];
			Vector3[] array3 = new Vector3[num2];
			double num3 = 180.0 / (double)(num + 1) * 3.141592 / 180.0;
			double num4 = 360.0 / (double)(meridianCount - 1) * 3.141592 / 180.0;
			int num5 = 0;
			for (int i = 0; i < (int)num; i++)
			{
				float num6 = radius * (float)(i * 2 - 1);
				for (int j = 0; j < (int)meridianCount; j++)
				{
					array[num5] = new Vector3((float)((double)radius * Math.Cos(num4 * (double)j)), num6, (float)((double)radius * Math.Sin(num4 * (double)j)));
					array2[num5] = new Vector2((float)j / (float)meridianCount, (float)((int)num - i) / (float)num);
					array3[num5] = Vector3.Normalize(array[num5]);
					num5++;
				}
			}
			array[num5] = new Vector3(0f, -radius, 0f);
			array2[num5] = new Vector2(1f, 1f);
			array3[num5] = Vector3.Normalize(array[num5]);
			num5++;
			array[num5] = new Vector3(0f, radius, 0f);
			array2[num5] = new Vector2(0f, 0f);
			array3[num5] = Vector3.Normalize(array[num5]);
			num5++;
			result.Count = (int)(meridianCount * (num - 1) * 2 * 3);
			result.Count += (int)(meridianCount * 2 * 3);
			ushort[] array4 = new ushort[result.Count];
			num5 = 0;
			for (int k = 0; k < (int)(num - 1); k++)
			{
				for (int l = 0; l < (int)meridianCount; l++)
				{
					array4[num5++] = (ushort)((int)meridianCount * k + l);
					array4[num5++] = (ushort)((int)meridianCount * (k + 1) + (l + 1) % (int)meridianCount);
					array4[num5++] = (ushort)((int)meridianCount * k + (l + 1) % (int)meridianCount);
					array4[num5++] = (ushort)((int)meridianCount * (k + 1) + (l + 1) % (int)meridianCount);
					array4[num5++] = (ushort)((int)meridianCount * k + l);
					array4[num5++] = (ushort)((int)meridianCount * (k + 1) + l);
				}
			}
			for (int m = 0; m < (int)meridianCount; m++)
			{
				array4[num5++] = meridianCount * num;
				array4[num5++] = (ushort)m;
				array4[num5++] = (ushort)((m + 1) % (int)meridianCount);
			}
			for (int n = 0; n < (int)meridianCount; n++)
			{
				array4[num5++] = meridianCount * num + 1;
				array4[num5++] = (ushort)((int)(meridianCount * (num - 1)) + (n + 1) % (int)meridianCount);
				array4[num5++] = (ushort)((int)(meridianCount * (num - 1)) + n);
			}
			bool flag = result.VertexArray == GLVertexArray.None;
			if (flag)
			{
				result.VertexArray = MeshProcessor._gl.GenVertexArray();
			}
			MeshProcessor._gl.BindVertexArray(result.VertexArray);
			bool flag2 = result.VerticesBuffer == GLBuffer.None;
			if (flag2)
			{
				result.VerticesBuffer = MeshProcessor._gl.GenBuffer();
			}
			MeshProcessor._gl.BindBuffer(result.VertexArray, GL.ARRAY_BUFFER, result.VerticesBuffer);
			int num7 = (vertPositionAttrib == -1) ? 0 : (array.Length * 4 * 3);
			int num8 = (vertTexCoordsAttrib == -1) ? 0 : (array2.Length * 4 * 2);
			int num9 = (vertNormalAttrib == -1) ? 0 : (array3.Length * 4 * 3);
			int value = num7 + num8 + num9;
			MeshProcessor._gl.BufferData(GL.ARRAY_BUFFER, (IntPtr)value, IntPtr.Zero, GL.STATIC_DRAW);
			int num10 = 0;
			bool flag3 = num7 != 0;
			if (flag3)
			{
				Vector3[] array5;
				Vector3* value2;
				if ((array5 = array) == null || array5.Length == 0)
				{
					value2 = null;
				}
				else
				{
					value2 = &array5[0];
				}
				MeshProcessor._gl.BufferSubData(GL.ARRAY_BUFFER, (IntPtr)num10, (IntPtr)num7, (IntPtr)((void*)value2));
				array5 = null;
				num10 += num7;
				MeshProcessor._gl.EnableVertexAttribArray((uint)vertPositionAttrib);
				MeshProcessor._gl.VertexAttribPointer((uint)vertPositionAttrib, 3, GL.FLOAT, false, 0, IntPtr.Zero);
			}
			bool flag4 = num8 != 0;
			if (flag4)
			{
				Vector2[] array6;
				Vector2* value3;
				if ((array6 = array2) == null || array6.Length == 0)
				{
					value3 = null;
				}
				else
				{
					value3 = &array6[0];
				}
				MeshProcessor._gl.BufferSubData(GL.ARRAY_BUFFER, (IntPtr)num10, (IntPtr)num8, (IntPtr)((void*)value3));
				array6 = null;
				num10 += num8;
				MeshProcessor._gl.EnableVertexAttribArray((uint)vertTexCoordsAttrib);
				MeshProcessor._gl.VertexAttribPointer((uint)vertTexCoordsAttrib, 2, GL.FLOAT, false, 0, (IntPtr)num7);
			}
			bool flag5 = num9 != 0;
			if (flag5)
			{
				Vector3[] array5;
				Vector3* value4;
				if ((array5 = array3) == null || array5.Length == 0)
				{
					value4 = null;
				}
				else
				{
					value4 = &array5[0];
				}
				MeshProcessor._gl.BufferSubData(GL.ARRAY_BUFFER, (IntPtr)num10, (IntPtr)num9, (IntPtr)((void*)value4));
				array5 = null;
				MeshProcessor._gl.EnableVertexAttribArray((uint)vertNormalAttrib);
				MeshProcessor._gl.VertexAttribPointer((uint)vertNormalAttrib, 3, GL.FLOAT, false, 0, (IntPtr)(num7 + num8));
			}
			bool flag6 = result.IndicesBuffer == GLBuffer.None;
			if (flag6)
			{
				result.IndicesBuffer = MeshProcessor._gl.GenBuffer();
			}
			MeshProcessor._gl.BindBuffer(result.VertexArray, GL.ELEMENT_ARRAY_BUFFER, result.IndicesBuffer);
			ushort[] array7;
			ushort* value5;
			if ((array7 = array4) == null || array7.Length == 0)
			{
				value5 = null;
			}
			else
			{
				value5 = &array7[0];
			}
			MeshProcessor._gl.BufferData(GL.ELEMENT_ARRAY_BUFFER, (IntPtr)(array4.Length * 2), (IntPtr)((void*)value5), GL.STATIC_DRAW);
			array7 = null;
		}

		// Token: 0x060053C9 RID: 21449 RVA: 0x0017EB78 File Offset: 0x0017CD78
		public unsafe static void CreateCone(ref Mesh result, ushort meridianCount = 8, float radius = 1f, int vertPositionAttrib = -1, int vertTexCoordsAttrib = -1, int vertNormalAttrib = -1)
		{
			uint num = (uint)(meridianCount + 2);
			Vector3[] array = new Vector3[num];
			Vector2[] array2 = new Vector2[num];
			Vector3[] array3 = new Vector3[num];
			double num2 = 360.0 / (double)(meridianCount - 1) * 3.141592 / 180.0;
			int num3 = 0;
			float num4 = -radius;
			for (int i = 0; i < (int)meridianCount; i++)
			{
				array[num3] = new Vector3((float)((double)radius * Math.Cos(num2 * (double)i)), num4, (float)((double)radius * Math.Sin(num2 * (double)i)));
				array2[num3] = new Vector2((float)i / (float)meridianCount, 1f);
				array3[num3] = Vector3.Normalize(array[num3]);
				num3++;
			}
			array[num3] = new Vector3(0f, -radius, 0f);
			array2[num3] = new Vector2(1f, 1f);
			array3[num3] = Vector3.Normalize(array[num3]);
			num3++;
			array[num3] = new Vector3(0f, radius, 0f);
			array2[num3] = new Vector2(0f, 0f);
			array3[num3] = Vector3.Normalize(array[num3]);
			num3++;
			result.Count = 0;
			result.Count += (int)(meridianCount * 2 * 3);
			ushort[] array4 = new ushort[result.Count];
			num3 = 0;
			for (int j = 0; j < (int)meridianCount; j++)
			{
				array4[num3++] = meridianCount + 1;
				array4[num3++] = (ushort)j;
				array4[num3++] = (ushort)((j + 1) % (int)meridianCount);
			}
			for (int k = 0; k < (int)meridianCount; k++)
			{
				array4[num3++] = meridianCount;
				array4[num3++] = (ushort)k;
				array4[num3++] = (ushort)((k + 1) % (int)meridianCount);
			}
			bool flag = result.VertexArray == GLVertexArray.None;
			if (flag)
			{
				result.VertexArray = MeshProcessor._gl.GenVertexArray();
			}
			MeshProcessor._gl.BindVertexArray(result.VertexArray);
			bool flag2 = result.VerticesBuffer == GLBuffer.None;
			if (flag2)
			{
				result.VerticesBuffer = MeshProcessor._gl.GenBuffer();
			}
			MeshProcessor._gl.BindBuffer(result.VertexArray, GL.ARRAY_BUFFER, result.VerticesBuffer);
			int num5 = (vertPositionAttrib == -1) ? 0 : (array.Length * 4 * 3);
			int num6 = (vertTexCoordsAttrib == -1) ? 0 : (array2.Length * 4 * 2);
			int num7 = (vertNormalAttrib == -1) ? 0 : (array3.Length * 4 * 3);
			int value = num5 + num6 + num7;
			MeshProcessor._gl.BufferData(GL.ARRAY_BUFFER, (IntPtr)value, IntPtr.Zero, GL.STATIC_DRAW);
			int num8 = 0;
			bool flag3 = num5 != 0;
			if (flag3)
			{
				Vector3[] array5;
				Vector3* value2;
				if ((array5 = array) == null || array5.Length == 0)
				{
					value2 = null;
				}
				else
				{
					value2 = &array5[0];
				}
				MeshProcessor._gl.BufferSubData(GL.ARRAY_BUFFER, (IntPtr)num8, (IntPtr)num5, (IntPtr)((void*)value2));
				array5 = null;
				num8 += num5;
				MeshProcessor._gl.EnableVertexAttribArray((uint)vertPositionAttrib);
				MeshProcessor._gl.VertexAttribPointer((uint)vertPositionAttrib, 3, GL.FLOAT, false, 0, IntPtr.Zero);
			}
			bool flag4 = num6 != 0;
			if (flag4)
			{
				Vector2[] array6;
				Vector2* value3;
				if ((array6 = array2) == null || array6.Length == 0)
				{
					value3 = null;
				}
				else
				{
					value3 = &array6[0];
				}
				MeshProcessor._gl.BufferSubData(GL.ARRAY_BUFFER, (IntPtr)num8, (IntPtr)num6, (IntPtr)((void*)value3));
				array6 = null;
				num8 += num6;
				MeshProcessor._gl.EnableVertexAttribArray((uint)vertTexCoordsAttrib);
				MeshProcessor._gl.VertexAttribPointer((uint)vertTexCoordsAttrib, 2, GL.FLOAT, false, 0, (IntPtr)num5);
			}
			bool flag5 = num7 != 0;
			if (flag5)
			{
				Vector3[] array5;
				Vector3* value4;
				if ((array5 = array3) == null || array5.Length == 0)
				{
					value4 = null;
				}
				else
				{
					value4 = &array5[0];
				}
				MeshProcessor._gl.BufferSubData(GL.ARRAY_BUFFER, (IntPtr)num8, (IntPtr)num7, (IntPtr)((void*)value4));
				array5 = null;
				MeshProcessor._gl.EnableVertexAttribArray((uint)vertNormalAttrib);
				MeshProcessor._gl.VertexAttribPointer((uint)vertNormalAttrib, 3, GL.FLOAT, false, 0, (IntPtr)(num5 + num6));
			}
			bool flag6 = result.IndicesBuffer == GLBuffer.None;
			if (flag6)
			{
				result.IndicesBuffer = MeshProcessor._gl.GenBuffer();
			}
			MeshProcessor._gl.BindBuffer(result.VertexArray, GL.ELEMENT_ARRAY_BUFFER, result.IndicesBuffer);
			ushort[] array7;
			ushort* value5;
			if ((array7 = array4) == null || array7.Length == 0)
			{
				value5 = null;
			}
			else
			{
				value5 = &array7[0];
			}
			MeshProcessor._gl.BufferData(GL.ELEMENT_ARRAY_BUFFER, (IntPtr)(array4.Length * 2), (IntPtr)((void*)value5), GL.STATIC_DRAW);
			array7 = null;
		}

		// Token: 0x060053CA RID: 21450 RVA: 0x0017F0B8 File Offset: 0x0017D2B8
		public unsafe static void CreateSphere(ref Mesh result, ushort parallelCount = 5, ushort meridianCount = 8, float radius = 1f, int vertPositionAttrib = -1, int vertTexCoordsAttrib = -1, int vertNormalAttrib = -1)
		{
			uint num = (uint)(parallelCount * meridianCount + 2);
			Vector3[] array = new Vector3[num];
			Vector2[] array2 = new Vector2[num];
			Vector3[] array3 = new Vector3[num];
			double num2 = 180.0 / (double)(parallelCount + 1) * 3.141592 / 180.0;
			double num3 = 360.0 / (double)(meridianCount - 1) * 3.141592 / 180.0;
			int num4 = 0;
			for (int i = 0; i < (int)parallelCount; i++)
			{
				double num5 = -1.570796 + num2 * (double)(i + 1);
				double num6 = (double)radius * Math.Sin(num5);
				double num7 = (double)radius * Math.Cos(num5);
				for (int j = 0; j < (int)meridianCount; j++)
				{
					array[num4] = new Vector3((float)(num7 * Math.Cos(num3 * (double)j)), (float)num6, (float)(num7 * Math.Sin(num3 * (double)j)));
					array2[num4] = new Vector2((float)j / (float)meridianCount, (float)((int)parallelCount - i) / (float)parallelCount);
					array3[num4] = Vector3.Normalize(array[num4]);
					num4++;
				}
			}
			array[num4] = new Vector3(0f, -radius, 0f);
			array2[num4] = new Vector2(0.5f, 1f);
			array3[num4] = Vector3.Normalize(array[num4]);
			num4++;
			array[num4] = new Vector3(0f, radius, 0f);
			array2[num4] = new Vector2(0.5f, 0f);
			array3[num4] = Vector3.Normalize(array[num4]);
			num4++;
			result.Count = (int)(meridianCount * (parallelCount - 1) * 2 * 3);
			result.Count += (int)(meridianCount * 2 * 3);
			ushort[] array4 = new ushort[result.Count];
			num4 = 0;
			for (int k = 0; k < (int)(parallelCount - 1); k++)
			{
				for (int l = 0; l < (int)meridianCount; l++)
				{
					array4[num4++] = (ushort)((int)meridianCount * k + l);
					array4[num4++] = (ushort)((int)meridianCount * (k + 1) + (l + 1) % (int)meridianCount);
					array4[num4++] = (ushort)((int)meridianCount * k + (l + 1) % (int)meridianCount);
					array4[num4++] = (ushort)((int)meridianCount * (k + 1) + (l + 1) % (int)meridianCount);
					array4[num4++] = (ushort)((int)meridianCount * k + l);
					array4[num4++] = (ushort)((int)meridianCount * (k + 1) + l);
				}
			}
			for (int m = 0; m < (int)meridianCount; m++)
			{
				array4[num4++] = meridianCount * parallelCount;
				array4[num4++] = (ushort)m;
				array4[num4++] = (ushort)((m + 1) % (int)meridianCount);
			}
			for (int n = 0; n < (int)meridianCount; n++)
			{
				array4[num4++] = meridianCount * parallelCount + 1;
				array4[num4++] = (ushort)((int)(meridianCount * (parallelCount - 1)) + (n + 1) % (int)meridianCount);
				array4[num4++] = (ushort)((int)(meridianCount * (parallelCount - 1)) + n);
			}
			bool flag = result.VertexArray == GLVertexArray.None;
			if (flag)
			{
				result.VertexArray = MeshProcessor._gl.GenVertexArray();
			}
			MeshProcessor._gl.BindVertexArray(result.VertexArray);
			bool flag2 = result.VerticesBuffer == GLBuffer.None;
			if (flag2)
			{
				result.VerticesBuffer = MeshProcessor._gl.GenBuffer();
			}
			MeshProcessor._gl.BindBuffer(result.VertexArray, GL.ARRAY_BUFFER, result.VerticesBuffer);
			int num8 = (vertPositionAttrib == -1) ? 0 : (array.Length * 4 * 3);
			int num9 = (vertTexCoordsAttrib == -1) ? 0 : (array2.Length * 4 * 2);
			int num10 = (vertNormalAttrib == -1) ? 0 : (array3.Length * 4 * 3);
			int value = num8 + num9 + num10;
			MeshProcessor._gl.BufferData(GL.ARRAY_BUFFER, (IntPtr)value, IntPtr.Zero, GL.STATIC_DRAW);
			int num11 = 0;
			bool flag3 = num8 != 0;
			if (flag3)
			{
				Vector3[] array5;
				Vector3* value2;
				if ((array5 = array) == null || array5.Length == 0)
				{
					value2 = null;
				}
				else
				{
					value2 = &array5[0];
				}
				MeshProcessor._gl.BufferSubData(GL.ARRAY_BUFFER, (IntPtr)num11, (IntPtr)num8, (IntPtr)((void*)value2));
				array5 = null;
				num11 += num8;
				MeshProcessor._gl.EnableVertexAttribArray((uint)vertPositionAttrib);
				MeshProcessor._gl.VertexAttribPointer((uint)vertPositionAttrib, 3, GL.FLOAT, false, 0, IntPtr.Zero);
			}
			bool flag4 = num9 != 0;
			if (flag4)
			{
				Vector2[] array6;
				Vector2* value3;
				if ((array6 = array2) == null || array6.Length == 0)
				{
					value3 = null;
				}
				else
				{
					value3 = &array6[0];
				}
				MeshProcessor._gl.BufferSubData(GL.ARRAY_BUFFER, (IntPtr)num11, (IntPtr)num9, (IntPtr)((void*)value3));
				array6 = null;
				num11 += num9;
				MeshProcessor._gl.EnableVertexAttribArray((uint)vertTexCoordsAttrib);
				MeshProcessor._gl.VertexAttribPointer((uint)vertTexCoordsAttrib, 2, GL.FLOAT, false, 0, (IntPtr)num8);
			}
			bool flag5 = num10 != 0;
			if (flag5)
			{
				Vector3[] array5;
				Vector3* value4;
				if ((array5 = array3) == null || array5.Length == 0)
				{
					value4 = null;
				}
				else
				{
					value4 = &array5[0];
				}
				MeshProcessor._gl.BufferSubData(GL.ARRAY_BUFFER, (IntPtr)num11, (IntPtr)num10, (IntPtr)((void*)value4));
				array5 = null;
				MeshProcessor._gl.EnableVertexAttribArray((uint)vertNormalAttrib);
				MeshProcessor._gl.VertexAttribPointer((uint)vertNormalAttrib, 3, GL.FLOAT, false, 0, (IntPtr)(num8 + num9));
			}
			bool flag6 = result.IndicesBuffer == GLBuffer.None;
			if (flag6)
			{
				result.IndicesBuffer = MeshProcessor._gl.GenBuffer();
			}
			MeshProcessor._gl.BindBuffer(result.VertexArray, GL.ELEMENT_ARRAY_BUFFER, result.IndicesBuffer);
			ushort[] array7;
			ushort* value5;
			if ((array7 = array4) == null || array7.Length == 0)
			{
				value5 = null;
			}
			else
			{
				value5 = &array7[0];
			}
			MeshProcessor._gl.BufferData(GL.ELEMENT_ARRAY_BUFFER, (IntPtr)(array4.Length * 2), (IntPtr)((void*)value5), GL.STATIC_DRAW);
			array7 = null;
		}

		// Token: 0x060053CB RID: 21451 RVA: 0x0017F73C File Offset: 0x0017D93C
		public unsafe static void CreateFrustum(ref Mesh result, ref BoundingFrustum frustum)
		{
			MeshProcessor.CreateSimpleBox(ref result, 2f);
			MeshProcessor._gl.BindBuffer(GL.ARRAY_BUFFER, result.VerticesBuffer);
			Vector3[] corners = frustum.GetCorners();
			Vector3[] array;
			Vector3* value;
			if ((array = corners) == null || array.Length == 0)
			{
				value = null;
			}
			else
			{
				value = &array[0];
			}
			MeshProcessor._gl.BufferData(GL.ARRAY_BUFFER, (IntPtr)(corners.Length * 3 * 4), (IntPtr)((void*)value), GL.STATIC_DRAW);
			array = null;
		}

		// Token: 0x04002EB4 RID: 11956
		private static GLFunctions _gl;
	}
}
