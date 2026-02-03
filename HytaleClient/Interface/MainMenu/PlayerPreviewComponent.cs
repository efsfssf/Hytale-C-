using System;
using HytaleClient.Application;
using HytaleClient.Interface.UI;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Math;

namespace HytaleClient.Interface.MainMenu
{
	// Token: 0x0200081A RID: 2074
	[UIMarkupElement]
	internal class PlayerPreviewComponent : Element
	{
		// Token: 0x06003981 RID: 14721 RVA: 0x0007C510 File Offset: 0x0007A710
		public PlayerPreviewComponent(Desktop desktop, Element parent) : base(desktop, parent)
		{
			this._interface = (Interface)this.Desktop.Provider;
		}

		// Token: 0x06003982 RID: 14722 RVA: 0x0007C548 File Offset: 0x0007A748
		protected override void OnUnmounted()
		{
			this._interface.App.MainMenu.RemoveCharacterFromScreen(this._id.ToString());
		}

		// Token: 0x06003983 RID: 14723 RVA: 0x0007C57C File Offset: 0x0007A77C
		protected override void LayoutSelf()
		{
			Rectangle anchoredRectangle = base.AnchoredRectangle;
			anchoredRectangle.Offset(this.Desktop.ViewportRectangle.Location);
			this._interface.App.MainMenu.AddCharacterOnScreen(new AppMainMenu.AddCharacterOnScreenEvent
			{
				Id = this._id.ToString(),
				InitialModelAngle = -0.3926991f,
				Scale = 1f,
				Viewport = anchoredRectangle
			});
		}

		// Token: 0x04001968 RID: 6504
		private Interface _interface;

		// Token: 0x04001969 RID: 6505
		private static int NextPlayerPreviewId;

		// Token: 0x0400196A RID: 6506
		private readonly int _id = ++PlayerPreviewComponent.NextPlayerPreviewId;
	}
}
