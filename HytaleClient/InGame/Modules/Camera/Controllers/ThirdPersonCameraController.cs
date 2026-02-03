using System;
using HytaleClient.InGame.Modules.CharacterController;
using HytaleClient.InGame.Modules.Collision;
using HytaleClient.InGame.Modules.Entities;
using HytaleClient.Math;
using HytaleClient.Protocol;
using SDL2;

namespace HytaleClient.InGame.Modules.Camera.Controllers
{
	// Token: 0x02000976 RID: 2422
	internal class ThirdPersonCameraController : ICameraController
	{
		// Token: 0x17001251 RID: 4689
		// (get) Token: 0x06004C81 RID: 19585 RVA: 0x00145A7F File Offset: 0x00143C7F
		public float SpeedModifier { get; } = 1f;

		// Token: 0x17001252 RID: 4690
		// (get) Token: 0x06004C82 RID: 19586 RVA: 0x00145A87 File Offset: 0x00143C87
		public bool AllowPitchControls
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001253 RID: 4691
		// (get) Token: 0x06004C83 RID: 19587 RVA: 0x00145A8A File Offset: 0x00143C8A
		public bool DisplayCursor
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001254 RID: 4692
		// (get) Token: 0x06004C84 RID: 19588 RVA: 0x00145A8D File Offset: 0x00143C8D
		public virtual bool DisplayReticle
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001255 RID: 4693
		// (get) Token: 0x06004C85 RID: 19589 RVA: 0x00145A90 File Offset: 0x00143C90
		public bool SkipCharacterPhysics
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001256 RID: 4694
		// (get) Token: 0x06004C86 RID: 19590 RVA: 0x00145A93 File Offset: 0x00143C93
		public virtual bool IsFirstPerson
		{
			get
			{
				return this._isFirstPersonOverride;
			}
		}

		// Token: 0x17001257 RID: 4695
		// (get) Token: 0x06004C87 RID: 19591 RVA: 0x00145A9B File Offset: 0x00143C9B
		public virtual bool InteractFromEntity
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001258 RID: 4696
		// (get) Token: 0x06004C88 RID: 19592 RVA: 0x00145A9E File Offset: 0x00143C9E
		public virtual Vector3 MovementForceRotation
		{
			get
			{
				return this._gameInstance.LocalPlayer.GetRelativeMovementStates().IsMounting ? this._gameInstance.CharacterControllerModule.MovementController.CameraRotation : this.Rotation;
			}
		}

		// Token: 0x17001259 RID: 4697
		// (get) Token: 0x06004C89 RID: 19593 RVA: 0x00145AD4 File Offset: 0x00143CD4
		// (set) Token: 0x06004C8A RID: 19594 RVA: 0x00145AEB File Offset: 0x00143CEB
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

		// Token: 0x1700125A RID: 4698
		// (get) Token: 0x06004C8B RID: 19595 RVA: 0x00145AF4 File Offset: 0x00143CF4
		// (set) Token: 0x06004C8C RID: 19596 RVA: 0x00145AFC File Offset: 0x00143CFC
		public Vector3 AttachmentPosition { get; private set; }

		// Token: 0x1700125B RID: 4699
		// (get) Token: 0x06004C8D RID: 19597 RVA: 0x00145B05 File Offset: 0x00143D05
		// (set) Token: 0x06004C8E RID: 19598 RVA: 0x00145B0D File Offset: 0x00143D0D
		public Vector3 PositionOffset { get; set; }

		// Token: 0x1700125C RID: 4700
		// (get) Token: 0x06004C8F RID: 19599 RVA: 0x00145B16 File Offset: 0x00143D16
		public Vector3 RotationOffset
		{
			get
			{
				return Vector3.Zero;
			}
		}

		// Token: 0x1700125D RID: 4701
		// (get) Token: 0x06004C90 RID: 19600 RVA: 0x00145B20 File Offset: 0x00143D20
		public Vector3 Position
		{
			get
			{
				return this.AttachmentPosition + this._rotatedPositionOffset + this._gameInstance.CameraModule.CameraShakeController.Offset + this._gameInstance.CharacterControllerModule.MovementController.MantleCameraOffset;
			}
		}

		// Token: 0x1700125E RID: 4702
		// (get) Token: 0x06004C91 RID: 19601 RVA: 0x00145B72 File Offset: 0x00143D72
		public Vector3 Rotation
		{
			get
			{
				return this._rotation + this._gameInstance.CameraModule.CameraShakeController.Rotation + this._gameInstance.CharacterControllerModule.MovementController.ThirdPersonRotationOffset;
			}
		}

		// Token: 0x1700125F RID: 4703
		// (get) Token: 0x06004C92 RID: 19602 RVA: 0x00145BAE File Offset: 0x00143DAE
		public Vector3 LookAt
		{
			get
			{
				return this._lookAt;
			}
		}

		// Token: 0x17001260 RID: 4704
		// (get) Token: 0x06004C93 RID: 19603 RVA: 0x00145BB6 File Offset: 0x00143DB6
		public bool CanMove
		{
			get
			{
				return this._entity == null || this._entity == this._gameInstance.LocalPlayer;
			}
		}

		// Token: 0x17001261 RID: 4705
		// (get) Token: 0x06004C94 RID: 19604 RVA: 0x00145BD6 File Offset: 0x00143DD6
		public virtual bool ApplyHeadRotation
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06004C95 RID: 19605 RVA: 0x00145BDC File Offset: 0x00143DDC
		public ThirdPersonCameraController(GameInstance gameInstance)
		{
			this._gameInstance = gameInstance;
		}

		// Token: 0x06004C96 RID: 19606 RVA: 0x00145C30 File Offset: 0x00143E30
		public virtual void Reset(GameInstance gameInstance, ICameraController previousCameraController)
		{
			bool flag = this.AttachedTo == null;
			if (flag)
			{
				this.PositionOffset = Vector3.Zero;
			}
			else
			{
				this.PositionOffset = new Vector3(this.AttachedTo.CameraSettings.PositionOffset.X, this.AttachedTo.CameraSettings.PositionOffset.Y, this.AttachedTo.CameraSettings.PositionOffset.Z);
			}
			bool flag2 = previousCameraController == null;
			if (!flag2)
			{
				bool flag3 = previousCameraController is FirstPersonCameraController;
				if (flag3)
				{
					this._horizontalCollisionDistanceOffset = (this._verticalCollisionDistanceOffset = 0f);
					this._rotatedPositionOffset = Vector3.Zero;
					this._transitionLookAt = previousCameraController.LookAt;
					this.LookAtPosition(this._transitionLookAt, 1f);
					Vector3 rotation = this.Rotation;
					Quaternion quaternion = Quaternion.CreateFromYawPitchRoll(rotation.Y, -1.5607964f, 0f);
					Vector3 positionOffset = this.PositionOffset;
					Vector3 vector;
					Vector3.Transform(ref positionOffset, ref quaternion, out vector);
					Vector2 value = new Vector2(this._transitionLookAt.X, this._transitionLookAt.Z);
					Vector2 vector2 = new Vector2(this.AttachedTo.Position.X, this.AttachedTo.Position.Z);
					Vector2 vector3 = new Vector2(vector.X, vector.Z);
					Vector2 value2 = value - vector2;
					float num = vector3.Length() + 0.2f;
					bool flag4 = value2.Length() < num;
					if (flag4)
					{
						value2.Normalize();
						Vector2 vector4 = value2 * num;
						this._transitionLookAt.X = vector2.X + vector4.X;
						this._transitionLookAt.Z = vector2.Y + vector4.Y;
					}
					this._lookAt = this._transitionLookAt;
					this._inTransition = true;
				}
			}
		}

		// Token: 0x06004C97 RID: 19607 RVA: 0x00145E14 File Offset: 0x00144014
		public void Update(float deltaTime)
		{
			Vector3 rotation = this.Rotation;
			rotation.Pitch = MathHelper.Clamp(rotation.Pitch, -1.5607964f, 1.5607964f);
			Quaternion rotation2 = Quaternion.CreateFromYawPitchRoll(rotation.Yaw, rotation.Pitch, rotation.Roll);
			Vector3 direction = Vector3.Transform(Vector3.Forward, rotation2);
			float num = this._inTransition ? 0.2f : 0.1f;
			num = MathHelper.Min(num / 0.016666668f * deltaTime, 1f);
			Vector3 vector = Vector3.Transform(new Vector3(this.PositionOffset.X, 0f, 0f), rotation2);
			float x = this.PositionOffset.X;
			this._rotatedPositionOffset = new Vector3(0f, this.PositionOffset.Y, 0f);
			bool flag = x != 0f;
			if (flag)
			{
				CollisionModule.BlockRaycastOptions @default = CollisionModule.BlockRaycastOptions.Default;
				@default.Block = ThirdPersonCameraController.ThirdPersonCameraRaycastOptions;
				Vector3 vector2 = Vector3.Normalize(vector);
				Ray ray = new Ray(this.Position, vector2);
				this._horizontalCollisionDistanceOffset = MathHelper.Lerp(this._horizontalCollisionDistanceOffset, x, num);
				@default.RaycastOptions.Distance = this._horizontalCollisionDistanceOffset;
				float maxPossibleOffset = this.GetMaxPossibleOffset(ref ray, ref @default, 1f);
				this._horizontalCollisionDistanceOffset = Math.Min(this._horizontalCollisionDistanceOffset, maxPossibleOffset);
				vector = vector2 * this._horizontalCollisionDistanceOffset;
			}
			this._rotatedPositionOffset = new Vector3(0f, this.PositionOffset.Y, 0f) + vector;
			Vector3 vector3 = Vector3.Transform(new Vector3(0f, 0f, this.PositionOffset.Z), rotation2);
			float z = this.PositionOffset.Z;
			bool flag2 = z != 0f;
			if (flag2)
			{
				CollisionModule.BlockRaycastOptions default2 = CollisionModule.BlockRaycastOptions.Default;
				default2.Block = ThirdPersonCameraController.ThirdPersonCameraRaycastOptions;
				Vector3 vector4 = Vector3.Normalize(vector3);
				Ray ray2 = new Ray(this.Position, vector4);
				this._verticalCollisionDistanceOffset = MathHelper.Lerp(this._verticalCollisionDistanceOffset, z, num);
				default2.RaycastOptions.Distance = this._verticalCollisionDistanceOffset;
				float maxPossibleOffset2 = this.GetMaxPossibleOffset(ref ray2, ref default2, 1f);
				this._verticalCollisionDistanceOffset = Math.Min(this._verticalCollisionDistanceOffset, maxPossibleOffset2);
				vector3 = vector4 * this._verticalCollisionDistanceOffset;
			}
			this._rotatedPositionOffset = new Vector3(0f, this.PositionOffset.Y, 0f) + vector + vector3;
			bool inTransition = this._inTransition;
			if (inTransition)
			{
				this.LookAtPosition(this._transitionLookAt, 1f);
			}
			this._isFirstPersonOverride = (this._rotatedPositionOffset.Length() < 0.5f);
			bool isFirstPersonOverride = this._isFirstPersonOverride;
			if (isFirstPersonOverride)
			{
				this._rotatedPositionOffset = Vector3.Zero;
			}
			bool applyHeadRotation = this.ApplyHeadRotation;
			if (applyHeadRotation)
			{
				Ray ray3 = new Ray(this.Position, direction);
				CollisionModule.CombinedOptions default3 = CollisionModule.CombinedOptions.Default;
				default3.Block = ThirdPersonCameraController.ThirdPersonCameraRaycastOptions;
				CollisionModule.BlockResult? blockResult;
				CollisionModule.EntityResult? entityResult;
				bool flag3 = this._gameInstance.CollisionModule.FindNearestTarget(ref ray3, ref default3, out blockResult, out entityResult);
				if (flag3)
				{
					bool flag4 = blockResult != null;
					Raycast.Result result;
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
					this._lookAt = result.GetTarget();
					this._headLookAt = ((result.NearT < 5f) ? ray3.GetAt(5f) : this._lookAt);
				}
				else
				{
					this._headLookAt = (this._lookAt = ray3.GetAt(default3.RaycastOptions.Distance));
				}
				this._gameInstance.LocalPlayer.LookAt(this._headLookAt, this._gameInstance.CharacterControllerModule.MovementController.MovementStates.IsIdle ? 0.2f : 0.5f);
			}
			this.UpdateAttachmentPosition(deltaTime);
		}

		// Token: 0x06004C98 RID: 19608 RVA: 0x00146224 File Offset: 0x00144424
		private void UpdateAttachmentPosition(float deltaTime)
		{
			bool flag = this._entity != null;
			if (flag)
			{
				this.AttachmentPosition = this._entity.RenderPosition + this.GetEyeOffsetVector();
			}
			else
			{
				MovementController movementController = this._gameInstance.CharacterControllerModule.MovementController;
				Vector3 value = new Vector3(movementController.ThirdPersonPositionOffset.X, movementController.ThirdPersonPositionOffset.Y, movementController.ThirdPersonPositionOffset.Z);
				Quaternion angle = Quaternion.CreateFromAxisAngle(Vector3.Up, this._rotation.Y);
				Vector3.Transform(ref value, ref angle, out value);
				this.AttachmentPosition = this.AttachedTo.RenderPosition + this.GetEyeOffsetVector() + value;
				this._gameInstance.CameraModule.CameraShakeController.Update(deltaTime, angle);
			}
		}

		// Token: 0x06004C99 RID: 19609 RVA: 0x001462F8 File Offset: 0x001444F8
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

		// Token: 0x06004C9A RID: 19610 RVA: 0x00146364 File Offset: 0x00144564
		public virtual void ApplyLook(float deltaTime, Vector2 lookOffset)
		{
			this._rotation.Pitch = MathHelper.Clamp(this._rotation.Pitch + lookOffset.X, -1.5607964f, 1.5607964f);
			this._rotation.Yaw = MathHelper.WrapAngle(this._rotation.Yaw + lookOffset.Y);
			bool flag = this._inTransition && (lookOffset.X > 0.001f || lookOffset.X < -0.001f || lookOffset.Y > 0.001f || lookOffset.Y < -0.001f);
			if (flag)
			{
				this._inTransition = false;
			}
		}

		// Token: 0x06004C9B RID: 19611 RVA: 0x00146411 File Offset: 0x00144611
		public void SetRotation(Vector3 rotation)
		{
			this._rotation = rotation;
			this._inTransition = false;
		}

		// Token: 0x06004C9C RID: 19612 RVA: 0x00146422 File Offset: 0x00144622
		public void OnMouseInput(SDL.SDL_Event evt)
		{
		}

		// Token: 0x06004C9D RID: 19613 RVA: 0x00146428 File Offset: 0x00144628
		protected float GetEyeOffset()
		{
			Entity attachedTo = this.AttachedTo;
			bool flag = attachedTo == null;
			float result;
			if (flag)
			{
				result = 0f;
			}
			else
			{
				bool flag2 = attachedTo != this._gameInstance.LocalPlayer;
				if (flag2)
				{
					result = attachedTo.EyeOffset;
				}
				else
				{
					CharacterControllerModule characterControllerModule = this._gameInstance.CharacterControllerModule;
					result = attachedTo.EyeOffset + characterControllerModule.MovementController.CrouchHeightShift;
				}
			}
			return result;
		}

		// Token: 0x06004C9E RID: 19614 RVA: 0x00146490 File Offset: 0x00144690
		private Vector3 GetEyeOffsetVector()
		{
			return new Vector3(0f, this.GetEyeOffset(), 0f);
		}

		// Token: 0x06004C9F RID: 19615 RVA: 0x001464B8 File Offset: 0x001446B8
		public Vector3 GetHitboxSize()
		{
			Entity attachedTo = this.AttachedTo;
			bool flag = attachedTo == null;
			Vector3 result;
			if (flag)
			{
				result = Vector3.Zero;
			}
			else
			{
				result = attachedTo.Hitbox.GetSize();
			}
			return result;
		}

		// Token: 0x06004CA0 RID: 19616 RVA: 0x001464F0 File Offset: 0x001446F0
		private void LookAtPosition(Vector3 relativePosition, float interpolation = 1f)
		{
			relativePosition -= this.Position;
			bool flag = !MathHelper.WithinEpsilon(relativePosition.X, 0f) || !MathHelper.WithinEpsilon(relativePosition.Z, 0f);
			if (flag)
			{
				float num = (float)Math.Atan2((double)(-(double)relativePosition.X), (double)(-(double)relativePosition.Z));
				num = MathHelper.WrapAngle(num);
				this._rotation.Yaw = MathHelper.WrapAngle(MathHelper.LerpAngle(this._rotation.Yaw, num, interpolation));
			}
			float num2 = relativePosition.Length();
			bool flag2 = num2 > 0f;
			if (flag2)
			{
				float num3 = 1.5707964f - (float)Math.Acos((double)(relativePosition.Y / num2));
				num3 = MathHelper.Clamp(num3, -1.5607964f, 1.5607964f);
				this._rotation.Pitch = MathHelper.Clamp(MathHelper.LerpAngle(this._rotation.Pitch, num3, interpolation), -1.5607964f, 1.5607964f);
			}
		}

		// Token: 0x06004CA1 RID: 19617 RVA: 0x001465EC File Offset: 0x001447EC
		private float GetMaxPossibleOffset(ref Ray ray, ref CollisionModule.BlockRaycastOptions options, float horizontalScale = 1f)
		{
			float num = options.RaycastOptions.Distance;
			CollisionModule.BlockResult @default = CollisionModule.BlockResult.Default;
			bool flag = this._gameInstance.CollisionModule.FindTargetBlock(ref ray, ref options, ref @default);
			if (flag)
			{
				Vector3 vector = @default.Result.GetTarget();
				CollisionModule.CollisionHitData collisionHitData;
				bool flag2 = this.CheckCollision(vector, ray.Direction, 1, horizontalScale, out collisionHitData);
				num = @default.Result.NearT;
				bool flag3 = flag2;
				if (flag3)
				{
					while (flag2 && num > 0f)
					{
						num -= 0.1f;
						vector = ray.GetAt(num);
						flag2 = this.CheckCollision(vector, ray.Direction, 1, horizontalScale, out collisionHitData);
					}
					bool flag4 = num <= 0f;
					if (flag4)
					{
						vector = ray.Position;
						num = 0f;
					}
					float num2;
					bool flag5 = this.VolumeCast(vector, ray.Direction, options.RaycastOptions.Distance, horizontalScale, out num2);
					if (flag5)
					{
						num += num2;
					}
				}
			}
			else
			{
				Vector3 vector2 = ray.GetAt(num);
				CollisionModule.CollisionHitData collisionHitData;
				bool flag6 = this.CheckCollision(vector2, ray.Direction, 1, horizontalScale, out collisionHitData);
				bool flag7 = flag6;
				if (flag7)
				{
					while (flag6 && num > 0f)
					{
						num -= 0.1f;
						vector2 = ray.GetAt(num);
						flag6 = this.CheckCollision(vector2, ray.Direction, 1, horizontalScale, out collisionHitData);
					}
					bool flag8 = num <= 0f;
					if (flag8)
					{
						vector2 = ray.Position;
						num = 0f;
					}
					float num3;
					bool flag9 = this.VolumeCast(vector2, ray.Direction, options.RaycastOptions.Distance, horizontalScale, out num3);
					if (flag9)
					{
						num += num3;
					}
				}
			}
			return num;
		}

		// Token: 0x06004CA2 RID: 19618 RVA: 0x001467B8 File Offset: 0x001449B8
		private bool VolumeCast(Vector3 origin, Vector3 direction, float distance, float horizontalScale, out float outDistance)
		{
			Vector3 position = origin;
			float num = 0.15f * horizontalScale;
			int num2 = 0;
			float num3 = 0f;
			outDistance = distance;
			while (num3 < distance && num2 < 5000)
			{
				bool flag = false;
				num2++;
				num3 = MathHelper.Min(num3 + 0.1f, distance);
				Vector3 vector = origin + direction * num3;
				position.Y = vector.Y;
				CollisionModule.CollisionHitData collisionHitData;
				bool flag2 = this.CheckCollision(position, direction, 1, horizontalScale, out collisionHitData);
				if (flag2)
				{
					Vector3 zero = Vector3.Zero;
					bool flag3 = direction.Y < 0f;
					if (flag3)
					{
						zero.Y = collisionHitData.Limit.Y + 0.15f + 0.0001f;
					}
					else
					{
						zero.Y = collisionHitData.Limit.Y - 0.15f - 0.0001f;
					}
					float value;
					bool flag4 = CollisionModule.CheckRayPlaneDistance(zero, new Vector3(0f, 1f, 0f), origin, direction, out value);
					if (flag4)
					{
						outDistance = MathHelper.Min(outDistance, value);
					}
					position.Y = zero.Y;
					flag = true;
				}
				position.Z = vector.Z;
				bool flag5 = this.CheckCollision(position, direction, 2, horizontalScale, out collisionHitData);
				if (flag5)
				{
					Vector3 zero2 = Vector3.Zero;
					bool flag6 = direction.Z < 0f;
					if (flag6)
					{
						zero2.Z = collisionHitData.Limit.Z + num + 0.0001f;
					}
					else
					{
						zero2.Z = collisionHitData.Limit.Z - num - 0.0001f;
					}
					float value2;
					bool flag7 = CollisionModule.CheckRayPlaneDistance(zero2, new Vector3(0f, 0f, 1f), origin, direction, out value2);
					if (flag7)
					{
						outDistance = MathHelper.Min(outDistance, value2);
					}
					position.Z = zero2.Z;
					flag = true;
				}
				position.X = vector.X;
				bool flag8 = this.CheckCollision(position, direction, 0, horizontalScale, out collisionHitData);
				if (flag8)
				{
					Vector3 zero3 = Vector3.Zero;
					bool flag9 = direction.X < 0f;
					if (flag9)
					{
						zero3.X = collisionHitData.Limit.X + num + 0.0001f;
					}
					else
					{
						zero3.X = collisionHitData.Limit.X - num - 0.0001f;
					}
					float value3;
					bool flag10 = CollisionModule.CheckRayPlaneDistance(zero3, new Vector3(1f, 0f, 0f), origin, direction, out value3);
					if (flag10)
					{
						outDistance = MathHelper.Min(outDistance, value3);
					}
					position.X = zero3.X;
					flag = true;
				}
				bool flag11 = flag;
				if (flag11)
				{
					outDistance = MathHelper.Max(0f, outDistance);
					return true;
				}
			}
			return false;
		}

		// Token: 0x06004CA3 RID: 19619 RVA: 0x00146A84 File Offset: 0x00144C84
		private bool CheckCollision(Vector3 position, Vector3 moveOffset, Axis axis, float horizontalScale, out CollisionModule.CollisionHitData hitData)
		{
			BoundingBox hitbox = this._hitbox;
			bool flag = horizontalScale != 1f;
			if (flag)
			{
				hitbox.Min.X = hitbox.Min.X * horizontalScale;
				hitbox.Min.Z = hitbox.Min.Z * horizontalScale;
				hitbox.Max.X = hitbox.Max.X * horizontalScale;
				hitbox.Max.Z = hitbox.Max.Z * horizontalScale;
			}
			hitbox.Translate(position);
			float num = 0.15f * horizontalScale;
			double num2 = Math.Abs((double)position.X - Math.Truncate((double)position.X));
			int num3 = 0;
			int num4 = (int)Math.Floor((double)position.X);
			bool flag2 = num2 < (double)num || num2 > (double)(1f - num);
			if (flag2)
			{
				num3 = 1;
				num4 = (int)Math.Round((double)position.X) - 1;
			}
			double num5 = Math.Abs((double)position.Y - Math.Truncate((double)position.Y));
			int num6 = 0;
			int num7 = (int)Math.Floor((double)position.Y);
			bool flag3 = num5 < (double)num || num5 > (double)(1f - num);
			if (flag3)
			{
				num6 = 1;
				num7 = (int)Math.Round((double)position.Y) - 1;
			}
			double num8 = Math.Abs((double)position.Z - Math.Truncate((double)position.Z));
			int num9 = 0;
			int num10 = (int)Math.Floor((double)position.Z);
			bool flag4 = num8 < (double)num || num8 > (double)(1f - num);
			if (flag4)
			{
				num9 = 1;
				num10 = (int)Math.Round((double)position.Z) - 1;
			}
			hitData = default(CollisionModule.CollisionHitData);
			float num11 = 0f;
			for (int i = 0; i <= num6; i++)
			{
				int y = num7 + i;
				for (int j = 0; j <= num9; j++)
				{
					int z = num10 + j;
					for (int k = 0; k <= num3; k++)
					{
						int x = num4 + k;
						CollisionModule.CollisionHitData collisionHitData;
						bool flag5 = this._gameInstance.CollisionModule.CheckBlockCollision(new IntVector3(x, y, z), hitbox, moveOffset, out collisionHitData);
						if (flag5)
						{
							float num12 = 0f;
							switch (axis)
							{
							case 0:
								num12 = collisionHitData.Overlap.X;
								break;
							case 1:
								num12 = collisionHitData.Overlap.Y;
								break;
							case 2:
								num12 = collisionHitData.Overlap.Z;
								break;
							}
							bool flag6 = num11 == 0f || num12 > num11;
							if (flag6)
							{
								hitData = collisionHitData;
								num11 = num12;
							}
						}
					}
				}
			}
			return num11 > 0f;
		}

		// Token: 0x0400280E RID: 10254
		private static readonly CollisionModule.BlockOptions ThirdPersonCameraRaycastOptions = new CollisionModule.BlockOptions
		{
			IgnoreEmptyCollisionMaterial = true,
			IgnoreFluids = true
		};

		// Token: 0x0400280F RID: 10255
		private const float CollisionPadding = 0.0001f;

		// Token: 0x04002810 RID: 10256
		private const float EdgePadding = 0.01f;

		// Token: 0x04002811 RID: 10257
		private const float TransitionCancellationMovePadding = 0.01f;

		// Token: 0x04002812 RID: 10258
		private const float TransitionCancellationLookPadding = 0.001f;

		// Token: 0x04002813 RID: 10259
		private const float FirstPersonDistanceMinDistance = 0.5f;

		// Token: 0x04002814 RID: 10260
		private const float HitboxHalfSize = 0.15f;

		// Token: 0x04002815 RID: 10261
		private const float MinLookDistance = 5f;

		// Token: 0x04002817 RID: 10263
		private Entity _entity;

		// Token: 0x0400281A RID: 10266
		protected Vector3 _rotation;

		// Token: 0x0400281B RID: 10267
		protected Vector3 _lookAt;

		// Token: 0x0400281C RID: 10268
		protected Vector3 _headLookAt;

		// Token: 0x0400281D RID: 10269
		protected Vector3 _transitionLookAt;

		// Token: 0x0400281E RID: 10270
		private readonly GameInstance _gameInstance;

		// Token: 0x0400281F RID: 10271
		internal Vector3 _rotatedPositionOffset;

		// Token: 0x04002820 RID: 10272
		internal float _horizontalCollisionDistanceOffset;

		// Token: 0x04002821 RID: 10273
		internal float _verticalCollisionDistanceOffset;

		// Token: 0x04002822 RID: 10274
		private bool _isFirstPersonOverride = false;

		// Token: 0x04002823 RID: 10275
		internal bool _inTransition = false;

		// Token: 0x04002824 RID: 10276
		private readonly BoundingBox _hitbox = new BoundingBox(new Vector3(-0.15f), new Vector3(0.15f));
	}
}
