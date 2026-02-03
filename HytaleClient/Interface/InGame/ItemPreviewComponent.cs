using System;
using HytaleClient.Data.Items;
using HytaleClient.InGame;
using HytaleClient.InGame.Modules.InterfaceRenderPreview;
using HytaleClient.Interface.UI;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Math;

namespace HytaleClient.Interface.InGame
{
	// Token: 0x02000889 RID: 2185
	[UIMarkupElement(AcceptsChildren = true)]
	internal class ItemPreviewComponent : Element
	{
		// Token: 0x06003E70 RID: 15984 RVA: 0x000A7504 File Offset: 0x000A5704
		public ItemPreviewComponent(Desktop Desktop, Element parent) : base(Desktop, parent)
		{
			this._interface = (Interface)Desktop.Provider;
			InGameView inGameView = this._interface.InGameView;
			int nextPreviewId = inGameView.NextPreviewId;
			inGameView.NextPreviewId = nextPreviewId + 1;
			this._id = nextPreviewId;
		}

		// Token: 0x06003E71 RID: 15985 RVA: 0x000A754D File Offset: 0x000A574D
		protected override void OnUnmounted()
		{
			GameInstance instance = this._interface.App.InGame.Instance;
			if (instance != null)
			{
				instance.InterfaceRenderPreviewModule.RemovePreview(this._id);
			}
		}

		// Token: 0x06003E72 RID: 15986 RVA: 0x000A757C File Offset: 0x000A577C
		protected override void LayoutSelf()
		{
			bool flag = this._itemId != null;
			if (flag)
			{
				this.Update();
			}
		}

		// Token: 0x06003E73 RID: 15987 RVA: 0x000A75A0 File Offset: 0x000A57A0
		public void SetItemId(string itemId)
		{
			this._itemId = itemId;
			bool isMounted = base.IsMounted;
			if (isMounted)
			{
				this.Update();
			}
		}

		// Token: 0x06003E74 RID: 15988 RVA: 0x000A75C8 File Offset: 0x000A57C8
		private void Update()
		{
			bool flag = this._itemId != null;
			if (flag)
			{
				ClientItemBase item = this._interface.InGameView.Items[this._itemId];
				ClientItemIconProperties iconProperties = IconHelper.GetIconProperties(item);
				Rectangle anchoredRectangle = base.AnchoredRectangle;
				anchoredRectangle.Offset(this.Desktop.ViewportRectangle.Location);
				InterfaceRenderPreviewModule interfaceRenderPreviewModule = this._interface.App.InGame.Instance.InterfaceRenderPreviewModule;
				InterfaceRenderPreviewModule.ItemPreviewParams itemPreviewParams = new InterfaceRenderPreviewModule.ItemPreviewParams();
				itemPreviewParams.Id = this._id;
				itemPreviewParams.ItemId = this._itemId;
				itemPreviewParams.Rotatable = false;
				InterfaceRenderPreviewModule.PreviewParams previewParams = itemPreviewParams;
				float[] array = new float[3];
				array[0] = iconProperties.Translation.Value.X;
				array[1] = iconProperties.Translation.Value.Y;
				previewParams.Translation = array;
				itemPreviewParams.Rotation = new float[]
				{
					iconProperties.Rotation.Value.X,
					iconProperties.Rotation.Value.Y,
					iconProperties.Rotation.Value.Z
				};
				itemPreviewParams.Scale = iconProperties.Scale * 0.8f;
				itemPreviewParams.Ortho = true;
				itemPreviewParams.Viewport = anchoredRectangle;
				interfaceRenderPreviewModule.AddItemPreview(itemPreviewParams);
			}
			else
			{
				this._interface.App.InGame.Instance.InterfaceRenderPreviewModule.RemovePreview(this._id);
			}
		}

		// Token: 0x04001D5B RID: 7515
		private Interface _interface;

		// Token: 0x04001D5C RID: 7516
		private int _id;

		// Token: 0x04001D5D RID: 7517
		private string _itemId;
	}
}
