using System;
using HytaleClient.Data.Items;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Protocol;
using Newtonsoft.Json.Linq;

namespace HytaleClient.Interface.InGame.Pages.InventoryPanels
{
	// Token: 0x02000897 RID: 2199
	internal class ContainerPanel : WindowPanel
	{
		// Token: 0x06003F58 RID: 16216 RVA: 0x000B0D88 File Offset: 0x000AEF88
		public ContainerPanel(InGameView inGameView, Element parent = null) : base(inGameView, parent)
		{
		}

		// Token: 0x06003F59 RID: 16217 RVA: 0x000B0D94 File Offset: 0x000AEF94
		public void Build()
		{
			base.Clear();
			Document document;
			this.Interface.TryGetDocument("InGame/Pages/Inventory/ContainerPanel.ui", out document);
			UIFragment uifragment = document.Instantiate(this.Desktop, this);
			uifragment.Get<TextButton>("AutosortButton").Activating = new Action(this.SortItems);
			Group parent = uifragment.Get<Group>("AutosortTypeDropdownContainer");
			this._autosortTypeDropdown = new AutosortTypeDropdown(this.Desktop, parent)
			{
				SortType = this._autosortType,
				SortTypeChanged = new Action(this.SortItems)
			};
			this._autosortTypeDropdown.Build();
			this._titleLabel = uifragment.Get<Label>("TitleLabel");
			this._itemGrid = uifragment.Get<ItemGrid>("ItemGrid");
			this._itemGrid.Slots = new ItemGridSlot[this._inGameView.DefaultItemSlotsPerRow];
			this._itemGrid.SlotClicking = delegate(int slotIndex, int button)
			{
				this._inGameView.HandleInventoryClick(this._inventoryWindow.Id, slotIndex, button);
			};
			this._itemGrid.SlotDoubleClicking = delegate(int slotIndex)
			{
				this._inGameView.HandleInventoryDoubleClick(this._inventoryWindow.Id, slotIndex);
			};
			this._itemGrid.Dropped = delegate(int targetSlotIndex, Element sourceItemGrid, ItemGrid.ItemDragData dragData)
			{
				this._inGameView.HandleInventoryDragEnd(this._itemGrid, this._inventoryWindow.Id, targetSlotIndex, sourceItemGrid, dragData);
			};
			this._itemGrid.DragCancelled = delegate(int slotIndex, int button)
			{
				this._inGameView.HandleInventoryDropItem(this._inventoryWindow.Id, slotIndex, button);
			};
			this._itemGrid.SlotMouseEntered = delegate(int slotIndex)
			{
				this._inGameView.HandleItemSlotMouseEntered(this._inventoryWindow.Id, slotIndex);
			};
			this._itemGrid.SlotMouseExited = delegate(int slotIndex)
			{
				this._inGameView.HandleItemSlotMouseExited(this._inventoryWindow.Id, slotIndex);
			};
			uifragment.Get<TextButton>("TakeAllButton").Activating = delegate()
			{
				this._inGameView.InGame.SendTakeAllItemStacksPacket(this._inventoryWindow.Id);
			};
		}

		// Token: 0x06003F5A RID: 16218 RVA: 0x000B0F14 File Offset: 0x000AF114
		protected override void Setup()
		{
			this._titleLabel.Text = this.Desktop.Provider.GetText(((string)this._inventoryWindow.WindowData["name"]) ?? "", null, true);
			this._itemGrid.Slots = new ItemGridSlot[this._inventoryWindow.Inventory.Length];
			this._itemGrid.InventorySectionId = new int?(this._inventoryWindow.Id);
			this.SetupGrid();
		}

		// Token: 0x06003F5B RID: 16219 RVA: 0x000B0FA4 File Offset: 0x000AF1A4
		protected override void Update()
		{
			bool flag = this._inventoryWindow.Inventory == null;
			if (!flag)
			{
				this.SetupGrid();
			}
		}

		// Token: 0x06003F5C RID: 16220 RVA: 0x000B0FCD File Offset: 0x000AF1CD
		public void SetSortType(SortType type)
		{
			this._autosortType = type;
		}

		// Token: 0x06003F5D RID: 16221 RVA: 0x000B0FD8 File Offset: 0x000AF1D8
		private void SortItems()
		{
			JObject jobject = new JObject();
			jobject.Add("sortType", this._autosortTypeDropdown.SortType.ToString());
			JObject jobject2 = jobject;
			this._inGameView.InGame.SendSendWindowActionPacket(this._inventoryWindow.Id, "sortItems", jobject2.ToString());
		}

		// Token: 0x06003F5E RID: 16222 RVA: 0x000B103C File Offset: 0x000AF23C
		public void SetupGrid()
		{
			for (int i = 0; i < this._inventoryWindow.Inventory.Length; i++)
			{
				ClientItemStack clientItemStack = this._inventoryWindow.Inventory[i];
				string activeItemFilter = this._inGameView.InventoryPage.StoragePanel.ActiveItemFilter;
				bool flag = false;
				bool flag2 = activeItemFilter != null;
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
							string text = activeItemFilter;
							string a = text;
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
				bool flag8 = clientItemStack == null && !flag;
				if (flag8)
				{
					this._itemGrid.Slots[i] = null;
				}
				else
				{
					this._itemGrid.Slots[i] = new ItemGridSlot
					{
						ItemStack = clientItemStack,
						IsItemIncompatible = flag
					};
				}
			}
			this._itemGrid.Layout(null, true);
		}

		// Token: 0x04001E0B RID: 7691
		private Label _titleLabel;

		// Token: 0x04001E0C RID: 7692
		private ItemGrid _itemGrid;

		// Token: 0x04001E0D RID: 7693
		private AutosortTypeDropdown _autosortTypeDropdown;

		// Token: 0x04001E0E RID: 7694
		private SortType _autosortType;
	}
}
