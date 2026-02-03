using System;
using HytaleClient.Interface.UI.Elements;

namespace HytaleClient.Interface.InGame.Pages.InventoryPanels
{
	// Token: 0x0200089C RID: 2204
	internal abstract class Panel : InterfaceComponent
	{
		// Token: 0x06003FB8 RID: 16312 RVA: 0x000B4070 File Offset: 0x000B2270
		public Panel(InGameView inGameView, Element parent = null) : base(inGameView.Interface, parent)
		{
			this._inGameView = inGameView;
		}

		// Token: 0x04001E44 RID: 7748
		protected readonly InGameView _inGameView;
	}
}
