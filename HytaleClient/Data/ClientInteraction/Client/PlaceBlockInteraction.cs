using System;
using System.Linq;
using System.Runtime.CompilerServices;
using HytaleClient.Data.Items;
using HytaleClient.Data.Map;
using HytaleClient.InGame;
using HytaleClient.InGame.Modules;
using HytaleClient.InGame.Modules.Entities;
using HytaleClient.InGame.Modules.Interaction;
using HytaleClient.Math;
using HytaleClient.Protocol;
using HytaleClient.Utils;

namespace HytaleClient.Data.ClientInteraction.Client
{
	// Token: 0x02000B45 RID: 2885
	internal class PlaceBlockInteraction : SimpleInteraction
	{
		// Token: 0x06005962 RID: 22882 RVA: 0x001B7F24 File Offset: 0x001B6124
		private void SetDragVector(IntVector3? direction, GameInstance gameInstance)
		{
			bool flag = direction == null;
			if (flag)
			{
				gameInstance.InteractionModule.FluidityActive = false;
			}
			else
			{
				gameInstance.InteractionModule.FluidityActive = true;
			}
			this._dragVector = direction;
		}

		// Token: 0x06005963 RID: 22883 RVA: 0x001B7F68 File Offset: 0x001B6168
		public bool IsFluidityActive()
		{
			return this._dragVector != null;
		}

		// Token: 0x1700136B RID: 4971
		// (get) Token: 0x06005964 RID: 22884 RVA: 0x001B7F85 File Offset: 0x001B6185
		public bool IsFluidityEnabled
		{
			get
			{
				return !this.Interaction.RemoveItemInHand;
			}
		}

		// Token: 0x06005965 RID: 22885 RVA: 0x001B7F95 File Offset: 0x001B6195
		public PlaceBlockInteraction(int id, Interaction interaction) : base(id, interaction)
		{
			this._interactionBlockIdOverride = interaction.BlockId;
		}

		// Token: 0x06005966 RID: 22886 RVA: 0x001B7FB0 File Offset: 0x001B61B0
		protected override void Tick0(GameInstance gameInstance, InteractionModule.ClickType clickType, bool hasAnyButtonClick, bool firstRun, float time, InteractionType type, InteractionContext context)
		{
			bool flag = clickType == InteractionModule.ClickType.None;
			if (flag)
			{
				this.ResetAndLeave(context, 0);
			}
			else
			{
				ItemLibraryModule itemLibraryModule = gameInstance.ItemLibraryModule;
				ClientItemStack heldItem = context.HeldItem;
				ClientItemBase item = itemLibraryModule.GetItem((heldItem != null) ? heldItem.Id : null);
				int? num = (item != null) ? new int?(item.BlockId) : null;
				ClientBlockType clientBlockType = (num != null) ? gameInstance.MapModule.ClientBlockTypes[num.Value] : null;
				bool flag2 = clientBlockType == null;
				if (flag2)
				{
					this.ResetAndLeave(context, 0);
				}
				else
				{
					this.Tick1(gameInstance, clickType, firstRun, time, context, clientBlockType);
				}
			}
		}

		// Token: 0x06005967 RID: 22887 RVA: 0x001B8054 File Offset: 0x001B6254
		private void ResetAndLeave(InteractionContext context, InteractionState state)
		{
			this._prevPlacePosition = null;
			this.SetDragVector(null, context.GameInstance);
			this._lastVariantId = null;
			this._initialVector = null;
			this._lastPlacementTime = 0f;
			context.State.State = state;
		}

		// Token: 0x06005968 RID: 22888 RVA: 0x001B80B4 File Offset: 0x001B62B4
		private void Tick1(GameInstance gameInstance, InteractionModule.ClickType clickType, bool firstRun, float time, InteractionContext context, ClientBlockType heldBlockType)
		{
			bool flag = clickType == InteractionModule.ClickType.Single;
			if (flag)
			{
				bool flag2 = !firstRun;
				if (flag2)
				{
					context.State.State = 3;
					return;
				}
			}
			bool flag3 = clickType == InteractionModule.ClickType.Held && this._dragVector == null;
			if (flag3)
			{
				this.TryLatch(gameInstance, clickType, time);
				bool flag4 = this._dragVector == null;
				if (flag4)
				{
					context.State.State = 4;
					return;
				}
			}
			bool flag5 = !this.UpdateCooldown(gameInstance, clickType, time);
			if (flag5)
			{
				context.State.State = 4;
			}
			else
			{
				this.PlaceNextBlock(gameInstance, clickType, context, heldBlockType);
				bool flag6 = context.State.State == 4;
				if (flag6)
				{
					this._lastPlacementTime = time;
				}
			}
		}

		// Token: 0x06005969 RID: 22889 RVA: 0x001B8180 File Offset: 0x001B6380
		private void TryLatch(GameInstance gameInstance, InteractionModule.ClickType clickType, float time)
		{
			IntVector3 playerMovementDirection = this.GetPlayerMovementDirection(gameInstance);
			bool flag = playerMovementDirection != IntVector3.Zero;
			if (flag)
			{
				this.SetDragVector(new IntVector3?(playerMovementDirection), gameInstance);
			}
			else
			{
				bool flag2 = this._prevPlacePosition == null;
				if (!flag2)
				{
					IntVector3 value = this._prevPlacePosition.Value;
					bool flag3 = time - this._lastPlacementTime > 0.4f;
					bool flag4 = gameInstance.InteractionModule.InteractionTarget == InteractionModule.InteractionTargetType.Block;
					if (flag4)
					{
						Vector3 blockPosition = gameInstance.InteractionModule.TargetBlockHit.BlockPosition;
						Vector3 blockNormal = gameInstance.InteractionModule.TargetBlockHit.BlockNormal;
						bool flag5 = blockPosition == value && (this._initialVector == null || blockNormal == this._initialVector) && !flag3;
						if (flag5)
						{
							return;
						}
						IntVector3 next = new IntVector3((int)Math.Floor((double)(blockPosition.X + blockNormal.X)), (int)Math.Floor((double)(blockPosition.Y + blockNormal.Y)), (int)Math.Floor((double)(blockPosition.Z + blockNormal.Z)));
						IntVector3? snappedPlacementPosition = this.GetSnappedPlacementPosition(value, next);
						bool flag6 = snappedPlacementPosition != null && this.IsValidPlacementPosition(snappedPlacementPosition.Value, clickType);
						if (flag6)
						{
							this.SetDragVector(snappedPlacementPosition - value, gameInstance);
						}
					}
					IntVector3? direction = null;
					float num = float.PositiveInfinity;
					foreach (IntVector3 intVector in PlaceBlockInteraction.CandidateVectors)
					{
						bool flag7 = this._initialVector == intVector && !flag3;
						if (!flag7)
						{
							IntVector3 v = value + intVector;
							BoundingBox forwardBox = new BoundingBox(v, v + Vector3.One);
							HitDetection.RayBoxCollision? rayBoxCollision = PlaceBlockInteraction.CheckForwardLookVector(gameInstance, forwardBox);
							bool flag8 = rayBoxCollision == null;
							if (!flag8)
							{
								float num2 = Vector3.DistanceSquared(rayBoxCollision.Value.Position, gameInstance.CameraModule.Controller.Position);
								bool flag9 = num2 > num;
								if (!flag9)
								{
									num = num2;
									direction = new IntVector3?(intVector);
								}
							}
						}
					}
					bool flag10 = direction != null;
					if (flag10)
					{
						this.SetDragVector(direction, gameInstance);
					}
				}
			}
		}

		// Token: 0x0600596A RID: 22890 RVA: 0x001B8454 File Offset: 0x001B6654
		private IntVector3 GetPlayerMovementDirection(GameInstance gameInstance)
		{
			Vector3 vector = gameInstance.LocalPlayer.Position - gameInstance.LocalPlayer.PreviousPosition;
			Vector3 vector2 = Vector3.Zero;
			float num = 0f;
			foreach (IntVector3 v in PlaceBlockInteraction.CandidateVectors)
			{
				float num2 = Vector3.Dot(vector, v);
				bool flag = num2 <= num;
				if (!flag)
				{
					num = num2;
					vector2 = v;
				}
			}
			return new IntVector3((int)vector2.X, (int)vector2.Y, (int)vector2.Z);
		}

		// Token: 0x0600596B RID: 22891 RVA: 0x001B84FC File Offset: 0x001B66FC
		private void PlaceNextBlock(GameInstance gameInstance, InteractionModule.ClickType clickType, InteractionContext context, ClientBlockType heldBlockType)
		{
			bool flag = this.TryPlaceBlock(gameInstance, clickType, context, heldBlockType);
			bool flag2 = clickType == InteractionModule.ClickType.Held;
			if (flag2)
			{
				context.State.State = 4;
			}
			else
			{
				bool flag3 = clickType == InteractionModule.ClickType.Single;
				if (flag3)
				{
					context.State.State = (flag ? 4 : 3);
				}
			}
		}

		// Token: 0x0600596C RID: 22892 RVA: 0x001B854C File Offset: 0x001B674C
		private bool UpdateCooldown(GameInstance gameInstance, InteractionModule.ClickType clickType, float time)
		{
			bool flag = clickType == InteractionModule.ClickType.Held;
			if (flag)
			{
				float cooldownRate = this.GetCooldownRate(gameInstance);
				bool flag2 = time - this._lastPlacementTime < cooldownRate;
				if (flag2)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600596D RID: 22893 RVA: 0x001B8588 File Offset: 0x001B6788
		private float GetCooldownRate(GameInstance gameInstance)
		{
			float playerSpeed = PlaceBlockInteraction.GetPlayerSpeed(gameInstance);
			float num = 1f - Math.Min(playerSpeed * 3f, 1f);
			return 0.12f * num;
		}

		// Token: 0x0600596E RID: 22894 RVA: 0x001B85C0 File Offset: 0x001B67C0
		private static float GetPlayerSpeed(GameInstance gameInstance)
		{
			return (gameInstance.LocalPlayer.Position - gameInstance.LocalPlayer.PreviousPosition).Length();
		}

		// Token: 0x0600596F RID: 22895 RVA: 0x001B85F8 File Offset: 0x001B67F8
		private bool TryPlaceBlock(GameInstance gameInstance, InteractionModule.ClickType clickType, InteractionContext context, ClientBlockType heldBlockType)
		{
			bool flag = clickType != InteractionModule.ClickType.Held;
			IntVector3 intVector;
			bool flag2;
			if (flag)
			{
				bool isVisible = gameInstance.InteractionModule.BlockPreview.IsVisible;
				if (isVisible)
				{
					intVector = gameInstance.InteractionModule.BlockPreview.BlockPosition;
					flag2 = ((gameInstance.InteractionModule.BlockPreview.IsVisible && gameInstance.InteractionModule.BlockPreview.HasValidPosition) || (!gameInstance.InteractionModule.BlockPreview.IsVisible && gameInstance.InteractionModule.HeldBlockCanBePlaced));
				}
				else
				{
					int x;
					int y;
					int z;
					ValueTuple<bool, IntVector3?> valueTuple = PlaceBlockInteraction.TryGetPlacementPosition(gameInstance, heldBlockType, out x, out y, out z);
					bool flag3 = !valueTuple.Item1;
					if (flag3)
					{
						return false;
					}
					intVector = new IntVector3(x, y, z);
					flag2 = true;
				}
			}
			else
			{
				IntVector3? nextPlacementPosition = this.GetNextPlacementPosition(gameInstance, heldBlockType, clickType);
				bool flag4 = nextPlacementPosition == null;
				if (flag4)
				{
					return false;
				}
				intVector = nextPlacementPosition.Value;
				flag2 = gameInstance.InteractionModule.HeldBlockCanBePlaced;
			}
			int x2 = intVector.X;
			int y2 = intVector.Y;
			int z2 = intVector.Z;
			int block = gameInstance.MapModule.GetBlock(x2, y2, z2, 1);
			bool flag5 = block == 1;
			bool result;
			if (flag5)
			{
				result = false;
			}
			else
			{
				ClientBlockType targetBlockType = gameInstance.MapModule.ClientBlockTypes[block];
				int num = this._interactionBlockIdOverride;
				bool flag6 = num == -1;
				if (flag6)
				{
					num = (this._lastVariantId ?? this._GetPlacedBlockVariant(gameInstance, heldBlockType, targetBlockType, x2, y2, z2));
				}
				ClientBlockType clientBlockType = gameInstance.MapModule.ClientBlockTypes[num];
				bool flag7 = !flag2;
				if (flag7)
				{
					context.State.State = 3;
					Vector3 worldPosition = new Vector3((float)x2 + 0.5f, (float)y2 + 0.5f, (float)z2 + 0.5f);
					gameInstance.AudioModule.TryPlayBlockSoundEvent(heldBlockType.BlockSoundSetIndex, 4, worldPosition, Vector3.Zero);
					result = false;
				}
				else
				{
					context.InstanceStore.OldBlockId = block;
					Vector3 worldPosition2 = new Vector3((float)x2 + 0.5f, (float)y2 + 0.5f, (float)z2 + 0.5f);
					gameInstance.AudioModule.TryPlayBlockSoundEvent(heldBlockType.BlockSoundSetIndex, 5, worldPosition2, Vector3.Zero);
					gameInstance.MapModule.SetClientBlock(x2, y2, z2, num);
					ClientItemStack heldItem = context.HeldItem;
					bool flag8 = this.Interaction.RemoveItemInHand && gameInstance.GameMode == null && heldItem != null && heldItem.Quantity == 1;
					if (flag8)
					{
						context.HeldItem = null;
					}
					context.State.BlockPosition_ = new BlockPosition(x2, y2, z2);
					context.State.BlockRotation_ = new BlockRotation(clientBlockType.RotationYaw, clientBlockType.RotationPitch, clientBlockType.RotationRoll);
					context.State.PlacedBlockId = num;
					bool flag9 = this._dragVector != null;
					if (flag9)
					{
						gameInstance.Connection.SendPacket(new PrototypeClientPlaceBlock(context.State.BlockPosition_, context.State.BlockRotation_));
					}
					this._lastVariantId = new int?(num);
					this._prevPlacePosition = new IntVector3?(intVector);
					Ray lookRay = gameInstance.CameraModule.GetLookRay();
					HitDetection.RayBoxCollision rayBoxCollision;
					bool flag10 = HitDetection.CheckRayBoxCollision(new BoundingBox(intVector, intVector + Vector3.One), lookRay.Position, lookRay.Direction, out rayBoxCollision, false);
					if (flag10)
					{
						this._initialVector = new Vector3?(rayBoxCollision.Normal);
					}
					else
					{
						this._initialVector = null;
					}
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06005970 RID: 22896 RVA: 0x001B8998 File Offset: 0x001B6B98
		private bool IsValidPlacementPosition(IntVector3 placementPosition, InteractionModule.ClickType clickType)
		{
			IntVector3? prevPlacePosition = this._prevPlacePosition;
			IntVector3? dragVector = this._dragVector;
			bool flag = clickType != InteractionModule.ClickType.Held;
			bool result;
			if (flag)
			{
				result = true;
			}
			else
			{
				bool flag2 = prevPlacePosition == null;
				if (flag2)
				{
					result = false;
				}
				else
				{
					bool flag3 = dragVector == null;
					if (flag3)
					{
						result = Enumerable.Contains<IntVector3>(PlaceBlockInteraction.CandidateVectors, placementPosition - prevPlacePosition.Value);
					}
					else
					{
						IntVector3 value = prevPlacePosition.Value + dragVector.Value;
						result = (value == placementPosition);
					}
				}
			}
			return result;
		}

		// Token: 0x06005971 RID: 22897 RVA: 0x001B8A28 File Offset: 0x001B6C28
		private IntVector3? GetNextPlacementPosition(GameInstance gameInstance, ClientBlockType heldBlockType, InteractionModule.ClickType clickType)
		{
			int x;
			int y;
			int z;
			bool item = PlaceBlockInteraction.TryGetPlacementPosition(gameInstance, heldBlockType, out x, out y, out z).Item1;
			if (item)
			{
				IntVector3 value = new IntVector3(x, y, z);
				bool flag = clickType == InteractionModule.ClickType.Held && this._prevPlacePosition != null;
				if (flag)
				{
					IntVector3? snappedPlacementPosition = this.GetSnappedPlacementPosition(this._prevPlacePosition.Value, value);
					bool flag2 = snappedPlacementPosition != null;
					if (flag2)
					{
						value = snappedPlacementPosition.Value;
					}
				}
				bool flag3 = this.IsValidPlacementPosition(value, clickType);
				if (flag3)
				{
					return new IntVector3?(value);
				}
			}
			bool flag4 = this._prevPlacePosition != null;
			IntVector3? result;
			if (flag4)
			{
				IntVector3 value2 = this._prevPlacePosition.Value;
				IntVector3 v = value2;
				Vector3 end = value2 + Vector3.One;
				IntVector3? intVector = null;
				float num = float.PositiveInfinity;
				foreach (IntVector3 intVector2 in PlaceBlockInteraction.CandidateVectors)
				{
					IntVector3 intVector3 = value2 + intVector2;
					bool flag5 = !this.IsValidPlacementPosition(intVector3, clickType);
					if (!flag5)
					{
						HitDetection.RayBoxCollision? rayBoxCollision = PlaceBlockInteraction.CheckForwardLookVector(gameInstance, PlaceBlockInteraction.CreateForwardLookBox(gameInstance, v, end, intVector2));
						bool flag6 = rayBoxCollision == null;
						if (!flag6)
						{
							Vector3 position = rayBoxCollision.Value.Position;
							bool flag7 = (float)intVector2.X != 0f;
							if (flag7)
							{
								bool flag8 = (float)intVector2.X > 0f && position.X < (float)intVector3.X - 1f;
								if (flag8)
								{
									goto IL_320;
								}
								bool flag9 = (float)intVector2.X < 0f && position.X > (float)intVector3.X + 1f;
								if (flag9)
								{
									goto IL_320;
								}
							}
							bool flag10 = (float)intVector2.Y != 0f;
							if (flag10)
							{
								bool flag11 = (float)intVector2.Y > 0f && position.Y < (float)intVector3.Y - 1f;
								if (flag11)
								{
									goto IL_320;
								}
								bool flag12 = (float)intVector2.Y < 0f && position.Y > (float)intVector3.Y + 1f;
								if (flag12)
								{
									goto IL_320;
								}
							}
							bool flag13 = (float)intVector2.Z != 0f;
							if (flag13)
							{
								bool flag14 = (float)intVector2.Z > 0f && position.Z < (float)intVector3.Z - 1f;
								if (flag14)
								{
									goto IL_320;
								}
								bool flag15 = (float)intVector2.Z < 0f && position.Z > (float)intVector3.Z + 1f;
								if (flag15)
								{
									goto IL_320;
								}
							}
							float num2 = Vector3.DistanceSquared(intVector3 + new Vector3(0.5f, 0.5f, 0.5f), gameInstance.LocalPlayer.Position);
							bool flag16 = num2 < num;
							if (flag16)
							{
								intVector = new IntVector3?(intVector3);
								num = num2;
							}
						}
					}
					IL_320:;
				}
				result = intVector;
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06005972 RID: 22898 RVA: 0x001B8D7C File Offset: 0x001B6F7C
		private IntVector3? GetSnappedPlacementPosition(IntVector3 prev, IntVector3 next)
		{
			bool flag = prev.X == next.X && prev.Y == next.Y;
			IntVector3? result;
			if (flag)
			{
				result = new IntVector3?(new IntVector3(next.X, next.Y, prev.Z + Math.Sign(next.Z - prev.Z)));
			}
			else
			{
				bool flag2 = prev.Y == next.Y && prev.Z == next.Z;
				if (flag2)
				{
					result = new IntVector3?(new IntVector3(prev.X + Math.Sign(next.X - prev.X), next.Y, next.Z));
				}
				else
				{
					bool flag3 = prev.Z == next.Z && prev.X == next.X;
					if (flag3)
					{
						result = new IntVector3?(new IntVector3(next.X, prev.Y + Math.Sign(next.Y - prev.Y), next.Z));
					}
					else
					{
						result = null;
					}
				}
			}
			return result;
		}

		// Token: 0x06005973 RID: 22899 RVA: 0x001B8EA0 File Offset: 0x001B70A0
		private static BoundingBox CreateForwardLookBox(GameInstance gameInstance, Vector3 start, Vector3 end, IntVector3 direction)
		{
			float num = Math.Min(3f, PlaceBlockInteraction.GetPlayerSpeed(gameInstance) * 3f);
			float num2 = 24f;
			bool flag = (float)direction.X != 0f;
			if (flag)
			{
				bool flag2 = (float)direction.X < 0f;
				if (flag2)
				{
					start.X -= num2;
				}
				else
				{
					bool flag3 = (float)direction.X > 0f;
					if (flag3)
					{
						end.X += num2;
					}
				}
				start.Y -= num;
				end.Y += num;
				start.Z -= num;
				end.Z += num;
			}
			bool flag4 = (float)direction.Y != 0f;
			if (flag4)
			{
				bool flag5 = (float)direction.Y < 0f;
				if (flag5)
				{
					start.Y -= num2;
				}
				else
				{
					bool flag6 = (float)direction.Y > 0f;
					if (flag6)
					{
						end.Y += num2;
					}
				}
				start.X -= num;
				end.X += num;
				start.Z -= num;
				end.Z += num;
			}
			bool flag7 = (float)direction.Z != 0f;
			if (flag7)
			{
				bool flag8 = (float)direction.Z < 0f;
				if (flag8)
				{
					start.Z -= num2;
				}
				else
				{
					bool flag9 = (float)direction.Z > 0f;
					if (flag9)
					{
						end.Z += num2;
					}
				}
				start.X -= num;
				end.Y += num;
				start.Z -= num;
				end.Y += num;
			}
			return new BoundingBox(start, end);
		}

		// Token: 0x06005974 RID: 22900 RVA: 0x001B9084 File Offset: 0x001B7284
		private static HitDetection.RayBoxCollision? CheckForwardLookVector(GameInstance gameInstance, BoundingBox forwardBox)
		{
			Ray lookRay = gameInstance.CameraModule.GetLookRay();
			HitDetection.RayBoxCollision value;
			bool flag = !HitDetection.CheckRayBoxCollision(forwardBox, lookRay.Position, lookRay.Direction, out value, false);
			HitDetection.RayBoxCollision? result;
			if (flag)
			{
				result = null;
			}
			else
			{
				float num = 8f;
				bool flag2 = Vector3.Distance(lookRay.Position, value.Position) >= num;
				if (flag2)
				{
					result = null;
				}
				else
				{
					HitDetection.RaycastOptions options = new HitDetection.RaycastOptions
					{
						Distance = num
					};
					bool flag3;
					HitDetection.RaycastHit raycastHit;
					bool flag4;
					HitDetection.EntityHitData entityHitData;
					gameInstance.HitDetection.Raycast(lookRay.Position, lookRay.Direction, options, out flag3, out raycastHit, out flag4, out entityHitData);
					bool flag5 = flag3;
					if (flag5)
					{
						result = null;
					}
					else
					{
						result = new HitDetection.RayBoxCollision?(value);
					}
				}
			}
			return result;
		}

		// Token: 0x06005975 RID: 22901 RVA: 0x001B9154 File Offset: 0x001B7354
		[return: TupleElementNames(new string[]
		{
			"Valid",
			"ConflictingBlock"
		})]
		public static ValueTuple<bool, IntVector3?> TryGetPlacementPosition(GameInstance gameInstance, ClientBlockType heldBlockType, out int blockX, out int blockY, out int blockZ)
		{
			HitDetection.RaycastHit targetBlockHit = gameInstance.InteractionModule.TargetBlockHit;
			ClientBlockType clientBlockType = gameInstance.MapModule.ClientBlockTypes[targetBlockHit.BlockId];
			blockX = (int)Math.Floor((double)targetBlockHit.BlockPositionNoFiller.X);
			blockY = (int)Math.Floor((double)targetBlockHit.BlockPositionNoFiller.Y);
			blockZ = (int)Math.Floor((double)targetBlockHit.BlockPositionNoFiller.Z);
			bool flag = clientBlockType.FillerX == 0 && clientBlockType.FillerY == 0 && clientBlockType.FillerZ == 0 && clientBlockType.CollisionMaterial == null && heldBlockType.CollisionMaterial == 1;
			ValueTuple<bool, IntVector3?> result;
			if (flag)
			{
				result = new ValueTuple<bool, IntVector3?>(true, null);
			}
			else
			{
				blockX += (int)Math.Floor((double)targetBlockHit.BlockNormal.X);
				blockY += (int)Math.Floor((double)targetBlockHit.BlockNormal.Y);
				blockZ += (int)Math.Floor((double)targetBlockHit.BlockNormal.Z);
				int block = gameInstance.MapModule.GetBlock(blockX, blockY, blockZ, int.MaxValue);
				bool flag2 = block == int.MaxValue;
				if (flag2)
				{
					result = new ValueTuple<bool, IntVector3?>(false, null);
				}
				else
				{
					clientBlockType = gameInstance.MapModule.ClientBlockTypes[block];
					int num = blockX;
					int num2 = blockY;
					int num3 = blockZ;
					bool flag3 = clientBlockType.FillerX != 0 || clientBlockType.FillerY != 0 || clientBlockType.FillerZ != 0;
					if (flag3)
					{
						num -= clientBlockType.FillerX;
						num2 -= clientBlockType.FillerY;
						num3 -= clientBlockType.FillerZ;
						block = gameInstance.MapModule.GetBlock(num, num2, num3, 1);
						clientBlockType = gameInstance.MapModule.ClientBlockTypes[block];
					}
					bool flag4 = block == 0;
					if (flag4)
					{
						result = new ValueTuple<bool, IntVector3?>(true, null);
					}
					else
					{
						bool flag5 = clientBlockType.CollisionMaterial == 1;
						if (flag5)
						{
							result = new ValueTuple<bool, IntVector3?>(false, new IntVector3?(new IntVector3(num, num2, num3)));
						}
						else
						{
							bool flag6 = clientBlockType.CollisionMaterial == 2;
							if (flag6)
							{
								result = new ValueTuple<bool, IntVector3?>(true, null);
							}
							else
							{
								bool flag7 = heldBlockType.CollisionMaterial == 1;
								if (flag7)
								{
									result = new ValueTuple<bool, IntVector3?>(true, null);
								}
								else
								{
									result = new ValueTuple<bool, IntVector3?>(false, new IntVector3?(new IntVector3(num, num2, num3)));
								}
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06005976 RID: 22902 RVA: 0x001B93BC File Offset: 0x001B75BC
		protected virtual int _GetPlacedBlockVariant(GameInstance gameInstance, ClientBlockType heldBlockType, ClientBlockType targetBlockType, int targetBlockX, int targetBlockY, int targetBlockZ)
		{
			bool flag = gameInstance.InteractionModule.RotatedBlockIdOverride != -1;
			int result;
			if (flag)
			{
				result = gameInstance.InteractionModule.RotatedBlockIdOverride;
			}
			else
			{
				result = PlaceBlockInteraction.GetPlacedBlockVariant(gameInstance, heldBlockType, targetBlockType, targetBlockX, targetBlockY, targetBlockZ);
			}
			return result;
		}

		// Token: 0x06005977 RID: 22903 RVA: 0x001B9400 File Offset: 0x001B7600
		private static bool FindBestConnectionType(GameInstance gameInstance, int blockX, int blockY, int blockZ, BlockType.BlockConnections connections, Vector3 horizontalNormal, out BlockConnectionType? connectionType, out Rotation connectionRotation)
		{
			int num;
			bool flag = connections.Outputs.TryGetValue(5, out num);
			if (flag)
			{
				bool flag2 = PlaceBlockInteraction.IsMatchingBlockType(gameInstance, blockX, blockY, blockZ, 0, -1, 0, 0, connections.ConnectableBlocks) && PlaceBlockInteraction.IsMatchingBlockType(gameInstance, blockX, blockY, blockZ, 0, 1, 0, 0, connections.ConnectableBlocks) && PlaceBlockInteraction.IsMatchingBlockType(gameInstance, blockX, blockY, blockZ, 0, 0, 0, -1, connections.ConnectableBlocks) && PlaceBlockInteraction.IsMatchingBlockType(gameInstance, blockX, blockY, blockZ, 0, 0, 0, 1, connections.ConnectableBlocks);
				if (flag2)
				{
					connectionType = new BlockConnectionType?(5);
					connectionRotation = 0;
					return true;
				}
			}
			bool flag3 = connections.Outputs.TryGetValue(3, out num);
			if (flag3)
			{
				foreach (Rotation rotation in PlaceBlockInteraction._connectionTestRotations)
				{
					bool flag4 = PlaceBlockInteraction.IsMatchingBlockType(gameInstance, blockX, blockY, blockZ, rotation, 1, 0, 0, connections.ConnectableBlocks) && PlaceBlockInteraction.IsMatchingBlockType(gameInstance, blockX, blockY, blockZ, rotation, -1, 0, 0, connections.ConnectableBlocks) && PlaceBlockInteraction.IsMatchingBlockType(gameInstance, blockX, blockY, blockZ, rotation, 0, 0, 1, connections.ConnectableBlocks);
					if (flag4)
					{
						connectionType = new BlockConnectionType?(3);
						connectionRotation = rotation;
						return true;
					}
				}
			}
			int num2;
			bool flag5 = connections.Outputs.TryGetValue(0, out num2);
			if (flag5)
			{
				int block = gameInstance.MapModule.GetBlock(blockX, blockY, blockZ, int.MaxValue);
				bool flag6 = block != int.MaxValue;
				if (flag6)
				{
					ClientBlockType clientBlockType = gameInstance.MapModule.ClientBlockTypes[block];
					bool flag7 = clientBlockType.TryGetRotatedVariant(0, clientBlockType.RotationPitch, 0) == num2;
					if (flag7)
					{
						Rotation yawRotation = RotationHelper.Subtract(clientBlockType.RotationYaw, 1);
						bool flag8 = PlaceBlockInteraction.IsMatchingBlockType(gameInstance, blockX, blockY, blockZ, yawRotation, 1, 0, 0, connections.ConnectableBlocks) && PlaceBlockInteraction.IsMatchingBlockType(gameInstance, blockX, blockY, blockZ, yawRotation, 0, 0, 1, connections.ConnectableBlocks);
						if (flag8)
						{
							connectionType = new BlockConnectionType?(0);
							connectionRotation = clientBlockType.RotationYaw;
							return true;
						}
					}
				}
				foreach (Rotation rotation2 in PlaceBlockInteraction._connectionTestRotations)
				{
					bool flag9 = PlaceBlockInteraction.IsMatchingBlockType(gameInstance, blockX, blockY, blockZ, rotation2, 1, 0, 0, connections.ConnectableBlocks) && PlaceBlockInteraction.IsMatchingBlockType(gameInstance, blockX, blockY, blockZ, rotation2, 0, 0, 1, connections.ConnectableBlocks);
					if (flag9)
					{
						connectionType = new BlockConnectionType?(0);
						connectionRotation = RotationHelper.Add(rotation2, 1);
						return true;
					}
				}
			}
			connectionType = null;
			connectionRotation = 0;
			return false;
		}

		// Token: 0x06005978 RID: 22904 RVA: 0x001B96A0 File Offset: 0x001B78A0
		private static bool IsMatchingBlockType(GameInstance gameInstance, int blockX, int blockY, int blockZ, Rotation yawRotation, int relX, int relY, int relZ, int[] allowedBlockIds)
		{
			RotationHelper.Rotate(relX, relZ, yawRotation, out relX, out relZ);
			int block = gameInstance.MapModule.GetBlock(blockX + relX, blockY + relY, blockZ + relZ, int.MaxValue);
			bool flag = block == int.MaxValue;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = Array.IndexOf<int>(allowedBlockIds, block) >= 0;
				bool flag3 = !flag2;
				if (flag3)
				{
					ClientBlockType clientBlockType = gameInstance.MapModule.ClientBlockTypes[block];
					bool flag4 = clientBlockType.VariantOriginalId != block;
					if (flag4)
					{
						flag2 = (Array.IndexOf<int>(allowedBlockIds, clientBlockType.VariantOriginalId) >= 0);
					}
				}
				result = flag2;
			}
			return result;
		}

		// Token: 0x06005979 RID: 22905 RVA: 0x001B9744 File Offset: 0x001B7944
		public static int GetPlacedBlockVariant(GameInstance gameInstance, ClientBlockType heldBlockType, ClientBlockType targetBlockType, int targetBlockX, int targetBlockY, int targetBlockZ)
		{
			HitDetection.RaycastHit targetBlockHit = gameInstance.InteractionModule.TargetBlockHit;
			Vector3 blockNormal = targetBlockHit.BlockNormal;
			bool rotateX;
			bool rotateY;
			Rotation defaultPitch;
			Rotation[] array;
			RotationHelper.GetVariantRotationOptions(blockNormal, heldBlockType.VariantRotation, out rotateX, out rotateY, out defaultPitch, out array);
			int num = heldBlockType.Id;
			bool flag = blockNormal.Y != 0f;
			if (flag)
			{
				RotationHelper.GetHorizontalNormal(gameInstance.CameraModule.Controller.Rotation, out blockNormal.X, out blockNormal.Z);
			}
			bool flag2 = heldBlockType.IsConnectable();
			if (flag2)
			{
				BlockConnectionType? blockConnectionType;
				Rotation rotation;
				bool flag3 = PlaceBlockInteraction.FindBestConnectionType(gameInstance, targetBlockX, targetBlockY, targetBlockZ, heldBlockType.Connections, blockNormal, out blockConnectionType, out rotation) && blockConnectionType != null;
				if (flag3)
				{
					int num2 = heldBlockType.Connections.Outputs[blockConnectionType.Value];
					rotation = array[rotation];
					return gameInstance.MapModule.ClientBlockTypes[num2].TryGetRotatedVariant(rotation, 0, 0);
				}
			}
			Rotation rotation2;
			Rotation pitch;
			PlaceBlockInteraction.GetVariantByNormal(blockNormal, rotateX, rotateY, defaultPitch, out rotation2, out pitch);
			bool useBlockSubfaces = gameInstance.App.Settings.UseBlockSubfaces;
			if (useBlockSubfaces)
			{
				PlaceBlockInteraction.SubfacePlacement(targetBlockHit, ref rotation2, ref pitch, heldBlockType, gameInstance);
			}
			Rotation rotationYawPlacementOffset = heldBlockType.RotationYawPlacementOffset;
			bool flag4 = true;
			if (flag4)
			{
				rotation2 = RotationHelper.Add(rotation2, heldBlockType.RotationYawPlacementOffset);
			}
			rotation2 = array[rotation2];
			num = heldBlockType.TryGetRotatedVariant(rotation2, pitch, 0);
			bool flag5 = targetBlockType.CollisionMaterial == 2;
			if (flag5)
			{
				ClientBlockType clientBlockType = gameInstance.MapModule.ClientBlockTypes[num];
				int num3;
				bool flag6 = clientBlockType.Variants.TryGetValue("Fluid=" + targetBlockType.Name, out num3);
				if (flag6)
				{
					num = num3;
				}
			}
			return num;
		}

		// Token: 0x0600597A RID: 22906 RVA: 0x001B98EC File Offset: 0x001B7AEC
		private static void GetVariantByNormal(Vector3 normal, bool rotateX, bool rotateY, Rotation defaultPitch, out Rotation yaw, out Rotation pitch)
		{
			yaw = 0;
			pitch = defaultPitch;
			bool flag = rotateY && normal.Y == -1f;
			if (flag)
			{
				pitch = 2;
				normal.X *= -1f;
				normal.Z *= -1f;
			}
			if (rotateX)
			{
				bool flag2 = normal.X == -1f;
				if (flag2)
				{
					yaw = 3;
				}
				else
				{
					bool flag3 = normal.X == 1f;
					if (flag3)
					{
						yaw = 1;
					}
					else
					{
						bool flag4 = normal.Z == -1f;
						if (flag4)
						{
							yaw = 2;
						}
						else
						{
							bool flag5 = normal.Z == 1f;
							if (flag5)
							{
								yaw = 0;
							}
						}
					}
				}
			}
		}

		// Token: 0x0600597B RID: 22907 RVA: 0x001B99A4 File Offset: 0x001B7BA4
		private static void SubfacePlacement(HitDetection.RaycastHit targetBlockHit, ref Rotation yaw, ref Rotation pitch, ClientBlockType blockType, GameInstance gameInstance)
		{
			BlockType.VariantRotation variantRotation = blockType.VariantRotation;
			BlockHitbox blockHitbox = gameInstance.ServerSettings.BlockHitboxes[blockType.HitboxType].Clone();
			bool flag = blockHitbox.BoundingBox.Max.X - blockHitbox.BoundingBox.Min.X > 1f || blockHitbox.BoundingBox.Max.Y - blockHitbox.BoundingBox.Min.Y > 1f || blockHitbox.BoundingBox.Max.Z - blockHitbox.BoundingBox.Min.Z > 1f;
			if (!flag)
			{
				bool flag2 = variantRotation != 3 && variantRotation != 4 && variantRotation != 6;
				if (!flag2)
				{
					Vector3 vector = targetBlockHit.HitPosition - targetBlockHit.BlockOrigin;
					float num = Math.Abs(vector.X - 0.5f);
					float num2 = Math.Abs(vector.Y - 0.5f);
					float num3 = Math.Abs(vector.Z - 0.5f);
					bool flag3 = targetBlockHit.BlockNormal.X != 0f;
					if (flag3)
					{
						num = 0f;
					}
					bool flag4 = targetBlockHit.BlockNormal.Y != 0f;
					if (flag4)
					{
						num2 = 0f;
					}
					bool flag5 = targetBlockHit.BlockNormal.Z != 0f;
					if (flag5)
					{
						num3 = 0f;
					}
					float num4 = 0.35f;
					float num5 = 1f - num4;
					bool flag6 = variantRotation == 6;
					if (flag6)
					{
						bool flag7 = targetBlockHit.BlockNormal.Y == 0f && vector.Y > num5;
						if (flag7)
						{
							yaw = RotationHelper.Subtract(yaw, 2);
							pitch = 2;
						}
					}
					else
					{
						bool flag8 = variantRotation == 3 || variantRotation == 4;
						if (flag8)
						{
							bool flag9 = num2 > num3 && num2 > num;
							if (flag9)
							{
								bool flag10 = vector.Y > num5;
								if (flag10)
								{
									yaw = 0;
									pitch = 2;
								}
								else
								{
									bool flag11 = vector.Y < num4;
									if (flag11)
									{
										yaw = 0;
										pitch = 0;
									}
								}
							}
							bool flag12 = num3 > num2 && num3 > num;
							if (flag12)
							{
								bool flag13 = vector.Z > num5;
								if (flag13)
								{
									pitch = 1;
									yaw = 2;
								}
								else
								{
									bool flag14 = vector.Z < num4;
									if (flag14)
									{
										pitch = 1;
										yaw = 0;
									}
								}
							}
							bool flag15 = num > num2 && num > num3;
							if (flag15)
							{
								bool flag16 = vector.X > num5;
								if (flag16)
								{
									pitch = 1;
									yaw = 3;
								}
								else
								{
									bool flag17 = vector.X < num4;
									if (flag17)
									{
										pitch = 1;
										yaw = 1;
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x0600597C RID: 22908 RVA: 0x001B9C70 File Offset: 0x001B7E70
		public static bool CanPlaceBlock(GameInstance gameInstance, ClientBlockType heldBlockType, int worldX, int worldY, int worldZ, out BoundingBox? collisionArea)
		{
			collisionArea = null;
			ClientBlockType clientBlockType = gameInstance.MapModule.ClientBlockTypes[heldBlockType.Id];
			BlockHitbox blockHitbox = gameInstance.ServerSettings.BlockHitboxes[clientBlockType.HitboxType].Clone();
			BoundingBox voxelBounds = blockHitbox.GetVoxelBounds();
			int num = (int)voxelBounds.Min.X;
			while ((float)num < voxelBounds.Max.X)
			{
				int num2 = (int)voxelBounds.Min.Y;
				while ((float)num2 < voxelBounds.Max.Y)
				{
					int num3 = (int)voxelBounds.Min.Z;
					while ((float)num3 < voxelBounds.Max.Z)
					{
						int num4 = worldX + num;
						int num5 = worldY + num2;
						int num6 = worldZ + num3;
						int block = gameInstance.MapModule.GetBlock(num4, num5, num6, int.MaxValue);
						bool flag = block == int.MaxValue;
						if (!flag)
						{
							ClientBlockType clientBlockType2 = gameInstance.MapModule.ClientBlockTypes[block];
							bool flag2 = clientBlockType2.FillerX != 0 || clientBlockType2.FillerY != 0 || clientBlockType2.FillerZ != 0;
							if (flag2)
							{
								int worldX2 = num4 - clientBlockType2.FillerX;
								int worldY2 = num5 - clientBlockType2.FillerY;
								int worldZ2 = num6 - clientBlockType2.FillerZ;
								block = gameInstance.MapModule.GetBlock(worldX2, worldY2, worldZ2, 1);
								clientBlockType2 = gameInstance.MapModule.ClientBlockTypes[block];
							}
							bool flag3 = clientBlockType2.CollisionMaterial != 1;
							if (!flag3)
							{
								Vector3 vector = new Vector3((float)num4, (float)num5, (float)num6);
								collisionArea = new BoundingBox?(new BoundingBox(vector, vector + Vector3.One));
								return false;
							}
						}
						num3++;
					}
					num2++;
				}
				num++;
			}
			return true;
		}

		// Token: 0x0600597D RID: 22909 RVA: 0x001B9E5C File Offset: 0x001B805C
		public static bool IsEntityBlockingPlacement(GameInstance gameInstance, ClientBlockType heldBlockType, int targetBlockX, int targetBlockY, int targetBlockZ, out BoundingBox? collisionArea)
		{
			collisionArea = null;
			bool flag = heldBlockType.CollisionMaterial != 1;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				BlockHitbox blockHitbox = gameInstance.ServerSettings.BlockHitboxes[heldBlockType.HitboxType];
				Entity[] allEntities = gameInstance.EntityStoreModule.GetAllEntities();
				int entitiesCount = gameInstance.EntityStoreModule.GetEntitiesCount();
				float offsetX = (float)(targetBlockX - heldBlockType.FillerX);
				float offsetY = (float)(targetBlockY - heldBlockType.FillerY);
				float offsetZ = (float)(targetBlockZ - heldBlockType.FillerZ);
				for (int i = 0; i < entitiesCount; i++)
				{
					Entity entity = allEntities[i];
					bool flag2 = !entity.IsTangible();
					if (!flag2)
					{
						BoundingBox hitbox = entity.Hitbox;
						hitbox.Translate(entity.Position);
						foreach (BoundingBox box in blockHitbox.Boxes)
						{
							bool flag3 = hitbox.IntersectsExclusive(box, offsetX, offsetY, offsetZ) || hitbox.IntersectsExclusive(box, (float)targetBlockX, (float)targetBlockY, (float)targetBlockZ);
							if (flag3)
							{
								collisionArea = new BoundingBox?(hitbox);
								return true;
							}
						}
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x0600597E RID: 22910 RVA: 0x001B9F94 File Offset: 0x001B8194
		protected override void Revert0(GameInstance gameInstance, InteractionType type, InteractionContext context)
		{
			base.Revert0(gameInstance, type, context);
			InteractionSyncData state = context.State;
			int oldBlockId = context.InstanceStore.OldBlockId;
			bool flag = ((state != null) ? state.BlockPosition_ : null) == null || oldBlockId == int.MaxValue;
			if (!flag)
			{
				bool flag2 = this._dragVector != null;
				if (!flag2)
				{
					gameInstance.MapModule.SetClientBlock(state.BlockPosition_.X, state.BlockPosition_.Y, state.BlockPosition_.Z, oldBlockId);
				}
			}
		}

		// Token: 0x0600597F RID: 22911 RVA: 0x001BA020 File Offset: 0x001B8220
		// Note: this type is marked as 'beforefieldinit'.
		static PlaceBlockInteraction()
		{
			Rotation[] array = new Rotation[4];
			RuntimeHelpers.InitializeArray(array, fieldof(<PrivateImplementationDetails>.BAED642339816AFFB3FE8719792D0E4CE82F12DB72B7373D244EAA65445800FE).FieldHandle);
			PlaceBlockInteraction._connectionTestRotations = array;
			PlaceBlockInteraction.CandidateVectors = new IntVector3[]
			{
				IntVector3.Up,
				IntVector3.Down,
				IntVector3.Left,
				IntVector3.Right,
				IntVector3.Forward,
				IntVector3.Backward
			};
		}

		// Token: 0x0400376C RID: 14188
		private static readonly Rotation[] _connectionTestRotations;

		// Token: 0x0400376D RID: 14189
		private const float ForwardLookAllowance = 1f;

		// Token: 0x0400376E RID: 14190
		private static readonly IntVector3[] CandidateVectors;

		// Token: 0x0400376F RID: 14191
		private IntVector3? _prevPlacePosition;

		// Token: 0x04003770 RID: 14192
		private IntVector3? _dragVector;

		// Token: 0x04003771 RID: 14193
		private Vector3? _initialVector;

		// Token: 0x04003772 RID: 14194
		private int? _lastVariantId;

		// Token: 0x04003773 RID: 14195
		private readonly int _interactionBlockIdOverride;

		// Token: 0x04003774 RID: 14196
		private float _lastPlacementTime;
	}
}
