using System;
using HytaleClient.Interface.UI;
using HytaleClient.Interface.UI.Elements;

namespace HytaleClient.Interface.InGame.Pages.InventoryPanels.SelectionCommands
{
	// Token: 0x020008A9 RID: 2217
	internal class SetCommand : BaseMultipleMaterialsCommand
	{
		// Token: 0x0600403B RID: 16443 RVA: 0x000B84F2 File Offset: 0x000B66F2
		public SetCommand(InGameView inGameView, Desktop desktop, Element parent = null) : base(inGameView, desktop, parent)
		{
		}

		// Token: 0x0600403C RID: 16444 RVA: 0x000B8500 File Offset: 0x000B6700
		public override string GetChatCommand()
		{
			return "/set " + base.GetChatCommand();
		}
	}
}
