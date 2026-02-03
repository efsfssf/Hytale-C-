using System;
using HytaleClient.Data.UserSettings;
using HytaleClient.InGame.Modules.Camera.Controllers;
using HytaleClient.InGame.Modules.Camera.Controllers.CameraShake;
using HytaleClient.Math;

namespace HytaleClient.InGame.Modules.Camera
{
	// Token: 0x02000970 RID: 2416
	internal class CameraModule : Module
	{
		// Token: 0x17001208 RID: 4616
		// (get) Token: 0x06004BF2 RID: 19442 RVA: 0x00143052 File Offset: 0x00141252
		// (set) Token: 0x06004BF3 RID: 19443 RVA: 0x0014305A File Offset: 0x0014125A
		public ICameraController Controller { get; private set; }

		// Token: 0x17001209 RID: 4617
		// (get) Token: 0x06004BF4 RID: 19444 RVA: 0x00143063 File Offset: 0x00141263
		public CameraShakeController CameraShakeController { get; }

		// Token: 0x06004BF5 RID: 19445 RVA: 0x0014306C File Offset: 0x0014126C
		private void MouseSensitivityEasingUpdate()
		{
			bool flag = this._easingDurationLeft == 0f;
			if (!flag)
			{
				this._easingDurationLeft -= 0.016666668f;
				bool flag2 = this._easingDurationLeft <= 0f;
				if (flag2)
				{
					this._easingDurationLeft = 0f;
				}
				this._easingProgress = Easing.Ease(Easing.EasingType.Linear, this._easingDuration - this._easingDurationLeft, 0f, 1f, this._easingDuration);
			}
		}

		// Token: 0x06004BF6 RID: 19446 RVA: 0x001430EC File Offset: 0x001412EC
		private float GetCurrentMouseModifierValue()
		{
			float num = this._mouseSensitivityStart - (this._mouseSensitivityStart - this._mouseSensitivityTarget) * this._easingProgress;
			bool flag = num < 1E-05f;
			float result;
			if (flag)
			{
				result = 0f;
			}
			else
			{
				result = num;
			}
			return result;
		}

		// Token: 0x06004BF7 RID: 19447 RVA: 0x00143130 File Offset: 0x00141330
		public CameraModule(GameInstance gameInstance) : base(gameInstance)
		{
			this._controllerTypes = new ICameraController[]
			{
				new FirstPersonCameraController(gameInstance),
				new ThirdPersonCameraController(gameInstance)
			};
			this.CameraShakeController = new CameraShakeController(gameInstance);
			this._freeRotateCameraController = new FreeRotateCameraController(gameInstance);
			this._controllerIndex = this._gameInstance.App.Settings.SavedCameraIndex;
			this.UpdateCameraController(this._controllerTypes[this._controllerIndex]);
		}

		// Token: 0x06004BF8 RID: 19448 RVA: 0x00143206 File Offset: 0x00141406
		public void SetTargetMouseModifier(float targetModifier, float modiferChangeRateTime)
		{
			this._mouseSensitivityStart = this.GetCurrentMouseModifierValue();
			this._mouseSensitivityTarget = targetModifier;
			this._easingDuration = modiferChangeRateTime;
			this._easingDurationLeft = modiferChangeRateTime;
		}

		// Token: 0x06004BF9 RID: 19449 RVA: 0x0014322C File Offset: 0x0014142C
		public void Update(float deltaTime)
		{
			this._offset = Vector2.Zero;
			bool shake = this._shake;
			if (shake)
			{
				this.ComputeShakeOffset();
			}
			else
			{
				bool flag = this._totalOffset != Vector2.Zero;
				if (flag)
				{
					this._offset = -this._totalOffset;
					this._totalOffset = Vector2.Zero;
				}
			}
			this._shakeTimer = MathHelper.Max(0f, this._shakeTimer - deltaTime);
			bool flag2 = this._gameInstance.Input.ConsumeBinding(this._gameInstance.App.Settings.InputBindings.SwitchCameraMode, false);
			if (flag2)
			{
				bool flag3 = this._controllerIndex != -1;
				if (flag3)
				{
					bool flag4 = this._gameInstance.Input.IsCtrlHeld() && this._gameInstance.Input.IsShiftHeld();
					this.SetCameraControllerIndex(this._controllerIndex + (flag4 ? -1 : 1));
				}
			}
			else
			{
				bool flag5 = this._gameInstance.Input.IsBindingHeld(this._gameInstance.App.Settings.InputBindings.ActivateCameraRotation, false);
				bool flag6 = flag5 && this.Controller != this._freeRotateCameraController && this._controllerIndex != -1;
				if (flag6)
				{
					this.SetCustomCameraController(this._freeRotateCameraController);
				}
				else
				{
					bool flag7 = !flag5 && this.Controller == this._freeRotateCameraController;
					if (flag7)
					{
						this.ResetCameraController();
					}
				}
			}
			MouseSettings mouseSettings = this._gameInstance.App.Settings.MouseSettings;
			float num = mouseSettings.MouseXSpeed / 20f;
			float num2 = mouseSettings.MouseYSpeed / 20f;
			this.MouseSensitivityEasingUpdate();
			float currentMouseModifierValue = this.GetCurrentMouseModifierValue();
			num *= currentMouseModifierValue;
			num2 *= currentMouseModifierValue;
			bool mouseRawInputMode = mouseSettings.MouseRawInputMode;
			if (mouseRawInputMode)
			{
				this._look.X = this._look.X * (0.01f * num * (mouseSettings.MouseInverted ? -1f : 1f));
				this._look.Y = this._look.Y * (0.01f * num2);
			}
			else
			{
				this._smoothedLook.X = this._smoothedLook.X + this._look.X * (mouseSettings.MouseInverted ? -1f : 1f);
				this._smoothedLook.Y = this._smoothedLook.Y + this._look.Y;
				this._smoothedLook.X = this._smoothedLook.X * num;
				this._smoothedLook.Y = this._smoothedLook.Y * num2;
				this._look.X = this._smoothedLook.X * 0.01f;
				this._look.Y = this._smoothedLook.Y * 0.01f;
			}
			this.Controller.ApplyLook(deltaTime, this._look + this._offset);
			this._look.X = (this._look.Y = 0f);
			this.Controller.Update(deltaTime);
		}

		// Token: 0x06004BFA RID: 19450 RVA: 0x0014354C File Offset: 0x0014174C
		private void ComputeShakeOffset()
		{
			this._offset = new Vector2((float)(Math.Sin((double)this._shakeStartAngle) * (double)this._shakeRadius), (float)(Math.Cos((double)this._shakeStartAngle) * (double)this._shakeRadius));
			this._offset /= 20f;
			this._totalOffset += this._offset;
			this._shakeRadius -= 0.1f;
			this._shakeStartAngle += this._random.NextFloat(0f, 6.2831855f);
			bool flag = this._shakeTimer == 0f || this._shakeRadius <= 0f;
			if (flag)
			{
				this._shake = false;
				this.ResetCameraController();
			}
		}

		// Token: 0x06004BFB RID: 19451 RVA: 0x00143624 File Offset: 0x00141824
		public void Shake(float duration, float force)
		{
			this._shake = true;
			this._shakeRadius = force;
			this._shakeStartAngle = this._random.NextFloat(0f, 6.2831855f);
			this._totalOffset = Vector2.Zero;
			this._shakeTimer += duration;
		}

		// Token: 0x06004BFC RID: 19452 RVA: 0x00143674 File Offset: 0x00141874
		private void UpdateCameraController(ICameraController cameraController)
		{
			ICameraController controller = this.Controller;
			this.Controller = cameraController;
			cameraController.Reset(this._gameInstance, controller);
			this._gameInstance.App.Interface.TriggerEvent("crosshair.setVisible", this.Controller.IsFirstPerson || this.Controller.DisplayReticle, null, null, null, null, null);
			this._gameInstance.Engine.Window.SetCursorVisible(this.Controller.DisplayCursor);
		}

		// Token: 0x06004BFD RID: 19453 RVA: 0x00143700 File Offset: 0x00141900
		public void OffsetLook(float x, float y)
		{
			this._look.X = this._look.X + x;
			this._look.Y = this._look.Y + y;
		}

		// Token: 0x06004BFE RID: 19454 RVA: 0x00143724 File Offset: 0x00141924
		public void SetCameraControllerIndex(int cameraControllerIndex)
		{
			this._controllerIndex = cameraControllerIndex;
			bool flag = this._controllerIndex < 0;
			if (flag)
			{
				this._controllerIndex = this._controllerTypes.Length - 1;
			}
			else
			{
				bool flag2 = this._controllerIndex >= this._controllerTypes.Length;
				if (flag2)
				{
					this._controllerIndex = 0;
				}
			}
			this._gameInstance.App.Settings.SavedCameraIndex = this._controllerIndex;
			this._gameInstance.App.Settings.Save();
			this.UpdateCameraController(this._controllerTypes[this._controllerIndex]);
		}

		// Token: 0x06004BFF RID: 19455 RVA: 0x001437BC File Offset: 0x001419BC
		public bool IsCustomCameraControllerSet()
		{
			return this._controllerIndex == -1;
		}

		// Token: 0x06004C00 RID: 19456 RVA: 0x001437D8 File Offset: 0x001419D8
		public void SetCustomCameraController(ICameraController cameraController)
		{
			bool flag = cameraController == null;
			if (flag)
			{
				this.ResetCameraController();
			}
			else
			{
				bool flag2 = this._controllerIndex != -1;
				if (flag2)
				{
					this._oldControllerIndex = this._controllerIndex;
					this._controllerIndex = -1;
					this._oldController = this.Controller;
				}
				this.UpdateCameraController(cameraController);
			}
		}

		// Token: 0x06004C01 RID: 19457 RVA: 0x00143834 File Offset: 0x00141A34
		public void ResetCameraController()
		{
			bool flag = this._controllerIndex != -1;
			if (!flag)
			{
				this.UpdateCameraController(this._oldController);
				this._controllerIndex = this._oldControllerIndex;
				this._oldControllerIndex = -1;
				this._oldController = null;
			}
		}

		// Token: 0x06004C02 RID: 19458 RVA: 0x0014387B File Offset: 0x00141A7B
		public void LockCamera()
		{
			this._controllerIndex = -1;
		}

		// Token: 0x06004C03 RID: 19459 RVA: 0x00143888 File Offset: 0x00141A88
		public Ray GetLookRay()
		{
			Quaternion rotation = Quaternion.CreateFromYawPitchRoll(this.Controller.Rotation.Yaw, this.Controller.Rotation.Pitch, 0f);
			Vector3 direction = Vector3.Transform(Vector3.Forward, rotation);
			return new Ray(this.Controller.Position, direction);
		}

		// Token: 0x040027CD RID: 10189
		private const float FloatEpsilon = 1E-05f;

		// Token: 0x040027D0 RID: 10192
		private ICameraController[] _controllerTypes;

		// Token: 0x040027D1 RID: 10193
		private FreeRotateCameraController _freeRotateCameraController;

		// Token: 0x040027D2 RID: 10194
		private int _controllerIndex;

		// Token: 0x040027D3 RID: 10195
		private ICameraController _oldController;

		// Token: 0x040027D4 RID: 10196
		private int _oldControllerIndex;

		// Token: 0x040027D5 RID: 10197
		private Vector2 _look;

		// Token: 0x040027D6 RID: 10198
		private Vector2 _smoothedLook;

		// Token: 0x040027D7 RID: 10199
		private bool _shake = false;

		// Token: 0x040027D8 RID: 10200
		private Vector2 _offset;

		// Token: 0x040027D9 RID: 10201
		private Random _random = new Random();

		// Token: 0x040027DA RID: 10202
		private float _shakeRadius;

		// Token: 0x040027DB RID: 10203
		private float _shakeStartAngle;

		// Token: 0x040027DC RID: 10204
		private Vector2 _totalOffset;

		// Token: 0x040027DD RID: 10205
		private float _shakeTimer = 0f;

		// Token: 0x040027DE RID: 10206
		private Easing.EasingType _mouseSensitivityEasing = Easing.EasingType.Linear;

		// Token: 0x040027DF RID: 10207
		private float _easingDurationLeft = 0f;

		// Token: 0x040027E0 RID: 10208
		private float _easingDuration = 0f;

		// Token: 0x040027E1 RID: 10209
		private float _mouseSensitivityTarget = 1f;

		// Token: 0x040027E2 RID: 10210
		private float _mouseSensitivityStart = 1f;

		// Token: 0x040027E3 RID: 10211
		private float _easingProgress = 1f;
	}
}
