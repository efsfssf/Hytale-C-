using System;
using HytaleClient.Data.Map;
using HytaleClient.Math;

namespace HytaleClient.InGame.Modules.Entities.Projectile
{
	// Token: 0x0200094C RID: 2380
	internal class BlockCollisionProvider : BoxBlockIterator.BoxIterationConsumer
	{
		// Token: 0x170011F5 RID: 4597
		// (set) Token: 0x06004A6C RID: 19052 RVA: 0x0012F1E9 File Offset: 0x0012D3E9
		public bool ReportOverlaps
		{
			set
			{
				this._reportOverlaps = value;
				this._movingBoxBoxCollisionEvaluator.ComputeOverlaps = this._reportOverlaps;
			}
		}

		// Token: 0x06004A6D RID: 19053 RVA: 0x0012F204 File Offset: 0x0012D404
		public bool Next()
		{
			return this.OnSliceFinished();
		}

		// Token: 0x06004A6E RID: 19054 RVA: 0x0012F21C File Offset: 0x0012D41C
		public bool Accept(long x, long y, long z)
		{
			return this.ProcessBlockDynamic((int)x, (int)y, (int)z);
		}

		// Token: 0x06004A6F RID: 19055 RVA: 0x0012F23C File Offset: 0x0012D43C
		public void Cast(GameInstance gameInstance, BoundingBox collider, Vector3 pos, Vector3 v, PredictedProjectile collisionConsumer, BlockTracker activeTriggers, float collisionStop)
		{
			this._collisionConsumer = collisionConsumer;
			this._activeTriggers = activeTriggers;
			this._motion = v;
			this._gameInstance = gameInstance;
			this._blockData.Initialize(gameInstance);
			bool flag = !CollisionMath.IsBelowMovementThreshold(v);
			bool flag2 = flag;
			if (flag2)
			{
				this.CastIterative(collider, pos, v, collisionStop);
			}
			else
			{
				this.CastShortDistance(collider, pos, v);
			}
			collisionConsumer.OnCollisionFinished();
			this._blockData.Cleanup();
			this._triggerTracker.Reset();
			this._damageTracker.Reset();
			this._collisionConsumer = null;
			this._activeTriggers = null;
			this._motion = Vector3.Zero;
		}

		// Token: 0x06004A70 RID: 19056 RVA: 0x0012F2EC File Offset: 0x0012D4EC
		protected void CastShortDistance(BoundingBox collider, Vector3 pos, Vector3 v)
		{
			this._boxBlockIntersectionEvaluator.SetBox(collider, pos).OffsetPosition(v);
			collider.ForEachBlock<BlockCollisionProvider>(pos.X + v.X, pos.Y + v.Y, pos.Z + v.Z, 1E-05f, this, (int x, int y, int z, BlockCollisionProvider _this) => _this.ProcessBlockStatic(x, y, z));
			this.GenerateTriggerExit();
		}

		// Token: 0x06004A71 RID: 19057 RVA: 0x0012F36C File Offset: 0x0012D56C
		protected bool ProcessBlockStatic(int x, int y, int z)
		{
			this._blockData.Read(x, y, z);
			BlockHitbox blockBoundingBoxes = this._blockData.GetBlockBoundingBoxes(this._gameInstance);
			int num = this._blockData.OriginX(x);
			int num2 = this._blockData.OriginY(y);
			int num3 = this._blockData.OriginZ(z);
			bool flag = this._blockData.IsTrigger && !this._triggerTracker.IsTracked(num, num2, num3);
			int num4 = this._blockData.BlockDamage;
			bool flag2 = this.CanCollide();
			BoundingBox[] boxes = blockBoundingBoxes.Boxes;
			this._boxBlockIntersectionEvaluator.SetDamageAndSubmerged(num4, false);
			bool flag3 = this._blockData.OriginalBlockType.CollisionMaterial != 2;
			bool result;
			if (flag3)
			{
				bool flag4 = num4 != 0 && blockBoundingBoxes.IsOversized();
				if (flag4)
				{
					bool flag5 = this._damageTracker.IsTracked(num, num2, num3);
					if (flag5)
					{
						num4 = 0;
					}
					else
					{
						this._damageTracker.TrackNew(num, num2, num3);
					}
				}
				bool flag6 = flag2 && blockBoundingBoxes.IsOversized();
				if (flag6)
				{
					bool flag7 = this._collisionTracker.IsTracked(num, num2, num3);
					if (flag7)
					{
						flag2 = false;
					}
					else
					{
						this._collisionTracker.TrackNew(num, num2, num3);
					}
				}
				int num5 = 0;
				while ((flag2 || flag || num4 > 0) && num5 < boxes.Length)
				{
					BoundingBox boundingBox = boxes[num5];
					bool flag8 = !CollisionMath.IsDisjoint(this._boxBlockIntersectionEvaluator.IntersectBoxComputeTouch(boundingBox, (float)num, (float)num2, (float)num3));
					if (flag8)
					{
						bool flag9 = flag2 || (this._boxBlockIntersectionEvaluator.Overlapping && this._reportOverlaps);
						if (flag9)
						{
							this._collisionConsumer.OnCollision(num, num2, num3, this._motion, this._boxBlockIntersectionEvaluator, this._blockData, boundingBox);
							flag2 = false;
						}
						bool flag10 = flag;
						if (flag10)
						{
							bool flag11 = this._activeTriggers.IsTracked(num, num2, num3);
							if (flag11)
							{
								this._collisionConsumer.OnCollisionTrigger(num, num2, num3, this._motion, this._boxBlockIntersectionEvaluator, this._blockData);
							}
							else
							{
								this._collisionConsumer.OnCollisionTriggerEnter(num, num2, num3, this._motion, this._boxBlockIntersectionEvaluator, this._blockData);
								this._activeTriggers.TrackNew(num, num2, num3);
							}
							this._triggerTracker.TrackNew(num, num2, num3);
							flag = false;
						}
						bool flag12 = num4 != 0;
						if (flag12)
						{
							this._collisionConsumer.OnCollisionDamage(num, num2, num3, this._motion, this._boxBlockIntersectionEvaluator, this._blockData);
							num4 = 0;
						}
					}
					num5++;
				}
				ClientBlockType submergeFluid = this._blockData.GetSubmergeFluid(this._gameInstance);
				bool flag13 = submergeFluid != null;
				if (flag13)
				{
					this.ProcessBlockStaticFluid(x, y, z, submergeFluid, true);
				}
				result = true;
			}
			else
			{
				bool flag14 = flag;
				if (flag14)
				{
					this._boxBlockIntersectionEvaluator.SetDamageAndSubmerged(num4, false);
					foreach (BoundingBox otherBox in boxes)
					{
						bool flag15 = !CollisionMath.IsDisjoint(this._boxBlockIntersectionEvaluator.IntersectBoxComputeTouch(otherBox, (float)num, (float)num2, (float)num3));
						if (flag15)
						{
							this._collisionConsumer.OnCollisionTrigger(num, num2, num3, this._motion, this._boxBlockIntersectionEvaluator, this._blockData);
							this._triggerTracker.TrackNew(num, num2, num3);
							break;
						}
					}
				}
				this.ProcessBlockStaticFluid(x, y, z, this._blockData.BlockType, false);
				result = true;
			}
			return result;
		}

		// Token: 0x06004A72 RID: 19058 RVA: 0x0012F700 File Offset: 0x0012D900
		protected void ProcessBlockStaticFluid(int x, int y, int z, ClientBlockType fluid, bool submergeFluid)
		{
			bool flag = false;
			bool flag2 = this.CanCollide(2);
			bool flag3 = flag || flag2;
			if (flag3)
			{
				this._fluidBox.Max.Y = this._blockData.FillHeight;
				bool flag4 = !CollisionMath.IsDisjoint(this._boxBlockIntersectionEvaluator.IntersectBoxComputeTouch(this._fluidBox, (float)x, (float)y, (float)z));
				if (flag4)
				{
					this._boxBlockIntersectionEvaluator.SetDamageAndSubmerged(0, submergeFluid);
					bool flag5 = flag2;
					if (flag5)
					{
						this._collisionConsumer.OnCollision(x, y, z, this._motion, this._boxBlockIntersectionEvaluator, this._blockData, this._fluidBox);
					}
					bool flag6 = flag;
					if (flag6)
					{
						this._collisionConsumer.OnCollisionDamage(x, y, z, this._motion, this._boxBlockIntersectionEvaluator, this._blockData);
					}
				}
			}
		}

		// Token: 0x06004A73 RID: 19059 RVA: 0x0012F7D0 File Offset: 0x0012D9D0
		protected bool CanCollide()
		{
			return this.CanCollide(this._blockData.CollisionMaterials);
		}

		// Token: 0x06004A74 RID: 19060 RVA: 0x0012F7F4 File Offset: 0x0012D9F4
		protected bool CanCollide(int collisionMaterials)
		{
			return (collisionMaterials & this.RequestedCollisionMaterials) != 0;
		}

		// Token: 0x06004A75 RID: 19061 RVA: 0x0012F814 File Offset: 0x0012DA14
		protected void CastIterative(BoundingBox collider, Vector3 pos, Vector3 v, float collisionStop)
		{
			this._relativeStopDistance = MathHelper.Clamp(collisionStop, 0f, 1f);
			this._collisionState = PredictedProjectile.Result.Continue;
			this._movingBoxBoxCollisionEvaluator.SetCollider(collider).SetMove(pos, v);
			collider.ForEachBlock<BlockCollisionProvider>(pos, 1E-05f, this, (int x, int y, int z, BlockCollisionProvider _this) => _this.ProcessBlockDynamic(x, y, z));
			BoxBlockIterator.Iterate(collider, pos, v, v.Length(), this);
			int count = this._damageTracker.Count;
			for (int i = 0; i < count; i++)
			{
				BlockContactData contactData = this._damageTracker.GetContactData(i);
				bool flag = contactData.CollisionStart <= this._relativeStopDistance;
				if (flag)
				{
					IntVector3 position = this._damageTracker.GetPosition(i);
					this._collisionConsumer.OnCollisionDamage(position.X, position.Y, position.Z, this._motion, contactData, this._damageTracker.GetBlockData(i));
				}
			}
			this.GenerateTriggerExit();
			count = this._triggerTracker.Count;
			for (int j = 0; j < count; j++)
			{
				BlockContactData contactData2 = this._triggerTracker.GetContactData(j);
				bool flag2 = contactData2.CollisionStart <= this._relativeStopDistance;
				if (flag2)
				{
					IntVector3 position2 = this._triggerTracker.GetPosition(j);
					int x2 = position2.X;
					int y2 = position2.Y;
					int z2 = position2.Z;
					bool flag3 = this._activeTriggers.IsTracked(x2, y2, z2);
					if (flag3)
					{
						this._collisionConsumer.OnCollisionTrigger(x2, y2, z2, this._motion, contactData2, this._triggerTracker.GetBlockData(j));
					}
					else
					{
						this._collisionConsumer.OnCollisionTriggerEnter(x2, y2, z2, this._motion, contactData2, this._triggerTracker.GetBlockData(j));
						this._activeTriggers.TrackNew(x2, y2, z2);
					}
				}
			}
		}

		// Token: 0x06004A76 RID: 19062 RVA: 0x0012FA20 File Offset: 0x0012DC20
		protected bool OnSliceFinished()
		{
			PredictedProjectile.Result result = this._collisionConsumer.OnCollisionSliceFinished();
			bool flag = this._collisionState < result;
			if (flag)
			{
				this._collisionState = result;
			}
			return this._collisionState == PredictedProjectile.Result.Continue;
		}

		// Token: 0x06004A77 RID: 19063 RVA: 0x0012FA5C File Offset: 0x0012DC5C
		protected bool ProcessBlockDynamic(int x, int y, int z)
		{
			this._blockData.Read(x, y, z);
			int num = this._blockData.OriginX(x);
			int num2 = this._blockData.OriginY(y);
			int num3 = this._blockData.OriginZ(z);
			BlockHitbox blockBoundingBoxes = this._blockData.GetBlockBoundingBoxes(this._gameInstance);
			bool flag = this.CanCollide();
			int num4 = this._blockData.BlockDamage;
			bool flag2 = this._blockData.IsTrigger;
			this._movingBoxBoxCollisionEvaluator.SetDamageAndSubmerged(num4, false);
			BlockContactData blockContactData = null;
			BlockContactData blockContactData2 = null;
			bool flag3 = flag2;
			if (flag3)
			{
				blockContactData = this._triggerTracker.GetContactData(num, num2, num3);
				bool flag4 = blockContactData != null;
				if (flag4)
				{
					flag2 = false;
				}
			}
			bool flag5 = num4 != 0;
			if (flag5)
			{
				bool flag6 = blockBoundingBoxes.IsOversized();
				if (flag6)
				{
					blockContactData2 = this._damageTracker.GetContactData(num, num2, num3);
					bool flag7 = blockContactData2 != null;
					if (flag7)
					{
						num4 = 0;
					}
				}
			}
			bool flag8 = this._blockData.OriginalBlockType.CollisionMaterial != 2;
			bool result3;
			if (flag8)
			{
				BoundingBox[] boxes = blockBoundingBoxes.Boxes;
				int i = 0;
				while (i < boxes.Length)
				{
					BoundingBox boundingBox = boxes[i];
					bool flag9 = this._movingBoxBoxCollisionEvaluator.IsBoundingBoxColliding(boundingBox, (float)num, (float)num2, (float)num3);
					if (flag9)
					{
						bool flag10 = this._movingBoxBoxCollisionEvaluator.CollisionStart > this._relativeStopDistance;
						if (flag10)
						{
							bool flag11 = this._movingBoxBoxCollisionEvaluator.Overlapping && this._reportOverlaps;
							if (flag11)
							{
								PredictedProjectile.Result result = this._collisionConsumer.OnCollision(num, num2, num3, this._motion, this._movingBoxBoxCollisionEvaluator, this._blockData, boundingBox);
								this.UpdateStopDistance(result);
							}
						}
						else
						{
							bool flag12 = flag || (this._movingBoxBoxCollisionEvaluator.Overlapping && this._reportOverlaps);
							if (flag12)
							{
								PredictedProjectile.Result result2 = this._collisionConsumer.OnCollision(num, num2, num3, this._motion, this._movingBoxBoxCollisionEvaluator, this._blockData, boundingBox);
								this.UpdateStopDistance(result2);
							}
							bool flag13 = flag2;
							if (flag13)
							{
								blockContactData = this.ProcessTriggerDynamic(num, num2, num3, blockContactData);
							}
							bool flag14 = num4 != 0;
							if (flag14)
							{
								blockContactData2 = this.ProcessDamageDynamic(num, num2, num3, blockContactData2);
							}
						}
					}
					IL_228:
					i++;
					continue;
					goto IL_228;
				}
				ClientBlockType submergeFluid = this._blockData.GetSubmergeFluid(this._gameInstance);
				bool flag15 = submergeFluid != null;
				if (flag15)
				{
					this.ProcessBlockDynamicFluid(x, y, z, submergeFluid, blockContactData2, true);
				}
				result3 = (this._collisionState != PredictedProjectile.Result.StopNow);
			}
			else
			{
				bool flag16 = flag2;
				if (flag16)
				{
					foreach (BoundingBox blockBoundingBox in blockBoundingBoxes.Boxes)
					{
						bool flag17 = this._movingBoxBoxCollisionEvaluator.IsBoundingBoxColliding(blockBoundingBox, (float)num, (float)num2, (float)num3) && this._movingBoxBoxCollisionEvaluator.CollisionStart <= this._relativeStopDistance;
						if (flag17)
						{
							blockContactData = this.ProcessTriggerDynamic(num, num2, num3, blockContactData);
						}
					}
				}
				this.ProcessBlockDynamicFluid(x, y, z, this._blockData.BlockType, blockContactData2, false);
				result3 = (this._collisionState != PredictedProjectile.Result.StopNow);
			}
			return result3;
		}

		// Token: 0x06004A78 RID: 19064 RVA: 0x0012FD84 File Offset: 0x0012DF84
		protected void ProcessBlockDynamicFluid(int x, int y, int z, ClientBlockType fluid, BlockContactData damageCollisionData, bool isSubmergeFluid)
		{
			bool flag = false;
			bool flag2 = this.CanCollide(2);
			bool flag3 = !flag && !flag2;
			if (!flag3)
			{
				this._fluidBox.Max.Y = this._blockData.FillHeight;
				bool flag4 = this._movingBoxBoxCollisionEvaluator.IsBoundingBoxColliding(this._fluidBox, (float)x, (float)y, (float)z) && this._movingBoxBoxCollisionEvaluator.CollisionStart <= this._relativeStopDistance;
				if (flag4)
				{
					this._movingBoxBoxCollisionEvaluator.SetDamageAndSubmerged(0, isSubmergeFluid);
					bool flag5 = flag2;
					if (flag5)
					{
						PredictedProjectile.Result result = this._collisionConsumer.OnCollision(x, y, z, this._motion, this._movingBoxBoxCollisionEvaluator, this._blockData, this._fluidBox);
						this.UpdateStopDistance(result);
					}
					bool flag6 = flag;
					if (flag6)
					{
						this.ProcessDamageDynamic(x, y, z, damageCollisionData);
					}
				}
			}
		}

		// Token: 0x06004A79 RID: 19065 RVA: 0x0012FE5C File Offset: 0x0012E05C
		protected BlockContactData ProcessTriggerDynamic(int blockX, int blockY, int blockZ, BlockContactData collisionData)
		{
			PredictedProjectile.Result result = this._collisionConsumer.ProbeCollisionTrigger(blockX, blockY, blockZ, this._motion, this._movingBoxBoxCollisionEvaluator, this._blockData);
			this.UpdateStopDistance(result);
			bool flag = collisionData == null;
			BlockContactData result2;
			if (flag)
			{
				result2 = this._triggerTracker.TrackNew(blockX, blockY, blockZ, this._movingBoxBoxCollisionEvaluator, this._blockData);
			}
			else
			{
				float collisionEnd = Math.Max(collisionData.CollisionEnd, this._movingBoxBoxCollisionEvaluator.CollisionEnd);
				bool flag2 = this._movingBoxBoxCollisionEvaluator.CollisionStart < collisionData.CollisionStart;
				if (flag2)
				{
					collisionData.Assign(this._movingBoxBoxCollisionEvaluator);
				}
				collisionData.CollisionEnd = collisionEnd;
				result2 = collisionData;
			}
			return result2;
		}

		// Token: 0x06004A7A RID: 19066 RVA: 0x0012FF0C File Offset: 0x0012E10C
		protected BlockContactData ProcessDamageDynamic(int blockX, int blockY, int blockZ, BlockContactData collisionData)
		{
			PredictedProjectile.Result result = this._collisionConsumer.ProbeCollisionDamage(blockX, blockY, blockZ, this._motion, this._movingBoxBoxCollisionEvaluator, this._blockData);
			this.UpdateStopDistance(result);
			bool flag = collisionData == null;
			BlockContactData result2;
			if (flag)
			{
				result2 = this._damageTracker.TrackNew(blockX, blockY, blockZ, this._movingBoxBoxCollisionEvaluator, this._blockData);
			}
			else
			{
				bool flag2 = this._movingBoxBoxCollisionEvaluator.CollisionStart < collisionData.CollisionStart;
				if (flag2)
				{
					collisionData.Assign(this._movingBoxBoxCollisionEvaluator);
				}
				result2 = collisionData;
			}
			return result2;
		}

		// Token: 0x06004A7B RID: 19067 RVA: 0x0012FF98 File Offset: 0x0012E198
		protected void UpdateStopDistance(PredictedProjectile.Result result)
		{
			bool flag = result == PredictedProjectile.Result.Continue;
			if (!flag)
			{
				bool flag2 = this._movingBoxBoxCollisionEvaluator.CollisionStart < this._relativeStopDistance;
				if (flag2)
				{
					this._relativeStopDistance = this._movingBoxBoxCollisionEvaluator.CollisionStart;
				}
				bool flag3 = result > this._collisionState;
				if (flag3)
				{
					this._collisionState = result;
				}
			}
		}

		// Token: 0x06004A7C RID: 19068 RVA: 0x0012FFF0 File Offset: 0x0012E1F0
		protected void GenerateTriggerExit()
		{
			for (int i = this._activeTriggers.Count - 1; i >= 0; i--)
			{
				IntVector3 position = this._activeTriggers.GetPosition(i);
				bool flag = !this._triggerTracker.IsTracked(position.X, position.Y, position.Z);
				if (flag)
				{
					this._collisionConsumer.OnCollisionTriggerExit(position.X, position.Y, position.Z);
					this._activeTriggers.Untrack(position.X, position.Y, position.Z);
				}
			}
		}

		// Token: 0x0400262B RID: 9771
		protected readonly BoxBlockIntersectionEvaluator _boxBlockIntersectionEvaluator = new BoxBlockIntersectionEvaluator();

		// Token: 0x0400262C RID: 9772
		protected readonly MovingBoxBoxCollisionEvaluator _movingBoxBoxCollisionEvaluator = new MovingBoxBoxCollisionEvaluator();

		// Token: 0x0400262D RID: 9773
		protected readonly BlockDataProvider _blockData = new BlockDataProvider();

		// Token: 0x0400262E RID: 9774
		protected BoundingBox _fluidBox = new BoundingBox(Vector3.Zero, Vector3.One);

		// Token: 0x0400262F RID: 9775
		protected readonly CollisionTracker _damageTracker = new CollisionTracker();

		// Token: 0x04002630 RID: 9776
		protected readonly CollisionTracker _triggerTracker = new CollisionTracker();

		// Token: 0x04002631 RID: 9777
		protected readonly BlockTracker _collisionTracker = new BlockTracker();

		// Token: 0x04002632 RID: 9778
		public int RequestedCollisionMaterials = 4;

		// Token: 0x04002633 RID: 9779
		protected bool _reportOverlaps;

		// Token: 0x04002634 RID: 9780
		protected PredictedProjectile _collisionConsumer;

		// Token: 0x04002635 RID: 9781
		protected BlockTracker _activeTriggers;

		// Token: 0x04002636 RID: 9782
		protected Vector3 _motion;

		// Token: 0x04002637 RID: 9783
		protected GameInstance _gameInstance;

		// Token: 0x04002638 RID: 9784
		protected float _relativeStopDistance;

		// Token: 0x04002639 RID: 9785
		protected PredictedProjectile.Result _collisionState;
	}
}
