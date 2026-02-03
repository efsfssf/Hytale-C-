using System;
using System.Collections.Generic;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Interface.UI.Styles;
using HytaleClient.Math;

namespace HytaleClient.Interface.UI.Elements
{
	// Token: 0x0200086E RID: 2158
	internal class PopupMenuLayer : Element
	{
		// Token: 0x1700106F RID: 4207
		// (get) Token: 0x06003CD5 RID: 15573 RVA: 0x0009910A File Offset: 0x0009730A
		// (set) Token: 0x06003CD6 RID: 15574 RVA: 0x00099112 File Offset: 0x00097312
		public IReadOnlyList<PopupMenuItem> Items { get; private set; }

		// Token: 0x06003CD7 RID: 15575 RVA: 0x0009911C File Offset: 0x0009731C
		public PopupMenuLayer(Desktop uiManager, Element parent) : base(uiManager, null)
		{
			this._parent = parent;
			this._title = new Label(uiManager, this);
			this._title.Visible = false;
			this._itemsContainer = new Group(uiManager, this);
		}

		// Token: 0x06003CD8 RID: 15576 RVA: 0x0009917A File Offset: 0x0009737A
		public void SetTitle(string title)
		{
			this._title.Text = title;
			this._title.Visible = (title != null);
		}

		// Token: 0x06003CD9 RID: 15577 RVA: 0x0009919C File Offset: 0x0009739C
		public void SetItems(IReadOnlyList<PopupMenuItem> items)
		{
			this.Items = (items ?? new List<PopupMenuItem>());
			this._itemsContainer.Clear();
			foreach (PopupMenuItem popupMenuItem in this.Items)
			{
				bool flag = popupMenuItem.IconTexturePath != null;
				if (flag)
				{
					this.HasIcons = true;
				}
				this._itemsContainer.Add(new PopupMenuRow(this, popupMenuItem), -1);
			}
			this.ApplyStyles();
		}

		// Token: 0x06003CDA RID: 15578 RVA: 0x00099234 File Offset: 0x00097434
		private int ComputeHeight()
		{
			int num = this.Style.BaseHeight + this._itemsContainer.Children.Count * this.Style.RowHeight;
			bool visible = this._title.Visible;
			if (visible)
			{
				num += this.Style.RowHeight + 3;
			}
			return num;
		}

		// Token: 0x06003CDB RID: 15579 RVA: 0x00099290 File Offset: 0x00097490
		protected override void ApplyStyles()
		{
			this._layoutMode = LayoutMode.Top;
			this.Padding = new Padding(this.Style.Padding);
			this.Background = this.Style.Background;
			this.Anchor.MaxWidth = new int?(this.Style.MaxWidth);
			this.Anchor.Height = new int?(this.ComputeHeight());
			this._title.Style = this.Style.TitleStyle;
			this._title.Padding = this.Style.ItemPadding;
			this._title.Background = this.Style.TitleBackground;
			bool hasIcons = this.HasIcons;
			if (hasIcons)
			{
				this._title.Padding.Left = this.Desktop.UnscaleRound((float)this.Style.ItemIconSize) + this.Style.ItemPadding.Left * 2;
			}
			this._itemsContainer.LayoutMode = LayoutMode.Top;
			base.ApplyStyles();
		}

		// Token: 0x06003CDC RID: 15580 RVA: 0x000993E4 File Offset: 0x000975E4
		public override Element HitTest(Point position)
		{
			bool flag = !this._anchoredRectangle.Contains(position);
			Element result;
			if (flag)
			{
				result = this;
			}
			else
			{
				result = base.HitTest(position);
			}
			return result;
		}

		// Token: 0x06003CDD RID: 15581 RVA: 0x00099414 File Offset: 0x00097614
		protected override void OnMouseButtonDown(MouseButtonEvent evt)
		{
			this.Close();
			this.Desktop.RefreshHover();
			this.Desktop.OnMouseDown(evt.Button, evt.Clicks);
		}

		// Token: 0x06003CDE RID: 15582 RVA: 0x00099442 File Offset: 0x00097642
		protected internal override void Dismiss()
		{
			this.Close();
		}

		// Token: 0x06003CDF RID: 15583 RVA: 0x0009944C File Offset: 0x0009764C
		public void Open()
		{
			this.Anchor.Left = new int?(this.Desktop.UnscaleRound((float)this.Desktop.MousePosition.X));
			this.Anchor.Top = new int?(this.Desktop.UnscaleRound((float)this.Desktop.MousePosition.Y));
			float num = this.Desktop.Scale * (float)this.Anchor.MaxWidth.Value;
			bool flag = (float)this.Desktop.MousePosition.X + num > (float)this.Desktop.ViewportRectangle.Width;
			if (flag)
			{
				this.Anchor.Left = this.Anchor.Left - this.Anchor.MaxWidth;
			}
			int num2 = this.ComputeHeight();
			float num3 = (float)num2 * this.Desktop.Scale;
			bool flag2 = (float)this.Desktop.MousePosition.Y + num3 > (float)this.Desktop.ViewportRectangle.Height;
			if (flag2)
			{
				this.Anchor.Top = this.Anchor.Top - num2;
			}
			this.Desktop.SetTransientLayer(this);
		}

		// Token: 0x06003CE0 RID: 15584 RVA: 0x000995E5 File Offset: 0x000977E5
		public void Close()
		{
			this.Desktop.SetTransientLayer(null);
		}

		// Token: 0x04001C3E RID: 7230
		[UIMarkupProperty]
		public PopupMenuLayerStyle Style = new PopupMenuLayerStyle();

		// Token: 0x04001C3F RID: 7231
		private readonly Element _parent;

		// Token: 0x04001C40 RID: 7232
		private readonly Group _itemsContainer;

		// Token: 0x04001C41 RID: 7233
		private readonly Label _title;

		// Token: 0x04001C42 RID: 7234
		public bool HasIcons = false;

		// Token: 0x04001C43 RID: 7235
		public bool CloseOnActivate = true;
	}
}
