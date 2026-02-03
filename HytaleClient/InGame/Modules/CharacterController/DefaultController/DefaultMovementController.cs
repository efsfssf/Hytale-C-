using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using HytaleClient.Audio;
using HytaleClient.Core;
using HytaleClient.Data.ClientInteraction;
using HytaleClient.Data.Entities;
using HytaleClient.Data.EntityStats;
using HytaleClient.Data.Items;
using HytaleClient.Data.Map;
using HytaleClient.Data.UserSettings;
using HytaleClient.InGame.Modules.Camera.Controllers;
using HytaleClient.InGame.Modules.Entities;
using HytaleClient.InGame.Modules.Interaction;
using HytaleClient.Math;
using HytaleClient.Protocol;
using HytaleClient.Utils;
using SDL2;

namespace HytaleClient.InGame.Modules.CharacterController.DefaultController
{
	// Token: 0x0200096F RID: 2415
	internal class DefaultMovementController : MovementController
	{
		// Token: 0x06004BBD RID: 19389 RVA: 0x0013C0DC File Offset: 0x0013A2DC
		public DefaultMovementController(GameInstance gameInstance) : base(gameInstance)
		{
			this._averageFluidMovementSettings = new FluidFX.FluidFXMovementSettings();
		}

		// Token: 0x06004BBE RID: 19390 RVA: 0x0013C19C File Offset: 0x0013A39C
		public unsafe override void Tick()
		{
			DefaultMovementController.<>c__DisplayClass78_0 CS$<>8__locals1 = new DefaultMovementController.<>c__DisplayClass78_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.input = this._gameInstance.Input;
			InputBindings inputBindings = this._gameInstance.App.Settings.InputBindings;
			this._previousMovementStates = this.MovementStates;
			this._previousFallingVelocity = this._velocity.Y;
			bool flag = MovementController.DebugMovementMode == MovementController.DebugMovement.RunCycle || MovementController.DebugMovementMode == MovementController.DebugMovement.RunCycleJump;
			if (flag)
			{
				CS$<>8__locals1.<Tick>g__FakeInput|0(SDL.SDL_Keycode.SDLK_w, SDL.SDL_Scancode.SDL_SCANCODE_W);
				this._gameInstance.CameraModule.OffsetLook(0f, -7.5f);
				bool flag2 = MovementController.DebugMovementMode == MovementController.DebugMovement.RunCycleJump;
				if (flag2)
				{
					CS$<>8__locals1.<Tick>g__FakeInput|0(SDL.SDL_Keycode.SDLK_SPACE, SDL.SDL_Scancode.SDL_SCANCODE_SPACE);
				}
			}
			MovementController.StateFrame stateFrame = *base.CaptureStateFrame(CS$<>8__locals1.input, inputBindings);
			bool flag3 = this._knockbackHitPosition != null;
			if (flag3)
			{
				int num = base.FindNearestStateFrame(this._knockbackHitPosition.Value, (float)(this._gameInstance.TimeModule.GetAveragePing(2) / 1000000.0));
				ref MovementController.StateFrame ptr = ref this._stateFrames[num];
				this._velocity = ptr.Velocity;
				this._gameInstance.LocalPlayer.SetPosition(this._knockbackHitPosition.Value);
				this.MovementStates = ptr.MovementStates;
				this._knockbackHitPosition = null;
				while (num != this._stateFrameOffset)
				{
					ref MovementController.StateFrame ptr2 = ref this._stateFrames[num];
					num = (this._stateFrames.Length + num + 1) % this._stateFrames.Length;
					ptr2.Position = this._gameInstance.LocalPlayer.Position;
					ptr2.Velocity = this._velocity;
					this.DoTick(ref ptr2);
				}
			}
			bool flag4 = (!this.MovementStates.IsFlying || !base.SkipHitDetectionWhenFlying) && this.IsColliding(ref stateFrame, out CS$<>8__locals1.hitData);
			if (flag4)
			{
				int? num2 = base.FindMatchingStateFrame(delegate(ref MovementController.StateFrame frame)
				{
					return !CS$<>8__locals1.<>4__this.IsColliding(ref frame, out CS$<>8__locals1.hitData);
				});
				bool flag5 = num2 == null;
				if (flag5)
				{
					Trace.WriteLine("Can't resolve. Pushing away");
					bool flag6 = CS$<>8__locals1.hitData.HitEntity != null;
					if (flag6)
					{
						int num3 = 0;
						int value = CS$<>8__locals1.hitData.HitEntity.Value;
						int num4 = 0;
						do
						{
							num4++;
							bool flag7 = num4 > 100;
							if (flag7)
							{
								break;
							}
							bool flag8 = value != CS$<>8__locals1.hitData.HitEntity.Value;
							if (flag8)
							{
								num3 = 0;
								value = CS$<>8__locals1.hitData.HitEntity.Value;
							}
							num3++;
							Entity entity = this._gameInstance.EntityStoreModule.GetEntity(CS$<>8__locals1.hitData.HitEntity.Value);
							Vector3 value2 = entity.NextPosition - entity.PreviousPosition;
							stateFrame.Position += value2 * (float)num3 * 0.016666668f;
						}
						while (this.IsColliding(ref stateFrame, out CS$<>8__locals1.hitData) && CS$<>8__locals1.hitData.HitEntity != null);
						Trace.WriteLine(string.Format("Resolved after {0} steps", num4));
					}
					this._gameInstance.LocalPlayer.SetPosition(stateFrame.Position);
				}
				else
				{
					Trace.WriteLine("Resolving collision by resim");
					int num5 = num2.Value;
					ref MovementController.StateFrame ptr3 = ref this._stateFrames[num5];
					this._velocity = ptr3.Velocity;
					this._gameInstance.LocalPlayer.SetPosition(ptr3.Position);
					this.MovementStates = ptr3.MovementStates;
					while (num5 != this._stateFrameOffset)
					{
						ref MovementController.StateFrame ptr4 = ref this._stateFrames[num5];
						num5 = (this._stateFrames.Length + num5 + 1) % this._stateFrames.Length;
						ptr4.Position = this._gameInstance.LocalPlayer.Position;
						ptr4.Velocity = this._velocity;
						this.DoTick(ref ptr4);
					}
				}
			}
			this.DoTick(ref stateFrame);
		}

		// Token: 0x06004BBF RID: 19391 RVA: 0x0013C5E0 File Offset: 0x0013A7E0
		public override void PreUpdate(float timeFraction)
		{
			base.AutoJumpHeightShift = MathHelper.Lerp(this._previousAutoJumpHeightShift, this._nextAutoJumpHeightShift, timeFraction);
			base.CrouchHeightShift = MathHelper.Lerp(this._previousCrouchHeightShift, this._nextCrouchHeightShift, timeFraction);
			bool flag = this._mantleDurationLeft > 0f;
			if (flag)
			{
				this.MantleCameraOffset.X = MathHelper.Lerp(this._previousMantleCameraOffset.X, this._nextMantleCameraOffset.X, timeFraction);
				this.MantleCameraOffset.Z = MathHelper.Lerp(this._previousMantleCameraOffset.Z, this._nextMantleCameraOffset.Z, timeFraction);
				this.MantleCameraOffset.Y = MathHelper.Lerp(this._previousMantleCameraOffset.Y, this._nextMantleCameraOffset.Y, timeFraction);
			}
			PlayerEntity localPlayer = this._gameInstance.LocalPlayer;
			localPlayer.UpdateClientInterpolation(timeFraction);
		}

		// Token: 0x06004BC0 RID: 19392 RVA: 0x0013C6BC File Offset: 0x0013A8BC
		public override void ApplyKnockback(ApplyKnockback applyKnockback)
		{
			this.RequestVelocityChange(applyKnockback.X, applyKnockback.Y, applyKnockback.Z, applyKnockback.ChangeType, null);
			this.RunningKnockbackRemainingTime = 1.5f;
			this._knockbackHitPosition = new Vector3?(new Vector3((float)applyKnockback.HitPosition.X, (float)applyKnockback.HitPosition.Y, (float)applyKnockback.HitPosition.Z));
		}

		// Token: 0x06004BC1 RID: 19393 RVA: 0x0013C72C File Offset: 0x0013A92C
		public override void ApplyMovementOffset(Vector3 movementOffset)
		{
			ICameraController controller = this._gameInstance.CameraModule.Controller;
			bool flag = (this.MovementStates.IsFlying && base.SkipHitDetectionWhenFlying) || controller.SkipCharacterPhysics;
			if (flag)
			{
				this._gameInstance.LocalPlayer.SetPosition(this._gameInstance.LocalPlayer.Position + movementOffset);
				this.MovementStates.IsOnGround = (this._wasOnGround = false);
			}
			else
			{
				this._wasOnGround = this.MovementStates.IsOnGround;
				int num = (int)Math.Ceiling((double)(movementOffset.Length() / 0.25f));
				Vector3 offset = movementOffset / (float)num;
				for (int i = 0; i < num; i++)
				{
					this.DoMoveCycle(offset);
				}
				bool flag2 = num == 0;
				if (flag2)
				{
					this._gameInstance.LocalPlayer.SetPosition(this._gameInstance.LocalPlayer.Position);
				}
				bool isOnGround = this.MovementStates.IsOnGround;
				if (isOnGround)
				{
					this._fluidJump = false;
					this._velocity.Y = 0f;
				}
			}
			this._wasFalling = this.MovementStates.IsFalling;
			bool canMove = controller.CanMove;
			if (canMove)
			{
				this.UpdateViewModifiers();
			}
			this.UpdateMovementStates();
		}

		// Token: 0x06004BC2 RID: 19394 RVA: 0x0013C884 File Offset: 0x0013AA84
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

		// Token: 0x06004BC3 RID: 19395 RVA: 0x0013C998 File Offset: 0x0013AB98
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

		// Token: 0x06004BC4 RID: 19396 RVA: 0x0013CA74 File Offset: 0x0013AC74
		public override Vector2 GetWishDirection()
		{
			Vector2 wishDirection = this._wishDirection;
			bool isClimbing = this.MovementStates.IsClimbing;
			if (isClimbing)
			{
				wishDirection.X = 0f;
			}
			return wishDirection;
		}

		// Token: 0x06004BC5 RID: 19397 RVA: 0x0013CAAC File Offset: 0x0013ACAC
		private bool IsColliding(ref MovementController.StateFrame currentFrame, out HitDetection.CollisionHitData hitData)
		{
			return this.CheckCollision(currentFrame.Position, Vector3.Zero, HitDetection.CollisionAxis.X, out hitData) || this.CheckCollision(currentFrame.Position, Vector3.Zero, HitDetection.CollisionAxis.Y, out hitData) || this.CheckCollision(currentFrame.Position, Vector3.Zero, HitDetection.CollisionAxis.Z, out hitData);
		}

		// Token: 0x06004BC6 RID: 19398 RVA: 0x0013CB00 File Offset: 0x0013AD00
		private bool CheckCollision(Vector3 position, Vector3 moveOffset, HitDetection.CollisionAxis axis, out HitDetection.CollisionHitData hitData)
		{
			BoundingBox hitbox = this._gameInstance.LocalPlayer.Hitbox;
			return base.CheckCollision(position, moveOffset, hitbox, axis, out hitData);
		}

		// Token: 0x06004BC7 RID: 19399 RVA: 0x0013CB30 File Offset: 0x0013AD30
		private void DoTick(ref MovementController.StateFrame frame)
		{
			this.CollidedEntities.Clear();
			this.MovementStates.IsEntityCollided = false;
			this._movementForceRotation = frame.MovementForceRotation;
			ref MovementController.InputFrame ptr = ref frame.Input;
			base.UpdateInputSettings();
			InputBindings inputBindings = this._gameInstance.App.Settings.InputBindings;
			ICameraController controller = this._gameInstance.CameraModule.Controller;
			bool flag = controller.CanMove && this.MovementEnabled;
			bool skipCharacterPhysics = controller.SkipCharacterPhysics;
			base.UpdateCameraSettings();
			bool flag2 = flag && !this._wasHoldingJump && ptr.IsBindingHeld(inputBindings.Jump) && base.MovementSettings.CanFly;
			if (flag2)
			{
				long epochMilliseconds = TimeHelper.GetEpochMilliseconds(null);
				long num = epochMilliseconds - this._jumpReleaseTime;
				bool flag3 = num < 300L;
				if (flag3)
				{
					this.MovementStates.IsFlying = !this.MovementStates.IsFlying;
					this._jumpReleaseTime = -1L;
				}
				else
				{
					this._jumpReleaseTime = epochMilliseconds;
				}
			}
			bool flag4 = this._jumpBufferDurationLeft > 0f;
			if (flag4)
			{
				this._jumpBufferDurationLeft -= 0.016666668f;
			}
			bool flag5 = this._gameInstance.GameMode == 1;
			bool flag6 = flag5 || !this._gameInstance.LocalPlayer.DisableJump;
			bool flag7 = flag5 || !this._gameInstance.LocalPlayer.DisableCrouch;
			bool flag8 = flag5 || !this._gameInstance.LocalPlayer.DisableSprint;
			bool flag9 = ptr.IsBindingHeld(inputBindings.Jump) && flag6;
			bool flag10 = !this._wasHoldingJump && flag9 && (!base.MovementSettings.AutoJumpDisableJumping || (base.MovementSettings.AutoJumpDisableJumping && this._autoJumpFrame <= 0)) && !this.MovementStates.IsRolling;
			if (flag10)
			{
				this._jumpBufferDurationLeft = base.MovementSettings.JumpBufferDuration;
				this._jumpInputVelocity = this._velocity.Y;
				this._jumpInputConsumed = false;
				this._jumpInputReleased = false;
			}
			else
			{
				bool flag11 = this._wasHoldingJump && !flag9;
				if (flag11)
				{
					this._jumpInputReleased = true;
				}
			}
			ClientEntityStatValue entityStat = this._gameInstance.LocalPlayer.GetEntityStat(DefaultEntityStats.Stamina);
			bool flag12 = this.MovementStates.IsSprinting && entityStat != null && entityStat.Value <= 0f;
			if (flag12)
			{
				this.MovementStates.IsSprinting = false;
				this._canStartRunning = false;
			}
			bool flag13 = this._jumpObstacleDurationLeft > 0f;
			if (flag13)
			{
				this._jumpObstacleDurationLeft -= 0.016666668f;
			}
			bool flag14 = this._mantleDurationLeft > 0f;
			if (flag14)
			{
				this.UpdateMantle();
			}
			bool flag15 = ptr.IsBindingHeld(inputBindings.Sprint);
			bool flag16 = flag && ptr.IsBindingHeld(inputBindings.MoveForwards) && this._canStartRunning && flag15 && (flag5 || !this._gameInstance.LocalPlayer.DisableSprint) && (this.MovementStates.IsFlying || !ptr.IsBindingHeld(inputBindings.Crouch));
			bool flag17 = this._gameInstance.ClientFeatureModule.IsFeatureEnabled(4);
			if (flag17)
			{
				this.UpdateSliding(flag, ref ptr);
			}
			bool flag18 = !flag15 || this.MovementStates.IsIdle;
			if (flag18)
			{
				this._canStartRunning = true;
			}
			bool flag19 = false;
			this.UpdateFluidData();
			this.UpdateSpecialBlocks();
			BoundingBox hitbox = this._gameInstance.LocalPlayer.Hitbox;
			int num2 = base.MovementSettings.InvertedGravity ? 1 : -1;
			float num3 = 1f;
			bool flag20 = this._velocity.Y < 0f && !this._gameInstance.IsBuilderModeEnabled();
			if (flag20)
			{
				num3 = this._blockUnderFeet.MovementSettings.TerminalVelocityModifier;
			}
			float num4 = num3 * (float)num2 * this.GetVerticalMoveSpeed() * PhysicsMath.GetTerminalVelocity(base.MovementSettings.Mass, 0.001225f, Math.Abs((hitbox.Max.X - hitbox.Min.X) * (hitbox.Max.Z - hitbox.Min.Z)), base.MovementSettings.DragCoefficient);
			float num5 = (float)num2 * PhysicsMath.GetAcceleration(this._velocity.Y, num4) * 0.016666668f;
			bool flag21 = this.SprintForceDurationLeft > 0f;
			if (flag21)
			{
				this.SprintForceDurationLeft -= 0.016666668f;
			}
			bool flag22 = this._slideForceDurationLeft > 0f;
			if (flag22)
			{
				this._slideForceDurationLeft -= 0.016666668f;
			}
			bool flag23 = this.MovementStates.IsFlying || skipCharacterPhysics;
			if (flag23)
			{
				this.MovementStates.IsFalling = false;
				this.MovementStates.IsSprinting = flag16;
				this.MovementStates.IsWalking = (flag && ptr.IsBindingHeld(inputBindings.Walk));
				this.MovementStates.IsJumping = (flag && ptr.IsBindingHeld(inputBindings.Jump));
				this.UpdateCrouching(flag && !this.MovementStates.IsJumping, ref ptr, ref inputBindings);
				this._fluidJump = false;
				bool flag24 = flag && ptr.IsBindingHeld(inputBindings.FlyDown);
				if (flag24)
				{
					this._velocity.Y = -base.MovementSettings.VerticalFlySpeed * this.SpeedMultiplier * this._gameInstance.CameraModule.Controller.SpeedModifier;
				}
				else
				{
					bool flag25 = flag && ptr.IsBindingHeld(inputBindings.FlyUp);
					if (flag25)
					{
						this._velocity.Y = base.MovementSettings.VerticalFlySpeed * this.SpeedMultiplier * this._gameInstance.CameraModule.Controller.SpeedModifier;
						this.MovementStates.IsOnGround = false;
					}
					else
					{
						this._velocity.Y = 0f;
					}
				}
			}
			else
			{
				bool flag26 = this.MovementStates.IsClimbing && !this.MovementStates.IsOnGround;
				if (flag26)
				{
					bool flag27 = this._gameInstance.MapModule.GetBlock((int)this._climbingBlockPosition.X, (int)this._climbingBlockPosition.Y, (int)this._climbingBlockPosition.Z, 1) != this._climbingBlockId;
					if (flag27)
					{
						this.MovementStates.IsClimbing = false;
					}
					else
					{
						this.MovementStates.IsFalling = false;
						this.MovementStates.IsSprinting = false;
						this.MovementStates.IsJumping = (flag && this.HasJumpInputQueued());
						this.UpdateCrouching(flag && !this.MovementStates.IsJumping, ref ptr, ref inputBindings);
						this._fluidJump = false;
						this._velocity.Y = 0f;
						bool flag28 = ptr.IsBindingHeld(inputBindings.StrafeLeft) || ptr.IsBindingHeld(inputBindings.StrafeRight) || ptr.IsBindingHeld(inputBindings.MoveBackwards);
						if (flag28)
						{
							flag19 = true;
						}
						else
						{
							bool isJumping = this.MovementStates.IsJumping;
							if (isJumping)
							{
								this._velocity.Y = base.MovementSettings.JumpForce;
								bool flag29 = (this._collisionForward || this._collisionBackward) && (this._collisionLeft || this._collisionRight);
								if (flag29)
								{
									this._velocity.Z = (this._velocity.X = base.MovementSettings.JumpForce * 0.25f);
								}
								else
								{
									bool flag30 = this._collisionForward || this._collisionBackward;
									if (flag30)
									{
										this._velocity.Z = base.MovementSettings.JumpForce * 0.4f;
									}
									else
									{
										this._velocity.X = base.MovementSettings.JumpForce * 0.4f;
									}
								}
								bool collisionRight = this._collisionRight;
								if (collisionRight)
								{
									this._velocity.X = this._velocity.X * -1f;
								}
								bool collisionBackward = this._collisionBackward;
								if (collisionBackward)
								{
									this._velocity.Z = this._velocity.Z * -1f;
								}
								this.MovementStates.IsClimbing = false;
								this._jumpBufferDurationLeft = 0f;
								this._jumpInputConsumed = true;
							}
						}
					}
				}
				else
				{
					bool isSwimming = this.MovementStates.IsSwimming;
					if (isSwimming)
					{
						this.MovementStates.IsSprinting = (flag16 && flag8);
						this.MovementStates.IsJumping = (flag9 && flag && ptr.IsBindingHeld(inputBindings.Jump));
						this.UpdateCrouching(flag, ref ptr, ref inputBindings);
						bool flag31 = !this.MovementStates.IsSwimJumping;
						if (flag31)
						{
							this._velocity.Y = 0f;
							bool isJumping2 = this.MovementStates.IsJumping;
							if (isJumping2)
							{
								this._velocity.Y = this._averageFluidMovementSettings.SwimUpSpeed;
							}
							else
							{
								bool isCrouching = this.MovementStates.IsCrouching;
								if (isCrouching)
								{
									this._velocity.Y = (flag7 ? this._averageFluidMovementSettings.SwimDownSpeed : 0f);
								}
							}
							float num6 = -1f;
							for (int i = 0; i <= this.GetHitboxHeight() + 3; i++)
							{
								ClientBlockType clientBlockType;
								bool flag32 = !this.GetRelativeFluid(0f, (float)i, 0f, out clientBlockType);
								if (flag32)
								{
									num6 = (float)((int)Math.Floor((double)(this._gameInstance.LocalPlayer.Position.Y + (float)i)));
									ClientBlockType clientBlockType2;
									bool relativeFluid = this.GetRelativeFluid(0f, (float)(i - 1), 0f, out clientBlockType2);
									if (relativeFluid)
									{
										bool flag33 = clientBlockType2.VerticalFill != clientBlockType2.MaxFillLevel;
										if (flag33)
										{
											num6 -= 1f - (float)clientBlockType2.VerticalFill / (float)clientBlockType2.MaxFillLevel;
										}
									}
									break;
								}
							}
							float y = this._gameInstance.LocalPlayer.Position.Y;
							int hitboxHeight = this.GetHitboxHeight();
							float num7 = num6 - (float)hitboxHeight * 0.7f;
							bool flag34 = num6 != -1f && y > num7;
							if (flag34)
							{
								float num8 = (num7 - y) * 10f;
								bool flag35 = Math.Abs(num8) > 0.1f;
								if (flag35)
								{
									this._velocity.Y = num8;
								}
							}
							bool flag36 = this._velocity.Y != 0f;
							bool flag37 = !flag36;
							if (flag37)
							{
								bool flag38 = num6 == -1f || y < num6 - (float)hitboxHeight;
								if (flag38)
								{
									this._velocity.Y = this._averageFluidMovementSettings.SinkSpeed;
								}
							}
							float num9 = y + this._velocity.Y * 0.016666668f;
							float num10 = (float)Math.Ceiling((double)((float)hitboxHeight * 0.5f));
							bool flag39 = num6 != -1f && num9 > y && num9 >= num6 - num10;
							if (flag39)
							{
								this._velocity.Y = num6 - num10 - y;
							}
							bool flag40 = this.MovementStates.IsJumping && this._velocity.Y >= 0f && Math.Abs(y - num7) < 0.2f;
							if (flag40)
							{
								this.MovementStates.IsSwimJumping = true;
								this._velocity.Y = base.MovementSettings.SwimJumpForce;
								this._swimJumpLastY = y;
							}
						}
						this._fluidJump = this.MovementStates.IsSwimJumping;
						this.MovementStates.IsFalling = (this._velocity.Y < 0f && !this.MovementStates.IsCrouching);
					}
					else
					{
						bool isMantling = this.MovementStates.IsMantling;
						if (isMantling)
						{
							this._velocity.Y = 0f;
						}
						else
						{
							this.MovementStates.IsSprinting = (flag16 && (this.MovementStates.IsSprinting || this.MovementStates.IsOnGround));
							this.MovementStates.IsWalking = (flag && !this.MovementStates.IsSprinting && ptr.IsBindingHeld(inputBindings.Walk) && this.MovementStates.IsOnGround);
							flag19 |= this.MovementStates.IsRolling;
							bool flag41 = this._gameInstance.ClientFeatureModule.IsFeatureEnabled(3) && this._gameInstance.App.Settings.SprintForce;
							if (flag41)
							{
								this.UpdateSprintForceValues();
							}
							bool flag42 = this._gameInstance.ClientFeatureModule.IsFeatureEnabled(4);
							if (flag42)
							{
								this.UpdateSlidingForceValues();
							}
							bool flag43 = this._velocity.Y < num4 && num5 > 0f;
							if (flag43)
							{
								this._velocity.Y = Math.Min(this._velocity.Y + num5, num4);
							}
							else
							{
								bool flag44 = this._velocity.Y > num4 && num5 < 0f;
								if (flag44)
								{
									this._velocity.Y = Math.Max(this._velocity.Y + num5, num4);
								}
							}
							bool applyMarioFallForce = base.ApplyMarioFallForce;
							if (applyMarioFallForce)
							{
								bool flag45 = this._velocity.Y > 0f && !ptr.IsBindingHeld(inputBindings.Jump);
								if (flag45)
								{
									this._velocity.Y = this._velocity.Y - base.MovementSettings.MarioJumpFallForce * 0.016666668f;
								}
								else
								{
									bool flag46 = this._velocity.Y <= 0f;
									if (flag46)
									{
										base.ApplyMarioFallForce = false;
									}
								}
							}
							this.CheckBounce(flag);
							bool flag47 = flag && this.MovementStates.IsOnGround;
							if (flag47)
							{
								this.MovementStates.IsJumping = (this.HasJumpInputQueued() && this._wasOnGround);
								bool flag48 = base.MovementSettings.AutoJumpDisableJumping && this._autoJumpFrame > 0;
								if (flag48)
								{
									this.MovementStates.IsJumping = false;
								}
								this.MovementStates.IsFalling = false;
								this._touchedGround = true;
								bool isJumping3 = this.MovementStates.IsJumping;
								if (isJumping3)
								{
									this._velocity.Y = this.ComputeJumpForce();
									bool flag49 = this._fallEffectDurationLeft > 0f;
									if (flag49)
									{
										this._velocity.Y = this._fallEffectJumpForce;
									}
									this.MovementStates.IsOnGround = false;
									this._fluidJump = this.MovementStates.IsInFluid;
									this._jumpCombo = (int)MathHelper.Min((float)(this._jumpCombo + 1), 3f);
									this._jumpBufferDurationLeft = 0f;
									this._jumpInputConsumed = true;
									bool flag50 = this._fallEffectDurationLeft <= 0f;
									if (flag50)
									{
										this._consecutiveMomentumLoss = 0f;
									}
									base.ApplyMarioFallForce = true;
								}
								else
								{
									bool flag51 = flag6 && this._gameInstance.App.Settings.AutoJumpGap && this.MovementStates.IsSprinting && this._wishDirection.Y != 0f && this.IsGapAhead();
									if (flag51)
									{
										this._velocity.Y = this.ComputeJumpForce();
										this.MovementStates.IsOnGround = false;
										this._fluidJump = this.MovementStates.IsInFluid;
										this._jumpCombo = (int)MathHelper.Min((float)(this._jumpCombo + 1), 3f);
										this._jumpBufferDurationLeft = 0f;
										this._jumpInputConsumed = true;
										base.ApplyMarioFallForce = false;
									}
								}
							}
							else
							{
								bool flag52 = this._gameInstance.ClientFeatureModule.IsFeatureEnabled(0) && flag && this._touchedGround && !this._wasHoldingJump;
								if (flag52)
								{
									this.MovementStates.IsJumping = this._gameInstance.Input.IsBindingDown(inputBindings.Jump, false);
									bool isJumping4 = this.MovementStates.IsJumping;
									if (isJumping4)
									{
										InventoryModule inventoryModule = this._gameInstance.InventoryModule;
										foreach (ClientItemStack clientItemStack in inventoryModule._armorInventory)
										{
											bool flag53 = clientItemStack == null;
											if (!flag53)
											{
												bool flag54 = !clientItemStack.Id.Equals("Trinket_Magic_Feather");
												if (!flag54)
												{
													this.MovementStates.IsFalling = false;
													this._touchedGround = false;
													this._gameInstance.InteractionModule.StartChain(17, InteractionModule.ClickType.Single, null);
													base.ApplyMarioFallForce = false;
												}
											}
										}
									}
								}
								Vector3 position = this._gameInstance.LocalPlayer.Position;
								position.Y += 0.1f;
								HitDetection.RaycastHit raycastHit;
								this.MovementStates.IsFalling = (this._velocity.Y < 0f && !this._gameInstance.HitDetection.RaycastBlock(position, Vector3.Down, DefaultMovementController.FallRaycastOptions, out raycastHit));
								bool isFalling = this.MovementStates.IsFalling;
								if (isFalling)
								{
									this.MovementStates.IsJumping = false;
									this.MovementStates.IsSwimJumping = false;
								}
							}
							bool flag55 = this._jumpCombo != 0 && ((this.MovementStates.IsOnGround && this._wasOnGround) || Math.Abs(this._velocity.X) <= 1E-07f || Math.Abs(this._velocity.Z) <= 1E-07f);
							if (flag55)
							{
								this._jumpCombo = 0;
							}
							this.UpdateCrouching(flag, ref ptr, ref inputBindings);
							this.MovementStates.IsSprinting = (this.MovementStates.IsSprinting & (!this.MovementStates.IsCrouching && !this.MovementStates.IsSliding));
						}
					}
				}
			}
			this._wasHoldingJump = flag9;
			Vector3 value = this.CreateRepulsionVector();
			this._velocity += value;
			base.ComputeWishDirection(flag19, flag, ptr, inputBindings);
			base.WishDirection = this._wishDirection;
			this.ComputeMoveForce();
			bool flag56 = !this.MovementStates.IsFlying && this._requestedVelocityChangeType != null;
			if (flag56)
			{
				ChangeVelocityType? requestedVelocityChangeType = this._requestedVelocityChangeType;
				ChangeVelocityType changeVelocityType = 0;
				bool flag57 = requestedVelocityChangeType.GetValueOrDefault() == changeVelocityType & requestedVelocityChangeType != null;
				if (flag57)
				{
					this._velocity.X = this._velocity.X + this._requestedVelocity.X * (1f - base.DefaultBlockDrag) * base.MovementSettings.VelocityResistance;
					this._velocity.Y = this._velocity.Y + this._requestedVelocity.Y;
					this._velocity.Z = this._velocity.Z + this._requestedVelocity.Z * (1f - base.DefaultBlockDrag) * base.MovementSettings.VelocityResistance;
				}
				else
				{
					bool flag58 = this._requestedVelocityChangeType.GetValueOrDefault() == 1;
					if (flag58)
					{
						this._velocity.X = this._requestedVelocity.X;
						this._velocity.Y = this._requestedVelocity.Y;
						this._velocity.Z = this._requestedVelocity.Z;
					}
				}
			}
			this._requestedVelocity.X = (this._requestedVelocity.Y = (this._requestedVelocity.Z = 0f));
			this._requestedVelocityChangeType = null;
			this._wasClimbing = (this.MovementStates.IsClimbing && !this.MovementStates.IsOnGround);
			bool flag59 = Math.Abs(this._velocity.X) <= 1E-07f;
			if (flag59)
			{
				this._velocity.X = 0f;
			}
			bool flag60 = Math.Abs(this._velocity.Y) <= 1E-07f;
			if (flag60)
			{
				this._velocity.Y = 0f;
			}
			bool flag61 = Math.Abs(this._velocity.Z) <= 1E-07f;
			if (flag61)
			{
				this._velocity.Z = 0f;
			}
			Vector3 vector = this._velocity;
			vector = this.ApplyExternalForces(vector);
			this.PreviousMovementOffset = this.MovementOffset;
			this.MovementOffset = vector;
			bool flag62 = this.RaycastMode == 0;
			if (flag62)
			{
				this.CheckEntityCollision(vector);
			}
			else
			{
				Vector3 movement = Vector3.CreateFromYawPitch(this._gameInstance.LocalPlayer.LookOrientation.Yaw, this._gameInstance.LocalPlayer.LookOrientation.Pitch);
				this.CheckEntityCollision(movement);
			}
			this.ComputeFallEffect();
			controller.ApplyMove(vector * 0.016666668f);
			this.RunningKnockbackRemainingTime -= 0.016666668f;
			bool flag63 = !this.MovementStates.IsOnGround && this.RunningKnockbackRemainingTime > 0f;
			if (flag63)
			{
				this.RunningKnockbackRemainingTime = 1.5f;
			}
			else
			{
				bool flag64 = this.RunningKnockbackRemainingTime <= 0f;
				if (flag64)
				{
					this.RunningKnockbackRemainingTime = 0f;
				}
			}
			float num11 = (this._fallEffectDurationLeft > 0f) ? (this._speedMultiplier * this._fallEffectSpeedMultiplier) : this._speedMultiplier;
			this.CurrentSpeedMultiplierDiff = (num11 - this._baseSpeedMultiplier) / this._baseSpeedMultiplier;
			this._gameInstance.LocalPlayer.OnSpeedMultipliersChanged(this.CurrentSpeedMultiplierDiff);
			this._gameInstance.App.Interface.InGameView.OnCharacterControllerTicked(this.MovementStates);
		}

		// Token: 0x06004BC8 RID: 19400 RVA: 0x0013E104 File Offset: 0x0013C304
		private void CheckBounce(bool canMove)
		{
			bool flag = !canMove || this._wasOnGround || !this._blockUnderFeet.MovementSettings.IsBouncy || this._gameInstance.IsBuilderModeEnabled();
			if (!flag)
			{
				this._velocity.Y = this._blockUnderFeet.MovementSettings.BounceVelocity;
				this.MovementStates.IsOnGround = true;
				this.MovementStates.IsJumping = false;
				this._fluidJump = this.MovementStates.IsInFluid;
				this._jumpCombo = (int)MathHelper.Min((float)(this._jumpCombo + 1), 3f);
				this._jumpBufferDurationLeft = 0f;
				this._jumpInputConsumed = true;
				int value;
				bool flag2 = !this._gameInstance.ServerSettings.BlockSoundSets[this._blockUnderFeet.BlockSoundSetIndex].SoundEventIndices.TryGetValue(5, out value);
				if (!flag2)
				{
					uint networkWwiseId = ResourceManager.GetNetworkWwiseId(value);
					bool flag3 = networkWwiseId == 0U;
					if (!flag3)
					{
						float x = this._gameInstance.LocalPlayer.Position.X;
						float num = this._gameInstance.LocalPlayer.Position.Y - 1f;
						float z = this._gameInstance.LocalPlayer.Position.Z;
						Vector3 position = new Vector3(x + 0.5f, num + 0.5f, z + 0.5f);
						this._gameInstance.AudioModule.PlaySoundEvent(networkWwiseId, position, Vector3.Zero);
					}
				}
			}
		}

		// Token: 0x06004BC9 RID: 19401 RVA: 0x0013E284 File Offset: 0x0013C484
		private void UpdateFluidData()
		{
			int hitboxHeight = this.GetHitboxHeight();
			int num = 0;
			int i = 0;
			while (i < hitboxHeight)
			{
				ClientBlockType clientBlockType;
				bool relativeFluid = this.GetRelativeFluid(0f, (float)i, 0f, out clientBlockType);
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
				IL_184:
				i++;
				continue;
				goto IL_184;
			}
			this.MovementStates.IsInFluid = (num > 0);
			this.MovementStates.IsSwimming = (num == hitboxHeight);
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
			bool isSwimJumping = this.MovementStates.IsSwimJumping;
			if (isSwimJumping)
			{
				bool flag4 = this._gameInstance.LocalPlayer.Position.Y <= this._swimJumpLastY;
				if (flag4)
				{
					this.MovementStates.IsSwimJumping = false;
				}
				this._swimJumpLastY = this._gameInstance.LocalPlayer.Position.Y;
			}
		}

		// Token: 0x06004BCA RID: 19402 RVA: 0x0013E528 File Offset: 0x0013C728
		public void UpdateSpecialBlocks()
		{
			int worldX = (int)Math.Floor((double)this._gameInstance.LocalPlayer.Position.X);
			int worldY = (int)Math.Floor((double)this._gameInstance.LocalPlayer.Position.Y - 0.2);
			int worldZ = (int)Math.Floor((double)this._gameInstance.LocalPlayer.Position.Z);
			int block = this._gameInstance.MapModule.GetBlock(worldX, worldY, worldZ, 0);
			ClientBlockType clientBlockType = this._gameInstance.MapModule.ClientBlockTypes[block];
			this._blockUnderFeet = clientBlockType;
			bool flag = clientBlockType.CollisionMaterial == 1 || this._lastBlockUnderFeet == null;
			if (flag)
			{
				this._lastBlockUnderFeet = clientBlockType;
			}
		}

		// Token: 0x06004BCB RID: 19403 RVA: 0x0013E5EC File Offset: 0x0013C7EC
		private int GetHitboxHeight()
		{
			return (int)Math.Max(Math.Ceiling((double)this._gameInstance.LocalPlayer.Hitbox.GetSize().Y), 1.0);
		}

		// Token: 0x06004BCC RID: 19404 RVA: 0x0013E62C File Offset: 0x0013C82C
		private bool GetRelativeFluid(float xOffset, float yOffset, float zOffset, out ClientBlockType blockTypeOut)
		{
			int worldX = (int)Math.Floor((double)(this._gameInstance.LocalPlayer.Position.X + xOffset));
			int worldY = (int)Math.Floor((double)(this._gameInstance.LocalPlayer.Position.Y + yOffset));
			int worldZ = (int)Math.Floor((double)(this._gameInstance.LocalPlayer.Position.Z + zOffset));
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

		// Token: 0x06004BCD RID: 19405 RVA: 0x0013E6E8 File Offset: 0x0013C8E8
		private void UpdateCrouching(bool canMove, ref MovementController.InputFrame input, ref InputBindings inputBindings)
		{
			bool flag = this._gameInstance.GameMode == 1 || !this._gameInstance.LocalPlayer.DisableCrouch;
			this.MovementStates.IsCrouching = (canMove && input.IsBindingHeld(inputBindings.Crouch) && flag);
			this.MovementStates.IsForcedCrouching = false;
			bool flag2 = this.MovementStates.IsFlying && base.SkipHitDetectionWhenFlying;
			if (flag2)
			{
				this._targetCrouchHeightShift = 0f;
			}
			else
			{
				bool isCrouching = this.MovementStates.IsCrouching;
				if (isCrouching)
				{
					this.MovementStates.IsForcedCrouching = this.EnforceCrouch();
					this._targetCrouchHeightShift = ((!this.MovementStates.IsFlying || this.MovementStates.IsForcedCrouching) ? this._gameInstance.LocalPlayer.CrouchOffset : 0f);
				}
				else
				{
					this.MovementStates.IsForcedCrouching = this.EnforceCrouch();
					this.MovementStates.IsCrouching = this.MovementStates.IsForcedCrouching;
					this._targetCrouchHeightShift = (this.MovementStates.IsCrouching ? this._gameInstance.LocalPlayer.CrouchOffset : 0f);
				}
			}
		}

		// Token: 0x06004BCE RID: 19406 RVA: 0x0013E828 File Offset: 0x0013CA28
		private bool EnforceCrouch()
		{
			PlayerEntity localPlayer = this._gameInstance.LocalPlayer;
			Vector3 position = localPlayer.Position;
			BoundingBox defaultHitbox = localPlayer.DefaultHitbox;
			defaultHitbox.Translate(position);
			float y = defaultHitbox.Max.Y;
			defaultHitbox.Translate(new Vector3(0f, 0.0001f, 0f));
			bool result = false;
			for (int i = -2; i <= 2; i++)
			{
				for (int j = -2; j <= 2; j++)
				{
					Vector3 pos = new Vector3(position.X + (float)j, y, position.Z + (float)i);
					HitDetection.CollisionHitData collisionHitData;
					bool flag = this._gameInstance.HitDetection.CheckBlockCollision(defaultHitbox, pos, Vector3.Up, out collisionHitData);
					if (flag)
					{
						bool ycollideState = collisionHitData.GetYCollideState();
						if (ycollideState)
						{
							result = true;
							bool flag2 = collisionHitData.Overlap.Y >= -localPlayer.CrouchOffset;
							if (flag2)
							{
								return false;
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06004BCF RID: 19407 RVA: 0x0013E944 File Offset: 0x0013CB44
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

		// Token: 0x06004BD0 RID: 19408 RVA: 0x0013E9A4 File Offset: 0x0013CBA4
		private void UpdateSprintForceValues()
		{
			bool flag = this.MovementStates.IsSprinting == this._previousMovementStates.IsSprinting && this.MovementStates.IsInFluid == this._previousMovementStates.IsInFluid;
			if (!flag)
			{
				bool flag2 = this.MovementStates.IsIdle && !this._previousMovementStates.IsIdle;
				if (flag2)
				{
					this.SprintForceDurationLeft = -1f;
				}
				else
				{
					this._sprintForceInitialSpeed = (this._previousMovementStates.IsIdle ? 0f : this._lastLateralSpeed);
					bool isSprinting = this.MovementStates.IsSprinting;
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

		// Token: 0x06004BD1 RID: 19409 RVA: 0x0013EAD8 File Offset: 0x0013CCD8
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
				float num = 0.8f;
				float num2 = 0.2f;
				bool flag2 = Math.Abs(vector2.X) > Math.Abs(vector2.Z);
				if (flag2)
				{
					bool flag3 = vector2.X > 0f && Math.Abs(position.X) % 1f < num2;
					if (flag3)
					{
						return false;
					}
					bool flag4 = vector2.X < 0f && Math.Abs(position.X) % 1f > num;
					if (flag4)
					{
						return false;
					}
				}
				else
				{
					bool flag5 = vector2.Z > 0f && Math.Abs(position.Z) % 1f > num;
					if (flag5)
					{
						return false;
					}
					bool flag6 = vector2.Z < 0f && Math.Abs(position.Z) % 1f < num2;
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

		// Token: 0x06004BD2 RID: 19410 RVA: 0x0013ECA4 File Offset: 0x0013CEA4
		private Vector3 CreateRepulsionVector()
		{
			Entity[] allEntities = this._gameInstance.EntityStoreModule.GetAllEntities();
			Vector2 value = new Vector2(this._gameInstance.LocalPlayer.Position.X, this._gameInstance.LocalPlayer.Position.Z);
			Vector3 vector = Vector3.Zero;
			BoundingBox hitbox = this._gameInstance.LocalPlayer.Hitbox;
			hitbox.Translate(this._gameInstance.LocalPlayer.Position);
			for (int i = 0; i < this._gameInstance.EntityStoreModule.GetEntitiesCount(); i++)
			{
				Entity entity = allEntities[i];
				bool flag = entity == null || entity.NetworkId == this._gameInstance.LocalPlayerNetworkId || !entity.IsTangible() || entity.RepulsionConfigIndex == -1 || entity.Position.Equals(this._gameInstance.LocalPlayer.Position);
				if (!flag)
				{
					Vector2 value2 = new Vector2(entity.Position.X, entity.Position.Z);
					float num = Vector2.Distance(value, value2);
					ClientRepulsionConfig clientRepulsionConfig = this._gameInstance.ServerSettings.RepulsionConfigs[entity.RepulsionConfigIndex];
					float radius = clientRepulsionConfig.Radius;
					BoundingBox hitbox2 = entity.Hitbox;
					hitbox2.Translate(entity.Position);
					entity.LastPush = Vector2.Zero;
					bool flag2 = this.MovementStates.IsOnGround && num <= radius && (double)num > 0.1 && DefaultMovementController.IntersectsY(hitbox, hitbox2);
					if (flag2)
					{
						float num2 = (radius - num) / radius;
						Vector2 vector2 = value - value2;
						vector2.Normalize();
						float num3 = clientRepulsionConfig.MaxForce;
						int num4 = 1;
						bool flag3 = num3 < 0f;
						if (flag3)
						{
							num4 = -1;
							num3 *= (float)num4;
						}
						float num5 = Math.Max(clientRepulsionConfig.MinForce, num3 * num2);
						num5 *= (float)num4;
						bool flag4 = this._wishDirection.Length() == 0f;
						if (flag4)
						{
							num5 = 1f;
						}
						vector2 *= num5;
						entity.LastPush = vector2;
						bool flag5 = vector == Vector3.Zero;
						if (flag5)
						{
							vector = new Vector3(vector2.X, 0f, vector2.Y);
						}
						else
						{
							vector = (vector + new Vector3(vector2.X, 0f, vector2.Y)) / 2f;
						}
					}
				}
			}
			return vector;
		}

		// Token: 0x06004BD3 RID: 19411 RVA: 0x0013EF4C File Offset: 0x0013D14C
		private static bool IntersectsY(BoundingBox a, BoundingBox b)
		{
			return DefaultMovementController.Intersects((double)a.Min.Y, (double)a.Max.Y, (double)b.Min.Y, (double)b.Max.Y);
		}

		// Token: 0x06004BD4 RID: 19412 RVA: 0x0013EF94 File Offset: 0x0013D194
		private static bool Intersects(double x1, double x2, double y1, double y2)
		{
			return x2 >= y1 && y2 >= x1;
		}

		// Token: 0x06004BD5 RID: 19413 RVA: 0x0013EFB4 File Offset: 0x0013D1B4
		private void ComputeMoveForce()
		{
			this.LastMoveForce = Vector3.Zero;
			float num = 0f;
			float num2 = 1f;
			float num3 = 1f;
			float value = (float)Math.Sqrt((double)(this._velocity.X * this._velocity.X + this._velocity.Z * this._velocity.Z));
			bool flag = !this.MovementStates.IsFlying && !this.MovementStates.IsClimbing && !this._gameInstance.IsBuilderModeEnabled();
			if (flag)
			{
				num = ((!this.MovementStates.IsOnGround && !this.MovementStates.IsSwimming) ? MathHelper.ConvertToNewRange(value, base.MovementSettings.AirDragMinSpeed, base.MovementSettings.AirDragMaxSpeed, base.MovementSettings.AirDragMin, base.MovementSettings.AirDragMax) : this._lastBlockUnderFeet.MovementSettings.Drag);
				num2 = ((!this.MovementStates.IsOnGround && !this.MovementStates.IsSwimming) ? MathHelper.ConvertToNewRange(value, base.MovementSettings.AirFrictionMinSpeed, base.MovementSettings.AirFrictionMaxSpeed, base.MovementSettings.AirFrictionMax, base.MovementSettings.AirFrictionMin) : this._lastBlockUnderFeet.MovementSettings.Friction);
				num3 = base.MovementSettings.Acceleration;
			}
			this._velocity.X = this._velocity.X * num;
			this._velocity.Z = this._velocity.Z * num;
			Vector3 movementForceRotation = this._movementForceRotation;
			bool flag2 = (this._gameInstance.App.Settings.UseNewFlyCamera && this.MovementStates.IsFlying) || this._gameInstance.CameraModule.Controller.AllowPitchControls;
			Quaternion rotation = Quaternion.CreateFromYawPitchRoll(movementForceRotation.Y, flag2 ? movementForceRotation.X : 0f, 0f);
			Vector3 value2 = Vector3.Transform(Vector3.Forward, rotation);
			Vector3 value3 = Vector3.Transform(Vector3.Right, rotation);
			Vector3 vector = value2 * this._wishDirection.Y + value3 * this._wishDirection.X;
			bool flag3 = vector.LengthSquared() < 0.0001f;
			if (flag3)
			{
				this._acceleration *= num3;
			}
			else
			{
				vector.Normalize();
				float num4 = 1f;
				bool flag4 = this._gameInstance.GameMode == 1;
				if (flag4)
				{
					float num5 = this.SpeedMultiplier;
					bool isSprinting = this.MovementStates.IsSprinting;
					if (isSprinting)
					{
						num5 = ((this.SpeedMultiplier < 1f) ? 1f : this.SpeedMultiplier);
					}
					num4 = num5 * this._gameInstance.CameraModule.Controller.SpeedModifier;
				}
				else
				{
					bool flag5 = !this.MovementStates.IsOnGround && !this.MovementStates.IsSwimming && !this._wasClimbing;
					if (flag5)
					{
						num4 += MathHelper.ConvertToNewRange(value, base.MovementSettings.AirControlMinSpeed, base.MovementSettings.AirControlMaxSpeed, base.MovementSettings.AirControlMaxMultiplier, base.MovementSettings.AirControlMinMultiplier);
					}
				}
				float num6 = this.GetHorizontalMoveSpeed() * num4;
				bool flag6 = this._gameInstance.ClientFeatureModule.IsFeatureEnabled(3) && this._gameInstance.App.Settings.SprintForce;
				if (flag6)
				{
					this.ComputeSprintForce(ref num6);
				}
				bool flag7 = this._gameInstance.ClientFeatureModule.IsFeatureEnabled(5);
				if (flag7)
				{
					this.ComputeAndUpdateRollingForce();
				}
				bool flag8 = this._gameInstance.ClientFeatureModule.IsFeatureEnabled(4);
				if (flag8)
				{
					this.ComputeSlideForce(ref num6);
				}
				bool flag9 = !this._gameInstance.IsBuilderModeEnabled();
				if (flag9)
				{
					num6 *= this._lastBlockUnderFeet.MovementSettings.HorizontalSpeedMultiplier;
				}
				float num7 = Vector3.Dot(this._velocity, vector);
				bool flag10 = !this.MovementStates.IsOnGround;
				if (flag10)
				{
					Vector3 vector2 = value2 * this._wishDirection.Y;
					vector2.Normalize();
					float num8 = Vector3.Dot(this._velocity, vector2);
					bool flag11 = num8 > num7;
					if (flag11)
					{
						num7 = num8;
					}
				}
				float num9 = num6 - num7;
				bool flag12 = num9 <= 0f;
				if (!flag12)
				{
					float num10 = num6 * num2;
					bool flag13 = this._fallEffectDurationLeft > 0f;
					if (flag13)
					{
						num10 *= this._fallEffectSpeedMultiplier;
					}
					bool flag14 = this._jumpObstacleDurationLeft > 0f;
					if (flag14)
					{
						num10 *= 1f - (this.MovementStates.IsSprinting ? base.MovementSettings.AutoJumpObstacleSprintSpeedLoss : base.MovementSettings.AutoJumpObstacleSpeedLoss);
					}
					bool flag15 = num10 > num9;
					if (flag15)
					{
						num10 = num9;
					}
					this._acceleration += ((base.MovementSettings.BaseSpeed != 0f) ? (num10 * (num6 / base.MovementSettings.BaseSpeed * num3)) : 0f);
					bool flag16 = this._acceleration > num10;
					if (flag16)
					{
						this._acceleration = num10;
					}
					this._acceleration *= this.ComputeMomentumLossMultiplier();
					vector.X *= this._acceleration;
					vector.Y *= this.GetVerticalMoveSpeed() * num4;
					vector.Z *= this._acceleration;
					this.LastMoveForce = vector;
					this._velocity += vector;
					this._lastLateralSpeed = (float)Math.Sqrt((double)(this._velocity.X * this._velocity.X) + (double)(this._velocity.Z * this._velocity.Z));
				}
			}
		}

		// Token: 0x06004BD6 RID: 19414 RVA: 0x0013F580 File Offset: 0x0013D780
		private float ComputeJumpForce()
		{
			float num = base.MovementSettings.JumpForce;
			bool flag = !this._gameInstance.IsBuilderModeEnabled();
			if (flag)
			{
				num *= this._blockUnderFeet.MovementSettings.JumpForceMultiplier;
			}
			bool flag2 = this._gameInstance.GameMode != 1 || this.SpeedMultiplier <= 1f;
			float result;
			if (flag2)
			{
				result = num;
			}
			else
			{
				result = num + Math.Min((this.SpeedMultiplier - 1f) * this._gameInstance.App.Settings.JumpForceSpeedMultiplierStep, this._gameInstance.App.Settings.MaxJumpForceSpeedMultiplier);
			}
			return result;
		}

		// Token: 0x06004BD7 RID: 19415 RVA: 0x0013F62C File Offset: 0x0013D82C
		private float GetHorizontalMoveSpeed()
		{
			bool flag = this._gameInstance.GameMode == 1 && this.MovementStates.IsFlying;
			float result;
			if (flag)
			{
				result = base.MovementSettings.HorizontalFlySpeed * (this.MovementStates.IsSprinting ? base.MovementSettings.ForwardSprintSpeedMultiplier : 1f);
			}
			else
			{
				Vector2 zero = Vector2.Zero;
				Vector2 zero2 = Vector2.Zero;
				bool isRolling = this.MovementStates.IsRolling;
				if (isRolling)
				{
					this._wishDirection.X = 0f;
					zero.Y = 1f;
					zero2.Y = this._curRollSpeedMultiplier;
				}
				else
				{
					bool isSprinting = this.MovementStates.IsSprinting;
					if (isSprinting)
					{
						zero.Y = 1.65f;
						zero2.Y = base.MovementSettings.ForwardSprintSpeedMultiplier;
					}
					else
					{
						bool isSliding = this.MovementStates.IsSliding;
						if (isSliding)
						{
							zero.Y = 0.55f;
							zero2.Y = base.MovementSettings.StrafeWalkSpeedMultiplier;
						}
						else
						{
							bool isCrouching = this.MovementStates.IsCrouching;
							if (isCrouching)
							{
								bool flag2 = this._wishDirection.X != 0f;
								if (flag2)
								{
									zero.X = 0.45f;
									zero2.X = base.MovementSettings.StrafeCrouchSpeedMultiplier;
								}
								bool flag3 = this._wishDirection.Y > 0f;
								if (flag3)
								{
									zero.Y = 0.55f;
									zero2.Y = base.MovementSettings.ForwardCrouchSpeedMultiplier;
								}
								else
								{
									bool flag4 = this._wishDirection.Y < 0f;
									if (flag4)
									{
										zero.Y = 0.4f;
										zero2.Y = base.MovementSettings.BackwardCrouchSpeedMultiplier;
									}
								}
							}
							else
							{
								bool isWalking = this.MovementStates.IsWalking;
								if (isWalking)
								{
									bool flag5 = this._wishDirection.X != 0f;
									if (flag5)
									{
										zero.X = 0.3f;
										zero2.X = base.MovementSettings.StrafeWalkSpeedMultiplier;
									}
									bool flag6 = this._wishDirection.Y > 0f;
									if (flag6)
									{
										zero.Y = 0.3f;
										zero2.Y = base.MovementSettings.ForwardWalkSpeedMultiplier;
									}
									else
									{
										bool flag7 = this._wishDirection.Y < 0f;
										if (flag7)
										{
											zero.Y = 0.3f;
											zero2.Y = base.MovementSettings.BackwardWalkSpeedMultiplier;
										}
									}
								}
								else
								{
									bool flag8 = this._wishDirection.X != 0f;
									if (flag8)
									{
										zero.X = 0.8f;
										zero2.X = base.MovementSettings.StrafeRunSpeedMultiplier;
									}
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
				}
				float baseSpeedMultiplier = 0f;
				float num = 0f;
				bool flag11 = zero2.Y > 0f;
				if (flag11)
				{
					baseSpeedMultiplier = zero.Y;
					num = zero2.Y;
				}
				else
				{
					bool flag12 = zero2.X > 0f;
					if (flag12)
					{
						baseSpeedMultiplier = zero.X;
						num = zero2.X;
					}
				}
				float num2 = 1f;
				bool flag13 = this.MovementStates.IsJumping || this.MovementStates.IsFalling;
				if (flag13)
				{
					num2 = MathHelper.Lerp(base.MovementSettings.AirSpeedMultiplier, base.MovementSettings.ComboAirSpeedMultiplier, ((float)this._jumpCombo - 1f) * 0.5f);
				}
				float num3 = (this.MovementStates.IsInFluid || this._fluidJump) ? this._averageFluidMovementSettings.HorizontalSpeedMultiplier : 1f;
				float num4 = this._gameInstance.InteractionModule.ForEachInteraction<float>((InteractionChain chain, ClientInteraction interaction, float mul) => mul * interaction.Interaction.HorizontalSpeedMultiplier, 1f);
				float horizontalSpeedMultiplier = this._gameInstance.LocalPlayer.HorizontalSpeedMultiplier;
				this._baseSpeedMultiplier = baseSpeedMultiplier;
				this._speedMultiplier = num * num2 * num3 * num4 * horizontalSpeedMultiplier;
				result = base.MovementSettings.BaseSpeed * this._speedMultiplier;
			}
			return result;
		}

		// Token: 0x06004BD8 RID: 19416 RVA: 0x0013FAD8 File Offset: 0x0013DCD8
		private void ComputeSprintForce(ref float wishSpeed)
		{
			bool flag = this.SprintForceDurationLeft < 0f || this.MovementStates.IsWalking || this.MovementStates.IsSliding;
			if (!flag)
			{
				Settings settings = this._gameInstance.App.Settings;
				Easing.EasingType easingType = this.MovementStates.IsSprinting ? settings.SprintAccelerationEasingType : settings.SprintDecelerationEasingType;
				float num = this.MovementStates.IsSprinting ? settings.SprintAccelerationDuration : settings.SprintDecelerationDuration;
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
				this._baseSpeedMultiplier = 1.65f;
				this._speedMultiplier = ((base.MovementSettings.BaseSpeed <= 0f) ? 1.65f : (wishSpeed / base.MovementSettings.BaseSpeed + 0.4f));
			}
		}

		// Token: 0x06004BD9 RID: 19417 RVA: 0x0013FC98 File Offset: 0x0013DE98
		private float ComputeMomentumLossMultiplier()
		{
			float num = 1f;
			bool flag = this.MovementStates.IsOnGround || this.MovementStates.IsSwimming || this._consecutiveMomentumLoss <= 0f;
			float result;
			if (flag)
			{
				result = num;
			}
			else
			{
				int num2 = 0;
				while ((float)num2 < this._consecutiveMomentumLoss)
				{
					num *= 1f - base.MovementSettings.FallMomentumLoss;
					num2++;
				}
				result = num;
			}
			return result;
		}

		// Token: 0x06004BDA RID: 19418 RVA: 0x0013FD14 File Offset: 0x0013DF14
		private float GetVerticalMoveSpeed()
		{
			bool isFlying = this.MovementStates.IsFlying;
			float result;
			if (isFlying)
			{
				result = base.MovementSettings.VerticalFlySpeed;
			}
			else
			{
				bool isSwimming = this.MovementStates.IsSwimming;
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

		// Token: 0x06004BDB RID: 19419 RVA: 0x0013FD98 File Offset: 0x0013DF98
		private Vector3 ApplyExternalForces(Vector3 movementOffset)
		{
			bool isFlying = this.MovementStates.IsFlying;
			Vector3 result;
			if (isFlying)
			{
				bool flag = this._appliedVelocities.Count > 0;
				if (flag)
				{
					this._appliedVelocities.Clear();
				}
				result = movementOffset;
			}
			else
			{
				for (int i = 0; i < this._appliedVelocities.Count; i++)
				{
					MovementController.AppliedVelocity appliedVelocity = this._appliedVelocities[i];
					bool flag2 = appliedVelocity.Velocity.Y + this._velocity.Y <= 0f || appliedVelocity.Velocity.Y < 0f;
					if (flag2)
					{
						appliedVelocity.CanClear = true;
					}
					bool flag3 = this.MovementStates.IsOnGround && appliedVelocity.CanClear;
					if (flag3)
					{
						appliedVelocity.Velocity.Y = 0f;
					}
					movementOffset += appliedVelocity.Velocity;
					bool isOnGround = this.MovementStates.IsOnGround;
					float num;
					float num2;
					if (isOnGround)
					{
						num = appliedVelocity.Config.GroundResistance;
						num2 = appliedVelocity.Config.GroundResistanceMax;
					}
					else
					{
						num = appliedVelocity.Config.AirResistance;
						num2 = appliedVelocity.Config.AirResistanceMax;
					}
					float num3 = num;
					bool flag4 = num2 >= 0f;
					if (flag4)
					{
						VelocityConfig.VelocityThresholdStyle style = appliedVelocity.Config.Style;
						VelocityConfig.VelocityThresholdStyle velocityThresholdStyle = style;
						if (velocityThresholdStyle != null)
						{
							if (velocityThresholdStyle != 1)
							{
								throw new ArgumentOutOfRangeException();
							}
							float num4 = appliedVelocity.Velocity.LengthSquared();
							bool flag5 = num4 < appliedVelocity.Config.Threshold * appliedVelocity.Config.Threshold;
							if (flag5)
							{
								float num5 = num4 / (appliedVelocity.Config.Threshold * appliedVelocity.Config.Threshold);
								num3 = num * num5 + num2 * (1f - num5);
							}
						}
						else
						{
							float num4 = appliedVelocity.Velocity.Length();
							bool flag6 = num4 < appliedVelocity.Config.Threshold;
							if (flag6)
							{
								float num6 = num4 / appliedVelocity.Config.Threshold;
								num3 = num * num6 + num2 * (1f - num6);
							}
						}
					}
					MovementController.AppliedVelocity appliedVelocity2 = appliedVelocity;
					appliedVelocity2.Velocity.X = appliedVelocity2.Velocity.X * num3;
					MovementController.AppliedVelocity appliedVelocity3 = appliedVelocity;
					appliedVelocity3.Velocity.Z = appliedVelocity3.Velocity.Z * num3;
					bool flag7 = (double)appliedVelocity.Velocity.LengthSquared() < 0.001;
					if (flag7)
					{
						this._appliedVelocities.RemoveAt(i);
						i--;
					}
				}
				result = movementOffset;
			}
			return result;
		}

		// Token: 0x06004BDC RID: 19420 RVA: 0x00140030 File Offset: 0x0013E230
		private void CheckEntityCollision(Vector3 movement)
		{
			bool flag = this.RaycastDistance <= 0f || this.MovementStates.IsEntityCollided || !this._gameInstance.LocalPlayer.IsTangible();
			if (!flag)
			{
				Vector3 rayPosition = Vector3.Add(this._gameInstance.LocalPlayer.Position, new Vector3(0f, this.RaycastHeightOffset, 0f));
				Vector3 vector = new Vector3(movement.X, movement.Y, movement.Z);
				bool flag2 = vector == Vector3.Zero;
				if (!flag2)
				{
					Vector3 rayDirection = Vector3.Normalize(vector);
					bool flag3 = this.RaycastMode == 1;
					if (flag3)
					{
						Vector3 lookOrientation = this._gameInstance.LocalPlayer.LookOrientation;
						Quaternion rotation = Quaternion.CreateFromYawPitchRoll(lookOrientation.Yaw, lookOrientation.Pitch, 0f);
						Vector3 vector2 = Vector3.Transform(Vector3.Forward, rotation);
						rayPosition = new Vector3(this._gameInstance.LocalPlayer.Position.X, this._gameInstance.LocalPlayer.Position.Y + this._gameInstance.LocalPlayer.EyeOffset + this.RaycastHeightOffset, this._gameInstance.LocalPlayer.Position.Z);
						rayDirection = vector2;
					}
					HitDetection.EntityHitData entityHitData;
					this._gameInstance.HitDetection.RaycastEntity(rayPosition, rayDirection, this.RaycastDistance, false, out entityHitData);
					bool flag4 = entityHitData.Entity != null;
					if (flag4)
					{
						this.MovementStates.IsEntityCollided = true;
					}
				}
			}
		}

		// Token: 0x06004BDD RID: 19421 RVA: 0x001401C8 File Offset: 0x0013E3C8
		private void ComputeFallEffect()
		{
			bool flag = this._gameInstance.IsBuilderModeEnabled();
			if (!flag)
			{
				bool flag2 = this._fallEffectDurationLeft > 0f;
				if (flag2)
				{
					this._fallEffectDurationLeft -= 0.016666668f;
				}
				double num = (double)(-(double)this._velocity.Y);
				bool flag3 = num > 18.0 && this._wasFalling && !this.MovementStates.IsFalling;
				if (flag3)
				{
					double num2 = (Math.Pow(0.75 * (num - 18.0), 2.0) + 10.0) / 100.0;
					this._fallEffectDurationLeft = base.MovementSettings.FallEffectDuration;
					this._fallEffectSpeedMultiplier = Math.Max((float)(1.0 - num2), 0.3f);
					this._fallEffectJumpForce = base.MovementSettings.FallJumpForce;
					this._gameInstance.AudioModule.PlayLocalSoundEvent("PLAYER_LAND_PENALTY_MAJOR");
				}
				this._fallEffectToApply = false;
			}
		}

		// Token: 0x06004BDE RID: 19422 RVA: 0x001402E0 File Offset: 0x0013E4E0
		private void DoMoveCycle(Vector3 offset)
		{
			InputBindings inputBindings = this._gameInstance.App.Settings.InputBindings;
			Input input = this._gameInstance.Input;
			PlayerEntity localPlayer = this._gameInstance.LocalPlayer;
			Vector3 size = localPlayer.Hitbox.GetSize();
			Vector3 position = localPlayer.Position;
			float num = (this.MovementStates.IsFlying || this.MovementStates.IsOnGround || this.MovementStates.IsSwimming) ? 0.625f : 0.15625f;
			bool flag = false;
			bool flag2 = false;
			this._previousAutoJumpHeightShift = this._nextAutoJumpHeightShift;
			position.Y += offset.Y;
			HitDetection.CollisionHitData hitData;
			bool flag3 = this.CheckCollision(position, offset, HitDetection.CollisionAxis.Y, out hitData);
			bool flag4 = this.MovementStates.IsOnGround && offset.Y < 0f;
			if (flag4)
			{
				bool flag5 = !flag3;
				if (flag5)
				{
					this.MovementStates.IsOnGround = false;
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
						this.MovementStates.IsOnGround = true;
						position.Y = hitData.Limit.Y;
						bool flag8 = this._gameInstance.ClientFeatureModule.IsFeatureEnabled(5);
						if (flag8)
						{
							this.CheckAndSetRollingState();
						}
					}
					else
					{
						this.MovementStates.IsOnGround = false;
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
					this.MovementStates.IsOnGround = false;
				}
			}
			int num2 = 0;
			this.MovementStates.IsClimbing = false;
			this._collisionForward = (this._collisionBackward = (this._collisionLeft = (this._collisionRight = false)));
			ClientHitboxCollisionConfig clientHitboxCollisionConfig;
			Entity entity;
			bool flag9 = this.MovementStates.IsIdle && this.CheckEntityCollision(position, offset, out clientHitboxCollisionConfig, out entity);
			if (flag9)
			{
				Vector3 vector = position - entity.Position;
				vector.Normalize();
				vector *= base.MovementSettings.CollisionExpulsionForce;
				offset.X = vector.X;
				offset.Z = vector.Z;
			}
			bool flag10 = offset.X != 0f && !this.MovementStates.IsMantling;
			if (flag10)
			{
				position.X += offset.X;
				bool flag11 = this.CheckCollision(position, offset, HitDetection.CollisionAxis.X, out hitData);
				bool flag12 = hitData.Overlap.X > 0f;
				if (flag12)
				{
					bool flag13 = offset.Y > 0f && this._requestedVelocity.X > 0f;
					if (flag13)
					{
						this._requestedVelocity.X = 0f;
					}
					else
					{
						bool flag14 = offset.Y < 0f && this._requestedVelocity.X < 0f;
						if (flag14)
						{
							this._requestedVelocity.X = 0f;
						}
					}
					Vector3 zero = Vector3.Zero;
					num2 = this._gameInstance.MapModule.GetBlock((int)Math.Floor((double)(hitData.Limit.X + offset.X)), (int)Math.Floor((double)position.Y), (int)Math.Floor((double)position.Z), 1);
					bool flag15 = this._gameInstance.MapModule.ClientBlockTypes[num2].MovementSettings.IsClimbable;
					bool flag16 = flag15;
					if (flag16)
					{
						BlockHitbox blockHitbox = this._gameInstance.ServerSettings.BlockHitboxes[this._gameInstance.MapModule.ClientBlockTypes[num2].HitboxType];
						zero = new Vector3((float)((int)Math.Floor((double)(hitData.Limit.X + offset.X))), (float)((int)Math.Floor((double)position.Y)), (float)((int)Math.Floor((double)position.Z)));
						num2 = this._gameInstance.MapModule.GetBlock((int)zero.X, (int)zero.Y, (int)zero.Z, 1);
						float num3 = Math.Abs((hitData.Limit.X + offset.X) % 1f);
						bool flag17 = hitData.Limit.X < 0f;
						if (flag17)
						{
							num3 = 1f - num3;
						}
						float num4 = position.Z % 1f;
						bool flag18 = num4 < 0f;
						if (flag18)
						{
							num4 = 1f + num4;
						}
						flag15 = (num3 >= blockHitbox.BoundingBox.Min.X && num3 <= blockHitbox.BoundingBox.Max.X && num4 >= blockHitbox.BoundingBox.Min.Z && num4 <= blockHitbox.BoundingBox.Max.Z);
					}
					else
					{
						zero = new Vector3((float)((int)Math.Floor((double)position.X)), (float)((int)Math.Floor((double)position.Y)), (float)((int)Math.Floor((double)position.Z)));
						num2 = this._gameInstance.MapModule.GetBlock((int)zero.X, (int)zero.Y, (int)zero.Z, 1);
						ClientBlockType clientBlockType = this._gameInstance.MapModule.ClientBlockTypes[num2];
						bool isClimbable = clientBlockType.MovementSettings.IsClimbable;
						if (isClimbable)
						{
							BlockHitbox blockHitbox2 = this._gameInstance.ServerSettings.BlockHitboxes[clientBlockType.HitboxType];
							float num5 = position.Z % 1f;
							bool flag19 = num5 < 0f;
							if (flag19)
							{
								num5 = 1f + num5;
							}
							flag15 = (((offset.X < 0f && blockHitbox2.BoundingBox.Min.X == 0f) || (offset.X > 0f && blockHitbox2.BoundingBox.Max.X == 1f)) && num5 >= blockHitbox2.BoundingBox.Min.Z && num5 <= blockHitbox2.BoundingBox.Max.Z);
						}
					}
					bool flag20 = !this.MovementStates.IsOnGround;
					if (flag20)
					{
						float num6 = float.PositiveInfinity;
						bool flag21 = this._gameInstance.HitDetection.RaycastBlock(position, Vector3.Down, DefaultMovementController.FallRaycastOptions, out this._groundHit);
						if (flag21)
						{
							num6 = this._groundHit.Distance;
						}
						bool flag22 = this._gameInstance.HitDetection.RaycastBlock(this._gameInstance.LocalPlayer.Position, Vector3.Down, DefaultMovementController.FallRaycastOptions, out this._groundHit);
						if (flag22)
						{
							num6 = Math.Min(num6, this._groundHit.Distance);
						}
						bool flag23 = num6 < 0.375f;
						if (flag23)
						{
							num = 0.625f;
						}
					}
					bool flag24 = hitData.Limit.Y > position.Y && hitData.Limit.Y - position.Y <= num;
					flag24 = this.CanJumpObstacle(flag24, hitData, position, new Vector3(hitData.Limit.X + offset.X, position.Y, position.Z), new Vector2(hitData.Limit.X + offset.X - position.X, 0f), 90f);
					bool flag25 = !this.MovementStates.IsClimbing && flag24 && (this.MovementStates.IsFlying || this.MovementStates.IsSwimming || offset.Y < 0f);
					if (flag25)
					{
						float y = position.Y;
						position.Y = hitData.Limit.Y;
						HitDetection.CollisionHitData collisionHitData;
						bool flag26 = this.CheckCollision(position, offset, HitDetection.CollisionAxis.X, out collisionHitData);
						if (flag26)
						{
							bool flag27 = offset.X <= 0f;
							if (flag27)
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
							flag = true;
							this._autoJumpHeight = hitData.Overlap.Y;
							position.Y = hitData.Limit.Y + 0.0001f;
						}
					}
					else
					{
						bool flag28 = flag15 && !this.MovementStates.IsClimbing;
						if (flag28)
						{
							this._collisionLeft = (offset.X <= 0f);
							this._collisionRight = !this._collisionLeft;
							bool collisionLeft = this._collisionLeft;
							if (collisionLeft)
							{
								position.X = hitData.Limit.X + size.Z * 0.5f + 0.0001f;
							}
							else
							{
								position.X = hitData.Limit.X - size.Z * 0.5f - 0.0001f;
							}
							ClientBlockType clientBlockType2 = this._gameInstance.MapModule.ClientBlockTypes[num2];
							float num7 = 0f;
							bool flag29 = input.IsBindingHeld(inputBindings.MoveForwards, false);
							if (flag29)
							{
								num7 = base.MovementSettings.ClimbSpeed * clientBlockType2.MovementSettings.ClimbUpSpeedMultiplier;
							}
							else
							{
								bool flag30 = this.MovementStates.IsCrouching || input.IsBindingHeld(inputBindings.MoveBackwards, false);
								if (flag30)
								{
									num7 = -base.MovementSettings.ClimbSpeed * clientBlockType2.MovementSettings.ClimbDownSpeedMultiplier;
								}
							}
							position.Y += num7;
							HitDetection.CollisionHitData collisionHitData;
							bool flag31 = this.CheckCollision(position, new Vector3(0f, num7, 0f), HitDetection.CollisionAxis.Y, out collisionHitData);
							if (flag31)
							{
								bool flag32 = input.IsBindingHeld(inputBindings.MoveForwards, false);
								if (flag32)
								{
									position.Y = collisionHitData.Limit.Y - size.Y - 0.0001f;
								}
								else
								{
									position.Y = collisionHitData.Limit.Y + 0.0001f;
									this.MovementStates.IsOnGround = true;
								}
								offset.Y = 0f;
							}
							this.MovementStates.IsClimbing = true;
							flag2 = true;
							this._climbingBlockPosition = zero;
							this._climbingBlockId = num2;
						}
						else
						{
							bool flag33 = hitData.Overlap.X >= 0f;
							if (flag33)
							{
								this._collisionLeft = (offset.X <= 0f);
								this._collisionRight = !this._collisionLeft;
								bool collisionLeft2 = this._collisionLeft;
								if (collisionLeft2)
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
				}
				else
				{
					ClientHitboxCollisionConfig hitboxCollisionConfig;
					Entity entityCollided;
					bool flag34 = !this.MovementStates.IsIdle && this.CheckEntityCollision(position, offset, out hitboxCollisionConfig, out entityCollided);
					if (flag34)
					{
						this.ComputeHitboxCollisionOffsetMovement(hitboxCollisionConfig, ref position, offset, entityCollided, 0);
					}
					else
					{
						bool flag35 = this.CheckMantling(position);
						if (flag35)
						{
							this.Mantle(ref position);
							localPlayer.SetPositionTeleport(position);
						}
					}
				}
			}
			bool flag36 = offset.Z != 0f && !this.MovementStates.IsMantling;
			if (flag36)
			{
				position.Z += offset.Z;
				bool flag37 = this.CheckCollision(position, offset, HitDetection.CollisionAxis.Z, out hitData);
				bool flag38 = flag37;
				if (flag38)
				{
					bool flag39 = offset.Z > 0f && this._requestedVelocity.Z > 0f;
					if (flag39)
					{
						this._requestedVelocity.Z = 0f;
					}
					else
					{
						bool flag40 = offset.Z < 0f && this._requestedVelocity.Z < 0f;
						if (flag40)
						{
							this._requestedVelocity.Z = 0f;
						}
					}
					bool flag41 = flag2;
					Vector3 zero2 = Vector3.Zero;
					bool flag42 = !flag41;
					if (flag42)
					{
						zero2 = new Vector3((float)((int)Math.Floor((double)position.X)), (float)((int)Math.Floor((double)position.Y)), (float)((int)Math.Floor((double)(hitData.Limit.Z + offset.Z))));
						num2 = this._gameInstance.MapModule.GetBlock((int)zero2.X, (int)zero2.Y, (int)zero2.Z, 1);
						flag41 = this._gameInstance.MapModule.ClientBlockTypes[num2].MovementSettings.IsClimbable;
						bool flag43 = flag41;
						if (flag43)
						{
							BlockHitbox blockHitbox3 = this._gameInstance.ServerSettings.BlockHitboxes[this._gameInstance.MapModule.ClientBlockTypes[num2].HitboxType];
							float num8 = Math.Abs((hitData.Limit.Z + offset.Z) % 1f);
							bool flag44 = hitData.Limit.Z < 0f;
							if (flag44)
							{
								num8 = 1f - num8;
							}
							float num9 = position.X % 1f;
							bool flag45 = num9 < 0f;
							if (flag45)
							{
								num9 = 1f + num9;
							}
							flag41 = (num8 >= blockHitbox3.BoundingBox.Min.Z && num8 <= blockHitbox3.BoundingBox.Max.Z && num9 >= blockHitbox3.BoundingBox.Min.X && num9 <= blockHitbox3.BoundingBox.Max.X);
						}
						else
						{
							zero2 = new Vector3((float)((int)Math.Floor((double)position.X)), (float)((int)Math.Floor((double)position.Y)), (float)((int)Math.Floor((double)position.Z)));
							num2 = this._gameInstance.MapModule.GetBlock((int)zero2.X, (int)zero2.Y, (int)zero2.Z, 1);
							ClientBlockType clientBlockType3 = this._gameInstance.MapModule.ClientBlockTypes[num2];
							bool isClimbable2 = clientBlockType3.MovementSettings.IsClimbable;
							if (isClimbable2)
							{
								BlockHitbox blockHitbox4 = this._gameInstance.ServerSettings.BlockHitboxes[clientBlockType3.HitboxType];
								float num10 = position.X % 1f;
								bool flag46 = num10 < 0f;
								if (flag46)
								{
									num10 = 1f + num10;
								}
								flag41 = (((offset.Z < 0f && blockHitbox4.BoundingBox.Min.Z == 0f) || (offset.Z > 0f && blockHitbox4.BoundingBox.Max.Z == 1f)) && num10 >= blockHitbox4.BoundingBox.Min.X && num10 <= blockHitbox4.BoundingBox.Max.X);
							}
						}
					}
					bool flag47 = !this.MovementStates.IsOnGround;
					if (flag47)
					{
						float num11 = float.PositiveInfinity;
						bool flag48 = this._gameInstance.HitDetection.RaycastBlock(position, Vector3.Down, DefaultMovementController.FallRaycastOptions, out this._groundHit);
						if (flag48)
						{
							num11 = this._groundHit.Distance;
						}
						bool flag49 = this._gameInstance.HitDetection.RaycastBlock(this._gameInstance.LocalPlayer.Position, Vector3.Down, DefaultMovementController.FallRaycastOptions, out this._groundHit);
						if (flag49)
						{
							num11 = Math.Min(num11, this._groundHit.Distance);
						}
						bool flag50 = num11 < 0.375f;
						if (flag50)
						{
							num = 0.625f;
						}
					}
					bool flag51 = hitData.Limit.Y > position.Y && hitData.Limit.Y - position.Y < num;
					flag51 = this.CanJumpObstacle(flag51, hitData, position, new Vector3(position.X, position.Y, hitData.Limit.Z + offset.Z), new Vector2(0f, hitData.Limit.Z + offset.Z - position.Z), -90f);
					bool flag52 = !this.MovementStates.IsClimbing && flag51 && (this.MovementStates.IsFlying || this.MovementStates.IsSwimming || offset.Y < 0f);
					if (flag52)
					{
						float y2 = position.Y;
						position.Y = hitData.Limit.Y;
						HitDetection.CollisionHitData collisionHitData;
						bool flag53 = this.CheckCollision(position, offset, HitDetection.CollisionAxis.Z, out collisionHitData);
						if (flag53)
						{
							bool flag54 = offset.Z <= 0f;
							if (flag54)
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
							flag = true;
							this._autoJumpHeight = hitData.Overlap.Y;
							position.Y = hitData.Limit.Y + 0.0001f;
						}
					}
					else
					{
						bool flag55 = flag41 && !flag2;
						if (flag55)
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
							ClientBlockType clientBlockType4 = this._gameInstance.MapModule.ClientBlockTypes[num2];
							float num12 = 0f;
							bool flag56 = input.IsBindingHeld(inputBindings.MoveForwards, false);
							if (flag56)
							{
								num12 = base.MovementSettings.ClimbSpeed * clientBlockType4.MovementSettings.ClimbUpSpeedMultiplier;
							}
							else
							{
								bool flag57 = this.MovementStates.IsCrouching || input.IsBindingHeld(inputBindings.MoveBackwards, false);
								if (flag57)
								{
									num12 = -base.MovementSettings.ClimbSpeed * clientBlockType4.MovementSettings.ClimbDownSpeedMultiplier;
								}
							}
							position.Y += num12;
							HitDetection.CollisionHitData collisionHitData;
							bool flag58 = this.CheckCollision(position, new Vector3(0f, num12, 0f), HitDetection.CollisionAxis.Y, out collisionHitData);
							if (flag58)
							{
								bool flag59 = input.IsBindingHeld(inputBindings.MoveForwards, false);
								if (flag59)
								{
									position.Y = collisionHitData.Limit.Y - size.Y - 0.0001f;
								}
								else
								{
									position.Y = collisionHitData.Limit.Y + 0.0001f;
									this.MovementStates.IsOnGround = true;
								}
								offset.Y = 0f;
							}
							this.MovementStates.IsClimbing = true;
							this._climbingBlockPosition = zero2;
							this._climbingBlockId = num2;
						}
						else
						{
							bool flag60 = hitData.Overlap.Z >= 0f;
							if (flag60)
							{
								this._collisionForward = (offset.Z <= 0f);
								this._collisionBackward = !this._collisionForward;
								bool collisionForward2 = this._collisionForward;
								if (collisionForward2)
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
				}
				else
				{
					ClientHitboxCollisionConfig hitboxCollisionConfig2;
					Entity entityCollided2;
					bool flag61 = !this.MovementStates.IsIdle && this.CheckEntityCollision(position, offset, out hitboxCollisionConfig2, out entityCollided2);
					if (flag61)
					{
						this.ComputeHitboxCollisionOffsetMovement(hitboxCollisionConfig2, ref position, offset, entityCollided2, 2);
					}
					else
					{
						bool flag62 = this.CheckMantling(position);
						if (flag62)
						{
							this.Mantle(ref position);
							localPlayer.SetPositionTeleport(position);
						}
					}
				}
			}
			bool flag63 = flag;
			if (flag63)
			{
				float num13 = 1f / (Math.Max(Math.Abs(this._velocity.X), Math.Abs(this._velocity.Z)) * 0.25f);
				num13 = Math.Min(Math.Max(num13, 0.01f), 1.5f);
				this._autoJumpFrameCount = (int)Math.Floor((double)(20f * num13 * this._autoJumpHeight));
				bool flag64 = this._autoJumpFrameCount > 0;
				if (flag64)
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
			bool flag65 = this._autoJumpFrame > 0;
			if (flag65)
			{
				this._nextAutoJumpHeightShift += this._autoJumpHeight / (float)this._autoJumpFrameCount;
				this._autoJumpFrame--;
			}
			localPlayer.SetPosition(position);
		}

		// Token: 0x06004BDF RID: 19423 RVA: 0x001418EC File Offset: 0x0013FAEC
		private bool CheckEntityCollision(Vector3 position, Vector3 moveOffset, out ClientHitboxCollisionConfig hitboxCollisionConfig, out Entity entityCollided)
		{
			hitboxCollisionConfig = null;
			entityCollided = null;
			bool flag = !this._gameInstance.LocalPlayer.IsTangible();
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				BoundingBox hitbox = this._gameInstance.LocalPlayer.Hitbox;
				BoundingBox boundingBox = new BoundingBox(hitbox.Min, hitbox.Max);
				boundingBox.Translate(new Vector3(position.X + moveOffset.X, position.Y + 0.0001f + moveOffset.Y, position.Z + moveOffset.Z));
				EntityStoreModule entityStoreModule = this._gameInstance.EntityStoreModule;
				Entity[] allEntities = entityStoreModule.GetAllEntities();
				BoundingBox box = default(BoundingBox);
				for (int i = entityStoreModule.PlayerEntityLocalId + 1; i < entityStoreModule.GetEntitiesCount(); i++)
				{
					Entity entity = allEntities[i];
					bool flag2 = entity.HitboxCollisionConfigIndex == -1;
					if (!flag2)
					{
						bool flag3 = !entity.IsTangible();
						if (!flag3)
						{
							ClientHitboxCollisionConfig clientHitboxCollisionConfig = this._gameInstance.ServerSettings.HitboxCollisionConfigs[entity.HitboxCollisionConfigIndex];
							bool flag4 = clientHitboxCollisionConfig.CollisionType == ClientHitboxCollisionConfig.ClientCollisionType.Hard;
							if (!flag4)
							{
								box.Min = entity.Hitbox.Min;
								box.Max = entity.Hitbox.Max;
								bool flag5 = !boundingBox.IntersectsExclusive(box, entity.Position.X, entity.Position.Y, entity.Position.Z);
								if (!flag5)
								{
									bool flag6 = hitboxCollisionConfig == null || clientHitboxCollisionConfig.SoftCollisionOffsetRatio > hitboxCollisionConfig.SoftCollisionOffsetRatio;
									if (flag6)
									{
										hitboxCollisionConfig = clientHitboxCollisionConfig;
										entityCollided = entity;
									}
									bool debugInfoNeedsDrawing = this._gameInstance.EntityStoreModule.DebugInfoNeedsDrawing;
									if (debugInfoNeedsDrawing)
									{
										this.CollidedEntities.Add(entity.NetworkId);
									}
								}
							}
						}
					}
				}
				result = (hitboxCollisionConfig != null);
			}
			return result;
		}

		// Token: 0x06004BE0 RID: 19424 RVA: 0x00141AE0 File Offset: 0x0013FCE0
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
					bool flag3 = !this.MovementStates.IsOnGround || this.MovementStates.IsCrouching;
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
								float num = MathHelper.WrapAngle(this._gameInstance.LocalPlayer.LookOrientation.Y + MathHelper.ToRadians(angleOffset));
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
									this._jumpObstacleDurationLeft = (this.MovementStates.IsSprinting ? base.MovementSettings.AutoJumpObstacleSprintEffectDuration : base.MovementSettings.AutoJumpObstacleEffectDuration);
									this._gameInstance.LocalPlayer.SetServerAnimation(this.MovementStates.IsSprinting ? "StepSprint" : (this.MovementStates.IsWalking ? "StepWalk" : "StepRun"), 0, 0f, true);
									result = true;
								}
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06004BE1 RID: 19425 RVA: 0x00141D00 File Offset: 0x0013FF00
		private void ComputeHitboxCollisionOffsetMovement(ClientHitboxCollisionConfig hitboxCollisionConfig, ref Vector3 checkPos, Vector3 offset, Entity entityCollided, Axis axis)
		{
			ClientHitboxCollisionConfig.ClientCollisionType collisionType = hitboxCollisionConfig.CollisionType;
			ClientHitboxCollisionConfig.ClientCollisionType clientCollisionType = collisionType;
			if (clientCollisionType == ClientHitboxCollisionConfig.ClientCollisionType.Soft)
			{
				bool flag = axis == 0;
				if (flag)
				{
					checkPos.X += -offset.X + offset.X * hitboxCollisionConfig.SoftCollisionOffsetRatio;
				}
				else
				{
					bool flag2 = axis == 2;
					if (flag2)
					{
						checkPos.Z += -offset.Z + offset.Z * hitboxCollisionConfig.SoftCollisionOffsetRatio;
					}
				}
			}
		}

		// Token: 0x06004BE2 RID: 19426 RVA: 0x00141D74 File Offset: 0x0013FF74
		private void UpdateViewModifiers()
		{
			this._previousCrouchHeightShift = this._nextCrouchHeightShift;
			bool flag = this._nextCrouchHeightShift != this._targetCrouchHeightShift;
			if (flag)
			{
				float step = (this.MovementStates.IsOnGround || this.MovementStates.IsClimbing) ? 0.1f : 0.055f;
				this._nextCrouchHeightShift = MathHelper.Step(base.CrouchHeightShift, this._targetCrouchHeightShift, step);
			}
			Settings settings = this._gameInstance.App.Settings;
			float num = (float)settings.FieldOfView;
			bool flag2 = settings.SprintFovEffect && !this._gameInstance.IsBuilderModeEnabled() && (this.MovementStates.IsSprinting || (this.SprintForceDurationLeft > 0f && !this.MovementStates.IsWalking && !this.MovementStates.IsIdle));
			if (flag2)
			{
				num *= 1f + (settings.SprintFovIntensity - 1f) * ((this.SprintForceDurationLeft > 0f) ? (this.MovementStates.IsSprinting ? this.SprintForceProgress : (1f - this.SprintForceProgress)) : 1f);
			}
			bool flag3 = this.MovementStates.IsInFluid || this._fluidJump;
			if (flag3)
			{
				num *= this._averageFluidMovementSettings.FieldOfViewMultiplier;
			}
			bool flag4 = Math.Abs(this._gameInstance.ActiveFieldOfView - num) > 1f;
			if (flag4)
			{
				float fieldOfView = MathHelper.Lerp(this._gameInstance.ActiveFieldOfView, Math.Min(num, 180f), 0.1f);
				this._gameInstance.SetFieldOfView(fieldOfView);
			}
			this._gameInstance.LocalPlayer.ApplyFirstPersonMovementItemWiggle(this._wishDirection.X * -0.5f, (float)Math.Sign(this._velocity.Y) * -0.5f, this._wishDirection.Y * 0.5f);
		}

		// Token: 0x06004BE3 RID: 19427 RVA: 0x00141F68 File Offset: 0x00140168
		private void UpdateMovementStates()
		{
			bool flag = this.MovementStates.IsMantling || this.MovementStates.IsRolling;
			if (flag)
			{
				this.MovementStates.IsIdle = false;
				this.MovementStates.IsHorizontalIdle = false;
			}
			else
			{
				this.MovementStates.IsIdle = (Math.Abs(this._velocity.Y) <= 1E-07f && (this._wishDirection == Vector2.Zero || (Math.Abs(this._velocity.X) <= 1E-07f && Math.Abs(this._velocity.Z) <= 1E-07f)));
				this.MovementStates.IsHorizontalIdle = (this._wishDirection == Vector2.Zero);
			}
		}

		// Token: 0x06004BE4 RID: 19428 RVA: 0x0014203C File Offset: 0x0014023C
		private bool IsDisallowedInteraction(KeyValuePair<int, InteractionChain> chainPair)
		{
			return !chainPair.Value.InitialRootInteraction.Tags.Contains(this._gameInstance.ServerSettings.GetServerTag("Allows=Movement"));
		}

		// Token: 0x06004BE5 RID: 19429 RVA: 0x0014207C File Offset: 0x0014027C
		private bool CheckMantling(Vector3 position)
		{
			bool isFlying = this.MovementStates.IsFlying;
			bool result;
			if (isFlying)
			{
				result = false;
			}
			else
			{
				bool flag = !this._gameInstance.ClientFeatureModule.IsFeatureEnabled(2);
				if (flag)
				{
					result = false;
				}
				else
				{
					bool flag2 = !this._gameInstance.App.Settings.Mantling;
					if (flag2)
					{
						result = false;
					}
					else
					{
						bool flag3 = this._wishDirection.Y <= 0f;
						if (flag3)
						{
							result = false;
						}
						else
						{
							bool flag4 = this._velocity.Y < this._gameInstance.App.Settings.MinVelocityMantling || this._velocity.Y > this._gameInstance.App.Settings.MaxVelocityMantling;
							if (flag4)
							{
								result = false;
							}
							else
							{
								bool flag5 = (Math.Abs(position.Y) + 1.8f) % 1f < this._gameInstance.App.Settings.MantleBlockHeight;
								if (flag5)
								{
									result = false;
								}
								else
								{
									bool flag6 = this._gameInstance.InteractionModule.Chains.Count > 0 && Enumerable.Any<KeyValuePair<int, InteractionChain>>(this._gameInstance.InteractionModule.Chains, new Func<KeyValuePair<int, InteractionChain>, bool>(this.IsDisallowedInteraction));
									if (flag6)
									{
										result = false;
									}
									else
									{
										bool flag7 = this.MovementStates.IsCrouching || this.MovementStates.IsSliding;
										if (flag7)
										{
											result = false;
										}
										else
										{
											this._mantleOffset = Vector3.Zero;
											float num = position.X;
											float num2 = (float)Math.Floor((double)position.Y) - 1f;
											float num3 = position.Z;
											bool flag8 = !base.IsPositionGap(num, num2, num3);
											if (flag8)
											{
												result = false;
											}
											else
											{
												float num4 = MathHelper.WrapAngle(this._gameInstance.LocalPlayer.LookOrientation.Y + MathHelper.ToRadians(90f));
												Vector2 vector = new Vector2((float)Math.Cos((double)num4), (float)Math.Sin((double)num4));
												vector.Normalize();
												bool flag9 = Math.Abs(vector.X) > Math.Abs(vector.Y);
												if (flag9)
												{
													bool flag10 = vector.X > 0f;
													if (flag10)
													{
														num += 0.5f;
														this._mantleOffset.X = 0.5f;
													}
													else
													{
														num -= 0.5f;
														this._mantleOffset.X = -0.5f;
													}
													float num5 = Math.Abs(num3);
													bool flag11 = num5 % 1f < 0.3f && !base.IsPositionGap(num, num2 + 2f + 1f, num3 + 1f);
													if (flag11)
													{
														this._mantleOffset.Z = this._mantleOffset.Z - (0.3f - num5 % 1f);
													}
													else
													{
														bool flag12 = num5 % 1f > 0.7f && !base.IsPositionGap(num, num2 + 2f + 1f, num3 - 1f);
														if (flag12)
														{
															this._mantleOffset.Z = this._mantleOffset.Z - (0.7f - num5 % 1f);
														}
													}
												}
												else
												{
													bool flag13 = vector.Y > 0f;
													if (flag13)
													{
														num3 -= 0.5f;
														this._mantleOffset.Z = -0.5f;
													}
													else
													{
														num3 += 0.5f;
														this._mantleOffset.Z = 0.5f;
													}
													float num6 = Math.Abs(num);
													bool flag14 = num6 % 1f < 0.3f && !base.IsPositionGap(num - 1f, num2 + 2f + 1f, num3);
													if (flag14)
													{
														this._mantleOffset.X = 0.3f - num6 % 1f;
													}
													else
													{
														bool flag15 = num6 % 1f > 0.7f && !base.IsPositionGap(num + 1f, num2 + 2f + 1f, num3);
														if (flag15)
														{
															this._mantleOffset.X = 0.7f - num6 % 1f;
														}
													}
												}
												bool flag16 = base.IsPositionGap(num, num2 + 2f, num3);
												if (flag16)
												{
													result = false;
												}
												else
												{
													bool flag17 = !base.IsPositionGap(num, num2 + 2f + 1f, num3);
													if (flag17)
													{
														result = false;
													}
													else
													{
														bool flag18 = !base.IsPositionGap(num, num2 + 2f + 2f, num3);
														if (flag18)
														{
															result = false;
														}
														else
														{
															this._mantleOffset.Y = this._mantleOffset.Y - (1f - this.GetBlockHeight(num, num2 + 2f, num3));
															result = true;
														}
													}
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06004BE6 RID: 19430 RVA: 0x0014254C File Offset: 0x0014074C
		private float GetBlockHeight(float posX, float posY, float posZ)
		{
			int num = (int)Math.Floor((double)posX);
			int num2 = (int)Math.Floor((double)posY);
			int num3 = (int)Math.Floor((double)posZ);
			int block = this._gameInstance.MapModule.GetBlock(num, num2, num3, 1);
			bool flag = block > 0;
			if (flag)
			{
				ClientBlockType clientBlockType = this._gameInstance.MapModule.ClientBlockTypes[block];
				bool flag2 = clientBlockType.FillerX != 0 || clientBlockType.FillerY != 0 || clientBlockType.FillerZ != 0;
				if (flag2)
				{
					num -= clientBlockType.FillerX;
					num2 -= clientBlockType.FillerY;
					num3 -= clientBlockType.FillerZ;
					block = this._gameInstance.MapModule.GetBlock(num, num2, num3, 1);
					clientBlockType = this._gameInstance.MapModule.ClientBlockTypes[block];
				}
				BlockType.Material collisionMaterial = clientBlockType.CollisionMaterial;
				bool flag3 = collisionMaterial != null && collisionMaterial != 2;
				if (flag3)
				{
					BlockHitbox blockHitbox = this._gameInstance.ServerSettings.BlockHitboxes[clientBlockType.HitboxType];
					return blockHitbox.BoundingBox.Max.Y;
				}
			}
			return 0f;
		}

		// Token: 0x06004BE7 RID: 19431 RVA: 0x00142674 File Offset: 0x00140874
		private void Mantle(ref Vector3 checkPos)
		{
			this.MovementStates.IsMantling = true;
			this.MovementStates.IsJumping = false;
			this.MovementStates.IsFalling = false;
			this.MovementStates.IsOnGround = false;
			checkPos.Y = (float)Math.Floor((double)checkPos.Y) + 2f;
			checkPos += this._mantleOffset;
			this._velocity = Vector3.Zero;
			this._mantleDurationLeft = 1f;
			this.MantleCameraOffset.X = -this._mantleOffset.X;
			this.MantleCameraOffset.Y = this._gameInstance.App.Settings.MantlingCameraOffsetY;
			this.MantleCameraOffset.Z = -this._mantleOffset.Z;
			this._previousMantleCameraOffset = this.MantleCameraOffset;
			this._nextMantleCameraOffset = this.MantleCameraOffset;
			this._gameInstance.AudioModule.PlayLocalSoundEvent("MANTLING");
		}

		// Token: 0x06004BE8 RID: 19432 RVA: 0x00142774 File Offset: 0x00140974
		private void UpdateMantle()
		{
			this._mantleDurationLeft -= 0.016666668f;
			this._previousMantleCameraOffset = this._nextMantleCameraOffset;
			this._nextMantleCameraOffset.X = MathHelper.ConvertToNewRange(this._mantleDurationLeft, 1f, 0f, -this._mantleOffset.X, 0f);
			this._nextMantleCameraOffset.Z = MathHelper.ConvertToNewRange(this._mantleDurationLeft, 1f, 0f, -this._mantleOffset.Z, 0f);
			this._nextMantleCameraOffset.Y = MathHelper.ConvertToNewRange(this._mantleDurationLeft, 1f, 0f, this._gameInstance.App.Settings.MantlingCameraOffsetY, 0f);
			bool flag = this._mantleDurationLeft > 0f;
			if (!flag)
			{
				this.MovementStates.IsMantling = false;
				this.MantleCameraOffset = Vector3.Zero;
				this._previousMantleCameraOffset = Vector3.Zero;
				this._nextMantleCameraOffset = Vector3.Zero;
			}
		}

		// Token: 0x06004BE9 RID: 19433 RVA: 0x0014287C File Offset: 0x00140A7C
		private void CheckAndSetRollingState()
		{
			bool flag = this._wasOnGround || !this.MovementStates.IsOnGround;
			if (!flag)
			{
				InputBindings inputBindings = this._gameInstance.App.Settings.InputBindings;
				Input input = this._gameInstance.Input;
				bool flag2 = !input.IsBindingHeld(inputBindings.Crouch, false);
				if (!flag2)
				{
					bool flag3 = this._previousFallingVelocity > -base.MovementSettings.MinFallSpeedToEngageRoll;
					if (!flag3)
					{
						bool flag4 = this._gameInstance.InteractionModule.Chains.Count > 0 && Enumerable.Any<KeyValuePair<int, InteractionChain>>(this._gameInstance.InteractionModule.Chains, new Func<KeyValuePair<int, InteractionChain>, bool>(this.IsDisallowedInteraction));
						if (!flag4)
						{
							this.MovementStates.IsRolling = true;
							this.InitializeRollingForceValues();
						}
					}
				}
			}
		}

		// Token: 0x06004BEA RID: 19434 RVA: 0x00142958 File Offset: 0x00140B58
		private void InitializeRollingForceValues()
		{
			bool flag = !this.MovementStates.IsRolling;
			if (flag)
			{
				this._rollForceDurationLeft = -1f;
			}
			else
			{
				this._rollForceInitialSpeed = base.MovementSettings.BaseSpeed * base.MovementSettings.RollStartSpeedModifier;
				this._curRollSpeedMultiplier = base.MovementSettings.RollStartSpeedModifier;
				this._fallEffectDurationLeft = 0f;
				bool isRolling = this.MovementStates.IsRolling;
				if (isRolling)
				{
					bool flag2 = this._rollForceInitialSpeed < base.MovementSettings.BaseSpeed;
					if (flag2)
					{
						this._rollForceInitialSpeed = base.MovementSettings.BaseSpeed;
					}
					this._rollForceDurationLeft = base.MovementSettings.RollTimeToComplete;
				}
				else
				{
					this._rollForceDurationLeft = base.MovementSettings.RollTimeToComplete;
				}
				bool flag3 = this._rollForceDurationLeft <= 0f;
				if (flag3)
				{
					this._rollForceDurationLeft = -1f;
					this.MovementStates.IsRolling = false;
				}
			}
		}

		// Token: 0x06004BEB RID: 19435 RVA: 0x00142A50 File Offset: 0x00140C50
		private void ComputeAndUpdateRollingForce()
		{
			bool flag = this._rollForceDurationLeft < 0f || !this.MovementStates.IsRolling;
			if (!flag)
			{
				Settings settings = this._gameInstance.App.Settings;
				Easing.EasingType sprintDecelerationEasingType = settings.SprintDecelerationEasingType;
				float rollTimeToComplete = base.MovementSettings.RollTimeToComplete;
				bool flag2 = rollTimeToComplete == this._rollForceDurationLeft && base.MovementSettings.BaseSpeed > 0f;
				if (flag2)
				{
					float value = this._rollForceInitialSpeed * rollTimeToComplete / (base.MovementSettings.BaseSpeed * base.MovementSettings.RollStartSpeedModifier - base.MovementSettings.BaseSpeed * base.MovementSettings.RollExitSpeedModifier);
					this._rollForceDurationLeft = MathHelper.Clamp(value, 0f, rollTimeToComplete);
				}
				this._rollForceProgress = Easing.Ease(sprintDecelerationEasingType, rollTimeToComplete - this._rollForceDurationLeft, 0f, 1f, rollTimeToComplete);
				this._baseSpeedMultiplier = 1f;
				this._curRollSpeedMultiplier = base.MovementSettings.RollStartSpeedModifier - (base.MovementSettings.RollStartSpeedModifier - base.MovementSettings.RollExitSpeedModifier) * this._rollForceProgress;
				this.UpdateRollingDurationLeft();
			}
		}

		// Token: 0x06004BEC RID: 19436 RVA: 0x00142B80 File Offset: 0x00140D80
		private void UpdateRollingDurationLeft()
		{
			this._rollForceDurationLeft -= 0.016666668f;
			bool flag = this._rollForceDurationLeft <= 0f;
			if (flag)
			{
				this._rollForceDurationLeft = 0f;
				this.MovementStates.IsRolling = false;
			}
		}

		// Token: 0x06004BED RID: 19437 RVA: 0x00142BD0 File Offset: 0x00140DD0
		private void UpdateSliding(bool canMove, ref MovementController.InputFrame input)
		{
			InputBindings inputBindings = this._gameInstance.App.Settings.InputBindings;
			float num = (float)Math.Sqrt((double)(this._velocity.X * this._velocity.X + this._velocity.Z * this._velocity.Z));
			bool flag = !this.MovementStates.IsSliding;
			if (flag)
			{
				this.MovementStates.IsSliding = this.CanSlide(canMove, ref input);
			}
			else
			{
				this.MovementStates.IsSliding = (input.IsBindingHeld(inputBindings.Crouch) && canMove && num > base.MovementSettings.SlideExitSpeed && this.MovementStates.IsOnGround);
			}
		}

		// Token: 0x06004BEE RID: 19438 RVA: 0x00142C90 File Offset: 0x00140E90
		private void ComputeSlideForce(ref float wishSpeed)
		{
			bool flag = this._slideForceDurationLeft < 0f || !this.MovementStates.IsSliding || !this.MovementStates.IsOnGround;
			if (!flag)
			{
				wishSpeed = base.MovementSettings.SlideExitSpeed;
				this._baseSpeedMultiplier = 0.55f;
				this._speedMultiplier = ((base.MovementSettings.BaseSpeed <= 0f) ? 0.55f : (wishSpeed / base.MovementSettings.BaseSpeed + 0.55f));
				Settings settings = this._gameInstance.App.Settings;
				Easing.EasingType slideDecelerationEasingType = settings.SlideDecelerationEasingType;
				float slideDecelerationDuration = settings.SlideDecelerationDuration;
				bool flag2 = slideDecelerationDuration == this._slideForceDurationLeft && base.MovementSettings.BaseSpeed > 0f;
				if (flag2)
				{
					float num = base.MovementSettings.SlideExitSpeed - base.MovementSettings.BaseSpeed;
					float baseSpeed = base.MovementSettings.BaseSpeed;
					float num2 = baseSpeed * base.MovementSettings.ForwardCrouchSpeedMultiplier - this._slideForceInitialSpeed;
					float value = num2 * slideDecelerationDuration / num;
					this._slideForceDurationLeft = MathHelper.Clamp(value, 0f, slideDecelerationDuration);
				}
				this._slideForceProgress = Easing.Ease(slideDecelerationEasingType, slideDecelerationDuration - this._slideForceDurationLeft, 0f, 1f, slideDecelerationDuration);
				wishSpeed = this._slideForceInitialSpeed + (wishSpeed - this._slideForceInitialSpeed) * this._slideForceProgress;
				this._baseSpeedMultiplier = 0.55f;
				this._speedMultiplier = ((base.MovementSettings.BaseSpeed <= 0f) ? 0.55f : (wishSpeed / base.MovementSettings.BaseSpeed + 0.02f));
			}
		}

		// Token: 0x06004BEF RID: 19439 RVA: 0x00142E34 File Offset: 0x00141034
		private void UpdateSlidingForceValues()
		{
			bool flag = this.MovementStates.IsSliding == this._previousMovementStates.IsSliding;
			if (!flag)
			{
				bool flag2 = this.MovementStates.IsIdle && !this._previousMovementStates.IsIdle;
				if (flag2)
				{
					this._slideForceDurationLeft = -1f;
				}
				else
				{
					this._slideForceInitialSpeed = this._lastLateralSpeed;
					bool isSliding = this.MovementStates.IsSliding;
					if (isSliding)
					{
						this._slideForceDurationLeft = this._gameInstance.App.Settings.SlideDecelerationDuration;
					}
					bool flag3 = this._slideForceDurationLeft <= 0f;
					if (flag3)
					{
						this._slideForceDurationLeft = -1f;
					}
				}
			}
		}

		// Token: 0x06004BF0 RID: 19440 RVA: 0x00142EEC File Offset: 0x001410EC
		private bool CanSlide(bool canMove, ref MovementController.InputFrame input)
		{
			bool flag = !canMove;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = this._gameInstance.App.Settings.SlideDecelerationDuration <= 0f;
				if (flag2)
				{
					result = false;
				}
				else
				{
					bool flag3 = !this.MovementStates.IsOnGround;
					if (flag3)
					{
						result = false;
					}
					else
					{
						bool isRolling = this.MovementStates.IsRolling;
						if (isRolling)
						{
							result = false;
						}
						else
						{
							InputBindings inputBindings = this._gameInstance.App.Settings.InputBindings;
							bool flag4 = !input.IsBindingHeld(inputBindings.Crouch);
							if (flag4)
							{
								result = false;
							}
							else
							{
								float num = (float)Math.Sqrt((double)(this._velocity.X * this._velocity.X + this._velocity.Z * this._velocity.Z));
								bool flag5 = num < base.MovementSettings.MinSlideEntrySpeed;
								if (flag5)
								{
									result = false;
								}
								else
								{
									bool flag6 = this._gameInstance.InteractionModule.Chains.Count > 0 && Enumerable.Any<KeyValuePair<int, InteractionChain>>(this._gameInstance.InteractionModule.Chains, new Func<KeyValuePair<int, InteractionChain>, bool>(this.IsDisallowedInteraction));
									result = !flag6;
								}
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x0400276D RID: 10093
		private const float AutoJumpMaxHeight = 0.625f;

		// Token: 0x0400276E RID: 10094
		private const int DoubleJumpMaxDelay = 300;

		// Token: 0x0400276F RID: 10095
		private const float MaxCycleMovement = 0.25f;

		// Token: 0x04002770 RID: 10096
		private const int SurfaceCheckPadding = 3;

		// Token: 0x04002771 RID: 10097
		private const float BaseForwardWalkSpeedMultiplier = 0.3f;

		// Token: 0x04002772 RID: 10098
		private const float BaseBackwardWalkSpeedMultiplier = 0.3f;

		// Token: 0x04002773 RID: 10099
		private const float BaseStrafeWalkSpeedMultiplier = 0.3f;

		// Token: 0x04002774 RID: 10100
		private const float BaseForwardRunSpeedMultiplier = 1f;

		// Token: 0x04002775 RID: 10101
		private const float BaseBackwardRunSpeedMultiplier = 0.65f;

		// Token: 0x04002776 RID: 10102
		private const float BaseForwardSlideSpeedMultiplier = 0.55f;

		// Token: 0x04002777 RID: 10103
		private const float BaseStrafeRunSpeedMultiplier = 0.8f;

		// Token: 0x04002778 RID: 10104
		private const float BaseForwardCrouchSpeedMultiplier = 0.55f;

		// Token: 0x04002779 RID: 10105
		private const float BaseBackwardCrouchSpeedMultiplier = 0.4f;

		// Token: 0x0400277A RID: 10106
		private const float BaseStrafeCrouchSpeedMultiplier = 0.45f;

		// Token: 0x0400277B RID: 10107
		private const float BaseForwardSprintSpeedMultiplier = 1.65f;

		// Token: 0x0400277C RID: 10108
		private const float BaseForwardRollSpeedMultiplier = 1f;

		// Token: 0x0400277D RID: 10109
		private const double CurveModifier = 0.75;

		// Token: 0x0400277E RID: 10110
		private const double CurveMultiplier = 2.0;

		// Token: 0x0400277F RID: 10111
		private const double MinVelocity = 18.0;

		// Token: 0x04002780 RID: 10112
		private const double MinMultiplier = 10.0;

		// Token: 0x04002781 RID: 10113
		private const float MaxCurve = 0.3f;

		// Token: 0x04002782 RID: 10114
		private const string FallPenaltyWwiseId = "PLAYER_LAND_PENALTY_MAJOR";

		// Token: 0x04002783 RID: 10115
		private static readonly HitDetection.RaycastOptions FallRaycastOptions = new HitDetection.RaycastOptions
		{
			Distance = 1f,
			IgnoreEmptyCollisionMaterial = true
		};

		// Token: 0x04002784 RID: 10116
		private ClientBlockType _blockUnderFeet;

		// Token: 0x04002785 RID: 10117
		private ClientBlockType _lastBlockUnderFeet;

		// Token: 0x04002786 RID: 10118
		public const string Id = "Default";

		// Token: 0x04002787 RID: 10119
		public const float KnockbackSimulationTime = 1.5f;

		// Token: 0x04002788 RID: 10120
		private const string AllowsMovementTag = "Allows=Movement";

		// Token: 0x04002789 RID: 10121
		private const float HeightShiftStep = 0.1f;

		// Token: 0x0400278A RID: 10122
		private const int MaxJumpCombos = 3;

		// Token: 0x0400278B RID: 10123
		private const float MaxJumpComboFactor = 0.5f;

		// Token: 0x0400278C RID: 10124
		private const float BaseSprintForceMultiplier = 0.4f;

		// Token: 0x0400278D RID: 10125
		private ClientMovementStates _previousMovementStates = ClientMovementStates.Idle;

		// Token: 0x0400278E RID: 10126
		private float _previousFallingVelocity = 0f;

		// Token: 0x0400278F RID: 10127
		private float _baseSpeedMultiplier = 1f;

		// Token: 0x04002790 RID: 10128
		private float _speedMultiplier = 1f;

		// Token: 0x04002791 RID: 10129
		public new float CurrentSpeedMultiplierDiff;

		// Token: 0x04002792 RID: 10130
		private bool _wasOnGround;

		// Token: 0x04002793 RID: 10131
		private bool _touchedGround;

		// Token: 0x04002794 RID: 10132
		private float _acceleration;

		// Token: 0x04002795 RID: 10133
		private HitDetection.RaycastHit _groundHit;

		// Token: 0x04002796 RID: 10134
		private bool _wasClimbing;

		// Token: 0x04002797 RID: 10135
		private Vector3 _climbingBlockPosition = Vector3.Zero;

		// Token: 0x04002798 RID: 10136
		private int _climbingBlockId;

		// Token: 0x04002799 RID: 10137
		private Vector3? _knockbackHitPosition;

		// Token: 0x0400279A RID: 10138
		private Vector3 _requestedVelocity;

		// Token: 0x0400279B RID: 10139
		private ChangeVelocityType? _requestedVelocityChangeType;

		// Token: 0x0400279C RID: 10140
		private int _autoJumpFrame;

		// Token: 0x0400279D RID: 10141
		private int _autoJumpFrameCount;

		// Token: 0x0400279E RID: 10142
		private float _autoJumpHeight;

		// Token: 0x0400279F RID: 10143
		private float _previousAutoJumpHeightShift;

		// Token: 0x040027A0 RID: 10144
		private float _nextAutoJumpHeightShift;

		// Token: 0x040027A1 RID: 10145
		private long _jumpReleaseTime;

		// Token: 0x040027A2 RID: 10146
		private bool _wasHoldingJump;

		// Token: 0x040027A3 RID: 10147
		private bool _canStartRunning;

		// Token: 0x040027A4 RID: 10148
		private float _sprintForceInitialSpeed;

		// Token: 0x040027A5 RID: 10149
		private float _lastLateralSpeed;

		// Token: 0x040027A6 RID: 10150
		private float _targetCrouchHeightShift;

		// Token: 0x040027A7 RID: 10151
		private float _previousCrouchHeightShift;

		// Token: 0x040027A8 RID: 10152
		private float _nextCrouchHeightShift;

		// Token: 0x040027A9 RID: 10153
		private bool _fluidJump;

		// Token: 0x040027AA RID: 10154
		private float _swimJumpLastY;

		// Token: 0x040027AB RID: 10155
		private int _jumpCombo;

		// Token: 0x040027AC RID: 10156
		private readonly FluidFX.FluidFXMovementSettings _averageFluidMovementSettings;

		// Token: 0x040027AD RID: 10157
		private float _jumpBufferDurationLeft;

		// Token: 0x040027AE RID: 10158
		private float _jumpInputVelocity;

		// Token: 0x040027AF RID: 10159
		private bool _jumpInputConsumed = true;

		// Token: 0x040027B0 RID: 10160
		private bool _jumpInputReleased = true;

		// Token: 0x040027B1 RID: 10161
		private bool _wasFalling;

		// Token: 0x040027B2 RID: 10162
		private float _yStartFalling;

		// Token: 0x040027B3 RID: 10163
		private float _yStartInAir;

		// Token: 0x040027B4 RID: 10164
		private float _fallEffectDurationLeft;

		// Token: 0x040027B5 RID: 10165
		private float _fallEffectSpeedMultiplier;

		// Token: 0x040027B6 RID: 10166
		private float _fallEffectJumpForce;

		// Token: 0x040027B7 RID: 10167
		private float _consecutiveMomentumLoss;

		// Token: 0x040027B8 RID: 10168
		private bool _fallEffectToApply;

		// Token: 0x040027B9 RID: 10169
		private float _jumpObstacleDurationLeft;

		// Token: 0x040027BA RID: 10170
		private const string MantlingWWiseId = "MANTLING";

		// Token: 0x040027BB RID: 10171
		private const float MantleDuration = 1f;

		// Token: 0x040027BC RID: 10172
		private const float BlockLeftMargin = 0.3f;

		// Token: 0x040027BD RID: 10173
		private const float BlockRightMargin = 0.7f;

		// Token: 0x040027BE RID: 10174
		private const float HalfBlockSize = 0.5f;

		// Token: 0x040027BF RID: 10175
		private const float MantleJumpHeight = 2f;

		// Token: 0x040027C0 RID: 10176
		private const float ModelHeight = 1.8f;

		// Token: 0x040027C1 RID: 10177
		private Vector3 _nextMantleCameraOffset = Vector3.Zero;

		// Token: 0x040027C2 RID: 10178
		private Vector3 _previousMantleCameraOffset = Vector3.Zero;

		// Token: 0x040027C3 RID: 10179
		private float _mantleDurationLeft;

		// Token: 0x040027C4 RID: 10180
		private Vector3 _mantleOffset = Vector3.Zero;

		// Token: 0x040027C5 RID: 10181
		private float _rollForceDurationLeft = -1f;

		// Token: 0x040027C6 RID: 10182
		private float _rollForceProgress;

		// Token: 0x040027C7 RID: 10183
		private float _rollForceInitialSpeed = 0f;

		// Token: 0x040027C8 RID: 10184
		private float _curRollSpeedMultiplier = 1f;

		// Token: 0x040027C9 RID: 10185
		private float _slideForceInitialSpeed = 0f;

		// Token: 0x040027CA RID: 10186
		private const float BaseSlideForceMultiplier = 0.02f;

		// Token: 0x040027CB RID: 10187
		private float _slideForceDurationLeft = -1f;

		// Token: 0x040027CC RID: 10188
		private float _slideForceProgress;
	}
}
