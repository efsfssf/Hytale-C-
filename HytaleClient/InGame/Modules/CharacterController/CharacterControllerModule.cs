using System;
using System.Collections.Generic;
using HytaleClient.Core;
using HytaleClient.Data.UserSettings;
using HytaleClient.InGame.Modules.CharacterController.DefaultController;
using HytaleClient.InGame.Modules.CharacterController.MountController;
using HytaleClient.Math;
using HytaleClient.Protocol;

namespace HytaleClient.InGame.Modules.CharacterController
{
	// Token: 0x0200096C RID: 2412
	internal class CharacterControllerModule : Module
	{
		// Token: 0x170011FA RID: 4602
		// (get) Token: 0x06004B57 RID: 19287 RVA: 0x001366B4 File Offset: 0x001348B4
		// (set) Token: 0x06004B58 RID: 19288 RVA: 0x001366BC File Offset: 0x001348BC
		public MovementController MovementController { get; private set; }

		// Token: 0x170011FB RID: 4603
		// (get) Token: 0x06004B59 RID: 19289 RVA: 0x001366C5 File Offset: 0x001348C5
		// (set) Token: 0x06004B5A RID: 19290 RVA: 0x001366CD File Offset: 0x001348CD
		public float ForwardsTimestamp { get; private set; }

		// Token: 0x170011FC RID: 4604
		// (get) Token: 0x06004B5B RID: 19291 RVA: 0x001366D6 File Offset: 0x001348D6
		// (set) Token: 0x06004B5C RID: 19292 RVA: 0x001366DE File Offset: 0x001348DE
		public float BackwardsTimestamp { get; private set; }

		// Token: 0x170011FD RID: 4605
		// (get) Token: 0x06004B5D RID: 19293 RVA: 0x001366E7 File Offset: 0x001348E7
		// (set) Token: 0x06004B5E RID: 19294 RVA: 0x001366EF File Offset: 0x001348EF
		public float LeftTimestamp { get; private set; }

		// Token: 0x170011FE RID: 4606
		// (get) Token: 0x06004B5F RID: 19295 RVA: 0x001366F8 File Offset: 0x001348F8
		// (set) Token: 0x06004B60 RID: 19296 RVA: 0x00136700 File Offset: 0x00134900
		public float RightTimestamp { get; private set; }

		// Token: 0x06004B61 RID: 19297 RVA: 0x0013670C File Offset: 0x0013490C
		public CharacterControllerModule(GameInstance gameInstance) : base(gameInstance)
		{
			this._movementControllers.Add("Default", new DefaultMovementController(gameInstance));
			this._movementControllers.Add("Mount", new MountMovementController(gameInstance));
			this.MovementController = this._movementControllers["Default"];
		}

		// Token: 0x06004B62 RID: 19298 RVA: 0x00136774 File Offset: 0x00134974
		public override void Initialize()
		{
			bool flag = !this._gameInstance.Engine.Audio.ResourceManager.WwiseGameParameterIds.TryGetValue("SPEED", out this._playerSpeedRTPCId);
			if (flag)
			{
				this._gameInstance.App.DevTools.Error("Missing speed RTPC: SPEED");
			}
		}

		// Token: 0x06004B63 RID: 19299 RVA: 0x001367D0 File Offset: 0x001349D0
		public override void Tick()
		{
			this.MovementController.Tick();
			Vector3 velocity = this.MovementController.Velocity;
			float num = (float)Math.Sqrt((double)(velocity.X * velocity.X) + (double)(velocity.Z * velocity.Z) + (double)(velocity.Y * velocity.Y));
			bool flag = Math.Abs(num - this._previousSpeedRTPCValue) > 0.01f;
			if (flag)
			{
				this._gameInstance.Engine.Audio.SetRTPC(this._playerSpeedRTPCId, num);
				this._previousSpeedRTPCValue = num;
			}
		}

		// Token: 0x06004B64 RID: 19300 RVA: 0x00136868 File Offset: 0x00134A68
		public void MountNpc(MountNPC packet)
		{
			MountMovementController mountMovementController = (MountMovementController)this._movementControllers["Mount"];
			mountMovementController.OnMount(packet);
			MovementSettings movementSettings = this.MovementController.MovementSettings;
			this.MovementController = mountMovementController;
			this.MovementController.MovementSettings = movementSettings;
			Vector3 lookOrientation = this._gameInstance.LocalPlayer.LookOrientation;
			lookOrientation.Pitch = 0f;
			lookOrientation.Roll = 0f;
			this.MovementController.CameraRotation = lookOrientation;
			this._gameInstance.EntityStoreModule.MountEntityLocalId = packet.EntityId;
		}

		// Token: 0x06004B65 RID: 19301 RVA: 0x00136904 File Offset: 0x00134B04
		public void DismountNpc(bool isLocalInteraction = false)
		{
			MountMovementController mountMovementController = this.MovementController as MountMovementController;
			bool flag = mountMovementController != null;
			if (flag)
			{
				mountMovementController.OnDismount(isLocalInteraction);
			}
			this.MovementController = this._movementControllers["Default"];
			this._gameInstance.EntityStoreModule.MountEntityLocalId = -1;
		}

		// Token: 0x06004B66 RID: 19302 RVA: 0x00136958 File Offset: 0x00134B58
		public void Update(float deltaTime)
		{
			this._timeElapsed += deltaTime;
			Input input = this._gameInstance.Input;
			InputBindings inputBindings = this._gameInstance.App.Settings.InputBindings;
			this.ForwardsTimestamp = (input.IsBindingHeld(inputBindings.MoveForwards, false) ? ((this.ForwardsTimestamp == 0f) ? this._timeElapsed : this.ForwardsTimestamp) : 0f);
			this.BackwardsTimestamp = (input.IsBindingHeld(inputBindings.MoveBackwards, false) ? ((this.BackwardsTimestamp == 0f) ? this._timeElapsed : this.BackwardsTimestamp) : 0f);
			this.LeftTimestamp = (input.IsBindingHeld(inputBindings.StrafeLeft, false) ? ((this.LeftTimestamp == 0f) ? this._timeElapsed : this.LeftTimestamp) : 0f);
			this.RightTimestamp = (input.IsBindingHeld(inputBindings.StrafeRight, false) ? ((this.RightTimestamp == 0f) ? this._timeElapsed : this.RightTimestamp) : 0f);
		}

		// Token: 0x06004B67 RID: 19303 RVA: 0x00136A76 File Offset: 0x00134C76
		public void PreUpdate(float timeFraction)
		{
			this.MovementController.PreUpdate(timeFraction);
		}

		// Token: 0x040026F1 RID: 9969
		private const string PlayerSpeedRTPCName = "SPEED";

		// Token: 0x040026F2 RID: 9970
		private const float RTPCSpeedTolerance = 0.01f;

		// Token: 0x040026F3 RID: 9971
		private const float NotPressed = 0f;

		// Token: 0x040026F4 RID: 9972
		private readonly Dictionary<string, MovementController> _movementControllers = new Dictionary<string, MovementController>();

		// Token: 0x040026F5 RID: 9973
		private uint _playerSpeedRTPCId;

		// Token: 0x040026F6 RID: 9974
		private float _previousSpeedRTPCValue;

		// Token: 0x040026F7 RID: 9975
		private float _timeElapsed;
	}
}
