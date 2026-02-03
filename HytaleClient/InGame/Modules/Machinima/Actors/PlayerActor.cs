using System;
using Coherent.UI.Binding;
using HytaleClient.Graphics.Gizmos;
using HytaleClient.Graphics.Gizmos.Models;

namespace HytaleClient.InGame.Modules.Machinima.Actors
{
	// Token: 0x02000929 RID: 2345
	[CoherentType]
	internal class PlayerActor : EntityActor
	{
		// Token: 0x0600478B RID: 18315 RVA: 0x0010E6D1 File Offset: 0x0010C8D1
		protected override ActorType GetActorType()
		{
			return ActorType.Player;
		}

		// Token: 0x0600478C RID: 18316 RVA: 0x0010E6D4 File Offset: 0x0010C8D4
		public PlayerActor(GameInstance gameInstance, string name) : base(gameInstance, name, gameInstance.LocalPlayer)
		{
			this._gameInstance = gameInstance;
			this._modelRenderer = new PrimitiveModelRenderer(gameInstance.Engine.Graphics, gameInstance.Engine.Graphics.GPUProgramStore.BasicProgram);
			this._modelRenderer.UpdateModelData(CameraModel.BuildModelData());
		}

		// Token: 0x040023EC RID: 9196
		private GameInstance _gameInstance;

		// Token: 0x040023ED RID: 9197
		private PrimitiveModelRenderer _modelRenderer;
	}
}
