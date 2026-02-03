using System;
using System.Diagnostics;
using System.IO;
using HytaleClient.Graphics;
using HytaleClient.Graphics.Fonts;
using HytaleClient.Utils;

namespace HytaleClient.Core
{
	// Token: 0x02000B7D RID: 2941
	internal class FontManager : Disposable
	{
		// Token: 0x17001380 RID: 4992
		// (get) Token: 0x06005A55 RID: 23125 RVA: 0x001C18DE File Offset: 0x001BFADE
		// (set) Token: 0x06005A56 RID: 23126 RVA: 0x001C18E6 File Offset: 0x001BFAE6
		public Texture TextureArray2D { get; private set; }

		// Token: 0x17001381 RID: 4993
		// (get) Token: 0x06005A57 RID: 23127 RVA: 0x001C18EF File Offset: 0x001BFAEF
		// (set) Token: 0x06005A58 RID: 23128 RVA: 0x001C18F7 File Offset: 0x001BFAF7
		public FontFamily DefaultFontFamily { get; private set; }

		// Token: 0x17001382 RID: 4994
		// (get) Token: 0x06005A59 RID: 23129 RVA: 0x001C1900 File Offset: 0x001BFB00
		// (set) Token: 0x06005A5A RID: 23130 RVA: 0x001C1908 File Offset: 0x001BFB08
		public FontFamily SecondaryFontFamily { get; private set; }

		// Token: 0x17001383 RID: 4995
		// (get) Token: 0x06005A5B RID: 23131 RVA: 0x001C1911 File Offset: 0x001BFB11
		// (set) Token: 0x06005A5C RID: 23132 RVA: 0x001C1919 File Offset: 0x001BFB19
		public FontFamily MonospaceFontFamily { get; private set; }

		// Token: 0x06005A5D RID: 23133 RVA: 0x001C1924 File Offset: 0x001BFB24
		internal void LoadFonts(GraphicsDevice graphics)
		{
			string basePath = Path.Combine(Paths.SharedData, "Fonts/NotoSans-Regular");
			int fontCount = this._fontCount;
			this._fontCount = fontCount + 1;
			Font regularFont = new Font(graphics, basePath, fontCount, 32, 8, 8, null);
			string basePath2 = Path.Combine(Paths.SharedData, "Fonts/NotoSans-Bold");
			fontCount = this._fontCount;
			this._fontCount = fontCount + 1;
			Font boldFont = new Font(graphics, basePath2, fontCount, 32, 8, 8, null);
			string basePath3 = Path.Combine(Paths.SharedData, "Fonts/PenumbraSerifStd-Semibold");
			fontCount = this._fontCount;
			this._fontCount = fontCount + 1;
			Font regularFont2 = new Font(graphics, basePath3, fontCount, 32, 8, 8, null);
			string basePath4 = Path.Combine(Paths.SharedData, "Fonts/PenumbraSerifStd-Bold");
			fontCount = this._fontCount;
			this._fontCount = fontCount + 1;
			Font boldFont2 = new Font(graphics, basePath4, fontCount, 32, 8, 8, null);
			string basePath5 = Path.Combine(Paths.SharedData, "Fonts/NotoMono-Regular");
			fontCount = this._fontCount;
			this._fontCount = fontCount + 1;
			Font regularFont3 = new Font(graphics, basePath5, fontCount, 32, 8, 8, null);
			this.DefaultFontFamily = new FontFamily(regularFont, boldFont);
			this.SecondaryFontFamily = new FontFamily(regularFont2, boldFont2);
			this.MonospaceFontFamily = new FontFamily(regularFont3, null);
		}

		// Token: 0x06005A5E RID: 23134 RVA: 0x001C1A4C File Offset: 0x001BFC4C
		internal void BuildFontTextures()
		{
			Debug.Assert(ThreadHelper.IsMainThread());
			this.DefaultFontFamily.RegularFont.BuildTexture();
			this.DefaultFontFamily.BoldFont.BuildTexture();
			this.SecondaryFontFamily.RegularFont.BuildTexture();
			this.SecondaryFontFamily.BoldFont.BuildTexture();
			this.MonospaceFontFamily.RegularFont.BuildTexture();
			this.TextureArray2D = new Texture(Texture.TextureTypes.Texture2DArray);
			this.TextureArray2D.CreateTexture2DArray(2048, 2048, this._fontCount, null, GL.LINEAR, GL.LINEAR, GL.REPEAT, GL.REPEAT, GL.RGBA, GL.RGBA, GL.UNSIGNED_BYTE, false);
			this.TextureArray2D.UpdateTexture2DArrayLayer(this.DefaultFontFamily.RegularFont.TextureAtlas, this.DefaultFontFamily.RegularFont.FontId);
			this.TextureArray2D.UpdateTexture2DArrayLayer(this.DefaultFontFamily.BoldFont.TextureAtlas, this.DefaultFontFamily.BoldFont.FontId);
			this.TextureArray2D.UpdateTexture2DArrayLayer(this.SecondaryFontFamily.RegularFont.TextureAtlas, this.SecondaryFontFamily.RegularFont.FontId);
			this.TextureArray2D.UpdateTexture2DArrayLayer(this.SecondaryFontFamily.BoldFont.TextureAtlas, this.SecondaryFontFamily.BoldFont.FontId);
			this.TextureArray2D.UpdateTexture2DArrayLayer(this.MonospaceFontFamily.RegularFont.TextureAtlas, this.MonospaceFontFamily.RegularFont.FontId);
		}

		// Token: 0x06005A5F RID: 23135 RVA: 0x001C1BE4 File Offset: 0x001BFDE4
		internal void BuildMissingGlyphs()
		{
			FontFamily defaultFontFamily = this.DefaultFontFamily;
			if (defaultFontFamily != null)
			{
				defaultFontFamily.RegularFont.BuildMissingGlyphs();
			}
			FontFamily defaultFontFamily2 = this.DefaultFontFamily;
			if (defaultFontFamily2 != null)
			{
				defaultFontFamily2.BoldFont.BuildMissingGlyphs();
			}
			FontFamily secondaryFontFamily = this.SecondaryFontFamily;
			if (secondaryFontFamily != null)
			{
				secondaryFontFamily.RegularFont.BuildMissingGlyphs();
			}
			FontFamily secondaryFontFamily2 = this.SecondaryFontFamily;
			if (secondaryFontFamily2 != null)
			{
				secondaryFontFamily2.BoldFont.BuildMissingGlyphs();
			}
			FontFamily monospaceFontFamily = this.MonospaceFontFamily;
			if (monospaceFontFamily != null)
			{
				monospaceFontFamily.RegularFont.BuildMissingGlyphs();
			}
		}

		// Token: 0x06005A60 RID: 23136 RVA: 0x001C1C68 File Offset: 0x001BFE68
		protected override void DoDispose()
		{
			Texture textureArray2D = this.TextureArray2D;
			if (textureArray2D != null)
			{
				textureArray2D.Dispose();
			}
			FontFamily defaultFontFamily = this.DefaultFontFamily;
			if (defaultFontFamily != null)
			{
				defaultFontFamily.RegularFont.Dispose();
			}
			FontFamily defaultFontFamily2 = this.DefaultFontFamily;
			if (defaultFontFamily2 != null)
			{
				defaultFontFamily2.BoldFont.Dispose();
			}
			FontFamily secondaryFontFamily = this.SecondaryFontFamily;
			if (secondaryFontFamily != null)
			{
				secondaryFontFamily.RegularFont.Dispose();
			}
			FontFamily secondaryFontFamily2 = this.SecondaryFontFamily;
			if (secondaryFontFamily2 != null)
			{
				secondaryFontFamily2.BoldFont.Dispose();
			}
			FontFamily monospaceFontFamily = this.MonospaceFontFamily;
			if (monospaceFontFamily != null)
			{
				monospaceFontFamily.RegularFont.Dispose();
			}
		}

		// Token: 0x06005A61 RID: 23137 RVA: 0x001C1CFC File Offset: 0x001BFEFC
		public FontFamily GetFontFamilyByName(string name)
		{
			FontFamily result;
			if (!(name == "Default"))
			{
				if (!(name == "Secondary"))
				{
					if (!(name == "Mono"))
					{
						result = null;
					}
					else
					{
						result = this.MonospaceFontFamily;
					}
				}
				else
				{
					result = this.SecondaryFontFamily;
				}
			}
			else
			{
				result = this.DefaultFontFamily;
			}
			return result;
		}

		// Token: 0x04003875 RID: 14453
		private int _fontCount = 0;
	}
}
