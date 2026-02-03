using System;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;
using SDL2;

namespace HytaleClient.Interface.MainMenu.Pages.MyAvatar
{
	// Token: 0x02000824 RID: 2084
	internal class SkinJsonPopup : InterfaceComponent
	{
		// Token: 0x06003A1F RID: 14879 RVA: 0x00083169 File Offset: 0x00081369
		public SkinJsonPopup(MyAvatarPage page) : base(page.Interface, null)
		{
		}

		// Token: 0x06003A20 RID: 14880 RVA: 0x0008317C File Offset: 0x0008137C
		public void Build()
		{
			base.Clear();
			Document document;
			this.Desktop.Provider.TryGetDocument("MainMenu/MyAvatar/JsonPopup.ui", out document);
			UIFragment uifragment = document.Instantiate(this.Desktop, this);
			this._jsonLabel = uifragment.Get<Label>("Json");
			uifragment.Get<TextButton>("CopyButton").Activating = delegate()
			{
				SDL.SDL_SetClipboardText(this._json ?? "");
			};
			uifragment.Get<TextButton>("CloseButton").Activating = new Action(this.Dismiss);
		}

		// Token: 0x06003A21 RID: 14881 RVA: 0x00083204 File Offset: 0x00081404
		protected override void OnMounted()
		{
			this._json = this.Interface.App.MainMenu.GetSkinJson().ToString();
			this._jsonLabel.Text = this._json;
			this._jsonLabel.Parent.Layout(null, true);
		}

		// Token: 0x06003A22 RID: 14882 RVA: 0x0008325F File Offset: 0x0008145F
		protected override void OnUnmounted()
		{
			this._json = null;
		}

		// Token: 0x06003A23 RID: 14883 RVA: 0x00083269 File Offset: 0x00081469
		protected internal override void Dismiss()
		{
			this.Desktop.ClearLayer(2);
		}

		// Token: 0x04001A1D RID: 6685
		private Label _jsonLabel;

		// Token: 0x04001A1E RID: 6686
		private string _json;
	}
}
