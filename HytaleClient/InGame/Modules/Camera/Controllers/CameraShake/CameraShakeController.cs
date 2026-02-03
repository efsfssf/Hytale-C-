using System;
using System.Collections.Generic;
using HytaleClient.Math;
using HytaleClient.Protocol;

namespace HytaleClient.InGame.Modules.Camera.Controllers.CameraShake
{
	// Token: 0x02000978 RID: 2424
	internal class CameraShakeController
	{
		// Token: 0x17001262 RID: 4706
		// (get) Token: 0x06004CB4 RID: 19636 RVA: 0x00147068 File Offset: 0x00145268
		// (set) Token: 0x06004CB5 RID: 19637 RVA: 0x00147070 File Offset: 0x00145270
		public Vector3 Offset { get; private set; } = Vector3.Zero;

		// Token: 0x17001263 RID: 4707
		// (get) Token: 0x06004CB6 RID: 19638 RVA: 0x00147079 File Offset: 0x00145279
		// (set) Token: 0x06004CB7 RID: 19639 RVA: 0x00147081 File Offset: 0x00145281
		public Vector3 Rotation { get; private set; } = Vector3.Zero;

		// Token: 0x06004CB8 RID: 19640 RVA: 0x0014708C File Offset: 0x0014528C
		public CameraShakeController(GameInstance gameInstance)
		{
			this._gameInstance = gameInstance;
			this._viewBobbingCameraShake = new ViewBobbingCameraShake(this._gameInstance);
			this._activeCameraShakes.Add(this._viewBobbingCameraShake);
		}

		// Token: 0x06004CB9 RID: 19641 RVA: 0x00147100 File Offset: 0x00145300
		public void Reset()
		{
			for (int i = this._activeCameraShakes.Count - 1; i >= 0; i--)
			{
				CameraShake cameraShake = this._activeCameraShakes[i];
				cameraShake.Reset();
				bool flag = i == 0;
				if (!flag)
				{
					this._activeCameraShakes.RemoveAt(i);
				}
			}
		}

		// Token: 0x06004CBA RID: 19642 RVA: 0x0014715C File Offset: 0x0014535C
		public CameraShake GetCameraShake(int cameraShakeIndex)
		{
			bool flag = this._cameraShakes.ContainsKey(cameraShakeIndex);
			CameraShake result;
			if (flag)
			{
				result = this._cameraShakes[cameraShakeIndex];
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06004CBB RID: 19643 RVA: 0x00147190 File Offset: 0x00145390
		public bool PlayCameraShake(int cameraShakeIndex, float intensity, AccumulationMode accumulationMode)
		{
			CameraShake cameraShake = this.GetCameraShake(cameraShakeIndex);
			bool flag = cameraShake == null;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = cameraShake.IsActive();
				if (flag2)
				{
					cameraShake.ExtendDuration();
					cameraShake.SetIntensityTarget(intensity, accumulationMode);
				}
				else
				{
					cameraShake.SetIntensity(intensity);
					this._activeCameraShakes.Add(cameraShake);
				}
				result = true;
			}
			return result;
		}

		// Token: 0x06004CBC RID: 19644 RVA: 0x001471EC File Offset: 0x001453EC
		public void Update(float deltaTime, Quaternion angle)
		{
			bool flag = this._gameInstance.IsBuilderModeEnabled();
			if (flag)
			{
				bool enabled = this._enabled;
				if (enabled)
				{
					this.Reset();
					this._enabled = false;
				}
			}
			else
			{
				this._enabled = true;
				this._time += deltaTime;
				Vector3 offset;
				Vector3 rotation;
				this.UpdateActiveCameraShakes(deltaTime, angle, out offset, out rotation);
				this.Offset = offset;
				this.Rotation = rotation;
			}
		}

		// Token: 0x06004CBD RID: 19645 RVA: 0x00147258 File Offset: 0x00145458
		public void UpdateViewBobbingAssets(UpdateViewBobbing packet)
		{
			this._viewBobbingCameraShake.UpdateViewBobbingTypes(packet);
		}

		// Token: 0x06004CBE RID: 19646 RVA: 0x00147268 File Offset: 0x00145468
		public void UpdateCameraShakeAssets(UpdateCameraShake packet)
		{
			this.Reset();
			foreach (KeyValuePair<int, CameraShake> keyValuePair in packet.Profiles)
			{
				bool flag = keyValuePair.Value == null;
				if (flag)
				{
					this._cameraShakes.Remove(keyValuePair.Key);
				}
				else
				{
					CameraShake value = keyValuePair.Value;
					CameraShakeType firstPerson = new CameraShakeType(value.FirstPerson);
					CameraShakeType thirdPerson = new CameraShakeType(value.ThirdPerson);
					this._cameraShakes[keyValuePair.Key] = new TimedCameraShake(firstPerson, thirdPerson, this._gameInstance);
				}
			}
		}

		// Token: 0x06004CBF RID: 19647 RVA: 0x0014732C File Offset: 0x0014552C
		private void UpdateActiveCameraShakes(float deltaTime, Quaternion angle, out Vector3 offset, out Vector3 rotation)
		{
			offset = Vector3.Zero;
			rotation = Vector3.Zero;
			for (int i = this._activeCameraShakes.Count - 1; i >= 0; i--)
			{
				CameraShake cameraShake = this._activeCameraShakes[i];
				cameraShake.Update(this._time, deltaTime, angle);
				cameraShake.AddShake(ref offset, ref rotation);
				bool flag = !cameraShake.IsComplete();
				if (!flag)
				{
					cameraShake.Reset();
					this._activeCameraShakes.RemoveAt(i);
				}
			}
		}

		// Token: 0x04002831 RID: 10289
		private float _time;

		// Token: 0x04002832 RID: 10290
		private bool _enabled = true;

		// Token: 0x04002833 RID: 10291
		protected readonly GameInstance _gameInstance;

		// Token: 0x04002834 RID: 10292
		private readonly ViewBobbingCameraShake _viewBobbingCameraShake;

		// Token: 0x04002835 RID: 10293
		private readonly List<CameraShake> _activeCameraShakes = new List<CameraShake>();

		// Token: 0x04002836 RID: 10294
		private readonly Dictionary<int, CameraShake> _cameraShakes = new Dictionary<int, CameraShake>();
	}
}
