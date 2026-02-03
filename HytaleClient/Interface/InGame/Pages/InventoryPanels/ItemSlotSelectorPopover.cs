using System;
using System.Diagnostics;
using HytaleClient.Data.Items;
using HytaleClient.Interface.InGame.Hud;
using HytaleClient.Interface.UI;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Interface.UI.Styles;
using HytaleClient.Math;

namespace HytaleClient.Interface.InGame.Pages.InventoryPanels
{
	// Token: 0x0200089B RID: 2203
	internal class ItemSlotSelectorPopover : BaseItemSlotSelector
	{
		// Token: 0x170010B9 RID: 4281
		// (get) Token: 0x06003FA7 RID: 16295 RVA: 0x000B3A36 File Offset: 0x000B1C36
		// (set) Token: 0x06003FA8 RID: 16296 RVA: 0x000B3A3E File Offset: 0x000B1C3E
		public int InventorySectionId { get; private set; }

		// Token: 0x06003FA9 RID: 16297 RVA: 0x000B3A47 File Offset: 0x000B1C47
		public ItemSlotSelectorPopover(InGameView inGameView, Element parent) : base(inGameView, parent, true)
		{
			this._quantityFontSize = 16f;
		}

		// Token: 0x06003FAA RID: 16298 RVA: 0x000B3A70 File Offset: 0x000B1C70
		public void Build()
		{
			Document document;
			this.Interface.TryGetDocument("InGame/Pages/Inventory/ItemSlotSelectorPopover.ui", out document);
			base.Build(document);
			this.Anchor = this._containerBackgroundAnchor;
			document.TryResolveNamedValue<SoundStyle>(this.Desktop.Provider, "MouseDownSound", out this._mouseDownSound);
			document.TryResolveNamedValue<SoundStyle>(this.Desktop.Provider, "ItemMovedSound", out this.ItemMovedSound);
		}

		// Token: 0x06003FAB RID: 16299 RVA: 0x000B3AE0 File Offset: 0x000B1CE0
		protected override void OnUnmounted()
		{
			base.OnUnmounted();
			this._mouseDownSlotIndex = -1;
			bool flag = this._dragSlotIndex != -1;
			if (flag)
			{
				int dragSlotIndex = this._dragSlotIndex;
				this._dragSlotIndex = -1;
				this._inGameView.SetupDragAndDropItem(null);
				this.OnDragCancel(dragSlotIndex, this._pressedMouseButton);
				this.Desktop.ClearMouseDrag();
			}
			this._inGameView.InventoryPage.CharacterPanel.OnItemSlotSelectorClosed();
		}

		// Token: 0x06003FAC RID: 16300 RVA: 0x000B3B5C File Offset: 0x000B1D5C
		public void Setup(int sectionId, int activeSlot, int x, int y)
		{
			this.InventorySectionId = sectionId;
			this.Anchor.Left = new int?(x);
			this.Anchor.Top = new int?(y);
			this.SelectedSlot = activeSlot + 1;
			this._hoveredSlot = this.SelectedSlot;
			base.SetItemStacks(this._inGameView.GetItemStacks(sectionId));
		}

		// Token: 0x06003FAD RID: 16301 RVA: 0x000B3BC0 File Offset: 0x000B1DC0
		private void OnDragCancel(int slotIndex, int button)
		{
			this._inGameView.HandleInventoryDropItem(this.InventorySectionId, slotIndex, button);
			bool flag = base.IsMounted && this.HitTest(this.Desktop.MousePosition) != null;
			if (!flag)
			{
				base.Visible = false;
			}
		}

		// Token: 0x06003FAE RID: 16302 RVA: 0x000B3C10 File Offset: 0x000B1E10
		protected internal override void OnMouseDrop(object data, Element sourceElement, out bool accepted)
		{
			int num = this._hoveredSlot - 1;
			accepted = (num != -1 && data is ItemGrid.ItemDragData);
			bool flag = accepted;
			if (flag)
			{
				ItemGrid.ItemDragData itemDragData = (ItemGrid.ItemDragData)data;
				bool flag2 = (long)itemDragData.PressedMouseButton == 1L && this.Desktop.IsShiftKeyDown;
				if (flag2)
				{
					bool flag3 = itemDragData.ItemStack.Quantity > 1;
					if (flag3)
					{
						itemDragData.ItemStack.Quantity = (int)Math.Floor((double)((float)itemDragData.ItemStack.Quantity / 2f));
					}
				}
				else
				{
					bool flag4 = (long)itemDragData.PressedMouseButton == 3L;
					if (flag4)
					{
						itemDragData.ItemStack.Quantity = 1;
					}
				}
				this._inGameView.SetupDragAndDropItem(null);
				this._inGameView.HandleInventoryDragEnd(this, this.InventorySectionId, num, sourceElement, itemDragData);
			}
		}

		// Token: 0x06003FAF RID: 16303 RVA: 0x000B3CE8 File Offset: 0x000B1EE8
		protected internal override void OnMouseDragCancel(object data)
		{
			int dragSlotIndex = this._dragSlotIndex;
			this._dragSlotIndex = -1;
			this._inGameView.SetupDragAndDropItem(null);
			this.OnDragCancel(dragSlotIndex, this._pressedMouseButton);
		}

		// Token: 0x06003FB0 RID: 16304 RVA: 0x000B3D20 File Offset: 0x000B1F20
		protected internal override void OnMouseDragComplete(Element element, object data)
		{
			this._dragSlotIndex = -1;
			bool flag = this.HitTest(this.Desktop.MousePosition) != null;
			if (!flag)
			{
				base.Visible = false;
			}
		}

		// Token: 0x06003FB1 RID: 16305 RVA: 0x000B3D58 File Offset: 0x000B1F58
		protected override void OnMouseButtonUp(MouseButtonEvent evt, bool activate)
		{
			bool flag = !this._wasDragging && activate && this._dragSlotIndex == -1;
			if (flag)
			{
				int num = this._hoveredSlot - 1;
				bool flag2 = num != -1 && num == this._mouseDownSlotIndex;
				if (flag2)
				{
					this._inGameView.HandleInventoryClick(this.InventorySectionId, num, evt.Button);
				}
			}
		}

		// Token: 0x06003FB2 RID: 16306 RVA: 0x000B3DC0 File Offset: 0x000B1FC0
		protected override void OnMouseButtonDown(MouseButtonEvent evt)
		{
			this._mouseDownSlotIndex = this._hoveredSlot - 1;
			this._pressedMouseButton = evt.Button;
			this._wasDragging = false;
			bool flag = this._mouseDownSlotIndex != -1;
			if (flag)
			{
				this._mouseDownPosition = this.Desktop.MousePosition;
			}
			bool flag2 = this._mouseDownSound != null;
			if (flag2)
			{
				this.Desktop.Provider.PlaySound(this._mouseDownSound);
			}
		}

		// Token: 0x06003FB3 RID: 16307 RVA: 0x000B3E36 File Offset: 0x000B2036
		protected internal override void OnMouseDragMove()
		{
			base.RefreshHoveredSlot();
		}

		// Token: 0x06003FB4 RID: 16308 RVA: 0x000B3E40 File Offset: 0x000B2040
		protected override void OnMouseMove()
		{
			base.OnMouseMove();
			bool flag = base.CapturedMouseButton != null && this._mouseDownSlotIndex != -1 && !this.Desktop.IsMouseDragging && !this._wasDragging;
			if (flag)
			{
				float num = new Vector2((float)(this.Desktop.MousePosition.X - this._mouseDownPosition.X), (float)(this.Desktop.MousePosition.Y - this._mouseDownPosition.Y)).Length();
				bool flag2 = num >= 3f;
				if (flag2)
				{
					ClientItemStack clientItemStack = this._itemStacks[this._mouseDownSlotIndex];
					bool flag3 = clientItemStack == null;
					if (!flag3)
					{
						this._wasDragging = true;
						this._dragSlotIndex = this._mouseDownSlotIndex;
						ClientItemStack itemStack = new ClientItemStack(clientItemStack.Id, clientItemStack.Quantity)
						{
							Metadata = clientItemStack.Metadata
						};
						ItemGrid.ItemDragData itemDragData = new ItemGrid.ItemDragData(this._pressedMouseButton, this._mouseDownSlotIndex, itemStack, new int?(this.InventorySectionId), this._mouseDownSlotIndex);
						this._inGameView.SetupDragAndDropItem(itemDragData);
						this.Desktop.StartMouseDrag(itemDragData, this, null);
					}
				}
			}
		}

		// Token: 0x06003FB5 RID: 16309 RVA: 0x000B3F84 File Offset: 0x000B2184
		protected override void OnMouseLeave()
		{
			bool flag = this._dragSlotIndex != -1;
			if (!flag)
			{
				base.Visible = false;
			}
		}

		// Token: 0x06003FB6 RID: 16310 RVA: 0x000B3FAC File Offset: 0x000B21AC
		protected internal override void OnMouseDragExit(object data, Element sourceElement)
		{
			this.OnMouseLeave();
		}

		// Token: 0x06003FB7 RID: 16311 RVA: 0x000B3FB8 File Offset: 0x000B21B8
		public override Element HitTest(Point position)
		{
			Debug.Assert(base.IsMounted);
			float num = (float)base.AnchoredRectangle.Width / 2f;
			Point point = new Point(position.X - base.AnchoredRectangle.Left - base.AnchoredRectangle.Width / 2, position.Y - base.AnchoredRectangle.Top - base.AnchoredRectangle.Height / 2);
			bool flag = (double)(point.X * point.X) + (double)(point.Y * point.Y) > (double)(num * num);
			Element result;
			if (flag)
			{
				result = null;
			}
			else
			{
				result = (base.HitTest(position) ?? this);
			}
			return result;
		}

		// Token: 0x04001E3C RID: 7740
		private int _dragSlotIndex = -1;

		// Token: 0x04001E3D RID: 7741
		private int _mouseDownSlotIndex = -1;

		// Token: 0x04001E3E RID: 7742
		private int _pressedMouseButton;

		// Token: 0x04001E3F RID: 7743
		private Point _mouseDownPosition;

		// Token: 0x04001E40 RID: 7744
		private bool _wasDragging;

		// Token: 0x04001E41 RID: 7745
		private SoundStyle _mouseDownSound;

		// Token: 0x04001E42 RID: 7746
		public SoundStyle ItemMovedSound;
	}
}
