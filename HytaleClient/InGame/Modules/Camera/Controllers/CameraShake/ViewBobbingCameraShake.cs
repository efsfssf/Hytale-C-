using System;
using System.Collections.Generic;
using HytaleClient.InGame.Modules.CharacterController;
using HytaleClient.Math;
using HytaleClient.Protocol;

namespace HytaleClient.InGame.Modules.Camera.Controllers.CameraShake
{
	// Token: 0x0200097C RID: 2428
	internal class ViewBobbingCameraShake : CameraShake
	{
		// Token: 0x06004CD5 RID: 19669 RVA: 0x001478EA File Offset: 0x00145AEA
		public ViewBobbingCameraShake(GameInstance gameInstance)
		{
			this._movementType = 0;
			this._gameInstance = gameInstance;
		}

		// Token: 0x06004CD6 RID: 19670 RVA: 0x00147910 File Offset: 0x00145B10
		public void UpdateViewBobbingTypes(UpdateViewBobbing packet)
		{
			foreach (KeyValuePair<MovementType, ViewBobbing> keyValuePair in packet.Profiles)
			{
				bool flag = keyValuePair.Value == null;
				if (flag)
				{
					this._viewBobbingTypes[keyValuePair.Key] = CameraShakeType.None;
				}
				else
				{
					ViewBobbing value = keyValuePair.Value;
					this._viewBobbingTypes[keyValuePair.Key] = new CameraShakeType(value.FirstPerson);
				}
			}
			this._activeShakeType = (this._viewBobbingTypes[this._movementType] ?? CameraShakeType.None);
		}

		// Token: 0x06004CD7 RID: 19671 RVA: 0x001479D4 File Offset: 0x00145BD4
		public override void Reset()
		{
			base.Reset();
			this._movementType = 0;
		}

		// Token: 0x06004CD8 RID: 19672 RVA: 0x001479E8 File Offset: 0x00145BE8
		public override bool IsComplete()
		{
			return false;
		}

		// Token: 0x06004CD9 RID: 19673 RVA: 0x001479FC File Offset: 0x00145BFC
		protected override bool Update()
		{
			bool flag = !this._gameInstance.App.Settings.ViewBobbingEffect || !this._gameInstance.CameraModule.Controller.IsFirstPerson;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				MovementController movementController = this._gameInstance.CharacterControllerModule.MovementController;
				MovementType movementType = ViewBobbingCameraShake.GetMovementType(movementController);
				bool flag2 = movementType == 0;
				if (flag2)
				{
					this._timer.Set(0f, 0.5f, 0f, Easing.EasingType.Linear, Easing.EasingType.Linear);
					this._timer.Restart();
					result = false;
				}
				else
				{
					bool flag3 = this._movementType != movementType || !MathHelper.WithinEpsilon(this._frequency, movementController.SpeedMultiplier);
					if (flag3)
					{
						this._movementType = movementType;
						this._activeShakeType = (this._viewBobbingTypes[this._movementType] ?? CameraShakeType.None);
						this._timer.Set(0f, this._activeShakeType.EaseIn, 0f, Easing.EasingType.Linear, Easing.EasingType.Linear);
						this._timer.Restart();
						this._frequency = movementController.SpeedMultiplier;
						this._intensityMultiplier = this._gameInstance.App.Settings.ViewBobbingIntensity;
					}
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06004CDA RID: 19674 RVA: 0x00147B44 File Offset: 0x00145D44
		protected override Vector3 ComputeOffset(float time, float intensity, Quaternion angle)
		{
			Vector3 value = this._activeShakeType.Offset.Eval(this._seed, time) * intensity;
			Vector3.Transform(ref value, ref angle, out value);
			return Vector3.Lerp(this._offset, value, this._timer.Easing);
		}

		// Token: 0x06004CDB RID: 19675 RVA: 0x00147B98 File Offset: 0x00145D98
		protected override Vector3 ComputeRotation(float time, float intensity, Quaternion angle)
		{
			Vector3 value = this._activeShakeType.Rotation.Eval(this._seed, time) * intensity;
			return Vector3.Lerp(this._rotation, value, this._timer.Easing);
		}

		// Token: 0x06004CDC RID: 19676 RVA: 0x00147BE0 File Offset: 0x00145DE0
		private static MovementType GetMovementType(MovementController controller)
		{
			bool isFlying = controller.MovementStates.IsFlying;
			MovementType result;
			if (isFlying)
			{
				result = 8;
			}
			else
			{
				bool isSwimming = controller.MovementStates.IsSwimming;
				if (isSwimming)
				{
					result = 7;
				}
				else
				{
					bool flag = controller.MovementStates.IsIdle || controller.MovementStates.IsHorizontalIdle;
					if (flag)
					{
						result = 1;
					}
					else
					{
						bool isClimbing = controller.MovementStates.IsClimbing;
						if (isClimbing)
						{
							result = 6;
						}
						else
						{
							bool isSwimming2 = controller.MovementStates.IsSwimming;
							if (isSwimming2)
							{
								result = 7;
							}
							else
							{
								bool flag2 = !controller.MovementStates.IsOnGround;
								if (flag2)
								{
									result = 0;
								}
								else
								{
									bool isCrouching = controller.MovementStates.IsCrouching;
									if (isCrouching)
									{
										result = 2;
									}
									else
									{
										bool isWalking = controller.MovementStates.IsWalking;
										if (isWalking)
										{
											result = 3;
										}
										else
										{
											bool flag3 = controller.MovementStates.IsSprinting || (controller.SprintForceDurationLeft > 0f && !controller.MovementStates.IsWalking);
											if (flag3)
											{
												result = 5;
											}
											else
											{
												result = 4;
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

		// Token: 0x0400284D RID: 10317
		private const float DefaultEaseIn = 0.5f;

		// Token: 0x0400284E RID: 10318
		private const float DefaultDuration = 0f;

		// Token: 0x0400284F RID: 10319
		private MovementType _movementType;

		// Token: 0x04002850 RID: 10320
		private readonly GameInstance _gameInstance;

		// Token: 0x04002851 RID: 10321
		private readonly Dictionary<MovementType, CameraShakeType> _viewBobbingTypes = new Dictionary<MovementType, CameraShakeType>();
	}
}
