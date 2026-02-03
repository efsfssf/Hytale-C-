using System;
using HytaleClient.InGame.Modules.Interaction;
using HytaleClient.InGame.Modules.Machinima;
using HytaleClient.Protocol;

namespace HytaleClient.InGame.Modules.BuilderTools.Tools.Client
{
	// Token: 0x0200098A RID: 2442
	internal class MachinimaTool : ClientTool
	{
		// Token: 0x1700127A RID: 4730
		// (get) Token: 0x06004D82 RID: 19842 RVA: 0x0014E49A File Offset: 0x0014C69A
		public override string ToolId
		{
			get
			{
				return "Machinima";
			}
		}

		// Token: 0x06004D83 RID: 19843 RVA: 0x0014E4A1 File Offset: 0x0014C6A1
		public MachinimaTool(GameInstance gameInstance) : base(gameInstance)
		{
			this._machinima = this._gameInstance.MachinimaModule;
		}

		// Token: 0x06004D84 RID: 19844 RVA: 0x0014E4C0 File Offset: 0x0014C6C0
		public override void OnInteraction(InteractionType interactionType, InteractionModule.ClickType clickType, InteractionContext context, bool firstRun)
		{
			bool flag = clickType == InteractionModule.ClickType.None;
			if (!flag)
			{
				this._machinima.OnInteraction(interactionType);
			}
		}

		// Token: 0x040028C7 RID: 10439
		private readonly MachinimaModule _machinima;
	}
}
