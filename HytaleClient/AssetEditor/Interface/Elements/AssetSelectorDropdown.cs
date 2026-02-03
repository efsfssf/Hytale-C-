using System;
using HytaleClient.AssetEditor.Interface.Editor;
using HytaleClient.Interface.UI;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Interface.UI.Styles;
using HytaleClient.Math;

namespace HytaleClient.AssetEditor.Interface.Elements
{
	// Token: 0x02000BA6 RID: 2982
	internal class AssetSelectorDropdown : Element
	{
		// Token: 0x170013A4 RID: 5028
		// (get) Token: 0x06005C5F RID: 23647 RVA: 0x001D14D8 File Offset: 0x001CF6D8
		// (set) Token: 0x06005C60 RID: 23648 RVA: 0x001D14E0 File Offset: 0x001CF6E0
		public string AssetType
		{
			get
			{
				return this._assetType;
			}
			set
			{
				this._assetType = value;
				bool isMounted = this._assetSelector.IsMounted;
				if (isMounted)
				{
					this._assetSelector.UpdateSelectedItem();
				}
			}
		}

		// Token: 0x170013A5 RID: 5029
		// (get) Token: 0x06005C61 RID: 23649 RVA: 0x001D1510 File Offset: 0x001CF710
		// (set) Token: 0x06005C62 RID: 23650 RVA: 0x001D1518 File Offset: 0x001CF718
		public string Value
		{
			get
			{
				return this._value;
			}
			set
			{
				this._value = value;
				this._label.Text = value;
				bool isMounted = this._assetSelector.IsMounted;
				if (isMounted)
				{
					this._assetSelector.UpdateSelectedItem();
				}
			}
		}

		// Token: 0x06005C63 RID: 23651 RVA: 0x001D1558 File Offset: 0x001CF758
		public AssetSelectorDropdown(Desktop desktop, Element parent, AssetEditorOverlay overlay) : base(desktop, parent)
		{
			this._layoutMode = LayoutMode.Left;
			this._assetSelector = new AssetSelector(desktop, overlay, this);
			this._label = new Label(this.Desktop, this)
			{
				Style = new LabelStyle
				{
					VerticalAlignment = LabelStyle.LabelAlignment.Center
				},
				FlexWeight = 1
			};
			this._arrow = new Group(this.Desktop, this);
		}

		// Token: 0x06005C64 RID: 23652 RVA: 0x001D15C1 File Offset: 0x001CF7C1
		public override Element HitTest(Point position)
		{
			return this._anchoredRectangle.Contains(position) ? this : null;
		}

		// Token: 0x06005C65 RID: 23653 RVA: 0x001D15D8 File Offset: 0x001CF7D8
		protected override void OnUnmounted()
		{
			bool isMounted = this._assetSelector.IsMounted;
			if (isMounted)
			{
				this.CloseDropdown(null);
			}
		}

		// Token: 0x06005C66 RID: 23654 RVA: 0x001D1600 File Offset: 0x001CF800
		protected override void OnMouseButtonUp(MouseButtonEvent evt, bool activate)
		{
			bool flag = !activate || (long)evt.Button != 1L;
			if (!flag)
			{
				this.Desktop.SetTransientLayer(this._assetSelector);
			}
		}

		// Token: 0x06005C67 RID: 23655 RVA: 0x001D163C File Offset: 0x001CF83C
		protected override void OnMouseButtonDown(MouseButtonEvent evt)
		{
			base.Layout(null, true);
		}

		// Token: 0x06005C68 RID: 23656 RVA: 0x001D165C File Offset: 0x001CF85C
		protected override void OnMouseEnter()
		{
			base.Layout(null, true);
		}

		// Token: 0x06005C69 RID: 23657 RVA: 0x001D167C File Offset: 0x001CF87C
		protected override void OnMouseLeave()
		{
			base.Layout(null, true);
		}

		// Token: 0x06005C6A RID: 23658 RVA: 0x001D169C File Offset: 0x001CF89C
		protected internal void CloseDropdown(string newSelectedValue)
		{
			this.Desktop.SetTransientLayer(null);
			bool flag = newSelectedValue != null;
			if (flag)
			{
				this.Value = newSelectedValue;
				Action valueChanged = this.ValueChanged;
				if (valueChanged != null)
				{
					valueChanged();
				}
			}
			bool isMounted = base.IsMounted;
			if (isMounted)
			{
				base.Layout(null, true);
			}
		}

		// Token: 0x06005C6B RID: 23659 RVA: 0x001D16F8 File Offset: 0x001CF8F8
		protected override void ApplyStyles()
		{
			bool isMounted = this._assetSelector.IsMounted;
			if (isMounted)
			{
				PatchStyle background;
				if ((background = this.Style.PressedBackground) == null)
				{
					background = (this.Style.HoveredBackground ?? this.Style.DefaultBackground);
				}
				this.Background = background;
				Element arrow = this._arrow;
				PatchStyle patchStyle = new PatchStyle();
				UIPath texturePath;
				if ((texturePath = this.Style.PressedArrowTexturePath) == null)
				{
					texturePath = (this.Style.HoveredArrowTexturePath ?? this.Style.DefaultArrowTexturePath);
				}
				patchStyle.TexturePath = texturePath;
				arrow.Background = patchStyle;
			}
			else
			{
				bool isHovered = base.IsHovered;
				if (isHovered)
				{
					this.Background = (this.Style.HoveredBackground ?? this.Style.DefaultBackground);
					this._arrow.Background = new PatchStyle
					{
						TexturePath = (this.Style.HoveredArrowTexturePath ?? this.Style.DefaultArrowTexturePath)
					};
				}
				else
				{
					this.Background = this.Style.DefaultBackground;
					this._arrow.Background = new PatchStyle
					{
						TexturePath = this.Style.DefaultArrowTexturePath
					};
				}
			}
			base.ApplyStyles();
			this._arrow.Anchor.Width = new int?(this.Style.ArrowWidth);
			this._arrow.Anchor.Height = new int?(this.Style.ArrowHeight);
			this._arrow.Anchor.Right = new int?(this.Style.HorizontalPadding);
			this._label.Style = (this.Style.LabelStyle ?? new LabelStyle());
			this._label.Anchor.Left = (this._label.Anchor.Right = new int?(this.Style.HorizontalPadding));
		}

		// Token: 0x06005C6C RID: 23660 RVA: 0x001D18DC File Offset: 0x001CFADC
		protected override void LayoutSelf()
		{
			bool isMounted = this._assetSelector.IsMounted;
			if (isMounted)
			{
				this._assetSelector.Layout(null, true);
			}
		}

		// Token: 0x06005C6D RID: 23661 RVA: 0x001D190F File Offset: 0x001CFB0F
		public void Open()
		{
			this.Desktop.SetTransientLayer(this._assetSelector);
		}

		// Token: 0x040039DA RID: 14810
		private readonly AssetSelector _assetSelector;

		// Token: 0x040039DB RID: 14811
		private readonly Label _label;

		// Token: 0x040039DC RID: 14812
		private readonly Group _arrow;

		// Token: 0x040039DD RID: 14813
		public FileDropdownBoxStyle Style;

		// Token: 0x040039DE RID: 14814
		private string _assetType;

		// Token: 0x040039DF RID: 14815
		public Action ValueChanged;

		// Token: 0x040039E0 RID: 14816
		private string _value;
	}
}
