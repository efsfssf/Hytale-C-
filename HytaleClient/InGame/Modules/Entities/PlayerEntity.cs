using System;
using System.Collections.Generic;
using System.Diagnostics;
using HytaleClient.Data.BlockyModels;
using HytaleClient.Data.Characters;
using HytaleClient.Data.ClientInteraction;
using HytaleClient.Data.Entities;
using HytaleClient.Data.Items;
using HytaleClient.Data.UserSettings;
using HytaleClient.Graphics;
using HytaleClient.Graphics.BlockyModels;
using HytaleClient.Graphics.Particles;
using HytaleClient.Graphics.Programs;
using HytaleClient.Graphics.Trails;
using HytaleClient.Math;
using HytaleClient.Protocol;

namespace HytaleClient.InGame.Modules.Entities
{
	// Token: 0x02000948 RID: 2376
	internal class PlayerEntity : Entity
	{
		// Token: 0x170011E9 RID: 4585
		// (get) Token: 0x06004A1C RID: 18972 RVA: 0x0012B082 File Offset: 0x00129282
		// (set) Token: 0x06004A1D RID: 18973 RVA: 0x0012B08A File Offset: 0x0012928A
		public bool IsMounting { get; set; }

		// Token: 0x170011EA RID: 4586
		// (get) Token: 0x06004A1E RID: 18974 RVA: 0x0012B093 File Offset: 0x00129293
		public bool NeedsDistortionDraw
		{
			get
			{
				return this._needsDistortionEffect && this.FirstPersonViewNeedsDrawing();
			}
		}

		// Token: 0x06004A1F RID: 18975 RVA: 0x0012B0A8 File Offset: 0x001292A8
		public PlayerEntity(GameInstance gameInstance, int networkId) : base(gameInstance, networkId)
		{
		}

		// Token: 0x06004A20 RID: 18976 RVA: 0x0012B178 File Offset: 0x00129378
		protected override void DoDispose()
		{
			base.DoDispose();
			this.ClearFirstPersonView();
		}

		// Token: 0x06004A21 RID: 18977 RVA: 0x0012B18C File Offset: 0x0012938C
		public void ClearFirstPersonView()
		{
			bool flag = this._firstPersonModelRenderer != null;
			if (flag)
			{
				this._firstPersonModelRenderer.Dispose();
				this._firstPersonModelRenderer = null;
			}
			this.ClearFirstPersonItems();
		}

		// Token: 0x06004A22 RID: 18978 RVA: 0x0012B1C3 File Offset: 0x001293C3
		public void ClearFirstPersonItems()
		{
			this._itemModelRendererCount = 0;
		}

		// Token: 0x06004A23 RID: 18979 RVA: 0x0012B1D0 File Offset: 0x001293D0
		public void SetFirstPersonItems()
		{
			bool flag = base.PrimaryItem != null;
			if (flag)
			{
				this.SetFirstPersonItem(this._baseFirstPersonModel, base.PrimaryItem);
			}
			bool flag2 = base.SecondaryItem != null;
			if (flag2)
			{
				this.SetFirstPersonItem(this._baseFirstPersonModel, base.SecondaryItem);
			}
			this.CurrentFirstPersonAnimationId = null;
			this._forceFXViewSwitch = true;
		}

		// Token: 0x06004A24 RID: 18980 RVA: 0x0012B22C File Offset: 0x0012942C
		public void SetFirstPersonView(BlockyModel thirdPersonModel)
		{
			this._baseFirstPersonModel = thirdPersonModel.CloneArmsAndLegs(CharacterPartStore.RightArmNameId, CharacterPartStore.RightForearmNameId, CharacterPartStore.LeftArmNameId, CharacterPartStore.LeftForearmNameId, CharacterPartStore.RightThighNameId, CharacterPartStore.LeftThighNameId);
			bool flag = this._baseFirstPersonModel.NodeCount == 0;
			if (!flag)
			{
				this._firstPersonModelRenderer = new ModelRenderer(this._baseFirstPersonModel, this._gameInstance.AtlasSizes, this._gameInstance.Engine.Graphics, this._gameInstance.FrameCounter, false);
				for (int i = 0; i < this._entityParticles.Count; i++)
				{
					bool flag2 = !this._baseFirstPersonModel.NodeIndicesByNameId.TryGetValue(this._entityParticles[i].TargetNodeNameId, out this._entityParticles[i].TargetFirstPersonNodeIndex);
					if (flag2)
					{
						this._entityParticles[i].TargetFirstPersonNodeIndex = -1;
					}
				}
				for (int j = 0; j < this._entityTrails.Count; j++)
				{
					bool flag3 = !this._baseFirstPersonModel.NodeIndicesByNameId.TryGetValue(this._entityTrails[j].TargetNodeNameId, out this._entityTrails[j].TargetFirstPersonNodeIndex);
					if (flag3)
					{
						this._entityTrails[j].TargetFirstPersonNodeIndex = -1;
					}
				}
				this._baseFirstPersonModel.NodeIndicesByNameId.TryGetValue(CharacterPartStore.RightArmNameId, out this._firstPersonArmRightIdx);
				this._baseFirstPersonModel.NodeIndicesByNameId.TryGetValue(CharacterPartStore.LeftArmNameId, out this._firstPersonArmLeftIdx);
				this.SetFirstPersonItems();
			}
		}

		// Token: 0x06004A25 RID: 18981 RVA: 0x0012B3CC File Offset: 0x001295CC
		private void SetFirstPersonItem(BlockyModel model, ClientItemBase item)
		{
			model.NodeIndicesByNameId.TryGetValue(this.EntityItems[this._itemModelRendererCount].TargetNodeNameId, out this._itemModelDrawTasks[this._itemModelRendererCount].NodeIndex);
			this._itemModelRendererCount++;
		}

		// Token: 0x06004A26 RID: 18982 RVA: 0x0012B420 File Offset: 0x00129620
		public override void SetTransform(Vector3 position, Vector3 bodyOrientation, Vector3 lookOrientation)
		{
			bool flag = lookOrientation != this.LookOrientation;
			if (flag)
			{
				this._gameInstance.CameraModule.Controller.SetRotation(lookOrientation);
			}
			base.SetTransform(position, bodyOrientation, lookOrientation);
		}

		// Token: 0x06004A27 RID: 18983 RVA: 0x0012B460 File Offset: 0x00129660
		public override void SetServerAnimation(string animationId, AnimationSlot slot, float animationTime = -1f, bool storeCurrentAnimationId = false)
		{
			base.SetServerAnimation(animationId, slot, animationTime, storeCurrentAnimationId);
			bool flag = slot != 2 || this._firstPersonModelRenderer == null;
			if (!flag)
			{
				EntityAnimation entityAnimation = base.GetItemAnimation(base.PrimaryItem, animationId, true);
				bool flag2 = entityAnimation == null;
				if (flag2)
				{
					entityAnimation = base.GetAnimation(animationId);
				}
				bool flag3 = entityAnimation == null;
				if (!flag3)
				{
					this.animationClipsGeometry = entityAnimation.ClipsGeometry;
					int num = 5;
					BlockyAnimation slotAnimation = this._firstPersonModelRenderer.GetSlotAnimation(num);
					bool flag4 = entityAnimation.FirstPersonData == slotAnimation && !this._gameInstance.App.Settings.UseOverrideFirstPersonAnimations;
					if (!flag4)
					{
						bool flag5 = entityAnimation.FirstPersonOverrideData != null && entityAnimation.FirstPersonOverrideData == slotAnimation && this._gameInstance.App.Settings.UseOverrideFirstPersonAnimations;
						if (!flag5)
						{
							bool looping = entityAnimation.Looping;
							BlockyAnimation firstPersonAnimation = this.GetFirstPersonAnimation(entityAnimation);
							this._firstPersonModelRenderer.SetSlotAnimation(num, firstPersonAnimation, looping, 1f, 0f, 12f, entityAnimation.PullbackConfig, false);
							this.CurrentFirstPersonAnimationId = animationId;
						}
					}
				}
			}
		}

		// Token: 0x06004A28 RID: 18984 RVA: 0x0012B57C File Offset: 0x0012977C
		public override EntityAnimation GetTargetActionAnimation(InteractionType interactionType)
		{
			bool flag = interactionType == 1 && base.SecondaryItem != null;
			EntityAnimation result;
			if (flag)
			{
				EntityAnimation animation = base.SecondaryItem.GetAnimation("Attack");
				bool flag2 = animation != null;
				if (flag2)
				{
					result = animation;
				}
				else
				{
					result = EntityAnimation.Empty;
				}
			}
			else
			{
				ClientItemBase clientItemBase = base.PrimaryItem ?? base.SecondaryItem;
				EntityAnimation entityAnimation = (clientItemBase != null) ? clientItemBase.GetAnimation(this.CurrentFirstPersonAnimationId) : null;
				bool flag3 = entityAnimation != null;
				if (flag3)
				{
					result = entityAnimation;
				}
				else
				{
					result = EntityAnimation.Empty;
				}
			}
			return result;
		}

		// Token: 0x06004A29 RID: 18985 RVA: 0x0012B608 File Offset: 0x00129808
		public override void SetActionAnimation(EntityAnimation targetAnimation, float animationTime = 0f, bool holdLastFrame = false, bool force = false)
		{
			base.SetActionAnimation(targetAnimation, animationTime, holdLastFrame, force);
			bool flag = this._firstPersonModelRenderer == null;
			if (!flag)
			{
				BlockyAnimation slotAnimation = this._firstPersonModelRenderer.GetSlotAnimation(5);
				bool flag2 = !force && targetAnimation.FirstPersonData == slotAnimation && !this._gameInstance.App.Settings.UseOverrideFirstPersonAnimations;
				if (!flag2)
				{
					bool flag3 = !force && targetAnimation.FirstPersonOverrideData != null && targetAnimation.FirstPersonOverrideData == slotAnimation && this._gameInstance.App.Settings.UseOverrideFirstPersonAnimations;
					if (!flag3)
					{
						BlockyAnimation firstPersonAnimation = this.GetFirstPersonAnimation(targetAnimation);
						this._firstPersonModelRenderer.SetSlotAnimation(5, firstPersonAnimation, targetAnimation.Looping, targetAnimation.Speed, animationTime, targetAnimation.BlendingDuration, targetAnimation.PullbackConfig, force);
						this.animationClipsGeometry = targetAnimation.ClipsGeometry;
					}
				}
			}
		}

		// Token: 0x06004A2A RID: 18986 RVA: 0x0012B6E4 File Offset: 0x001298E4
		public bool ShouldDisplayHudForEntityStat(int entityStatIndex)
		{
			return (base.PrimaryItem != null && base.PrimaryItem.ShouldDisplayHudForEntityStat(entityStatIndex)) || (base.SecondaryItem != null && base.SecondaryItem.ShouldDisplayHudForEntityStat(entityStatIndex));
		}

		// Token: 0x06004A2B RID: 18987 RVA: 0x0012B726 File Offset: 0x00129926
		public void UsePrimaryItem()
		{
			this.UseItem(0, "Attack");
		}

		// Token: 0x06004A2C RID: 18988 RVA: 0x0012B736 File Offset: 0x00129936
		public void UseSecondaryItem()
		{
			this.UseItem(1, "SecondaryAction");
		}

		// Token: 0x06004A2D RID: 18989 RVA: 0x0012B748 File Offset: 0x00129948
		private void UseItem(InteractionType interactionType, string animationId)
		{
			this.CurrentFirstPersonAnimationId = animationId;
			EntityAnimation targetActionAnimation = this.GetTargetActionAnimation(interactionType);
			this.SetActionAnimation(targetActionAnimation, 0f, false, false);
		}

		// Token: 0x06004A2E RID: 18990 RVA: 0x0012B774 File Offset: 0x00129974
		public override void UpdateWithoutPosition(float deltaTime, float distanceToCamera, bool skipUpdateLogic = false)
		{
			base.UpdateWithoutPosition(deltaTime, distanceToCamera, skipUpdateLogic);
			ref ClientMovementStates relativeMovementStates = ref this.GetRelativeMovementStates();
			float pitch = base.BodyOrientation.Pitch;
			float num = base.BodyOrientation.Yaw;
			float roll = base.BodyOrientation.Roll;
			Vector2 vector = (this._gameInstance.CameraModule.Controller.AttachedTo == this) ? this._gameInstance.CharacterControllerModule.MovementController.GetWishDirection() : Vector2.Zero;
			bool flag = vector.Length() > 0.1f;
			if (flag)
			{
				float num2 = (vector.Y == 0f) ? 1f : vector.Y;
				float num3 = 0.7853982f * -vector.X * num2;
				this._moveAngle = MathHelper.LerpAngle(this._moveAngle, MathHelper.WrapAngle(this._gameInstance.CameraModule.Controller.MovementForceRotation.Yaw + num3), 0.4f);
				float num4 = MathHelper.WrapAngle(this.LookOrientation.Yaw - this._moveAngle);
				bool flag2 = num4 < -2.0943952f || num4 > 2.0943952f;
				if (flag2)
				{
					num4 = MathHelper.WrapAngle(num4 - 3.1415927f);
				}
				float num5 = this.LookOrientation.Yaw - MathHelper.Clamp(num4, base.CameraSettings.Yaw.AngleRange.Min, base.CameraSettings.Yaw.AngleRange.Max);
				float num6 = MathHelper.WrapAngle(num5 - num);
				num6 = MathHelper.Clamp(num6, -6f * deltaTime, 6f * deltaTime);
				num += num6;
			}
			else
			{
				bool flag3 = base.CameraSettings.Yaw.AngleRange.Max != 3.1415927f && base.CameraSettings.Yaw.AngleRange.Min != -3.1415927f;
				if (flag3)
				{
					float num7 = MathHelper.WrapAngle(this.LookOrientation.Yaw) - MathHelper.WrapAngle(num);
					bool flag4 = num7 < 0f;
					if (flag4)
					{
						num7 += 6.2831855f;
					}
					float num8 = MathHelper.WrapAngle(this.LookOrientation.Yaw - num);
					bool flag5 = num7 > 3.1415927f;
					bool flag6 = flag5 != this._wasLookYawOffsetClockwise;
					if (flag6)
					{
						bool flag7 = num8 > 2.0943952f || num8 < -2.0943952f;
						if (flag7)
						{
							bool wasLookYawOffsetClockwise = this._wasLookYawOffsetClockwise;
							if (wasLookYawOffsetClockwise)
							{
								num8 = -3.1415927f - (3.1415927f - num8);
							}
							else
							{
								num8 = 3.1415927f + (3.1415927f + num8);
							}
							flag5 = !flag5;
						}
					}
					bool flag8 = num8 > base.CameraSettings.Yaw.AngleRange.Max;
					if (flag8)
					{
						num += num8 - base.CameraSettings.Yaw.AngleRange.Max;
					}
					else
					{
						bool flag9 = num8 < base.CameraSettings.Yaw.AngleRange.Min;
						if (flag9)
						{
							num += num8 - base.CameraSettings.Yaw.AngleRange.Min;
						}
					}
					this._wasLookYawOffsetClockwise = flag5;
				}
			}
			bool flag10 = !this.IsMounting;
			if (flag10)
			{
				base.SetBodyOrientation(new Vector3(pitch, num, roll));
			}
			bool flag11 = this._firstPersonModelRenderer == null;
			if (!flag11)
			{
				bool flag12 = base.CurrentActionAnimation == null && this.CurrentFirstPersonAnimationId != this._currentAnimationId;
				if (flag12)
				{
					this.CurrentFirstPersonAnimationId = this._currentAnimationId;
					float currentSpeedMultiplierDiff = this._gameInstance.CharacterControllerModule.MovementController.CurrentSpeedMultiplierDiff;
					float slotAnimationTime = this._firstPersonModelRenderer.GetSlotAnimationTime(0);
					bool looping = (base.GetAnimation(this._currentAnimationId) ?? EntityAnimation.Empty).Looping;
					EntityAnimation itemAnimation = base.GetItemAnimation(base.PrimaryItem, this._currentAnimationId, true);
					bool flag13 = !itemAnimation.KeepPreviousFirstPersonAnimation;
					if (flag13)
					{
						BlockyAnimation firstPersonAnimation = this.GetFirstPersonAnimation(itemAnimation);
						this._firstPersonModelRenderer.SetSlotAnimation(1, firstPersonAnimation, looping, itemAnimation.Speed + itemAnimation.Speed * currentSpeedMultiplierDiff, slotAnimationTime, itemAnimation.BlendingDuration, itemAnimation.PullbackConfig, false);
						this.animationClipsGeometry = itemAnimation.ClipsGeometry;
					}
					EntityAnimation itemAnimation2 = base.GetItemAnimation(base.SecondaryItem, this._currentAnimationId, false);
					bool flag14 = !itemAnimation2.KeepPreviousFirstPersonAnimation;
					if (flag14)
					{
						BlockyAnimation firstPersonAnimation2 = this.GetFirstPersonAnimation(itemAnimation2);
						int slotIndex = (base.PrimaryItem == null) ? 1 : 2;
						this._firstPersonModelRenderer.SetSlotAnimation(slotIndex, firstPersonAnimation2, looping, itemAnimation2.Speed + itemAnimation2.Speed * currentSpeedMultiplierDiff, slotAnimationTime, itemAnimation2.BlendingDuration, itemAnimation2.PullbackConfig, false);
						bool flag15 = !this.animationClipsGeometry && itemAnimation2.ClipsGeometry;
						if (flag15)
						{
							this.animationClipsGeometry = true;
						}
					}
					bool flag16 = this._firstPersonModelRenderer.GetSlotAnimation(5) != null;
					if (flag16)
					{
						this._firstPersonModelRenderer.SetSlotAnimation(5, null, true, 1f, 0f, 12f, null, false);
					}
				}
				else
				{
					this._firstPersonModelRenderer.AdvancePlayback(deltaTime * 60f);
				}
				this.UpdateFirstPersonObstacleDetection();
				this._wasOnGround = relativeMovementStates.IsOnGround;
			}
		}

		// Token: 0x06004A2F RID: 18991 RVA: 0x0012BCC0 File Offset: 0x00129EC0
		private void UpdateFirstPersonObstacleDetection()
		{
			this._firstPersonObstacleDistance = -1f;
			Vector3 origin = new Vector3(base.Position.X, base.Position.Y + base.EyeOffset, base.Position.Z);
			Vector3 forward = Vector3.Forward;
			Vector3 rotation = this._gameInstance.CameraModule.Controller.Rotation;
			Quaternion quaternion = Quaternion.CreateFromYawPitchRoll(rotation.Yaw, rotation.Pitch, rotation.Roll);
			Vector3.Transform(ref forward, ref quaternion, out forward);
			bool flag2;
			HitDetection.RaycastHit raycastHit;
			bool flag3;
			HitDetection.EntityHitData entityHitData;
			bool flag = this._gameInstance.HitDetection.Raycast(origin, forward, this._firstPersonObstacleRaycastOptions, out flag2, out raycastHit, out flag3, out entityHitData);
			if (flag)
			{
				bool flag4 = !flag2 && !flag3;
				if (!flag4)
				{
					bool flag5 = flag2 && !flag3;
					if (flag5)
					{
						this._firstPersonObstacleDistance = raycastHit.Distance;
					}
					else
					{
						bool flag6 = !flag2;
						if (flag6)
						{
							this._firstPersonObstacleDistance = entityHitData.ClosestDistance;
						}
						else
						{
							this._firstPersonObstacleDistance = ((raycastHit.Distance < entityHitData.ClosestDistance) ? raycastHit.Distance : entityHitData.ClosestDistance);
						}
					}
				}
			}
		}

		// Token: 0x06004A30 RID: 18992 RVA: 0x0012BDE8 File Offset: 0x00129FE8
		public void UpdateClientInterpolation(float timeFraction)
		{
			this.RenderPosition = Vector3.Lerp(this._previousPosition, this._nextPosition, timeFraction) + new Vector3(0f, this._gameInstance.CharacterControllerModule.MovementController.AutoJumpHeightShift, 0f);
			this._movementWiggle.X = MathHelper.Lerp(this._previousMovementWiggle.X, this._nextMovementWiggle.X, timeFraction);
			this._movementWiggle.Y = MathHelper.Lerp(this._previousMovementWiggle.Y, this._nextMovementWiggle.Y, timeFraction);
			this._movementWiggle.Z = MathHelper.Lerp(this._previousMovementWiggle.Z, this._nextMovementWiggle.Z, timeFraction);
			this._movementWiggle.Roll = MathHelper.Lerp(this._previousMovementWiggle.Roll, this._nextMovementWiggle.Roll, timeFraction);
			this._movementWiggle.Pitch = MathHelper.Lerp(this._previousMovementWiggle.Pitch, this._nextMovementWiggle.Pitch, timeFraction);
		}

		// Token: 0x06004A31 RID: 18993 RVA: 0x0012BEFC File Offset: 0x0012A0FC
		public void UpdateClientInterpolationMouseWiggle(float timeFraction)
		{
			this._mouseWiggle.X = MathHelper.Lerp(this._previousMouseWiggle.X, this._nextMouseWiggle.X, timeFraction);
			this._mouseWiggle.Y = MathHelper.Lerp(this._previousMouseWiggle.Y, this._nextMouseWiggle.Y, timeFraction);
			this._mouseWiggle.Roll = MathHelper.Lerp(this._previousMouseWiggle.Roll, this._nextMouseWiggle.Roll, timeFraction);
			this._mouseWiggle.Pitch = MathHelper.Lerp(this._previousMouseWiggle.Pitch, this._nextMouseWiggle.Pitch, timeFraction);
		}

		// Token: 0x06004A32 RID: 18994 RVA: 0x0012BFA6 File Offset: 0x0012A1A6
		public bool FirstPersonViewNeedsDrawing()
		{
			return this._gameInstance.App.InGame.IsFirstPersonViewVisible && this._firstPersonModelRenderer != null && this._gameInstance.CameraModule.Controller.IsFirstPerson;
		}

		// Token: 0x06004A33 RID: 18995 RVA: 0x0012BFE0 File Offset: 0x0012A1E0
		public void ApplyFirstPersonMouseItemWiggle(float moveX, float moveY)
		{
			this._previousMouseWiggle = this._nextMouseWiggle;
			ClientItemBase clientItemBase = base.PrimaryItem ?? base.SecondaryItem;
			bool flag = clientItemBase == null;
			if (!flag)
			{
				this._nextMouseWiggle.X = this.CalculateUpdatedItemWiggle(this._nextMouseWiggle.X, moveX, clientItemBase.PlayerAnimations.WiggleWeights.XDeceleration);
				this._nextMouseWiggle.Y = this.CalculateUpdatedItemWiggle(this._nextMouseWiggle.Y, moveY, clientItemBase.PlayerAnimations.WiggleWeights.YDeceleration);
				this._nextMouseWiggle.Roll = this.CalculateUpdatedItemWiggle(this._nextMouseWiggle.Roll, -moveX, clientItemBase.PlayerAnimations.WiggleWeights.RollDeceleration);
				this._nextMouseWiggle.Pitch = this.CalculateUpdatedItemWiggle(this._nextMouseWiggle.Pitch, moveY, clientItemBase.PlayerAnimations.WiggleWeights.PitchDeceleration);
			}
		}

		// Token: 0x06004A34 RID: 18996 RVA: 0x0012C0D0 File Offset: 0x0012A2D0
		public void ApplyFirstPersonMovementItemWiggle(float moveX, float moveY, float moveZ)
		{
			this._previousMovementWiggle = this._nextMovementWiggle;
			ClientItemBase clientItemBase = base.PrimaryItem ?? base.SecondaryItem;
			bool flag = clientItemBase == null;
			if (!flag)
			{
				this._nextMovementWiggle.X = this.CalculateUpdatedItemWiggle(this._nextMovementWiggle.X, moveX, clientItemBase.PlayerAnimations.WiggleWeights.XDeceleration);
				this._nextMovementWiggle.Y = this.CalculateUpdatedItemWiggle(this._nextMovementWiggle.Y, moveY, clientItemBase.PlayerAnimations.WiggleWeights.YDeceleration);
				this._nextMovementWiggle.Z = this.CalculateUpdatedItemWiggle(this._nextMovementWiggle.Z, moveZ, clientItemBase.PlayerAnimations.WiggleWeights.ZDeceleration);
				this._nextMovementWiggle.Roll = this.CalculateUpdatedItemWiggle(this._nextMovementWiggle.Roll, -moveX, clientItemBase.PlayerAnimations.WiggleWeights.RollDeceleration);
				this._nextMovementWiggle.Pitch = this.CalculateUpdatedItemWiggle(this._nextMovementWiggle.Pitch, moveY, clientItemBase.PlayerAnimations.WiggleWeights.PitchDeceleration);
			}
		}

		// Token: 0x06004A35 RID: 18997 RVA: 0x0012C1EC File Offset: 0x0012A3EC
		private float CalculateUpdatedItemWiggle(float currentValue, float offset, float deceleration)
		{
			float num = (float)Math.Sqrt((double)(1f - Math.Abs(currentValue))) * 0.25f;
			currentValue = MathHelper.Step(currentValue, (offset > 0f) ? 1f : -1f, Math.Abs(offset) * num);
			currentValue = MathHelper.Step(currentValue, 0f, deceleration * MathHelper.Distance(currentValue, 0f));
			return currentValue;
		}

		// Token: 0x06004A36 RID: 18998 RVA: 0x0012C258 File Offset: 0x0012A458
		public void ClearFirstPersonItemWiggle()
		{
			this._nextMouseWiggle.Y = (this._nextMouseWiggle.X = (this._nextMouseWiggle.Pitch = (this._nextMouseWiggle.Roll = 0f)));
			this._nextMovementWiggle.Y = (this._nextMovementWiggle.X = (this._nextMovementWiggle.Z = (this._nextMovementWiggle.Pitch = (this._nextMovementWiggle.Roll = 0f))));
		}

		// Token: 0x06004A37 RID: 18999 RVA: 0x0012C2E8 File Offset: 0x0012A4E8
		public void LookAt(Vector3 relativePosition, float interpolation = 1f)
		{
			relativePosition -= base.Position;
			relativePosition.Y -= base.EyeOffset;
			bool flag = !MathHelper.WithinEpsilon(relativePosition.X, 0f) || !MathHelper.WithinEpsilon(relativePosition.Z, 0f);
			if (flag)
			{
				float num = (float)Math.Atan2((double)(-(double)relativePosition.X), (double)(-(double)relativePosition.Z));
				num = MathHelper.WrapAngle(num);
				this.LookOrientation.Yaw = MathHelper.LerpAngle(this.LookOrientation.Yaw, num, interpolation);
			}
			float num2 = relativePosition.Length();
			bool flag2 = num2 > 0f;
			if (flag2)
			{
				float num3 = 1.5707964f - (float)Math.Acos((double)(relativePosition.Y / num2));
				num3 = MathHelper.Clamp(num3, -1.5607964f, 1.5607964f);
				this.LookOrientation.Pitch = MathHelper.LerpAngle(this.LookOrientation.Pitch, num3, interpolation);
			}
			this.UpdateModelLookOrientation();
		}

		// Token: 0x06004A38 RID: 19000 RVA: 0x0012C3E8 File Offset: 0x0012A5E8
		public void UpdateModelLookOrientation()
		{
			Quaternion identity = Quaternion.Identity;
			Quaternion identity2 = Quaternion.Identity;
			bool flag = !this._gameInstance.CameraModule.Controller.IsFirstPerson;
			if (flag)
			{
				Vector3 bodyOrientation = base.BodyOrientation;
				bodyOrientation.Yaw -= 3.1415927f;
				Quaternion.CreateFromYawPitchRoll(bodyOrientation.Yaw, -bodyOrientation.Pitch, -bodyOrientation.Roll, out identity);
				Vector3 lookOrientation = this.LookOrientation;
				lookOrientation.Yaw -= 3.1415927f;
				float num = MathHelper.WrapAngle(bodyOrientation.Yaw - lookOrientation.Yaw);
				num = MathHelper.Clamp(num, base.CameraSettings.Yaw.AngleRange.Min, base.CameraSettings.Yaw.AngleRange.Max);
				lookOrientation.Yaw = bodyOrientation.Yaw - num;
				float num2 = MathHelper.WrapAngle(bodyOrientation.Pitch - lookOrientation.Pitch);
				num2 = MathHelper.Clamp(num2, base.CameraSettings.Pitch.AngleRange.Min, base.CameraSettings.Pitch.AngleRange.Max);
				lookOrientation.Pitch = bodyOrientation.Pitch - num2;
				Quaternion.CreateFromYawPitchRoll(lookOrientation.Yaw, -lookOrientation.Pitch, -lookOrientation.Roll, out identity2);
			}
			base.ModelRenderer.SetCameraOrientation(Quaternion.Inverse(identity) * identity2);
		}

		// Token: 0x06004A39 RID: 19001 RVA: 0x0012C568 File Offset: 0x0012A768
		public override bool AddCombatSequenceEffects(ModelParticle[] particles, ModelTrail[] trails)
		{
			bool flag = !base.AddCombatSequenceEffects(particles, trails);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool isFirstPerson = this._gameInstance.CameraModule.Controller.IsFirstPerson;
				for (int i = 0; i < this._combatSequenceParticles.Count; i++)
				{
					bool flag2 = !this._baseFirstPersonModel.NodeIndicesByNameId.TryGetValue(this._combatSequenceParticles[i].TargetNodeNameId, out this._combatSequenceParticles[i].TargetFirstPersonNodeIndex);
					if (flag2)
					{
						this._combatSequenceParticles[i].TargetFirstPersonNodeIndex = -1;
					}
					this._combatSequenceParticles[i].ParticleSystemProxy.SetFirstPerson(isFirstPerson);
				}
				for (int j = 0; j < this._combatSequenceTrails.Count; j++)
				{
					bool flag3 = !this._baseFirstPersonModel.NodeIndicesByNameId.TryGetValue(this._combatSequenceTrails[j].TargetNodeNameId, out this._combatSequenceTrails[j].TargetFirstPersonNodeIndex);
					if (flag3)
					{
						this._combatSequenceTrails[j].TargetFirstPersonNodeIndex = -1;
					}
					this._combatSequenceTrails[j].TrailProxy.SetFirstPerson(isFirstPerson);
				}
				result = true;
			}
			return result;
		}

		// Token: 0x06004A3A RID: 19002 RVA: 0x0012C6C0 File Offset: 0x0012A8C0
		public void RegisterAnimationTasks()
		{
			AnimationSystem animationSystem = this._gameInstance.Engine.AnimationSystem;
			animationSystem.PrepareForIncomingTasks(2 + this._itemModelRendererCount + this.EntityEffects.Length);
			bool flag = base.ModelRenderer != null;
			if (flag)
			{
				animationSystem.RegisterAnimationTask(base.ModelRenderer, false);
			}
			for (int i = 0; i < this._itemModelRendererCount; i++)
			{
				bool flag2 = this.EntityItems[i].ModelRenderer != null;
				if (flag2)
				{
					animationSystem.RegisterAnimationTask(this.EntityItems[i].ModelRenderer, false);
				}
			}
			for (int j = 0; j < this.EntityEffects.Length; j++)
			{
				ref Entity.UniqueEntityEffect ptr = ref this.EntityEffects[j];
				bool flag3 = ptr.ModelRenderer != null;
				if (flag3)
				{
					animationSystem.RegisterAnimationTask(ptr.ModelRenderer, false);
				}
			}
			bool flag4 = this._firstPersonModelRenderer != null;
			if (flag4)
			{
				animationSystem.RegisterAnimationTask(this._firstPersonModelRenderer, false);
			}
		}

		// Token: 0x06004A3B RID: 19003 RVA: 0x0012C7D0 File Offset: 0x0012A9D0
		public void UpdateFirstPersonFX()
		{
			Debug.Assert(this.FirstPersonViewNeedsDrawing(), "UpdateFirstPersonFX called when it was not required. Please check with FirstPersonViewNeedsDrawing() first before calling this.");
			this._renderOrientation *= this._renderOrientationOffset;
			for (int i = 0; i < this._entityParticles.Count; i++)
			{
				Entity.EntityParticle entityParticle = this._entityParticles[i];
				int num = entityParticle.TargetFirstPersonNodeIndex;
				ModelRenderer modelRenderer = this._firstPersonModelRenderer;
				entityParticle.ParticleSystemProxy.Position = Vector3.Zero;
				bool flag = num == -1;
				if (flag)
				{
					num = entityParticle.TargetNodeIndex;
					modelRenderer = base.ModelRenderer;
					ParticleSystemProxy particleSystemProxy = entityParticle.ParticleSystemProxy;
					particleSystemProxy.Position.Y = particleSystemProxy.Position.Y - base.EyeOffset;
				}
				ref AnimatedRenderer.NodeTransform ptr = ref modelRenderer.NodeTransforms[num];
				entityParticle.ParticleSystemProxy.Position = entityParticle.ParticleSystemProxy.Position + this._renderPosition + Vector3.Transform(ptr.Position, this._renderOrientation) * 0.015625f * this.Scale + Vector3.Transform(entityParticle.PositionOffset, this._renderOrientation * ptr.Orientation);
				entityParticle.ParticleSystemProxy.Rotation = this._renderOrientation * ptr.Orientation * entityParticle.RotationOffset;
			}
			for (int j = 0; j < this._entityTrails.Count; j++)
			{
				Entity.EntityTrail entityTrail = this._entityTrails[j];
				int num2 = entityTrail.TargetFirstPersonNodeIndex;
				ModelRenderer modelRenderer2 = this._firstPersonModelRenderer;
				entityTrail.TrailProxy.Position = Vector3.Zero;
				bool flag2 = num2 == -1;
				if (flag2)
				{
					num2 = entityTrail.TargetNodeIndex;
					modelRenderer2 = base.ModelRenderer;
					TrailProxy trailProxy = entityTrail.TrailProxy;
					trailProxy.Position.Y = trailProxy.Position.Y - base.EyeOffset;
				}
				ref AnimatedRenderer.NodeTransform ptr2 = ref modelRenderer2.NodeTransforms[num2];
				entityTrail.TrailProxy.Position = this._renderPosition + Vector3.Transform(ptr2.Position, this._renderOrientation) * 0.015625f * this.Scale + Vector3.Transform(entityTrail.PositionOffset, this._renderOrientation * ptr2.Orientation);
				bool fixedRotation = entityTrail.FixedRotation;
				if (fixedRotation)
				{
					Vector3 rotation = this._gameInstance.CameraModule.Controller.Rotation;
					entityTrail.TrailProxy.Rotation = Quaternion.CreateFromYawPitchRoll(0f, -rotation.X, 0f) * Quaternion.CreateFromYawPitchRoll(-rotation.Y, 0f, 0f) * entityTrail.RotationOffset;
				}
				else
				{
					entityTrail.TrailProxy.Rotation = this._renderOrientation * ptr2.Orientation * entityTrail.RotationOffset;
				}
			}
			for (int k = 0; k < this._itemModelRendererCount; k++)
			{
				Entity.EntityItem entityItem = this.EntityItems[k];
				bool flag3 = entityItem.Particles.Count == 0 && entityItem.Trails.Count == 0;
				if (!flag3)
				{
					ref AnimatedRenderer.NodeTransform ptr3 = ref this._firstPersonModelRenderer.NodeTransforms[this._itemModelDrawTasks[k].NodeIndex];
					Quaternion quaternion = ptr3.Orientation * entityItem.RootOrientationOffset;
					for (int l = 0; l < entityItem.Particles.Count; l++)
					{
						Entity.EntityParticle entityParticle2 = entityItem.Particles[l];
						ref AnimatedRenderer.NodeTransform ptr4 = ref entityItem.ModelRenderer.NodeTransforms[entityParticle2.TargetNodeIndex];
						entityParticle2.ParticleSystemProxy.Position = this._renderPosition + Vector3.Transform(ptr3.Position + Vector3.Transform(ptr4.Position * entityItem.Scale + entityItem.RootPositionOffset, quaternion), this._renderOrientation) * 0.015625f * this.Scale + Vector3.Transform(entityParticle2.PositionOffset, this._renderOrientation * quaternion * ptr4.Orientation);
						entityParticle2.ParticleSystemProxy.Rotation = this._renderOrientation * quaternion * ptr4.Orientation * entityParticle2.RotationOffset;
					}
					for (int m = 0; m < entityItem.Trails.Count; m++)
					{
						Entity.EntityTrail entityTrail2 = entityItem.Trails[m];
						ref AnimatedRenderer.NodeTransform ptr5 = ref entityItem.ModelRenderer.NodeTransforms[entityTrail2.TargetNodeIndex];
						entityTrail2.TrailProxy.Position = this._renderPosition + Vector3.Transform(ptr3.Position + Vector3.Transform(ptr5.Position * entityItem.Scale + entityItem.RootPositionOffset, quaternion), this._renderOrientation) * 0.015625f * this.Scale + Vector3.Transform(entityTrail2.PositionOffset, this._renderOrientation * quaternion * ptr5.Orientation);
						bool fixedRotation2 = entityTrail2.FixedRotation;
						if (fixedRotation2)
						{
							Vector3 rotation2 = this._gameInstance.CameraModule.Controller.Rotation;
							entityTrail2.TrailProxy.Rotation = Quaternion.CreateFromYawPitchRoll(0f, -rotation2.X, 0f) * Quaternion.CreateFromYawPitchRoll(-rotation2.Y, 0f, 0f) * entityTrail2.RotationOffset;
						}
						else
						{
							entityTrail2.TrailProxy.Rotation = this._renderOrientation * quaternion * ptr5.Orientation * entityTrail2.RotationOffset;
						}
					}
				}
			}
			bool flag4 = false;
			for (int n = 0; n < this._itemModelRendererCount; n++)
			{
				ClientModelVFX modelVFX = this.EntityItems[n].ModelVFX;
				int num3 = modelVFX.PackedModelVFXParams >> 3 & 3;
				flag4 = (flag4 || (modelVFX.AnimationProgress != 0f && num3 == 2));
			}
			int num4 = this.ModelVFX.PackedModelVFXParams >> 3 & 3;
			this._needsDistortionEffect = (flag4 || (this.ModelVFX.AnimationProgress != 0f && num4 == 2));
		}

		// Token: 0x06004A3C RID: 19004 RVA: 0x0012CEA4 File Offset: 0x0012B0A4
		public void PrepareFXForViewSwitch()
		{
			bool isFirstPerson = this._gameInstance.CameraModule.Controller.IsFirstPerson;
			bool flag = isFirstPerson == this._wasFirstPerson && !this._forceFXViewSwitch;
			if (!flag)
			{
				base.RefreshCharacterItemParticles();
				for (int i = 0; i < this._entityParticles.Count; i++)
				{
					this._entityParticles[i].ParticleSystemProxy.SetFirstPerson(isFirstPerson);
				}
				for (int j = 0; j < this._entityTrails.Count; j++)
				{
					this._entityTrails[j].TrailProxy.SetFirstPerson(isFirstPerson);
				}
				for (int k = 0; k < this._itemModelRendererCount; k++)
				{
					Entity.EntityItem entityItem = this.EntityItems[k];
					for (int l = 0; l < entityItem.Particles.Count; l++)
					{
						entityItem.Particles[l].ParticleSystemProxy.SetFirstPerson(isFirstPerson);
					}
					for (int m = 0; m < entityItem.Trails.Count; m++)
					{
						entityItem.Trails[m].TrailProxy.SetFirstPerson(isFirstPerson);
					}
				}
				this._wasFirstPerson = isFirstPerson;
				this._forceFXViewSwitch = false;
			}
		}

		// Token: 0x06004A3D RID: 19005 RVA: 0x0012D010 File Offset: 0x0012B210
		public void PrepareForDrawInFirstPersonView()
		{
			Debug.Assert(this.FirstPersonViewNeedsDrawing(), "PrepareFirstPersonViewForDraw called when it was not required. Please check with FirstPersonViewNeedsDrawing() first before calling this.");
			Matrix.CreateScale(0.015625f * this.Scale, out this._modelMatrix);
			Matrix.CreateRotationY(3.1415927f, out this._tempMatrix);
			Matrix.Multiply(ref this._modelMatrix, ref this._tempMatrix, out this._modelMatrix);
			this._renderPosition = Vector3.Zero;
			this._renderOrientation = Quaternion.Identity;
			Quaternion quaternion = Quaternion.Identity;
			bool flag = this.IsFirstPersonClipping();
			if (flag)
			{
				Quaternion.CreateFromYawPitchRoll(this.LookOrientation.Yaw, this.LookOrientation.Pitch, this.LookOrientation.Roll, out this._renderOrientation);
			}
			ClientItemBase clientItemBase = base.PrimaryItem ?? base.SecondaryItem;
			bool flag2 = clientItemBase != null;
			if (flag2)
			{
				ItemPlayerAnimations.WiggleWeights wiggleWeights = clientItemBase.PlayerAnimations.WiggleWeights;
				bool flag3 = wiggleWeights.X != 0f;
				if (flag3)
				{
					this._renderPosition.X = this._renderPosition.X + (this._movementWiggle.X + this._mouseWiggle.X) * wiggleWeights.X * 0.1f;
				}
				bool flag4 = wiggleWeights.Y != 0f;
				if (flag4)
				{
					this._renderPosition.Y = this._renderPosition.Y + (this._movementWiggle.Y + this._mouseWiggle.Y) * wiggleWeights.Y * 0.1f;
				}
				bool flag5 = wiggleWeights.Z != 0f;
				if (flag5)
				{
					this._renderPosition.Z = this._renderPosition.Z + this._movementWiggle.Z * wiggleWeights.Z * 0.1f;
				}
				bool flag6 = wiggleWeights.Roll != 0f;
				if (flag6)
				{
					this._renderOrientation *= Quaternion.CreateFromYawPitchRoll(0f, 0f, (this._movementWiggle.Roll + this._mouseWiggle.Roll) * 0.09817477f * wiggleWeights.Roll);
					quaternion *= Quaternion.CreateFromYawPitchRoll(0f, 0f, (this._movementWiggle.Roll + this._mouseWiggle.Roll) * 0.09817477f * wiggleWeights.Roll);
				}
				bool flag7 = wiggleWeights.Pitch != 0f;
				if (flag7)
				{
					this._renderOrientation *= Quaternion.CreateFromYawPitchRoll(0f, (this._movementWiggle.Pitch + this._mouseWiggle.Pitch) * 0.09817477f * wiggleWeights.Pitch, 0f);
					quaternion *= Quaternion.CreateFromYawPitchRoll(0f, (this._movementWiggle.Pitch + this._mouseWiggle.Pitch) * 0.09817477f * wiggleWeights.Pitch, 0f);
				}
			}
			this.UpdateFirstPersonWeaponPullback(0.03125f);
			Vector3 attachmentPosition = this._gameInstance.CameraModule.Controller.AttachmentPosition;
			Vector3 position = this._gameInstance.CameraModule.Controller.Position;
			Vector3 vector = attachmentPosition - position;
			Matrix.AddTranslation(ref this._modelMatrix, this._renderPosition.X, this._renderPosition.Y, this._renderPosition.Z);
			Matrix.CreateFromQuaternion(ref this._renderOrientation, out this._tempMatrix);
			Matrix.Multiply(ref this._modelMatrix, ref this._tempMatrix, out this._modelMatrix);
			bool flag8 = this.IsFirstPersonClipping();
			if (flag8)
			{
				Matrix.AddTranslation(ref this._modelMatrix, vector.X, vector.Y, vector.Z);
			}
			for (int i = 0; i < this._itemModelRendererCount; i++)
			{
				Entity.EntityItem entityItem = this.EntityItems[i];
				ref AnimatedRenderer.NodeTransform ptr = ref this._firstPersonModelRenderer.NodeTransforms[this._itemModelDrawTasks[i].NodeIndex];
				Matrix matrix;
				Matrix.Compose(ptr.Orientation, ptr.Position, out matrix);
				Matrix.Multiply(entityItem.RootOffsetMatrix, ref matrix, out matrix);
				Matrix.Multiply(ref matrix, ref this._modelMatrix, out this._itemModelDrawTasks[i].ModelMatrix);
				Matrix.ApplyScale(ref this._itemModelDrawTasks[i].ModelMatrix, entityItem.Scale);
				this._renderOrientation = quaternion;
			}
		}

		// Token: 0x06004A3E RID: 19006 RVA: 0x0012D490 File Offset: 0x0012B690
		private void UpdateFirstPersonWeaponPullback(float offsetScaleFactor)
		{
			Vector3 zero = Vector3.Zero;
			Vector3 zero2 = Vector3.Zero;
			Vector3 zero3 = Vector3.Zero;
			Vector3 zero4 = Vector3.Zero;
			float num = 1f;
			ref ClientMovementStates relativeMovementStates = ref this.GetRelativeMovementStates();
			bool flag = !relativeMovementStates.IsClimbing && !relativeMovementStates.IsMantling;
			float num2;
			bool flag2 = this.IsApproachingObstacle(out num2) && num2 <= this._firstPersonWeaponDrawbackStartDistance;
			bool flag3 = this._gameInstance.App.Settings.WeaponPullback && flag2 && flag;
			if (flag3)
			{
				this.CalculateItemPullbackOffsets();
				num = (this._firstPersonWeaponDrawbackStartDistance - num2) / this._firstPersonWeaponDrawbackStartDistance;
				float num3 = num * 32f;
				zero = new Vector3(this._pullbackRightOffset.X * num3, this._pullbackRightOffset.Y * num3, this._pullbackRightOffset.Z * num3);
				zero2 = new Vector3(this._pullbackLeftOffset.X * num3, this._pullbackLeftOffset.Y * num3, this._pullbackLeftOffset.Z * num3);
				float num4 = 3.1415927f * offsetScaleFactor;
				zero3 = new Vector3(this._pullbackRightRotation.X * num4, this._pullbackRightRotation.Y * num4, this._pullbackRightRotation.Z * num4);
				zero4 = new Vector3(this._pullbackLeftRotation.X * num4, this._pullbackLeftRotation.Y * num4, this._pullbackLeftRotation.Z * num4);
			}
			float amount = this._gameInstance.DeltaTime * (num * 3.5f);
			bool flag4 = this._firstPersonArmRightIdx != -1;
			if (flag4)
			{
				ref BlockyModelNode ptr = ref this._baseFirstPersonModel.AllNodes[this._firstPersonArmRightIdx];
				ptr.ProceduralOffset = Vector3.Lerp(ptr.ProceduralOffset, zero, amount);
				ptr.ProceduralRotation = Vector3.Lerp(ptr.ProceduralRotation, zero3, amount);
			}
			bool flag5 = this._firstPersonArmLeftIdx != -1;
			if (flag5)
			{
				ref BlockyModelNode ptr2 = ref this._baseFirstPersonModel.AllNodes[this._firstPersonArmLeftIdx];
				ptr2.ProceduralOffset = Vector3.Lerp(ptr2.ProceduralOffset, zero2, amount);
				ptr2.ProceduralRotation = Vector3.Lerp(ptr2.ProceduralRotation, zero4, amount);
			}
		}

		// Token: 0x06004A3F RID: 19007 RVA: 0x0012D6D8 File Offset: 0x0012B8D8
		private void CalculateItemPullbackOffsets()
		{
			ClientItemPullbackConfig slotPullbackConfig = this._firstPersonModelRenderer.GetSlotPullbackConfig(1);
			ClientItemPullbackConfig slotPullbackConfig2 = this._firstPersonModelRenderer.GetSlotPullbackConfig(2);
			bool flag = this._pullbackPrevPrimaryItem == base.PrimaryItem && this._pullbackPrevSecondaryItem == base.SecondaryItem && slotPullbackConfig == this._pullbackPrevPrimaryAnimConfig && slotPullbackConfig2 == this._pullbackPrevSecondaryAnimConfig;
			if (!flag)
			{
				this._pullbackPrevPrimaryItem = base.PrimaryItem;
				this._pullbackPrevSecondaryItem = base.SecondaryItem;
				this._pullbackPrevPrimaryAnimConfig = slotPullbackConfig;
				this._pullbackPrevSecondaryAnimConfig = slotPullbackConfig2;
				ClientItemBase primaryItem = base.PrimaryItem;
				ClientItemPullbackConfig clientItemPullbackConfig = (primaryItem != null) ? primaryItem.PullbackConfig : null;
				ClientItemBase secondaryItem = base.SecondaryItem;
				ClientItemPullbackConfig clientItemPullbackConfig2 = (secondaryItem != null) ? secondaryItem.PullbackConfig : null;
				bool flag2 = this.IsFirstPersonClipping();
				if (flag2)
				{
					this._pullbackRightOffset = Vector3.Zero;
					this._pullbackRightRotation = Vector3.Zero;
					this._pullbackLeftOffset = Vector3.Zero;
					this._pullbackLeftRotation = Vector3.Zero;
				}
				else
				{
					this._pullbackRightOffset = (((clientItemPullbackConfig != null) ? clientItemPullbackConfig.RightOffsetOverride : null) ?? (((slotPullbackConfig != null) ? slotPullbackConfig.RightOffsetOverride : null) ?? (((slotPullbackConfig2 != null) ? slotPullbackConfig2.RightOffsetOverride : null) ?? PlayerEntity.defaultPullbackOffsetRight)));
					this._pullbackRightRotation = (((clientItemPullbackConfig != null) ? clientItemPullbackConfig.RightRotationOverride : null) ?? (((clientItemPullbackConfig2 != null) ? clientItemPullbackConfig2.RightRotationOverride : null) ?? (((slotPullbackConfig != null) ? slotPullbackConfig.RightRotationOverride : null) ?? (((slotPullbackConfig2 != null) ? slotPullbackConfig2.RightRotationOverride : null) ?? PlayerEntity.defaultPullbackRotationRight))));
					this._pullbackLeftOffset = (((clientItemPullbackConfig != null) ? clientItemPullbackConfig.LeftOffsetOverride : null) ?? (((clientItemPullbackConfig2 != null) ? clientItemPullbackConfig2.LeftOffsetOverride : null) ?? (((slotPullbackConfig != null) ? slotPullbackConfig.LeftOffsetOverride : null) ?? (((slotPullbackConfig2 != null) ? slotPullbackConfig2.LeftOffsetOverride : null) ?? PlayerEntity.defaultPullbackOffsetLeft))));
					this._pullbackLeftRotation = (((clientItemPullbackConfig != null) ? clientItemPullbackConfig.LeftRotationOverride : null) ?? (((clientItemPullbackConfig2 != null) ? clientItemPullbackConfig2.LeftRotationOverride : null) ?? (((slotPullbackConfig != null) ? slotPullbackConfig.LeftRotationOverride : null) ?? (((slotPullbackConfig2 != null) ? slotPullbackConfig2.LeftRotationOverride : null) ?? PlayerEntity.defaultPullbackRotationLeft))));
				}
			}
		}

		// Token: 0x06004A40 RID: 19008 RVA: 0x0012DA68 File Offset: 0x0012BC68
		public void SendFirstPersonViewUniforms(Vector2 atlasSizeFactor0, Vector2 atlasSizeFactor1, Vector2 atlasSizeFactor2)
		{
			BlockyModelProgram blockyModelProgram = this.GetBlockyModelProgram();
			Debug.Assert(this.FirstPersonViewNeedsDrawing(), "SendFirstPersonViewUniforms called when it was not required. Please check with FirstPersonViewNeedsDrawing() first before calling this.");
			blockyModelProgram.AssertInUse();
			blockyModelProgram.StaticLightColor.SetValue(this._gameInstance.LocalPlayer.BlockLightColor);
			blockyModelProgram.BottomTint.SetValue(this._gameInstance.LocalPlayer.BottomTint);
			blockyModelProgram.TopTint.SetValue(this._gameInstance.LocalPlayer.TopTint);
			blockyModelProgram.ModelVFXHighlightColorAndThickness.SetValue(this.ModelVFX.HighlightColor.X, this.ModelVFX.HighlightColor.Y, this.ModelVFX.HighlightColor.Z, this.ModelVFX.HighlightThickness);
			blockyModelProgram.ModelVFXNoiseParams.SetValue(this.ModelVFX.NoiseScale.X, this.ModelVFX.NoiseScale.Y, this.ModelVFX.NoiseScrollSpeed.X, this.ModelVFX.NoiseScrollSpeed.Y);
			blockyModelProgram.ModelVFXAnimationProgress.SetValue(this.ModelVFX.AnimationProgress);
			blockyModelProgram.ModelVFXPackedParams.SetValue(this.ModelVFX.PackedModelVFXParams);
			blockyModelProgram.ModelVFXPostColor.SetValue(this.ModelVFX.PostColor);
			blockyModelProgram.AtlasSizeFactor0.SetValue(atlasSizeFactor0);
			blockyModelProgram.AtlasSizeFactor1.SetValue(atlasSizeFactor1);
			blockyModelProgram.AtlasSizeFactor2.SetValue(atlasSizeFactor2);
		}

		// Token: 0x06004A41 RID: 19009 RVA: 0x0012DBE8 File Offset: 0x0012BDE8
		public void DrawInFirstPersonView()
		{
			BlockyModelProgram blockyModelProgram = this.GetBlockyModelProgram();
			Debug.Assert(this.FirstPersonViewNeedsDrawing(), "DrawFirstPersonView called when it was not required. Please check with FirstPersonViewNeedsDrawing() first before calling this.");
			blockyModelProgram.AssertInUse();
			this._gameInstance.Engine.Graphics.GL.AssertTextureBound(GL.TEXTURE0, this._gameInstance.MapModule.TextureAtlas.GLTexture);
			this._gameInstance.Engine.Graphics.GL.AssertTextureBound(GL.TEXTURE1, this._gameInstance.EntityStoreModule.TextureAtlas.GLTexture);
			AnimationSystem animationSystem = this._gameInstance.Engine.AnimationSystem;
			blockyModelProgram.ModelMatrix.SetValue(ref this._modelMatrix);
			blockyModelProgram.NodeBlock.SetBufferRange(animationSystem.NodeBuffer, this._firstPersonModelRenderer.NodeBufferOffset, (uint)(this._firstPersonModelRenderer.NodeCount * 64));
			this._firstPersonModelRenderer.Draw();
			for (int i = 0; i < this._itemModelRendererCount; i++)
			{
				ClientModelVFX clientModelVFX = (this.EntityItems[i].ModelVFX.Id != null) ? this.EntityItems[i].ModelVFX : this.ModelVFX;
				blockyModelProgram.ModelVFXHighlightColorAndThickness.SetValue(clientModelVFX.HighlightColor.X, clientModelVFX.HighlightColor.Y, clientModelVFX.HighlightColor.Z, clientModelVFX.HighlightThickness);
				blockyModelProgram.ModelVFXNoiseParams.SetValue(clientModelVFX.NoiseScale.X, clientModelVFX.NoiseScale.Y, clientModelVFX.NoiseScrollSpeed.X, clientModelVFX.NoiseScrollSpeed.Y);
				blockyModelProgram.ModelVFXAnimationProgress.SetValue(clientModelVFX.AnimationProgress);
				blockyModelProgram.ModelVFXPackedParams.SetValue(clientModelVFX.PackedModelVFXParams);
				blockyModelProgram.ModelVFXPostColor.SetValue(clientModelVFX.PostColor);
				blockyModelProgram.ModelMatrix.SetValue(ref this._itemModelDrawTasks[i].ModelMatrix);
				blockyModelProgram.NodeBlock.SetBufferRange(animationSystem.NodeBuffer, this.EntityItems[i].ModelRenderer.NodeBufferOffset, (uint)(this.EntityItems[i].ModelRenderer.NodeCount * 64));
				this.EntityItems[i].ModelRenderer.Draw();
			}
		}

		// Token: 0x06004A42 RID: 19010 RVA: 0x0012DE40 File Offset: 0x0012C040
		public void DrawDistortionInFirstPersonView()
		{
			BlockyModelProgram firstPersonDistortionBlockyModelProgram = this._gameInstance.Engine.Graphics.GPUProgramStore.FirstPersonDistortionBlockyModelProgram;
			RenderTargetStore rtstore = this._gameInstance.Engine.Graphics.RTStore;
			Debug.Assert(this.FirstPersonViewNeedsDrawing(), "DrawInFirstPersonViewInDistortionBuffer called when it was not required. Please check with FirstPersonViewNeedsDrawing() first before calling this.");
			firstPersonDistortionBlockyModelProgram.AssertInUse();
			this._gameInstance.Engine.Graphics.GL.AssertTextureBound(GL.TEXTURE0, this._gameInstance.MapModule.TextureAtlas.GLTexture);
			this._gameInstance.Engine.Graphics.GL.AssertTextureBound(GL.TEXTURE1, this._gameInstance.EntityStoreModule.TextureAtlas.GLTexture);
			AnimationSystem animationSystem = this._gameInstance.Engine.AnimationSystem;
			firstPersonDistortionBlockyModelProgram.ModelMatrix.SetValue(ref this._modelMatrix);
			firstPersonDistortionBlockyModelProgram.ModelVFXNoiseParams.SetValue(this.ModelVFX.NoiseScale.X, this.ModelVFX.NoiseScale.Y, this.ModelVFX.NoiseScrollSpeed.X, this.ModelVFX.NoiseScrollSpeed.Y);
			firstPersonDistortionBlockyModelProgram.ModelVFXAnimationProgress.SetValue(this.ModelVFX.AnimationProgress);
			firstPersonDistortionBlockyModelProgram.ModelVFXPackedParams.SetValue(this.ModelVFX.PackedModelVFXParams);
			firstPersonDistortionBlockyModelProgram.CurrentInvViewportSize.SetValue(rtstore.Distortion.InvResolution);
			firstPersonDistortionBlockyModelProgram.NodeBlock.SetBufferRange(animationSystem.NodeBuffer, this._firstPersonModelRenderer.NodeBufferOffset, (uint)(this._firstPersonModelRenderer.NodeCount * 64));
			this._firstPersonModelRenderer.Draw();
			for (int i = 0; i < this._itemModelRendererCount; i++)
			{
				ClientModelVFX clientModelVFX = (this.EntityItems[i].ModelVFX.Id != null) ? this.EntityItems[i].ModelVFX : this.ModelVFX;
				firstPersonDistortionBlockyModelProgram.ModelVFXNoiseParams.SetValue(clientModelVFX.NoiseScale.X, clientModelVFX.NoiseScale.Y, clientModelVFX.NoiseScrollSpeed.X, clientModelVFX.NoiseScrollSpeed.Y);
				firstPersonDistortionBlockyModelProgram.ModelVFXAnimationProgress.SetValue(clientModelVFX.AnimationProgress);
				firstPersonDistortionBlockyModelProgram.ModelVFXPackedParams.SetValue(clientModelVFX.PackedModelVFXParams);
				firstPersonDistortionBlockyModelProgram.ModelMatrix.SetValue(ref this._itemModelDrawTasks[i].ModelMatrix);
				firstPersonDistortionBlockyModelProgram.NodeBlock.SetBufferRange(animationSystem.NodeBuffer, this.EntityItems[i].ModelRenderer.NodeBufferOffset, (uint)(this.EntityItems[i].ModelRenderer.NodeCount * 64));
				this.EntityItems[i].ModelRenderer.Draw();
			}
		}

		// Token: 0x06004A43 RID: 19011 RVA: 0x0012E118 File Offset: 0x0012C318
		private void PrepareForDrawInOcclusionMap(SceneView cameraSceneView)
		{
			Debug.Assert(!this.FirstPersonViewNeedsDrawing(), "PrepareForDrawInOcclusionMap called when it was not required. Please check with FirstPersonViewNeedsDrawing() first before calling this.");
			float scale = 0.015625f * this.Scale;
			Vector3 translation = this.RenderPosition - cameraSceneView.Position;
			Matrix.Compose(scale, this.RenderOrientation, translation, out this._modelMatrix);
			this.UpdateModelLookOrientation();
			for (int i = 0; i < this.EntityItems.Count; i++)
			{
				Entity.EntityItem entityItem = this.EntityItems[i];
				ref AnimatedRenderer.NodeTransform ptr = ref base.ModelRenderer.NodeTransforms[entityItem.TargetNodeIndex];
				Matrix matrix;
				Matrix.Compose(ptr.Orientation, ptr.Position, out matrix);
				Matrix.Multiply(entityItem.RootOffsetMatrix, ref matrix, out matrix);
				Matrix.Multiply(ref matrix, ref this._modelMatrix, out this._itemModelDrawTasks[i].ModelMatrix);
				Matrix.ApplyScale(ref this._itemModelDrawTasks[i].ModelMatrix, entityItem.Scale);
			}
		}

		// Token: 0x06004A44 RID: 19012 RVA: 0x0012E22C File Offset: 0x0012C42C
		private void DrawInOcclusionMap()
		{
			ZOnlyBlockyModelProgram blockyModelOcclusionMapProgram = this._gameInstance.Engine.Graphics.GPUProgramStore.BlockyModelOcclusionMapProgram;
			Debug.Assert(!this.FirstPersonViewNeedsDrawing(), "DrawOcclusion called when it was not required. Please check with FirstPersonViewNeedsDrawing() first before calling this.");
			blockyModelOcclusionMapProgram.AssertInUse();
			this._gameInstance.Engine.Graphics.GL.AssertTextureBound(GL.TEXTURE0, this._gameInstance.MapModule.TextureAtlas.GLTexture);
			this._gameInstance.Engine.Graphics.GL.AssertTextureBound(GL.TEXTURE1, this._gameInstance.EntityStoreModule.TextureAtlas.GLTexture);
			blockyModelOcclusionMapProgram.DrawId.SetValue(-1);
			blockyModelOcclusionMapProgram.ModelMatrix.SetValue(ref this._modelMatrix);
			float num = base.Hitbox.Max.Y * 64f;
			blockyModelOcclusionMapProgram.InvModelHeight.SetValue(1f / num);
			blockyModelOcclusionMapProgram.Time.SetValue(this._gameInstance.SceneRenderer.Data.Time);
			blockyModelOcclusionMapProgram.ModelVFXAnimationProgress.SetValue(this.ModelVFX.AnimationProgress);
			blockyModelOcclusionMapProgram.ModelVFXId.SetValue(this.ModelVFX.IdInTBO);
			AnimationSystem animationSystem = this._gameInstance.Engine.AnimationSystem;
			blockyModelOcclusionMapProgram.NodeBlock.SetBufferRange(animationSystem.NodeBuffer, base.ModelRenderer.NodeBufferOffset, (uint)(base.ModelRenderer.NodeCount * 64));
			base.ModelRenderer.Draw();
			for (int i = 0; i < this._itemModelRendererCount; i++)
			{
				blockyModelOcclusionMapProgram.ModelMatrix.SetValue(ref this._itemModelDrawTasks[i].ModelMatrix);
				blockyModelOcclusionMapProgram.NodeBlock.SetBufferRange(animationSystem.NodeBuffer, this.EntityItems[i].ModelRenderer.NodeBufferOffset, (uint)(this.EntityItems[i].ModelRenderer.NodeCount * 64));
				this.EntityItems[i].ModelRenderer.Draw();
			}
		}

		// Token: 0x06004A45 RID: 19013 RVA: 0x0012E44C File Offset: 0x0012C64C
		public void DrawOccluders(SceneView cameraSceneView)
		{
			GraphicsDevice graphics = this._gameInstance.Engine.Graphics;
			ref SceneRenderer.SceneData ptr = ref this._gameInstance.SceneRenderer.Data;
			bool isFirstPerson = this._gameInstance.CameraModule.Controller.IsFirstPerson;
			if (isFirstPerson)
			{
				bool flag = this.FirstPersonViewNeedsDrawing();
				if (flag)
				{
					this.PrepareForDrawInFirstPersonView();
					BlockyModelProgram blockyModelProgram = this.GetBlockyModelProgram();
					graphics.GL.UseProgram(blockyModelProgram);
					blockyModelProgram.ViewProjectionMatrix.SetValue(ref ptr.FirstPersonProjectionMatrix);
					this.DrawInFirstPersonView();
				}
			}
			else
			{
				this.PrepareForDrawInOcclusionMap(cameraSceneView);
				ZOnlyBlockyModelProgram blockyModelOcclusionMapProgram = graphics.GPUProgramStore.BlockyModelOcclusionMapProgram;
				graphics.GL.UseProgram(blockyModelOcclusionMapProgram);
				blockyModelOcclusionMapProgram.ViewProjectionMatrix.SetValue(ref ptr.ViewRotationProjectionMatrix);
				this.DrawInOcclusionMap();
			}
		}

		// Token: 0x06004A46 RID: 19014 RVA: 0x0012E51C File Offset: 0x0012C71C
		protected override void ApplyItemAppearanceConditionParticles(ClientItemBase item, int itemEntityIndex, ClientItemAppearanceCondition condition, ClientItemAppearanceCondition.Data data, bool firstPerson)
		{
			base.ApplyItemAppearanceConditionParticles(item, itemEntityIndex, condition, data, firstPerson);
			bool isFirstPerson = this._gameInstance.CameraModule.Controller.IsFirstPerson;
			for (int i = 0; i < data.EntityParticles.Length; i++)
			{
				Entity.EntityParticle entityParticle = data.EntityParticles[i];
				if (entityParticle != null)
				{
					entityParticle.ParticleSystemProxy.SetFirstPerson(isFirstPerson);
				}
			}
		}

		// Token: 0x06004A47 RID: 19015 RVA: 0x0012E584 File Offset: 0x0012C784
		public override ref ClientMovementStates GetRelativeMovementStates()
		{
			return ref this._gameInstance.CharacterControllerModule.MovementController.MovementStates;
		}

		// Token: 0x170011EB RID: 4587
		// (get) Token: 0x06004A48 RID: 19016 RVA: 0x0012E5AC File Offset: 0x0012C7AC
		public bool HasStaminaDepletedEffect
		{
			get
			{
				this.UpdateStatsDependentOnChanges();
				return this._hasStaminaDepletedEffect;
			}
		}

		// Token: 0x170011EC RID: 4588
		// (get) Token: 0x06004A49 RID: 19017 RVA: 0x0012E5CC File Offset: 0x0012C7CC
		public float HorizontalSpeedMultiplier
		{
			get
			{
				this.UpdateStatsDependentOnChanges();
				return this._horizontalSpeedMultiplier;
			}
		}

		// Token: 0x170011ED RID: 4589
		// (get) Token: 0x06004A4A RID: 19018 RVA: 0x0012E5EC File Offset: 0x0012C7EC
		public bool DisableForward
		{
			get
			{
				this.UpdateStatsDependentOnChanges();
				return this._movementEffects.DisableForward;
			}
		}

		// Token: 0x170011EE RID: 4590
		// (get) Token: 0x06004A4B RID: 19019 RVA: 0x0012E610 File Offset: 0x0012C810
		public bool DisableBackward
		{
			get
			{
				this.UpdateStatsDependentOnChanges();
				return this._movementEffects.DisableBackward;
			}
		}

		// Token: 0x170011EF RID: 4591
		// (get) Token: 0x06004A4C RID: 19020 RVA: 0x0012E634 File Offset: 0x0012C834
		public bool DisableLeft
		{
			get
			{
				this.UpdateStatsDependentOnChanges();
				return this._movementEffects.DisableLeft;
			}
		}

		// Token: 0x170011F0 RID: 4592
		// (get) Token: 0x06004A4D RID: 19021 RVA: 0x0012E658 File Offset: 0x0012C858
		public bool DisableRight
		{
			get
			{
				this.UpdateStatsDependentOnChanges();
				return this._movementEffects.DisableRight;
			}
		}

		// Token: 0x170011F1 RID: 4593
		// (get) Token: 0x06004A4E RID: 19022 RVA: 0x0012E67C File Offset: 0x0012C87C
		public bool DisableSprint
		{
			get
			{
				this.UpdateStatsDependentOnChanges();
				return this._movementEffects.DisableSprint;
			}
		}

		// Token: 0x170011F2 RID: 4594
		// (get) Token: 0x06004A4F RID: 19023 RVA: 0x0012E6A0 File Offset: 0x0012C8A0
		public bool DisableJump
		{
			get
			{
				this.UpdateStatsDependentOnChanges();
				return this._movementEffects.DisableJump;
			}
		}

		// Token: 0x170011F3 RID: 4595
		// (get) Token: 0x06004A50 RID: 19024 RVA: 0x0012E6C4 File Offset: 0x0012C8C4
		public bool DisableCrouch
		{
			get
			{
				this.UpdateStatsDependentOnChanges();
				return this._movementEffects.DisableCrouch;
			}
		}

		// Token: 0x170011F4 RID: 4596
		// (get) Token: 0x06004A51 RID: 19025 RVA: 0x0012E6E8 File Offset: 0x0012C8E8
		public HashSet<InteractionType> DisabledAbilities
		{
			get
			{
				this.UpdateStatsDependentOnChanges();
				return this._disabledAbilities;
			}
		}

		// Token: 0x06004A52 RID: 19026 RVA: 0x0012E708 File Offset: 0x0012C908
		public void UpdateActiveInteraction(int id, bool remove)
		{
			if (remove)
			{
				this._activeInteractions.Remove(id);
			}
			else
			{
				this._activeInteractions.Add(id);
			}
			this._effectsOnEntityDirty = true;
		}

		// Token: 0x06004A53 RID: 19027 RVA: 0x0012E744 File Offset: 0x0012C944
		private void UpdateStatsDependentOnChanges()
		{
			bool flag = !this._effectsOnEntityDirty;
			if (!flag)
			{
				this._movementEffects.DisableForward = false;
				this._movementEffects.DisableBackward = false;
				this._movementEffects.DisableLeft = false;
				this._movementEffects.DisableRight = false;
				this._movementEffects.DisableSprint = false;
				this._movementEffects.DisableJump = false;
				this._movementEffects.DisableCrouch = false;
				this._disabledAbilities = new HashSet<InteractionType>();
				this.UpdateStatsDependentUponInteractionsIfNecessary();
				this.UpdateStatsDependentUponEffectsIfNecessary();
				this._effectsOnEntityDirty = false;
			}
		}

		// Token: 0x06004A54 RID: 19028 RVA: 0x0012E7D8 File Offset: 0x0012C9D8
		private void UpdateStatsDependentUponInteractionsIfNecessary()
		{
			foreach (int num in this._activeInteractions)
			{
				ClientInteraction clientInteraction = this._gameInstance.InteractionModule.Interactions[num];
				Interaction interaction = (clientInteraction != null) ? clientInteraction.Interaction : null;
				MovementEffects movementEffects_ = interaction.Effects.MovementEffects_;
				this._movementEffects.DisableForward |= movementEffects_.DisableForward;
				this._movementEffects.DisableBackward |= movementEffects_.DisableBackward;
				this._movementEffects.DisableLeft |= movementEffects_.DisableLeft;
				this._movementEffects.DisableRight |= movementEffects_.DisableRight;
				this._movementEffects.DisableSprint |= movementEffects_.DisableSprint;
				this._movementEffects.DisableJump |= movementEffects_.DisableJump;
				this._movementEffects.DisableCrouch |= movementEffects_.DisableCrouch;
			}
		}

		// Token: 0x06004A55 RID: 19029 RVA: 0x0012E904 File Offset: 0x0012CB04
		private void UpdateStatsDependentUponEffectsIfNecessary()
		{
			float num = 1f;
			bool hasStaminaDepletedEffect = false;
			int num2 = this._gameInstance.EntityStoreModule.EntityEffectIndicesByIds["Stamina_Broken"];
			for (int i = 0; i < this.EntityEffects.Length; i++)
			{
				ref Entity.UniqueEntityEffect ptr = ref this.EntityEffects[i];
				bool isExpiring = ptr.IsExpiring;
				if (!isExpiring)
				{
					bool flag = ptr.NetworkEffectIndex == num2;
					if (flag)
					{
						hasStaminaDepletedEffect = true;
					}
					EntityEffect entityEffect = this._gameInstance.EntityStoreModule.EntityEffects[ptr.NetworkEffectIndex];
					bool flag2 = entityEffect.ApplicationEffects_ == null;
					if (!flag2)
					{
						num *= entityEffect.ApplicationEffects_.HorizontalSpeedMultiplier;
						MovementEffects movementEffects_ = entityEffect.ApplicationEffects_.MovementEffects_;
						bool flag3 = movementEffects_ != null;
						if (flag3)
						{
							this._movementEffects.DisableForward |= movementEffects_.DisableForward;
							this._movementEffects.DisableBackward |= movementEffects_.DisableBackward;
							this._movementEffects.DisableLeft |= movementEffects_.DisableLeft;
							this._movementEffects.DisableRight |= movementEffects_.DisableRight;
							this._movementEffects.DisableSprint |= movementEffects_.DisableSprint;
							this._movementEffects.DisableJump |= movementEffects_.DisableJump;
							this._movementEffects.DisableCrouch |= movementEffects_.DisableCrouch;
						}
						EntityEffect.ApplicationEffects.AbilityEffects abilityEffects_ = entityEffect.ApplicationEffects_.AbilityEffects_;
						bool flag4 = ((abilityEffects_ != null) ? abilityEffects_.Disabled : null) != null;
						if (flag4)
						{
							foreach (InteractionType interactionType in abilityEffects_.Disabled)
							{
								this._disabledAbilities.Add(interactionType);
							}
						}
					}
				}
			}
			this._horizontalSpeedMultiplier = num;
			this._hasStaminaDepletedEffect = hasStaminaDepletedEffect;
		}

		// Token: 0x06004A56 RID: 19030 RVA: 0x0012EAFC File Offset: 0x0012CCFC
		public void OnSpeedMultipliersChanged(float multiplierDiff)
		{
			bool flag = this._currentAnimationId == null;
			if (!flag)
			{
				float speed = (base.GetAnimation(this._currentAnimationId) ?? EntityAnimation.Empty).Speed;
				base.ModelRenderer.SetSlotAnimationSpeedMultiplier(0, speed + speed * multiplierDiff);
				float speed2 = base.GetItemAnimation(base.PrimaryItem, this._currentAnimationId, true).Speed;
				base.ModelRenderer.SetSlotAnimationSpeedMultiplier(1, speed2 + speed2 * multiplierDiff);
				float speed3 = base.GetItemAnimation(base.SecondaryItem, this._currentAnimationId, false).Speed;
				base.ModelRenderer.SetSlotAnimationSpeedMultiplier(2, speed3 + speed3 * multiplierDiff);
			}
		}

		// Token: 0x06004A57 RID: 19031 RVA: 0x0012EBA0 File Offset: 0x0012CDA0
		public void SetFpAnimation(string animationId, EntityAnimation targetAnimation)
		{
			int slotIndex = 5;
			bool looping = targetAnimation.Looping;
			BlockyAnimation firstPersonAnimation = this.GetFirstPersonAnimation(targetAnimation);
			this._firstPersonModelRenderer.SetSlotAnimation(slotIndex, firstPersonAnimation, looping, 1f, 0f, 12f, targetAnimation.PullbackConfig, false);
			this.CurrentFirstPersonAnimationId = animationId;
		}

		// Token: 0x06004A58 RID: 19032 RVA: 0x0012EBEC File Offset: 0x0012CDEC
		public bool IsApproachingObstacle(out float distance)
		{
			distance = this._firstPersonObstacleDistance;
			return distance >= 0f;
		}

		// Token: 0x06004A59 RID: 19033 RVA: 0x0012EC14 File Offset: 0x0012CE14
		private BlockyModelProgram GetBlockyModelProgram()
		{
			GPUProgramStore gpuprogramStore = this._gameInstance.Engine.Graphics.GPUProgramStore;
			return this.IsFirstPersonClipping() ? gpuprogramStore.FirstPersonClippingBlockyModelProgram : gpuprogramStore.FirstPersonBlockyModelProgram;
		}

		// Token: 0x06004A5A RID: 19034 RVA: 0x0012EC54 File Offset: 0x0012CE54
		public bool IsFirstPersonClipping()
		{
			Settings settings = this._gameInstance.App.Settings;
			bool flag = (base.PrimaryItem != null && base.PrimaryItem.ClipsGeometry) || (base.SecondaryItem != null && base.SecondaryItem.ClipsGeometry);
			return (flag && settings.ItemsClipGeometry) || (this.animationClipsGeometry && settings.ItemAnimationsClipGeometry);
		}

		// Token: 0x06004A5B RID: 19035 RVA: 0x0012ECC8 File Offset: 0x0012CEC8
		private BlockyAnimation GetFirstPersonAnimation(EntityAnimation animation)
		{
			bool flag = this._gameInstance.App.Settings.UseOverrideFirstPersonAnimations && animation.FirstPersonOverrideData != null;
			BlockyAnimation result;
			if (flag)
			{
				result = animation.FirstPersonOverrideData;
			}
			else
			{
				result = animation.FirstPersonData;
			}
			return result;
		}

		// Token: 0x06004A5C RID: 19036 RVA: 0x0012ED10 File Offset: 0x0012CF10
		public bool IsInteractionDisabled(InteractionType type)
		{
			HashSet<InteractionType> disabledAbilities = this.DisabledAbilities;
			return disabledAbilities != null && disabledAbilities.Contains(type);
		}

		// Token: 0x06004A5D RID: 19037 RVA: 0x0012ED38 File Offset: 0x0012CF38
		public void UpdateItemStatModifiers(ClientItemBase newItem, ClientItemBase newSecondaryItem)
		{
			InventoryModule inventoryModule = this._gameInstance.InventoryModule;
			bool flag = newItem != base.PrimaryItem || inventoryModule.HotbarActiveSlot != this._lastHotbarSlot;
			bool flag2 = newSecondaryItem != base.SecondaryItem || inventoryModule.UtilityActiveSlot != this._lastUtilitySlot;
			bool flag3 = flag;
			if (flag3)
			{
				this.AddWeaponStatModifiers(base.PrimaryItem, "*Weapon_");
			}
			bool flag4 = flag2;
			if (flag4)
			{
				this.AddUtilityStatModifiers(base.SecondaryItem, "*Utility_");
			}
			bool flag5;
			if (flag)
			{
				ClientItemBase primaryItem = base.PrimaryItem;
				object obj;
				if (primaryItem == null)
				{
					obj = null;
				}
				else
				{
					ItemBase.ItemWeapon weapon = primaryItem.Weapon;
					obj = ((weapon != null) ? weapon.EntityStatsToClear : null);
				}
				flag5 = (obj != null);
			}
			else
			{
				flag5 = false;
			}
			bool flag6 = flag5;
			if (flag6)
			{
				for (int i = 0; i < base.PrimaryItem.Weapon.EntityStatsToClear.Length; i++)
				{
					int index = base.PrimaryItem.Weapon.EntityStatsToClear[i];
					base.MinimizeStatValue(index);
				}
			}
			bool flag7;
			if (flag2)
			{
				ClientItemBase secondaryItem = base.SecondaryItem;
				object obj2;
				if (secondaryItem == null)
				{
					obj2 = null;
				}
				else
				{
					ItemBase.ItemUtility utility = secondaryItem.Utility;
					obj2 = ((utility != null) ? utility.EntityStatsToClear : null);
				}
				flag7 = (obj2 != null);
			}
			else
			{
				flag7 = false;
			}
			bool flag8 = flag7;
			if (flag8)
			{
				for (int j = 0; j < base.SecondaryItem.Utility.EntityStatsToClear.Length; j++)
				{
					int index2 = base.SecondaryItem.Utility.EntityStatsToClear[j];
					base.MinimizeStatValue(index2);
				}
			}
			this._lastHotbarSlot = inventoryModule.HotbarActiveSlot;
			this._lastUtilitySlot = inventoryModule.UtilityActiveSlot;
		}

		// Token: 0x06004A5E RID: 19038 RVA: 0x0012EEC8 File Offset: 0x0012D0C8
		public void AddWeaponStatModifiers(ClientItemBase item, string prefix)
		{
			Dictionary<int, Modifier[]> dictionary;
			if (item == null)
			{
				dictionary = null;
			}
			else
			{
				ItemBase.ItemWeapon weapon = item.Weapon;
				dictionary = ((weapon != null) ? weapon.StatModifiers : null);
			}
			Dictionary<int, Modifier[]> dictionary2 = dictionary;
			bool flag = dictionary2 == null;
			if (flag)
			{
				this.ClearAllStatModifiers(prefix, null);
			}
			else
			{
				this.AddItemStatModifiers(dictionary2, prefix);
			}
		}

		// Token: 0x06004A5F RID: 19039 RVA: 0x0012EF0C File Offset: 0x0012D10C
		public void AddUtilityStatModifiers(ClientItemBase item, string prefix)
		{
			Dictionary<int, Modifier[]> dictionary;
			if (item == null)
			{
				dictionary = null;
			}
			else
			{
				ItemBase.ItemUtility utility = item.Utility;
				dictionary = ((utility != null) ? utility.StatModifiers : null);
			}
			Dictionary<int, Modifier[]> dictionary2 = dictionary;
			bool flag = dictionary2 == null;
			if (flag)
			{
				this.ClearAllStatModifiers(prefix, null);
			}
			else
			{
				this.AddItemStatModifiers(dictionary2, prefix);
			}
		}

		// Token: 0x06004A60 RID: 19040 RVA: 0x0012EF50 File Offset: 0x0012D150
		private void AddItemStatModifiers(Dictionary<int, Modifier[]> itemStatModifiers, string prefix)
		{
			foreach (KeyValuePair<int, Modifier[]> keyValuePair in itemStatModifiers)
			{
				int num = 0;
				int key = keyValuePair.Key;
				int i = 0;
				while (i < keyValuePair.Value.Length)
				{
					Modifier modifier = keyValuePair.Value[i];
					string key2 = string.Format("{0}{1}", prefix, num);
					num++;
					Modifier modifier2;
					bool statModifier = base.GetStatModifier(key, key2, out modifier2);
					if (!statModifier)
					{
						goto IL_6F;
					}
					bool flag = modifier2.Equals(modifier);
					if (!flag)
					{
						goto IL_6F;
					}
					IL_7C:
					i++;
					continue;
					IL_6F:
					base.PutStatModifier(key, key2, modifier);
					goto IL_7C;
				}
				this.ClearStatModifiers(key, prefix, num);
			}
			this.ClearAllStatModifiers(prefix, itemStatModifiers);
		}

		// Token: 0x06004A61 RID: 19041 RVA: 0x0012F034 File Offset: 0x0012D234
		private void ClearAllStatModifiers(string prefix, Dictionary<int, Modifier[]> excluding)
		{
			for (int i = 0; i < this._entityStats.Length; i++)
			{
				bool flag = excluding != null && excluding.ContainsKey(i);
				if (!flag)
				{
					this.ClearStatModifiers(i, prefix, 0);
				}
			}
		}

		// Token: 0x06004A62 RID: 19042 RVA: 0x0012F078 File Offset: 0x0012D278
		private void ClearStatModifiers(int statIndex, string prefix, int offset)
		{
			bool flag;
			do
			{
				string key = string.Format("{0}{1}", prefix, offset);
				offset++;
				EntityStatUpdate entityStatUpdate;
				flag = !base.RemoveModifier(statIndex, key, out entityStatUpdate);
			}
			while (!flag);
		}

		// Token: 0x040025E2 RID: 9698
		public const float PitchEdgePadding = 0.01f;

		// Token: 0x040025E3 RID: 9699
		private const float ShakeMaxHeight = 15f;

		// Token: 0x040025E4 RID: 9700
		private const float MaxShake = 0.75f;

		// Token: 0x040025E5 RID: 9701
		private const float ShakeDuration = 0.1f;

		// Token: 0x040025E6 RID: 9702
		private static readonly Vector3 defaultPullbackOffsetRight = new Vector3(-1f, 0.25f, -1.2f);

		// Token: 0x040025E7 RID: 9703
		private static readonly Vector3 defaultPullbackOffsetLeft = new Vector3(1f, 0.25f, -1.2f);

		// Token: 0x040025E8 RID: 9704
		private static readonly Vector3 defaultPullbackRotationRight = new Vector3(-2f, -0.5f, 0f);

		// Token: 0x040025E9 RID: 9705
		private static readonly Vector3 defaultPullbackRotationLeft = new Vector3(-2f, 0.5f, 0f);

		// Token: 0x040025EB RID: 9707
		private bool _needsDistortionEffect;

		// Token: 0x040025EC RID: 9708
		public const int MaxItems = 2;

		// Token: 0x040025ED RID: 9709
		private int _lastHotbarSlot = -1;

		// Token: 0x040025EE RID: 9710
		private int _lastUtilitySlot = -1;

		// Token: 0x040025EF RID: 9711
		private BlockyModel _baseFirstPersonModel;

		// Token: 0x040025F0 RID: 9712
		private PlayerEntity.ItemModelDrawTask[] _itemModelDrawTasks = new PlayerEntity.ItemModelDrawTask[2];

		// Token: 0x040025F1 RID: 9713
		private int _itemModelRendererCount = 0;

		// Token: 0x040025F2 RID: 9714
		private ModelRenderer _firstPersonModelRenderer;

		// Token: 0x040025F3 RID: 9715
		public string CurrentFirstPersonAnimationId;

		// Token: 0x040025F4 RID: 9716
		private bool _wasFirstPerson = false;

		// Token: 0x040025F5 RID: 9717
		private bool _forceFXViewSwitch = false;

		// Token: 0x040025F6 RID: 9718
		private bool _wasLookYawOffsetClockwise = true;

		// Token: 0x040025F7 RID: 9719
		private Matrix _tempMatrix;

		// Token: 0x040025F8 RID: 9720
		private Matrix _modelMatrix;

		// Token: 0x040025F9 RID: 9721
		private float _moveAngle;

		// Token: 0x040025FA RID: 9722
		private PlayerEntity.MouseWiggle _previousMouseWiggle;

		// Token: 0x040025FB RID: 9723
		private PlayerEntity.MouseWiggle _nextMouseWiggle;

		// Token: 0x040025FC RID: 9724
		private PlayerEntity.MouseWiggle _mouseWiggle;

		// Token: 0x040025FD RID: 9725
		private PlayerEntity.MovementWiggle _previousMovementWiggle;

		// Token: 0x040025FE RID: 9726
		private PlayerEntity.MovementWiggle _nextMovementWiggle;

		// Token: 0x040025FF RID: 9727
		private PlayerEntity.MovementWiggle _movementWiggle;

		// Token: 0x04002600 RID: 9728
		private MovementEffects _movementEffects = new MovementEffects();

		// Token: 0x04002601 RID: 9729
		private List<int> _activeInteractions = new List<int>();

		// Token: 0x04002602 RID: 9730
		private float _horizontalSpeedMultiplier = 1f;

		// Token: 0x04002603 RID: 9731
		private bool _hasStaminaDepletedEffect = false;

		// Token: 0x04002604 RID: 9732
		private HashSet<InteractionType> _disabledAbilities;

		// Token: 0x04002605 RID: 9733
		private bool _wasOnGround = true;

		// Token: 0x04002606 RID: 9734
		private readonly Quaternion _renderOrientationOffset = Quaternion.CreateFromYawPitchRoll(3.1415927f, 0f, 0f);

		// Token: 0x04002607 RID: 9735
		private Vector3 _renderPosition;

		// Token: 0x04002608 RID: 9736
		private Quaternion _renderOrientation;

		// Token: 0x04002609 RID: 9737
		private float _firstPersonObstacleDistance = -1f;

		// Token: 0x0400260A RID: 9738
		private float _firstPersonWeaponDrawbackStartDistance = 1.5f;

		// Token: 0x0400260B RID: 9739
		private int _firstPersonArmRightIdx;

		// Token: 0x0400260C RID: 9740
		private int _firstPersonArmLeftIdx;

		// Token: 0x0400260D RID: 9741
		private readonly HitDetection.RaycastOptions _firstPersonObstacleRaycastOptions = new HitDetection.RaycastOptions
		{
			Distance = 5f,
			IgnoreFluids = true,
			IgnoreEmptyCollisionMaterial = true
		};

		// Token: 0x0400260E RID: 9742
		private ClientItemBase _pullbackPrevPrimaryItem;

		// Token: 0x0400260F RID: 9743
		private ClientItemBase _pullbackPrevSecondaryItem;

		// Token: 0x04002610 RID: 9744
		private ClientItemPullbackConfig _pullbackPrevPrimaryAnimConfig;

		// Token: 0x04002611 RID: 9745
		private ClientItemPullbackConfig _pullbackPrevSecondaryAnimConfig;

		// Token: 0x04002612 RID: 9746
		private Vector3 _pullbackLeftOffset;

		// Token: 0x04002613 RID: 9747
		private Vector3 _pullbackLeftRotation;

		// Token: 0x04002614 RID: 9748
		private Vector3 _pullbackRightOffset;

		// Token: 0x04002615 RID: 9749
		private Vector3 _pullbackRightRotation;

		// Token: 0x04002616 RID: 9750
		private bool animationClipsGeometry;

		// Token: 0x04002617 RID: 9751
		private const string WeaponModifierPrefix = "*Weapon_";

		// Token: 0x04002618 RID: 9752
		private const string UtilityModifierPrefix = "*Utility_";

		// Token: 0x02000E41 RID: 3649
		private struct ItemModelDrawTask
		{
			// Token: 0x040045C1 RID: 17857
			public int NodeIndex;

			// Token: 0x040045C2 RID: 17858
			public Matrix ModelMatrix;
		}

		// Token: 0x02000E42 RID: 3650
		private struct MouseWiggle
		{
			// Token: 0x040045C3 RID: 17859
			public float X;

			// Token: 0x040045C4 RID: 17860
			public float Y;

			// Token: 0x040045C5 RID: 17861
			public float Pitch;

			// Token: 0x040045C6 RID: 17862
			public float Roll;
		}

		// Token: 0x02000E43 RID: 3651
		private struct MovementWiggle
		{
			// Token: 0x040045C7 RID: 17863
			public float X;

			// Token: 0x040045C8 RID: 17864
			public float Y;

			// Token: 0x040045C9 RID: 17865
			public float Z;

			// Token: 0x040045CA RID: 17866
			public float Pitch;

			// Token: 0x040045CB RID: 17867
			public float Roll;
		}
	}
}
