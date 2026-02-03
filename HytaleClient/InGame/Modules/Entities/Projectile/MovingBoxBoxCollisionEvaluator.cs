using System;
using HytaleClient.Math;

namespace HytaleClient.InGame.Modules.Entities.Projectile
{
	// Token: 0x0200095F RID: 2399
	internal class MovingBoxBoxCollisionEvaluator : BlockContactData
	{
		// Token: 0x06004AF5 RID: 19189 RVA: 0x001324C8 File Offset: 0x001306C8
		public void SetCollisionData(BlockCollisionData data, CollisionConfig collisionConfig, int hitboxIndex)
		{
			data.SetStart(this.CollisionPoint, this.CollisionStart);
			data.SetEnd(this.CollisionEnd, this.CollisionNormal);
			data.SetBlockData(collisionConfig);
			data.SetDetailBoxIndex(hitboxIndex);
			data.SetTouchingOverlapping(this._touching, this.Overlapping);
		}

		// Token: 0x06004AF6 RID: 19190 RVA: 0x00132520 File Offset: 0x00130720
		public MovingBoxBoxCollisionEvaluator SetCollider(BoundingBox collider)
		{
			this._collider = collider;
			return this;
		}

		// Token: 0x06004AF7 RID: 19191 RVA: 0x0013253C File Offset: 0x0013073C
		public MovingBoxBoxCollisionEvaluator SetMove(Vector3 pos, Vector3 v)
		{
			this._pos = pos;
			this._v = v;
			this._cX.V = v.X;
			this._cY.V = v.Y;
			this._cZ.V = v.Z;
			return this;
		}

		// Token: 0x06004AF8 RID: 19192 RVA: 0x00132590 File Offset: 0x00130790
		public bool IsBoundingBoxColliding(BoundingBox blockBoundingBox, float x, float y, float z)
		{
			this._cX.P = this._pos.X - x;
			this._cY.P = this._pos.Y - y;
			this._cZ.P = this._pos.Z - z;
			this.OnGround = false;
			this._touching = false;
			this.Overlapping = false;
			bool flag = !this._cX.IsColliding(blockBoundingBox.Min.X - this._collider.Max.X, blockBoundingBox.Max.X - this._collider.Min.X);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = !this._cY.IsColliding(blockBoundingBox.Min.Y - this._collider.Max.Y, blockBoundingBox.Max.Y - this._collider.Min.Y);
				if (flag2)
				{
					result = false;
				}
				else
				{
					bool flag3 = !this._cZ.IsColliding(blockBoundingBox.Min.Z - this._collider.Max.Z, blockBoundingBox.Max.Z - this._collider.Min.Z);
					if (flag3)
					{
						result = false;
					}
					else
					{
						bool flag4 = this._cX.Kind == 1 && this._cY.Kind == 1 && this._cZ.Kind == 1;
						if (flag4)
						{
							this.Overlapping = true;
							bool flag5 = !this.ComputeOverlaps;
							if (flag5)
							{
								result = false;
							}
							else
							{
								this.CollisionStart = 0f;
								this.CollisionEnd = float.MaxValue;
								this.CollisionNormal = Vector3.Zero;
								bool flag6 = this._cX.TLeave < this.CollisionEnd;
								if (flag6)
								{
									this.CollisionEnd = this._cX.TLeave;
									this.CollisionNormal = new Vector3(this._cX.Normal, 0f, 0f);
								}
								bool flag7 = this._cY.TLeave < this.CollisionEnd;
								if (flag7)
								{
									this.CollisionEnd = this._cY.TLeave;
									this.CollisionNormal = new Vector3(0f, this._cY.Normal, 0f);
								}
								bool flag8 = this._cZ.TLeave < this.CollisionEnd;
								if (flag8)
								{
									this.CollisionEnd = this._cZ.TLeave;
									this.CollisionNormal = new Vector3(0f, 0f, this._cZ.Normal);
								}
								result = true;
							}
						}
						else
						{
							this.CollisionStart = float.MinValue;
							this.CollisionEnd = float.MaxValue;
							bool flag9 = this._cX.Kind == 0;
							if (flag9)
							{
								this.CollisionNormal = new Vector3(this._cX.Normal, 0f, 0f);
								this.CollisionStart = this._cX.TEnter;
							}
							bool flag10 = this._cY.Kind == 0 && this._cY.TEnter > this.CollisionStart;
							if (flag10)
							{
								this.CollisionNormal = new Vector3(0f, this._cY.Normal, 0f);
								this.CollisionStart = this._cY.TEnter;
							}
							bool flag11 = this._cZ.Kind == 0 && this._cZ.TEnter > this.CollisionStart;
							if (flag11)
							{
								this.CollisionNormal = new Vector3(0f, 0f, this._cZ.Normal);
								this.CollisionStart = this._cZ.TEnter;
							}
							bool flag12 = this.CollisionStart > float.MinValue;
							if (flag12)
							{
								this.CollisionEnd = MathHelper.Min(this._cX.TLeave, this._cY.TLeave, this._cZ.TLeave);
								bool flag13 = this.CollisionStart > this.CollisionEnd;
								if (flag13)
								{
									result = false;
								}
								else
								{
									this.CollisionPoint = this._pos;
									this.CollisionPoint += this._v * this.CollisionStart;
									bool flag14 = this._checkForOnGround && this._cY.Kind == 3;
									if (flag14)
									{
										this.CollisionNormal = new Vector3(0f, this._cY.Normal, 0f);
										this.OnGround = true;
										this._touching = true;
										result = false;
									}
									else
									{
										this._touching = (this._cX.Kind >= 2 || this._cY.Kind >= 2 || this._cZ.Kind >= 2);
										result = !this._touching;
									}
								}
							}
							else
							{
								bool flag15 = this._checkForOnGround && this._cY.Kind == 3;
								if (flag15)
								{
									this.CollisionStart = MathHelper.Max(this._cX.TEnter, this._cY.TEnter, this._cZ.TEnter);
									this.CollisionEnd = MathHelper.Min(this._cX.TLeave, this._cY.TLeave, this._cZ.TLeave);
									this.CollisionPoint = this._pos;
									this.CollisionPoint += this._v * this.CollisionStart;
									this.CollisionNormal = new Vector3(0f, this._cY.Normal, 0f);
									this.OnGround = true;
									this._touching = true;
								}
								result = false;
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x0400269B RID: 9883
		protected bool _touching;

		// Token: 0x0400269C RID: 9884
		protected BoundingBox _collider;

		// Token: 0x0400269D RID: 9885
		protected Vector3 _pos;

		// Token: 0x0400269E RID: 9886
		protected Vector3 _v;

		// Token: 0x0400269F RID: 9887
		protected bool _checkForOnGround = true;

		// Token: 0x040026A0 RID: 9888
		public bool ComputeOverlaps;

		// Token: 0x040026A1 RID: 9889
		protected MovingBoxBoxCollisionEvaluator.Collision1D _cX = new MovingBoxBoxCollisionEvaluator.Collision1D();

		// Token: 0x040026A2 RID: 9890
		protected MovingBoxBoxCollisionEvaluator.Collision1D _cY = new MovingBoxBoxCollisionEvaluator.Collision1D();

		// Token: 0x040026A3 RID: 9891
		protected MovingBoxBoxCollisionEvaluator.Collision1D _cZ = new MovingBoxBoxCollisionEvaluator.Collision1D();

		// Token: 0x02000E4A RID: 3658
		protected class Collision1D
		{
			// Token: 0x06006748 RID: 26440 RVA: 0x00217614 File Offset: 0x00215814
			public bool IsColliding(float min, float max)
			{
				this.Min = min;
				this.Max = max;
				this.TEnter = float.MinValue;
				this.TLeave = float.MaxValue;
				this.Normal = 0f;
				this.Touching = false;
				float num = min - this.P;
				bool flag = num >= -1E-05f;
				bool result;
				if (flag)
				{
					bool flag2 = this.V < num - 1E-05f;
					if (flag2)
					{
						result = false;
					}
					else
					{
						this.Normal = -1f;
						this.ComputeTouchOrOutside(max, num, 2);
						result = true;
					}
				}
				else
				{
					num = max - this.P;
					bool flag3 = num <= 1E-05f;
					if (flag3)
					{
						bool flag4 = this.V > num + 1E-05f;
						if (flag4)
						{
							result = false;
						}
						else
						{
							this.Normal = 1f;
							this.ComputeTouchOrOutside(min, num, 3);
							result = true;
						}
					}
					else
					{
						this.TEnter = 0f;
						bool flag5 = this.V < 0f;
						if (flag5)
						{
							this.TLeave = this.ClampPos((min - this.P) / this.V);
							this.Normal = 1f;
						}
						else
						{
							bool flag6 = this.V > 0f;
							if (flag6)
							{
								this.TLeave = this.ClampPos((max - this.P) / this.V);
								this.Normal = -1f;
							}
						}
						this.Kind = 1;
						result = true;
					}
				}
				return result;
			}

			// Token: 0x06006749 RID: 26441 RVA: 0x0021778C File Offset: 0x0021598C
			private void ComputeTouchOrOutside(float border, float dist, int touchCode)
			{
				bool flag = this.V != 0f;
				if (flag)
				{
					this.TEnter = MathHelper.Clamp(dist / this.V, 0f, 1f);
					bool flag2 = this.TEnter != 0f && (double)this.TEnter < 1E-08;
					if (flag2)
					{
						this.TEnter = 0f;
					}
					this.TLeave = this.ClampPos((border - this.P) / this.V);
					this.Kind = 0;
				}
				else
				{
					this.TEnter = 0f;
					this.Kind = touchCode;
				}
			}

			// Token: 0x0600674A RID: 26442 RVA: 0x00217838 File Offset: 0x00215A38
			private float ClampPos(float v)
			{
				return (v >= 0f) ? v : 0f;
			}

			// Token: 0x040045DD RID: 17885
			public const int CollisionOutside = 0;

			// Token: 0x040045DE RID: 17886
			public const int CollisionInside = 1;

			// Token: 0x040045DF RID: 17887
			public const int CollisionTouchMin = 2;

			// Token: 0x040045E0 RID: 17888
			public const int CollisionTouchMax = 3;

			// Token: 0x040045E1 RID: 17889
			public float P;

			// Token: 0x040045E2 RID: 17890
			public float V;

			// Token: 0x040045E3 RID: 17891
			public float Min;

			// Token: 0x040045E4 RID: 17892
			public float Max;

			// Token: 0x040045E5 RID: 17893
			public float TEnter;

			// Token: 0x040045E6 RID: 17894
			public float TLeave;

			// Token: 0x040045E7 RID: 17895
			public float Normal;

			// Token: 0x040045E8 RID: 17896
			public int Kind;

			// Token: 0x040045E9 RID: 17897
			public bool Touching;
		}
	}
}
