using System;
using HytaleClient.Data.Items;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;
using Newtonsoft.Json.Linq;

namespace HytaleClient.Interface.InGame.Pages.InventoryPanels
{
	// Token: 0x02000893 RID: 2195
	internal class BlockInfoPanel : Panel
	{
		// Token: 0x170010AF RID: 4271
		// (get) Token: 0x06003F06 RID: 16134 RVA: 0x000ADB5C File Offset: 0x000ABD5C
		// (set) Token: 0x06003F07 RID: 16135 RVA: 0x000ADB64 File Offset: 0x000ABD64
		public Group Panel { get; private set; }

		// Token: 0x06003F08 RID: 16136 RVA: 0x000ADB6D File Offset: 0x000ABD6D
		public BlockInfoPanel(InGameView inGameView, Element parent = null) : base(inGameView, parent)
		{
		}

		// Token: 0x06003F09 RID: 16137 RVA: 0x000ADB7C File Offset: 0x000ABD7C
		public void Build()
		{
			base.Clear();
			Document document;
			this.Interface.TryGetDocument("InGame/Pages/Inventory/BlockInfoPanel.ui", out document);
			UIFragment uifragment = document.Instantiate(this.Desktop, this);
			this.Panel = uifragment.Get<Group>("Panel");
			this._itemPreview = uifragment.Get<ItemPreviewComponent>("ItemPreview");
			this._nameLabel = uifragment.Get<Label>("NameLabel");
			this._infoButton = uifragment.Get<Button>("InfoButton");
		}

		// Token: 0x06003F0A RID: 16138 RVA: 0x000ADBF8 File Offset: 0x000ABDF8
		public void Update()
		{
			ClientItemBase clientItemBase = null;
			bool flag = this._inGameView.InventoryWindow != null && base.Visible;
			if (flag)
			{
				JToken jtoken = this._inGameView.InventoryWindow.WindowData["blockItemId"];
				string text = (jtoken != null) ? jtoken.ToObject<string>() : null;
				bool flag2 = text != null;
				if (flag2)
				{
					clientItemBase = this._inGameView.Items[text];
				}
			}
			this._blockItemId = ((clientItemBase != null) ? clientItemBase.Id : null);
			this.UpdatePreview();
			bool flag3 = clientItemBase != null;
			if (flag3)
			{
				this._nameLabel.Text = this.Desktop.Provider.GetText("items." + clientItemBase.Id + ".name", null, true);
				string text2 = this.Desktop.Provider.GetText("items." + clientItemBase.Id + ".description", null, false);
				bool flag4 = text2 != null;
				if (flag4)
				{
					this._infoButton.TooltipText = text2;
					this._infoButton.Visible = true;
				}
				else
				{
					this._infoButton.Visible = false;
				}
			}
			else
			{
				this._nameLabel.Text = "";
				this._infoButton.Visible = false;
			}
		}

		// Token: 0x06003F0B RID: 16139 RVA: 0x000ADD44 File Offset: 0x000ABF44
		public void UpdatePreview()
		{
			bool flag = this._blockItemId != null && this.Desktop.GetLayer(2) == null;
			if (flag)
			{
				this._itemPreview.SetItemId(this._blockItemId);
			}
			else
			{
				this._itemPreview.SetItemId(null);
			}
		}

		// Token: 0x04001DCC RID: 7628
		private ItemPreviewComponent _itemPreview;

		// Token: 0x04001DCD RID: 7629
		private Label _nameLabel;

		// Token: 0x04001DCE RID: 7630
		private Button _infoButton;

		// Token: 0x04001DCF RID: 7631
		private string _blockItemId;
	}
}
