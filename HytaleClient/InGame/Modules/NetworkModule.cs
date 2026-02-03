using System;
using System.Diagnostics;
using HytaleClient.Data.Entities;
using HytaleClient.Data.Entities.Initializers;
using HytaleClient.InGame.Modules.Entities;
using HytaleClient.Math;
using HytaleClient.Protocol;

namespace HytaleClient.InGame.Modules
{
	// Token: 0x020008F9 RID: 2297
	internal class NetworkModule : Module
	{
		// Token: 0x0600445E RID: 17502 RVA: 0x000E7212 File Offset: 0x000E5412
		public NetworkModule(GameInstance gameInstance) : base(gameInstance)
		{
		}

		// Token: 0x0600445F RID: 17503 RVA: 0x000E7228 File Offset: 0x000E5428
		[Obsolete]
		public override void Tick()
		{
			PlayerEntity localPlayer = this._gameInstance.LocalPlayer;
			ref ClientMovementStates ptr = ref this._gameInstance.CharacterControllerModule.MovementController.MovementStates;
			bool flag = localPlayer.Position != this._lastSentPosition;
			bool flag2 = localPlayer.BodyOrientation != this._lastSentBodyOrientation;
			bool flag3 = localPlayer.LookOrientation != this._lastSentLookOrientation;
			bool flag4 = ptr != this._lastSentMovementStates;
			bool flag5 = !flag && !flag2 && !flag3 && !flag4;
			if (!flag5)
			{
				ClientMovement clientMovement = new ClientMovement();
				bool flag6 = flag;
				if (flag6)
				{
					bool flag7 = false;
					bool flag8 = flag7 && this._lastAbsolutePosition.ElapsedMilliseconds < 2000L;
					if (flag8)
					{
						Vector3 vector = localPlayer.Position - this._lastSentPosition;
						clientMovement.RelativePosition = new HalfFloatPosition((short)(vector.X * 10000f), (short)(vector.Y * 10000f), (short)(vector.Z * 10000f));
					}
					else
					{
						clientMovement.AbsolutePosition = localPlayer.Position.ToPositionPacket();
						this._lastAbsolutePosition.Restart();
					}
				}
				bool flag9 = flag2;
				if (flag9)
				{
					clientMovement.BodyOrientation = localPlayer.BodyOrientation.ToDirectionPacket();
				}
				bool flag10 = flag3;
				if (flag10)
				{
					clientMovement.LookOrientation = localPlayer.LookOrientation.ToDirectionPacket();
				}
				bool flag11 = flag4;
				if (flag11)
				{
					clientMovement.MovementStates_ = ClientMovementStatesProtocolHelper.ToPacket(ref ptr);
				}
				bool flag12 = this._gameInstance.CharacterControllerModule.MovementController.RunningKnockbackRemainingTime > 0f;
				if (flag12)
				{
					clientMovement.WishMovement = this._gameInstance.CharacterControllerModule.MovementController.LastMoveForce.ToPositionPacket();
				}
				clientMovement.Velocity = new Vector3d((double)this._gameInstance.CharacterControllerModule.MovementController.PreviousMovementOffset.X, (double)this._gameInstance.CharacterControllerModule.MovementController.PreviousMovementOffset.Y, (double)this._gameInstance.CharacterControllerModule.MovementController.PreviousMovementOffset.Z);
				this._gameInstance.Connection.SendPacket(clientMovement);
				this._lastSentPosition = localPlayer.Position;
				this._lastSentBodyOrientation = localPlayer.BodyOrientation;
				this._lastSentLookOrientation = localPlayer.LookOrientation;
				this._lastSentMovementStates = ptr;
			}
		}

		// Token: 0x040021C1 RID: 8641
		private const float MaxRelativePositionDelta = 3.2767f;

		// Token: 0x040021C2 RID: 8642
		public const float RelativePositionScale = 10000f;

		// Token: 0x040021C3 RID: 8643
		private const long OccasionalAbsolutePositionDelay = 2000L;

		// Token: 0x040021C4 RID: 8644
		private readonly Stopwatch _lastAbsolutePosition = Stopwatch.StartNew();

		// Token: 0x040021C5 RID: 8645
		private Vector3 _lastSentPosition;

		// Token: 0x040021C6 RID: 8646
		private Vector3 _lastSentBodyOrientation;

		// Token: 0x040021C7 RID: 8647
		private Vector3 _lastSentLookOrientation;

		// Token: 0x040021C8 RID: 8648
		private ClientMovementStates _lastSentMovementStates;
	}
}
