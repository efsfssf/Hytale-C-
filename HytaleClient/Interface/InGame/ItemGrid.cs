using System;
using System.Collections.Generic;
using System.Diagnostics;
using HytaleClient.Data.Items;
using HytaleClient.Graphics;
using HytaleClient.Graphics.Fonts;
using HytaleClient.InGame.Modules.BuilderTools.Tools;
using HytaleClient.Interface.UI;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Interface.UI.Styles;
using HytaleClient.Math;
using HytaleClient.Protocol;
using HytaleClient.Utils;

namespace HytaleClient.Interface.InGame
{
	// Token: 0x02000886 RID: 2182
	[UIMarkupElement]
	internal class ItemGrid : Element
	{
		// Token: 0x170010A1 RID: 4257
		// (set) Token: 0x06003E48 RID: 15944 RVA: 0x000A4C78 File Offset: 0x000A2E78
		[UIMarkupProperty]
		public ScrollbarStyle ScrollbarStyle
		{
			set
			{
				this._scrollbarStyle = value;
			}
		}

		// Token: 0x170010A2 RID: 4258
		// (set) Token: 0x06003E49 RID: 15945 RVA: 0x000A4C81 File Offset: 0x000A2E81
		[UIMarkupProperty]
		public ClientItemStack[] ItemStacks
		{
			set
			{
				this.Slots = new ItemGridSlot[value.Length];
				this.SetItemStacks(value, 0);
			}
		}

		// Token: 0x06003E4A RID: 15946 RVA: 0x000A4C9C File Offset: 0x000A2E9C
		public ItemGrid(Desktop desktop, Element parent) : base(desktop, parent)
		{
			IUIProvider provider = desktop.Provider;
			IUIProvider iuiprovider = provider;
			CustomUIProvider customUIProvider = iuiprovider as CustomUIProvider;
			if (customUIProvider == null)
			{
				Interface @interface = iuiprovider as Interface;
				if (@interface == null)
				{
					throw new Exception("IUIProvider must be of type CustomUIProvider or Interface");
				}
				this._inGameView = @interface.InGameView;
			}
			else
			{
				this._inGameView = customUIProvider.Interface.InGameView;
			}
			this._tooltip = new ItemTooltipLayer(this._inGameView)
			{
				ShowDelay = 0.1f
			};
		}

		// Token: 0x06003E4B RID: 15947 RVA: 0x000A4D88 File Offset: 0x000A2F88
		protected override void OnUnmounted()
		{
			this._quantityPopupSlotIndex = -1;
			bool flag = this._mouseOverSlotIndex != -1;
			if (flag)
			{
				Action<int> slotMouseExited = this.SlotMouseExited;
				if (slotMouseExited != null)
				{
					slotMouseExited(this._mouseDownSlotIndex);
				}
				this._mouseDownSlotIndex = -1;
			}
			bool flag2 = this._dragSlotIndex != -1;
			if (flag2)
			{
				this.Desktop.CancelMouseDrag();
			}
			else
			{
				bool isQuantityPopupOpen = this._isQuantityPopupOpen;
				if (isQuantityPopupOpen)
				{
					this._isQuantityPopupOpen = false;
					this._quantityPopupSlotIndex = -1;
					this._inGameView.SetupDragAndDropItem(null);
					this.Desktop.SetTransientLayer(null);
				}
			}
		}

		// Token: 0x06003E4C RID: 15948 RVA: 0x000A4E24 File Offset: 0x000A3024
		protected override void ApplyStyles()
		{
			this._regularFont = this.Desktop.Provider.GetFontFamily("Default").RegularFont;
			this._boldFont = this.Desktop.Provider.GetFontFamily("Default").BoldFont;
			this._blockSlotBackgroundPatch = this.Desktop.MakeTexturePatch((this.Desktop.Provider is CustomUIProvider) ? new PatchStyle("Common/SlotBlock.png") : new PatchStyle("InGame/Pages/Inventory/SlotBlock.png"));
			this._infoPaneBackgroundPatch = this.Desktop.MakeTexturePatch((this.Desktop.Provider is CustomUIProvider) ? new PatchStyle("Common/SlotInfoPane.png") : new PatchStyle("InGame/Pages/Inventory/SlotInfoPane.png"));
			this._slotBackgroundPatches.Clear();
			ClientItemQuality[] itemQualities = this._inGameView.InGame.Instance.ServerSettings.ItemQualities;
			for (int i = 0; i < itemQualities.Length; i++)
			{
				ClientItemQuality clientItemQuality = itemQualities[i];
				TextureArea textureArea;
				bool flag = this._inGameView.TryMountAssetTexture(clientItemQuality.SlotTexture, out textureArea);
				if (flag)
				{
					this._slotBackgroundPatches[i] = this.Desktop.MakeTexturePatch(new PatchStyle(textureArea));
				}
				else
				{
					this._slotBackgroundPatches[i] = this.Desktop.MakeTexturePatch(new PatchStyle(this.Desktop.Provider.MissingTexture));
				}
				TextureArea textureArea2;
				bool flag2 = this._inGameView.TryMountAssetTexture(clientItemQuality.SpecialSlotTexture, out textureArea2);
				if (flag2)
				{
					this._specialSlotBackgroundPatches[i] = this.Desktop.MakeTexturePatch(new PatchStyle(textureArea2));
				}
				else
				{
					this._specialSlotBackgroundPatches[i] = this.Desktop.MakeTexturePatch(new PatchStyle(this.Desktop.Provider.MissingTexture));
				}
			}
			this._slotBackgroundPatch = ((this.Style.SlotBackground != null) ? this.Desktop.MakeTexturePatch(this.Style.SlotBackground) : null);
			this._quantityPopupSlotOverlayPatch = ((this.Style.QuantityPopupSlotOverlay != null) ? this.Desktop.MakeTexturePatch(this.Style.QuantityPopupSlotOverlay) : null);
			this._brokenSlotBackgroundOverlayPatch = ((this.Style.BrokenSlotBackgroundOverlay != null) ? this.Desktop.MakeTexturePatch(this.Style.BrokenSlotBackgroundOverlay) : null);
			this._brokenSlotIconOverlayPatch = ((this.Style.BrokenSlotIconOverlay != null) ? this.Desktop.MakeTexturePatch(this.Style.BrokenSlotIconOverlay) : null);
			this._defaultItemIconPatch = ((this.Style.DefaultItemIcon != null) ? this.Desktop.MakeTexturePatch(this.Style.DefaultItemIcon) : null);
			this._durabilityBarBackgroundTexture = ((this.Style.DurabilityBarBackground != null) ? this.Desktop.MakeTexturePatch(this.Style.DurabilityBarBackground) : null);
			this._durabilityBarTexture = ((this.Style.DurabilityBar != null) ? this.Desktop.Provider.MakeTextureArea(this.Style.DurabilityBar.Value) : null);
			foreach (ItemGridSlot itemGridSlot in this.Slots)
			{
				if (itemGridSlot != null)
				{
					itemGridSlot.ApplyStyles(this._inGameView, this.Desktop);
				}
			}
			base.ApplyStyles();
		}

		// Token: 0x06003E4D RID: 15949 RVA: 0x000A5184 File Offset: 0x000A3384
		public override Point ComputeScaledMinSize(int? maxWidth, int? maxHeight)
		{
			Point zero = Point.Zero;
			int num = this.Desktop.ScaleRound((float)(this._scrollbarStyle.Size + this._scrollbarStyle.Spacing));
			int itemsPerRow = this.GetItemsPerRow();
			bool flag = this.Anchor.Height != null;
			if (flag)
			{
				zero.Y = this.Desktop.ScaleRound((float)this.Anchor.Height.Value);
			}
			else
			{
				int num2 = (int)Math.Ceiling((double)((float)this.Slots.Length / (float)itemsPerRow));
				zero.Y = this.Desktop.ScaleRound((float)(this.Style.SlotSize * num2 + this.Style.SlotSpacing * (num2 - 1)));
				bool flag2 = this._layoutMode == LayoutMode.TopScrolling || this._layoutMode == LayoutMode.BottomScrolling;
				if (flag2)
				{
					zero.X += num;
				}
				bool flag3 = maxHeight != null;
				if (flag3)
				{
					zero.Y = Math.Min(zero.Y, maxHeight.Value);
				}
			}
			bool flag4 = this.Anchor.Width != null;
			if (flag4)
			{
				zero.X = this.Desktop.ScaleRound((float)this.Anchor.Width.Value);
			}
			else
			{
				zero.X = this.Desktop.ScaleRound((float)(this.Style.SlotSize * itemsPerRow + this.Style.SlotSpacing * (itemsPerRow - 1)));
				bool flag5 = this._layoutMode == LayoutMode.LeftScrolling || this._layoutMode == LayoutMode.RightScrolling;
				if (flag5)
				{
					zero.Y += num;
				}
				bool flag6 = maxWidth != null;
				if (flag6)
				{
					zero.X = Math.Min(zero.X, maxWidth.Value);
				}
			}
			bool flag7 = this.Padding.Horizontal != null;
			if (flag7)
			{
				zero.X += this.Desktop.ScaleRound((float)this.Padding.Horizontal.Value);
			}
			bool flag8 = this.Padding.Vertical != null;
			if (flag8)
			{
				zero.Y += this.Desktop.ScaleRound((float)this.Padding.Vertical.Value);
			}
			return zero;
		}

		// Token: 0x06003E4E RID: 15950 RVA: 0x000A53F0 File Offset: 0x000A35F0
		protected override void LayoutSelf()
		{
			bool showScrollbar = this.ShowScrollbar;
			if (showScrollbar)
			{
				int num = (int)Math.Ceiling((double)((float)this.Slots.Length / (float)this.GetItemsPerRow()));
				this.ContentHeight = new int?(num * this.Style.SlotSize + (num - 1) * this.Style.SlotSpacing);
			}
			else
			{
				this.ContentHeight = null;
			}
			this.RefreshMouseOver(true);
		}

		// Token: 0x06003E4F RID: 15951 RVA: 0x000A5464 File Offset: 0x000A3664
		public override Element HitTest(Point position)
		{
			Debug.Assert(base.IsMounted);
			return this._anchoredRectangle.Contains(position) ? this : null;
		}

		// Token: 0x06003E50 RID: 15952 RVA: 0x000A5494 File Offset: 0x000A3694
		public int GetItemsPerRow()
		{
			bool flag = this.InfoDisplay != 1;
			int result;
			if (flag)
			{
				result = this.SlotsPerRow;
			}
			else
			{
				bool flag2 = this.SlotsPerRow < 3;
				if (flag2)
				{
					throw new Exception("ItemGrid.SlotsPerRow cannot be less than 3 when using InfoDisplayMode.Adjacent");
				}
				bool flag3 = this.SlotsPerRow % 3 == 0;
				if (flag3)
				{
					result = this.SlotsPerRow / 3;
				}
				else
				{
					result = ((this.SlotsPerRow > 10) ? (this.SlotsPerRow / 4) : (this.SlotsPerRow / 2));
				}
			}
			return result;
		}

		// Token: 0x06003E51 RID: 15953 RVA: 0x000A5510 File Offset: 0x000A3710
		private int GetSlotIndexAtMousePosition()
		{
			int itemsPerRow = this.GetItemsPerRow();
			float num = this.Desktop.ScaleNoRound((float)this.Style.SlotSize) + this.Desktop.ScaleNoRound((float)this.Style.SlotSpacing);
			float num2 = (this.InfoDisplay == 1) ? (num * (float)itemsPerRow) : num;
			int num3 = (int)((float)(this.Desktop.MousePosition.X - this._rectangleAfterPadding.X) / num2);
			bool flag = num3 >= itemsPerRow;
			int result;
			if (flag)
			{
				result = -1;
			}
			else
			{
				int num4 = (int)((float)(this.Desktop.MousePosition.Y - this._rectangleAfterPadding.Y + base.ScaledScrollOffset.Y) / num);
				int num5 = num4 * itemsPerRow + num3;
				bool flag2 = num5 >= this.Slots.Length || num5 < 0;
				if (flag2)
				{
					result = -1;
				}
				else
				{
					result = num5;
				}
			}
			return result;
		}

		// Token: 0x06003E52 RID: 15954 RVA: 0x000A55F8 File Offset: 0x000A37F8
		private Point GetSlotCoordinatesByIndex(int index)
		{
			int itemsPerRow = this.GetItemsPerRow();
			int num = index % itemsPerRow;
			bool flag = index > 0 && this.InfoDisplay == 1;
			if (flag)
			{
				num *= itemsPerRow;
			}
			int y = index / itemsPerRow;
			return new Point(num, y);
		}

		// Token: 0x06003E53 RID: 15955 RVA: 0x000A563C File Offset: 0x000A383C
		private Point GetSlotCenterPointByIndex(int index)
		{
			Point slotCoordinatesByIndex = this.GetSlotCoordinatesByIndex(index);
			int slotSize = this.Style.SlotSize;
			int slotIconSize = this.Style.SlotIconSize;
			int slotSpacing = this.Style.SlotSpacing;
			int num = this.Desktop.ScaleRound((float)((slotSize - slotIconSize) / 2));
			int num2 = this.Desktop.ScaleRound((float)slotIconSize);
			int x = this._rectangleAfterPadding.X + this.Desktop.ScaleRound((float)(slotCoordinatesByIndex.X * (slotSize + slotSpacing))) + num + num2 / 2;
			int y = this._rectangleAfterPadding.Y + this.Desktop.ScaleRound((float)(slotCoordinatesByIndex.Y * (slotSize + slotSpacing))) + num + num2 / 2 - this._scaledScrollOffset.Y;
			return new Point(x, y);
		}

		// Token: 0x06003E54 RID: 15956 RVA: 0x000A570C File Offset: 0x000A390C
		protected override void OnMouseButtonDown(MouseButtonEvent evt)
		{
			this._wasDragging = false;
			int slotIndexAtMousePosition = this.GetSlotIndexAtMousePosition();
			bool flag = this.SlotMouseDown != null;
			if (flag)
			{
				bool flag2 = !this.SlotMouseDown(slotIndexAtMousePosition);
				if (flag2)
				{
					return;
				}
			}
			this._mouseDownSlotIndex = slotIndexAtMousePosition;
			this._pressedMouseButton = evt.Button;
			bool flag3 = this._mouseDownSlotIndex == -1;
			if (!flag3)
			{
				this._mouseDownPosition = this.Desktop.MousePosition;
				bool flag4;
				if (this.Style.ItemStackMouseDownSound != null)
				{
					ItemGridSlot itemGridSlot = this.Slots[slotIndexAtMousePosition];
					flag4 = (((itemGridSlot != null) ? itemGridSlot.ItemStack : null) != null);
				}
				else
				{
					flag4 = false;
				}
				bool flag5 = flag4;
				if (flag5)
				{
					this.Desktop.Provider.PlaySound(this.Style.ItemStackMouseDownSound);
				}
				bool flag6;
				if ((long)evt.Button == 2L)
				{
					ItemGridSlot itemGridSlot2 = this.Slots[slotIndexAtMousePosition];
					flag6 = (((itemGridSlot2 != null) ? itemGridSlot2.ItemStack : null) != null);
				}
				else
				{
					flag6 = false;
				}
				bool flag7 = flag6;
				if (flag7)
				{
					InGameView inGameView = this._inGameView;
					ItemGridSlot itemGridSlot3 = this.Slots[slotIndexAtMousePosition];
					inGameView.checkForSettingBrush((itemGridSlot3 != null) ? itemGridSlot3.ItemStack.Id : null);
				}
			}
		}

		// Token: 0x06003E55 RID: 15957 RVA: 0x000A5820 File Offset: 0x000A3A20
		protected internal override void OnMouseDragMove()
		{
			this.RefreshMouseOver(false);
		}

		// Token: 0x06003E56 RID: 15958 RVA: 0x000A582C File Offset: 0x000A3A2C
		protected override void OnMouseMove()
		{
			this.RefreshMouseOver(false);
			bool flag = !this.AreItemsDraggable || base.CapturedMouseButton == null || this._mouseDownSlotIndex == -1 || this.Desktop.IsMouseDragging || this._wasDragging;
			if (!flag)
			{
				bool flag2 = (long)this._pressedMouseButton == 2L && this._inGameView.canSetActiveBrushMaterial();
				if (!flag2)
				{
					float num = new Vector2((float)(this.Desktop.MousePosition.X - this._mouseDownPosition.X), (float)(this.Desktop.MousePosition.Y - this._mouseDownPosition.Y)).Length();
					bool flag3 = num < 3f;
					if (!flag3)
					{
						ItemGridSlot itemGridSlot = this.Slots[this._mouseDownSlotIndex];
						bool flag4 = ((itemGridSlot != null) ? itemGridSlot.ItemStack : null) == null;
						if (!flag4)
						{
							this._wasDragging = true;
							this._dragSlotIndex = this._mouseDownSlotIndex;
							ClientItemStack itemStack = this.Slots[this._mouseDownSlotIndex].ItemStack;
							ClientItemBase clientItemBase = this._inGameView.Items[itemStack.Id];
							int quantity = this.AllowMaxStackDraggableItems ? clientItemBase.MaxStack : itemStack.Quantity;
							ClientItemStack itemStack2 = new ClientItemStack(itemStack.Id, quantity)
							{
								Metadata = itemStack.Metadata
							};
							this._tooltip.Stop();
							ItemGrid.ItemDragData itemDragData = new ItemGrid.ItemDragData(this._pressedMouseButton, this._mouseDownSlotIndex, itemStack2, this.InventorySectionId, itemGridSlot.InventorySlotIndex ?? this._mouseDownSlotIndex);
							this._inGameView.SetupDragAndDropItem(itemDragData);
							this.Desktop.StartMouseDrag(itemDragData, this, null);
						}
					}
				}
			}
		}

		// Token: 0x06003E57 RID: 15959 RVA: 0x000A5A02 File Offset: 0x000A3C02
		protected override void OnMouseEnter()
		{
			this.RefreshMouseOver(false);
		}

		// Token: 0x06003E58 RID: 15960 RVA: 0x000A5A0C File Offset: 0x000A3C0C
		protected override void OnMouseLeave()
		{
			this._slotIndexForDoubleClick = -1;
			this.RefreshMouseOver(false);
		}

		// Token: 0x06003E59 RID: 15961 RVA: 0x000A5A20 File Offset: 0x000A3C20
		protected internal override void OnMouseDrop(object data, Element draggedElement, out bool accepted)
		{
			int slotIndex = this.GetSlotIndexAtMousePosition();
			accepted = (slotIndex != -1 && data is ItemGrid.ItemDragData);
			bool flag = accepted;
			if (flag)
			{
				ItemGrid.ItemDragData itemDragData = (ItemGrid.ItemDragData)data;
				bool flag2 = (long)itemDragData.PressedMouseButton == 3L && this.Desktop.IsShiftKeyDown;
				bool flag3 = draggedElement == this && slotIndex == itemDragData.ItemGridIndex;
				bool flag4 = flag2 && itemDragData.ItemStack.Quantity > 1 && !flag3;
				if (flag4)
				{
					Point slotCenterPointByIndex = this.GetSlotCenterPointByIndex(slotIndex);
					slotCenterPointByIndex.X = this.Desktop.UnscaleRound((float)slotCenterPointByIndex.X);
					slotCenterPointByIndex.Y = this.Desktop.UnscaleRound((float)slotCenterPointByIndex.Y) - this.Style.SlotSize / 2;
					this._quantityPopupSlotIndex = slotIndex;
					this._isQuantityPopupOpen = true;
					int startingQuantity = 1;
					bool flag5 = itemDragData.ItemStack.Quantity > 1;
					if (flag5)
					{
						startingQuantity = (int)Math.Floor((double)((float)itemDragData.ItemStack.Quantity / 2f));
					}
					this._inGameView.ItemQuantityPopup.Open(slotCenterPointByIndex, itemDragData.ItemStack.Quantity, startingQuantity, itemDragData.ItemStack.Id, delegate(int quantity)
					{
						this._isQuantityPopupOpen = false;
						this._quantityPopupSlotIndex = -1;
						this._inGameView.SetupDragAndDropItem(null);
						bool flag8 = quantity == 0;
						if (!flag8)
						{
							itemDragData.ItemStack.Quantity = quantity;
							Action<int, Element, ItemGrid.ItemDragData> dropped2 = this.Dropped;
							if (dropped2 != null)
							{
								dropped2(slotIndex, draggedElement, itemDragData);
							}
						}
					});
					this._inGameView.SetupCursorFloatingItem();
				}
				else
				{
					bool isShiftKeyDown = this.Desktop.IsShiftKeyDown;
					if (isShiftKeyDown)
					{
						bool flag6 = itemDragData.ItemStack.Quantity > 1;
						if (flag6)
						{
							itemDragData.ItemStack.Quantity = (int)Math.Floor((double)((float)itemDragData.ItemStack.Quantity / 2f));
						}
					}
					else
					{
						bool flag7 = (long)itemDragData.PressedMouseButton == 3L;
						if (flag7)
						{
							itemDragData.ItemStack.Quantity = 1;
						}
					}
					this._inGameView.SetupDragAndDropItem(null);
					Action<int, Element, ItemGrid.ItemDragData> dropped = this.Dropped;
					if (dropped != null)
					{
						dropped(slotIndex, draggedElement, itemDragData);
					}
				}
			}
		}

		// Token: 0x06003E5A RID: 15962 RVA: 0x000A5CC0 File Offset: 0x000A3EC0
		protected internal override void OnMouseDragCancel(object data)
		{
			int dragSlotIndex = this._dragSlotIndex;
			this._dragSlotIndex = -1;
			this._inGameView.SetupDragAndDropItem(null);
			Action<int, int> dragCancelled = this.DragCancelled;
			if (dragCancelled != null)
			{
				dragCancelled(dragSlotIndex, this._pressedMouseButton);
			}
		}

		// Token: 0x06003E5B RID: 15963 RVA: 0x000A5D02 File Offset: 0x000A3F02
		protected internal override void OnMouseDragComplete(Element element, object data)
		{
			Action<int, Element, ItemGrid.ItemDragData> slotMouseDragCompleted = this.SlotMouseDragCompleted;
			if (slotMouseDragCompleted != null)
			{
				slotMouseDragCompleted(this._dragSlotIndex, element, (ItemGrid.ItemDragData)data);
			}
			this._dragSlotIndex = -1;
		}

		// Token: 0x06003E5C RID: 15964 RVA: 0x000A5D2B File Offset: 0x000A3F2B
		protected internal override void OnMouseDragEnter(object data, Element sourceElement)
		{
			this._isMouseDraggingOver = true;
		}

		// Token: 0x06003E5D RID: 15965 RVA: 0x000A5D34 File Offset: 0x000A3F34
		protected internal override void OnMouseDragExit(object data, Element sourceElement)
		{
			this._isMouseDraggingOver = false;
			Action<int, int> slotMouseDragExited = this.SlotMouseDragExited;
			if (slotMouseDragExited != null)
			{
				slotMouseDragExited(this._dragSlotIndex, this._mouseOverSlotIndex);
			}
		}

		// Token: 0x06003E5E RID: 15966 RVA: 0x000A5D5C File Offset: 0x000A3F5C
		public void SetItemStacks(ClientItemStack[] itemStacks, int slotOffset = 0)
		{
			for (int i = 0; i < this.Slots.Length; i++)
			{
				int num = i + slotOffset;
				bool flag = num >= itemStacks.Length;
				if (flag)
				{
					break;
				}
				ClientItemStack clientItemStack = itemStacks[num];
				bool flag2 = clientItemStack != null;
				if (flag2)
				{
					ClientItemBase clientItemBase = this._inGameView.Items[clientItemStack.Id];
					BuilderTool builderTool = (clientItemBase != null) ? clientItemBase.BuilderTool : null;
					this.Slots[i] = new ItemGridSlot
					{
						ItemStack = clientItemStack,
						InventorySlotIndex = new int?(i + slotOffset),
						Name = this.Desktop.Provider.GetText((builderTool != null) ? ("builderTools.tools." + builderTool.Id + ".name") : ("items." + clientItemStack.Id + ".name"), null, true),
						Description = this.Desktop.Provider.GetText((builderTool != null) ? ("builderTools.tools." + builderTool.Id + ".description") : ("items." + clientItemStack.Id + ".description"), null, true)
					};
				}
				else
				{
					this.Slots[i] = null;
				}
			}
		}

		// Token: 0x06003E5F RID: 15967 RVA: 0x000A5E9C File Offset: 0x000A409C
		public void RefreshMouseOver(bool forceTooltipRefresh = false)
		{
			int mouseOverSlotIndex = this._mouseOverSlotIndex;
			this._mouseOverSlotIndex = (((base.IsHovered || this._isMouseDraggingOver) && this._inGameView.Items != null) ? this.GetSlotIndexAtMousePosition() : -1);
			bool flag;
			if (mouseOverSlotIndex != this._mouseOverSlotIndex && this._mouseOverSlotIndex > 0)
			{
				ItemGridSlot itemGridSlot = this.Slots[this._mouseOverSlotIndex];
				flag = (((itemGridSlot != null) ? itemGridSlot.ItemStack : null) != null);
			}
			else
			{
				flag = false;
			}
			bool flag2 = flag;
			if (flag2)
			{
				this._inGameView.InGame.Instance.AudioModule.PlayLocalSoundEvent("UI_BUTTONSLIGHT_HOVER");
			}
			bool flag3 = this._mouseOverSlotIndex != mouseOverSlotIndex;
			if (flag3)
			{
				bool flag4 = this._mouseOverSlotIndex != -1;
				if (flag4)
				{
					bool flag5 = mouseOverSlotIndex != -1;
					if (flag5)
					{
						Action<int> slotMouseExited = this.SlotMouseExited;
						if (slotMouseExited != null)
						{
							slotMouseExited(mouseOverSlotIndex);
						}
					}
					Action<int> slotMouseEntered = this.SlotMouseEntered;
					if (slotMouseEntered != null)
					{
						slotMouseEntered(this._mouseOverSlotIndex);
					}
				}
				else
				{
					Action<int> slotMouseExited2 = this.SlotMouseExited;
					if (slotMouseExited2 != null)
					{
						slotMouseExited2(mouseOverSlotIndex);
					}
				}
			}
			bool flag6 = this.InfoDisplay == null && (forceTooltipRefresh || this._mouseOverSlotIndex != mouseOverSlotIndex) && !this._isMouseDraggingOver;
			if (flag6)
			{
				bool flag7 = this._mouseOverSlotIndex != -1;
				if (flag7)
				{
					ItemGridSlot itemGridSlot2 = this.Slots[this._mouseOverSlotIndex];
					Point slotCenterPointByIndex = this.GetSlotCenterPointByIndex(this._mouseOverSlotIndex);
					bool flag8 = ((itemGridSlot2 != null) ? itemGridSlot2.Name : null) != null;
					if (flag8)
					{
						ItemTooltipLayer tooltip = this._tooltip;
						Point centerPoint = slotCenterPointByIndex;
						ClientItemStack stack = null;
						string name = itemGridSlot2.Name;
						string description = itemGridSlot2.Description;
						ClientItemStack itemStack = itemGridSlot2.ItemStack;
						tooltip.UpdateTooltip(centerPoint, stack, name, description, (itemStack != null) ? itemStack.Id : null);
						this._tooltip.Start(false);
					}
					else
					{
						bool flag9 = ((itemGridSlot2 != null) ? itemGridSlot2.ItemStack : null) != null;
						if (flag9)
						{
							this._tooltip.UpdateTooltip(slotCenterPointByIndex, itemGridSlot2.ItemStack, null, null, null);
							this._tooltip.Start(false);
						}
						else
						{
							this._tooltip.Stop();
						}
					}
				}
				else
				{
					this._tooltip.Stop();
				}
			}
		}

		// Token: 0x06003E60 RID: 15968 RVA: 0x000A60BC File Offset: 0x000A42BC
		protected override void OnMouseButtonUp(MouseButtonEvent evt, bool activate)
		{
			bool flag = (long)evt.Button == 2L && this._inGameView.canSetActiveBrushMaterial();
			if (!flag)
			{
				bool flag2 = !this._wasDragging && activate && this._dragSlotIndex == -1;
				if (flag2)
				{
					int slotIndexAtMousePosition = this.GetSlotIndexAtMousePosition();
					bool flag3 = slotIndexAtMousePosition != -1 && slotIndexAtMousePosition == this._mouseDownSlotIndex;
					if (flag3)
					{
						bool flag4 = this.SlotDoubleClicking != null && (long)evt.Button == 1L && evt.Clicks == 2 && this._slotIndexForDoubleClick == slotIndexAtMousePosition;
						if (flag4)
						{
							this.SlotDoubleClicking(slotIndexAtMousePosition);
						}
						else
						{
							this._slotIndexForDoubleClick = slotIndexAtMousePosition;
							ItemGridSlot itemGridSlot = this.Slots[slotIndexAtMousePosition];
							bool flag5 = itemGridSlot == null || itemGridSlot.IsActivatable;
							if (flag5)
							{
								ItemGridSlot itemGridSlot2 = this.Slots[slotIndexAtMousePosition];
								bool flag6 = ((itemGridSlot2 != null) ? itemGridSlot2.ItemStack : null) != null && this.Style.ItemStackActivateSound != null;
								if (flag6)
								{
									this.Desktop.Provider.PlaySound(this.Style.ItemStackActivateSound);
								}
								Action<int, int> slotClicking = this.SlotClicking;
								if (slotClicking != null)
								{
									slotClicking(slotIndexAtMousePosition, evt.Button);
								}
							}
						}
					}
					else
					{
						this._slotIndexForDoubleClick = -1;
					}
				}
			}
		}

		// Token: 0x06003E61 RID: 15969 RVA: 0x000A6208 File Offset: 0x000A4408
		protected override void PrepareForDrawSelf()
		{
			base.PrepareForDrawSelf();
			int slotSize = this.Style.SlotSize;
			int slotIconSize = this.Style.SlotIconSize;
			int slotSpacing = this.Style.SlotSpacing;
			int num = this.Desktop.ScaleRound((float)(slotSize - slotIconSize) / 2f);
			int num2 = this.Desktop.ScaleRound((float)slotIconSize);
			int num3 = this.Desktop.ScaleRound((float)slotSize);
			Anchor durabilityBarAnchor = this.Style.DurabilityBarAnchor;
			ColorHsva color = ColorHsva.FromUInt32Color(this.Style.DurabilityBarColorStart);
			ColorHsva color2 = ColorHsva.FromUInt32Color(this.Style.DurabilityBarColorEnd);
			bool showScrollbar = this.ShowScrollbar;
			if (showScrollbar)
			{
				this.Desktop.Batcher2D.PushScissor(this._rectangleAfterPadding);
			}
			for (int i = 0; i < this.Slots.Length; i++)
			{
				Point slotCoordinatesByIndex = this.GetSlotCoordinatesByIndex(i);
				int num4 = this._rectangleAfterPadding.Y + this.Desktop.ScaleRound((float)(slotCoordinatesByIndex.Y * (slotSize + slotSpacing))) - this._scaledScrollOffset.Y;
				bool flag = num4 > this._rectangleAfterPadding.Bottom;
				if (flag)
				{
					break;
				}
				int num5 = num4 + num3;
				bool flag2 = num5 < this._rectangleAfterPadding.Top;
				if (!flag2)
				{
					Rectangle rectangle = new Rectangle(this._rectangleAfterPadding.X + this.Desktop.ScaleRound((float)(slotCoordinatesByIndex.X * (slotSize + slotSpacing))), num4, num3, num3);
					ItemGridSlot itemGridSlot = this.Slots[i];
					bool flag3 = ((itemGridSlot != null) ? itemGridSlot.ItemStack : null) != null;
					if (flag3)
					{
						ClientItemBase clientItemBase;
						bool flag4 = itemGridSlot.ItemIcon != null && this._inGameView.Items.TryGetValue(itemGridSlot.ItemStack.Id, out clientItemBase);
						if (flag4)
						{
							bool flag5 = itemGridSlot.Background != null;
							if (flag5)
							{
								this.Desktop.Batcher2D.RequestDrawPatch(itemGridSlot.BackgroundPatch, rectangle, this.Desktop.Scale, this.Slots[i].IsItemIncompatible ? ItemGrid.IncompatibleItemStackSlotColor : UInt32Color.White);
							}
							else
							{
								bool flag6 = this._slotBackgroundPatch != null;
								if (flag6)
								{
									this.Desktop.Batcher2D.RequestDrawPatch(this._slotBackgroundPatch, rectangle, this.Desktop.Scale, this.Slots[i].IsItemIncompatible ? ItemGrid.IncompatibleItemStackSlotColor : UInt32Color.White);
								}
							}
							bool flag7 = this.RenderItemQualityBackground && !itemGridSlot.SkipItemQualityBackground;
							if (flag7)
							{
								bool flag8 = this._inGameView.InGame.Instance.ServerSettings.ItemQualities[clientItemBase.QualityIndex].RenderSpecialSlot && (clientItemBase.Consumable || (clientItemBase.Utility != null && clientItemBase.Utility.Usable));
								if (flag8)
								{
									this.Desktop.Batcher2D.RequestDrawPatch(this._specialSlotBackgroundPatches[clientItemBase.QualityIndex], rectangle, this.Desktop.Scale, this.Slots[i].IsItemIncompatible ? ItemGrid.IncompatibleItemStackSlotColor : UInt32Color.White);
								}
								else
								{
									bool flag9 = clientItemBase.BlockId == 0;
									if (flag9)
									{
										this.Desktop.Batcher2D.RequestDrawPatch(this._slotBackgroundPatches[clientItemBase.QualityIndex], rectangle, this.Desktop.Scale, this.Slots[i].IsItemIncompatible ? ItemGrid.IncompatibleItemStackSlotColor : UInt32Color.White);
									}
									else
									{
										this.Desktop.Batcher2D.RequestDrawPatch(this._blockSlotBackgroundPatch, rectangle, this.Desktop.Scale, this.Slots[i].IsItemIncompatible ? ItemGrid.IncompatibleItemStackSlotColor : UInt32Color.White);
									}
								}
							}
							bool flag10 = itemGridSlot.ItemStack != null && itemGridSlot.ItemStack.MaxDurability > 0.0 && itemGridSlot.ItemStack.Durability < 0.0001;
							if (flag10)
							{
								this.Desktop.Batcher2D.RequestDrawPatch(this._brokenSlotBackgroundOverlayPatch, rectangle, this.Desktop.Scale, itemGridSlot.IsItemIncompatible ? ItemGrid.IncompatibleItemStackSlotColor : UInt32Color.White);
							}
						}
						else
						{
							bool flag11 = itemGridSlot.Background != null;
							if (flag11)
							{
								this.Desktop.Batcher2D.RequestDrawPatch(itemGridSlot.BackgroundPatch, rectangle, this.Desktop.Scale, this.Slots[i].IsItemIncompatible ? ItemGrid.IncompatibleItemStackSlotColor : UInt32Color.White);
							}
							else
							{
								bool flag12 = this._slotBackgroundPatch != null;
								if (flag12)
								{
									this.Desktop.Batcher2D.RequestDrawPatch(this._slotBackgroundPatch, rectangle, this.Desktop.Scale, this.Slots[i].IsItemIncompatible ? ItemGrid.IncompatibleItemStackSlotColor : UInt32Color.White);
								}
							}
							bool flag13 = this.RenderItemQualityBackground && !itemGridSlot.SkipItemQualityBackground;
							if (flag13)
							{
								this.Desktop.Batcher2D.RequestDrawPatch(this._slotBackgroundPatches[0], rectangle, this.Desktop.Scale, this.Slots[i].IsItemIncompatible ? ItemGrid.IncompatibleItemStackSlotColor : UInt32Color.White);
							}
							int y = this._rectangleAfterPadding.Y + this.Desktop.ScaleRound((float)(slotCoordinatesByIndex.Y * (slotSize + slotSpacing))) + num - this._scaledScrollOffset.Y;
							Rectangle destRect = new Rectangle(this._rectangleAfterPadding.X + this.Desktop.ScaleRound((float)(slotCoordinatesByIndex.X * (slotSize + slotSpacing))) + num, y, num2, num2);
							this.Desktop.Batcher2D.RequestDrawTexture(this._defaultItemIconPatch.TextureArea.Texture, this._defaultItemIconPatch.TextureArea.Rectangle, destRect, this.Slots[i].IsItemIncompatible ? ItemGrid.IncompatibleItemStackSlotColor : ((i == this._dragSlotIndex) ? ItemGrid.DraggingItemStackColor : UInt32Color.White));
						}
						bool flag14 = this._durabilityBarTexture != null;
						if (flag14)
						{
							ClientItemStack itemStack = itemGridSlot.ItemStack;
							bool flag15 = itemStack.MaxDurability >= 0.0 && itemStack.Durability > 0.0001 && itemStack.Durability < itemStack.MaxDurability;
							if (flag15)
							{
								float num6 = (float)(itemStack.Durability / itemStack.MaxDurability);
								Rectangle destRect2 = new Rectangle(rectangle.X + this.Desktop.ScaleRound((float)durabilityBarAnchor.Left.Value), rectangle.Bottom - this.Desktop.ScaleRound((float)durabilityBarAnchor.Bottom.Value), this.Desktop.ScaleRound((float)durabilityBarAnchor.Width.Value), this.Desktop.ScaleRound((float)durabilityBarAnchor.Height.Value));
								ColorHsva colorHsva = ColorHsva.Lerp(color, color2, num6);
								bool isItemIncompatible = itemGridSlot.IsItemIncompatible;
								if (isItemIncompatible)
								{
									colorHsva.A = 0.14901961f;
								}
								else
								{
									bool flag16 = i == this._dragSlotIndex;
									if (flag16)
									{
										colorHsva.A = 0.49019608f;
									}
								}
								this.Desktop.Batcher2D.RequestDrawPatch(this._durabilityBarBackgroundTexture, destRect2, this.Desktop.Scale, itemGridSlot.IsItemIncompatible ? ItemGrid.IncompatibleItemStackColor : UInt32Color.White);
								destRect2.Width = this.Desktop.ScaleRound((float)durabilityBarAnchor.Width.Value * num6);
								Rectangle sourceRect = new Rectangle(this._durabilityBarTexture.Rectangle.X, this._durabilityBarTexture.Rectangle.Y, (int)((float)this._durabilityBarTexture.Rectangle.Width * num6), this._durabilityBarTexture.Rectangle.Height);
								this.Desktop.Batcher2D.RequestDrawTexture(this._durabilityBarTexture.Texture, sourceRect, destRect2, colorHsva.ToUInt32Color());
							}
						}
					}
					else
					{
						UInt32Color uint32Color = (itemGridSlot != null && itemGridSlot.IsItemIncompatible) ? ItemGrid.IncompatibleItemStackSlotColor : UInt32Color.White;
						bool flag17 = ((itemGridSlot != null) ? itemGridSlot.Background : null) != null;
						if (flag17)
						{
							this.Desktop.Batcher2D.RequestDrawPatch(itemGridSlot.BackgroundPatch, rectangle, this.Desktop.Scale, uint32Color);
						}
						else
						{
							bool flag18 = this._slotBackgroundPatch != null;
							if (flag18)
							{
								this.Desktop.Batcher2D.RequestDrawPatch(this._slotBackgroundPatch, rectangle, this.Desktop.Scale, uint32Color);
							}
						}
						bool flag19 = this._quantityPopupSlotIndex == i;
						if (flag19)
						{
							this.Desktop.Batcher2D.RequestDrawPatch(this._quantityPopupSlotOverlayPatch, rectangle, this.Desktop.Scale, uint32Color);
						}
						bool flag20 = ((itemGridSlot != null) ? itemGridSlot.Icon : null) != null;
						if (flag20)
						{
							int y2 = this._rectangleAfterPadding.Y + this.Desktop.ScaleRound((float)(slotCoordinatesByIndex.Y * (slotSize + slotSpacing))) + num - this._scaledScrollOffset.Y;
							Rectangle destRect3 = new Rectangle(this._rectangleAfterPadding.X + this.Desktop.ScaleRound((float)(slotCoordinatesByIndex.X * (slotSize + slotSpacing))) + num, y2, num2, num2);
							this.Desktop.Batcher2D.RequestDrawTexture(this.Slots[i].IconTextureArea.Texture, this.Slots[i].IconTextureArea.Rectangle, destRect3, uint32Color);
						}
					}
					bool flag21 = this.InfoDisplay == 1;
					if (flag21)
					{
						Rectangle destRect4 = new Rectangle(this._rectangleAfterPadding.X + this.Desktop.ScaleRound((float)(slotCoordinatesByIndex.X * (slotSize + slotSpacing) + slotSpacing - 1)) + num3, num4, (this.SlotsPerRow / this.GetItemsPerRow() - 1) * (num3 + 1), num3);
						this.Desktop.Batcher2D.RequestDrawPatch(this._infoPaneBackgroundPatch, destRect4, this.Desktop.Scale, UInt32Color.White);
					}
				}
			}
			for (int j = 0; j < this.Slots.Length; j++)
			{
				ItemGridSlot itemGridSlot2 = this.Slots[j];
				bool flag22 = ((itemGridSlot2 != null) ? itemGridSlot2.ItemIcon : null) == null;
				if (!flag22)
				{
					Point slotCoordinatesByIndex2 = this.GetSlotCoordinatesByIndex(j);
					int num7 = this._rectangleAfterPadding.Y + this.Desktop.ScaleRound((float)(slotCoordinatesByIndex2.Y * (slotSize + slotSpacing))) + num - this._scaledScrollOffset.Y;
					bool flag23 = num7 > this._rectangleAfterPadding.Bottom;
					if (flag23)
					{
						break;
					}
					int num8 = num7 + num3;
					bool flag24 = num8 < this._rectangleAfterPadding.Top;
					if (!flag24)
					{
						bool isItemIncompatible2 = itemGridSlot2.IsItemIncompatible;
						UInt32Color color3;
						if (isItemIncompatible2)
						{
							color3 = ItemGrid.IncompatibleItemStackColor;
						}
						else
						{
							bool flag25 = j == this._dragSlotIndex;
							if (flag25)
							{
								color3 = ItemGrid.DraggingItemStackColor;
							}
							else
							{
								bool flag26 = itemGridSlot2.ItemStack != null && itemGridSlot2.ItemStack.MaxDurability > 0.0 && itemGridSlot2.ItemStack.Durability < 0.0001;
								if (flag26)
								{
									color3 = ItemGrid.BrokenItemStackColor;
								}
								else
								{
									color3 = UInt32Color.White;
								}
							}
						}
						Rectangle destRect5 = new Rectangle(this._rectangleAfterPadding.X + this.Desktop.ScaleRound((float)(slotCoordinatesByIndex2.X * (slotSize + slotSpacing))) + num, num7, num2, num2);
						this.Desktop.Batcher2D.RequestDrawTexture(itemGridSlot2.ItemIcon.Texture, itemGridSlot2.ItemIcon.Rectangle, destRect5, color3);
					}
				}
			}
			for (int k = 0; k < this.Slots.Length; k++)
			{
				ItemGridSlot itemGridSlot3 = this.Slots[k];
				bool flag27 = ((itemGridSlot3 != null) ? itemGridSlot3.ItemStack : null) == null;
				if (!flag27)
				{
					Point slotCoordinatesByIndex3 = this.GetSlotCoordinatesByIndex(k);
					int num9 = this._rectangleAfterPadding.Y + this.Desktop.ScaleRound((float)(slotCoordinatesByIndex3.Y * (slotSize + slotSpacing))) + num - this._scaledScrollOffset.Y;
					bool flag28 = num9 > this._rectangleAfterPadding.Bottom;
					if (flag28)
					{
						break;
					}
					int num10 = num9 + num3;
					bool flag29 = num10 < this._rectangleAfterPadding.Top;
					if (!flag29)
					{
						bool flag30 = this.InfoDisplay == 1;
						if (flag30)
						{
							float x = (float)(this._rectangleAfterPadding.X + this.Desktop.ScaleRound((float)(slotCoordinatesByIndex3.X * (slotSize + slotSpacing))) + num3) + 15f;
							float y3 = (float)(num10 - num - num3) + 27f * this.Desktop.Scale;
							this.Desktop.Batcher2D.RequestDrawText(this._regularFont, 14f * this.Desktop.Scale, this.Slots[k].Name, new Vector3(x, y3, 0f), ItemGrid.InfoPanePrimaryTextColor, false, false, 0f);
						}
						bool flag31 = this.Slots[k].ItemStack.Quantity <= 1;
						if (!flag31)
						{
							string text = this.Slots[k].ItemStack.Quantity.ToString();
							int num11 = this._rectangleAfterPadding.X + this.Desktop.ScaleRound((float)(slotCoordinatesByIndex3.X * (slotSize + slotSpacing))) + num + num2;
							num11 -= this.Desktop.ScaleRound(this._boldFont.CalculateTextWidth(text) * 16f / (float)this._boldFont.BaseSize);
							float y4 = (float)(num10 - num) - 26f * this.Desktop.Scale;
							this.Desktop.Batcher2D.RequestDrawText(this._boldFont, 16f * this.Desktop.Scale, text, new Vector3((float)num11, y4, 0f), this.Slots[k].IsItemIncompatible ? ItemGrid.IncompatibleItemStackColor : UInt32Color.White, false, false, 0f);
						}
					}
				}
			}
			for (int l = 0; l < this.Slots.Length; l++)
			{
				ItemGridSlot itemGridSlot4 = this.Slots[l];
				bool flag32 = itemGridSlot4 == null || (((itemGridSlot4 != null) ? itemGridSlot4.Overlay : null) == null && this._brokenSlotIconOverlayPatch == null);
				if (!flag32)
				{
					Point slotCoordinatesByIndex4 = this.GetSlotCoordinatesByIndex(l);
					int num12 = this._rectangleAfterPadding.Y + this.Desktop.ScaleRound((float)(slotCoordinatesByIndex4.Y * (slotSize + slotSpacing))) - this._scaledScrollOffset.Y;
					bool flag33 = num12 > this._rectangleAfterPadding.Bottom;
					if (flag33)
					{
						break;
					}
					int num13 = num12 + num3;
					bool flag34 = num13 < this._rectangleAfterPadding.Top;
					if (!flag34)
					{
						Rectangle destRect6 = new Rectangle(this._rectangleAfterPadding.X + this.Desktop.ScaleRound((float)(slotCoordinatesByIndex4.X * (slotSize + slotSpacing))), num12, num3, num3);
						bool flag35 = itemGridSlot4.ItemStack != null && itemGridSlot4.ItemStack.MaxDurability > 0.0 && itemGridSlot4.ItemStack.Durability < 0.0001;
						if (flag35)
						{
							this.Desktop.Batcher2D.RequestDrawPatch(this._brokenSlotIconOverlayPatch, destRect6, this.Desktop.Scale, itemGridSlot4.IsItemIncompatible ? ItemGrid.IncompatibleItemStackSlotColor : UInt32Color.White);
						}
						bool flag36 = itemGridSlot4.Overlay != null;
						if (flag36)
						{
							this.Desktop.Batcher2D.RequestDrawPatch(itemGridSlot4.OverlayPatch, destRect6, this.Desktop.Scale);
						}
					}
				}
			}
			bool showScrollbar2 = this.ShowScrollbar;
			if (showScrollbar2)
			{
				this.Desktop.Batcher2D.PopScissor();
			}
		}

		// Token: 0x04001D16 RID: 7446
		private const byte DraggingItemStackOpacity = 125;

		// Token: 0x04001D17 RID: 7447
		private const byte IncompatibleItemStackOpacity = 38;

		// Token: 0x04001D18 RID: 7448
		private const float DraggingItemStackOpacityFloat = 0.49019608f;

		// Token: 0x04001D19 RID: 7449
		private const float IncompatibleItemStackOpacityFloat = 0.14901961f;

		// Token: 0x04001D1A RID: 7450
		private static readonly UInt32Color DraggingItemStackColor = UInt32Color.FromRGBA(byte.MaxValue, byte.MaxValue, byte.MaxValue, 125);

		// Token: 0x04001D1B RID: 7451
		private static readonly UInt32Color IncompatibleItemStackColor = UInt32Color.FromRGBA(byte.MaxValue, byte.MaxValue, byte.MaxValue, 38);

		// Token: 0x04001D1C RID: 7452
		private static readonly UInt32Color BrokenItemStackColor = UInt32Color.FromRGBA(190, 190, 190, byte.MaxValue);

		// Token: 0x04001D1D RID: 7453
		private static readonly UInt32Color IncompatibleItemStackSlotColor = UInt32Color.FromRGBA(byte.MaxValue, byte.MaxValue, byte.MaxValue, 120);

		// Token: 0x04001D1E RID: 7454
		private static readonly UInt32Color InfoPanePrimaryTextColor = UInt32Color.FromRGBA(170, 180, 190, byte.MaxValue);

		// Token: 0x04001D1F RID: 7455
		private readonly InGameView _inGameView;

		// Token: 0x04001D20 RID: 7456
		public Action<int, int> SlotClicking;

		// Token: 0x04001D21 RID: 7457
		public Action<int> SlotDoubleClicking;

		// Token: 0x04001D22 RID: 7458
		public Action<int, Element, ItemGrid.ItemDragData> Dropped;

		// Token: 0x04001D23 RID: 7459
		public Action<int, int> DragCancelled;

		// Token: 0x04001D24 RID: 7460
		public Action<int> SlotMouseEntered;

		// Token: 0x04001D25 RID: 7461
		public Action<int> SlotMouseExited;

		// Token: 0x04001D26 RID: 7462
		public Action<int, Element, ItemGrid.ItemDragData> SlotMouseDragCompleted;

		// Token: 0x04001D27 RID: 7463
		public Action<int, int> SlotMouseDragExited;

		// Token: 0x04001D28 RID: 7464
		public Func<int, bool> SlotMouseDown;

		// Token: 0x04001D29 RID: 7465
		[UIMarkupProperty]
		public int SlotsPerRow;

		// Token: 0x04001D2A RID: 7466
		[UIMarkupProperty]
		public bool ShowScrollbar;

		// Token: 0x04001D2B RID: 7467
		[UIMarkupProperty]
		public ItemGrid.ItemGridStyle Style;

		// Token: 0x04001D2C RID: 7468
		[UIMarkupProperty]
		public bool RenderItemQualityBackground = true;

		// Token: 0x04001D2D RID: 7469
		[UIMarkupProperty]
		public ItemGridInfoDisplayMode InfoDisplay = 0;

		// Token: 0x04001D2E RID: 7470
		[UIMarkupProperty]
		public int AdjacentInfoPaneGridWidth = 2;

		// Token: 0x04001D2F RID: 7471
		[UIMarkupProperty]
		public bool AreItemsDraggable = true;

		// Token: 0x04001D30 RID: 7472
		[UIMarkupProperty]
		public int? InventorySectionId;

		// Token: 0x04001D31 RID: 7473
		[UIMarkupProperty]
		public ItemGridSlot[] Slots = new ItemGridSlot[0];

		// Token: 0x04001D32 RID: 7474
		[UIMarkupProperty]
		public bool AllowMaxStackDraggableItems;

		// Token: 0x04001D33 RID: 7475
		private ItemTooltipLayer _tooltip;

		// Token: 0x04001D34 RID: 7476
		private bool _wasDragging;

		// Token: 0x04001D35 RID: 7477
		private Point _mouseDownPosition;

		// Token: 0x04001D36 RID: 7478
		private int _mouseDownSlotIndex = -1;

		// Token: 0x04001D37 RID: 7479
		private int _mouseOverSlotIndex = -1;

		// Token: 0x04001D38 RID: 7480
		private int _dragSlotIndex = -1;

		// Token: 0x04001D39 RID: 7481
		private int _pressedMouseButton = -1;

		// Token: 0x04001D3A RID: 7482
		private bool _isMouseDraggingOver;

		// Token: 0x04001D3B RID: 7483
		private int _slotIndexForDoubleClick = -1;

		// Token: 0x04001D3C RID: 7484
		private bool _isQuantityPopupOpen;

		// Token: 0x04001D3D RID: 7485
		private int _quantityPopupSlotIndex = -1;

		// Token: 0x04001D3E RID: 7486
		private Font _regularFont;

		// Token: 0x04001D3F RID: 7487
		private Font _boldFont;

		// Token: 0x04001D40 RID: 7488
		private TexturePatch _durabilityBarBackgroundTexture;

		// Token: 0x04001D41 RID: 7489
		private TextureArea _durabilityBarTexture;

		// Token: 0x04001D42 RID: 7490
		private TexturePatch _slotBackgroundPatch;

		// Token: 0x04001D43 RID: 7491
		private TexturePatch _quantityPopupSlotOverlayPatch;

		// Token: 0x04001D44 RID: 7492
		private TexturePatch _defaultItemIconPatch;

		// Token: 0x04001D45 RID: 7493
		private TexturePatch _brokenSlotBackgroundOverlayPatch;

		// Token: 0x04001D46 RID: 7494
		private TexturePatch _brokenSlotIconOverlayPatch;

		// Token: 0x04001D47 RID: 7495
		private TexturePatch _blockSlotBackgroundPatch;

		// Token: 0x04001D48 RID: 7496
		private TexturePatch _infoPaneBackgroundPatch;

		// Token: 0x04001D49 RID: 7497
		private readonly Dictionary<int, TexturePatch> _slotBackgroundPatches = new Dictionary<int, TexturePatch>();

		// Token: 0x04001D4A RID: 7498
		private readonly Dictionary<int, TexturePatch> _specialSlotBackgroundPatches = new Dictionary<int, TexturePatch>();

		// Token: 0x02000D58 RID: 3416
		public class ItemDragData
		{
			// Token: 0x06006526 RID: 25894 RVA: 0x00210E4E File Offset: 0x0020F04E
			public ItemDragData(int pressedMouseButton, int itemGridIndex, ClientItemStack itemStack, int? inventorySectionId, int slotId)
			{
				this.PressedMouseButton = pressedMouseButton;
				this.ItemGridIndex = itemGridIndex;
				this.ItemStack = itemStack;
				this.InventorySectionId = inventorySectionId;
				this.SlotId = slotId;
			}

			// Token: 0x04004197 RID: 16791
			public readonly int PressedMouseButton;

			// Token: 0x04004198 RID: 16792
			public readonly int ItemGridIndex;

			// Token: 0x04004199 RID: 16793
			public readonly ClientItemStack ItemStack;

			// Token: 0x0400419A RID: 16794
			public readonly int? InventorySectionId;

			// Token: 0x0400419B RID: 16795
			public readonly int SlotId;
		}

		// Token: 0x02000D59 RID: 3417
		[UIMarkupData]
		public class ItemGridStyle
		{
			// Token: 0x06006527 RID: 25895 RVA: 0x00210E80 File Offset: 0x0020F080
			public ItemGrid.ItemGridStyle Clone()
			{
				return new ItemGrid.ItemGridStyle
				{
					SlotSize = this.SlotSize,
					SlotIconSize = this.SlotIconSize,
					SlotSpacing = this.SlotSpacing,
					DurabilityBar = this.DurabilityBar,
					DurabilityBarBackground = this.DurabilityBarBackground,
					DurabilityBarAnchor = this.DurabilityBarAnchor,
					DurabilityBarColorStart = this.DurabilityBarColorStart,
					DurabilityBarColorEnd = this.DurabilityBarColorEnd,
					SlotBackground = this.SlotBackground,
					QuantityPopupSlotOverlay = this.QuantityPopupSlotOverlay,
					BrokenSlotBackgroundOverlay = this.BrokenSlotBackgroundOverlay,
					BrokenSlotIconOverlay = this.BrokenSlotIconOverlay,
					DefaultItemIcon = this.DefaultItemIcon
				};
			}

			// Token: 0x0400419C RID: 16796
			public int SlotSize;

			// Token: 0x0400419D RID: 16797
			public int SlotIconSize;

			// Token: 0x0400419E RID: 16798
			public int SlotSpacing;

			// Token: 0x0400419F RID: 16799
			public PatchStyle DurabilityBarBackground;

			// Token: 0x040041A0 RID: 16800
			public UIPath DurabilityBar;

			// Token: 0x040041A1 RID: 16801
			public Anchor DurabilityBarAnchor;

			// Token: 0x040041A2 RID: 16802
			public UInt32Color DurabilityBarColorStart;

			// Token: 0x040041A3 RID: 16803
			public UInt32Color DurabilityBarColorEnd;

			// Token: 0x040041A4 RID: 16804
			public PatchStyle SlotBackground;

			// Token: 0x040041A5 RID: 16805
			public PatchStyle QuantityPopupSlotOverlay;

			// Token: 0x040041A6 RID: 16806
			public PatchStyle BrokenSlotBackgroundOverlay;

			// Token: 0x040041A7 RID: 16807
			public PatchStyle BrokenSlotIconOverlay;

			// Token: 0x040041A8 RID: 16808
			public PatchStyle DefaultItemIcon;

			// Token: 0x040041A9 RID: 16809
			public SoundStyle ItemStackMouseDownSound;

			// Token: 0x040041AA RID: 16810
			public SoundStyle ItemStackActivateSound;

			// Token: 0x040041AB RID: 16811
			public SoundStyle ItemStackMovedSound;
		}
	}
}
