using System;
using HytaleClient.Data.Items;
using HytaleClient.InGame.Modules;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Interface.UI.Styles;
using HytaleClient.Protocol;

namespace HytaleClient.Interface.InGame.Pages.InventoryPanels
{
	// Token: 0x0200089F RID: 2207
	internal class StoragePanel : Panel
	{
		// Token: 0x170010BC RID: 4284
		// (get) Token: 0x06003FDB RID: 16347 RVA: 0x000B5EBA File Offset: 0x000B40BA
		public string ActiveItemFilter
		{
			get
			{
				return this._filter.SelectedTab;
			}
		}

		// Token: 0x170010BD RID: 4285
		// (get) Token: 0x06003FDC RID: 16348 RVA: 0x000B5EC7 File Offset: 0x000B40C7
		// (set) Token: 0x06003FDD RID: 16349 RVA: 0x000B5ECF File Offset: 0x000B40CF
		public int Offset { get; private set; }

		// Token: 0x06003FDE RID: 16350 RVA: 0x000B5ED8 File Offset: 0x000B40D8
		public StoragePanel(InGameView inGameView, Element parent = null) : base(inGameView, parent)
		{
		}

		// Token: 0x06003FDF RID: 16351 RVA: 0x000B5EEC File Offset: 0x000B40EC
		public void Build()
		{
			base.Clear();
			this._highlightedSlotOverlay = new PatchStyle("InGame/Pages/Inventory/SlotHighlight.png");
			Document document;
			this.Interface.TryGetDocument("InGame/Pages/Inventory/StoragePanel.ui", out document);
			UIFragment uifragment = document.Instantiate(this.Desktop, this);
			this.Offset = document.ResolveNamedValue<int>(this.Interface, "Offset");
			Group parent = uifragment.Get<Group>("AutosortTypeDropdownContainer");
			this._autosortTypeDropdown = new AutosortTypeDropdown(this.Desktop, parent)
			{
				SortType = this._autosortType,
				SortTypeChanged = new Action(this.SortItems)
			};
			this._autosortTypeDropdown.Build();
			this._filter = uifragment.Get<TabNavigation>("Filter");
			this._filter.SelectedTabChanged = delegate()
			{
				this.UpdateGrid();
				this._inGameView.HotbarComponent.SetupGrid();
				bool isMounted = this._inGameView.InventoryPage.ContainerPanel.IsMounted;
				if (isMounted)
				{
					this._inGameView.InventoryPage.ContainerPanel.SetupGrid();
				}
			};
			uifragment.Get<TextButton>("AutosortButton").Activating = new Action(this.SortItems);
			this._itemGrid = uifragment.Get<ItemGrid>("ItemGrid");
			this._itemGrid.Slots = new ItemGridSlot[4 * this._inGameView.DefaultItemSlotsPerRow];
			this._itemGrid.InventorySectionId = new int?(-2);
			this._itemGrid.SlotClicking = delegate(int slotIndex, int button)
			{
				this._inGameView.HandleInventoryClick(-2, slotIndex, button);
			};
			this._itemGrid.SlotDoubleClicking = delegate(int slotIndex)
			{
				this._inGameView.HandleInventoryDoubleClick(-2, slotIndex);
			};
			this._itemGrid.Dropped = delegate(int targetSlotIndex, Element sourceItemGrid, ItemGrid.ItemDragData dragData)
			{
				this._inGameView.HandleInventoryDragEnd(this._itemGrid, -2, targetSlotIndex, sourceItemGrid, dragData);
			};
			this._itemGrid.DragCancelled = delegate(int slotIndex, int button)
			{
				this._inGameView.HandleInventoryDropItem(-2, slotIndex, button);
			};
			this._itemGrid.SlotMouseEntered = delegate(int slotIndex)
			{
				this._inGameView.HandleItemSlotMouseEntered(-2, slotIndex);
			};
			this._itemGrid.SlotMouseExited = delegate(int slotIndex)
			{
				this._inGameView.HandleItemSlotMouseExited(-2, slotIndex);
			};
			bool flag = this._inGameView.StorageStacks != null;
			if (flag)
			{
				this.UpdateGrid();
			}
		}

		// Token: 0x06003FE0 RID: 16352 RVA: 0x000B60B9 File Offset: 0x000B42B9
		protected override void OnUnmounted()
		{
			this._filter.SelectedTab = null;
		}

		// Token: 0x06003FE1 RID: 16353 RVA: 0x000B60C8 File Offset: 0x000B42C8
		protected override void OnMounted()
		{
			this.UpdateGrid();
		}

		// Token: 0x06003FE2 RID: 16354 RVA: 0x000B60D2 File Offset: 0x000B42D2
		public void SetSortType(SortType type)
		{
			this._autosortType = type;
		}

		// Token: 0x06003FE3 RID: 16355 RVA: 0x000B60DB File Offset: 0x000B42DB
		private void SortItems()
		{
			this._inGameView.InGame.SendSortInventoryPacket(this._autosortTypeDropdown.SortType);
		}

		// Token: 0x06003FE4 RID: 16356 RVA: 0x000B60FC File Offset: 0x000B42FC
		public void HighlightSlot(int slot)
		{
			bool flag = this._highlightedSlot == slot || this._itemGrid.Slots[slot] == null;
			if (!flag)
			{
				this.ApplySlotHighlight(slot);
				bool isMounted = this._itemGrid.IsMounted;
				if (isMounted)
				{
					this._itemGrid.Layout(null, true);
				}
				this._highlightedSlot = slot;
			}
		}

		// Token: 0x06003FE5 RID: 16357 RVA: 0x000B6160 File Offset: 0x000B4360
		private void ApplySlotHighlight(int slot)
		{
			this._itemGrid.Slots[slot].Overlay = this._highlightedSlotOverlay;
		}

		// Token: 0x06003FE6 RID: 16358 RVA: 0x000B617C File Offset: 0x000B437C
		public void ClearSlotHighlight()
		{
			bool flag = this._highlightedSlot == -1 || this._itemGrid.Slots[this._highlightedSlot] == null;
			if (!flag)
			{
				this._itemGrid.Slots[this._highlightedSlot].Overlay = null;
				bool isMounted = this._itemGrid.IsMounted;
				if (isMounted)
				{
					this._itemGrid.Layout(null, true);
				}
				this._highlightedSlot = -1;
			}
		}

		// Token: 0x06003FE7 RID: 16359 RVA: 0x000B61F8 File Offset: 0x000B43F8
		public void UpdateGrid()
		{
			this._itemGrid.Slots = new ItemGridSlot[this._inGameView.StorageStacks.Length];
			for (int i = 0; i < this._inGameView.StorageStacks.Length; i++)
			{
				ClientItemStack clientItemStack = this._inGameView.StorageStacks[i];
				bool flag = false;
				bool flag2 = this.ActiveItemFilter != null;
				if (flag2)
				{
					bool flag3 = clientItemStack == null;
					if (flag3)
					{
						flag = true;
					}
					else
					{
						ClientItemBase clientItemBase;
						bool flag4 = this._inGameView.Items.TryGetValue(clientItemStack.Id, out clientItemBase);
						if (flag4)
						{
							string activeItemFilter = this.ActiveItemFilter;
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
											flag = true;
										}
									}
								}
								else
								{
									bool flag6 = clientItemBase.Armor == null;
									if (flag6)
									{
										flag = true;
									}
								}
							}
							else
							{
								bool flag7 = clientItemBase.Weapon == null;
								if (flag7)
								{
									flag = true;
								}
							}
						}
						else
						{
							flag = true;
						}
					}
				}
				else
				{
					bool flag8 = !this.Desktop.IsMouseDragging;
					if (flag8)
					{
						ClientItemBase item;
						bool flag9 = clientItemStack == null || !this._inGameView.Items.TryGetValue(clientItemStack.Id, out item);
						if (flag9)
						{
							item = null;
						}
						flag = !this._inGameView.IsItemValid(i, item, InventorySectionType.Storage);
					}
				}
				bool flag10 = clientItemStack == null && !flag;
				if (!flag10)
				{
					this._itemGrid.Slots[i] = new ItemGridSlot
					{
						ItemStack = clientItemStack,
						IsItemIncompatible = flag
					};
				}
			}
			bool flag11 = this._highlightedSlot >= 0 && this._itemGrid.Slots[this._highlightedSlot] != null;
			if (flag11)
			{
				this.ApplySlotHighlight(this._highlightedSlot);
			}
			bool isMounted = this._itemGrid.IsMounted;
			if (isMounted)
			{
				this._itemGrid.Layout(null, true);
			}
		}

		// Token: 0x04001E61 RID: 7777
		private ItemGrid _itemGrid;

		// Token: 0x04001E62 RID: 7778
		private TabNavigation _filter;

		// Token: 0x04001E63 RID: 7779
		private AutosortTypeDropdown _autosortTypeDropdown;

		// Token: 0x04001E64 RID: 7780
		private int _highlightedSlot = -1;

		// Token: 0x04001E65 RID: 7781
		private PatchStyle _highlightedSlotOverlay;

		// Token: 0x04001E66 RID: 7782
		private SortType _autosortType;
	}
}
