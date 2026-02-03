using System;
using HytaleClient.Application;
using HytaleClient.InGame.Modules;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Math;

namespace HytaleClient.Interface.InGame.Overlays
{
	// Token: 0x020008AD RID: 2221
	internal class MachinimaEditorOverlay : InterfaceComponent
	{
		// Token: 0x06004050 RID: 16464 RVA: 0x000B8B12 File Offset: 0x000B6D12
		public MachinimaEditorOverlay(InGameView inGameView) : base(inGameView.Interface, null)
		{
			this._inGameView = inGameView;
		}

		// Token: 0x06004051 RID: 16465 RVA: 0x000B8B2A File Offset: 0x000B6D2A
		public void Build()
		{
			base.Clear();
		}

		// Token: 0x06004052 RID: 16466 RVA: 0x000B8B34 File Offset: 0x000B6D34
		protected override void OnMounted()
		{
			this._inGameView.InGame.Instance.EditorWebViewModule.SetCurrentWebView(EditorWebViewModule.WebViewType.MachinimaEditor);
		}

		// Token: 0x06004053 RID: 16467 RVA: 0x000B8B54 File Offset: 0x000B6D54
		protected override void OnUnmounted()
		{
			bool flag = this.Interface.App.Stage == App.AppStage.InGame;
			if (flag)
			{
				this._inGameView.InGame.Instance.EditorWebViewModule.SetCurrentWebView(EditorWebViewModule.WebViewType.None);
			}
		}

		// Token: 0x06004054 RID: 16468 RVA: 0x000B8B97 File Offset: 0x000B6D97
		public override Element HitTest(Point position)
		{
			return this;
		}

		// Token: 0x04001EA8 RID: 7848
		private readonly InGameView _inGameView;
	}
}
