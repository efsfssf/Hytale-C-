using System;
using HytaleClient.InGame.Modules.CharacterController;
using HytaleClient.InGame.Modules.Collision;
using HytaleClient.InGame.Modules.Entities;
using HytaleClient.Math;
using SDL2;

namespace HytaleClient.InGame.Modules.Camera.Controllers
{
	// Token: 0x02000971 RID: 2417
	internal class FirstPersonCameraController : ICameraController
	{
		// Token: 0x1700120A RID: 4618
		// (get) Token: 0x06004C04 RID: 19460 RVA: 0x001438E8 File Offset: 0x00141AE8
		public float SpeedModifier { get; } = 1f;

		// Token: 0x1700120B RID: 4619
		// (get) Token: 0x06004C05 RID: 19461 RVA: 0x001438F0 File Offset: 0x00141AF0
		public bool AllowPitchControls
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700120C RID: 4620
		// (get) Token: 0x06004C06 RID: 19462 RVA: 0x001438F3 File Offset: 0x00141AF3
		public bool DisplayCursor
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700120D RID: 4621
		// (get) Token: 0x06004C07 RID: 19463 RVA: 0x001438F6 File Offset: 0x00141AF6
		public bool DisplayReticle
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700120E RID: 4622
		// (get) Token: 0x06004C08 RID: 19464 RVA: 0x001438F9 File Offset: 0x00141AF9
		public bool SkipCharacterPhysics
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700120F RID: 4623
		// (get) Token: 0x06004C09 RID: 19465 RVA: 0x001438FC File Offset: 0x00141AFC
		public bool IsFirstPerson
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001210 RID: 4624
		// (get) Token: 0x06004C0A RID: 19466 RVA: 0x001438FF File Offset: 0x00141AFF
		public bool InteractFromEntity
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001211 RID: 4625
		// (get) Token: 0x06004C0B RID: 19467 RVA: 0x00143902 File Offset: 0x00141B02
		public virtual Vector3 MovementForceRotation
		{
			get
			{
				return this._gameInstance.LocalPlayer.GetRelativeMovementStates().IsMounting ? this._gameInstance.CharacterControllerModule.MovementController.CameraRotation : this.AttachedTo.LookOrientation;
			}
		}

		// Token: 0x17001212 RID: 4626
		// (get) Token: 0x06004C0C RID: 19468 RVA: 0x0014393D File Offset: 0x00141B3D
		// (set) Token: 0x06004C0D RID: 19469 RVA: 0x00143954 File Offset: 0x00141B54
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

		// Token: 0x17001213 RID: 4627
		// (get) Token: 0x06004C0E RID: 19470 RVA: 0x0014395D File Offset: 0x00141B5D
		// (set) Token: 0x06004C0F RID: 19471 RVA: 0x00143965 File Offset: 0x00141B65
		public Vector3 AttachmentPosition { get; private set; }

		// Token: 0x17001214 RID: 4628
		// (get) Token: 0x06004C10 RID: 19472 RVA: 0x0014396E File Offset: 0x00141B6E
		public Vector3 PositionOffset
		{
			get
			{
				return Vector3.Zero;
			}
		}

		// Token: 0x17001215 RID: 4629
		// (get) Token: 0x06004C11 RID: 19473 RVA: 0x00143975 File Offset: 0x00141B75
		public Vector3 RotationOffset
		{
			get
			{
				return Vector3.Zero;
			}
		}

		// Token: 0x17001216 RID: 4630
		// (get) Token: 0x06004C12 RID: 19474 RVA: 0x0014397C File Offset: 0x00141B7C
		public Vector3 Position
		{
			get
			{
				return this.AttachmentPosition + this._gameInstance.CameraModule.CameraShakeController.Offset + this._gameInstance.CharacterControllerModule.MovementController.MantleCameraOffset;
			}
		}

		// Token: 0x17001217 RID: 4631
		// (get) Token: 0x06004C13 RID: 19475 RVA: 0x001439B8 File Offset: 0x00141BB8
		public Vector3 Rotation
		{
			get
			{
				return this.AttachedTo.LookOrientation + this._gameInstance.CharacterControllerModule.MovementController.FirstPersonRotationOffset + this._gameInstance.CameraModule.CameraShakeController.Rotation;
			}
		}

		// Token: 0x17001218 RID: 4632
		// (get) Token: 0x06004C14 RID: 19476 RVA: 0x00143A04 File Offset: 0x00141C04
		public Vector3 LookAt
		{
			get
			{
				return this._lookAt;
			}
		}

		// Token: 0x17001219 RID: 4633
		// (get) Token: 0x06004C15 RID: 19477 RVA: 0x00143A0C File Offset: 0x00141C0C
		public bool CanMove
		{
			get
			{
				return this._entity == null || this._entity == this._gameInstance.LocalPlayer;
			}
		}

		// Token: 0x06004C16 RID: 19478 RVA: 0x00143A2C File Offset: 0x00141C2C
		public FirstPersonCameraController(GameInstance gameInstance)
		{
			this._gameInstance = gameInstance;
		}

		// Token: 0x06004C17 RID: 19479 RVA: 0x00143A50 File Offset: 0x00141C50
		public void Reset(GameInstance gameInstance, ICameraController previousCameraController)
		{
			bool flag = previousCameraController is ThirdPersonCameraController && !(previousCameraController is FreeRotateCameraController);
			if (flag)
			{
				this._lookAt = (this._transitionLookAt = previousCameraController.LookAt);
				this._gameInstance.LocalPlayer.LookAt(this._transitionLookAt, 1f);
				this._inTransition = true;
			}
		}

		// Token: 0x06004C18 RID: 19480 RVA: 0x00143AB4 File Offset: 0x00141CB4
		public void Update(float deltaTime)
		{
			Quaternion rotation = Quaternion.CreateFromYawPitchRoll(this.Rotation.Yaw, this.Rotation.Pitch, 0f);
			Vector3 direction = Vector3.Transform(Vector3.Forward, rotation);
			bool inTransition = this._inTransition;
			if (inTransition)
			{
				this._gameInstance.LocalPlayer.LookAt(this._transitionLookAt, 1f);
			}
			Ray ray = new Ray(this.Position, direction);
			CollisionModule.CombinedOptions @default = CollisionModule.CombinedOptions.Default;
			@default.Block.IgnoreEmptyCollisionMaterial = false;
			@default.Block.IgnoreFluids = true;
			CollisionModule.BlockResult? blockResult;
			CollisionModule.EntityResult? entityResult;
			Raycast.Result result;
			bool flag = this._gameInstance.CollisionModule.FindNearestTarget(ref ray, ref @default, out blockResult, out entityResult, out result);
			if (flag)
			{
				this._lookAt = result.GetTarget();
			}
			else
			{
				this._lookAt = ray.GetAt(@default.RaycastOptions.Distance);
			}
			this.UpdateAttachmentPosition(deltaTime);
		}

		// Token: 0x06004C19 RID: 19481 RVA: 0x00143BA0 File Offset: 0x00141DA0
		private void UpdateAttachmentPosition(float deltaTime)
		{
			bool flag = this._entity != null;
			if (flag)
			{
				this.AttachmentPosition = this._entity.RenderPosition + new Vector3(0f, this.AttachedTo.EyeOffset, 0f);
			}
			else
			{
				MovementController movementController = this._gameInstance.CharacterControllerModule.MovementController;
				Vector3 value = new Vector3(movementController.FirstPersonPositionOffset.X, movementController.FirstPersonPositionOffset.Y, movementController.FirstPersonPositionOffset.Z);
				Quaternion angle = Quaternion.CreateFromAxisAngle(Vector3.Up, this.AttachedTo.LookOrientation.Y);
				Vector3.Transform(ref value, ref angle, out value);
				this.AttachmentPosition = this.AttachedTo.RenderPosition + new Vector3(0f, this.AttachedTo.EyeOffset + movementController.CrouchHeightShift, 0f) + value;
				this._gameInstance.CameraModule.CameraShakeController.Update(deltaTime, angle);
			}
		}

		// Token: 0x06004C1A RID: 19482 RVA: 0x00143CA8 File Offset: 0x00141EA8
		public void ApplyMove(Vector3 movementOffset)
		{
			Vector3 position = this._gameInstance.LocalPlayer.Position;
			this._gameInstance.CharacterControllerModule.MovementController.ApplyMovementOffset(movementOffset);
			bool flag = this._inTransition && Vector3.Distance(position, this._gameInstance.LocalPlayer.Position) > 0.01f;
			if (flag)
			{
				this._inTransition = false;
			}
		}

		// Token: 0x06004C1B RID: 19483 RVA: 0x00143D14 File Offset: 0x00141F14
		public void ApplyLook(float deltaTime, Vector2 lookOffset)
		{
			Vector3 lookOrientation = this._gameInstance.LocalPlayer.LookOrientation;
			ref Vector3 ptr = ref this._gameInstance.LocalPlayer.LookOrientation;
			ptr.Pitch = MathHelper.Clamp(ptr.Pitch + lookOffset.X, -1.5607964f, 1.5607964f);
			ptr.Yaw = MathHelper.WrapAngle(ptr.Yaw + lookOffset.Y);
			this._itemWiggleTickAccumulator += deltaTime;
			this._itemWiggleAmountAccumulator.X = this._itemWiggleAmountAccumulator.X + (lookOrientation.Pitch - ptr.Pitch) * 4f;
			this._itemWiggleAmountAccumulator.Y = this._itemWiggleAmountAccumulator.Y + lookOffset.Y * 4f;
			bool flag = this._itemWiggleTickAccumulator > 0.083333336f;
			if (flag)
			{
				this._itemWiggleTickAccumulator = 0.083333336f;
			}
			while (this._itemWiggleTickAccumulator >= 0.016666668f)
			{
				this._gameInstance.LocalPlayer.ApplyFirstPersonMouseItemWiggle(this._itemWiggleAmountAccumulator.Y, this._itemWiggleAmountAccumulator.X);
				this._itemWiggleAmountAccumulator.X = (this._itemWiggleAmountAccumulator.Y = 0f);
				this._itemWiggleTickAccumulator -= 0.016666668f;
			}
			float timeFraction = Math.Min(this._itemWiggleTickAccumulator / 0.016666668f, 1f);
			this._gameInstance.LocalPlayer.UpdateClientInterpolationMouseWiggle(timeFraction);
			bool flag2 = this._inTransition && (lookOffset.X > 0.001f || lookOffset.X < -0.001f || lookOffset.Y > 0.001f || lookOffset.Y < -0.001f);
			if (flag2)
			{
				this._inTransition = false;
			}
		}

		// Token: 0x06004C1C RID: 19484 RVA: 0x00143ED5 File Offset: 0x001420D5
		public void SetRotation(Vector3 rotation)
		{
			this._inTransition = false;
		}

		// Token: 0x06004C1D RID: 19485 RVA: 0x00143EDF File Offset: 0x001420DF
		public void OnMouseInput(SDL.SDL_Event evt)
		{
		}

		// Token: 0x040027E4 RID: 10212
		private const float EdgePadding = 0.01f;

		// Token: 0x040027E5 RID: 10213
		private const float TransitionCancellationMovePadding = 0.01f;

		// Token: 0x040027E6 RID: 10214
		private const float TransitionCancellationLookPadding = 0.001f;

		// Token: 0x040027E8 RID: 10216
		private Entity _entity;

		// Token: 0x040027EA RID: 10218
		protected Vector3 _lookAt;

		// Token: 0x040027EB RID: 10219
		protected Vector3 _transitionLookAt;

		// Token: 0x040027EC RID: 10220
		protected bool _inTransition = false;

		// Token: 0x040027ED RID: 10221
		private readonly GameInstance _gameInstance;

		// Token: 0x040027EE RID: 10222
		private float _itemWiggleTickAccumulator;

		// Token: 0x040027EF RID: 10223
		private Vector2 _itemWiggleAmountAccumulator;
	}
}
