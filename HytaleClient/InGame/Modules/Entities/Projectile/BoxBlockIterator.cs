using System;
using System.Threading;
using HytaleClient.Math;

namespace HytaleClient.InGame.Modules.Entities.Projectile
{
	// Token: 0x02000952 RID: 2386
	internal static class BoxBlockIterator
	{
		// Token: 0x170011F8 RID: 4600
		// (get) Token: 0x06004AA9 RID: 19113 RVA: 0x00130FC3 File Offset: 0x0012F1C3
		public static BoxBlockIterator.BoxIterationBuffer Buffer
		{
			get
			{
				return BoxBlockIterator.ThreadLocalBuffer.Value;
			}
		}

		// Token: 0x06004AAA RID: 19114 RVA: 0x00130FD0 File Offset: 0x0012F1D0
		public static bool Iterate(BoundingBox box, Vector3 position, Vector3 d, float maxDistance, BoxBlockIterator.BoxIterationConsumer consumer)
		{
			return BoxBlockIterator.Iterate(box, position, d, maxDistance, consumer, BoxBlockIterator.Buffer);
		}

		// Token: 0x06004AAB RID: 19115 RVA: 0x00130FF4 File Offset: 0x0012F1F4
		public static bool Iterate(BoundingBox box, Vector3 pos, Vector3 d, float maxDistance, BoxBlockIterator.BoxIterationConsumer consumer, BoxBlockIterator.BoxIterationBuffer buffer)
		{
			return BoxBlockIterator.Iterate(box.Min, box.Max, pos, d, maxDistance, consumer, buffer);
		}

		// Token: 0x06004AAC RID: 19116 RVA: 0x00131020 File Offset: 0x0012F220
		public static bool Iterate(BoundingBox box, float px, float py, float pz, float dx, float dy, float dz, float maxDistance, BoxBlockIterator.BoxIterationConsumer consumer)
		{
			return BoxBlockIterator.Iterate(box, px, py, pz, dx, dy, dz, maxDistance, consumer, BoxBlockIterator.Buffer);
		}

		// Token: 0x06004AAD RID: 19117 RVA: 0x0013104C File Offset: 0x0012F24C
		public static bool Iterate(BoundingBox box, float px, float py, float pz, float dx, float dy, float dz, float maxDistance, BoxBlockIterator.BoxIterationConsumer consumer, BoxBlockIterator.BoxIterationBuffer buffer)
		{
			return BoxBlockIterator.Iterate(box.Min, box.Max, px, py, pz, dx, dy, dz, maxDistance, consumer, buffer);
		}

		// Token: 0x06004AAE RID: 19118 RVA: 0x00131080 File Offset: 0x0012F280
		public static bool Iterate(Vector3 min, Vector3 max, float px, float py, float pz, float dx, float dy, float dz, float maxDistance, BoxBlockIterator.BoxIterationConsumer consumer)
		{
			return BoxBlockIterator.Iterate(min, max, px, py, pz, dx, dy, dz, maxDistance, consumer, BoxBlockIterator.Buffer);
		}

		// Token: 0x06004AAF RID: 19119 RVA: 0x001310AC File Offset: 0x0012F2AC
		public static bool Iterate(Vector3 min, Vector3 max, float px, float py, float pz, float dx, float dy, float dz, float maxDistance, BoxBlockIterator.BoxIterationConsumer consumer, BoxBlockIterator.BoxIterationBuffer buffer)
		{
			return BoxBlockIterator.Iterate(min.X, min.Y, min.Z, max.X, max.Y, max.Z, px, py, pz, dx, dy, dz, maxDistance, consumer, buffer);
		}

		// Token: 0x06004AB0 RID: 19120 RVA: 0x001310F8 File Offset: 0x0012F2F8
		public static bool Iterate(Vector3 min, Vector3 max, Vector3 pos, Vector3 d, float maxDistance, BoxBlockIterator.BoxIterationConsumer consumer)
		{
			return BoxBlockIterator.Iterate(min, max, pos, d, maxDistance, consumer, BoxBlockIterator.Buffer);
		}

		// Token: 0x06004AB1 RID: 19121 RVA: 0x0013111C File Offset: 0x0012F31C
		public static bool Iterate(Vector3 min, Vector3 max, Vector3 pos, Vector3 d, float maxDistance, BoxBlockIterator.BoxIterationConsumer consumer, BoxBlockIterator.BoxIterationBuffer buffer)
		{
			return BoxBlockIterator.Iterate(min.X, min.Y, min.Z, max.X, max.Y, max.Z, pos.X, pos.Y, pos.Z, d.X, d.Y, d.Z, maxDistance, consumer, buffer);
		}

		// Token: 0x06004AB2 RID: 19122 RVA: 0x00131184 File Offset: 0x0012F384
		public static bool Iterate(float minX, float minY, float minZ, float maxX, float maxY, float maxZ, float px, float py, float pz, float dx, float dy, float dz, float maxDistance, BoxBlockIterator.BoxIterationConsumer consumer)
		{
			return BoxBlockIterator.Iterate(minX, minY, minZ, maxX, maxY, maxZ, px, py, pz, dx, dy, dz, maxDistance, consumer, BoxBlockIterator.Buffer);
		}

		// Token: 0x06004AB3 RID: 19123 RVA: 0x001311B8 File Offset: 0x0012F3B8
		public static bool Iterate(float minX, float minY, float minZ, float maxX, float maxY, float maxZ, float px, float py, float pz, float dx, float dy, float dz, float maxDistance, BoxBlockIterator.BoxIterationConsumer consumer, BoxBlockIterator.BoxIterationBuffer buffer)
		{
			bool flag = minX > maxX;
			if (flag)
			{
				throw new ArgumentException("minX is larger than maxX! Given: " + minX.ToString() + " > " + maxX.ToString());
			}
			bool flag2 = minY > maxY;
			if (flag2)
			{
				throw new ArgumentException("minY is larger than maxY! Given: " + minY.ToString() + " > " + maxY.ToString());
			}
			bool flag3 = minZ > maxZ;
			if (flag3)
			{
				throw new ArgumentException("minZ is larger than maxZ! Given: " + minZ.ToString() + " > " + maxZ.ToString());
			}
			bool flag4 = consumer == null;
			if (flag4)
			{
				throw new ArgumentException("consumer is null!");
			}
			bool flag5 = buffer == null;
			if (flag5)
			{
				throw new ArgumentException("buffer is null!");
			}
			return BoxBlockIterator.Iterate0(minX, minY, minZ, maxX, maxY, maxZ, px, py, pz, dx, dy, dz, maxDistance, consumer, buffer);
		}

		// Token: 0x06004AB4 RID: 19124 RVA: 0x00131298 File Offset: 0x0012F498
		private static bool Iterate0(float minX, float minY, float minZ, float maxX, float maxY, float maxZ, float posX, float posY, float posZ, float dx, float dy, float dz, float maxDistance, BoxBlockIterator.BoxIterationConsumer consumer, BoxBlockIterator.BoxIterationBuffer buffer)
		{
			buffer.Consumer = consumer;
			buffer.Mx = maxX - minX;
			buffer.My = maxY - minY;
			buffer.Mz = maxZ - minZ;
			buffer.SignX = ((dx > 0f) ? -1 : 1);
			buffer.SignY = ((dy > 0f) ? -1 : 1);
			buffer.SignZ = ((dz > 0f) ? -1 : 1);
			float num = posX + ((dx > 0f) ? maxX : minX);
			float num2 = posY + ((dy > 0f) ? maxY : minY);
			float num3 = posZ + ((dz > 0f) ? maxZ : minZ);
			buffer.PosX = (long)num;
			buffer.PosY = (long)num2;
			buffer.PosZ = (long)num3;
			return ServerBlockIterator.Iterate<BoxBlockIterator.BoxIterationBuffer>(num, num2, num3, dx, dy, dz, maxDistance, delegate(int x, int y, int z, float px, float py, float pz, float qx, float qy, float qz, BoxBlockIterator.BoxIterationBuffer buf)
			{
				int num4 = (int)Math.Ceiling((double)(((buf.SignX < 0) ? (1f - px) : px) + buf.Mx));
				int num5 = (int)Math.Ceiling((double)(((buf.SignY < 0) ? (1f - py) : py) + buf.My));
				int num6 = (int)Math.Ceiling((double)(((buf.SignZ < 0) ? (1f - pz) : pz) + buf.Mz));
				bool flag = (long)x != buf.PosX;
				if (flag)
				{
					for (int i = 0; i < num5; i++)
					{
						for (int j = 0; j < num6; j++)
						{
							bool flag2 = !buf.Consumer.Accept((long)x, (long)(y + i * buf.SignY), (long)(z + j * buf.SignZ));
							if (flag2)
							{
								return false;
							}
						}
					}
					buf.PosX = (long)x;
				}
				bool flag3 = (long)y != buf.PosY;
				if (flag3)
				{
					for (int k = 0; k < num6; k++)
					{
						for (int l = 0; l < num4; l++)
						{
							bool flag4 = !buf.Consumer.Accept((long)(x + l * buf.SignX), (long)y, (long)(z + k * buf.SignZ));
							if (flag4)
							{
								return false;
							}
						}
					}
					buf.PosY = (long)y;
				}
				bool flag5 = (long)z != buf.PosZ;
				if (flag5)
				{
					for (int m = 0; m < num4; m++)
					{
						for (int n = 0; n < num5; n++)
						{
							bool flag6 = !buf.Consumer.Accept((long)(x + m * buf.SignX), (long)(y + n * buf.SignY), (long)z);
							if (flag6)
							{
								return false;
							}
						}
					}
					buf.PosZ = (long)z;
				}
				return buf.Consumer.Next();
			}, buffer);
		}

		// Token: 0x0400265A RID: 9818
		private static ThreadLocal<BoxBlockIterator.BoxIterationBuffer> ThreadLocalBuffer = new ThreadLocal<BoxBlockIterator.BoxIterationBuffer>(() => new BoxBlockIterator.BoxIterationBuffer());

		// Token: 0x02000E46 RID: 3654
		public interface BoxIterationConsumer
		{
			// Token: 0x0600673E RID: 26430
			bool Next();

			// Token: 0x0600673F RID: 26431
			bool Accept(long x, long y, long z);
		}

		// Token: 0x02000E47 RID: 3655
		public class BoxIterationBuffer
		{
			// Token: 0x040045CF RID: 17871
			public BoxBlockIterator.BoxIterationConsumer Consumer;

			// Token: 0x040045D0 RID: 17872
			public float Mx;

			// Token: 0x040045D1 RID: 17873
			public float My;

			// Token: 0x040045D2 RID: 17874
			public float Mz;

			// Token: 0x040045D3 RID: 17875
			public int SignX;

			// Token: 0x040045D4 RID: 17876
			public int SignY;

			// Token: 0x040045D5 RID: 17877
			public int SignZ;

			// Token: 0x040045D6 RID: 17878
			public long PosX;

			// Token: 0x040045D7 RID: 17879
			public long PosY;

			// Token: 0x040045D8 RID: 17880
			public long PosZ;
		}
	}
}
