using System;
using HytaleClient.Core;
using HytaleClient.Data.ClientInteraction;
using HytaleClient.Data.Entities;
using HytaleClient.Data.Entities.Initializers;
using HytaleClient.Data.EntityStats;
using HytaleClient.Data.Map;
using HytaleClient.Data.UserSettings;
using HytaleClient.InGame.Modules.Camera.Controllers;
using HytaleClient.InGame.Modules.Entities;
using HytaleClient.InGame.Modules.Interaction;
using HytaleClient.Math;
using HytaleClient.Protocol;
using HytaleClient.Utils;

namespace HytaleClient.InGame.Modules.CharacterController.MountController
{
	// Token: 0x0200096E RID: 2414
	internal class MountMovementController : MovementController
	{
		// Token: 0x06004B90 RID: 19344 RVA: 0x00137CF8 File Offset: 0x00135EF8
		public MountMovementController(GameInstance gameInstance) : base(gameInstance)
		{
			this._averageFluidMovementSettings = new FluidFX.FluidFXMovementSettings();
			this.MovementEnabled = true;
		}

		// Token: 0x06004B91 RID: 19345 RVA: 0x00137D44 File Offset: 0x00135F44
		public unsafe override void Tick()
		{
			Input input = this._gameInstance.Input;
			InputBindings inputBindings = this._gameInstance.App.Settings.InputBindings;
			this._previousMovementStates = this.MountMovementStates;
			MovementController.StateFrame stateFrame = *this.CaptureStateFrame(input, inputBindings);
			this.DoTick(ref stateFrame);
		}

		// Token: 0x06004B92 RID: 19346 RVA: 0x00137D98 File Offset: 0x00135F98
		public override void PreUpdate(float timeFraction)
		{
			PlayerEntity localPlayer = this._gameInstance.LocalPlayer;
			localPlayer.UpdateClientInterpolation(timeFraction);
			Entity entity = this._gameInstance.EntityStoreModule.GetEntity(this.MountEntityId);
			if (entity != null)
			{
				entity.UpdateInterpolation(timeFraction);
			}
		}

		// Token: 0x06004B93 RID: 19347 RVA: 0x00137DDE File Offset: 0x00135FDE
		public override void ApplyKnockback(ApplyKnockback packet)
		{
		}

		// Token: 0x06004B94 RID: 19348 RVA: 0x00137DE4 File Offset: 0x00135FE4
		public override void ApplyMovementOffset(Vector3 movementOffset)
		{
			ICameraController controller = this._gameInstance.CameraModule.Controller;
			bool flag = (this.MountMovementStates.IsFlying && base.SkipHitDetectionWhenFlying) || controller.SkipCharacterPhysics;
			if (flag)
			{
				this.MoveEntities(this._gameInstance.LocalPlayer.Position - this._anchor + movementOffset);
				this.MountMovementStates.IsOnGround = (this._wasOnGround = false);
			}
			else
			{
				this._wasOnGround = this.MountMovementStates.IsOnGround;
				int num = (int)Math.Ceiling((double)(movementOffset.Length() / 0.25f));
				Vector3 offset = movementOffset / (float)num;
				for (int i = 0; i < num; i++)
				{
					this.DoMoveCycle(offset);
				}
				bool flag2 = num == 0;
				if (flag2)
				{
					this.MoveEntities(this._gameInstance.LocalPlayer.Position - this._anchor);
				}
				bool isOnGround = this.MountMovementStates.IsOnGround;
				if (isOnGround)
				{
					this._fluidJump = false;
					this._velocity.Y = 0f;
				}
			}
			this._wasFalling = this.MountMovementStates.IsFalling;
			bool canMove = controller.CanMove;
			if (canMove)
			{
				this.UpdateViewModifiers();
			}
			this.UpdateMovementStates();
		}

		// Token: 0x06004B95 RID: 19349 RVA: 0x00137F40 File Offset: 0x00136140
		public override void RequestVelocityChange(float x, float y, float z, ChangeVelocityType changeType, VelocityConfig config)
		{
			bool flag = this._gameInstance.ClientFeatureModule.IsFeatureEnabled(1) && config != null;
			if (flag)
			{
				bool flag2 = changeType == 1;
				if (flag2)
				{
					this._appliedVelocities.Clear();
				}
				this._appliedVelocities.Add(new MovementController.AppliedVelocity(new Vector3(x, y, z), config));
			}
			else
			{
				bool flag3;
				if (this._requestedVelocityChangeType != null)
				{
					ChangeVelocityType? requestedVelocityChangeType = this._requestedVelocityChangeType;
					ChangeVelocityType changeVelocityType = 0;
					flag3 = (requestedVelocityChangeType.GetValueOrDefault() == changeVelocityType & requestedVelocityChangeType != null);
				}
				else
				{
					flag3 = true;
				}
				bool flag4 = flag3;
				if (flag4)
				{
					this._requestedVelocityChangeType = new ChangeVelocityType?(changeType);
				}
				bool flag5 = changeType == 0;
				if (flag5)
				{
					this._requestedVelocity.X = this._requestedVelocity.X + x;
					this._requestedVelocity.Y = this._requestedVelocity.Y + y;
					this._requestedVelocity.Z = this._requestedVelocity.Z + z;
				}
				else
				{
					bool flag6 = changeType == 1;
					if (flag6)
					{
						this._requestedVelocity.X = x;
						this._requestedVelocity.Y = y;
						this._requestedVelocity.Z = z;
					}
				}
			}
		}

		// Token: 0x06004B96 RID: 19350 RVA: 0x00138054 File Offset: 0x00136254
		public override void VelocityChange(float x, float y, float z, ChangeVelocityType changeType, VelocityConfig config)
		{
			bool flag = this._gameInstance.ClientFeatureModule.IsFeatureEnabled(1) && config != null;
			if (flag)
			{
				bool flag2 = changeType == 1;
				if (flag2)
				{
					this._appliedVelocities.Clear();
					this._velocity = Vector3.Zero;
				}
				this._appliedVelocities.Add(new MovementController.AppliedVelocity(new Vector3(x, y, z), config));
			}
			else
			{
				bool flag3 = changeType == 0;
				if (flag3)
				{
					this._velocity.X = this._velocity.X + x;
					this._velocity.Y = this._velocity.Y + y;
					this._velocity.Z = this._velocity.Z + z;
				}
				else
				{
					bool flag4 = changeType == 1;
					if (flag4)
					{
						this._velocity.X = x;
						this._velocity.Y = y;
						this._velocity.Z = z;
					}
				}
			}
		}

		// Token: 0x06004B97 RID: 19351 RVA: 0x00138130 File Offset: 0x00136330
		public override Vector2 GetWishDirection()
		{
			return this._wishDirection;
		}

		// Token: 0x06004B98 RID: 19352 RVA: 0x00138148 File Offset: 0x00136348
		public void OnMount(MountNPC packet)
		{
			this._anchor.X = packet.AnchorX;
			this._anchor.Y = packet.AnchorY;
			this._anchor.Z = packet.AnchorZ;
			this.MountEntityId = packet.EntityId;
			this._gameInstance.LocalPlayer.IsMounting = true;
			this.MovementStates.IsMounting = true;
		}

		// Token: 0x06004B99 RID: 19353 RVA: 0x001381B4 File Offset: 0x001363B4
		public void OnDismount(bool isLocalInteraction = false)
		{
			this._anchor = Vector3.Zero;
			this._gameInstance.LocalPlayer.IsMounting = false;
			this.MovementStates.IsMounting = false;
			bool flag = !isLocalInteraction;
			if (!flag)
			{
				this._gameInstance.Connection.SendPacket(new DismountNPC());
			}
		}

		// Token: 0x06004B9A RID: 19354 RVA: 0x0013820C File Offset: 0x0013640C
		protected new ref MovementController.StateFrame CaptureStateFrame(Input input, InputBindings bindings)
		{
			bool flag = !this._stateFrames[this._stateFrameOffset].Valid;
			if (flag)
			{
				this._stateFrames[this._stateFrameOffset] = new MovementController.StateFrame(true);
			}
			ref MovementController.StateFrame ptr = ref this._stateFrames[this._stateFrameOffset];
			this._stateFrameOffset = (this._stateFrameOffset + 1) % this._stateFrames.Length;
			ptr.Input.Capture(input, bindings.Jump);
			ptr.Input.Capture(input, bindings.MoveForwards);
			ptr.Input.Capture(input, bindings.MoveBackwards);
			ptr.Input.Capture(input, bindings.StrafeLeft);
			ptr.Input.Capture(input, bindings.StrafeRight);
			ptr.Input.Capture(input, bindings.Sprint);
			ptr.Input.Capture(input, bindings.Walk);
			ptr.Input.Capture(input, bindings.Crouch);
			ptr.Input.Capture(input, bindings.FlyDown);
			ptr.Input.Capture(input, bindings.FlyUp);
			ptr.Velocity = this._velocity;
			ptr.Position = this._gameInstance.LocalPlayer.Position - this._anchor;
			ptr.MovementForceRotation = this._gameInstance.CameraModule.Controller.MovementForceRotation;
			ptr.MovementStates = this.MountMovementStates;
			return ref ptr;
		}

		// Token: 0x06004B9B RID: 19355 RVA: 0x00138390 File Offset: 0x00136590
		private void DoTick(ref MovementController.StateFrame currentFrame)
		{
			this._movementForceRotation = currentFrame.MovementForceRotation;
			ref MovementController.InputFrame ptr = ref currentFrame.Input;
			base.UpdateInputSettings();
			InputBindings inputBindings = this._gameInstance.App.Settings.InputBindings;
			ICameraController controller = this._gameInstance.CameraModule.Controller;
			bool flag = ptr.IsBindingHeld(inputBindings.MoveForwards);
			bool flag2 = ptr.IsBindingHeld(inputBindings.MoveBackwards);
			bool needToReleaseInput = this._needToReleaseInput;
			if (needToReleaseInput)
			{
				bool flag3 = !flag && !flag2;
				if (flag3)
				{
					this._needToReleaseInput = false;
				}
				bool flag4 = this._runForward && flag && !flag2;
				if (flag4)
				{
					this._needToReleaseInput = false;
				}
				bool flag5 = !this._runForward && flag2 && !flag;
				if (flag5)
				{
					this._needToReleaseInput = false;
				}
			}
			bool flag6 = controller.CanMove && this.MovementEnabled;
			bool flag7 = this._gameInstance.App.Settings.MountRequireNewInput && this._needToReleaseInput;
			if (flag7)
			{
				flag6 = false;
			}
			bool canJump = controller.CanMove && this.MovementEnabled;
			bool skipCharacterPhysics = controller.SkipCharacterPhysics;
			base.UpdateCameraSettings();
			this.CheckDoubleJumpToFly(flag6, ptr, inputBindings);
			this.HandleJumpBuffer(ptr, inputBindings);
			this.CheckOutOfStaminaSprint();
			bool flag8 = this._jumpObstacleDurationLeft > 0f;
			if (flag8)
			{
				this._jumpObstacleDurationLeft -= 0.016666668f;
			}
			bool flag9 = ptr.IsBindingHeld(inputBindings.Sprint);
			bool canRun = flag6 && flag && !flag2 && this._canStartRunning && flag9 && (this.MountMovementStates.IsFlying || !ptr.IsBindingHeld(inputBindings.Crouch)) && this._runForceDurationLeft <= 0f;
			bool flag10 = !flag9;
			if (flag10)
			{
				this._canStartRunning = true;
			}
			Entity entity = this._gameInstance.EntityStoreModule.GetEntity(this.MountEntityId);
			bool flag11 = entity == null;
			if (!flag11)
			{
				this.UpdateFluidData(entity);
				this.UpdateRunForceDuration(flag2, flag, flag9);
				this.ApplyMovementStateLogic(ptr, inputBindings, flag6, canRun, skipCharacterPhysics, entity, canJump);
				bool flag12 = this.SprintForceDurationLeft > 0f;
				if (flag12)
				{
					this.SprintForceDurationLeft -= 0.016666668f;
				}
				this.ComputeWishDirection(flag6, currentFrame.Input, inputBindings);
				this.ComputeTurnRate(currentFrame.Input, inputBindings);
				base.WishDirection = this._wishDirection;
				this.ComputeMoveForce();
				this.ApplyRequestedVelocityVector();
				bool flag13 = Math.Abs(this._velocity.X) <= 1E-07f;
				if (flag13)
				{
					this._velocity.X = 0f;
				}
				bool flag14 = Math.Abs(this._velocity.Y) <= 1E-07f;
				if (flag14)
				{
					this._velocity.Y = 0f;
				}
				bool flag15 = Math.Abs(this._velocity.Z) <= 1E-07f;
				if (flag15)
				{
					this._velocity.Z = 0f;
				}
				Vector3 velocity = this._velocity;
				this.PreviousMovementOffset = this.MovementOffset;
				this.MovementOffset = velocity;
				controller.ApplyMove(velocity * 0.016666668f);
				this._gameInstance.App.Interface.InGameView.OnCharacterControllerTicked(this.MountMovementStates);
			}
		}

		// Token: 0x06004B9C RID: 19356 RVA: 0x00138704 File Offset: 0x00136904
		private void ApplyRequestedVelocityVector()
		{
			bool flag = !this.MountMovementStates.IsFlying && this._requestedVelocityChangeType != null;
			if (flag)
			{
				ChangeVelocityType? requestedVelocityChangeType = this._requestedVelocityChangeType;
				ChangeVelocityType changeVelocityType = 0;
				bool flag2 = requestedVelocityChangeType.GetValueOrDefault() == changeVelocityType & requestedVelocityChangeType != null;
				if (flag2)
				{
					this._velocity.X = this._velocity.X + this._requestedVelocity.X * (1f - base.DefaultBlockDrag) * base.MovementSettings.VelocityResistance;
					this._velocity.Y = this._velocity.Y + this._requestedVelocity.Y;
					this._velocity.Z = this._velocity.Z + this._requestedVelocity.Z * (1f - base.DefaultBlockDrag) * base.MovementSettings.VelocityResistance;
				}
				else
				{
					bool flag3 = this._requestedVelocityChangeType.GetValueOrDefault() == 1;
					if (flag3)
					{
						this._velocity.X = this._requestedVelocity.X;
						this._velocity.Y = this._requestedVelocity.Y;
						this._velocity.Z = this._requestedVelocity.Z;
					}
				}
			}
			this._requestedVelocity.X = (this._requestedVelocity.Y = (this._requestedVelocity.Z = 0f));
			this._requestedVelocityChangeType = null;
		}

		// Token: 0x06004B9D RID: 19357 RVA: 0x00138870 File Offset: 0x00136A70
		private void CheckDoubleJumpToFly(bool canMove, MovementController.InputFrame input, InputBindings inputBindings)
		{
			bool flag = canMove && !this._wasHoldingJump && input.IsBindingHeld(inputBindings.Jump) && base.MovementSettings.CanFly;
			if (flag)
			{
				long epochMilliseconds = TimeHelper.GetEpochMilliseconds(null);
				long num = epochMilliseconds - this._jumpReleaseTime;
				bool flag2 = num < 300L;
				if (flag2)
				{
					this.MountMovementStates.IsFlying = !this.MountMovementStates.IsFlying;
					this._jumpReleaseTime = -1L;
				}
				else
				{
					this._jumpReleaseTime = epochMilliseconds;
				}
			}
		}

		// Token: 0x06004B9E RID: 19358 RVA: 0x00138904 File Offset: 0x00136B04
		private void HandleJumpBuffer(MovementController.InputFrame input, InputBindings inputBindings)
		{
			bool flag = this._jumpBufferDurationLeft > 0f;
			if (flag)
			{
				this._jumpBufferDurationLeft -= 0.016666668f;
			}
			bool flag2 = input.IsBindingHeld(inputBindings.Jump);
			bool flag3 = !this._wasHoldingJump && flag2 && (!base.MovementSettings.AutoJumpDisableJumping || (base.MovementSettings.AutoJumpDisableJumping && this._autoJumpFrame <= 0));
			if (flag3)
			{
				this._jumpBufferDurationLeft = base.MovementSettings.JumpBufferDuration;
				this._jumpInputVelocity = this._velocity.Y;
				this._jumpInputConsumed = false;
				this._jumpInputReleased = false;
			}
			else
			{
				bool flag4 = this._wasHoldingJump && !flag2;
				if (flag4)
				{
					this._jumpInputReleased = true;
				}
			}
			this._wasHoldingJump = flag2;
		}

		// Token: 0x06004B9F RID: 19359 RVA: 0x001389DC File Offset: 0x00136BDC
		private void CheckOutOfStaminaSprint()
		{
			bool flag = this.MountMovementStates.IsSprinting && this._gameInstance.LocalPlayer.GetEntityStat(DefaultEntityStats.Stamina).Value <= 0f;
			if (flag)
			{
				this.MountMovementStates.IsSprinting = false;
				this._canStartRunning = false;
			}
		}

		// Token: 0x06004BA0 RID: 19360 RVA: 0x00138A38 File Offset: 0x00136C38
		private void UpdateFluidData(Entity mountEntity)
		{
			int hitboxHeight = this.GetHitboxHeight(mountEntity);
			int num = 0;
			int i = 0;
			while (i < hitboxHeight)
			{
				ClientBlockType clientBlockType;
				bool relativeFluid = this.GetRelativeFluid(mountEntity.Position.X, mountEntity.Position.Y + (float)i, mountEntity.Position.Z, out clientBlockType);
				if (relativeFluid)
				{
					bool flag = num == 0;
					if (flag)
					{
						this._averageFluidMovementSettings.SwimUpSpeed = 0f;
						this._averageFluidMovementSettings.SwimDownSpeed = 0f;
						this._averageFluidMovementSettings.SinkSpeed = 0f;
						this._averageFluidMovementSettings.HorizontalSpeedMultiplier = 0f;
						this._averageFluidMovementSettings.FieldOfViewMultiplier = 0f;
						this._averageFluidMovementSettings.EntryVelocityMultiplier = 0f;
					}
					num++;
					FluidFX fluidFX = this._gameInstance.ServerSettings.FluidFXs[clientBlockType.FluidFXIndex];
					bool flag2 = fluidFX.MovementSettings == null;
					if (!flag2)
					{
						this._averageFluidMovementSettings.SwimUpSpeed += fluidFX.MovementSettings.SwimUpSpeed;
						this._averageFluidMovementSettings.SwimDownSpeed += fluidFX.MovementSettings.SwimDownSpeed;
						this._averageFluidMovementSettings.SinkSpeed += fluidFX.MovementSettings.SinkSpeed;
						this._averageFluidMovementSettings.HorizontalSpeedMultiplier += fluidFX.MovementSettings.HorizontalSpeedMultiplier;
						this._averageFluidMovementSettings.FieldOfViewMultiplier += fluidFX.MovementSettings.FieldOfViewMultiplier;
						this._averageFluidMovementSettings.EntryVelocityMultiplier += fluidFX.MovementSettings.EntryVelocityMultiplier;
					}
				}
				IL_19D:
				i++;
				continue;
				goto IL_19D;
			}
			this.MountMovementStates.IsInFluid = (num > 0);
			this.MountMovementStates.IsSwimming = (num == hitboxHeight);
			bool flag3 = num > 1;
			if (flag3)
			{
				this._averageFluidMovementSettings.SwimUpSpeed /= (float)num;
				this._averageFluidMovementSettings.SwimDownSpeed /= (float)num;
				this._averageFluidMovementSettings.SinkSpeed /= (float)num;
				this._averageFluidMovementSettings.HorizontalSpeedMultiplier /= (float)num;
				this._averageFluidMovementSettings.FieldOfViewMultiplier /= (float)num;
				this._averageFluidMovementSettings.EntryVelocityMultiplier /= (float)num;
			}
			bool isSwimJumping = this.MountMovementStates.IsSwimJumping;
			if (isSwimJumping)
			{
				bool flag4 = mountEntity.Position.Y <= this._swimJumpLastY;
				if (flag4)
				{
					this.MountMovementStates.IsSwimJumping = false;
				}
				this._swimJumpLastY = mountEntity.Position.Y;
			}
		}

		// Token: 0x06004BA1 RID: 19361 RVA: 0x00138CE4 File Offset: 0x00136EE4
		private int GetHitboxHeight(Entity entity)
		{
			return (int)Math.Max(Math.Ceiling((double)entity.Hitbox.GetSize().Y), 1.0);
		}

		// Token: 0x06004BA2 RID: 19362 RVA: 0x00138D20 File Offset: 0x00136F20
		private bool GetRelativeFluid(float posX, float posY, float posZ, out ClientBlockType blockTypeOut)
		{
			int worldX = (int)Math.Floor((double)posX);
			int worldY = (int)Math.Floor((double)posY);
			int worldZ = (int)Math.Floor((double)posZ);
			int block = this._gameInstance.MapModule.GetBlock(worldX, worldY, worldZ, 0);
			ClientBlockType clientBlockType = this._gameInstance.MapModule.ClientBlockTypes[block];
			bool flag = clientBlockType.FluidFXIndex != 0;
			bool result;
			if (flag)
			{
				blockTypeOut = clientBlockType;
				result = true;
			}
			else
			{
				blockTypeOut = null;
				result = false;
			}
			return result;
		}

		// Token: 0x06004BA3 RID: 19363 RVA: 0x00138D98 File Offset: 0x00136F98
		private void UpdateRunForceDuration(bool isBackwardsHeld, bool isForwardsHeld, bool isSprintingHeld)
		{
			bool flag = this._runForceDurationLeft <= 0f;
			if (!flag)
			{
				Settings settings = this._gameInstance.App.Settings;
				float num = 0.016666668f;
				bool flag2 = (this._runForward && isBackwardsHeld) || (!this._runForward && isForwardsHeld);
				if (flag2)
				{
					num *= settings.MountForcedDecelerationMultiplier;
				}
				else
				{
					bool flag3 = this._runForward && isForwardsHeld && isSprintingHeld;
					if (flag3)
					{
						num *= settings.MountForcedAccelerationMultiplier;
					}
				}
				this._runForceDurationLeft -= num;
				bool flag4 = this._runForceDurationLeft > 0f;
				if (!flag4)
				{
					bool flag5 = this._isDecelerating && (isForwardsHeld || isBackwardsHeld);
					if (flag5)
					{
						this._needToReleaseInput = true;
					}
					this._isDecelerating = false;
				}
			}
		}

		// Token: 0x06004BA4 RID: 19364 RVA: 0x00138E60 File Offset: 0x00137060
		protected void ComputeWishDirection(bool canMove, MovementController.InputFrame input, InputBindings inputBindings)
		{
			bool flag = canMove && input.IsBindingHeld(inputBindings.MoveForwards) && !input.IsBindingHeld(inputBindings.MoveBackwards);
			if (flag)
			{
				this._wishDirection.Y = MathHelper.Step(this._wishDirection.Y, 1f, base.MovementSettings.WishDirectionWeightY);
			}
			else
			{
				bool flag2 = canMove && input.IsBindingHeld(inputBindings.MoveBackwards) && !input.IsBindingHeld(inputBindings.MoveForwards);
				if (flag2)
				{
					this._wishDirection.Y = MathHelper.Step(this._wishDirection.Y, -1f, base.MovementSettings.WishDirectionWeightY);
				}
				else
				{
					this._wishDirection.Y = MathHelper.Step(this._wishDirection.Y, 0f, base.MovementSettings.WishDirectionGravityY);
				}
			}
		}

		// Token: 0x06004BA5 RID: 19365 RVA: 0x00138F48 File Offset: 0x00137148
		private void ApplyMovementStateLogic(MovementController.InputFrame input, InputBindings inputBindings, bool canMove, bool canRun, bool skipCharacterPhysics, Entity mountEntity, bool canJump)
		{
			bool flag = this.MountMovementStates.IsFlying || skipCharacterPhysics;
			if (flag)
			{
				this.ApplyFlyingMovementStateLogic(input, inputBindings, canMove, canRun);
			}
			else
			{
				bool isSwimming = this.MountMovementStates.IsSwimming;
				if (isSwimming)
				{
					this.ApplySwimmingMovementStateLogic(input, inputBindings, canRun, canJump, mountEntity);
				}
				else
				{
					this.ApplyDefaultMovementStateLogic(input, inputBindings, canMove, canRun, canJump);
				}
			}
		}

		// Token: 0x06004BA6 RID: 19366 RVA: 0x00138FAC File Offset: 0x001371AC
		private void ApplyFlyingMovementStateLogic(MovementController.InputFrame input, InputBindings inputBindings, bool canMove, bool canRun)
		{
			this.MountMovementStates.IsFalling = false;
			this.MountMovementStates.IsSprinting = canRun;
			this.MountMovementStates.IsWalking = (canMove && input.IsBindingHeld(inputBindings.Walk));
			this.MountMovementStates.IsJumping = (canMove && input.IsBindingHeld(inputBindings.Jump));
			this._fluidJump = false;
			bool flag = canMove && input.IsBindingHeld(inputBindings.FlyDown);
			if (flag)
			{
				this._velocity.Y = -base.MovementSettings.VerticalFlySpeed * this.SpeedMultiplier * this._gameInstance.CameraModule.Controller.SpeedModifier;
			}
			else
			{
				bool flag2 = canMove && input.IsBindingHeld(inputBindings.FlyUp);
				if (flag2)
				{
					this._velocity.Y = base.MovementSettings.VerticalFlySpeed * this.SpeedMultiplier * this._gameInstance.CameraModule.Controller.SpeedModifier;
					this.MountMovementStates.IsOnGround = false;
				}
				else
				{
					this._velocity.Y = 0f;
				}
			}
		}

		// Token: 0x06004BA7 RID: 19367 RVA: 0x001390D4 File Offset: 0x001372D4
		private void ApplySwimmingMovementStateLogic(MovementController.InputFrame input, InputBindings inputBindings, bool canRun, bool canJump, Entity mountEntity)
		{
			this.MountMovementStates.IsSprinting = canRun;
			this.MountMovementStates.IsJumping = (canJump && input.IsBindingHeld(inputBindings.Jump));
			bool flag = !this.MountMovementStates.IsSwimJumping;
			if (flag)
			{
				this._velocity.Y = 0f;
				bool isJumping = this.MountMovementStates.IsJumping;
				if (isJumping)
				{
					this._velocity.Y = this._averageFluidMovementSettings.SwimUpSpeed;
				}
				else
				{
					bool isCrouching = this.MountMovementStates.IsCrouching;
					if (isCrouching)
					{
						this._velocity.Y = this._averageFluidMovementSettings.SwimDownSpeed;
					}
				}
				float num = -1f;
				for (int i = 0; i <= this.GetHitboxHeight(mountEntity) + 3; i++)
				{
					ClientBlockType clientBlockType;
					bool flag2 = !this.GetRelativeFluid(mountEntity.Position.X, mountEntity.Position.Y + (float)i, mountEntity.Position.Z, out clientBlockType);
					if (flag2)
					{
						num = (float)((int)Math.Floor((double)(mountEntity.Position.Y + (float)i)));
						ClientBlockType clientBlockType2;
						bool relativeFluid = this.GetRelativeFluid(mountEntity.Position.X, mountEntity.Position.Y + (float)i - 1f, mountEntity.Position.Z, out clientBlockType2);
						if (relativeFluid)
						{
							bool flag3 = clientBlockType2.VerticalFill != clientBlockType2.MaxFillLevel;
							if (flag3)
							{
								num -= 1f - (float)clientBlockType2.VerticalFill / (float)clientBlockType2.MaxFillLevel;
							}
						}
						break;
					}
				}
				float y = mountEntity.Position.Y;
				int hitboxHeight = this.GetHitboxHeight(mountEntity);
				float num2 = num - (float)hitboxHeight * 0.7f;
				bool flag4 = num != -1f && y > num2;
				if (flag4)
				{
					float num3 = (num2 - y) * 10f;
					bool flag5 = Math.Abs(num3) > 0.1f;
					if (flag5)
					{
						this._velocity.Y = num3;
					}
				}
				bool flag6 = this._velocity.Y != 0f;
				bool flag7 = !flag6;
				if (flag7)
				{
					bool flag8 = num == -1f || y < num - (float)hitboxHeight;
					if (flag8)
					{
						this._velocity.Y = this._averageFluidMovementSettings.SinkSpeed;
					}
				}
				float num4 = y + this._velocity.Y * 0.016666668f;
				float num5 = (float)Math.Ceiling((double)((float)hitboxHeight * 0.5f));
				bool flag9 = num != -1f && num4 > y && num4 >= num - num5;
				if (flag9)
				{
					this._velocity.Y = num - num5 - y;
				}
				bool flag10 = this.MountMovementStates.IsJumping && this._velocity.Y >= 0f && Math.Abs(y - num2) < 0.2f;
				if (flag10)
				{
					this.MountMovementStates.IsSwimJumping = true;
					this._velocity.Y = base.MovementSettings.SwimJumpForce;
					this._swimJumpLastY = y;
				}
			}
			this._fluidJump = this.MountMovementStates.IsSwimJumping;
			this.MountMovementStates.IsFalling = (this._velocity.Y < 0f && !this.MountMovementStates.IsCrouching);
		}

		// Token: 0x06004BA8 RID: 19368 RVA: 0x00139434 File Offset: 0x00137634
		private void ApplyDefaultMovementStateLogic(MovementController.InputFrame input, InputBindings inputBindings, bool canMove, bool canRun, bool canJump)
		{
			this.UpdateRunForceValues(input, inputBindings, canRun);
			this.MountMovementStates.IsSprinting = (canRun && (this.MountMovementStates.IsSprinting || this.MountMovementStates.IsOnGround) && this._runForceDurationLeft <= 0f);
			this.MountMovementStates.IsWalking = (canMove && !this.MountMovementStates.IsSprinting && input.IsBindingHeld(inputBindings.Walk) && this.MountMovementStates.IsOnGround);
			bool flag = this._gameInstance.ClientFeatureModule.IsFeatureEnabled(3) && this._gameInstance.App.Settings.SprintForce;
			if (flag)
			{
				this.UpdateSprintForceValues();
			}
			this.ApplyGravity();
			bool applyMarioFallForce = base.ApplyMarioFallForce;
			if (applyMarioFallForce)
			{
				bool flag2 = this._velocity.Y > 0f && !input.IsBindingHeld(inputBindings.Jump);
				if (flag2)
				{
					this._velocity.Y = this._velocity.Y - base.MovementSettings.MarioJumpFallForce * 0.016666668f;
				}
				else
				{
					bool flag3 = this._velocity.Y <= 0f;
					if (flag3)
					{
						base.ApplyMarioFallForce = false;
					}
				}
			}
			bool flag4 = canJump && this.MountMovementStates.IsOnGround;
			if (flag4)
			{
				this.MountMovementStates.IsJumping = (this.HasJumpInputQueued() && this._wasOnGround);
				bool flag5 = base.MovementSettings.AutoJumpDisableJumping && this._autoJumpFrame > 0;
				if (flag5)
				{
					this.MountMovementStates.IsJumping = false;
				}
				this.MountMovementStates.IsFalling = false;
				bool isJumping = this.MountMovementStates.IsJumping;
				if (isJumping)
				{
					this._velocity.Y = this.ComputeJumpForce();
					this.MountMovementStates.IsOnGround = false;
					this._fluidJump = this.MountMovementStates.IsInFluid;
					this._jumpCombo = (int)MathHelper.Min((float)(this._jumpCombo + 1), 3f);
					this._jumpBufferDurationLeft = 0f;
					this._jumpInputConsumed = true;
					base.ApplyMarioFallForce = true;
				}
				else
				{
					bool flag6 = this._gameInstance.App.Settings.AutoJumpGap && this.MountMovementStates.IsSprinting && this._wishDirection.Y != 0f && this.IsGapAhead();
					if (flag6)
					{
						this._velocity.Y = this.ComputeJumpForce();
						this.MountMovementStates.IsOnGround = false;
						this._fluidJump = this.MountMovementStates.IsInFluid;
						this._jumpCombo = (int)MathHelper.Min((float)(this._jumpCombo + 1), 3f);
						this._jumpBufferDurationLeft = 0f;
						this._jumpInputConsumed = true;
						base.ApplyMarioFallForce = false;
					}
				}
			}
			bool flag7 = this._jumpCombo != 0 && ((this.MountMovementStates.IsOnGround && this._wasOnGround) || Math.Abs(this._velocity.X) <= 1E-07f || Math.Abs(this._velocity.Z) <= 1E-07f);
			if (flag7)
			{
				this._jumpCombo = 0;
			}
			this.MountMovementStates.IsSprinting = (this.MountMovementStates.IsSprinting & !this.MountMovementStates.IsCrouching);
		}

		// Token: 0x06004BA9 RID: 19369 RVA: 0x0013978C File Offset: 0x0013798C
		private void UpdateRunForceValues(MovementController.InputFrame input, InputBindings inputBindings, bool canRun)
		{
			bool flag = (this.MountMovementStates.IsSprinting && canRun) || this.MountMovementStates.IsWalking;
			if (!flag)
			{
				Settings settings = this._gameInstance.App.Settings;
				bool flag2 = settings.MountRequireNewInput && this._needToReleaseInput;
				if (!flag2)
				{
					bool flag3 = input.IsBindingHeld(inputBindings.MoveForwards);
					bool flag4 = input.IsBindingHeld(inputBindings.MoveBackwards);
					bool flag5 = flag3 ^ flag4;
					bool flag6 = flag5 && this._isDecelerating;
					if (flag6)
					{
						bool flag7 = this._runForward && flag4;
						if (flag7)
						{
							flag5 = false;
						}
						bool flag8 = !this._runForward && flag3;
						if (flag8)
						{
							flag5 = false;
						}
					}
					bool flag9 = this.MountMovementStates.IsSprinting && !canRun;
					if (flag9)
					{
						flag5 = false;
					}
					bool flag10 = flag5 == this._wasIntendingToRun;
					if (flag10)
					{
						this._wasIntendingToRun = flag5;
					}
					else
					{
						this._runForceInitialSpeed = (float)Math.Sqrt((double)(this._velocity.X * this._velocity.X) + (double)(this._velocity.Z * this._velocity.Z));
						this._wasIntendingToRun = flag5;
						bool flag11 = flag5;
						if (flag11)
						{
							this._runForward = flag3;
							this._runForceDurationLeft = (this._runForward ? settings.MountForwardsAccelerationDuration : settings.MountBackwardsAccelerationDuration);
							this._isDecelerating = false;
						}
						else
						{
							this._runForceDurationLeft = (this._runForward ? settings.MountForwardsDecelerationDuration : settings.MountBackwardsDecelerationDuration);
							this._isDecelerating = true;
						}
						bool flag12 = this._runForceDurationLeft <= 0f;
						if (flag12)
						{
							this._isDecelerating = false;
						}
					}
				}
			}
		}

		// Token: 0x06004BAA RID: 19370 RVA: 0x0013993C File Offset: 0x00137B3C
		private void UpdateSprintForceValues()
		{
			bool flag = this.MountMovementStates.IsSprinting == this._previousMovementStates.IsSprinting && this.MountMovementStates.IsInFluid == this._previousMovementStates.IsInFluid;
			if (!flag)
			{
				bool flag2 = this.MountMovementStates.IsIdle && !this._previousMovementStates.IsIdle;
				if (flag2)
				{
					this.SprintForceDurationLeft = -1f;
				}
				else
				{
					this._sprintForceInitialSpeed = (this._previousMovementStates.IsIdle ? 0f : this._lastLateralSpeed);
					bool isSprinting = this.MountMovementStates.IsSprinting;
					if (isSprinting)
					{
						bool flag3 = this._sprintForceInitialSpeed < base.MovementSettings.BaseSpeed;
						if (flag3)
						{
							this._sprintForceInitialSpeed = base.MovementSettings.BaseSpeed;
						}
						this.SprintForceDurationLeft = this._gameInstance.App.Settings.SprintAccelerationDuration;
					}
					else
					{
						this.SprintForceDurationLeft = this._gameInstance.App.Settings.SprintDecelerationDuration;
					}
					bool flag4 = this.SprintForceDurationLeft <= 0f;
					if (flag4)
					{
						this.SprintForceDurationLeft = -1f;
					}
				}
			}
		}

		// Token: 0x06004BAB RID: 19371 RVA: 0x00139A70 File Offset: 0x00137C70
		private bool HasJumpInputQueued()
		{
			bool flag = !this._jumpInputConsumed && !this._jumpInputReleased;
			bool result;
			if (flag)
			{
				result = true;
			}
			else
			{
				bool flag2 = this._jumpBufferDurationLeft > 0f && this._jumpInputVelocity <= base.MovementSettings.JumpBufferMaxYVelocity;
				result = flag2;
			}
			return result;
		}

		// Token: 0x06004BAC RID: 19372 RVA: 0x00139AD0 File Offset: 0x00137CD0
		private float ComputeJumpForce()
		{
			float jumpForce = base.MovementSettings.JumpForce;
			bool flag = this._gameInstance.GameMode != 1 || this.SpeedMultiplier <= 1f;
			float result;
			if (flag)
			{
				result = jumpForce;
			}
			else
			{
				result = jumpForce + Math.Min((this.SpeedMultiplier - 1f) * this._gameInstance.App.Settings.JumpForceSpeedMultiplierStep, this._gameInstance.App.Settings.MaxJumpForceSpeedMultiplier);
			}
			return result;
		}

		// Token: 0x06004BAD RID: 19373 RVA: 0x00139B58 File Offset: 0x00137D58
		private void ApplyGravity()
		{
			Entity entity = this._gameInstance.EntityStoreModule.GetEntity(this.MountEntityId);
			BoundingBox hitbox = entity.Hitbox;
			int num = base.MovementSettings.InvertedGravity ? 1 : -1;
			float num2 = (float)num * PhysicsMath.GetTerminalVelocity(base.MovementSettings.Mass, 0.001225f, Math.Abs((hitbox.Max.X - hitbox.Min.X) * (hitbox.Max.Z - hitbox.Min.Z)), base.MovementSettings.DragCoefficient);
			float num3 = (float)num * PhysicsMath.GetAcceleration(this._velocity.Y, num2) * 0.016666668f;
			bool flag = this._velocity.Y < num2 && num3 > 0f;
			if (flag)
			{
				this._velocity.Y = Math.Min(this._velocity.Y + num3, num2);
			}
			else
			{
				bool flag2 = this._velocity.Y > num2 && num3 < 0f;
				if (flag2)
				{
					this._velocity.Y = Math.Max(this._velocity.Y + num3, num2);
				}
			}
		}

		// Token: 0x06004BAE RID: 19374 RVA: 0x00139C90 File Offset: 0x00137E90
		private bool IsGapAhead()
		{
			Vector3 position = this._gameInstance.LocalPlayer.Position;
			Vector3 vector = new Vector3(position.X, (float)Math.Floor((double)position.Y) - 1f, position.Z);
			Vector3 vector2 = new Vector3(this._velocity.X, 0f, this._velocity.Z);
			vector2.Normalize();
			bool flag = !base.IsPositionGap(vector);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = Math.Abs(vector2.X) > Math.Abs(vector2.Z);
				if (flag2)
				{
					bool flag3 = vector2.X > 0f && Math.Abs(position.X) % 1f < 0.2f;
					if (flag3)
					{
						return false;
					}
					bool flag4 = vector2.X < 0f && Math.Abs(position.X) % 1f > 0.8f;
					if (flag4)
					{
						return false;
					}
				}
				else
				{
					bool flag5 = vector2.Z > 0f && Math.Abs(position.Z) % 1f > 0.8f;
					if (flag5)
					{
						return false;
					}
					bool flag6 = vector2.Z < 0f && Math.Abs(position.Z) % 1f < 0.2f;
					if (flag6)
					{
						return false;
					}
				}
				bool flag7 = !base.IsPositionGap(vector + vector2);
				if (flag7)
				{
					result = false;
				}
				else
				{
					bool flag8 = !base.IsPositionGap(vector + new Vector3(0f, -1f, 0f));
					result = !flag8;
				}
			}
			return result;
		}

		// Token: 0x06004BAF RID: 19375 RVA: 0x00139E60 File Offset: 0x00138060
		private void ComputeTurnRate(MovementController.InputFrame input, InputBindings inputBindings)
		{
			float num = 0f;
			bool flag = input.IsBindingHeld(inputBindings.StrafeLeft) && !input.IsBindingHeld(inputBindings.StrafeRight);
			if (flag)
			{
				num = 1f;
			}
			else
			{
				bool flag2 = input.IsBindingHeld(inputBindings.StrafeRight) && !input.IsBindingHeld(inputBindings.StrafeLeft);
				if (flag2)
				{
					num = -1f;
				}
			}
			Settings settings = this._gameInstance.App.Settings;
			float num2 = MathHelper.ConvertToNewRange(this._lastLateralSpeed, settings.MountSpeedMinTurnRate, settings.MountSpeedMaxTurnRate, settings.MountMinTurnRate, settings.MountMaxTurnRate);
			this.CameraRotation.Y = this.CameraRotation.Y + MathHelper.ToRadians(num * num2 * 0.016666668f);
		}

		// Token: 0x06004BB0 RID: 19376 RVA: 0x00139F28 File Offset: 0x00138128
		private void ComputeMoveForce()
		{
			this.LastMoveForce = Vector3.Zero;
			float num = 0f;
			float num2 = 1f;
			float num3 = 1f;
			float value = (float)Math.Sqrt((double)(this._velocity.X * this._velocity.X + this._velocity.Z * this._velocity.Z));
			bool flag = !this.MountMovementStates.IsFlying && !this.MountMovementStates.IsClimbing;
			if (flag)
			{
				num = ((!this.MountMovementStates.IsOnGround && !this.MountMovementStates.IsSwimming && this.MountMovementStates.IsFalling) ? MathHelper.ConvertToNewRange(value, base.MovementSettings.AirDragMinSpeed, base.MovementSettings.AirDragMaxSpeed, base.MovementSettings.AirDragMin, base.MovementSettings.AirDragMax) : base.DefaultBlockDrag);
				num2 = ((!this.MountMovementStates.IsOnGround && !this.MountMovementStates.IsSwimming && this.MountMovementStates.IsFalling) ? MathHelper.ConvertToNewRange(value, base.MovementSettings.AirFrictionMinSpeed, base.MovementSettings.AirFrictionMaxSpeed, base.MovementSettings.AirFrictionMax, base.MovementSettings.AirFrictionMin) : (1f - num));
				num3 = base.MovementSettings.Acceleration;
			}
			this._velocity.X = this._velocity.X * num;
			this._velocity.Z = this._velocity.Z * num;
			Vector3 movementForceRotation = this._movementForceRotation;
			Quaternion rotation = Quaternion.CreateFromYawPitchRoll(movementForceRotation.Y, this._gameInstance.CameraModule.Controller.AllowPitchControls ? movementForceRotation.X : 0f, 0f);
			Vector3 value2 = Vector3.Transform(Vector3.Forward, rotation);
			Vector3 vector = value2 * (this._isDecelerating ? ((float)(this._runForward ? 1 : -1)) : this._wishDirection.Y);
			bool flag2 = vector.LengthSquared() < 0.0001f;
			if (flag2)
			{
				this._acceleration *= num3;
			}
			else
			{
				vector.Normalize();
				float num4 = (this._gameInstance.GameMode == 1) ? (this.SpeedMultiplier * this._gameInstance.CameraModule.Controller.SpeedModifier) : 1f;
				bool flag3 = !this.MountMovementStates.IsOnGround && !this.MountMovementStates.IsSwimming;
				if (flag3)
				{
					num4 += MathHelper.ConvertToNewRange(value, base.MovementSettings.AirControlMinSpeed, base.MovementSettings.AirControlMaxSpeed, base.MovementSettings.AirControlMaxMultiplier, base.MovementSettings.AirControlMinMultiplier);
				}
				float num5 = this.GetHorizontalMoveSpeed() * num4;
				this.ComputeRunForce(ref num5);
				bool flag4 = this._gameInstance.ClientFeatureModule.IsFeatureEnabled(3) && this._gameInstance.App.Settings.SprintForce;
				if (flag4)
				{
					this.ComputeSprintForce(ref num5);
				}
				float num6 = Vector3.Dot(this._velocity, vector);
				bool flag5 = !this.MountMovementStates.IsOnGround;
				if (flag5)
				{
					Vector3 vector2 = value2 * this._wishDirection.Y;
					vector2.Normalize();
					float num7 = Vector3.Dot(this._velocity, vector2);
					bool flag6 = num7 > num6;
					if (flag6)
					{
						num6 = num7;
					}
				}
				float num8 = num5 - num6;
				bool flag7 = num8 <= 0f;
				if (!flag7)
				{
					float num9 = num5 * num2;
					bool flag8 = this._jumpObstacleDurationLeft > 0f;
					if (flag8)
					{
						num9 *= 1f - (this.MountMovementStates.IsSprinting ? base.MovementSettings.AutoJumpObstacleSprintSpeedLoss : base.MovementSettings.AutoJumpObstacleSpeedLoss);
					}
					bool flag9 = num9 > num8;
					if (flag9)
					{
						num9 = num8;
					}
					this._acceleration += ((base.MovementSettings.BaseSpeed != 0f) ? (num9 * (num5 / base.MovementSettings.BaseSpeed * num3)) : 0f);
					bool flag10 = this._acceleration > num9;
					if (flag10)
					{
						this._acceleration = num9;
					}
					vector.X *= this._acceleration;
					vector.Y *= this.GetVerticalMoveSpeed() * num4;
					vector.Z *= this._acceleration;
					this.LastMoveForce = vector;
					this._velocity += vector;
					this._lastLateralSpeed = (float)Math.Sqrt((double)(this._velocity.X * this._velocity.X) + (double)(this._velocity.Z * this._velocity.Z));
				}
			}
		}

		// Token: 0x06004BB1 RID: 19377 RVA: 0x0013A3E4 File Offset: 0x001385E4
		private void ComputeRunForce(ref float wishSpeed)
		{
			bool flag = this._runForceDurationLeft <= 0f || this.MountMovementStates.IsWalking || this.MountMovementStates.IsSprinting;
			if (!flag)
			{
				Settings settings = this._gameInstance.App.Settings;
				bool wasIntendingToRun = this._wasIntendingToRun;
				Easing.EasingType easingType;
				float num;
				if (wasIntendingToRun)
				{
					easingType = (this._runForward ? settings.MountForwardsAccelerationEasingType : settings.MountBackwardsAccelerationEasingType);
					num = (this._runForward ? settings.MountForwardsAccelerationDuration : settings.MountBackwardsAccelerationDuration);
				}
				else
				{
					easingType = (this._runForward ? settings.MountForwardsDecelerationEasingType : settings.MountBackwardsDecelerationEasingType);
					num = (this._runForward ? settings.MountForwardsDecelerationDuration : settings.MountBackwardsDecelerationDuration);
				}
				bool flag2 = num == this._runForceDurationLeft && base.MovementSettings.BaseSpeed > 0f;
				if (flag2)
				{
					float value = this._runForceInitialSpeed * num / base.MovementSettings.BaseSpeed;
					bool isDecelerating = this._isDecelerating;
					if (isDecelerating)
					{
						this._runForceDurationLeft = MathHelper.Clamp(value, 0f, num);
					}
					else
					{
						this._runForceDurationLeft = num - MathHelper.Clamp(value, 0f, num);
					}
				}
				this._runForceProgress = Easing.Ease(easingType, num - this._runForceDurationLeft, 0f, 1f, num);
				wishSpeed = this._runForceInitialSpeed + (wishSpeed - this._runForceInitialSpeed) * this._runForceProgress;
			}
		}

		// Token: 0x06004BB2 RID: 19378 RVA: 0x0013A54C File Offset: 0x0013874C
		private void ComputeSprintForce(ref float wishSpeed)
		{
			bool flag = this.SprintForceDurationLeft < 0f || this.MountMovementStates.IsWalking;
			if (!flag)
			{
				Settings settings = this._gameInstance.App.Settings;
				Easing.EasingType easingType = this.MountMovementStates.IsSprinting ? settings.SprintAccelerationEasingType : settings.SprintDecelerationEasingType;
				float num = this.MountMovementStates.IsSprinting ? settings.SprintAccelerationDuration : settings.SprintDecelerationDuration;
				bool flag2 = num == this.SprintForceDurationLeft && base.MovementSettings.BaseSpeed > 0f;
				if (flag2)
				{
					float num2 = base.MovementSettings.BaseSpeed * base.MovementSettings.ForwardSprintSpeedMultiplier - base.MovementSettings.BaseSpeed;
					float num3 = this.MovementStates.IsFlying ? base.MovementSettings.HorizontalFlySpeed : base.MovementSettings.BaseSpeed;
					float num4 = this.MovementStates.IsSprinting ? (num3 * base.MovementSettings.ForwardSprintSpeedMultiplier - this._sprintForceInitialSpeed) : (this._sprintForceInitialSpeed - num3);
					float value = num4 * num / num2;
					this.SprintForceDurationLeft = MathHelper.Clamp(value, 0f, num);
				}
				this.SprintForceProgress = Easing.Ease(easingType, num - this.SprintForceDurationLeft, 0f, 1f, num);
				wishSpeed = this._sprintForceInitialSpeed + (wishSpeed - this._sprintForceInitialSpeed) * this.SprintForceProgress;
			}
		}

		// Token: 0x06004BB3 RID: 19379 RVA: 0x0013A6C0 File Offset: 0x001388C0
		private float GetHorizontalMoveSpeed()
		{
			bool isFlying = this.MountMovementStates.IsFlying;
			float result;
			if (isFlying)
			{
				result = base.MovementSettings.HorizontalFlySpeed * (this.MountMovementStates.IsSprinting ? base.MovementSettings.ForwardSprintSpeedMultiplier : 1f);
			}
			else
			{
				Vector2 zero = Vector2.Zero;
				Vector2 zero2 = Vector2.Zero;
				bool isSprinting = this.MountMovementStates.IsSprinting;
				if (isSprinting)
				{
					zero.Y = 1.65f;
					zero2.Y = base.MovementSettings.ForwardSprintSpeedMultiplier;
				}
				else
				{
					bool isCrouching = this.MountMovementStates.IsCrouching;
					if (isCrouching)
					{
						bool flag = this._wishDirection.X != 0f;
						if (flag)
						{
							zero.X = 0.45f;
							zero2.X = base.MovementSettings.StrafeCrouchSpeedMultiplier;
						}
						bool flag2 = this._wishDirection.Y > 0f;
						if (flag2)
						{
							zero.Y = 0.55f;
							zero2.Y = base.MovementSettings.ForwardCrouchSpeedMultiplier;
						}
						else
						{
							bool flag3 = this._wishDirection.Y < 0f;
							if (flag3)
							{
								zero.Y = 0.4f;
								zero2.Y = base.MovementSettings.BackwardCrouchSpeedMultiplier;
							}
						}
					}
					else
					{
						bool isWalking = this.MountMovementStates.IsWalking;
						if (isWalking)
						{
							bool flag4 = this._wishDirection.X != 0f;
							if (flag4)
							{
								zero.X = 0.3f;
								zero2.X = base.MovementSettings.StrafeWalkSpeedMultiplier;
							}
							bool flag5 = this._wishDirection.Y > 0f;
							if (flag5)
							{
								zero.Y = 0.3f;
								zero2.Y = base.MovementSettings.ForwardWalkSpeedMultiplier;
							}
							else
							{
								bool flag6 = this._wishDirection.Y < 0f;
								if (flag6)
								{
									zero.Y = 0.3f;
									zero2.Y = base.MovementSettings.BackwardWalkSpeedMultiplier;
								}
							}
						}
						else
						{
							bool flag7 = this._wishDirection.X != 0f;
							if (flag7)
							{
								zero.X = 0.8f;
								zero2.X = base.MovementSettings.StrafeRunSpeedMultiplier;
							}
							bool flag8 = !this._isDecelerating;
							if (flag8)
							{
								bool flag9 = this._wishDirection.Y > 0f;
								if (flag9)
								{
									zero.Y = 1f;
									zero2.Y = base.MovementSettings.ForwardRunSpeedMultiplier;
								}
								else
								{
									bool flag10 = this._wishDirection.Y < 0f;
									if (flag10)
									{
										zero.Y = 0.65f;
										zero2.Y = base.MovementSettings.BackwardRunSpeedMultiplier;
									}
								}
							}
						}
					}
				}
				float num = 0f;
				bool flag11 = zero2.Y > 0f;
				if (flag11)
				{
					float num2 = zero.Y;
					num = zero2.Y;
				}
				else
				{
					bool flag12 = zero2.X > 0f;
					if (flag12)
					{
						float num2 = zero.X;
						num = zero2.X;
					}
				}
				float num3 = 1f;
				bool flag13 = this.MountMovementStates.IsJumping || this.MountMovementStates.IsFalling;
				if (flag13)
				{
					num3 = MathHelper.Lerp(base.MovementSettings.AirSpeedMultiplier, base.MovementSettings.ComboAirSpeedMultiplier, ((float)this._jumpCombo - 1f) * 0.5f);
				}
				float num4 = (this.MountMovementStates.IsInFluid || this._fluidJump) ? this._averageFluidMovementSettings.HorizontalSpeedMultiplier : 1f;
				float num5 = this._gameInstance.InteractionModule.ForEachInteraction<float>((InteractionChain chain, ClientInteraction interaction, float mul) => mul * interaction.Interaction.HorizontalSpeedMultiplier, 1f);
				float horizontalSpeedMultiplier = this._gameInstance.LocalPlayer.HorizontalSpeedMultiplier;
				float num6 = num * num3 * num4 * num5 * horizontalSpeedMultiplier;
				result = base.MovementSettings.BaseSpeed * num6;
			}
			return result;
		}

		// Token: 0x06004BB4 RID: 19380 RVA: 0x0013AAE8 File Offset: 0x00138CE8
		private float GetVerticalMoveSpeed()
		{
			bool isFlying = this.MountMovementStates.IsFlying;
			float result;
			if (isFlying)
			{
				result = base.MovementSettings.VerticalFlySpeed;
			}
			else
			{
				bool isSwimming = this.MountMovementStates.IsSwimming;
				if (isSwimming)
				{
					bool flag = this._gameInstance.LocalPlayer.LookOrientation.Pitch >= 0f;
					if (flag)
					{
						result = this._averageFluidMovementSettings.SwimUpSpeed;
					}
					else
					{
						result = this._averageFluidMovementSettings.SwimDownSpeed;
					}
				}
				else
				{
					result = 1f;
				}
			}
			return result;
		}

		// Token: 0x06004BB5 RID: 19381 RVA: 0x0013AB6C File Offset: 0x00138D6C
		private void DoMoveCycle(Vector3 offset)
		{
			Entity entity = this._gameInstance.EntityStoreModule.GetEntity(this.MountEntityId);
			bool flag = entity == null;
			if (!flag)
			{
				InputBindings inputBindings = this._gameInstance.App.Settings.InputBindings;
				Input input = this._gameInstance.Input;
				Vector3 size = entity.Hitbox.GetSize();
				Vector3 position = entity.Position;
				float num = (this.MountMovementStates.IsFlying || this.MountMovementStates.IsOnGround || this.MountMovementStates.IsSwimming) ? 0.625f : 0.15625f;
				bool flag2 = false;
				this._previousAutoJumpHeightShift = this._nextAutoJumpHeightShift;
				position.Y += offset.Y;
				HitDetection.CollisionHitData hitData;
				bool flag3 = this.CheckCollision(position, offset, HitDetection.CollisionAxis.Y, out hitData);
				bool flag4 = this.MountMovementStates.IsOnGround && offset.Y < 0f;
				if (flag4)
				{
					bool flag5 = !flag3;
					if (flag5)
					{
						this.MountMovementStates.IsOnGround = false;
					}
					else
					{
						position.Y -= offset.Y;
					}
				}
				else
				{
					bool flag6 = flag3;
					if (flag6)
					{
						bool flag7 = offset.Y <= 0f;
						if (flag7)
						{
							this.MountMovementStates.IsOnGround = true;
							position.Y = hitData.Limit.Y;
						}
						else
						{
							this.MountMovementStates.IsOnGround = false;
							this._jumpCombo = 0;
							position.Y -= offset.Y;
						}
						foreach (MovementController.AppliedVelocity appliedVelocity in this._appliedVelocities)
						{
							appliedVelocity.Velocity.Y = 0f;
						}
						this._velocity.Y = 0f;
					}
					else
					{
						this.MountMovementStates.IsOnGround = false;
					}
				}
				this.MountMovementStates.IsClimbing = false;
				this._collisionForward = (this._collisionBackward = (this._collisionLeft = (this._collisionRight = false)));
				bool flag8 = offset.X != 0f && !this.MountMovementStates.IsMantling;
				if (flag8)
				{
					position.X += offset.X;
					bool flag9 = this.CheckCollision(position, offset, HitDetection.CollisionAxis.X, out hitData);
					bool flag10 = hitData.Overlap.X > 0f;
					if (flag10)
					{
						bool flag11 = offset.Y > 0f && this._requestedVelocity.X > 0f;
						if (flag11)
						{
							this._requestedVelocity.X = 0f;
						}
						else
						{
							bool flag12 = offset.Y < 0f && this._requestedVelocity.X < 0f;
							if (flag12)
							{
								this._requestedVelocity.X = 0f;
							}
						}
						int block = this._gameInstance.MapModule.GetBlock((int)Math.Floor((double)(hitData.Limit.X + offset.X)), (int)Math.Floor((double)position.Y), (int)Math.Floor((double)position.Z), 1);
						bool flag13 = !this.MountMovementStates.IsOnGround;
						if (flag13)
						{
							float num2 = float.PositiveInfinity;
							bool flag14 = this._gameInstance.HitDetection.RaycastBlock(position, Vector3.Down, MountMovementController.FallRaycastOptions, out this._groundHit);
							if (flag14)
							{
								num2 = this._groundHit.Distance;
							}
							bool flag15 = this._gameInstance.HitDetection.RaycastBlock(entity.Position, Vector3.Down, MountMovementController.FallRaycastOptions, out this._groundHit);
							if (flag15)
							{
								num2 = Math.Min(num2, this._groundHit.Distance);
							}
							bool flag16 = num2 < 0.375f;
							if (flag16)
							{
								num = 0.625f;
							}
						}
						bool flag17 = hitData.Limit.Y > position.Y && hitData.Limit.Y - position.Y <= num;
						flag17 = this.CanJumpObstacle(flag17, hitData, position, new Vector3(hitData.Limit.X + offset.X, position.Y, position.Z), new Vector2(hitData.Limit.X + offset.X - position.X, 0f), 90f);
						bool flag18 = !this.MountMovementStates.IsClimbing && flag17 && (this.MountMovementStates.IsFlying || this.MountMovementStates.IsSwimming || offset.Y < 0f);
						if (flag18)
						{
							float y = position.Y;
							position.Y = hitData.Limit.Y;
							HitDetection.CollisionHitData collisionHitData;
							bool flag19 = this.CheckCollision(position, offset, HitDetection.CollisionAxis.X, out collisionHitData);
							if (flag19)
							{
								bool flag20 = offset.X <= 0f;
								if (flag20)
								{
									position.X = hitData.Limit.X + size.X * 0.5f + 0.0001f;
								}
								else
								{
									position.X = hitData.Limit.X - size.X * 0.5f - 0.0001f;
								}
								position.Y = y;
							}
							else
							{
								flag2 = true;
								this._autoJumpHeight = hitData.Overlap.Y;
								position.Y = hitData.Limit.Y + 0.0001f;
							}
						}
						else
						{
							bool flag21 = hitData.Overlap.X >= 0f;
							if (flag21)
							{
								this._collisionLeft = (offset.X <= 0f);
								this._collisionRight = !this._collisionLeft;
								bool collisionLeft = this._collisionLeft;
								if (collisionLeft)
								{
									position.X = hitData.Limit.X + size.X * 0.5f + 0.0001f;
								}
								else
								{
									position.X = hitData.Limit.X - size.X * 0.5f - 0.0001f;
								}
								this._velocity.X = 0f;
							}
						}
					}
					else
					{
						bool flag22 = !this.MountMovementStates.IsFlying && this.MountMovementStates.IsOnGround && input.IsBindingHeld(inputBindings.Crouch, false);
						if (flag22)
						{
							Vector3 position2 = new Vector3(position.X, position.Y - 0.625f, position.Z);
							this.CheckCollision(position2, offset, HitDetection.CollisionAxis.X, out hitData);
							bool flag23 = hitData.Overlap.Y <= 0f;
							if (flag23)
							{
								position.X -= offset.X;
							}
						}
					}
				}
				bool flag24 = offset.Z != 0f && !this.MountMovementStates.IsMantling;
				if (flag24)
				{
					position.Z += offset.Z;
					bool flag25 = this.CheckCollision(position, offset, HitDetection.CollisionAxis.Z, out hitData);
					bool flag26 = flag25;
					if (flag26)
					{
						bool flag27 = offset.Z > 0f && this._requestedVelocity.Z > 0f;
						if (flag27)
						{
							this._requestedVelocity.Z = 0f;
						}
						else
						{
							bool flag28 = offset.Z < 0f && this._requestedVelocity.Z < 0f;
							if (flag28)
							{
								this._requestedVelocity.Z = 0f;
							}
						}
						Vector3 zero = Vector3.Zero;
						bool flag29 = !this.MountMovementStates.IsOnGround;
						if (flag29)
						{
							float num3 = float.PositiveInfinity;
							bool flag30 = this._gameInstance.HitDetection.RaycastBlock(position, Vector3.Down, MountMovementController.FallRaycastOptions, out this._groundHit);
							if (flag30)
							{
								num3 = this._groundHit.Distance;
							}
							bool flag31 = this._gameInstance.HitDetection.RaycastBlock(entity.Position, Vector3.Down, MountMovementController.FallRaycastOptions, out this._groundHit);
							if (flag31)
							{
								num3 = Math.Min(num3, this._groundHit.Distance);
							}
							bool flag32 = num3 < 0.375f;
							if (flag32)
							{
								num = 0.625f;
							}
						}
						bool flag33 = hitData.Limit.Y > position.Y && hitData.Limit.Y - position.Y < num;
						flag33 = this.CanJumpObstacle(flag33, hitData, position, new Vector3(position.X, position.Y, hitData.Limit.Z + offset.Z), new Vector2(0f, hitData.Limit.Z + offset.Z - position.Z), -90f);
						bool flag34 = !this.MountMovementStates.IsClimbing && flag33 && (this.MountMovementStates.IsFlying || this.MountMovementStates.IsSwimming || offset.Y < 0f);
						if (flag34)
						{
							float y2 = position.Y;
							position.Y = hitData.Limit.Y;
							HitDetection.CollisionHitData collisionHitData;
							bool flag35 = this.CheckCollision(position, offset, HitDetection.CollisionAxis.Z, out collisionHitData);
							if (flag35)
							{
								bool flag36 = offset.Z <= 0f;
								if (flag36)
								{
									position.Z = hitData.Limit.Z + size.Z * 0.5f + 0.0001f;
								}
								else
								{
									position.Z = hitData.Limit.Z - size.Z * 0.5f - 0.0001f;
								}
								position.Y = y2;
							}
							else
							{
								flag2 = true;
								this._autoJumpHeight = hitData.Overlap.Y;
								position.Y = hitData.Limit.Y + 0.0001f;
							}
						}
						else
						{
							bool flag37 = hitData.Overlap.Z >= 0f;
							if (flag37)
							{
								this._collisionForward = (offset.Z <= 0f);
								this._collisionBackward = !this._collisionForward;
								bool collisionForward = this._collisionForward;
								if (collisionForward)
								{
									position.Z = hitData.Limit.Z + size.Z * 0.5f + 0.0001f;
								}
								else
								{
									position.Z = hitData.Limit.Z - size.Z * 0.5f - 0.0001f;
								}
								this._velocity.Z = 0f;
							}
						}
					}
					else
					{
						bool flag38 = !this.MountMovementStates.IsFlying && this.MountMovementStates.IsOnGround && input.IsBindingHeld(inputBindings.Crouch, false);
						if (flag38)
						{
							Vector3 position3 = new Vector3(position.X, position.Y - 0.625f, position.Z);
							this.CheckCollision(position3, offset, HitDetection.CollisionAxis.Z, out hitData);
							bool flag39 = hitData.Overlap.Y <= 0f;
							if (flag39)
							{
								position.Z -= offset.Z;
							}
						}
					}
				}
				bool flag40 = flag2;
				if (flag40)
				{
					float num4 = 1f / (Math.Max(Math.Abs(this._velocity.X), Math.Abs(this._velocity.Z)) * 0.25f);
					num4 = Math.Min(Math.Max(num4, 0.01f), 1.5f);
					this._autoJumpFrameCount = (int)Math.Floor((double)(20f * num4 * this._autoJumpHeight));
					bool flag41 = this._autoJumpFrameCount > 0;
					if (flag41)
					{
						this._autoJumpFrame = this._autoJumpFrameCount;
						this._nextAutoJumpHeightShift = -this._autoJumpHeight;
					}
					else
					{
						this._autoJumpFrame = 0;
						this._nextAutoJumpHeightShift = 0f;
					}
				}
				bool flag42 = this._autoJumpFrame > 0;
				if (flag42)
				{
					this._nextAutoJumpHeightShift += this._autoJumpHeight / (float)this._autoJumpFrameCount;
					this._autoJumpFrame--;
				}
				this.MoveEntities(position);
			}
		}

		// Token: 0x06004BB6 RID: 19382 RVA: 0x0013B7D8 File Offset: 0x001399D8
		private bool CanJumpObstacle(bool canReach, HitDetection.CollisionHitData hitData, Vector3 checkPos, Vector3 blockPos, Vector2 collision, float angleOffset)
		{
			bool flag = !this._gameInstance.App.Settings.AutoJumpObstacle;
			bool result;
			if (flag)
			{
				result = canReach;
			}
			else if (canReach)
			{
				result = true;
			}
			else
			{
				bool flag2 = this._jumpObstacleDurationLeft > 0f;
				if (flag2)
				{
					result = false;
				}
				else
				{
					bool flag3 = !this.MountMovementStates.IsOnGround || this.MountMovementStates.IsCrouching;
					if (flag3)
					{
						result = false;
					}
					else
					{
						bool flag4 = hitData.Limit.Y <= checkPos.Y || hitData.Limit.Y - checkPos.Y > 1f;
						if (flag4)
						{
							result = false;
						}
						else
						{
							bool flag5 = base.IsPositionGap(blockPos.X, blockPos.Y, blockPos.Z) || !base.IsPositionGap(blockPos.X, blockPos.Y + 1f, blockPos.Z);
							if (flag5)
							{
								result = false;
							}
							else
							{
								collision.Normalize();
								float num = MathHelper.WrapAngle(this.CameraRotation.Y + MathHelper.ToRadians(angleOffset));
								Vector2 value = new Vector2((float)Math.Cos((double)num), (float)Math.Sin((double)num));
								value.Normalize();
								float num2 = Vector2.Dot(value, collision);
								num2 /= value.Length() * collision.Length();
								float radians = (float)Math.Acos((double)num2);
								float num3 = MathHelper.ToDegrees(radians);
								bool flag6 = num3 > base.MovementSettings.AutoJumpObstacleMaxAngle;
								if (flag6)
								{
									result = false;
								}
								else
								{
									this._jumpObstacleDurationLeft = (this.MountMovementStates.IsSprinting ? base.MovementSettings.AutoJumpObstacleSprintEffectDuration : base.MovementSettings.AutoJumpObstacleEffectDuration);
									result = true;
								}
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06004BB7 RID: 19383 RVA: 0x0013B9A8 File Offset: 0x00139BA8
		private bool CheckCollision(Vector3 position, Vector3 moveOffset, HitDetection.CollisionAxis axis, out HitDetection.CollisionHitData hitData)
		{
			hitData = default(HitDetection.CollisionHitData);
			Entity entity = this._gameInstance.EntityStoreModule.GetEntity(this.MountEntityId);
			bool flag = entity == null;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = this.CheckCollision(position, moveOffset, entity.Hitbox, axis, out hitData);
				bool flag3 = flag2;
				if (flag3)
				{
					result = true;
				}
				else
				{
					BoundingBox hitbox = this._gameInstance.LocalPlayer.Hitbox;
					result = this.CheckCollision(position + this._anchor, moveOffset, hitbox, axis, out hitData);
				}
			}
			return result;
		}

		// Token: 0x06004BB8 RID: 19384 RVA: 0x0013BA30 File Offset: 0x00139C30
		private void MoveEntities(Vector3 position)
		{
			Entity entity = this._gameInstance.EntityStoreModule.GetEntity(this.MountEntityId);
			bool flag = entity == null;
			if (!flag)
			{
				entity.SetPosition(position);
				entity.LookOrientation = this.CameraRotation;
				entity.SetBodyOrientation(this.CameraRotation);
				entity.ServerMovementStates = this.MountMovementStates;
				this._gameInstance.Connection.SendPacket(new EntityMovement(this.MountEntityId, position.ToPositionPacket(), entity.BodyOrientation.ToDirectionPacket(), ClientMovementStatesProtocolHelper.ToPacket(ref this.MountMovementStates)));
				PlayerEntity localPlayer = this._gameInstance.LocalPlayer;
				localPlayer.SetPosition(position + this._anchor);
				localPlayer.SetBodyOrientation(this.CameraRotation);
			}
		}

		// Token: 0x06004BB9 RID: 19385 RVA: 0x0013BAF8 File Offset: 0x00139CF8
		private new bool CheckCollision(Vector3 position, Vector3 moveOffset, BoundingBox boundingBox, HitDetection.CollisionAxis axis, out HitDetection.CollisionHitData hitData)
		{
			boundingBox.Translate(new Vector3(position.X, position.Y + 0.0001f, position.Z));
			int num = (int)position.X;
			int num2 = (int)position.Y;
			int num3 = (int)position.Z;
			hitData = default(HitDetection.CollisionHitData);
			float num4 = 0f;
			for (int i = -1; i <= 2; i++)
			{
				int num5 = num2 + i;
				for (int j = -2; j <= 2; j++)
				{
					int num6 = num3 + j;
					for (int k = -2; k <= 2; k++)
					{
						int num7 = num + k;
						HitDetection.CollisionHitData collisionHitData;
						bool flag = this._gameInstance.HitDetection.CheckBlockCollision(boundingBox, new Vector3((float)num7, (float)num5, (float)num6), moveOffset, out collisionHitData);
						if (flag)
						{
							float num8 = 0f;
							switch (axis)
							{
							case HitDetection.CollisionAxis.X:
								num8 = collisionHitData.Overlap.X;
								break;
							case HitDetection.CollisionAxis.Y:
								num8 = collisionHitData.Overlap.Y;
								break;
							case HitDetection.CollisionAxis.Z:
								num8 = collisionHitData.Overlap.Z;
								break;
							}
							bool flag2 = num4 == 0f || num8 > num4;
							if (flag2)
							{
								hitData = collisionHitData;
								num4 = num8;
							}
						}
					}
				}
			}
			EntityStoreModule entityStoreModule = this._gameInstance.EntityStoreModule;
			Entity[] allEntities = entityStoreModule.GetAllEntities();
			for (int l = entityStoreModule.PlayerEntityLocalId + 1; l < entityStoreModule.GetEntitiesCount(); l++)
			{
				bool flag3 = l == this.MountEntityId;
				if (!flag3)
				{
					Entity entity = allEntities[l];
					bool flag4 = entity.HitboxCollisionConfigIndex == -1;
					if (!flag4)
					{
						bool flag5 = !entity.IsTangible();
						if (!flag5)
						{
							BoundingBox hitbox = entity.Hitbox;
							hitbox.Grow(this.EntityHitboxExpand * 2f);
							ClientHitboxCollisionConfig clientHitboxCollisionConfig = this._gameInstance.ServerSettings.HitboxCollisionConfigs[entity.HitboxCollisionConfigIndex];
							bool flag6 = clientHitboxCollisionConfig.CollisionType > ClientHitboxCollisionConfig.ClientCollisionType.Hard;
							if (!flag6)
							{
								Vector3 position2 = entity.Position;
								HitDetection.CollisionHitData collisionHitData2;
								bool flag7 = !HitDetection.CheckBoxCollision(boundingBox, hitbox, position2.X, position2.Y + 0.0001f, position2.Z, moveOffset, out collisionHitData2);
								if (!flag7)
								{
									float num9 = 0f;
									switch (axis)
									{
									case HitDetection.CollisionAxis.X:
										num9 = collisionHitData2.Overlap.X;
										break;
									case HitDetection.CollisionAxis.Y:
										num9 = collisionHitData2.Overlap.Y;
										break;
									case HitDetection.CollisionAxis.Z:
										num9 = collisionHitData2.Overlap.Z;
										break;
									}
									bool flag8 = num4 == 0f || num9 > num4;
									if (flag8)
									{
										hitData = collisionHitData2;
										hitData.HitEntity = new int?(entity.NetworkId);
										num4 = num9;
									}
									bool flag9 = this._gameInstance.EntityStoreModule.DebugInfoNeedsDrawing && !this.CollidedEntities.Contains(entity.NetworkId);
									if (flag9)
									{
										this.CollidedEntities.Add(entity.NetworkId);
									}
								}
							}
						}
					}
				}
			}
			return num4 > 0f;
		}

		// Token: 0x06004BBA RID: 19386 RVA: 0x0013BE64 File Offset: 0x0013A064
		private void UpdateViewModifiers()
		{
			Settings settings = this._gameInstance.App.Settings;
			float num = (float)settings.FieldOfView;
			bool flag = settings.SprintFovEffect && !this._gameInstance.IsBuilderModeEnabled() && (this.MountMovementStates.IsSprinting || (this.SprintForceDurationLeft > 0f && !this.MountMovementStates.IsWalking && !this.MountMovementStates.IsIdle));
			if (flag)
			{
				num *= 1f + (settings.SprintFovIntensity - 1f) * ((this.SprintForceDurationLeft > 0f) ? (this.MountMovementStates.IsSprinting ? this.SprintForceProgress : (1f - this.SprintForceProgress)) : 1f);
			}
			bool flag2 = this.MountMovementStates.IsInFluid || this._fluidJump;
			if (flag2)
			{
				num *= this._averageFluidMovementSettings.FieldOfViewMultiplier;
			}
			bool flag3 = Math.Abs(this._gameInstance.ActiveFieldOfView - num) > 1f;
			if (flag3)
			{
				float fieldOfView = MathHelper.Lerp(this._gameInstance.ActiveFieldOfView, Math.Min(num, 180f), 0.1f);
				this._gameInstance.SetFieldOfView(fieldOfView);
			}
			this._gameInstance.LocalPlayer.ApplyFirstPersonMovementItemWiggle(this._wishDirection.X * -0.5f, (float)Math.Sign(this._velocity.Y) * -0.5f, this._wishDirection.Y * 0.5f);
		}

		// Token: 0x06004BBB RID: 19387 RVA: 0x0013BFF4 File Offset: 0x0013A1F4
		private void UpdateMovementStates()
		{
			bool flag = Math.Abs(this._velocity.Y) <= 1E-07f && (this._wishDirection == Vector2.Zero || (Math.Abs(this._velocity.X) <= 1E-07f && Math.Abs(this._velocity.Z) <= 1E-07f)) && this._runForceDurationLeft <= 0f;
			if (flag)
			{
				this.MountMovementStates.IsIdle = true;
			}
			else
			{
				this.MountMovementStates.IsIdle = false;
			}
			this.MountMovementStates.IsHorizontalIdle = (this._wishDirection == Vector2.Zero && this._runForceDurationLeft <= 0f);
		}

		// Token: 0x04002737 RID: 10039
		private const float AutoJumpMaxHeight = 0.625f;

		// Token: 0x04002738 RID: 10040
		private const int DoubleJumpMaxDelay = 300;

		// Token: 0x04002739 RID: 10041
		private const float MaxCycleMovement = 0.25f;

		// Token: 0x0400273A RID: 10042
		private const int MaxJumpCombos = 3;

		// Token: 0x0400273B RID: 10043
		private const float MaxJumpComboFactor = 0.5f;

		// Token: 0x0400273C RID: 10044
		private const int SurfaceCheckPadding = 3;

		// Token: 0x0400273D RID: 10045
		private const float BaseForwardWalkSpeedMultiplier = 0.3f;

		// Token: 0x0400273E RID: 10046
		private const float BaseBackwardWalkSpeedMultiplier = 0.3f;

		// Token: 0x0400273F RID: 10047
		private const float BaseStrafeWalkSpeedMultiplier = 0.3f;

		// Token: 0x04002740 RID: 10048
		private const float BaseForwardRunSpeedMultiplier = 1f;

		// Token: 0x04002741 RID: 10049
		private const float BaseBackwardRunSpeedMultiplier = 0.65f;

		// Token: 0x04002742 RID: 10050
		private const float BaseStrafeRunSpeedMultiplier = 0.8f;

		// Token: 0x04002743 RID: 10051
		private const float BaseForwardCrouchSpeedMultiplier = 0.55f;

		// Token: 0x04002744 RID: 10052
		private const float BaseBackwardCrouchSpeedMultiplier = 0.4f;

		// Token: 0x04002745 RID: 10053
		private const float BaseStrafeCrouchSpeedMultiplier = 0.45f;

		// Token: 0x04002746 RID: 10054
		private const float BaseForwardSprintSpeedMultiplier = 1.65f;

		// Token: 0x04002747 RID: 10055
		private static readonly HitDetection.RaycastOptions FallRaycastOptions = new HitDetection.RaycastOptions
		{
			Distance = 1f,
			IgnoreEmptyCollisionMaterial = true
		};

		// Token: 0x04002748 RID: 10056
		public const string Id = "Mount";

		// Token: 0x04002749 RID: 10057
		public int MountEntityId;

		// Token: 0x0400274A RID: 10058
		public ClientMovementStates MountMovementStates = ClientMovementStates.Idle;

		// Token: 0x0400274B RID: 10059
		private readonly FluidFX.FluidFXMovementSettings _averageFluidMovementSettings;

		// Token: 0x0400274C RID: 10060
		private Vector3 _anchor;

		// Token: 0x0400274D RID: 10061
		private bool _wasOnGround;

		// Token: 0x0400274E RID: 10062
		private bool _wasFalling;

		// Token: 0x0400274F RID: 10063
		private bool _fluidJump;

		// Token: 0x04002750 RID: 10064
		private float _swimJumpLastY;

		// Token: 0x04002751 RID: 10065
		private int _jumpCombo;

		// Token: 0x04002752 RID: 10066
		private float _acceleration;

		// Token: 0x04002753 RID: 10067
		private float _lastLateralSpeed;

		// Token: 0x04002754 RID: 10068
		private int _autoJumpFrame;

		// Token: 0x04002755 RID: 10069
		private int _autoJumpFrameCount;

		// Token: 0x04002756 RID: 10070
		private float _autoJumpHeight;

		// Token: 0x04002757 RID: 10071
		private float _previousAutoJumpHeightShift;

		// Token: 0x04002758 RID: 10072
		private float _nextAutoJumpHeightShift;

		// Token: 0x04002759 RID: 10073
		private float _jumpObstacleDurationLeft;

		// Token: 0x0400275A RID: 10074
		private Vector3 _requestedVelocity;

		// Token: 0x0400275B RID: 10075
		private ChangeVelocityType? _requestedVelocityChangeType;

		// Token: 0x0400275C RID: 10076
		private HitDetection.RaycastHit _groundHit;

		// Token: 0x0400275D RID: 10077
		private long _jumpReleaseTime;

		// Token: 0x0400275E RID: 10078
		private bool _wasHoldingJump;

		// Token: 0x0400275F RID: 10079
		private bool _canStartRunning;

		// Token: 0x04002760 RID: 10080
		private float _jumpBufferDurationLeft;

		// Token: 0x04002761 RID: 10081
		private float _jumpInputVelocity;

		// Token: 0x04002762 RID: 10082
		private bool _jumpInputConsumed = true;

		// Token: 0x04002763 RID: 10083
		private bool _jumpInputReleased = true;

		// Token: 0x04002764 RID: 10084
		private float _sprintForceInitialSpeed;

		// Token: 0x04002765 RID: 10085
		private ClientMovementStates _previousMovementStates = ClientMovementStates.Idle;

		// Token: 0x04002766 RID: 10086
		private float _runForceDurationLeft;

		// Token: 0x04002767 RID: 10087
		private float _runForceInitialSpeed;

		// Token: 0x04002768 RID: 10088
		private float _runForceProgress;

		// Token: 0x04002769 RID: 10089
		private bool _runForward;

		// Token: 0x0400276A RID: 10090
		private bool _wasIntendingToRun;

		// Token: 0x0400276B RID: 10091
		private bool _isDecelerating;

		// Token: 0x0400276C RID: 10092
		private bool _needToReleaseInput;
	}
}
