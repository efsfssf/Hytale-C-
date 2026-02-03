using System;

namespace HytaleClient.Graphics.Gizmos.Models
{
	// Token: 0x02000AA4 RID: 2724
	public class CubeModel
	{
		// Token: 0x060055B7 RID: 21943 RVA: 0x00197A88 File Offset: 0x00195C88
		public static PrimitiveModelData BuildModelData(float halfWidth, float halfHeight, float halfDepth = 0f)
		{
			bool flag = halfDepth == 0f;
			if (flag)
			{
				halfDepth = halfWidth;
			}
			float[] array = new float[64];
			for (int i = 0; i < 8; i++)
			{
				int num = i * 8;
				array[num] = (float)CubeModel.CubeVertices[i * 3] * halfWidth;
				array[num + 1] = (float)CubeModel.CubeVertices[i * 3 + 1] * halfHeight;
				array[num + 2] = (float)CubeModel.CubeVertices[i * 3 + 2] * halfDepth;
				array[num + 3] = 0f;
				array[num + 4] = 0f;
				array[num + 5] = 0f;
				array[num + 6] = 0f;
				array[num + 7] = 0f;
			}
			return new PrimitiveModelData(array, CubeModel.CubeVertexIndices);
		}

		// Token: 0x060055B8 RID: 21944 RVA: 0x00197B40 File Offset: 0x00195D40
		public static bool[,,] BuildVoxelData(int radiusX, int radiusY, int radiusZ)
		{
			bool[,,] array = new bool[radiusX * 2 + 1, radiusY * 2 + 1, radiusZ * 2 + 1];
			for (int i = -radiusX; i <= radiusX; i++)
			{
				for (int j = -radiusZ; j <= radiusZ; j++)
				{
					for (int k = radiusY; k >= -radiusY; k--)
					{
						array[i + radiusX, k + radiusY, j + radiusZ] = true;
					}
				}
			}
			return array;
		}

		// Token: 0x040032C0 RID: 12992
		private static readonly int[] CubeVertices = new int[]
		{
			-1,
			-1,
			1,
			1,
			-1,
			1,
			1,
			1,
			1,
			-1,
			1,
			1,
			-1,
			-1,
			-1,
			-1,
			1,
			-1,
			1,
			1,
			-1,
			1,
			-1,
			-1
		};

		// Token: 0x040032C1 RID: 12993
		private static readonly ushort[] CubeVertexIndices = new ushort[]
		{
			0,
			1,
			2,
			0,
			2,
			3,
			4,
			5,
			6,
			4,
			6,
			7,
			5,
			3,
			2,
			5,
			2,
			6,
			4,
			7,
			1,
			4,
			1,
			0,
			7,
			6,
			2,
			7,
			2,
			1,
			4,
			0,
			3,
			4,
			3,
			5
		};
	}
}
