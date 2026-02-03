using System;

namespace HytaleClient.Interface.InGame
{
	// Token: 0x0200087D RID: 2173
	internal class CustomMarkupErrorOverlay : MarkupErrorOverlay
	{
		// Token: 0x06003D68 RID: 15720 RVA: 0x0009CF27 File Offset: 0x0009B127
		public CustomMarkupErrorOverlay(InGameView inGameView) : base(inGameView.Desktop, null, "Custom UI — Markup Error")
		{
			this._inGameView = inGameView;
		}

		// Token: 0x06003D69 RID: 15721 RVA: 0x0009CF44 File Offset: 0x0009B144
		protected internal override void Dismiss()
		{
			this._inGameView.Dismiss();
		}

		// Token: 0x04001C96 RID: 7318
		private readonly InGameView _inGameView;
	}
}
