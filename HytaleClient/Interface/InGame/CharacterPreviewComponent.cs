using System;
using HytaleClient.InGame.Modules.InterfaceRenderPreview;
using HytaleClient.Interface.UI;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Math;

namespace HytaleClient.Interface.InGame
{
	// Token: 0x0200087C RID: 2172
	[UIMarkupElement(AcceptsChildren = true)]
	internal class CharacterPreviewComponent : Element
	{
		// Token: 0x06003D64 RID: 15716 RVA: 0x0009CDF0 File Offset: 0x0009AFF0
		public CharacterPreviewComponent(Desktop desktop, Element parent) : base(desktop, parent)
		{
			this._interface = (Interface)this.Desktop.Provider;
			InGameView inGameView = this._interface.InGameView;
			int nextPreviewId = inGameView.NextPreviewId;
			inGameView.NextPreviewId = nextPreviewId + 1;
			this._id = nextPreviewId;
		}

		// Token: 0x06003D65 RID: 15717 RVA: 0x0009CE3E File Offset: 0x0009B03E
		protected override void OnUnmounted()
		{
			this._interface.App.InGame.Instance.InterfaceRenderPreviewModule.RemovePreview(this._id);
		}

		// Token: 0x06003D66 RID: 15718 RVA: 0x0009CE67 File Offset: 0x0009B067
		protected override void LayoutSelf()
		{
			this.Update();
		}

		// Token: 0x06003D67 RID: 15719 RVA: 0x0009CE74 File Offset: 0x0009B074
		private void Update()
		{
			Rectangle anchoredRectangle = base.AnchoredRectangle;
			anchoredRectangle.Offset(this.Desktop.ViewportRectangle.Location);
			InterfaceRenderPreviewModule interfaceRenderPreviewModule = this._interface.App.InGame.Instance.InterfaceRenderPreviewModule;
			InterfaceRenderPreviewModule.PreviewParams previewParams = new InterfaceRenderPreviewModule.PreviewParams();
			previewParams.Id = this._id;
			previewParams.Rotatable = false;
			previewParams.Translation = new float[]
			{
				0f,
				-70f,
				-200f
			};
			InterfaceRenderPreviewModule.PreviewParams previewParams2 = previewParams;
			float[] array = new float[3];
			array[1] = 15f;
			previewParams2.Rotation = array;
			previewParams.Scale = 0.05f;
			previewParams.Ortho = false;
			previewParams.Viewport = anchoredRectangle;
			interfaceRenderPreviewModule.AddCharacterPreview(previewParams);
		}

		// Token: 0x04001C94 RID: 7316
		private Interface _interface;

		// Token: 0x04001C95 RID: 7317
		private int _id;
	}
}
