using System;
using HytaleClient.Math;

namespace HytaleClient.InGame.Modules.Entities.Projectile
{
	// Token: 0x02000951 RID: 2385
	internal class BoxBlockIntersectionEvaluator : BlockContactData
	{
		// Token: 0x06004AA0 RID: 19104 RVA: 0x00130A91 File Offset: 0x0012EC91
		public BoxBlockIntersectionEvaluator()
		{
			this.SetStartEnd(0f, 1f);
		}

		// Token: 0x06004AA1 RID: 19105 RVA: 0x00130AB8 File Offset: 0x0012ECB8
		public void SetCollisionData(BlockCollisionData data, CollisionConfig collisionConfig, int hitboxIndex)
		{
			data.SetStart(this.CollisionPoint, this.CollisionStart);
			data.SetEnd(this.CollisionEnd, this.CollisionNormal);
			data.SetBlockData(collisionConfig);
			data.SetDetailBoxIndex(hitboxIndex);
			data.SetTouchingOverlapping(CollisionMath.IsTouching(this._resultCode), CollisionMath.IsOverlapping(this._resultCode));
		}

		// Token: 0x06004AA2 RID: 19106 RVA: 0x00130B1C File Offset: 0x0012ED1C
		public BoxBlockIntersectionEvaluator SetBox(BoundingBox box)
		{
			this._box = box;
			return this;
		}

		// Token: 0x06004AA3 RID: 19107 RVA: 0x00130B38 File Offset: 0x0012ED38
		public BoxBlockIntersectionEvaluator ExpandBox(float radius)
		{
			this._box.Grow(new Vector3(radius));
			return this;
		}

		// Token: 0x06004AA4 RID: 19108 RVA: 0x00130B60 File Offset: 0x0012ED60
		public BoxBlockIntersectionEvaluator SetPosition(Vector3 pos)
		{
			this.CollisionPoint = pos;
			return this;
		}

		// Token: 0x06004AA5 RID: 19109 RVA: 0x00130B7C File Offset: 0x0012ED7C
		public BoxBlockIntersectionEvaluator SetBox(BoundingBox box, Vector3 pos)
		{
			return this.SetBox(box).SetPosition(pos);
		}

		// Token: 0x06004AA6 RID: 19110 RVA: 0x00130B9C File Offset: 0x0012ED9C
		public BoxBlockIntersectionEvaluator OffsetPosition(Vector3 offset)
		{
			this.CollisionPoint += offset;
			return this;
		}

		// Token: 0x06004AA7 RID: 19111 RVA: 0x00130BC4 File Offset: 0x0012EDC4
		public BoxBlockIntersectionEvaluator SetStartEnd(float start, float end)
		{
			this.CollisionStart = start;
			this.CollisionEnd = end;
			return this;
		}

		// Token: 0x06004AA8 RID: 19112 RVA: 0x00130BE8 File Offset: 0x0012EDE8
		public int IntersectBoxComputeTouch(BoundingBox otherBox, float x, float y, float z)
		{
			int num = CollisionMath.IntersectAABBs(this.CollisionPoint.X, this.CollisionPoint.Y, this.CollisionPoint.Z, this._box, x, y, z, otherBox);
			this._resultCode = num;
			this.OnGround = false;
			this._touchCeil = false;
			this.CollisionNormal = Vector3.Zero;
			this.Overlapping = CollisionMath.IsOverlapping(this._resultCode);
			bool flag = (num & 7) != 0;
			if (flag)
			{
				bool flag2 = this._worldUp.Y != 0f;
				if (flag2)
				{
					bool flag3 = (num & 2) != 0;
					if (flag3)
					{
						this.CollisionNormal.Y = (float)((y + otherBox.Min.Y < this.CollisionPoint.Y + this._box.Min.Y) ? 1 : -1);
						this.OnGround = (this.CollisionNormal.Y == this._worldUp.Y);
						this._touchCeil = !this.OnGround;
					}
					else
					{
						bool flag4 = (num & 1) != 0;
						if (flag4)
						{
							this.CollisionNormal.X = (float)((x + otherBox.Min.X < this.CollisionPoint.X + this._box.Min.X) ? 1 : -1);
						}
						else
						{
							this.CollisionNormal.Z = (float)((z + otherBox.Min.Z < this.CollisionPoint.Z + this._box.Min.Z) ? 1 : -1);
						}
					}
				}
				else
				{
					bool flag5 = this._worldUp.X != 0f;
					if (flag5)
					{
						bool flag6 = (num & 1) != 0;
						if (flag6)
						{
							this.CollisionNormal.X = (float)((x + otherBox.Min.X < this.CollisionPoint.X + this._box.Min.X) ? 1 : -1);
							this.OnGround = (this.CollisionNormal.X == this._worldUp.X);
							this._touchCeil = !this.OnGround;
						}
						else
						{
							bool flag7 = (num & 2) != 0;
							if (flag7)
							{
								this.CollisionNormal.Y = (float)((y + otherBox.Min.Y < this.CollisionPoint.Y + this._box.Min.Y) ? 1 : -1);
							}
							else
							{
								this.CollisionNormal.Z = (float)((z + otherBox.Min.Z < this.CollisionPoint.Z + this._box.Min.Z) ? 1 : -1);
							}
						}
					}
					else
					{
						bool flag8 = (num & 4) != 0;
						if (flag8)
						{
							this.CollisionNormal.Z = (float)((z + otherBox.Min.Z < this.CollisionPoint.Z + this._box.Min.Z) ? 1 : -1);
							this.OnGround = (this.CollisionNormal.Z == this._worldUp.Z);
							this._touchCeil = !this.OnGround;
						}
						else
						{
							bool flag9 = (num & 2) != 0;
							if (flag9)
							{
								this.CollisionNormal.Y = (float)((y + otherBox.Min.Y < this.CollisionPoint.Y + this._box.Min.Y) ? 1 : -1);
							}
							else
							{
								this.CollisionNormal.X = (float)((x + otherBox.Min.X < this.CollisionPoint.X + this._box.Min.X) ? 1 : -1);
							}
						}
					}
				}
			}
			return num;
		}

		// Token: 0x04002656 RID: 9814
		protected BoundingBox _box;

		// Token: 0x04002657 RID: 9815
		protected Vector3 _worldUp = Vector3.Up;

		// Token: 0x04002658 RID: 9816
		protected bool _touchCeil;

		// Token: 0x04002659 RID: 9817
		protected int _resultCode;
	}
}
