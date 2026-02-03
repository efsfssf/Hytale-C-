using System;
using HytaleClient.Math;

namespace HytaleClient.Graphics.Gizmos.Models
{
	// Token: 0x02000AA6 RID: 2726
	public class PrimitiveModelData
	{
		// Token: 0x060055BF RID: 21951 RVA: 0x00197F2F File Offset: 0x0019612F
		public PrimitiveModelData(float[] vertices, ushort[] indices)
		{
			this.Vertices = vertices;
			this.Indices = indices;
		}

		// Token: 0x060055C0 RID: 21952 RVA: 0x00197F48 File Offset: 0x00196148
		public static ushort[] MakeRadialIndices(int segments, int rings)
		{
			int num = 0;
			ushort[] array = new ushort[segments * rings * 6];
			for (int i = 0; i < rings - 1; i++)
			{
				for (int j = 0; j < segments - 1; j++)
				{
					array[num++] = (ushort)(i * segments + j);
					array[num++] = (ushort)((i + 1) * segments + j);
					array[num++] = (ushort)((i + 1) * segments + j + 1);
					array[num++] = (ushort)((i + 1) * segments + j + 1);
					array[num++] = (ushort)(i * segments + j + 1);
					array[num++] = (ushort)(i * segments + j);
				}
			}
			return array;
		}

		// Token: 0x060055C1 RID: 21953 RVA: 0x00197FF4 File Offset: 0x001961F4
		public void OffsetVertices(Vector3 offset)
		{
			for (int i = 0; i < this.Vertices.Length - 1; i += 8)
			{
				this.Vertices[i] += offset.X;
				this.Vertices[i + 1] += offset.Y;
				this.Vertices[i + 2] += offset.Z;
			}
		}

		// Token: 0x060055C2 RID: 21954 RVA: 0x00198064 File Offset: 0x00196264
		public static PrimitiveModelData CombineData(PrimitiveModelData model1, PrimitiveModelData model2)
		{
			float[] vertices = model1.Vertices;
			float[] vertices2 = model2.Vertices;
			float[] array = new float[vertices.Length + vertices2.Length];
			Array.Copy(vertices, array, vertices.Length);
			Array.Copy(vertices2, 0, array, vertices.Length, vertices2.Length);
			ushort[] indices = model1.Indices;
			ushort[] indices2 = model2.Indices;
			ushort[] array2 = new ushort[indices.Length + indices2.Length];
			Array.Copy(indices, array2, indices.Length);
			Array.Copy(indices2, 0, array2, indices.Length, indices2.Length);
			ushort num = 0;
			for (int i = 0; i < indices.Length; i++)
			{
				bool flag = num == 0 || array2[i] > num;
				if (flag)
				{
					num = array2[i];
				}
			}
			num += 1;
			for (int j = indices.Length; j < indices.Length + indices2.Length; j++)
			{
				ushort[] array3 = array2;
				int num2 = j;
				array3[num2] += num;
			}
			return new PrimitiveModelData(array, array2);
		}

		// Token: 0x040032C2 RID: 12994
		public const int VertexSize = 8;

		// Token: 0x040032C3 RID: 12995
		public readonly float[] Vertices;

		// Token: 0x040032C4 RID: 12996
		public readonly ushort[] Indices;
	}
}
