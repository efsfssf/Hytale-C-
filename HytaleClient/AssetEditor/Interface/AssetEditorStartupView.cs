using System;
using System.IO;
using HytaleClient.Data;
using HytaleClient.Graphics;
using HytaleClient.Interface.UI;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Math;
using HytaleClient.Utils;

namespace HytaleClient.AssetEditor.Interface
{
	// Token: 0x02000B96 RID: 2966
	public class AssetEditorStartupView : Element
	{
		// Token: 0x06005B9A RID: 23450 RVA: 0x001CA974 File Offset: 0x001C8B74
		public AssetEditorStartupView(Desktop desktop, Element parent) : base(desktop, parent)
		{
		}

		// Token: 0x06005B9B RID: 23451 RVA: 0x001CA980 File Offset: 0x001C8B80
		protected override void OnMounted()
		{
			Image image = new Image(File.ReadAllBytes(Path.Combine(Paths.EditorData, "Splashscreen.png")));
			this._splashScreenTexture = new Texture(Texture.TextureTypes.Texture2D);
			this._splashScreenTexture.CreateTexture2D(image.Width, image.Height, image.Pixels, 5, GL.LINEAR, GL.LINEAR, GL.CLAMP_TO_EDGE, GL.CLAMP_TO_EDGE, GL.RGBA, GL.RGBA, GL.UNSIGNED_BYTE, false);
		}

		// Token: 0x06005B9C RID: 23452 RVA: 0x001CA9F7 File Offset: 0x001C8BF7
		protected override void OnUnmounted()
		{
			this._splashScreenTexture.Dispose();
			this._splashScreenTexture = null;
		}

		// Token: 0x06005B9D RID: 23453 RVA: 0x001CAA10 File Offset: 0x001C8C10
		protected override void PrepareForDrawSelf()
		{
			Rectangle viewportRectangle = this.Desktop.ViewportRectangle;
			int num = viewportRectangle.Width / viewportRectangle.Height;
			bool flag = (float)num > (float)this._splashScreenTexture.Width / (float)this._splashScreenTexture.Height;
			if (flag)
			{
				viewportRectangle.Width = this._splashScreenTexture.Width * this.Desktop.ViewportRectangle.Height / this._splashScreenTexture.Height;
				viewportRectangle.X = this.Desktop.ViewportRectangle.Center.X - viewportRectangle.Width / 2;
			}
			else
			{
				viewportRectangle.Height = this._splashScreenTexture.Height * this.Desktop.ViewportRectangle.Width / this._splashScreenTexture.Width;
				viewportRectangle.Y = this.Desktop.ViewportRectangle.Center.Y - viewportRectangle.Height / 2;
			}
			this.Desktop.Batcher2D.RequestDrawTexture(this._splashScreenTexture, new Rectangle(0, 0, this._splashScreenTexture.Width, this._splashScreenTexture.Height), viewportRectangle, UInt32Color.White);
		}

		// Token: 0x0400394A RID: 14666
		private Texture _splashScreenTexture;
	}
}
