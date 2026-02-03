using System;
using HytaleClient.Interface.UI;
using HytaleClient.Interface.UI.Elements;

namespace HytaleClient.Interface.InGame.Pages.InventoryPanels.SelectionCommands
{
	// Token: 0x020008A3 RID: 2211
	internal abstract class BaseSelectionCommand : Element
	{
		// Token: 0x06004016 RID: 16406 RVA: 0x000B76BC File Offset: 0x000B58BC
		public BaseSelectionCommand(InGameView inGameView, Desktop desktop, Element parent = null) : base(desktop, parent)
		{
			this._inGameView = inGameView;
		}

		// Token: 0x06004017 RID: 16407
		public abstract string GetChatCommand();

		// Token: 0x06004018 RID: 16408
		public abstract void Build();

		// Token: 0x04001E7D RID: 7805
		protected InGameView _inGameView;
	}
}
