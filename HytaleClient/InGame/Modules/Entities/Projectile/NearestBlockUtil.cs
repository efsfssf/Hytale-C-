using System;
using HytaleClient.Math;

namespace HytaleClient.InGame.Modules.Entities.Projectile
{
	// Token: 0x02000960 RID: 2400
	internal class NearestBlockUtil
	{
		// Token: 0x06004AFA RID: 19194 RVA: 0x00132B9C File Offset: 0x00130D9C
		public static IntVector3? FindNearestBlock<T>(Vector3 position, Func<IntVector3, T, bool> validBlock, T t)
		{
			return NearestBlockUtil.FindNearestBlock<T>(NearestBlockUtil.DefaultElements, position.X, position.Y, position.Z, validBlock, t);
		}

		// Token: 0x06004AFB RID: 19195 RVA: 0x00132BCC File Offset: 0x00130DCC
		public static IntVector3? FindNearestBlock<T>(NearestBlockUtil.IterationElement[] elements, Vector3 position, Func<IntVector3, T, bool> validBlock, T t)
		{
			return NearestBlockUtil.FindNearestBlock<T>(elements, position.X, position.Y, position.Z, validBlock, t);
		}

		// Token: 0x06004AFC RID: 19196 RVA: 0x00132BF8 File Offset: 0x00130DF8
		public static IntVector3? FindNearestBlock<T>(float x, float y, float z, Func<IntVector3, T, bool> validBlock, T t)
		{
			return NearestBlockUtil.FindNearestBlock<T>(NearestBlockUtil.DefaultElements, x, y, z, validBlock, t);
		}

		// Token: 0x06004AFD RID: 19197 RVA: 0x00132C1C File Offset: 0x00130E1C
		public static IntVector3? FindNearestBlock<T>(NearestBlockUtil.IterationElement[] elements, float x, float y, float z, Func<IntVector3, T, bool> validBlock, T t)
		{
			int num = (int)Math.Floor((double)x);
			int num2 = (int)Math.Floor((double)y);
			int num3 = (int)Math.Floor((double)z);
			float num4 = x % 1f;
			float num5 = y % 1f;
			float num6 = z % 1f;
			IntVector3? result = null;
			float num7 = float.PositiveInfinity;
			foreach (NearestBlockUtil.IterationElement iterationElement in elements)
			{
				float num8 = num4 - iterationElement.X(num4);
				float num9 = num5 - iterationElement.Y(num5);
				float num10 = num6 - iterationElement.Z(num6);
				float num11 = num8 * num8 + num9 * num9 + num10 * num10;
				IntVector3 intVector = new IntVector3(num + iterationElement.OffsetX, num2 + iterationElement.OffsetY, num3 + iterationElement.OffsetZ);
				bool flag = num11 < num7 && validBlock(intVector, t);
				if (flag)
				{
					num7 = num11;
					bool flag2 = result == null;
					if (flag2)
					{
						result = new IntVector3?(default(IntVector3));
					}
					result = new IntVector3?(intVector);
				}
			}
			return result;
		}

		// Token: 0x040026A4 RID: 9892
		public static readonly NearestBlockUtil.IterationElement[] DefaultElements = new NearestBlockUtil.IterationElement[]
		{
			new NearestBlockUtil.IterationElement(-1, 0, 0, (float x) => 0f, (float y) => y, (float z) => z),
			new NearestBlockUtil.IterationElement(1, 0, 0, (float x) => 1f, (float y) => y, (float z) => z),
			new NearestBlockUtil.IterationElement(0, -1, 0, (float x) => x, (float y) => 0f, (float z) => z),
			new NearestBlockUtil.IterationElement(0, 1, 0, (float x) => x, (float y) => 1f, (float z) => z),
			new NearestBlockUtil.IterationElement(0, 0, -1, (float x) => x, (float y) => y, (float z) => 0f),
			new NearestBlockUtil.IterationElement(0, 0, 1, (float x) => x, (float y) => y, (float z) => 1f)
		};

		// Token: 0x02000E4B RID: 3659
		public class IterationElement
		{
			// Token: 0x0600674C RID: 26444 RVA: 0x00217863 File Offset: 0x00215A63
			public IterationElement(int offsetX, int offsetY, int offsetZ, Func<float, float> x, Func<float, float> y, Func<float, float> z)
			{
				this.OffsetX = offsetX;
				this.OffsetY = offsetY;
				this.OffsetZ = offsetZ;
				this.X = x;
				this.Y = y;
				this.Z = z;
			}

			// Token: 0x040045EA RID: 17898
			public int OffsetX;

			// Token: 0x040045EB RID: 17899
			public int OffsetY;

			// Token: 0x040045EC RID: 17900
			public int OffsetZ;

			// Token: 0x040045ED RID: 17901
			public Func<float, float> X;

			// Token: 0x040045EE RID: 17902
			public Func<float, float> Y;

			// Token: 0x040045EF RID: 17903
			public Func<float, float> Z;
		}
	}
}
