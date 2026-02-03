using System;
using HytaleClient.Interface.UI.Styles;

namespace HytaleClient.Interface.UI.Elements
{
	// Token: 0x0200086F RID: 2159
	internal class PopupMenuRow : Button
	{
		// Token: 0x06003CE1 RID: 15585 RVA: 0x000995F8 File Offset: 0x000977F8
		public PopupMenuRow(PopupMenuLayer layer, PopupMenuItem menuItem) : base(layer.Desktop, null)
		{
			this._layer = layer;
			this._menuItem = menuItem;
			bool flag = menuItem.IconTexturePath != null;
			if (flag)
			{
				this._icon = new Element(this.Desktop, this);
			}
			this._label = new Label(this.Desktop, this)
			{
				Text = menuItem.Label
			};
			this.Activating = delegate()
			{
				bool closeOnActivate = layer.CloseOnActivate;
				if (closeOnActivate)
				{
					layer.Close();
				}
				Action activating = menuItem.Activating;
				if (activating != null)
				{
					activating();
				}
			};
			this.Background = this._layer.Style.ItemBackground;
			BaseButtonStyle<Button.ButtonStyleState> style = this.Style;
			ButtonSounds itemSounds = this._layer.Style.ItemSounds;
			style.Sounds = (((itemSounds != null) ? itemSounds.Clone() : null) ?? new ButtonSounds());
			bool flag2 = menuItem.ActivateSound != null;
			if (flag2)
			{
				this.Style.Sounds.Activate = menuItem.ActivateSound;
			}
		}

		// Token: 0x06003CE2 RID: 15586 RVA: 0x00099714 File Offset: 0x00097914
		protected override void ApplyStyles()
		{
			this.Anchor.Height = new int?(this._layer.Style.RowHeight);
			this._label.Padding = this._layer.Style.ItemPadding;
			bool hasIcons = this._layer.HasIcons;
			if (hasIcons)
			{
				bool flag = this._icon == null;
				if (flag)
				{
					this._label.Padding.Left = this.Desktop.UnscaleRound((float)this._layer.Style.ItemIconSize) + this._layer.Style.ItemPadding.Left * 2;
				}
				else
				{
					base.LayoutMode = LayoutMode.Left;
					this._icon.Background = new PatchStyle
					{
						TexturePath = this._menuItem.IconTexturePath
					};
					this._icon.Anchor.Left = this._layer.Style.ItemPadding.Left;
					this._icon.Anchor.Top = new int?((this._layer.Style.RowHeight - this.Desktop.UnscaleRound((float)this._layer.Style.ItemIconSize)) / 2);
					this._icon.Anchor.Width = new int?(this.Desktop.UnscaleRound((float)this._layer.Style.ItemIconSize));
					this._icon.Anchor.Height = new int?(this.Desktop.UnscaleRound((float)this._layer.Style.ItemIconSize));
				}
			}
			this._label.Style = this._layer.Style.ItemLabelStyle;
			base.ApplyStyles();
		}

		// Token: 0x06003CE3 RID: 15587 RVA: 0x0009992D File Offset: 0x00097B2D
		protected override void OnMouseEnter()
		{
			this.Background = this._layer.Style.HoveredItemBackground;
			this.ApplyStyles();
		}

		// Token: 0x06003CE4 RID: 15588 RVA: 0x0009994D File Offset: 0x00097B4D
		protected override void OnMouseLeave()
		{
			this.Background = this._layer.Style.ItemBackground;
			this.ApplyStyles();
		}

		// Token: 0x06003CE5 RID: 15589 RVA: 0x00099970 File Offset: 0x00097B70
		protected override void OnMouseButtonDown(MouseButtonEvent evt)
		{
			bool flag = (long)evt.Button != 1L;
			if (!flag)
			{
				this.Background = this._layer.Style.PressedItemBackground;
				this.ApplyStyles();
			}
		}

		// Token: 0x06003CE6 RID: 15590 RVA: 0x000999B0 File Offset: 0x00097BB0
		protected override void OnMouseButtonUp(MouseButtonEvent evt, bool activate)
		{
			bool flag = (long)evt.Button != 1L;
			if (!flag)
			{
				this.Background = (activate ? this._layer.Style.HoveredItemBackground : this._layer.Style.ItemBackground);
				this.ApplyStyles();
				base.OnMouseButtonUp(evt, activate);
			}
		}

		// Token: 0x04001C45 RID: 7237
		private readonly PopupMenuLayer _layer;

		// Token: 0x04001C46 RID: 7238
		private readonly PopupMenuItem _menuItem;

		// Token: 0x04001C47 RID: 7239
		private readonly Element _icon;

		// Token: 0x04001C48 RID: 7240
		private readonly Label _label;
	}
}
