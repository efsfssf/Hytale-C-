using System;
using HytaleClient.Data.Items;
using HytaleClient.Interface.UI;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Interface.UI.Styles;

namespace HytaleClient.Interface.InGame
{
	// Token: 0x02000888 RID: 2184
	[UIMarkupElement(AcceptsChildren = false)]
	internal class ItemIcon : Element
	{
		// Token: 0x06003E6E RID: 15982 RVA: 0x000A742C File Offset: 0x000A562C
		public ItemIcon(Desktop Desktop, Element parent) : base(Desktop, parent)
		{
			IUIProvider provider = Desktop.Provider;
			IUIProvider iuiprovider = provider;
			CustomUIProvider customUIProvider = iuiprovider as CustomUIProvider;
			if (customUIProvider == null)
			{
				Interface @interface = iuiprovider as Interface;
				if (@interface == null)
				{
					throw new Exception("IUIProvider must be of type CustomUIProvider or Interface");
				}
				this._inGameView = @interface.InGameView;
			}
			else
			{
				this._inGameView = customUIProvider.Interface.InGameView;
			}
		}

		// Token: 0x06003E6F RID: 15983 RVA: 0x000A7494 File Offset: 0x000A5694
		protected override void ApplyStyles()
		{
			ClientItemBase clientItemBase;
			bool flag = this.ItemId != null && this._inGameView.Items.TryGetValue(this.ItemId, out clientItemBase) && clientItemBase.Icon != null;
			if (flag)
			{
				this.Background = new PatchStyle(this._inGameView.GetTextureAreaForItemIcon(clientItemBase.Icon));
			}
			else
			{
				this.Background = null;
			}
			base.ApplyStyles();
		}

		// Token: 0x04001D59 RID: 7513
		private InGameView _inGameView;

		// Token: 0x04001D5A RID: 7514
		[UIMarkupProperty]
		public string ItemId;
	}
}
