using System;
using System.IO;
using HytaleClient.Core;
using HytaleClient.Data;
using HytaleClient.Graphics;
using HytaleClient.Math;
using HytaleClient.Utils;

namespace HytaleClient.Interface
{
	// Token: 0x02000806 RID: 2054
	internal class StartupView : InterfaceComponent
	{
		// Token: 0x06003903 RID: 14595 RVA: 0x00076C77 File Offset: 0x00074E77
		public StartupView(Interface @interface) : base(@interface, null)
		{
		}

		// Token: 0x06003904 RID: 14596 RVA: 0x00076C84 File Offset: 0x00074E84
		protected override void OnMounted()
		{
			Image image = new Image(File.ReadAllBytes(Path.Combine(Paths.GameData, "Splashscreen.png")));
			this._splashScreenTexture = new Texture(Texture.TextureTypes.Texture2D);
			this._splashScreenTexture.CreateTexture2D(image.Width, image.Height, image.Pixels, 5, GL.LINEAR, GL.LINEAR, GL.CLAMP_TO_EDGE, GL.CLAMP_TO_EDGE, GL.RGBA, GL.RGBA, GL.UNSIGNED_BYTE, false);
			this.Desktop.RegisterAnimationCallback(new Action<float>(this.Animate));
		}

		// Token: 0x06003905 RID: 14597 RVA: 0x00076D13 File Offset: 0x00074F13
		protected override void OnUnmounted()
		{
			this._splashScreenTexture.Dispose();
			this._splashScreenTexture = null;
			this.Desktop.UnregisterAnimationCallback(new Action<float>(this.Animate));
		}

		// Token: 0x06003906 RID: 14598 RVA: 0x00076D41 File Offset: 0x00074F41
		private void Animate(float deltaTime)
		{
			this._lerpLoadingProgress = MathHelper.Lerp(this._lerpLoadingProgress, AssetManager.BuiltInAssetsMetadataLoadProgress, MathHelper.Min(deltaTime * 10f, 1f));
		}

		// Token: 0x06003907 RID: 14599 RVA: 0x00076D6C File Offset: 0x00074F6C
		protected override void PrepareForDrawSelf()
		{
			Engine engine = this.Interface.Engine;
			Rectangle viewport = engine.Window.Viewport;
			bool flag = engine.Window.AspectRatio > (double)((float)this._splashScreenTexture.Width / (float)this._splashScreenTexture.Height);
			if (flag)
			{
				viewport.Width = this._splashScreenTexture.Width * engine.Window.Viewport.Height / this._splashScreenTexture.Height;
				viewport.X = engine.Window.Viewport.Center.X - viewport.Width / 2;
			}
			else
			{
				viewport.Height = this._splashScreenTexture.Height * engine.Window.Viewport.Width / this._splashScreenTexture.Width;
				viewport.Y = engine.Window.Viewport.Center.Y - viewport.Height / 2;
			}
			this.Desktop.Batcher2D.RequestDrawTexture(this._splashScreenTexture, new Rectangle(0, 0, this._splashScreenTexture.Width, this._splashScreenTexture.Height), viewport, UInt32Color.White);
			int width = (int)((float)viewport.Width * this._lerpLoadingProgress);
			int num = viewport.Height / 144;
			this.Desktop.Batcher2D.RequestDrawTexture(this.Desktop.Graphics.WhitePixelTexture, new Rectangle(0, 0, 1, 1), new Rectangle(viewport.X, viewport.Bottom - num, width, num), UInt32Color.FromRGBA(2915031551U));
		}

		// Token: 0x040018BF RID: 6335
		private Texture _splashScreenTexture;

		// Token: 0x040018C0 RID: 6336
		private float _lerpLoadingProgress;
	}
}
