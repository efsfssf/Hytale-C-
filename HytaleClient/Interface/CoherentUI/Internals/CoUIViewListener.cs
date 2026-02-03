using System;
using Coherent.UI;

namespace HytaleClient.Interface.CoherentUI.Internals
{
	// Token: 0x020008E1 RID: 2273
	internal class CoUIViewListener : ViewListener
	{
		// Token: 0x06004257 RID: 16983 RVA: 0x000C8A1C File Offset: 0x000C6C1C
		public CoUIViewListener(WebView webView, CoUIManager manager)
		{
			this._webView = webView;
			this._manager = manager;
			base.ViewCreated += new ViewListener.CoherentUI_OnViewCreated(this._webView.OnCoherentViewCreated);
			base.ReadyForBindings += new ViewListener.CoherentUI_OnReadyForBindings(this._webView.OnCoherentReadyForBindings);
			base.Draw += new ViewListener.CoherentUI_OnDraw(this._webView.OnCoherentDraw);
			base.CursorChanged += new ViewListener.CoherentUI_OnCursorChanged(this._webView.OnCursorChanged);
			base.NavigateTo += new ViewListener.CoherentUI_OnNavigateTo(this._webView.OnNavigateTo);
			base.ScriptMessage += new ViewListener.CoherentUI_OnScriptMessage(this._webView.OnCoherentScriptMessage);
			base.Error += new ViewListener.CoherentUI_OnError(this._webView.OnCoherentError);
			base.FailLoad += new ViewListener.CoherentUI_OnFailLoad(this._webView.OnCoherentFailLoad);
			base.FinishLoad += new ViewListener.CoherentUI_OnFinishLoad(this._webView.OnCoherentFinishLoad);
		}

		// Token: 0x06004258 RID: 16984 RVA: 0x000C8B17 File Offset: 0x000C6D17
		public override void CreateSurface(bool sharedMemory, uint width, uint height, SurfaceResponse response)
		{
			response.Signal(this._manager.TextureBufferHelper.CreateSharedMemory(width, height));
		}

		// Token: 0x06004259 RID: 16985 RVA: 0x000C8B34 File Offset: 0x000C6D34
		public override void DestroySurface(CoherentHandle surface, bool usesSharedMemory)
		{
			this._manager.TextureBufferHelper.DestroySharedMemory(surface);
		}

		// Token: 0x0400206E RID: 8302
		private readonly CoUIManager _manager;

		// Token: 0x0400206F RID: 8303
		private readonly WebView _webView;
	}
}
