using System;
using HytaleClient.Interface.UI;
using HytaleClient.Interface.UI.Elements;

namespace HytaleClient.Interface.InGame.Pages.InventoryPanels.SelectionCommands
{
	// Token: 0x020008A4 RID: 2212
	internal class FillCommand : BaseMultipleMaterialsCommand
	{
		// Token: 0x06004019 RID: 16409 RVA: 0x000B76CF File Offset: 0x000B58CF
		public FillCommand(InGameView inGameView, Desktop desktop, Element parent = null) : base(inGameView, desktop, parent)
		{
		}

		// Token: 0x0600401A RID: 16410 RVA: 0x000B76DC File Offset: 0x000B58DC
		public override string GetChatCommand()
		{
			return "/fill " + base.GetChatCommand();
		}
	}
}
