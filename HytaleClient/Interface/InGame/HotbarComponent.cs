using System;
using System.Collections.Generic;
using System.Linq;
using HytaleClient.Application;
using HytaleClient.Data.Items;
using HytaleClient.InGame.Modules;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Interface.UI.Styles;
using HytaleClient.Math;
using HytaleClient.Protocol;

namespace HytaleClient.Interface.InGame
{
	// Token: 0x02000882 RID: 2178
	internal class HotbarComponent : InterfaceComponent
	{
		// Token: 0x06003D96 RID: 15766 RVA: 0x0009F3F8 File Offset: 0x0009D5F8
		public HotbarComponent(InGameView inGameView) : base(inGameView.Interface, inGameView.HudContainer)
		{
			this.InGameView = inGameView;
		}

		// Token: 0x06003D97 RID: 15767 RVA: 0x0009F428 File Offset: 0x0009D628
		public void Build()
		{
			base.Clear();
			this._hotkeyLabels.Clear();
			this._slotCount = 0;
			this._highlightedSlotOverlay = new PatchStyle("InGame/Pages/Inventory/SlotHighlight.png");
			Document document;
			this.Interface.TryGetDocument("InGame/Hud/Hotbar.ui", out document);
			UIFragment uifragment = document.Instantiate(this.Desktop, this);
			this._inventoryClosedContainerMargin = document.ResolveNamedValue<int>(this.Desktop.Provider, "InventoryClosedContainerMargin");
			this._inventoryOpenedContainerMargin = document.ResolveNamedValue<int>(this.Desktop.Provider, "InventoryOpenedContainerMargin");
			this._container = uifragment.Get<Group>("Container");
			this._background = uifragment.Get<Group>("Background");
			this._itemGrid = uifragment.Get<ItemGrid>("ItemGrid");
			this._itemGrid.Slots = new ItemGridSlot[this.InGameView.DefaultItemSlotsPerRow];
			this._itemGrid.InventorySectionId = new int?(-1);
			this._itemGrid.SlotClicking = delegate(int slotIndex, int button)
			{
				this.InGameView.HandleInventoryClick(-1, slotIndex, button);
			};
			this._itemGrid.SlotDoubleClicking = delegate(int slotIndex)
			{
				this.InGameView.HandleInventoryDoubleClick(-1, slotIndex);
			};
			this._itemGrid.Dropped = delegate(int targetSlotIndex, Element sourceItemGrid, ItemGrid.ItemDragData dragData)
			{
				this.InGameView.HandleInventoryDragEnd(this._itemGrid, -1, targetSlotIndex, sourceItemGrid, dragData);
			};
			this._itemGrid.DragCancelled = delegate(int slotIndex, int button)
			{
				this.InGameView.HandleInventoryDropItem(-1, slotIndex, button);
			};
			this._itemGrid.SlotMouseEntered = delegate(int slotIndex)
			{
				this.InGameView.HandleItemSlotMouseEntered(-1, slotIndex);
			};
			this._itemGrid.SlotMouseExited = delegate(int slotIndex)
			{
				this.InGameView.HandleItemSlotMouseExited(-1, slotIndex);
			};
			this._utilitySlotContainer = uifragment.Get<Group>("UtilitySlotContainer");
			this._utilitySlotPointer = uifragment.Get<Group>("UtilitySlotPointer");
			this._utilityItemGrid = uifragment.Get<ItemGrid>("UtilitySlotItemGrid");
			this._utilityItemGrid.InventorySectionId = new int?(-5);
			this._utilityItemGrid.Slots = new ItemGridSlot[1];
			this._consumableSlotContainer = uifragment.Get<Group>("ConsumableSlotContainer");
			this._consumableSlotPointer = uifragment.Get<Group>("ConsumableSlotPointer");
			this._consumableItemGrid = uifragment.Get<ItemGrid>("ConsumableSlotItemGrid");
			this._consumableItemGrid.InventorySectionId = new int?(-6);
			this._consumableItemGrid.Slots = new ItemGridSlot[1];
			this._activeSlotOverlay = uifragment.Get<Group>("ActiveSlotOverlay");
			this._activeSlotSwitchableOverlay = uifragment.Get<Group>("ActiveSlotSwitchableOverlay");
			int num = this._activeHotbarSlot * (this._itemGrid.Style.SlotSize + this._itemGrid.Style.SlotSpacing) + this._itemGrid.Padding.Horizontal.GetValueOrDefault();
			int num2 = (int)(((float)this._activeSlotOverlay.Anchor.Width.Value - (float)this._itemGrid.Style.SlotSize) / 2f);
			this._activeSlotOverlay.Anchor.Left = new int?(num - num2);
			this._activeSlotSwitchableOverlay.Anchor.Left = new int?(num - num2);
			this._activeItemNameLabel = uifragment.Get<Label>("ActiveItemNameLabel");
			this.UpdateActiveHotbarSlotOverlay();
			bool flag = this.InGameView.HotbarStacks != null;
			if (flag)
			{
				this.SetupGrid();
			}
			bool isMounted = base.IsMounted;
			if (isMounted)
			{
				this.UpdateBackgroundVisibility();
				this.UpdateInputBindings();
			}
		}

		// Token: 0x06003D98 RID: 15768 RVA: 0x0009F766 File Offset: 0x0009D966
		protected override void OnMounted()
		{
			this.Desktop.RegisterAnimationCallback(new Action<float>(this.Animate));
			this.UpdateInputBindings();
		}

		// Token: 0x06003D99 RID: 15769 RVA: 0x0009F788 File Offset: 0x0009D988
		protected override void OnUnmounted()
		{
			this.Desktop.UnregisterAnimationCallback(new Action<float>(this.Animate));
		}

		// Token: 0x06003D9A RID: 15770 RVA: 0x0009F7A4 File Offset: 0x0009D9A4
		private void Animate(float deltaTime)
		{
			bool flag = this._activeHotbarItemNameLabelTime != 0f;
			if (flag)
			{
				this._activeHotbarItemNameLabelTime -= deltaTime;
				bool flag2 = this._activeHotbarItemNameLabelTime <= 0f;
				if (flag2)
				{
					this._activeHotbarItemNameLabelTime = 0f;
					this._activeItemNameLabel.Visible = false;
				}
			}
		}

		// Token: 0x06003D9B RID: 15771 RVA: 0x0009F804 File Offset: 0x0009DA04
		public void OnSetStacks()
		{
			this.SetupGrid();
			this.UpdateActiveItemNameLabel();
		}

		// Token: 0x06003D9C RID: 15772 RVA: 0x0009F818 File Offset: 0x0009DA18
		public void OnToggleItemSlotSelector()
		{
			this._itemGrid.InfoDisplay = ((this.InGameView.InGame.ActiveItemSelector == AppInGame.ItemSelector.None) ? 0 : 2);
			this._utilitySlotPointer.Visible = (this.InGameView.InGame.ActiveItemSelector == AppInGame.ItemSelector.Utility);
			this._utilitySlotPointer.Layout(new Rectangle?(this._utilitySlotPointer.Parent.RectangleAfterPadding), true);
			this._utilitySlotContainer.Find<Group>("Highlight").Visible = (this.InGameView.InGame.ActiveItemSelector == AppInGame.ItemSelector.Utility);
			this._utilitySlotContainer.Layout(null, true);
			this._consumableSlotPointer.Visible = (this.InGameView.InGame.ActiveItemSelector == AppInGame.ItemSelector.Consumable);
			this._consumableSlotPointer.Layout(new Rectangle?(this._consumableSlotContainer.Parent.RectangleAfterPadding), true);
			this._consumableSlotContainer.Find<Group>("Highlight").Visible = (this.InGameView.InGame.ActiveItemSelector == AppInGame.ItemSelector.Consumable);
			this._consumableSlotContainer.Layout(null, true);
		}

		// Token: 0x06003D9D RID: 15773 RVA: 0x0009F948 File Offset: 0x0009DB48
		public void OnToggleInventoryOpen()
		{
			bool visible = this.InGameView.InGame.CurrentPage != 1 && this.InGameView.InGame.CurrentPage != 2;
			this._utilitySlotContainer.Visible = (this._consumableSlotContainer.Visible = visible);
			bool isMounted = this._container.IsMounted;
			if (isMounted)
			{
				this._container.Layout(null, true);
			}
			bool isMounted2 = this._utilitySlotContainer.IsMounted;
			if (isMounted2)
			{
				this._utilitySlotContainer.Layout(new Rectangle?(this._utilitySlotContainer.RectangleAfterPadding), true);
				this._consumableSlotContainer.Layout(new Rectangle?(this._consumableSlotContainer.RectangleAfterPadding), true);
			}
			this.UpdateBackgroundVisibility();
		}

		// Token: 0x06003D9E RID: 15774 RVA: 0x0009FA18 File Offset: 0x0009DC18
		private void UpdateInputBindings()
		{
			this._utilitySlotContainer.Find<Label>("InputBinding").Text = this.InGameView.Interface.App.Settings.InputBindings.ShowUtilitySlotSelector.BoundInputLabel;
			bool isMounted = this._utilitySlotContainer.IsMounted;
			if (isMounted)
			{
				this._utilitySlotContainer.Layout(null, true);
			}
			this._consumableSlotContainer.Find<Label>("InputBinding").Text = this.InGameView.Interface.App.Settings.InputBindings.ShowConsumableSlotSelector.BoundInputLabel;
			bool isMounted2 = this._consumableSlotContainer.IsMounted;
			if (isMounted2)
			{
				this._consumableSlotContainer.Layout(null, true);
			}
		}

		// Token: 0x06003D9F RID: 15775 RVA: 0x0009FAE4 File Offset: 0x0009DCE4
		public void UpdateBackgroundVisibility()
		{
			this._background.Visible = (this.InGameView.InGame.Instance.GameMode == null && this.InGameView.InGame.CurrentPage != 1 && this.InGameView.InGame.CurrentPage != 2);
			bool isMounted = this._background.IsMounted;
			if (isMounted)
			{
				this._background.Layout(new Rectangle?(this._background.Parent.RectangleAfterPadding), true);
			}
		}

		// Token: 0x06003DA0 RID: 15776 RVA: 0x0009FB74 File Offset: 0x0009DD74
		public void OnPageChanged()
		{
			this._itemGrid.AreItemsDraggable = (this.Interface.App.InGame.CurrentPage == 2 || this.Interface.App.InGame.CurrentPage == 1 || this.Interface.App.InGame.IsToolsSettingsModalOpened);
		}

		// Token: 0x06003DA1 RID: 15777 RVA: 0x0009FBD8 File Offset: 0x0009DDD8
		public void OnItemIconsUpdated()
		{
			bool flag = this.InGameView.HotbarStacks != null;
			if (flag)
			{
				this.SetupGrid();
			}
		}

		// Token: 0x06003DA2 RID: 15778 RVA: 0x0009FC00 File Offset: 0x0009DE00
		public void SetupGrid()
		{
			bool flag = this._slotCount != this.InGameView.HotbarStacks.Length;
			if (flag)
			{
				foreach (Label label in this._hotkeyLabels)
				{
					label.Parent.Remove(label);
				}
				this._hotkeyLabels.Clear();
				Document document;
				this.Interface.TryGetDocument("InGame/Hud/HotKey.ui", out document);
				for (int i = 0; i < this.InGameView.HotbarStacks.Length; i++)
				{
					Label label2 = (Label)document.Instantiate(this.Desktop, this._container).RootElements[0];
					label2.Anchor.Left = new int?(i * (this._itemGrid.Style.SlotSize + this._itemGrid.Style.SlotSpacing) + 4);
					label2.Text = ((i == 9) ? 0 : (i + 1)).ToString();
					this._hotkeyLabels.Add(label2);
				}
				this._slotCount = this.InGameView.HotbarStacks.Length;
				this._container.Layout(null, true);
			}
			this._itemGrid.Slots = new ItemGridSlot[this.InGameView.HotbarStacks.Length];
			for (int j = 0; j < this.InGameView.HotbarStacks.Length; j++)
			{
				ClientItemStack clientItemStack = this.InGameView.HotbarStacks[j];
				bool flag2 = clientItemStack == null;
				if (!flag2)
				{
					bool isItemIncompatible = false;
					bool flag3 = this.InGameView.InventoryPage.StoragePanel.ActiveItemFilter != null;
					if (flag3)
					{
						ClientItemBase clientItemBase;
						bool flag4 = this.InGameView.Items.TryGetValue(clientItemStack.Id, out clientItemBase);
						if (flag4)
						{
							string activeItemFilter = this.InGameView.InventoryPage.StoragePanel.ActiveItemFilter;
							string a = activeItemFilter;
							if (!(a == "Weapon"))
							{
								if (!(a == "Armor"))
								{
									if (a == "Material")
									{
										bool flag5 = clientItemBase.BlockId == 0;
										if (flag5)
										{
											isItemIncompatible = true;
										}
									}
								}
								else
								{
									bool flag6 = clientItemBase.Armor == null;
									if (flag6)
									{
										isItemIncompatible = true;
									}
								}
							}
							else
							{
								bool flag7 = clientItemBase.Weapon == null;
								if (flag7)
								{
									isItemIncompatible = true;
								}
							}
						}
						else
						{
							isItemIncompatible = true;
						}
					}
					else
					{
						bool flag8 = !this.Desktop.IsMouseDragging;
						if (flag8)
						{
							ClientItemBase item;
							this.InGameView.Items.TryGetValue(clientItemStack.Id, out item);
							isItemIncompatible = !this.InGameView.IsItemValid(j, item, InventorySectionType.Hotbar);
						}
					}
					this._itemGrid.Slots[j] = new ItemGridSlot(clientItemStack)
					{
						IsItemIncompatible = isItemIncompatible
					};
				}
			}
			bool flag9 = this._highlightedHotbarSlot >= 0 && this._itemGrid.Slots[this._highlightedHotbarSlot] != null;
			if (flag9)
			{
				this.ApplySlotHighlight(this._highlightedHotbarSlot);
			}
			this._utilityItemGrid.Slots[0] = ((this._activeUtilitySlot == -1) ? null : new ItemGridSlot(this.InGameView.UtilityStacks[this._activeUtilitySlot]));
			this._utilityItemGrid.Layout(null, true);
			this._consumableItemGrid.Slots[0] = ((this._activeConsumableSlot == -1) ? null : new ItemGridSlot(this.InGameView.ConsumableStacks[this._activeConsumableSlot]));
			this._consumableItemGrid.Layout(null, true);
			this._itemGrid.Layout(null, true);
		}

		// Token: 0x06003DA3 RID: 15779 RVA: 0x0009FFF8 File Offset: 0x0009E1F8
		public void OnSetActiveHotbarSlot(int slot)
		{
			this._activeHotbarSlot = slot;
			bool flag = !this.Interface.HasMarkupError;
			if (flag)
			{
				this.UpdateActiveHotbarSlotOverlay();
				bool isMounted = this._activeSlotOverlay.IsMounted;
				if (isMounted)
				{
					this._activeSlotOverlay.Layout(new Rectangle?(this._activeSlotOverlay.Parent.AnchoredRectangle), true);
				}
				bool isMounted2 = this._activeSlotSwitchableOverlay.IsMounted;
				if (isMounted2)
				{
					this._activeSlotSwitchableOverlay.Layout(new Rectangle?(this._activeSlotSwitchableOverlay.Parent.AnchoredRectangle), true);
				}
			}
			this.UpdateActiveItemNameLabel();
		}

		// Token: 0x06003DA4 RID: 15780 RVA: 0x000A0094 File Offset: 0x0009E294
		private void UpdateActiveHotbarSlotOverlay()
		{
			bool flag = this._activeHotbarSlot != -1;
			ClientItemStack clientItemStack;
			if (!flag)
			{
				clientItemStack = null;
			}
			else
			{
				InGameView inGameView = this.InGameView;
				clientItemStack = ((inGameView != null) ? inGameView.GetHotbarItem(this._activeHotbarSlot) : null);
			}
			ClientItemStack clientItemStack2 = clientItemStack;
			bool flag2 = clientItemStack2 != null && this.IsGroupedBlock(clientItemStack2);
			bool flag3 = flag;
			if (flag3)
			{
				this._activeSlotOverlay.Visible = !flag2;
				this._activeSlotSwitchableOverlay.Visible = flag2;
			}
			else
			{
				this._activeSlotOverlay.Visible = false;
				this._activeSlotSwitchableOverlay.Visible = false;
			}
			int num = this._activeHotbarSlot * (this._itemGrid.Style.SlotSize + this._itemGrid.Style.SlotSpacing) + this._itemGrid.Padding.Left.GetValueOrDefault();
			int num2 = (int)(((float)this._activeSlotOverlay.Anchor.Width.Value - (float)this._itemGrid.Style.SlotSize) / 2f);
			this._activeSlotOverlay.Anchor.Left = new int?(num - num2);
			this._activeSlotSwitchableOverlay.Anchor.Left = this._activeSlotOverlay.Anchor.Left;
		}

		// Token: 0x06003DA5 RID: 15781 RVA: 0x000A01D0 File Offset: 0x0009E3D0
		private bool IsGroupedBlock(ClientItemStack item)
		{
			string id = item.Id;
			return Enumerable.Any<BlockGroup>(this.InGameView.InGame.Instance.ServerSettings.BlockGroups.Values, (BlockGroup group) => Enumerable.Contains<string>(group.Names, id));
		}

		// Token: 0x06003DA6 RID: 15782 RVA: 0x000A0224 File Offset: 0x0009E424
		public void OnSetActiveUtilitySlot(int slot)
		{
			this._activeUtilitySlot = slot;
			this._utilityItemGrid.Slots[0] = ((this._activeUtilitySlot == -1) ? null : new ItemGridSlot(this.InGameView.UtilityStacks[this._activeUtilitySlot]));
			this._utilityItemGrid.Layout(null, true);
		}

		// Token: 0x06003DA7 RID: 15783 RVA: 0x000A0280 File Offset: 0x0009E480
		public void OnSetActiveConsumableSlot(int slot)
		{
			this._activeConsumableSlot = slot;
			this._consumableItemGrid.Slots[0] = ((this._activeConsumableSlot == -1) ? null : new ItemGridSlot(this.InGameView.ConsumableStacks[this._activeConsumableSlot]));
			this._consumableItemGrid.Layout(null, true);
		}

		// Token: 0x06003DA8 RID: 15784 RVA: 0x000A02DC File Offset: 0x0009E4DC
		public void HighlightSlot(int slot)
		{
			bool flag = this._highlightedHotbarSlot == slot || this._itemGrid.Slots[slot] == null;
			if (!flag)
			{
				this.ApplySlotHighlight(slot);
				bool isMounted = this._itemGrid.IsMounted;
				if (isMounted)
				{
					this._itemGrid.Layout(null, true);
				}
				this._highlightedHotbarSlot = slot;
			}
		}

		// Token: 0x06003DA9 RID: 15785 RVA: 0x000A0340 File Offset: 0x0009E540
		private void ApplySlotHighlight(int slot)
		{
			this._itemGrid.Slots[slot].Overlay = this._highlightedSlotOverlay;
		}

		// Token: 0x06003DAA RID: 15786 RVA: 0x000A035C File Offset: 0x0009E55C
		public void ClearSlotHighlight()
		{
			bool flag = this._highlightedHotbarSlot == -1 || this._itemGrid.Slots[this._highlightedHotbarSlot] == null;
			if (!flag)
			{
				this._itemGrid.Slots[this._highlightedHotbarSlot].Overlay = null;
				bool isMounted = this._itemGrid.IsMounted;
				if (isMounted)
				{
					this._itemGrid.Layout(null, true);
				}
				this._highlightedHotbarSlot = -1;
			}
		}

		// Token: 0x06003DAB RID: 15787 RVA: 0x000A03D8 File Offset: 0x0009E5D8
		public void UpdateActiveItemNameLabel()
		{
			ClientItemStack hotbarItem = this.InGameView.GetHotbarItem(this._activeHotbarSlot);
			bool flag = this.InGameView.InGame.CurrentPage == 2 || this.InGameView.InGame.CurrentPage == 1;
			if (flag)
			{
				this._activeHotbarItemId = ((hotbarItem != null) ? hotbarItem.Id : null);
				this._activeItemNameLabel.Visible = false;
				this._activeHotbarItemNameLabelTime = 0f;
			}
			else
			{
				bool flag2 = ((hotbarItem != null) ? hotbarItem.Id : null) != this._activeHotbarItemId;
				if (flag2)
				{
					this._activeHotbarItemId = ((hotbarItem != null) ? hotbarItem.Id : null);
					bool flag3 = this._activeHotbarItemId == null;
					if (flag3)
					{
						this._activeItemNameLabel.Visible = false;
						this._activeHotbarItemNameLabelTime = 0f;
					}
					else
					{
						this._activeItemNameLabel.Text = this.Desktop.Provider.GetText("items." + this._activeHotbarItemId + ".name", null, true);
						this._activeItemNameLabel.Visible = true;
						this._activeItemNameLabel.Layout(new Rectangle?(this._activeItemNameLabel.Parent.RectangleAfterPadding), true);
						this._activeHotbarItemNameLabelTime = 1.25f;
					}
				}
			}
		}

		// Token: 0x06003DAC RID: 15788 RVA: 0x000A0520 File Offset: 0x0009E720
		public void ResetState()
		{
			this._itemGrid.AreItemsDraggable = false;
			this._itemGrid.Slots = new ItemGridSlot[this.InGameView.DefaultItemSlotsPerRow];
			this._activeHotbarItemNameLabelTime = 0f;
			this._activeItemNameLabel.Visible = false;
			this._activeHotbarSlot = 0;
			this.UpdateActiveHotbarSlotOverlay();
			this._activeUtilitySlot = -1;
			this._utilityItemGrid.Slots[0] = null;
			this._activeConsumableSlot = -1;
			this._consumableItemGrid.Slots[0] = null;
			this._highlightedHotbarSlot = -1;
		}

		// Token: 0x04001CAF RID: 7343
		public readonly InGameView InGameView;

		// Token: 0x04001CB0 RID: 7344
		private ItemGrid _itemGrid;

		// Token: 0x04001CB1 RID: 7345
		private ItemGrid _utilityItemGrid;

		// Token: 0x04001CB2 RID: 7346
		private ItemGrid _consumableItemGrid;

		// Token: 0x04001CB3 RID: 7347
		private Group _consumableSlotPointer;

		// Token: 0x04001CB4 RID: 7348
		private Group _consumableSlotContainer;

		// Token: 0x04001CB5 RID: 7349
		private Group _utilitySlotPointer;

		// Token: 0x04001CB6 RID: 7350
		private Group _utilitySlotContainer;

		// Token: 0x04001CB7 RID: 7351
		private Group _background;

		// Token: 0x04001CB8 RID: 7352
		private Group _container;

		// Token: 0x04001CB9 RID: 7353
		private Element _activeSlotOverlay;

		// Token: 0x04001CBA RID: 7354
		private Element _activeSlotSwitchableOverlay;

		// Token: 0x04001CBB RID: 7355
		private int _slotCount;

		// Token: 0x04001CBC RID: 7356
		private readonly List<Label> _hotkeyLabels = new List<Label>();

		// Token: 0x04001CBD RID: 7357
		private Label _activeItemNameLabel;

		// Token: 0x04001CBE RID: 7358
		private int _activeHotbarSlot;

		// Token: 0x04001CBF RID: 7359
		private string _activeHotbarItemId;

		// Token: 0x04001CC0 RID: 7360
		private float _activeHotbarItemNameLabelTime;

		// Token: 0x04001CC1 RID: 7361
		private int _highlightedHotbarSlot = -1;

		// Token: 0x04001CC2 RID: 7362
		private PatchStyle _highlightedSlotOverlay;

		// Token: 0x04001CC3 RID: 7363
		private int _activeUtilitySlot;

		// Token: 0x04001CC4 RID: 7364
		private int _activeConsumableSlot;

		// Token: 0x04001CC5 RID: 7365
		private int _inventoryOpenedContainerMargin;

		// Token: 0x04001CC6 RID: 7366
		private int _inventoryClosedContainerMargin;
	}
}
