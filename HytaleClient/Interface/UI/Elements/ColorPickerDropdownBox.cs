using System;
using HytaleClient.Graphics;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Interface.UI.Styles;
using HytaleClient.Math;

namespace HytaleClient.Interface.UI.Elements
{
	// Token: 0x02000858 RID: 2136
	[UIMarkupElement]
	public class ColorPickerDropdownBox : Element
	{
		// Token: 0x17001034 RID: 4148
		// (get) Token: 0x06003B4C RID: 15180 RVA: 0x0008BA7F File Offset: 0x00089C7F
		// (set) Token: 0x06003B4D RID: 15181 RVA: 0x0008BA91 File Offset: 0x00089C91
		[UIMarkupProperty]
		public UInt32Color Color
		{
			get
			{
				return this._dropdownLayer.ColorPicker.Value;
			}
			set
			{
				this._dropdownLayer.ColorPicker.Value = value;
			}
		}

		// Token: 0x17001035 RID: 4149
		// (get) Token: 0x06003B4E RID: 15182 RVA: 0x0008BAA5 File Offset: 0x00089CA5
		// (set) Token: 0x06003B4F RID: 15183 RVA: 0x0008BAB7 File Offset: 0x00089CB7
		[UIMarkupProperty]
		public ColorPicker.ColorFormat Format
		{
			get
			{
				return this._dropdownLayer.ColorPicker.Format;
			}
			set
			{
				this._dropdownLayer.ColorPicker.Format = value;
			}
		}

		// Token: 0x17001036 RID: 4150
		// (get) Token: 0x06003B50 RID: 15184 RVA: 0x0008BACB File Offset: 0x00089CCB
		// (set) Token: 0x06003B51 RID: 15185 RVA: 0x0008BADD File Offset: 0x00089CDD
		[UIMarkupProperty]
		public bool ResetTransparencyWhenChangingColor
		{
			get
			{
				return this._dropdownLayer.ColorPicker.ResetTransparencyWhenChangingColor;
			}
			set
			{
				this._dropdownLayer.ColorPicker.ResetTransparencyWhenChangingColor = value;
			}
		}

		// Token: 0x17001037 RID: 4151
		// (get) Token: 0x06003B52 RID: 15186 RVA: 0x0008BAF0 File Offset: 0x00089CF0
		// (set) Token: 0x06003B53 RID: 15187 RVA: 0x0008BB02 File Offset: 0x00089D02
		[UIMarkupProperty]
		public bool DisplayTextField
		{
			get
			{
				return this._dropdownLayer.ColorPicker.DisplayTextField;
			}
			set
			{
				this._dropdownLayer.ColorPicker.DisplayTextField = value;
			}
		}

		// Token: 0x17001038 RID: 4152
		// (set) Token: 0x06003B54 RID: 15188 RVA: 0x0008BB16 File Offset: 0x00089D16
		public Action ValueChanged
		{
			set
			{
				this._dropdownLayer.ColorPicker.ValueChanged = value;
			}
		}

		// Token: 0x06003B55 RID: 15189 RVA: 0x0008BB29 File Offset: 0x00089D29
		public ColorPickerDropdownBox(Desktop desktop, Element parent) : base(desktop, parent)
		{
			this._dropdownLayer = new ColorPickerDropdownLayer(this);
			this._arrow = new Element(desktop, this);
		}

		// Token: 0x06003B56 RID: 15190 RVA: 0x0008BB50 File Offset: 0x00089D50
		protected override void OnUnmounted()
		{
			bool isMounted = this._dropdownLayer.IsMounted;
			if (isMounted)
			{
				this.CloseDropdown();
			}
		}

		// Token: 0x06003B57 RID: 15191 RVA: 0x0008BB74 File Offset: 0x00089D74
		protected override void ApplyStyles()
		{
			bool flag = base.CapturedMouseButton != null;
			PatchStyle patchStyle;
			if (flag)
			{
				object background;
				if (this.Style.Background == null)
				{
					background = null;
				}
				else if ((background = this.Style.Background.Pressed) == null)
				{
					background = (this.Style.Background.Hovered ?? this.Style.Background.Default);
				}
				this.Background = background;
				Element arrow = this._arrow;
				object background2;
				if (this.Style.ArrowBackground == null)
				{
					background2 = null;
				}
				else if ((background2 = this.Style.ArrowBackground.Pressed) == null)
				{
					background2 = (this.Style.ArrowBackground.Hovered ?? this.Style.ArrowBackground.Default);
				}
				arrow.Background = background2;
				object obj;
				if (this.Style.Overlay == null)
				{
					obj = null;
				}
				else if ((obj = this.Style.Overlay.Pressed) == null)
				{
					obj = (this.Style.Overlay.Hovered ?? this.Style.Overlay.Default);
				}
				patchStyle = obj;
			}
			else
			{
				bool isHovered = base.IsHovered;
				if (isHovered)
				{
					this.Background = ((this.Style.Background != null) ? (this.Style.Background.Hovered ?? this.Style.Background.Default) : null);
					this._arrow.Background = ((this.Style.Background != null) ? (this.Style.ArrowBackground.Hovered ?? this.Style.ArrowBackground.Default) : null);
					patchStyle = ((this.Style.Overlay != null) ? (this.Style.Overlay.Hovered ?? this.Style.Overlay.Default) : null);
				}
				else
				{
					ColorPickerDropdownBoxStyle.ColorPickerDropdownBoxStateBackground background3 = this.Style.Background;
					this.Background = ((background3 != null) ? background3.Default : null);
					Element arrow2 = this._arrow;
					ColorPickerDropdownBoxStyle.ColorPickerDropdownBoxStateBackground arrowBackground = this.Style.ArrowBackground;
					arrow2.Background = ((arrowBackground != null) ? arrowBackground.Default : null);
					ColorPickerDropdownBoxStyle.ColorPickerDropdownBoxStateBackground overlay = this.Style.Overlay;
					patchStyle = ((overlay != null) ? overlay.Default : null);
				}
			}
			base.ApplyStyles();
			this._arrow.Anchor = this.Style.ArrowAnchor;
			this._overlayPatch = ((patchStyle != null) ? this.Desktop.MakeTexturePatch(patchStyle) : null);
		}

		// Token: 0x06003B58 RID: 15192 RVA: 0x0008BDD8 File Offset: 0x00089FD8
		protected override void LayoutSelf()
		{
			bool isMounted = this._dropdownLayer.IsMounted;
			if (isMounted)
			{
				this._dropdownLayer.Layout(null, true);
			}
		}

		// Token: 0x06003B59 RID: 15193 RVA: 0x0008BE0B File Offset: 0x0008A00B
		public override Element HitTest(Point position)
		{
			return this._anchoredRectangle.Contains(position) ? this : null;
		}

		// Token: 0x06003B5A RID: 15194 RVA: 0x0008BE20 File Offset: 0x0008A020
		protected override void OnMouseEnter()
		{
			ButtonSounds sounds = this.Style.Sounds;
			bool flag = ((sounds != null) ? sounds.MouseHover : null) != null;
			if (flag)
			{
				this.Desktop.Provider.PlaySound(this.Style.Sounds.MouseHover);
			}
			base.Layout(null, true);
		}

		// Token: 0x06003B5B RID: 15195 RVA: 0x0008BE80 File Offset: 0x0008A080
		protected override void OnMouseLeave()
		{
			base.Layout(null, true);
		}

		// Token: 0x06003B5C RID: 15196 RVA: 0x0008BEA0 File Offset: 0x0008A0A0
		protected override void OnMouseButtonDown(MouseButtonEvent evt)
		{
			bool flag = (long)evt.Button != 1L;
			if (!flag)
			{
				ButtonSounds sounds = this.Style.Sounds;
				bool flag2 = ((sounds != null) ? sounds.Activate : null) != null;
				if (flag2)
				{
					this.Desktop.Provider.PlaySound(this.Style.Sounds.Activate);
				}
				base.Layout(null, true);
				this.Open();
			}
		}

		// Token: 0x06003B5D RID: 15197 RVA: 0x0008BF1C File Offset: 0x0008A11C
		protected override void OnMouseButtonUp(MouseButtonEvent evt, bool activate)
		{
			bool flag = !activate || (long)evt.Button != 3L;
			if (!flag)
			{
				Action rightClicking = this.RightClicking;
				if (rightClicking != null)
				{
					rightClicking();
				}
			}
		}

		// Token: 0x06003B5E RID: 15198 RVA: 0x0008BF58 File Offset: 0x0008A158
		internal void CloseDropdown()
		{
			this.Desktop.SetTransientLayer(null);
			bool isMounted = base.IsMounted;
			if (isMounted)
			{
				base.Layout(null, true);
			}
		}

		// Token: 0x06003B5F RID: 15199 RVA: 0x0008BF8E File Offset: 0x0008A18E
		public void Open()
		{
			this.Desktop.SetTransientLayer(this._dropdownLayer);
		}

		// Token: 0x06003B60 RID: 15200 RVA: 0x0008BFA4 File Offset: 0x0008A1A4
		protected override void PrepareForDrawSelf()
		{
			bool flag = this._backgroundPatch != null;
			if (flag)
			{
				this.Desktop.Batcher2D.RequestDrawPatch(this._backgroundPatch, this._backgroundRectangle, this.Desktop.Scale);
			}
			this.Desktop.Batcher2D.RequestDrawTexture(this.Desktop.Provider.WhitePixel.Texture, this.Desktop.Provider.WhitePixel.Rectangle, this._rectangleAfterPadding, this.Color);
			bool flag2 = this._overlayPatch != null;
			if (flag2)
			{
				this.Desktop.Batcher2D.RequestDrawPatch(this._overlayPatch, this._rectangleAfterPadding, this.Desktop.Scale);
			}
			bool flag3 = this.OutlineSize > 0f;
			if (flag3)
			{
				TextureArea whitePixel = this.Desktop.Provider.WhitePixel;
				this.Desktop.Batcher2D.RequestDrawOutline(whitePixel.Texture, whitePixel.Rectangle, this._anchoredRectangle, this.OutlineSize * this.Desktop.Scale, this.OutlineColor);
			}
		}

		// Token: 0x04001B56 RID: 6998
		[UIMarkupProperty]
		public ColorPickerDropdownBoxStyle Style;

		// Token: 0x04001B57 RID: 6999
		public Action RightClicking;

		// Token: 0x04001B58 RID: 7000
		private readonly ColorPickerDropdownLayer _dropdownLayer;

		// Token: 0x04001B59 RID: 7001
		private readonly Element _arrow;

		// Token: 0x04001B5A RID: 7002
		private TexturePatch _overlayPatch;
	}
}
