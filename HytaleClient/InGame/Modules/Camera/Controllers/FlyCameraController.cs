using System;
using HytaleClient.Core;
using HytaleClient.Data.UserSettings;
using HytaleClient.InGame.Modules.Entities;
using HytaleClient.Math;
using SDL2;

namespace HytaleClient.InGame.Modules.Camera.Controllers
{
	// Token: 0x02000972 RID: 2418
	internal class FlyCameraController : ICameraController
	{
		// Token: 0x1700121A RID: 4634
		// (get) Token: 0x06004C1E RID: 19486 RVA: 0x00143EE2 File Offset: 0x001420E2
		public float SpeedModifier
		{
			get
			{
				return this.CanMove ? 1f : 3f;
			}
		}

		// Token: 0x1700121B RID: 4635
		// (get) Token: 0x06004C1F RID: 19487 RVA: 0x00143EF8 File Offset: 0x001420F8
		public bool AllowPitchControls
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700121C RID: 4636
		// (get) Token: 0x06004C20 RID: 19488 RVA: 0x00143EFB File Offset: 0x001420FB
		public bool DisplayCursor
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700121D RID: 4637
		// (get) Token: 0x06004C21 RID: 19489 RVA: 0x00143EFE File Offset: 0x001420FE
		public bool DisplayReticle
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700121E RID: 4638
		// (get) Token: 0x06004C22 RID: 19490 RVA: 0x00143F01 File Offset: 0x00142101
		public bool SkipCharacterPhysics
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700121F RID: 4639
		// (get) Token: 0x06004C23 RID: 19491 RVA: 0x00143F04 File Offset: 0x00142104
		public bool IsFirstPerson
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001220 RID: 4640
		// (get) Token: 0x06004C24 RID: 19492 RVA: 0x00143F07 File Offset: 0x00142107
		public bool InteractFromEntity
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001221 RID: 4641
		// (get) Token: 0x06004C25 RID: 19493 RVA: 0x00143F0A File Offset: 0x0014210A
		public Vector3 MovementForceRotation
		{
			get
			{
				return this.CanMove ? new Vector3(0f, this._gameInstance.LocalPlayer.LookOrientation.Yaw, 0f) : this.Rotation;
			}
		}

		// Token: 0x17001222 RID: 4642
		// (get) Token: 0x06004C26 RID: 19494 RVA: 0x00143F40 File Offset: 0x00142140
		// (set) Token: 0x06004C27 RID: 19495 RVA: 0x00143F57 File Offset: 0x00142157
		public Entity AttachedTo
		{
			get
			{
				return this._entity ?? this._gameInstance.LocalPlayer;
			}
			set
			{
				this._entity = value;
			}
		}

		// Token: 0x17001223 RID: 4643
		// (get) Token: 0x06004C28 RID: 19496 RVA: 0x00143F60 File Offset: 0x00142160
		public Vector3 AttachmentPosition
		{
			get
			{
				return this.AttachedTo.RenderPosition + new Vector3(0f, this.AttachedTo.EyeOffset, 0f);
			}
		}

		// Token: 0x17001224 RID: 4644
		// (get) Token: 0x06004C29 RID: 19497 RVA: 0x00143F8C File Offset: 0x0014218C
		public Vector3 PositionOffset
		{
			get
			{
				return Vector3.Zero;
			}
		}

		// Token: 0x17001225 RID: 4645
		// (get) Token: 0x06004C2A RID: 19498 RVA: 0x00143F93 File Offset: 0x00142193
		public Vector3 RotationOffset
		{
			get
			{
				return Vector3.Zero;
			}
		}

		// Token: 0x17001226 RID: 4646
		// (get) Token: 0x06004C2B RID: 19499 RVA: 0x00143F9A File Offset: 0x0014219A
		// (set) Token: 0x06004C2C RID: 19500 RVA: 0x00143FA2 File Offset: 0x001421A2
		public Vector3 Position { get; private set; }

		// Token: 0x17001227 RID: 4647
		// (get) Token: 0x06004C2D RID: 19501 RVA: 0x00143FAB File Offset: 0x001421AB
		// (set) Token: 0x06004C2E RID: 19502 RVA: 0x00143FB3 File Offset: 0x001421B3
		public Vector3 Rotation { get; private set; }

		// Token: 0x17001228 RID: 4648
		// (get) Token: 0x06004C2F RID: 19503 RVA: 0x00143FBC File Offset: 0x001421BC
		// (set) Token: 0x06004C30 RID: 19504 RVA: 0x00143FC4 File Offset: 0x001421C4
		public Vector3 LookAt { get; private set; }

		// Token: 0x17001229 RID: 4649
		// (get) Token: 0x06004C31 RID: 19505 RVA: 0x00143FCD File Offset: 0x001421CD
		// (set) Token: 0x06004C32 RID: 19506 RVA: 0x00143FD5 File Offset: 0x001421D5
		public bool CanMove { get; private set; }

		// Token: 0x06004C33 RID: 19507 RVA: 0x00143FDE File Offset: 0x001421DE
		public FlyCameraController(GameInstance gameInstance)
		{
			this._gameInstance = gameInstance;
		}

		// Token: 0x06004C34 RID: 19508 RVA: 0x00143FF8 File Offset: 0x001421F8
		public void Reset(GameInstance gameInstance, ICameraController previousCameraController)
		{
			bool isFirstUsage = this._isFirstUsage;
			if (isFirstUsage)
			{
				this.Position = previousCameraController.Position;
				this.Rotation = previousCameraController.Rotation;
				this._isFirstUsage = false;
			}
			this._previousPosition = this.Position;
			this._nextPosition = this.Position;
			this._move = Vector3.Zero;
		}

		// Token: 0x06004C35 RID: 19509 RVA: 0x00144058 File Offset: 0x00142258
		public void ResetPosition()
		{
			this.Position = this.AttachmentPosition;
			this.Rotation = this.AttachedTo.LookOrientation;
			this._previousPosition = this.Position;
			this._nextPosition = this.Position;
			this._move = Vector3.Zero;
		}

		// Token: 0x06004C36 RID: 19510 RVA: 0x001440A8 File Offset: 0x001422A8
		public void Update(float deltaTime)
		{
			bool canMove = this.CanMove;
			if (!canMove)
			{
				Quaternion rotation = Quaternion.CreateFromYawPitchRoll(this.Rotation.Yaw, this.Rotation.Pitch, 0f);
				Vector3 value = Vector3.Transform(Vector3.Forward, rotation);
				Vector3 value2 = Vector3.Transform(Vector3.Right, rotation);
				Input input = this._gameInstance.Input;
				InputBindings inputBindings = this._gameInstance.App.Settings.InputBindings;
				float scaleFactor = this._gameInstance.CharacterControllerModule.MovementController.SpeedMultiplier * (input.IsBindingHeld(inputBindings.Sprint, false) ? 1.25f : 0.125f);
				this._tickAccumulator += deltaTime;
				bool flag = this._tickAccumulator > 0.083333336f;
				if (flag)
				{
					this._tickAccumulator = 0.083333336f;
				}
				while (this._tickAccumulator >= 0.016666668f)
				{
					bool flag2 = input.IsBindingHeld(inputBindings.MoveForwards, false);
					if (flag2)
					{
						this._move += value * scaleFactor;
					}
					bool flag3 = input.IsBindingHeld(inputBindings.MoveBackwards, false);
					if (flag3)
					{
						this._move -= value * scaleFactor;
					}
					bool flag4 = input.IsBindingHeld(inputBindings.StrafeRight, false);
					if (flag4)
					{
						this._move += value2 * scaleFactor;
					}
					bool flag5 = input.IsBindingHeld(inputBindings.StrafeLeft, false);
					if (flag5)
					{
						this._move -= value2 * scaleFactor;
					}
					bool flag6 = input.IsBindingHeld(inputBindings.Crouch, false);
					if (flag6)
					{
						this._move -= Vector3.Up * scaleFactor;
					}
					bool flag7 = input.IsBindingHeld(inputBindings.Jump, false);
					if (flag7)
					{
						this._move += Vector3.Up * scaleFactor;
					}
					this._move *= 0.7f;
					this._previousPosition = this._nextPosition;
					this._nextPosition += this._move;
					this._tickAccumulator -= 0.016666668f;
				}
				float amount = Math.Min(this._tickAccumulator / 0.016666668f, 1f);
				this.Position = Vector3.Lerp(this._previousPosition, this._nextPosition, amount);
			}
		}

		// Token: 0x06004C37 RID: 19511 RVA: 0x0014433E File Offset: 0x0014253E
		public void ApplyMove(Vector3 movementOffset)
		{
			this._gameInstance.CharacterControllerModule.MovementController.ApplyMovementOffset(movementOffset);
		}

		// Token: 0x06004C38 RID: 19512 RVA: 0x00144358 File Offset: 0x00142558
		public void ApplyLook(float deltaTime, Vector2 lookOffset)
		{
			bool canMove = this.CanMove;
			if (canMove)
			{
				this.ApplyLookPlayer(lookOffset);
			}
			else
			{
				Vector3 rotation = this.Rotation;
				this.Rotation = new Vector3(MathHelper.Clamp(rotation.X + lookOffset.X, -1.5707964f, 1.5707964f), MathHelper.WrapAngle(rotation.Y + lookOffset.Y), rotation.Roll);
			}
		}

		// Token: 0x06004C39 RID: 19513 RVA: 0x001443C4 File Offset: 0x001425C4
		private void ApplyLookPlayer(Vector2 lookOffset)
		{
			PlayerEntity localPlayer = this._gameInstance.LocalPlayer;
			Vector3 lookOrientation = localPlayer.LookOrientation;
			lookOrientation = new Vector3(MathHelper.Clamp(lookOrientation.X + lookOffset.X, -1.5707964f, 1.5707964f), MathHelper.WrapAngle(lookOrientation.Y + lookOffset.Y), lookOrientation.Roll);
			localPlayer.LookOrientation.Yaw = lookOrientation.Yaw;
			localPlayer.LookOrientation.Pitch = lookOrientation.Pitch;
			localPlayer.UpdateModelLookOrientation();
		}

		// Token: 0x06004C3A RID: 19514 RVA: 0x0014444E File Offset: 0x0014264E
		public void SetRotation(Vector3 rotation)
		{
			this.Rotation = rotation;
		}

		// Token: 0x06004C3B RID: 19515 RVA: 0x00144459 File Offset: 0x00142659
		public void OnMouseInput(SDL.SDL_Event evt)
		{
		}

		// Token: 0x06004C3C RID: 19516 RVA: 0x0014445C File Offset: 0x0014265C
		public void ToggleControlTarget()
		{
			this.CanMove = !this.CanMove;
			this._gameInstance.Chat.Log("Now controlling the " + (this.CanMove ? "player" : "camera"));
		}

		// Token: 0x040027F0 RID: 10224
		private const float WalkSpeed = 0.125f;

		// Token: 0x040027F1 RID: 10225
		private const float RunSpeed = 1.25f;

		// Token: 0x040027F2 RID: 10226
		private const float MovementSmoothingFactor = 0.7f;

		// Token: 0x040027F3 RID: 10227
		private Entity _entity;

		// Token: 0x040027F8 RID: 10232
		private readonly GameInstance _gameInstance;

		// Token: 0x040027F9 RID: 10233
		private Vector3 _move;

		// Token: 0x040027FA RID: 10234
		private Vector3 _previousPosition;

		// Token: 0x040027FB RID: 10235
		private Vector3 _nextPosition;

		// Token: 0x040027FC RID: 10236
		private bool _isFirstUsage = true;

		// Token: 0x040027FD RID: 10237
		private float _tickAccumulator;
	}
}
