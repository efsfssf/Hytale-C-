using System;
using System.Diagnostics;
using HytaleClient.Application;
using HytaleClient.Core;
using HytaleClient.Graphics;
using HytaleClient.Graphics.Programs;
using HytaleClient.Interface.CoherentUI;
using HytaleClient.Math;
using HytaleClient.Utils;
using NLog;

namespace HytaleClient.InGame.Modules
{
	// Token: 0x020008F1 RID: 2289
	internal class EditorWebViewModule : Module
	{
		// Token: 0x060043FA RID: 17402 RVA: 0x000E3660 File Offset: 0x000E1860
		public EditorWebViewModule(GameInstance gameInstance) : base(gameInstance)
		{
			this._engine = this._gameInstance.Engine;
			this._quadRenderer = new QuadRenderer(this._engine.Graphics, this._engine.Graphics.GPUProgramStore.BasicProgram.AttribPosition, this._engine.Graphics.GPUProgramStore.BasicProgram.AttribTexCoords);
			this.WebView = new WebView(this._gameInstance.Engine, this._gameInstance.App.CoUIManager, "coui://interface/index.html", this._engine.Window.Viewport.Width, this._engine.Window.Viewport.Height, this._engine.Window.ViewportScale);
			GLFunctions gl = this._engine.Graphics.GL;
			this.Texture = gl.GenTexture();
			gl.BindTexture(GL.TEXTURE_2D, this.Texture);
			gl.TexParameteri(GL.TEXTURE_2D, GL.TEXTURE_MIN_FILTER, 9728);
			gl.TexParameteri(GL.TEXTURE_2D, GL.TEXTURE_MAG_FILTER, 9728);
			gl.TexParameteri(GL.TEXTURE_2D, GL.TEXTURE_WRAP_S, 33071);
			gl.TexParameteri(GL.TEXTURE_2D, GL.TEXTURE_WRAP_T, 33071);
			this.WebView.TriggerEvent("i18n.setClientMessages", Language.LoadLanguage(this._gameInstance.App.Settings.Language), null, null, null, null);
			this.WebView.RegisterForEvent("closeInGameOverlay", this._gameInstance, delegate()
			{
				this._gameInstance.App.InGame.SetCurrentOverlay(AppInGame.InGameOverlay.None);
			});
			this.WebView.RegisterForEvent("reload", this._engine, new Action(this.OnReload));
		}

		// Token: 0x060043FB RID: 17403 RVA: 0x000E3848 File Offset: 0x000E1A48
		protected override void DoDispose()
		{
			this.WebView.UnregisterFromEvent("closeInGameOverlay");
			this.WebView.UnregisterFromEvent("reload");
			this._gameInstance.App.CoUIManager.RunInThread(delegate
			{
				this.WebView.Destroy();
				this._engine.RunOnMainThread(this._engine, delegate
				{
					this.WebView.Dispose();
				}, false, false);
			});
			this._quadRenderer.Dispose();
			GLFunctions gl = this._engine.Graphics.GL;
			gl.DeleteTexture(this.Texture);
		}

		// Token: 0x060043FC RID: 17404 RVA: 0x000E38C4 File Offset: 0x000E1AC4
		private void OnReload()
		{
			Debug.Assert(ThreadHelper.IsMainThread());
			bool flag = this._currentWebView == EditorWebViewModule.WebViewType.None;
			if (!flag)
			{
				this.WebView.Reload();
				this.WebView.TriggerEvent("i18n.setClientMessages", Language.LoadLanguage(this._gameInstance.App.Settings.Language), null, null, null, null);
			}
		}

		// Token: 0x060043FD RID: 17405 RVA: 0x000E3928 File Offset: 0x000E1B28
		public void OnWindowSizeChanged()
		{
			this.WebView.Resize(this._engine.Window.Viewport.Width, this._engine.Window.Viewport.Height, this._engine.Window.ViewportScale);
		}

		// Token: 0x060043FE RID: 17406 RVA: 0x000E397C File Offset: 0x000E1B7C
		public void SetCurrentWebView(EditorWebViewModule.WebViewType webViewType)
		{
			this._currentWebView = webViewType;
			this.WebView.TriggerEvent("setInGameOverlay", this._currentWebView, null, null, null, null);
		}

		// Token: 0x060043FF RID: 17407 RVA: 0x000E39A6 File Offset: 0x000E1BA6
		public bool NeedsDrawing()
		{
			return this._currentWebView > EditorWebViewModule.WebViewType.None;
		}

		// Token: 0x06004400 RID: 17408 RVA: 0x000E39B4 File Offset: 0x000E1BB4
		public void Draw()
		{
			bool flag = !this.NeedsDrawing();
			if (flag)
			{
				throw new Exception("Draw called when it was not required. Please check with NeedsDrawing() first before calling this.");
			}
			GLFunctions gl = this._engine.Graphics.GL;
			BasicProgram basicProgram = this._engine.Graphics.GPUProgramStore.BasicProgram;
			gl.Disable(GL.DEPTH_TEST);
			gl.BlendFunc(GL.ONE, GL.ONE_MINUS_SRC_ALPHA);
			gl.AssertEnabled(GL.BLEND);
			gl.AssertBlendFunc(GL.ONE, GL.ONE_MINUS_SRC_ALPHA);
			basicProgram.AssertInUse();
			basicProgram.Color.SetValue(this._engine.Graphics.WhiteColor);
			basicProgram.Opacity.SetValue(1f);
			basicProgram.MVPMatrix.SetValue(ref EditorWebViewModule.Matrix);
			gl.BindTexture(GL.TEXTURE_2D, this.Texture);
			this.WebView.RenderToTexture();
			this._quadRenderer.Draw();
			gl.BlendFunc(GL.SRC_ALPHA, GL.ONE_MINUS_SRC_ALPHA);
			gl.Enable(GL.DEPTH_TEST);
		}

		// Token: 0x04002182 RID: 8578
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x04002183 RID: 8579
		public readonly WebView WebView;

		// Token: 0x04002184 RID: 8580
		public readonly GLTexture Texture;

		// Token: 0x04002185 RID: 8581
		private readonly QuadRenderer _quadRenderer;

		// Token: 0x04002186 RID: 8582
		private EditorWebViewModule.WebViewType _currentWebView;

		// Token: 0x04002187 RID: 8583
		private readonly Engine _engine;

		// Token: 0x04002188 RID: 8584
		private static Matrix Matrix = Matrix.CreateTranslation(0f, 0f, -1f) * Matrix.CreateOrthographicOffCenter(0f, 1f, 0f, 1f, 0.1f, 1000f);

		// Token: 0x02000DBF RID: 3519
		public enum WebViewType
		{
			// Token: 0x040043B3 RID: 17331
			None,
			// Token: 0x040043B4 RID: 17332
			MachinimaEditor
		}
	}
}
