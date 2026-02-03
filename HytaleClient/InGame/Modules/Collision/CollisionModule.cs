using System;
using System.Collections.Generic;
using HytaleClient.Data.Map;
using HytaleClient.InGame.Modules.Entities;
using HytaleClient.InGame.Modules.Map;
using HytaleClient.Math;
using HytaleClient.Protocol;

namespace HytaleClient.InGame.Modules.Collision
{
	// Token: 0x02000969 RID: 2409
	internal class CollisionModule : Module
	{
		// Token: 0x06004B3F RID: 19263 RVA: 0x001358C0 File Offset: 0x00133AC0
		public bool FindTargetBlockOut(ref Ray ray, ref CollisionModule.BlockRaycastOptions options, out CollisionModule.BlockResult result)
		{
			result = CollisionModule.BlockResult.Default;
			return this.FindTargetBlock(ref ray, ref options, ref result);
		}

		// Token: 0x06004B40 RID: 19264 RVA: 0x001358E8 File Offset: 0x00133AE8
		public bool FindTargetBlock(ref Ray ray, ref CollisionModule.BlockRaycastOptions options, ref CollisionModule.BlockResult result)
		{
			return this.FindTargetBlock(ref ray, ref options.RaycastOptions, ref options.Block, ref result);
		}

		// Token: 0x06004B41 RID: 19265 RVA: 0x00135910 File Offset: 0x00133B10
		public bool FindTargetBlock(ref Ray ray, ref Raycast.Options raycastOptions, ref CollisionModule.BlockOptions blockOptions, ref CollisionModule.BlockResult result)
		{
			BlockIterator blockIterator = new BlockIterator(ray, raycastOptions.Distance);
			BlockAccessor blockAccessor = new BlockAccessor(this._gameInstance.MapModule);
			while (blockIterator.HasNext())
			{
				IntVector3 intVector;
				Vector3 vector;
				Vector3 vector2;
				IntVector3 blockNormal;
				blockIterator.Step(out intVector, out vector, out vector2, out blockNormal);
				int blockIdFiller = blockAccessor.GetBlockIdFiller(intVector);
				bool flag = blockIdFiller == 0;
				if (!flag)
				{
					bool flag2 = blockOptions.BlockWhitelist != null && !blockOptions.BlockWhitelist.Contains(blockIdFiller);
					if (!flag2)
					{
						ClientBlockType clientBlockType = this._gameInstance.MapModule.ClientBlockTypes[blockIdFiller];
						BlockType.Material collisionMaterial = clientBlockType.CollisionMaterial;
						bool flag3 = blockOptions.IgnoreFluids && collisionMaterial == 2;
						if (!flag3)
						{
							bool flag4 = blockOptions.IgnoreEmptyCollisionMaterial && collisionMaterial == 0;
							if (!flag4)
							{
								BlockHitbox blockHitbox = this._gameInstance.ServerSettings.BlockHitboxes[clientBlockType.HitboxType];
								for (int i = 0; i < blockHitbox.Boxes.Length; i++)
								{
									BoundingBox box = blockHitbox.Boxes[i];
									bool flag5 = !Raycast.RaycastBox(ray, box, intVector, ref result.Result, ref raycastOptions);
									if (!flag5)
									{
										result.Block = intVector;
										result.BlockId = clientBlockType.Id;
										result.BlockType = clientBlockType;
										result.BoxId = i;
										result.BlockNormal = blockNormal;
									}
								}
							}
						}
					}
				}
			}
			return result.IsSuccess();
		}

		// Token: 0x06004B42 RID: 19266 RVA: 0x00135AAE File Offset: 0x00133CAE
		public CollisionModule(GameInstance gameInstance) : base(gameInstance)
		{
		}

		// Token: 0x06004B43 RID: 19267 RVA: 0x00135ABC File Offset: 0x00133CBC
		public bool FindNearestTarget(ref Ray ray, ref CollisionModule.CombinedOptions options, out CollisionModule.BlockResult? blockResult, out CollisionModule.EntityResult? entityResult)
		{
			Raycast.Result result;
			return this.FindNearestTarget(ref ray, ref options, out blockResult, out entityResult, out result);
		}

		// Token: 0x06004B44 RID: 19268 RVA: 0x00135ADC File Offset: 0x00133CDC
		public bool FindNearestTarget(ref Ray ray, ref CollisionModule.CombinedOptions options, out CollisionModule.BlockResult? blockResult, out CollisionModule.EntityResult? entityResult, out Raycast.Result result)
		{
			bool flag = !this.FindAllTargets(ref ray, ref options, out blockResult, out entityResult);
			bool result2;
			if (flag)
			{
				result = Raycast.Result.Default;
				result2 = false;
			}
			else
			{
				bool flag2 = blockResult != null && entityResult != null;
				if (flag2)
				{
					bool flag3 = blockResult.Value.Result.NearT < entityResult.Value.Result.NearT;
					if (flag3)
					{
						result = blockResult.Value.Result;
						entityResult = null;
					}
					else
					{
						result = entityResult.Value.Result;
						blockResult = null;
					}
				}
				else
				{
					bool flag4 = blockResult != null;
					if (flag4)
					{
						result = blockResult.Value.Result;
					}
					else
					{
						bool flag5 = entityResult != null;
						if (!flag5)
						{
							throw new InvalidOperationException();
						}
						result = entityResult.Value.Result;
					}
				}
				result2 = true;
			}
			return result2;
		}

		// Token: 0x06004B45 RID: 19269 RVA: 0x00135BD8 File Offset: 0x00133DD8
		public bool FindAllTargets(ref Ray ray, ref CollisionModule.CombinedOptions options, out CollisionModule.BlockResult? blockResult, out CollisionModule.EntityResult? entityResult)
		{
			blockResult = null;
			entityResult = null;
			bool enableBlock = options.EnableBlock;
			if (enableBlock)
			{
				CollisionModule.BlockResult @default = CollisionModule.BlockResult.Default;
				bool flag = this.FindTargetBlock(ref ray, ref options.RaycastOptions, ref options.Block, ref @default);
				if (flag)
				{
					blockResult = new CollisionModule.BlockResult?(@default);
				}
			}
			bool enableEntity = options.EnableEntity;
			if (enableEntity)
			{
				CollisionModule.EntityResult default2 = CollisionModule.EntityResult.Default;
				bool flag2 = this.FindTargetEntity(ref ray, ref options.RaycastOptions, ref options.Entity, ref default2);
				if (flag2)
				{
					entityResult = new CollisionModule.EntityResult?(default2);
				}
			}
			return blockResult != null || entityResult != null;
		}

		// Token: 0x06004B46 RID: 19270 RVA: 0x00135C84 File Offset: 0x00133E84
		public bool FindTargetEntityOut(ref Ray ray, ref CollisionModule.EntityRaycastOptions options, out CollisionModule.EntityResult result)
		{
			result = CollisionModule.EntityResult.Default;
			return this.FindTargetEntity(ref ray, ref options, ref result);
		}

		// Token: 0x06004B47 RID: 19271 RVA: 0x00135CAC File Offset: 0x00133EAC
		public bool FindTargetEntity(ref Ray ray, ref CollisionModule.EntityRaycastOptions options, ref CollisionModule.EntityResult result)
		{
			return this.FindTargetEntity(ref ray, ref options.RaycastOptions, ref options.Entity, ref result);
		}

		// Token: 0x06004B48 RID: 19272 RVA: 0x00135CD4 File Offset: 0x00133ED4
		public bool FindTargetEntity(ref Ray ray, ref Raycast.Options raycastOptions, ref CollisionModule.EntityOptions entityOptions, ref CollisionModule.EntityResult result)
		{
			Entity[] allEntities = this._gameInstance.EntityStoreModule.GetAllEntities();
			int entitiesCount = this._gameInstance.EntityStoreModule.GetEntitiesCount();
			for (int i = 0; i < entitiesCount; i++)
			{
				Entity entity = allEntities[i];
				bool flag = !entityOptions.CheckLocalPlayer && entity.NetworkId == this._gameInstance.LocalPlayerNetworkId;
				if (!flag)
				{
					bool flag2 = entityOptions.CheckOnlyTangibleEntities && !entity.IsTangible();
					if (!flag2)
					{
						bool flag3 = !Raycast.RaycastBox(ray, entity.Hitbox, entity.Position, ref result.Result, ref raycastOptions);
						if (!flag3)
						{
							result.Entity = entity;
						}
					}
				}
			}
			return result.IsSuccess();
		}

		// Token: 0x06004B49 RID: 19273 RVA: 0x00135DA0 File Offset: 0x00133FA0
		public bool CheckBlockCollision(IntVector3 block, BoundingBox box, Vector3 moveOffset, out CollisionModule.CollisionHitData hitData)
		{
			BlockAccessor blockAccessor = new BlockAccessor(this._gameInstance.MapModule);
			int blockIdFiller = blockAccessor.GetBlockIdFiller(block);
			ClientBlockType clientBlockType = this._gameInstance.MapModule.ClientBlockTypes[blockIdFiller];
			hitData = default(CollisionModule.CollisionHitData);
			bool flag = blockIdFiller == 0;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				BlockType.Material collisionMaterial = clientBlockType.CollisionMaterial;
				bool flag2 = collisionMaterial == null || collisionMaterial == 2;
				if (flag2)
				{
					result = false;
				}
				else
				{
					BlockHitbox blockHitbox = this._gameInstance.ServerSettings.BlockHitboxes[clientBlockType.HitboxType];
					foreach (BoundingBox boxB in blockHitbox.Boxes)
					{
						bool flag3 = CollisionModule.CheckBoxCollision(box, boxB, (float)block.X, (float)block.Y, (float)block.Z, moveOffset, out hitData);
						if (flag3)
						{
							return true;
						}
					}
					result = false;
				}
			}
			return result;
		}

		// Token: 0x06004B4A RID: 19274 RVA: 0x00135E88 File Offset: 0x00134088
		public static bool CheckBoxCollision(BoundingBox boxA, BoundingBox boxB, float offsetX, float offsetY, float offsetZ, Vector3 moveOffset, out CollisionModule.CollisionHitData hitData)
		{
			Vector3 zero = Vector3.Zero;
			Vector3 zero2 = Vector3.Zero;
			CollisionModule.GetRangeOverlap(boxA.Min.X, boxA.Max.X, boxB.Min.X + offsetX, boxB.Max.X + offsetX, moveOffset.X, ref zero2.X, ref zero.X);
			CollisionModule.GetRangeOverlap(boxA.Min.Y, boxA.Max.Y, boxB.Min.Y + offsetY, boxB.Max.Y + offsetY, moveOffset.Y, ref zero2.Y, ref zero.Y);
			CollisionModule.GetRangeOverlap(boxA.Min.Z, boxA.Max.Z, boxB.Min.Z + offsetZ, boxB.Max.Z + offsetZ, moveOffset.Z, ref zero2.Z, ref zero.Z);
			hitData = new CollisionModule.CollisionHitData(zero, zero2);
			return zero.X > 0f && zero.Y > 0f && zero.Z > 0f;
		}

		// Token: 0x06004B4B RID: 19275 RVA: 0x00135FC4 File Offset: 0x001341C4
		public static bool Get2DRayIntersection(Vector2 ray1Position, Vector2 ray1Direction, Vector2 ray2Position, Vector2 ray2Direction, out Vector2 intersection)
		{
			intersection = Vector2.Zero;
			float num = ray1Direction.Y / ray1Direction.X;
			float num2 = ray1Position.Y - num * ray1Position.X;
			float num3 = ray2Direction.Y / ray2Direction.X;
			float num4 = ray2Position.Y - num3 * ray2Position.X;
			bool flag = num - num3 == 0f;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				float num5 = (num4 - num2) / (num - num3);
				float y = num * num5 + num2;
				intersection = new Vector2(num5, y);
				result = true;
			}
			return result;
		}

		// Token: 0x06004B4C RID: 19276 RVA: 0x00136058 File Offset: 0x00134258
		public static bool CheckRayPlaneDistance(Vector3 planePoint, Vector3 planeNormal, Vector3 linePoint, Vector3 lineDirection, out float distance)
		{
			float num = Vector3.Dot(planeNormal, lineDirection);
			bool flag = num == 0f;
			bool result;
			if (flag)
			{
				distance = 0f;
				result = false;
			}
			else
			{
				distance = (Vector3.Dot(planeNormal, planePoint) - Vector3.Dot(planeNormal, linePoint)) / num;
				result = true;
			}
			return result;
		}

		// Token: 0x06004B4D RID: 19277 RVA: 0x001360A0 File Offset: 0x001342A0
		private static void GetRangeOverlap(float minA, float maxA, float minB, float maxB, float offset, ref float limit, ref float overlap)
		{
			bool flag = (minA <= minB && maxA >= maxB) || (minA > minB && maxA < maxB) || (minA < maxB && minA > minB) || (maxA > minB && maxA < maxB);
			if (flag)
			{
				overlap = ((offset > 0f) ? (maxA - minB) : (maxB - minA));
				limit = ((offset > 0f) ? minB : maxB);
			}
		}

		// Token: 0x02000E54 RID: 3668
		public struct BlockOptions
		{
			// Token: 0x04004609 RID: 17929
			public static CollisionModule.BlockOptions Default = new CollisionModule.BlockOptions
			{
				IgnoreFluids = false,
				IgnoreEmptyCollisionMaterial = false,
				BlockWhitelist = null
			};

			// Token: 0x0400460A RID: 17930
			public bool IgnoreFluids;

			// Token: 0x0400460B RID: 17931
			public bool IgnoreEmptyCollisionMaterial;

			// Token: 0x0400460C RID: 17932
			public HashSet<int> BlockWhitelist;
		}

		// Token: 0x02000E55 RID: 3669
		public struct BlockRaycastOptions
		{
			// Token: 0x0400460D RID: 17933
			public static CollisionModule.BlockRaycastOptions Default = new CollisionModule.BlockRaycastOptions
			{
				RaycastOptions = Raycast.Options.Default,
				Block = CollisionModule.BlockOptions.Default
			};

			// Token: 0x0400460E RID: 17934
			public Raycast.Options RaycastOptions;

			// Token: 0x0400460F RID: 17935
			public CollisionModule.BlockOptions Block;
		}

		// Token: 0x02000E56 RID: 3670
		public struct BlockResult
		{
			// Token: 0x06006774 RID: 26484 RVA: 0x00217E18 File Offset: 0x00216018
			public BlockPosition GetBlockPosition()
			{
				return new BlockPosition(this.Block.X, this.Block.Y, this.Block.Z);
			}

			// Token: 0x06006775 RID: 26485 RVA: 0x00217E50 File Offset: 0x00216050
			public bool IsSuccess()
			{
				return this.Result.IsSuccess();
			}

			// Token: 0x04004610 RID: 17936
			public static CollisionModule.BlockResult Default = new CollisionModule.BlockResult
			{
				Result = Raycast.Result.Default,
				Block = IntVector3.Zero
			};

			// Token: 0x04004611 RID: 17937
			public Raycast.Result Result;

			// Token: 0x04004612 RID: 17938
			public IntVector3 Block;

			// Token: 0x04004613 RID: 17939
			public IntVector3 BlockNormal;

			// Token: 0x04004614 RID: 17940
			public int BlockId;

			// Token: 0x04004615 RID: 17941
			public ClientBlockType BlockType;

			// Token: 0x04004616 RID: 17942
			public int BoxId;
		}

		// Token: 0x02000E57 RID: 3671
		public struct CombinedOptions
		{
			// Token: 0x04004617 RID: 17943
			public static readonly CollisionModule.CombinedOptions Default = new CollisionModule.CombinedOptions
			{
				RaycastOptions = Raycast.Options.Default,
				EnableBlock = true,
				Block = CollisionModule.BlockOptions.Default,
				EnableEntity = true,
				Entity = CollisionModule.EntityOptions.Default
			};

			// Token: 0x04004618 RID: 17944
			public Raycast.Options RaycastOptions;

			// Token: 0x04004619 RID: 17945
			public bool EnableBlock;

			// Token: 0x0400461A RID: 17946
			public CollisionModule.BlockOptions Block;

			// Token: 0x0400461B RID: 17947
			public bool EnableEntity;

			// Token: 0x0400461C RID: 17948
			public CollisionModule.EntityOptions Entity;
		}

		// Token: 0x02000E58 RID: 3672
		public struct EntityOptions
		{
			// Token: 0x0400461D RID: 17949
			public static CollisionModule.EntityOptions Default = new CollisionModule.EntityOptions
			{
				CheckLocalPlayer = false,
				CheckOnlyTangibleEntities = false
			};

			// Token: 0x0400461E RID: 17950
			public bool CheckLocalPlayer;

			// Token: 0x0400461F RID: 17951
			public bool CheckOnlyTangibleEntities;
		}

		// Token: 0x02000E59 RID: 3673
		public struct EntityRaycastOptions
		{
			// Token: 0x04004620 RID: 17952
			public static CollisionModule.EntityRaycastOptions Default = new CollisionModule.EntityRaycastOptions
			{
				RaycastOptions = Raycast.Options.Default,
				Entity = CollisionModule.EntityOptions.Default
			};

			// Token: 0x04004621 RID: 17953
			public Raycast.Options RaycastOptions;

			// Token: 0x04004622 RID: 17954
			public CollisionModule.EntityOptions Entity;
		}

		// Token: 0x02000E5A RID: 3674
		public struct EntityResult
		{
			// Token: 0x0600677A RID: 26490 RVA: 0x00217F54 File Offset: 0x00216154
			public bool IsSuccess()
			{
				return this.Result.IsSuccess();
			}

			// Token: 0x04004623 RID: 17955
			public static CollisionModule.EntityResult Default = new CollisionModule.EntityResult
			{
				Result = Raycast.Result.Default
			};

			// Token: 0x04004624 RID: 17956
			public Raycast.Result Result;

			// Token: 0x04004625 RID: 17957
			public Entity Entity;
		}

		// Token: 0x02000E5B RID: 3675
		public struct CollisionHitData
		{
			// Token: 0x17001469 RID: 5225
			// (get) Token: 0x0600677C RID: 26492 RVA: 0x00217F9B File Offset: 0x0021619B
			// (set) Token: 0x0600677D RID: 26493 RVA: 0x00217FA3 File Offset: 0x002161A3
			public Vector3 Overlap { get; private set; }

			// Token: 0x1700146A RID: 5226
			// (get) Token: 0x0600677E RID: 26494 RVA: 0x00217FAC File Offset: 0x002161AC
			// (set) Token: 0x0600677F RID: 26495 RVA: 0x00217FB4 File Offset: 0x002161B4
			public Vector3 Limit { get; private set; }

			// Token: 0x06006780 RID: 26496 RVA: 0x00217FBD File Offset: 0x002161BD
			public CollisionHitData(Vector3 overlap, Vector3 limit)
			{
				this.Overlap = overlap;
				this.Limit = limit;
			}

			// Token: 0x06006781 RID: 26497 RVA: 0x00217FD0 File Offset: 0x002161D0
			public bool GetXCollideState()
			{
				return this.Overlap.X > 0f;
			}

			// Token: 0x06006782 RID: 26498 RVA: 0x00217FF4 File Offset: 0x002161F4
			public bool GetYCollideState()
			{
				return this.Overlap.Y > 0f;
			}

			// Token: 0x06006783 RID: 26499 RVA: 0x00218018 File Offset: 0x00216218
			public bool GetZCollideState()
			{
				return this.Overlap.Z > 0f;
			}

			// Token: 0x06006784 RID: 26500 RVA: 0x0021803C File Offset: 0x0021623C
			public bool HasCollided()
			{
				return this.Overlap.X > 0f && this.Overlap.Y > 0f && this.Overlap.Z > 0f;
			}

			// Token: 0x06006785 RID: 26501 RVA: 0x00218088 File Offset: 0x00216288
			public override string ToString()
			{
				return string.Concat(new string[]
				{
					"{Overlap: ",
					this.Overlap.ToString(),
					", Limit: ",
					this.Limit.ToString(),
					"}"
				});
			}
		}
	}
}
