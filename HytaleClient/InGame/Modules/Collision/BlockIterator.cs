using System;
using HytaleClient.Math;

namespace HytaleClient.InGame.Modules.Collision
{
	// Token: 0x02000968 RID: 2408
	public struct BlockIterator
	{
		// Token: 0x06004B39 RID: 19257 RVA: 0x001351B5 File Offset: 0x001333B5
		public BlockIterator(Ray ray, float maxDistance)
		{
			this = new BlockIterator(ray.Position, ray.Direction, maxDistance);
		}

		// Token: 0x06004B3A RID: 19258 RVA: 0x001351CC File Offset: 0x001333CC
		public BlockIterator(Vector3 origin, Vector3 direction, float maxDistance)
		{
			IntVector3 intVector = new IntVector3(origin);
			Vector3 offset = new Vector3(origin.X - (float)intVector.X, origin.Y - (float)intVector.Y, origin.Z - (float)intVector.Z);
			this = new BlockIterator(intVector, offset, direction, maxDistance);
		}

		// Token: 0x06004B3B RID: 19259 RVA: 0x00135224 File Offset: 0x00133424
		public BlockIterator(IntVector3 block, Vector3 offset, Vector3 direction, float maxDistance)
		{
			bool flag = offset.X < 0f || offset.Y < 0f || offset.Z < 0f || offset.X > 1f || offset.Y > 1f || offset.Z > 1f;
			if (flag)
			{
				string str = "Offset is out of bounds 0 <= ? <= 1! Given: ";
				Vector3 vector = offset;
				throw new Exception(str + vector.ToString());
			}
			this._block = block;
			this._direction = Vector3.Normalize(direction);
			this._maxDistance = maxDistance;
			this._position = offset;
			this._t = 0f;
			this._iterations = 0;
		}

		// Token: 0x06004B3C RID: 19260 RVA: 0x001352DC File Offset: 0x001334DC
		public bool HasNext()
		{
			return this._t <= this._maxDistance || this._iterations > 5000;
		}

		// Token: 0x06004B3D RID: 19261 RVA: 0x0013530C File Offset: 0x0013350C
		public void Step(out IntVector3 b, out Vector3 p, out Vector3 q, out IntVector3 n)
		{
			float num = this.Intersection();
			b = this._block;
			p = this._position;
			q = this._position + num * this._direction;
			n = IntVector3.Zero;
			bool flag = this._direction.X < 0f && q.X <= 0f;
			if (flag)
			{
				q.X += 1f;
				n.X = 1;
				this._block.X = this._block.X - 1;
			}
			else
			{
				bool flag2 = this._direction.X > 0f && q.X >= 1f;
				if (flag2)
				{
					q.X -= 1f;
					n.X = -1;
					this._block.X = this._block.X + 1;
				}
			}
			bool flag3 = this._direction.Y < 0f && q.Y <= 0f;
			if (flag3)
			{
				q.Y += 1f;
				n.Y = 1;
				this._block.Y = this._block.Y - 1;
			}
			else
			{
				bool flag4 = this._direction.Y > 0f && q.Y >= 1f;
				if (flag4)
				{
					q.Y -= 1f;
					n.Y = -1;
					this._block.Y = this._block.Y + 1;
				}
			}
			bool flag5 = this._direction.Z < 0f && q.Z <= 0f;
			if (flag5)
			{
				q.Z += 1f;
				n.Z = 1;
				this._block.Z = this._block.Z - 1;
			}
			else
			{
				bool flag6 = this._direction.Z > 0f && q.Z >= 1f;
				if (flag6)
				{
					q.Z -= 1f;
					n.Z = -1;
					this._block.Z = this._block.Z + 1;
				}
			}
			this._t += num;
			this._position = q;
			this._iterations++;
		}

		// Token: 0x06004B3E RID: 19262 RVA: 0x00135584 File Offset: 0x00133784
		private float Intersection()
		{
			Vector3 position = this._position;
			Vector3 direction = this._direction;
			float num = 0f;
			bool flag = direction.X < 0f;
			if (flag)
			{
				float num2 = -position.X / direction.X;
				float num3 = position.Z + direction.Z * num2;
				float num4 = position.Y + direction.Y * num2;
				bool flag2 = num2 > num && num3 >= 0f && num3 <= 1f && num4 >= 0f && num4 <= 1f;
				if (flag2)
				{
					num = num2;
				}
			}
			else
			{
				bool flag3 = direction.X > 0f;
				if (flag3)
				{
					float num2 = (1f - position.X) / direction.X;
					float num3 = position.Z + direction.Z * num2;
					float num4 = position.Y + direction.Y * num2;
					bool flag4 = num2 > num && num3 >= 0f && num3 <= 1f && num4 >= 0f && num4 <= 1f;
					if (flag4)
					{
						num = num2;
					}
				}
			}
			bool flag5 = direction.Y < 0f;
			if (flag5)
			{
				float num2 = -position.Y / direction.Y;
				float num3 = position.X + direction.X * num2;
				float num4 = position.Z + direction.Z * num2;
				bool flag6 = num2 > num && num3 >= 0f && num3 <= 1f && num4 >= 0f && num4 <= 1f;
				if (flag6)
				{
					num = num2;
				}
			}
			else
			{
				bool flag7 = direction.Y > 0f;
				if (flag7)
				{
					float num2 = (1f - position.Y) / direction.Y;
					float num3 = position.X + direction.X * num2;
					float num4 = position.Z + direction.Z * num2;
					bool flag8 = num2 > num && num3 >= 0f && num3 <= 1f && num4 >= 0f && num4 <= 1f;
					if (flag8)
					{
						num = num2;
					}
				}
			}
			bool flag9 = direction.Z < 0f;
			if (flag9)
			{
				float num2 = -position.Z / direction.Z;
				float num3 = position.X + direction.X * num2;
				float num4 = position.Y + direction.Y * num2;
				bool flag10 = num2 > num && num3 >= 0f && num3 <= 1f && num4 >= 0f && num4 <= 1f;
				if (flag10)
				{
					num = num2;
				}
			}
			else
			{
				bool flag11 = direction.Z > 0f;
				if (flag11)
				{
					float num2 = (1f - position.Z) / direction.Z;
					float num3 = position.X + direction.X * num2;
					float num4 = position.Y + direction.Y * num2;
					bool flag12 = num2 > num && num3 >= 0f && num3 <= 1f && num4 >= 0f && num4 <= 1f;
					if (flag12)
					{
						num = num2;
					}
				}
			}
			return num;
		}

		// Token: 0x040026E5 RID: 9957
		private const int MaxIterations = 5000;

		// Token: 0x040026E6 RID: 9958
		private readonly Vector3 _direction;

		// Token: 0x040026E7 RID: 9959
		private readonly float _maxDistance;

		// Token: 0x040026E8 RID: 9960
		private IntVector3 _block;

		// Token: 0x040026E9 RID: 9961
		private Vector3 _position;

		// Token: 0x040026EA RID: 9962
		private float _t;

		// Token: 0x040026EB RID: 9963
		private int _iterations;
	}
}
