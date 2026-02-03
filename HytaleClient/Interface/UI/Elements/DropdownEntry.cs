using System;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Interface.UI.Styles;

namespace HytaleClient.Interface.UI.Elements
{
	// Token: 0x0200085C RID: 2140
	[UIMarkupElement]
	public class DropdownEntry : Button
	{
		// Token: 0x1700103E RID: 4158
		// (get) Token: 0x06003B83 RID: 15235 RVA: 0x0008D05A File Offset: 0x0008B25A
		// (set) Token: 0x06003B84 RID: 15236 RVA: 0x0008D062 File Offset: 0x0008B262
		[UIMarkupProperty]
		public string Text
		{
			get
			{
				return this._text;
			}
			set
			{
				this._text = value;
				this._label.Text = value;
			}
		}

		// Token: 0x06003B85 RID: 15237 RVA: 0x0008D079 File Offset: 0x0008B279
		public DropdownEntry(Desktop desktop, Element parent) : base(desktop, parent)
		{
			this._label = new Label(this.Desktop, this);
		}

		// Token: 0x06003B86 RID: 15238 RVA: 0x0008D098 File Offset: 0x0008B298
		public DropdownEntry(DropdownLayer layer, string value, string text, bool selected = false) : base(layer.Desktop, layer.EntriesContainer)
		{
			this.Layer = layer;
			this.Value = value;
			this.Selected = selected;
			this._icon = new Group(this.Desktop, this)
			{
				Visible = false
			};
			this._label = new Label(this.Desktop, this)
			{
				Text = text
			};
		}

		// Token: 0x06003B87 RID: 15239 RVA: 0x0008D104 File Offset: 0x0008B304
		public void ApplyStylesFromDropdownBox()
		{
			DropdownBoxStyle style = this.Layer.DropdownBox.Style;
			int num = 0;
			this.Style.Sounds = style.EntrySounds;
			this.Anchor = new Anchor
			{
				Height = new int?(this.Layer.DropdownBox.Style.EntryHeight)
			};
			bool flag = style.EntryIconBackground != null || style.SelectedEntryIconBackground != null;
			if (flag)
			{
				this._icon.Anchor = new Anchor
				{
					Width = new int?(style.EntryIconWidth),
					Height = new int?(style.EntryIconHeight),
					Left = new int?(style.HorizontalEntryPadding)
				};
				this._icon.Visible = true;
				num = style.EntryIconWidth + style.HorizontalEntryPadding;
				bool selected = this.Selected;
				if (selected)
				{
					bool flag2 = style.SelectedEntryIconBackground != null;
					if (flag2)
					{
						this._icon.Background = style.SelectedEntryIconBackground;
					}
					else
					{
						this._icon.Visible = false;
						num = 0;
					}
				}
				else
				{
					bool flag3 = style.EntryIconBackground != null;
					if (flag3)
					{
						this._icon.Background = style.EntryIconBackground;
					}
					else
					{
						this._icon.Visible = false;
						num = 0;
					}
				}
			}
			this._label.Style = ((this.Selected && style.SelectedEntryLabelStyle != null) ? style.SelectedEntryLabelStyle : (style.EntryLabelStyle ?? new LabelStyle()));
			this._label.Anchor = new Anchor
			{
				Left = new int?(style.HorizontalEntryPadding + num),
				Right = new int?(style.HorizontalEntryPadding)
			};
			this.OutlineColor = style.FocusOutlineColor;
		}

		// Token: 0x06003B88 RID: 15240 RVA: 0x0008D2DD File Offset: 0x0008B4DD
		protected override void OnMouseEnter()
		{
			base.OnMouseEnter();
			this.Background = this.Layer.DropdownBox.Style.HoveredEntryBackground;
			this.ApplyStyles();
		}

		// Token: 0x06003B89 RID: 15241 RVA: 0x0008D309 File Offset: 0x0008B509
		protected override void OnMouseLeave()
		{
			base.OnMouseLeave();
			this.Background = null;
			this.ApplyStyles();
		}

		// Token: 0x06003B8A RID: 15242 RVA: 0x0008D324 File Offset: 0x0008B524
		protected override void OnMouseButtonDown(MouseButtonEvent evt)
		{
			bool flag = (long)evt.Button == 1L;
			if (flag)
			{
				this.Background = this.Layer.DropdownBox.Style.PressedEntryBackground;
				this.ApplyStyles();
			}
			base.OnMouseButtonDown(evt);
		}

		// Token: 0x06003B8B RID: 15243 RVA: 0x0008D370 File Offset: 0x0008B570
		protected override void OnMouseButtonUp(MouseButtonEvent evt, bool activate)
		{
			bool flag = (long)evt.Button == 1L;
			if (flag)
			{
				this.Background = (activate ? this.Layer.DropdownBox.Style.HoveredEntryBackground : null);
				this.ApplyStyles();
			}
			base.OnMouseButtonUp(evt, activate);
			bool flag2 = (long)evt.Button == 1L && activate;
			if (flag2)
			{
				this.Layer.OnActivateEntry(this);
			}
		}

		// Token: 0x04001B70 RID: 7024
		public DropdownLayer Layer;

		// Token: 0x04001B71 RID: 7025
		private readonly Group _icon;

		// Token: 0x04001B72 RID: 7026
		private readonly Label _label;

		// Token: 0x04001B73 RID: 7027
		private string _text;

		// Token: 0x04001B74 RID: 7028
		[UIMarkupProperty]
		public string Value;

		// Token: 0x04001B75 RID: 7029
		[UIMarkupProperty]
		public bool Selected;
	}
}
