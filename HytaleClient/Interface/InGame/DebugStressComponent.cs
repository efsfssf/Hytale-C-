using System;
using HytaleClient.Graphics;
using HytaleClient.Graphics.Fonts;
using HytaleClient.Math;

namespace HytaleClient.Interface.InGame
{
	// Token: 0x0200087F RID: 2175
	internal class DebugStressComponent : InterfaceComponent
	{
		// Token: 0x06003D87 RID: 15751 RVA: 0x0009ED57 File Offset: 0x0009CF57
		public DebugStressComponent(InGameView view) : base(view.Interface, view.HudContainer)
		{
		}

		// Token: 0x06003D88 RID: 15752 RVA: 0x0009ED6D File Offset: 0x0009CF6D
		protected override void ApplyStyles()
		{
			base.ApplyStyles();
			this._font = this.Desktop.Provider.GetFontFamily("Default").RegularFont;
		}

		// Token: 0x06003D89 RID: 15753 RVA: 0x0009ED98 File Offset: 0x0009CF98
		protected override void PrepareForDrawSelf()
		{
			Vector3 zero = Vector3.Zero;
			Rectangle destRect = new Rectangle(0, 0, 20, 10);
			UInt32Color color = UInt32Color.FromRGBA(0, 0, 0, 100);
			TextureArea whitePixel = this.Interface.WhitePixel;
			for (int i = 0; i < 1000; i++)
			{
				zero.X = (float)(destRect.X = 400 + i % 20 * 40);
				zero.Y = (float)(destRect.Y = i / 20 * 11);
				this.Desktop.Batcher2D.RequestDrawText(this._font, 10f, "Debug", zero, UInt32Color.White, false, false, 0f);
				this.Desktop.Batcher2D.RequestDrawTexture(whitePixel.Texture, whitePixel.Rectangle, destRect, color);
			}
		}

		// Token: 0x04001C9F RID: 7327
		private Font _font;
	}
}
