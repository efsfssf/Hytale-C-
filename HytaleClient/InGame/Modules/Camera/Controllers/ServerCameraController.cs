using System;
using System.Collections.Generic;
using HytaleClient.InGame.Modules.CharacterController;
using HytaleClient.InGame.Modules.Collision;
using HytaleClient.InGame.Modules.Entities;
using HytaleClient.Math;
using HytaleClient.Protocol;
using HytaleClient.Utils;
using SDL2;

namespace HytaleClient.InGame.Modules.Camera.Controllers
{
	// Token: 0x02000975 RID: 2421
	internal class ServerCameraController : ICameraController
	{
		// Token: 0x1700123F RID: 4671
		// (get) Token: 0x06004C5D RID: 19549 RVA: 0x00144668 File Offset: 0x00142868
		// (set) Token: 0x06004C5E RID: 19550 RVA: 0x00144670 File Offset: 0x00142870
		public ServerCameraSettings CameraSettings
		{
			get
			{
				return this._cameraSettings;
			}
			set
			{
				this._cameraSettings = value;
				this.UpdateCameraSettings();
			}
		}

		// Token: 0x06004C5F RID: 19551 RVA: 0x00144684 File Offset: 0x00142884
		private void UpdateCameraSettings()
		{
			bool flag = this._cameraSettings.MovementForceRotation != null;
			if (flag)
			{
				this._customMovementForceRotation = new Vector3(this._cameraSettings.MovementForceRotation.Pitch, this._cameraSettings.MovementForceRotation.Yaw, this._cameraSettings.MovementForceRotation.Roll);
			}
			else
			{
				this._customMovementForceRotation = Vector3.Zero;
			}
			bool flag2 = this._cameraSettings.PositionOffset != null;
			if (flag2)
			{
				this._customPositionOffset = new Vector3((float)this._cameraSettings.PositionOffset.X, (float)this._cameraSettings.PositionOffset.Y, (float)this._cameraSettings.PositionOffset.Z);
			}
			else
			{
				this._customPositionOffset = Vector3.Zero;
			}
			bool flag3 = this._cameraSettings.RotationOffset != null;
			if (flag3)
			{
				this.RotationOffset = new Vector3(this._cameraSettings.RotationOffset.Pitch, this._cameraSettings.RotationOffset.Yaw, this._cameraSettings.RotationOffset.Roll);
			}
			else
			{
				this.RotationOffset = Vector3.Zero;
			}
			bool flag4 = this._cameraSettings.Position_ != null;
			if (flag4)
			{
				this._customPosition = new Vector3((float)this._cameraSettings.Position_.X, (float)this._cameraSettings.Position_.Y, (float)this._cameraSettings.Position_.Z);
			}
			else
			{
				this._customPosition = Vector3.Zero;
			}
			bool flag5 = this._cameraSettings.Rotation != null;
			if (flag5)
			{
				this._customRotation = new Vector3(this._cameraSettings.Rotation.Pitch, this._cameraSettings.Rotation.Yaw, this._cameraSettings.Rotation.Roll);
			}
			else
			{
				this._customRotation = Vector3.Zero;
			}
		}

		// Token: 0x17001240 RID: 4672
		// (get) Token: 0x06004C60 RID: 19552 RVA: 0x0014485E File Offset: 0x00142A5E
		public float Distance
		{
			get
			{
				return this._cameraSettings.Distance;
			}
		}

		// Token: 0x17001241 RID: 4673
		// (get) Token: 0x06004C61 RID: 19553 RVA: 0x0014486B File Offset: 0x00142A6B
		public float SpeedModifier
		{
			get
			{
				return this._cameraSettings.SpeedModifier;
			}
		}

		// Token: 0x17001242 RID: 4674
		// (get) Token: 0x06004C62 RID: 19554 RVA: 0x00144878 File Offset: 0x00142A78
		public bool AllowPitchControls
		{
			get
			{
				return this._cameraSettings.AllowPitchControls;
			}
		}

		// Token: 0x17001243 RID: 4675
		// (get) Token: 0x06004C63 RID: 19555 RVA: 0x00144885 File Offset: 0x00142A85
		public bool DisplayCursor
		{
			get
			{
				return this._cameraSettings.DisplayCursor;
			}
		}

		// Token: 0x17001244 RID: 4676
		// (get) Token: 0x06004C64 RID: 19556 RVA: 0x00144892 File Offset: 0x00142A92
		public bool DisplayReticle
		{
			get
			{
				return this._cameraSettings.DisplayReticle;
			}
		}

		// Token: 0x17001245 RID: 4677
		// (get) Token: 0x06004C65 RID: 19557 RVA: 0x0014489F File Offset: 0x00142A9F
		public bool SkipCharacterPhysics
		{
			get
			{
				return this._cameraSettings.SkipCharacterPhysics;
			}
		}

		// Token: 0x17001246 RID: 4678
		// (get) Token: 0x06004C66 RID: 19558 RVA: 0x001448AC File Offset: 0x00142AAC
		public bool IsFirstPerson
		{
			get
			{
				return this._cameraSettings.IsFirstPerson;
			}
		}

		// Token: 0x17001247 RID: 4679
		// (get) Token: 0x06004C67 RID: 19559 RVA: 0x001448B9 File Offset: 0x00142AB9
		public bool InteractFromEntity
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001248 RID: 4680
		// (get) Token: 0x06004C68 RID: 19560 RVA: 0x001448BC File Offset: 0x00142ABC
		public Vector3 MovementForceRotation
		{
			get
			{
				Vector3 result;
				switch (this._cameraSettings.MovementForceRotationType_)
				{
				case 0:
					result = this.AttachedTo.LookOrientation;
					break;
				case 1:
					result = this.Rotation;
					break;
				case 2:
					result = this._customMovementForceRotation;
					break;
				default:
					throw new ArgumentOutOfRangeException(string.Format("Unknown MovementForceRotationType {0}", this._cameraSettings.MovementForceRotationType_));
				}
				return result;
			}
		}

		// Token: 0x17001249 RID: 4681
		// (get) Token: 0x06004C69 RID: 19561 RVA: 0x00144930 File Offset: 0x00142B30
		public Entity AttachedTo
		{
			get
			{
				Entity result;
				switch (this._cameraSettings.AttachedToType_)
				{
				case 0:
					result = this._gameInstance.LocalPlayer;
					break;
				case 1:
				{
					bool flag = this._cachedEntity != null && this._cachedEntity.NetworkId == this._cameraSettings.AttachedToEntityId;
					if (flag)
					{
						result = this._cachedEntity;
					}
					else
					{
						result = (this._cachedEntity = this._gameInstance.EntityStoreModule.GetEntity(this._cameraSettings.AttachedToEntityId));
					}
					break;
				}
				case 2:
					result = null;
					break;
				default:
					throw new ArgumentOutOfRangeException(string.Format("Unknown AttachedToType {0}", this._cameraSettings.AttachedToType_));
				}
				return result;
			}
		}

		// Token: 0x1700124A RID: 4682
		// (get) Token: 0x06004C6A RID: 19562 RVA: 0x001449EC File Offset: 0x00142BEC
		public Vector3 AttachmentPosition
		{
			get
			{
				ServerCameraSettings.AttachedToType attachedToType_ = this._cameraSettings.AttachedToType_;
				ServerCameraSettings.AttachedToType attachedToType = attachedToType_;
				Vector3 result;
				if (attachedToType > 1)
				{
					if (attachedToType != 2)
					{
						throw new ArgumentOutOfRangeException(string.Format("Unknown AttachedToType {0}", this._cameraSettings.AttachedToType_));
					}
					result = Vector3.Zero;
				}
				else
				{
					result = this.AttachedTo.Position + this.GetEyeOffset();
				}
				return result;
			}
		}

		// Token: 0x1700124B RID: 4683
		// (get) Token: 0x06004C6B RID: 19563 RVA: 0x00144A56 File Offset: 0x00142C56
		public Vector3 PositionOffset
		{
			get
			{
				return this._customPositionOffset + this._positionDistanceOffset;
			}
		}

		// Token: 0x1700124C RID: 4684
		// (get) Token: 0x06004C6C RID: 19564 RVA: 0x00144A69 File Offset: 0x00142C69
		// (set) Token: 0x06004C6D RID: 19565 RVA: 0x00144A71 File Offset: 0x00142C71
		public Vector3 RotationOffset { get; private set; }

		// Token: 0x1700124D RID: 4685
		// (get) Token: 0x06004C6E RID: 19566 RVA: 0x00144A7A File Offset: 0x00142C7A
		// (set) Token: 0x06004C6F RID: 19567 RVA: 0x00144A82 File Offset: 0x00142C82
		public Vector3 Position { get; private set; }

		// Token: 0x1700124E RID: 4686
		// (get) Token: 0x06004C70 RID: 19568 RVA: 0x00144A8B File Offset: 0x00142C8B
		// (set) Token: 0x06004C71 RID: 19569 RVA: 0x00144A93 File Offset: 0x00142C93
		public Vector3 Rotation { get; private set; }

		// Token: 0x1700124F RID: 4687
		// (get) Token: 0x06004C72 RID: 19570 RVA: 0x00144A9C File Offset: 0x00142C9C
		// (set) Token: 0x06004C73 RID: 19571 RVA: 0x00144AA4 File Offset: 0x00142CA4
		public Vector3 LookAt { get; private set; }

		// Token: 0x17001250 RID: 4688
		// (get) Token: 0x06004C74 RID: 19572 RVA: 0x00144AB0 File Offset: 0x00142CB0
		public bool CanMove
		{
			get
			{
				ServerCameraSettings.CanMoveType canMoveType_ = this._cameraSettings.CanMoveType_;
				ServerCameraSettings.CanMoveType canMoveType = canMoveType_;
				bool result;
				if (canMoveType != null)
				{
					if (canMoveType != 1)
					{
						throw new ArgumentOutOfRangeException(string.Format("Unknown CanMoveType {0}", this._cameraSettings.CanMoveType_));
					}
					result = true;
				}
				else
				{
					result = (this.AttachedTo == this._gameInstance.LocalPlayer);
				}
				return result;
			}
		}

		// Token: 0x06004C75 RID: 19573 RVA: 0x00144B12 File Offset: 0x00142D12
		public ServerCameraController(GameInstance gameInstance, ServerCameraSettings cameraSettings)
		{
			this._gameInstance = gameInstance;
			this.CameraSettings = cameraSettings;
		}

		// Token: 0x06004C76 RID: 19574 RVA: 0x00144B2C File Offset: 0x00142D2C
		public void Reset(GameInstance gameInstance, ICameraController previousCameraController)
		{
			bool flag = this._gameInstance.LocalPlayer != null;
			if (flag)
			{
				this.Position = previousCameraController.Position;
				this.Rotation = previousCameraController.Rotation;
			}
		}

		// Token: 0x06004C77 RID: 19575 RVA: 0x00144B68 File Offset: 0x00142D68
		public void OnWorldJoined()
		{
			this.Position = this.GetPosition();
			this.Rotation = this.GetRotation();
		}

		// Token: 0x06004C78 RID: 19576 RVA: 0x00144B88 File Offset: 0x00142D88
		public void Update(float deltaTime)
		{
			ServerCameraSettings.PositionDistanceOffsetType positionDistanceOffsetType_ = this._cameraSettings.PositionDistanceOffsetType_;
			ServerCameraSettings.PositionDistanceOffsetType positionDistanceOffsetType = positionDistanceOffsetType_;
			if (positionDistanceOffsetType > 1)
			{
				if (positionDistanceOffsetType != 2)
				{
					throw new ArgumentOutOfRangeException(string.Format("Unknown PositionDistanceOffsetType {0}", this._cameraSettings.PositionDistanceOffsetType_));
				}
				this._positionDistanceOffset = Vector3.Zero;
			}
			else
			{
				Vector3 position = this.Position;
				Vector3 rotation = this.Rotation;
				Quaternion rotation2 = Quaternion.CreateFromYawPitchRoll(rotation.Y, rotation.X, 0f);
				Vector3 value = Vector3.Transform(Vector3.Forward, rotation2);
				float amount = 12f * deltaTime;
				Ray ray = new Ray(position, -value);
				CollisionModule.BlockRaycastOptions @default = CollisionModule.BlockRaycastOptions.Default;
				@default.Block.IgnoreFluids = true;
				@default.Block.IgnoreEmptyCollisionMaterial = true;
				Vector3 value2 = Vector3.Zero;
				CollisionModule.BlockResult blockResult;
				bool flag = (!this._gameInstance.CharacterControllerModule.MovementController.SkipHitDetectionWhenFlying || !this._gameInstance.CharacterControllerModule.MovementController.MovementStates.IsFlying) && !this.SkipCharacterPhysics && this._cameraSettings.PositionDistanceOffsetType_ == 1 && this._gameInstance.CollisionModule.FindTargetBlockOut(ref ray, ref @default, out blockResult);
				if (flag)
				{
					float num = MathHelper.Clamp(blockResult.Result.Distance() - 1f, 0f, this.Distance);
					bool flag2 = num < this._smoothCameraDistance;
					if (flag2)
					{
						this._smoothCameraDistance = num;
					}
					else
					{
						this._smoothCameraDistance = MathHelper.Lerp(this._smoothCameraDistance, num, amount);
					}
					bool flag3 = this._smoothCameraDistance < this.Distance;
					if (flag3)
					{
						value2 = blockResult.BlockNormal * 0.01f;
					}
				}
				else
				{
					this._smoothCameraDistance = MathHelper.Lerp(this._smoothCameraDistance, this.Distance, amount);
				}
				this._positionDistanceOffset = -value * this._smoothCameraDistance + value2;
			}
			this.Position = Vector3.Lerp(this.Position, this.GetPosition(), this._cameraSettings.PositionLerpSpeed / 0.016666668f * deltaTime);
			this.Rotation = Vector3.LerpAngle(this.Rotation, this.GetRotation(), this._cameraSettings.RotationLerpSpeed / 0.016666668f * deltaTime);
		}

		// Token: 0x06004C79 RID: 19577 RVA: 0x00144DD8 File Offset: 0x00142FD8
		public void ApplyMove(Vector3 movementOffset)
		{
			bool flag = this._cameraSettings.MovementMultiplier != null;
			if (flag)
			{
				movementOffset.X *= this._cameraSettings.MovementMultiplier.X;
				movementOffset.Y *= this._cameraSettings.MovementMultiplier.Y;
				movementOffset.Z *= this._cameraSettings.MovementMultiplier.Z;
			}
			ServerCameraSettings.ApplyMovementType applyMovementType_ = this._cameraSettings.ApplyMovementType_;
			ServerCameraSettings.ApplyMovementType applyMovementType = applyMovementType_;
			if (applyMovementType != null)
			{
				if (applyMovementType != 1)
				{
					throw new ArgumentOutOfRangeException(string.Format("Unknown ApplyMovementType {0}", this._cameraSettings.ApplyMovementType_));
				}
				this._customPosition += movementOffset;
			}
			else
			{
				this._gameInstance.CharacterControllerModule.MovementController.ApplyMovementOffset(movementOffset);
			}
		}

		// Token: 0x06004C7A RID: 19578 RVA: 0x00144EB4 File Offset: 0x001430B4
		public void ApplyLook(float deltaTime, Vector2 lookOffset)
		{
			bool flag = this._cameraSettings.LookMultiplier != null;
			if (flag)
			{
				lookOffset.X *= this._cameraSettings.LookMultiplier.X;
				lookOffset.Y *= this._cameraSettings.LookMultiplier.Y;
			}
			ServerCameraSettings.ApplyLookType applyLookType_ = this._cameraSettings.ApplyLookType_;
			ServerCameraSettings.ApplyLookType applyLookType = applyLookType_;
			if (applyLookType != null)
			{
				if (applyLookType != 1)
				{
					throw new ArgumentOutOfRangeException(string.Format("Unknown ApplyLookType {0}", this._cameraSettings.ApplyLookType_));
				}
				Vector3 customRotation = this._customRotation;
				ref Vector3 ptr = ref this._customRotation;
				ptr.Pitch = MathHelper.Clamp(ptr.Pitch + lookOffset.X, -1.5607964f, 1.5607964f);
				ptr.Yaw = MathHelper.WrapAngle(ptr.Yaw + lookOffset.Y);
				bool isFirstPerson = this.IsFirstPerson;
				if (isFirstPerson)
				{
					this._itemWiggleTickAccumulator += deltaTime;
					this._itemWiggleAmountAccumulator.X = this._itemWiggleAmountAccumulator.X + (customRotation.Pitch - ptr.Pitch) * 4f;
					this._itemWiggleAmountAccumulator.Y = this._itemWiggleAmountAccumulator.Y + lookOffset.Y * 4f;
					bool flag2 = this._itemWiggleTickAccumulator > 0.083333336f;
					if (flag2)
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
				}
			}
			else
			{
				bool flag3 = this.AttachedTo != this._gameInstance.LocalPlayer;
				if (!flag3)
				{
					Vector3 lookOrientation = this._gameInstance.LocalPlayer.LookOrientation;
					ref Vector3 ptr2 = ref this._gameInstance.LocalPlayer.LookOrientation;
					ptr2.Pitch = MathHelper.Clamp(ptr2.Pitch + lookOffset.X, -1.5607964f, 1.5607964f);
					ptr2.Yaw = MathHelper.WrapAngle(ptr2.Yaw + lookOffset.Y);
					bool isFirstPerson2 = this.IsFirstPerson;
					if (isFirstPerson2)
					{
						this._itemWiggleTickAccumulator += deltaTime;
						this._itemWiggleAmountAccumulator.X = this._itemWiggleAmountAccumulator.X + (lookOrientation.Pitch - ptr2.Pitch) * 4f;
						this._itemWiggleAmountAccumulator.Y = this._itemWiggleAmountAccumulator.Y + lookOffset.Y * 4f;
						bool flag4 = this._itemWiggleTickAccumulator > 0.083333336f;
						if (flag4)
						{
							this._itemWiggleTickAccumulator = 0.083333336f;
						}
						while (this._itemWiggleTickAccumulator >= 0.016666668f)
						{
							this._gameInstance.LocalPlayer.ApplyFirstPersonMouseItemWiggle(this._itemWiggleAmountAccumulator.Y, this._itemWiggleAmountAccumulator.X);
							this._itemWiggleAmountAccumulator.X = (this._itemWiggleAmountAccumulator.Y = 0f);
							this._itemWiggleTickAccumulator -= 0.016666668f;
						}
						float timeFraction2 = Math.Min(this._itemWiggleTickAccumulator / 0.016666668f, 1f);
						this._gameInstance.LocalPlayer.UpdateClientInterpolationMouseWiggle(timeFraction2);
					}
				}
			}
		}

		// Token: 0x06004C7B RID: 19579 RVA: 0x00145258 File Offset: 0x00143458
		public void SetRotation(Vector3 rotation)
		{
			this.Rotation = rotation;
		}

		// Token: 0x06004C7C RID: 19580 RVA: 0x00145264 File Offset: 0x00143464
		public void OnMouseInput(SDL.SDL_Event evt)
		{
			bool flag = evt.type == SDL.SDL_EventType.SDL_MOUSEBUTTONDOWN || evt.type == SDL.SDL_EventType.SDL_MOUSEBUTTONUP;
			if (flag)
			{
				MouseButtonType mouseButtonType_;
				switch (evt.button.button)
				{
				case 1:
					mouseButtonType_ = 0;
					break;
				case 2:
					mouseButtonType_ = 1;
					break;
				case 3:
					mouseButtonType_ = 2;
					break;
				case 4:
					mouseButtonType_ = 3;
					break;
				case 5:
					mouseButtonType_ = 4;
					break;
				default:
					return;
				}
				Vector2 vector = this._gameInstance.Engine.Window.SDLToNormalizedScreenCenterCoords(evt.button.x, evt.button.y);
				InventoryModule inventoryModule = this._gameInstance.InventoryModule;
				MouseInteraction mouseInteraction = new MouseInteraction
				{
					ClientTimestamp = TimeHelper.GetEpochMilliseconds(null),
					ActiveSlot = inventoryModule.HotbarActiveSlot,
					ItemInHand = inventoryModule.GetActiveItem().ToItemPacket(true),
					ScreenPoint = new Vector2f(vector.X, vector.Y),
					MouseButton = new MouseInteraction.MouseButtonEvent
					{
						Clicks = (sbyte)evt.button.clicks,
						MouseButtonType_ = mouseButtonType_,
						State = ((evt.button.state == 1) ? 0 : 1)
					}
				};
				Vector3 position;
				Vector3 direction;
				Vector3.ScreenToWorldRay(vector, this.Position, this._gameInstance.SceneRenderer.Data.InvViewProjectionMatrix, out position, out direction);
				Ray ray = new Ray(position, direction);
				CollisionModule.BlockResult? blockResult;
				CollisionModule.EntityResult? entityResult;
				this.MouseRaycast(mouseInteraction, ref ray, out blockResult, out entityResult);
				this._gameInstance.Connection.SendPacket(mouseInteraction);
			}
			else
			{
				bool flag2 = evt.type == SDL.SDL_EventType.SDL_MOUSEMOTION;
				if (flag2)
				{
					Vector2 vector2 = this._gameInstance.Engine.Window.SDLToNormalizedScreenCenterCoords(evt.motion.x, evt.motion.y);
					Vector3 vector3;
					Vector3 vector4;
					Vector3.ScreenToWorldRay(vector2, this.Position, this._gameInstance.SceneRenderer.Data.InvViewProjectionMatrix, out vector3, out vector4);
					Ray ray2 = new Ray(vector3, vector4);
					bool sendMouseMotion = this._cameraSettings.SendMouseMotion;
					if (sendMouseMotion)
					{
						List<MouseButtonType> list = new List<MouseButtonType>();
						bool flag3 = ((uint)evt.motion.state & SDL.SDL_BUTTON_LMASK) > 0U;
						if (flag3)
						{
							list.Add(0);
						}
						bool flag4 = ((uint)evt.motion.state & SDL.SDL_BUTTON_MMASK) > 0U;
						if (flag4)
						{
							list.Add(1);
						}
						bool flag5 = ((uint)evt.motion.state & SDL.SDL_BUTTON_RMASK) > 0U;
						if (flag5)
						{
							list.Add(2);
						}
						bool flag6 = ((uint)evt.motion.state & SDL.SDL_BUTTON_X1MASK) > 0U;
						if (flag6)
						{
							list.Add(3);
						}
						bool flag7 = ((uint)evt.motion.state & SDL.SDL_BUTTON_X2MASK) > 0U;
						if (flag7)
						{
							list.Add(4);
						}
						InventoryModule inventoryModule2 = this._gameInstance.InventoryModule;
						MouseInteraction mouseInteraction2 = new MouseInteraction
						{
							ClientTimestamp = TimeHelper.GetEpochMilliseconds(null),
							ActiveSlot = inventoryModule2.HotbarActiveSlot,
							ItemInHand = inventoryModule2.GetActiveItem().ToItemPacket(true),
							ScreenPoint = new Vector2f(vector2.X, vector2.Y),
							MouseMotion = new MouseInteraction.MouseMotionEvent
							{
								MouseButtonType_ = list.ToArray(),
								RelativeMotion = new Vector2i(evt.motion.xrel, evt.motion.yrel)
							}
						};
						CollisionModule.BlockResult? blockResult;
						CollisionModule.EntityResult? entityResult;
						this.MouseRaycast(mouseInteraction2, ref ray2, out blockResult, out entityResult);
						this._gameInstance.Connection.SendPacket(mouseInteraction2);
					}
					bool flag8 = this.AttachedTo != this._gameInstance.LocalPlayer;
					if (!flag8)
					{
						ServerCameraSettings.MouseInputType mouseInputType_ = this._cameraSettings.MouseInputType_;
						ServerCameraSettings.MouseInputType mouseInputType = mouseInputType_;
						if (mouseInputType > 2)
						{
							if (mouseInputType != 3)
							{
								throw new ArgumentOutOfRangeException(string.Format("Unknown MouseInputType {0}", this._cameraSettings.MouseInputType_));
							}
							Vector3 attachmentPosition = this.AttachmentPosition;
							Vector3 vector5 = (this._cameraSettings.PlaneNormal != null) ? new Vector3(this._cameraSettings.PlaneNormal.X, this._cameraSettings.PlaneNormal.Y, this._cameraSettings.PlaneNormal.Z) : Vector3.Up;
							float num = vector5.X * attachmentPosition.X + vector5.Y * attachmentPosition.Y + vector5.Z * attachmentPosition.Z;
							Plane plane = new Plane(vector5, -num);
							float? num2 = ray2.Intersects(plane);
							bool flag9 = num2 != null;
							if (flag9)
							{
								this._gameInstance.LocalPlayer.LookAt(vector3 + vector4 * num2.Value, 1f);
							}
						}
						else
						{
							CollisionModule.CombinedOptions @default = CollisionModule.CombinedOptions.Default;
							ServerCameraSettings.MouseInputType mouseInputType_2 = this._cameraSettings.MouseInputType_;
							ServerCameraSettings.MouseInputType mouseInputType2 = mouseInputType_2;
							if (mouseInputType2 != 1)
							{
								if (mouseInputType2 == 2)
								{
									@default.EnableBlock = false;
								}
							}
							else
							{
								@default.Block.IgnoreFluids = true;
								@default.EnableEntity = false;
							}
							CollisionModule.BlockResult? blockResult;
							CollisionModule.EntityResult? entityResult;
							Raycast.Result result;
							bool flag10 = this._gameInstance.CollisionModule.FindNearestTarget(ref ray2, ref @default, out blockResult, out entityResult, out result);
							if (flag10)
							{
								this._gameInstance.LocalPlayer.LookAt(result.GetTarget(), 1f);
							}
						}
					}
				}
			}
		}

		// Token: 0x06004C7D RID: 19581 RVA: 0x001457C4 File Offset: 0x001439C4
		private void MouseRaycast(MouseInteraction mouseInteraction, ref Ray ray, out CollisionModule.BlockResult? blockResult, out CollisionModule.EntityResult? entityResult)
		{
			mouseInteraction.WorldInteraction_ = new WorldInteraction();
			CollisionModule.CombinedOptions @default = CollisionModule.CombinedOptions.Default;
			switch (this._cameraSettings.MouseInputTargetType_)
			{
			case 0:
				break;
			case 1:
				@default.EnableEntity = false;
				break;
			case 2:
				@default.EnableBlock = false;
				break;
			case 3:
				blockResult = null;
				entityResult = null;
				return;
			default:
				throw new ArgumentOutOfRangeException(string.Format("Unknown MouseClickTargetType_ {0}", this._cameraSettings.MouseInputTargetType_));
			}
			bool flag = this._gameInstance.CollisionModule.FindNearestTarget(ref ray, ref @default, out blockResult, out entityResult);
			if (flag)
			{
				bool flag2 = entityResult != null;
				if (flag2)
				{
					mouseInteraction.WorldInteraction_.EntityId = entityResult.Value.Entity.NetworkId;
				}
				else
				{
					bool flag3 = blockResult != null;
					if (flag3)
					{
						mouseInteraction.WorldInteraction_.BlockPosition_ = blockResult.Value.GetBlockPosition();
					}
				}
			}
		}

		// Token: 0x06004C7E RID: 19582 RVA: 0x001458C4 File Offset: 0x00143AC4
		private Vector3 GetEyeOffset()
		{
			bool flag = !this._cameraSettings.EyeOffset || this.AttachedTo == null;
			Vector3 result;
			if (flag)
			{
				result = Vector3.Zero;
			}
			else
			{
				bool flag2 = this._cameraSettings.AttachedToType_ > 0;
				if (flag2)
				{
					result = new Vector3(0f, this.AttachedTo.EyeOffset, 0f);
				}
				else
				{
					CharacterControllerModule characterControllerModule = this._gameInstance.CharacterControllerModule;
					result = new Vector3(0f, this.AttachedTo.EyeOffset + characterControllerModule.MovementController.CrouchHeightShift, 0f);
				}
			}
			return result;
		}

		// Token: 0x06004C7F RID: 19583 RVA: 0x00145960 File Offset: 0x00143B60
		private Vector3 GetPosition()
		{
			ServerCameraSettings.PositionType positionType_ = this._cameraSettings.PositionType_;
			ServerCameraSettings.PositionType positionType = positionType_;
			Vector3 result;
			if (positionType != null)
			{
				if (positionType != 1)
				{
					throw new ArgumentOutOfRangeException(string.Format("Unknown PositionType {0}", this._cameraSettings.PositionType_));
				}
				result = this._customPosition;
			}
			else
			{
				result = this.AttachmentPosition + this.PositionOffset;
			}
			return result;
		}

		// Token: 0x06004C80 RID: 19584 RVA: 0x001459C8 File Offset: 0x00143BC8
		private Vector3 GetRotation()
		{
			ServerCameraSettings.RotationType rotationType_ = this._cameraSettings.RotationType_;
			ServerCameraSettings.RotationType rotationType = rotationType_;
			Vector3 result;
			if (rotationType != null)
			{
				if (rotationType != 1)
				{
					throw new ArgumentOutOfRangeException(string.Format("Unknown RotationType {0}", this._cameraSettings.RotationType_));
				}
				Vector3 vector = Vector3.WrapAngle(this._customRotation);
				vector.Pitch = MathHelper.Clamp(vector.Pitch, -1.5607964f, 1.5607964f);
				result = vector;
			}
			else
			{
				Vector3 vector2 = Vector3.WrapAngle(this.AttachedTo.LookOrientation + this.RotationOffset);
				vector2.Pitch = MathHelper.Clamp(vector2.Pitch, -1.5607964f, 1.5607964f);
				result = vector2;
			}
			return result;
		}

		// Token: 0x040027FF RID: 10239
		private ServerCameraSettings _cameraSettings;

		// Token: 0x04002800 RID: 10240
		private Vector3 _customMovementForceRotation;

		// Token: 0x04002801 RID: 10241
		private Entity _cachedEntity;

		// Token: 0x04002802 RID: 10242
		private Vector3 _customPositionOffset;

		// Token: 0x04002803 RID: 10243
		private Vector3 _positionDistanceOffset;

		// Token: 0x04002805 RID: 10245
		private Vector3 _customPosition;

		// Token: 0x04002807 RID: 10247
		private Vector3 _customRotation;

		// Token: 0x0400280A RID: 10250
		private readonly GameInstance _gameInstance;

		// Token: 0x0400280B RID: 10251
		private float _smoothCameraDistance;

		// Token: 0x0400280C RID: 10252
		private float _itemWiggleTickAccumulator;

		// Token: 0x0400280D RID: 10253
		private Vector2 _itemWiggleAmountAccumulator;
	}
}
