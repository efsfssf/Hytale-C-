using System;
using System.Collections.Generic;
using HytaleClient.Core;
using HytaleClient.Data.Entities;
using HytaleClient.Data.Map;
using HytaleClient.Data.UserSettings;
using HytaleClient.InGame.Modules.Camera.Controllers;
using HytaleClient.InGame.Modules.Entities;
using HytaleClient.Math;
using HytaleClient.Protocol;

namespace HytaleClient.InGame.Modules.CharacterController
{
	// Token: 0x0200096D RID: 2413
	internal abstract class MovementController
	{
		// Token: 0x170011FF RID: 4607
		// (get) Token: 0x06004B68 RID: 19304 RVA: 0x00136A86 File Offset: 0x00134C86
		// (set) Token: 0x06004B69 RID: 19305 RVA: 0x00136A8E File Offset: 0x00134C8E
		public float DefaultBlockDrag { get; set; } = 0.82f;

		// Token: 0x17001200 RID: 4608
		// (get) Token: 0x06004B6A RID: 19306 RVA: 0x00136A97 File Offset: 0x00134C97
		// (set) Token: 0x06004B6B RID: 19307 RVA: 0x00136A9F File Offset: 0x00134C9F
		public float DefaultBlockFriction { get; set; } = 0.18f;

		// Token: 0x17001201 RID: 4609
		// (get) Token: 0x06004B6C RID: 19308 RVA: 0x00136AA8 File Offset: 0x00134CA8
		// (set) Token: 0x06004B6D RID: 19309 RVA: 0x00136AB0 File Offset: 0x00134CB0
		public float AutoJumpHeightShift { get; protected set; }

		// Token: 0x17001202 RID: 4610
		// (get) Token: 0x06004B6E RID: 19310 RVA: 0x00136AB9 File Offset: 0x00134CB9
		// (set) Token: 0x06004B6F RID: 19311 RVA: 0x00136AC1 File Offset: 0x00134CC1
		public float CrouchHeightShift { get; protected set; }

		// Token: 0x17001203 RID: 4611
		// (get) Token: 0x06004B70 RID: 19312 RVA: 0x00136ACA File Offset: 0x00134CCA
		// (set) Token: 0x06004B71 RID: 19313 RVA: 0x00136AD2 File Offset: 0x00134CD2
		public bool SkipHitDetectionWhenFlying { get; protected set; }

		// Token: 0x17001204 RID: 4612
		// (get) Token: 0x06004B72 RID: 19314 RVA: 0x00136ADB File Offset: 0x00134CDB
		// (set) Token: 0x06004B73 RID: 19315 RVA: 0x00136AE3 File Offset: 0x00134CE3
		public bool ApplyMarioFallForce { get; set; }

		// Token: 0x17001205 RID: 4613
		// (get) Token: 0x06004B74 RID: 19316 RVA: 0x00136AEC File Offset: 0x00134CEC
		// (set) Token: 0x06004B75 RID: 19317 RVA: 0x00136AF4 File Offset: 0x00134CF4
		public Vector2 WishDirection { get; protected set; }

		// Token: 0x17001206 RID: 4614
		// (get) Token: 0x06004B76 RID: 19318 RVA: 0x00136AFD File Offset: 0x00134CFD
		public Vector3 Velocity
		{
			get
			{
				return this._velocity;
			}
		}

		// Token: 0x17001207 RID: 4615
		// (get) Token: 0x06004B77 RID: 19319 RVA: 0x00136B05 File Offset: 0x00134D05
		// (set) Token: 0x06004B78 RID: 19320 RVA: 0x00136B0D File Offset: 0x00134D0D
		public MovementSettings MovementSettings { get; set; } = new MovementSettings();

		// Token: 0x06004B79 RID: 19321 RVA: 0x00136B18 File Offset: 0x00134D18
		protected MovementController(GameInstance gameInstance)
		{
			this._gameInstance = gameInstance;
			this._flyCameraController = new FlyCameraController(gameInstance);
		}

		// Token: 0x06004B7A RID: 19322
		public abstract void Tick();

		// Token: 0x06004B7B RID: 19323
		public abstract void PreUpdate(float timeFraction);

		// Token: 0x06004B7C RID: 19324
		public abstract void ApplyKnockback(ApplyKnockback packet);

		// Token: 0x06004B7D RID: 19325
		public abstract void ApplyMovementOffset(Vector3 movementOffset);

		// Token: 0x06004B7E RID: 19326
		public abstract void RequestVelocityChange(float x, float y, float z, ChangeVelocityType changeType, VelocityConfig config);

		// Token: 0x06004B7F RID: 19327
		public abstract void VelocityChange(float x, float y, float z, ChangeVelocityType changeType, VelocityConfig config);

		// Token: 0x06004B80 RID: 19328
		public abstract Vector2 GetWishDirection();

		// Token: 0x06004B81 RID: 19329 RVA: 0x00136C4C File Offset: 0x00134E4C
		public void SetSavedMovementStates(SavedMovementStates movementStates)
		{
			this.MovementStates.IsFlying = movementStates.Flying;
		}

		// Token: 0x06004B82 RID: 19330 RVA: 0x00136C60 File Offset: 0x00134E60
		public void SetFirstPersonCameraOffset(float time, Vector3 position, Vector3 rotation)
		{
			this._firstPersonCameraLerpTime = time;
			this._firstPersonCurrentCameraLerpTime = 0f;
			this._firstPersonPositionOffsetLast = this._firstPersonPositionOffsetTarget;
			this._firstPersonPositionOffsetTarget = position;
			this._firstPersonRotationOffsetLast = this._firstPersonRotationOffsetTarget;
			this._firstPersonRotationOffsetTarget = rotation;
		}

		// Token: 0x06004B83 RID: 19331 RVA: 0x00136C9B File Offset: 0x00134E9B
		public void SetThirdPersonCameraOffset(float time, Vector3 position, Vector3 rotation)
		{
			this._thirdPersonCameraLerpTime = time;
			this._thirdPersonCurrentCameraLerpTime = 0f;
			this._thirdPersonPositionOffsetLast = this._thirdPersonPositionOffsetTarget;
			this._thirdPersonPositionOffsetTarget = position;
			this._thirdPersonRotationOffsetLast = this._thirdPersonRotationOffsetTarget;
			this._thirdPersonRotationOffsetTarget = rotation;
		}

		// Token: 0x06004B84 RID: 19332 RVA: 0x00136CD8 File Offset: 0x00134ED8
		public void UpdateMovementSettings(MovementSettings movementSettings)
		{
			this.MovementSettings = movementSettings;
			bool flag = this.MovementSettings.BaseSpeed == 0f;
			if (flag)
			{
				this._velocity.X = 0f;
				this._velocity.Z = 0f;
			}
			bool flag2 = this.MovementSettings.JumpForce == 0f;
			if (flag2)
			{
				this._velocity.Y = 0f;
			}
		}

		// Token: 0x06004B85 RID: 19333 RVA: 0x00136D50 File Offset: 0x00134F50
		protected void UpdateInputSettings()
		{
			InputBindings inputBindings = this._gameInstance.App.Settings.InputBindings;
			bool flag = this._gameInstance.GameMode == 1;
			if (flag)
			{
				bool flag2 = this._gameInstance.Input.ConsumeBinding(inputBindings.DecreaseSpeedMultiplier, false);
				if (flag2)
				{
					float num = (this.SpeedMultiplier >= 2f) ? 1f : 0.1f;
					this.SpeedMultiplier = (float)Math.Round((double)(Math.Max(this.MovementSettings.MinSpeedMultiplier, this.SpeedMultiplier - num) * 10f)) * 0.1f;
					this._gameInstance.Chat.Log(string.Format("Speed Multiplier: {0}", this.SpeedMultiplier));
				}
				bool flag3 = this._gameInstance.Input.ConsumeBinding(inputBindings.IncreaseSpeedMultiplier, false);
				if (flag3)
				{
					float num2 = (this.SpeedMultiplier >= 1f) ? 1f : 0.1f;
					this.SpeedMultiplier = (float)Math.Round((double)(Math.Min(this.MovementSettings.MaxSpeedMultiplier, this.SpeedMultiplier + num2) * 10f)) * 0.1f;
					this._gameInstance.Chat.Log(string.Format("Speed Multiplier: {0}", this.SpeedMultiplier));
				}
				bool flag4 = this._gameInstance.Input.ConsumeBinding(inputBindings.ToggleCreativeCollision, false);
				if (flag4)
				{
					bool flag5 = !this.MovementStates.IsFlying || !this.SkipHitDetectionWhenFlying;
					if (flag5)
					{
						this.MovementStates.IsFlying = true;
						this.SkipHitDetectionWhenFlying = true;
						this._gameInstance.Chat.Log("Collision Disabled");
					}
					else
					{
						this.SkipHitDetectionWhenFlying = false;
						this._gameInstance.Chat.Log("Collision Enabled");
					}
				}
			}
			bool flag6 = this._gameInstance.Input.ConsumeBinding(inputBindings.ToggleFlyCamera, false);
			if (flag6)
			{
				bool flag7 = this._gameInstance.Input.IsShiftHeld();
				if (flag7)
				{
					this._flyCameraController.ResetPosition();
				}
				else
				{
					bool flag8 = this._gameInstance.CameraModule.Controller == this._flyCameraController;
					if (flag8)
					{
						this._gameInstance.CameraModule.ResetCameraController();
						this._gameInstance.Chat.Log("Fly Camera Disabled");
					}
					else
					{
						this._gameInstance.CameraModule.SetCustomCameraController(this._flyCameraController);
						this._gameInstance.Chat.Log("Fly Camera Enabled");
					}
				}
			}
			bool flag9 = this._gameInstance.CameraModule.Controller == this._flyCameraController && this._gameInstance.Input.ConsumeBinding(inputBindings.ToggleFlyCameraControlTarget, false);
			if (flag9)
			{
				this._flyCameraController.ToggleControlTarget();
			}
		}

		// Token: 0x06004B86 RID: 19334 RVA: 0x00137040 File Offset: 0x00135240
		protected void UpdateCameraSettings()
		{
			bool flag = this._firstPersonCurrentCameraLerpTime < this._firstPersonCameraLerpTime;
			if (flag)
			{
				this._firstPersonCurrentCameraLerpTime += 0.016666668f;
			}
			bool flag2 = this._firstPersonCurrentCameraLerpTime > this._firstPersonCameraLerpTime;
			if (flag2)
			{
				this._firstPersonCurrentCameraLerpTime = this._firstPersonCameraLerpTime;
			}
			bool flag3 = this._thirdPersonCurrentCameraLerpTime < this._thirdPersonCameraLerpTime;
			if (flag3)
			{
				this._thirdPersonCurrentCameraLerpTime += 0.016666668f;
			}
			bool flag4 = this._thirdPersonCurrentCameraLerpTime > this._thirdPersonCameraLerpTime;
			if (flag4)
			{
				this._thirdPersonCurrentCameraLerpTime = this._thirdPersonCameraLerpTime;
			}
			bool flag5 = this._firstPersonCurrentCameraLerpTime == this._firstPersonCameraLerpTime;
			if (flag5)
			{
				this.FirstPersonPositionOffset = this._firstPersonPositionOffsetTarget;
			}
			else
			{
				this.FirstPersonPositionOffset = Vector3.Lerp(this._firstPersonPositionOffsetLast, this._firstPersonPositionOffsetTarget, this._firstPersonCurrentCameraLerpTime / this._firstPersonCameraLerpTime);
			}
			bool flag6 = this._firstPersonCurrentCameraLerpTime == this._firstPersonCameraLerpTime;
			if (flag6)
			{
				this.FirstPersonRotationOffset = this._firstPersonRotationOffsetTarget;
			}
			else
			{
				this.FirstPersonRotationOffset = Vector3.LerpAngle(this._firstPersonRotationOffsetLast, this._firstPersonRotationOffsetTarget, this._firstPersonCurrentCameraLerpTime / this._firstPersonCameraLerpTime);
			}
			bool flag7 = this._thirdPersonCurrentCameraLerpTime == this._thirdPersonCameraLerpTime;
			if (flag7)
			{
				this.ThirdPersonPositionOffset = this._thirdPersonPositionOffsetTarget;
			}
			else
			{
				this.ThirdPersonPositionOffset = Vector3.Lerp(this._thirdPersonPositionOffsetLast, this._thirdPersonPositionOffsetTarget, this._thirdPersonCurrentCameraLerpTime / this._thirdPersonCameraLerpTime);
			}
			bool flag8 = this._thirdPersonCurrentCameraLerpTime == this._thirdPersonCameraLerpTime;
			if (flag8)
			{
				this.ThirdPersonRotationOffset = this._thirdPersonRotationOffsetTarget;
			}
			else
			{
				this.ThirdPersonRotationOffset = Vector3.LerpAngle(this._thirdPersonRotationOffsetLast, this._thirdPersonRotationOffsetTarget, this._thirdPersonCurrentCameraLerpTime / this._thirdPersonCameraLerpTime);
			}
		}

		// Token: 0x06004B87 RID: 19335 RVA: 0x001371F4 File Offset: 0x001353F4
		protected void ComputeWishDirection(bool forceMoveForward, bool canMove, MovementController.InputFrame input, InputBindings inputBindings)
		{
			bool flag = forceMoveForward || (canMove && input.IsBindingHeld(inputBindings.MoveForwards) && !input.IsBindingHeld(inputBindings.MoveBackwards));
			if (flag)
			{
				this._wishDirection.Y = MathHelper.Step(this._wishDirection.Y, 1f, this.MovementSettings.WishDirectionWeightY);
			}
			else
			{
				bool flag2 = canMove && input.IsBindingHeld(inputBindings.MoveBackwards) && !input.IsBindingHeld(inputBindings.MoveForwards);
				if (flag2)
				{
					this._wishDirection.Y = MathHelper.Step(this._wishDirection.Y, -1f, this.MovementSettings.WishDirectionWeightY);
				}
				else
				{
					this._wishDirection.Y = MathHelper.Step(this._wishDirection.Y, 0f, this.MovementSettings.WishDirectionGravityY);
				}
			}
			bool flag3 = canMove && input.IsBindingHeld(inputBindings.StrafeRight) && !input.IsBindingHeld(inputBindings.StrafeLeft);
			if (flag3)
			{
				this._wishDirection.X = MathHelper.Step(this._wishDirection.X, 1f, this.MovementSettings.WishDirectionWeightX);
			}
			else
			{
				bool flag4 = canMove && input.IsBindingHeld(inputBindings.StrafeLeft) && !input.IsBindingHeld(inputBindings.StrafeRight);
				if (flag4)
				{
					this._wishDirection.X = MathHelper.Step(this._wishDirection.X, -1f, this.MovementSettings.WishDirectionWeightX);
				}
				else
				{
					this._wishDirection.X = MathHelper.Step(this._wishDirection.X, 0f, this.MovementSettings.WishDirectionGravityX);
				}
			}
			this.ComputeEntityEffectWishDirection();
		}

		// Token: 0x06004B88 RID: 19336 RVA: 0x001373CC File Offset: 0x001355CC
		protected void ComputeEntityEffectWishDirection()
		{
			PlayerEntity localPlayer = this._gameInstance.LocalPlayer;
			bool flag = localPlayer == null || this._gameInstance.GameMode == 1;
			if (!flag)
			{
				bool flag2 = this._wishDirection.Y > 0f && localPlayer.DisableForward;
				if (flag2)
				{
					this._wishDirection.Y = 0f;
				}
				else
				{
					bool flag3 = this._wishDirection.Y < 0f && localPlayer.DisableBackward;
					if (flag3)
					{
						this._wishDirection.Y = 0f;
					}
				}
				bool flag4 = this._wishDirection.X > 0f && localPlayer.DisableRight;
				if (flag4)
				{
					this._wishDirection.X = 0f;
				}
				else
				{
					bool flag5 = this._wishDirection.X < 0f && localPlayer.DisableLeft;
					if (flag5)
					{
						this._wishDirection.X = 0f;
					}
				}
			}
		}

		// Token: 0x06004B89 RID: 19337 RVA: 0x001374C8 File Offset: 0x001356C8
		protected bool CheckCollision(Vector3 position, Vector3 moveOffset, BoundingBox boundingBox, HitDetection.CollisionAxis axis, out HitDetection.CollisionHitData hitData)
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
				Entity entity = allEntities[l];
				bool flag3 = entity.HitboxCollisionConfigIndex == -1;
				if (!flag3)
				{
					bool flag4 = !entity.IsTangible();
					if (!flag4)
					{
						BoundingBox hitbox = entity.Hitbox;
						hitbox.Grow(this.EntityHitboxExpand * 2f);
						ClientHitboxCollisionConfig clientHitboxCollisionConfig = this._gameInstance.ServerSettings.HitboxCollisionConfigs[entity.HitboxCollisionConfigIndex];
						bool flag5 = clientHitboxCollisionConfig.CollisionType > ClientHitboxCollisionConfig.ClientCollisionType.Hard;
						if (!flag5)
						{
							Vector3 position2 = entity.Position;
							HitDetection.CollisionHitData collisionHitData2;
							bool flag6 = !HitDetection.CheckBoxCollision(boundingBox, hitbox, position2.X, position2.Y + 0.0001f, position2.Z, moveOffset, out collisionHitData2);
							if (!flag6)
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
								bool flag7 = num4 == 0f || num9 > num4;
								if (flag7)
								{
									hitData = collisionHitData2;
									hitData.HitEntity = new int?(entity.NetworkId);
									num4 = num9;
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
			return num4 > 0f;
		}

		// Token: 0x06004B8A RID: 19338 RVA: 0x00137804 File Offset: 0x00135A04
		protected bool IsPositionGap(Vector3 pos)
		{
			return this.IsPositionGap(pos.X, pos.Y, pos.Z);
		}

		// Token: 0x06004B8B RID: 19339 RVA: 0x00137830 File Offset: 0x00135A30
		protected bool IsPositionGap(float posX, float posY, float posZ)
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
					return false;
				}
			}
			return true;
		}

		// Token: 0x06004B8C RID: 19340 RVA: 0x0013792C File Offset: 0x00135B2C
		protected ref MovementController.StateFrame CaptureStateFrame(Input input, InputBindings bindings)
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
			ptr.Position = this._gameInstance.LocalPlayer.Position;
			ptr.MovementForceRotation = this._gameInstance.CameraModule.Controller.MovementForceRotation;
			ptr.MovementStates = this.MovementStates;
			return ref ptr;
		}

		// Token: 0x06004B8D RID: 19341 RVA: 0x00137AA8 File Offset: 0x00135CA8
		public void InvalidateState()
		{
			for (int i = 0; i < this._stateFrames.Length; i++)
			{
				this._stateFrames[i].Valid = false;
			}
			this._stateFrameOffset = 0;
		}

		// Token: 0x06004B8E RID: 19342 RVA: 0x00137AE8 File Offset: 0x00135CE8
		protected int? FindMatchingStateFrame(MovementController.StatePredicate matcher)
		{
			int num = this._stateFrameOffset - 1;
			for (num = (this._stateFrames.Length + num) % this._stateFrames.Length; num != this._stateFrameOffset; num = (this._stateFrames.Length + num) % this._stateFrames.Length)
			{
				ref MovementController.StateFrame ptr = ref this._stateFrames[num];
				bool flag = !ptr.Valid;
				if (flag)
				{
					break;
				}
				bool flag2 = matcher(ref ptr);
				if (flag2)
				{
					return new int?(num);
				}
				num--;
			}
			return null;
		}

		// Token: 0x06004B8F RID: 19343 RVA: 0x00137B84 File Offset: 0x00135D84
		protected int FindNearestStateFrame(Vector3 position, float ping)
		{
			int num = this._stateFrameOffset - 1;
			num = (this._stateFrames.Length + num) % this._stateFrames.Length;
			int? num2 = null;
			float num3 = float.MaxValue;
			while (num != this._stateFrameOffset)
			{
				ref MovementController.StateFrame ptr = ref this._stateFrames[num];
				bool flag = !ptr.Valid;
				if (flag)
				{
					break;
				}
				float num4 = (ptr.Position - position).LengthSquared();
				bool flag2 = num4 < 0.001f;
				if (flag2)
				{
					return num;
				}
				bool flag3 = num4 < num3;
				if (flag3)
				{
					num3 = num4;
					num2 = new int?(num);
				}
				num--;
				num = (this._stateFrames.Length + num) % this._stateFrames.Length;
			}
			bool flag4 = num2 != null && num3 < 0.25f;
			if (flag4)
			{
				return num2.Value;
			}
			num = this._stateFrameOffset - (int)Math.Ceiling((double)(ping / 0.016666668f));
			for (num = (this._stateFrames.Length + num % this._stateFrames.Length) % this._stateFrames.Length; num != this._stateFrameOffset; num %= this._stateFrames.Length)
			{
				ref MovementController.StateFrame ptr2 = ref this._stateFrames[num];
				bool valid = ptr2.Valid;
				if (valid)
				{
					return num;
				}
				num++;
			}
			return num;
		}

		// Token: 0x040026FC RID: 9980
		public static MovementController.DebugMovement DebugMovementMode;

		// Token: 0x040026FD RID: 9981
		protected const int CollisionCheckRadius = 2;

		// Token: 0x040026FE RID: 9982
		protected const float CollisionPadding = 0.0001f;

		// Token: 0x040026FF RID: 9983
		protected const float VelocityEpsilon = 1E-07f;

		// Token: 0x04002702 RID: 9986
		public float SpeedMultiplier = 1f;

		// Token: 0x04002703 RID: 9987
		public bool MovementEnabled;

		// Token: 0x04002704 RID: 9988
		public float RunningKnockbackRemainingTime;

		// Token: 0x04002705 RID: 9989
		public float CurrentSpeedMultiplierDiff;

		// Token: 0x04002706 RID: 9990
		public float SprintForceDurationLeft = -1f;

		// Token: 0x04002707 RID: 9991
		public float SprintForceProgress;

		// Token: 0x0400270C RID: 9996
		public float RaycastDistance;

		// Token: 0x0400270D RID: 9997
		public float RaycastHeightOffset;

		// Token: 0x0400270E RID: 9998
		public Interaction.RaycastMode RaycastMode = 0;

		// Token: 0x0400270F RID: 9999
		public Vector3 LastMoveForce;

		// Token: 0x04002711 RID: 10001
		public Vector3 MovementOffset = Vector3.Zero;

		// Token: 0x04002712 RID: 10002
		public Vector3 PreviousMovementOffset = Vector3.Zero;

		// Token: 0x04002713 RID: 10003
		public ClientMovementStates MovementStates = ClientMovementStates.Idle;

		// Token: 0x04002715 RID: 10005
		public readonly HashSet<int> CollidedEntities = new HashSet<int>();

		// Token: 0x04002716 RID: 10006
		public readonly Vector3 EntityHitboxExpand = new Vector3(0.125f, 0f, 0.125f);

		// Token: 0x04002717 RID: 10007
		public Vector3 FirstPersonPositionOffset;

		// Token: 0x04002718 RID: 10008
		public Vector3 FirstPersonRotationOffset;

		// Token: 0x04002719 RID: 10009
		public Vector3 ThirdPersonPositionOffset;

		// Token: 0x0400271A RID: 10010
		public Vector3 ThirdPersonRotationOffset;

		// Token: 0x0400271B RID: 10011
		public Vector3 MantleCameraOffset = Vector3.Zero;

		// Token: 0x0400271C RID: 10012
		public Vector3 CameraRotation;

		// Token: 0x0400271D RID: 10013
		protected GameInstance _gameInstance;

		// Token: 0x0400271E RID: 10014
		protected Vector3 _velocity;

		// Token: 0x0400271F RID: 10015
		protected Vector2 _wishDirection;

		// Token: 0x04002720 RID: 10016
		protected readonly FlyCameraController _flyCameraController;

		// Token: 0x04002721 RID: 10017
		protected Vector3 _movementForceRotation;

		// Token: 0x04002722 RID: 10018
		protected float _firstPersonCameraLerpTime;

		// Token: 0x04002723 RID: 10019
		protected float _firstPersonCurrentCameraLerpTime;

		// Token: 0x04002724 RID: 10020
		protected Vector3 _firstPersonPositionOffsetLast = Vector3.Zero;

		// Token: 0x04002725 RID: 10021
		protected Vector3 _firstPersonPositionOffsetTarget = Vector3.Zero;

		// Token: 0x04002726 RID: 10022
		protected Vector3 _firstPersonRotationOffsetLast = Vector3.Zero;

		// Token: 0x04002727 RID: 10023
		protected Vector3 _firstPersonRotationOffsetTarget = Vector3.Zero;

		// Token: 0x04002728 RID: 10024
		protected float _thirdPersonCameraLerpTime;

		// Token: 0x04002729 RID: 10025
		protected float _thirdPersonCurrentCameraLerpTime;

		// Token: 0x0400272A RID: 10026
		protected Vector3 _thirdPersonPositionOffsetLast = Vector3.Zero;

		// Token: 0x0400272B RID: 10027
		protected Vector3 _thirdPersonPositionOffsetTarget = Vector3.Zero;

		// Token: 0x0400272C RID: 10028
		protected Vector3 _thirdPersonRotationOffsetLast = Vector3.Zero;

		// Token: 0x0400272D RID: 10029
		protected Vector3 _thirdPersonRotationOffsetTarget = Vector3.Zero;

		// Token: 0x0400272E RID: 10030
		protected readonly List<MovementController.AppliedVelocity> _appliedVelocities = new List<MovementController.AppliedVelocity>();

		// Token: 0x0400272F RID: 10031
		protected bool _collisionForward;

		// Token: 0x04002730 RID: 10032
		protected bool _collisionBackward;

		// Token: 0x04002731 RID: 10033
		protected bool _collisionLeft;

		// Token: 0x04002732 RID: 10034
		protected bool _collisionRight;

		// Token: 0x04002733 RID: 10035
		protected const int MaxResolveSteps = 100;

		// Token: 0x04002734 RID: 10036
		private const float MaxStateFrameHistoryS = 0.3f;

		// Token: 0x04002735 RID: 10037
		protected MovementController.StateFrame[] _stateFrames = new MovementController.StateFrame[(int)Math.Ceiling(18.0)];

		// Token: 0x04002736 RID: 10038
		protected int _stateFrameOffset;

		// Token: 0x02000E5E RID: 3678
		public enum DebugMovement
		{
			// Token: 0x04004633 RID: 17971
			None,
			// Token: 0x04004634 RID: 17972
			RunCycle,
			// Token: 0x04004635 RID: 17973
			RunCycleJump
		}

		// Token: 0x02000E5F RID: 3679
		protected class AppliedVelocity
		{
			// Token: 0x0600678E RID: 26510 RVA: 0x00218213 File Offset: 0x00216413
			public AppliedVelocity(Vector3 velocity, VelocityConfig config)
			{
				this.Velocity = velocity;
				this.Config = config;
			}

			// Token: 0x04004636 RID: 17974
			public Vector3 Velocity;

			// Token: 0x04004637 RID: 17975
			public readonly VelocityConfig Config;

			// Token: 0x04004638 RID: 17976
			public bool CanClear;
		}

		// Token: 0x02000E60 RID: 3680
		// (Invoke) Token: 0x06006790 RID: 26512
		protected delegate bool StatePredicate(ref MovementController.StateFrame frame);

		// Token: 0x02000E61 RID: 3681
		protected struct StateFrame
		{
			// Token: 0x06006793 RID: 26515 RVA: 0x0021822C File Offset: 0x0021642C
			public StateFrame(bool valid)
			{
				this.Valid = valid;
				this.Input = new MovementController.InputFrame(valid);
				this.Position = default(Vector3);
				this.Velocity = default(Vector3);
				this.MovementForceRotation = default(Vector3);
				this.MovementStates = default(ClientMovementStates);
			}

			// Token: 0x04004639 RID: 17977
			public bool Valid;

			// Token: 0x0400463A RID: 17978
			public MovementController.InputFrame Input;

			// Token: 0x0400463B RID: 17979
			public Vector3 Position;

			// Token: 0x0400463C RID: 17980
			public Vector3 Velocity;

			// Token: 0x0400463D RID: 17981
			public Vector3 MovementForceRotation;

			// Token: 0x0400463E RID: 17982
			public ClientMovementStates MovementStates;
		}

		// Token: 0x02000E62 RID: 3682
		protected struct InputFrame
		{
			// Token: 0x06006794 RID: 26516 RVA: 0x0021827D File Offset: 0x0021647D
			public InputFrame(bool valid)
			{
				this.BindingDown = new Dictionary<InputBinding, bool>();
			}

			// Token: 0x06006795 RID: 26517 RVA: 0x0021828B File Offset: 0x0021648B
			public void Capture(Input input, InputBinding binding)
			{
				this.BindingDown[binding] = input.IsBindingHeld(binding, false);
			}

			// Token: 0x06006796 RID: 26518 RVA: 0x002182A4 File Offset: 0x002164A4
			public bool IsBindingHeld(InputBinding binding)
			{
				return this.BindingDown[binding];
			}

			// Token: 0x0400463F RID: 17983
			private Dictionary<InputBinding, bool> BindingDown;
		}
	}
}
