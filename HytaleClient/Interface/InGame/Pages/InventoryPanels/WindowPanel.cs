using System;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Networking;

namespace HytaleClient.Interface.InGame.Pages.InventoryPanels
{
	// Token: 0x020008A1 RID: 2209
	internal abstract class WindowPanel : Panel
	{
		// Token: 0x06004007 RID: 16391 RVA: 0x000B7321 File Offset: 0x000B5521
		public WindowPanel(InGameView inGameView, Element parent = null) : base(inGameView, parent)
		{
		}

		// Token: 0x06004008 RID: 16392 RVA: 0x000B732D File Offset: 0x000B552D
		public void SetupWindow(PacketHandler.InventoryWindow inventoryWindow)
		{
			this._inventoryWindow = inventoryWindow;
			this.Setup();
		}

		// Token: 0x06004009 RID: 16393 RVA: 0x000B733E File Offset: 0x000B553E
		public void UpdateWindow(PacketHandler.InventoryWindow inventoryWindow)
		{
			this._inventoryWindow = inventoryWindow;
			this.Update();
		}

		// Token: 0x0600400A RID: 16394 RVA: 0x000B734F File Offset: 0x000B554F
		public void RefreshWindow()
		{
			this.Setup();
		}

		// Token: 0x0600400B RID: 16395 RVA: 0x000B7359 File Offset: 0x000B5559
		protected virtual void Setup()
		{
		}

		// Token: 0x0600400C RID: 16396 RVA: 0x000B735C File Offset: 0x000B555C
		protected virtual void Update()
		{
		}

		// Token: 0x04001E79 RID: 7801
		protected PacketHandler.InventoryWindow _inventoryWindow;
	}
}
