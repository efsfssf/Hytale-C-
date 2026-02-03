using System;
using System.Diagnostics;
using HytaleClient.Graphics;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Math;

namespace HytaleClient.Interface.UI.Styles
{
	// Token: 0x0200083B RID: 2107
	[UIMarkupData]
	public class PatchStyle
	{
		// Token: 0x1700102A RID: 4138
		// (get) Token: 0x06003AA2 RID: 15010 RVA: 0x00085CA2 File Offset: 0x00083EA2
		// (set) Token: 0x06003AA3 RID: 15011 RVA: 0x00085CAA File Offset: 0x00083EAA
		internal TextureArea TextureArea
		{
			get
			{
				return this._textureArea;
			}
			set
			{
				Debug.Assert(this._texturePath == null);
				this._textureArea = value;
			}
		}

		// Token: 0x1700102B RID: 4139
		// (get) Token: 0x06003AA4 RID: 15012 RVA: 0x00085CC3 File Offset: 0x00083EC3
		// (set) Token: 0x06003AA5 RID: 15013 RVA: 0x00085CCB File Offset: 0x00083ECB
		public UIPath TexturePath
		{
			get
			{
				return this._texturePath;
			}
			set
			{
				Debug.Assert(this._textureArea == null);
				this._texturePath = value;
			}
		}

		// Token: 0x1700102C RID: 4140
		// (set) Token: 0x06003AA6 RID: 15014 RVA: 0x00085CE4 File Offset: 0x00083EE4
		public int Border
		{
			set
			{
				this.VerticalBorder = value;
				this.HorizontalBorder = value;
			}
		}

		// Token: 0x06003AA7 RID: 15015 RVA: 0x00085D01 File Offset: 0x00083F01
		public PatchStyle()
		{
		}

		// Token: 0x06003AA8 RID: 15016 RVA: 0x00085D16 File Offset: 0x00083F16
		public PatchStyle(TextureArea textureArea)
		{
			this._textureArea = textureArea;
		}

		// Token: 0x06003AA9 RID: 15017 RVA: 0x00085D32 File Offset: 0x00083F32
		public PatchStyle(string texturePath)
		{
			this._texturePath = new UIPath(texturePath);
		}

		// Token: 0x06003AAA RID: 15018 RVA: 0x00085D53 File Offset: 0x00083F53
		public PatchStyle(UInt32Color color)
		{
			this.Color = color;
		}

		// Token: 0x06003AAB RID: 15019 RVA: 0x00085D6F File Offset: 0x00083F6F
		public PatchStyle(uint rgba) : this(UInt32Color.FromRGBA(rgba))
		{
		}

		// Token: 0x06003AAC RID: 15020 RVA: 0x00085D80 File Offset: 0x00083F80
		public PatchStyle Clone()
		{
			return new PatchStyle
			{
				_texturePath = this._texturePath,
				_textureArea = this._textureArea,
				Anchor = this.Anchor,
				Area = this.Area,
				Color = this.Color,
				HorizontalBorder = this.HorizontalBorder,
				VerticalBorder = this.VerticalBorder
			};
		}

		// Token: 0x04001ADE RID: 6878
		private TextureArea _textureArea;

		// Token: 0x04001ADF RID: 6879
		private UIPath _texturePath;

		// Token: 0x04001AE0 RID: 6880
		public Rectangle? Area;

		// Token: 0x04001AE1 RID: 6881
		public UInt32Color Color = UInt32Color.White;

		// Token: 0x04001AE2 RID: 6882
		public Anchor? Anchor;

		// Token: 0x04001AE3 RID: 6883
		public int HorizontalBorder;

		// Token: 0x04001AE4 RID: 6884
		public int VerticalBorder;
	}
}
