using System;
using HytaleClient.InGame;
using HytaleClient.InGame.Modules.Interaction;
using HytaleClient.Protocol;

namespace HytaleClient.Data.ClientInteraction.None
{
	// Token: 0x02000B25 RID: 2853
	internal class BuilderToolInteraction : SimpleInteraction
	{
		// Token: 0x060058EF RID: 22767 RVA: 0x001B30DC File Offset: 0x001B12DC
		public BuilderToolInteraction(int id, Interaction interaction) : base(id, interaction)
		{
			interaction.AllowIndefiniteHold = true;
		}

		// Token: 0x060058F0 RID: 22768 RVA: 0x001B30EF File Offset: 0x001B12EF
		protected override void Tick0(GameInstance gameInstance, InteractionModule.ClickType clickType, bool hasAnyButtonClick, bool firstRun, float time, InteractionType type, InteractionContext context)
		{
			gameInstance.BuilderToolsModule.OnInteraction(type, clickType, context, firstRun);
			base.Tick0(gameInstance, clickType, hasAnyButtonClick, firstRun, time, type, context);
		}
	}
}
