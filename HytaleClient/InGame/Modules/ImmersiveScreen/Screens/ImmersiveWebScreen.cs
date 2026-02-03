using System;
using System.Diagnostics;
using HytaleClient.Application;
using HytaleClient.Core;
using HytaleClient.Graphics;
using HytaleClient.Graphics.Programs;
using HytaleClient.InGame.Modules.ImmersiveScreen.Data;
using HytaleClient.Math;
using HytaleClient.Protocol;

namespace HytaleClient.InGame.Modules.ImmersiveScreen.Screens
{
	// Token: 0x02000940 RID: 2368
	internal class ImmersiveWebScreen : BaseImmersiveScreen
	{
		// Token: 0x060048E8 RID: 18664 RVA: 0x0011B138 File Offset: 0x00119338
		public ImmersiveWebScreen(GameInstance gameInstance, Vector3 blockPosition, ImmersiveView.ViewScreen screen) : base(gameInstance, blockPosition, screen)
		{
		}

		// Token: 0x060048E9 RID: 18665 RVA: 0x0011B148 File Offset: 0x00119348
		public void SetViewData(ImmersiveView viewData)
		{
			Debug.Assert(viewData.Web.App == 0);
			this._mediaData = new ClientMediaData(viewData.Web.MediaData);
			bool flag = this._gameInstance.ImmersiveScreenModule.ActiveWebScreen == this;
			if (flag)
			{
				this.SendMediaData();
			}
		}

		// Token: 0x060048EA RID: 18666 RVA: 0x0011B19E File Offset: 0x0011939E
		protected override void DoDispose()
		{
		}

		// Token: 0x060048EB RID: 18667 RVA: 0x0011B1A1 File Offset: 0x001193A1
		public string GetUrl()
		{
			return "coui://interface/immersiveView.html";
		}

		// Token: 0x060048EC RID: 18668 RVA: 0x0011B1A8 File Offset: 0x001193A8
		public void SendMediaData()
		{
			bool isReady = this._gameInstance.ImmersiveScreenModule.CoUIWebView.IsReady;
			if (isReady)
			{
				this._gameInstance.ImmersiveScreenModule.CoUIWebView.TriggerEvent("immersiveScreens.mediaData.update", this._mediaData, null, null, null, null);
			}
			this._gameInstance.App.Interface.InGameView.MediaBrowserPage.OnImmersiveViewDataUpdated(this._mediaData);
		}

		// Token: 0x060048ED RID: 18669 RVA: 0x0011B21A File Offset: 0x0011941A
		public void OnActivate()
		{
			this.SendMediaData();
		}

		// Token: 0x060048EE RID: 18670 RVA: 0x0011B224 File Offset: 0x00119424
		public void OnDeactivate()
		{
			App app = this._gameInstance.App;
			bool flag = app.Stage == App.AppStage.InGame && app.InGame.CurrentPage == 3;
			if (flag)
			{
				app.InGame.SetCurrentPage(0, false, false);
			}
			this._gameInstance.App.Interface.InGameView.MediaBrowserPage.OnImmersiveViewDataUpdated(null);
		}

		// Token: 0x060048EF RID: 18671 RVA: 0x0011B28C File Offset: 0x0011948C
		public override void Draw()
		{
			bool flag = !base.NeedsDrawing();
			if (flag)
			{
				throw new Exception("Draw called when it was not required. Please check with NeedsDrawing() first before calling this.");
			}
			Debug.Assert(!this._gameInstance.ImmersiveScreenModule.CoUIWebView.Disposed);
			bool flag2 = this._gameInstance.Engine.Window.GetState() == Window.WindowState.Minimized;
			if (!flag2)
			{
				GLFunctions gl = this._gameInstance.Engine.Graphics.GL;
				BasicProgram basicProgram = this._gameInstance.Engine.Graphics.GPUProgramStore.BasicProgram;
				gl.BlendFunc(GL.ONE, GL.ONE_MINUS_SRC_ALPHA);
				gl.AssertEnabled(GL.BLEND);
				gl.AssertBlendFunc(GL.ONE, GL.ONE_MINUS_SRC_ALPHA);
				basicProgram.AssertInUse();
				basicProgram.Color.AssertValue(this._gameInstance.Engine.Graphics.WhiteColor);
				basicProgram.Opacity.AssertValue(1f);
				basicProgram.MVPMatrix.SetValue(ref this._mvpMatrix);
				gl.BindTexture(GL.TEXTURE_2D, this._gameInstance.ImmersiveScreenModule.CoUIWebViewTexture);
				this._gameInstance.ImmersiveScreenModule.CoUIWebView.RenderToTexture();
				this._gameInstance.ImmersiveScreenModule.CoUIQuadRenderer.Draw();
				gl.BlendFunc(GL.SRC_ALPHA, GL.ONE_MINUS_SRC_ALPHA);
			}
		}

		// Token: 0x040024EA RID: 9450
		private ClientMediaData _mediaData;
	}
}
