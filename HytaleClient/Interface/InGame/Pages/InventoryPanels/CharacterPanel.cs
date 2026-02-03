using System;
using System.Runtime.CompilerServices;
using HytaleClient.Application;
using HytaleClient.Data.EntityStats;
using HytaleClient.Data.Items;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Interface.UI.Styles;
using HytaleClient.Math;
using HytaleClient.Protocol;

namespace HytaleClient.Interface.InGame.Pages.InventoryPanels
{
	// Token: 0x02000896 RID: 2198
	internal class CharacterPanel : Panel
	{
		// Token: 0x170010B3 RID: 4275
		// (get) Token: 0x06003F43 RID: 16195 RVA: 0x000AFD0F File Offset: 0x000ADF0F
		// (set) Token: 0x06003F44 RID: 16196 RVA: 0x000AFD17 File Offset: 0x000ADF17
		public Group Panel { get; private set; }

		// Token: 0x06003F45 RID: 16197 RVA: 0x000AFD20 File Offset: 0x000ADF20
		public CharacterPanel(InGameView inGameView, Element parent = null) : base(inGameView, parent)
		{
		}

		// Token: 0x06003F46 RID: 16198 RVA: 0x000AFD3C File Offset: 0x000ADF3C
		protected override void OnMounted()
		{
			this._nameLabel.Text = this._inGameView.Interface.App.AuthManager.Settings.Username;
			this.UpdateCharacterVisibility(false);
			this.UpdateInputBindings(true);
			this.UpdateGrid();
			this.UpdateCompatibleSlotHighlight(true);
		}

		// Token: 0x06003F47 RID: 16199 RVA: 0x000AFD94 File Offset: 0x000ADF94
		public void Build()
		{
			base.Clear();
			Document document;
			this.Interface.TryGetDocument("InGame/Pages/Inventory/CharacterPanel.ui", out document);
			this._specialSlotBackground = document.ResolveNamedValue<PatchStyle>(this.Desktop.Provider, "SpecialSlotBackground");
			this._specialSlotCompatibleBackground = document.ResolveNamedValue<PatchStyle>(this.Desktop.Provider, "SpecialSlotCompatibleBackground");
			UIFragment uifragment = document.Instantiate(this.Desktop, this);
			this.Panel = uifragment.Get<Group>("Panel");
			this._nameLabel = uifragment.Get<Label>("NameLabel");
			this._previewContainer = uifragment.Get<CharacterPreviewComponent>("PreviewContainer");
			this._statsHealth = uifragment.Get<Label>("StatsHealth");
			this._statsHealthGain = uifragment.Get<Label>("StatsHealthGain");
			this._statsMana = uifragment.Get<Label>("StatsMana");
			this._statsManaGain = uifragment.Get<Label>("StatsManaGain");
			this._statsArmorRating = uifragment.Get<Label>("StatsArmorRating");
			this._statsMeleePower = uifragment.Get<Label>("StatsMeleePower");
			this._statsRangedPower = uifragment.Get<Label>("StatsRangedPower");
			this._statsMagicPower = uifragment.Get<Label>("StatsMagicPower");
			this._utilitySlotInputBinding = uifragment.Get<Label>("UtilitySlotInputBinding");
			this._consumableSlotInputBinding = uifragment.Get<Label>("ConsumableSlotInputBinding");
			this._specialSlotBackdrop = uifragment.Get<Group>("SpecialSlotBackdrop");
			this._itemGridLeft = uifragment.Get<ItemGrid>("ItemGridLeft");
			this._itemGridLeft.Slots = new ItemGridSlot[3];
			this._itemGridLeft.InventorySectionId = new int?(-3);
			this._itemGridLeft.SlotClicking = delegate(int slotIndex, int button)
			{
				this._inGameView.HandleInventoryClick(-3, CharacterPanel.<Build>g__TranslateArmorSlotIndex|27_0(slotIndex, 0), button);
			};
			this._itemGridLeft.Dropped = delegate(int targetSlotIndex, Element sourceItemGrid, ItemGrid.ItemDragData dragData)
			{
				this._inGameView.HandleInventoryDragEnd(this._itemGridLeft, -3, CharacterPanel.<Build>g__TranslateArmorSlotIndex|27_0(targetSlotIndex, 0), sourceItemGrid, dragData);
			};
			this._itemGridLeft.DragCancelled = delegate(int slotIndex, int button)
			{
				this._inGameView.HandleInventoryDropItem(-3, CharacterPanel.<Build>g__TranslateArmorSlotIndex|27_0(slotIndex, 0), button);
			};
			this._itemGridLeft.SlotMouseEntered = delegate(int slotIndex)
			{
				bool flag2 = slotIndex == 2;
				if (flag2)
				{
					this.OnSpecialSlotMouseEntered(-5, slotIndex);
				}
				else
				{
					this.OnArmorSlotMouseEntered(slotIndex);
				}
			};
			this._itemGridLeft.SlotMouseExited = delegate(int slotIndex)
			{
				bool flag2 = slotIndex == 2;
				if (flag2)
				{
					this.OnSpecialSlotMouseExited(-5);
				}
				else
				{
					this.OnArmorSlotMouseExited(slotIndex);
				}
			};
			this._itemGridRight = uifragment.Get<ItemGrid>("ItemGridRight");
			this._itemGridRight.Slots = new ItemGridSlot[3];
			this._itemGridRight.InventorySectionId = new int?(-3);
			this._itemGridRight.SlotClicking = delegate(int slotIndex, int button)
			{
				this._inGameView.HandleInventoryClick(-3, CharacterPanel.<Build>g__TranslateArmorSlotIndex|27_0(slotIndex, 2), button);
			};
			this._itemGridRight.Dropped = delegate(int targetSlotIndex, Element sourceItemGrid, ItemGrid.ItemDragData dragData)
			{
				this._inGameView.HandleInventoryDragEnd(this._itemGridRight, -3, CharacterPanel.<Build>g__TranslateArmorSlotIndex|27_0(targetSlotIndex, 2), sourceItemGrid, dragData);
			};
			this._itemGridRight.DragCancelled = delegate(int slotIndex, int button)
			{
				this._inGameView.HandleInventoryDropItem(-3, CharacterPanel.<Build>g__TranslateArmorSlotIndex|27_0(slotIndex, 2), button);
			};
			this._itemGridRight.SlotMouseEntered = delegate(int slotIndex)
			{
				bool flag2 = slotIndex == 2;
				if (flag2)
				{
					this.OnSpecialSlotMouseEntered(-6, slotIndex);
				}
				else
				{
					this.OnArmorSlotMouseEntered(slotIndex + 2);
				}
			};
			this._itemGridRight.SlotMouseExited = delegate(int slotIndex)
			{
				bool flag2 = slotIndex == 2;
				if (flag2)
				{
					this.OnSpecialSlotMouseExited(-6);
				}
				else
				{
					this.OnArmorSlotMouseExited(slotIndex + 2);
				}
			};
			this._itemGridBottom = uifragment.Get<ItemGrid>("ItemGridBottom");
			this._itemGridBottom.InventorySectionId = new int?(-3);
			this._itemGridBottom.Slots = new ItemGridSlot[3];
			for (int i = 0; i < this._itemGridBottom.Slots.Length; i++)
			{
				this._itemGridBottom.Slots[i] = new ItemGridSlot
				{
					Icon = new PatchStyle("InGame/Pages/Inventory/RingSlotIconSpecial.png")
				};
			}
			int ringSlotOffset = 5;
			this._itemGridBottom.SlotClicking = delegate(int slotIndex, int button)
			{
				this._inGameView.HandleInventoryClick(-3, slotIndex + ringSlotOffset, button);
			};
			this._itemGridBottom.Dropped = delegate(int targetSlotIndex, Element sourceItemGrid, ItemGrid.ItemDragData dragData)
			{
				this._inGameView.HandleInventoryDragEnd(this._itemGridBottom, -3, targetSlotIndex + ringSlotOffset, sourceItemGrid, dragData);
			};
			this._itemGridBottom.DragCancelled = delegate(int slotIndex, int button)
			{
				this._inGameView.HandleInventoryDropItem(-3, slotIndex + ringSlotOffset, button);
			};
			this._itemGridBottom.SlotMouseEntered = delegate(int slotIndex)
			{
				this.OnArmorSlotMouseEntered(slotIndex + ringSlotOffset);
			};
			this._itemGridBottom.SlotMouseExited = delegate(int slotIndex)
			{
				this.OnArmorSlotMouseExited(slotIndex + ringSlotOffset);
			};
			bool flag = this._inGameView.ArmorStacks != null;
			if (flag)
			{
				this.UpdateGrid();
			}
			bool isMounted = base.IsMounted;
			if (isMounted)
			{
				this.UpdateCharacterVisibility(false);
				this.UpdateInputBindings(false);
			}
		}

		// Token: 0x06003F48 RID: 16200 RVA: 0x000B0178 File Offset: 0x000AE378
		private void OnSpecialSlotMouseEntered(int sectionId, int slotIndex)
		{
			this._inGameView.HandleItemSlotMouseEntered(sectionId, 0);
			this._inGameView.InventoryPage.StoragePanel.UpdateGrid();
			this._inGameView.HotbarComponent.SetupGrid();
			ItemGrid.ItemDragData itemDragData = this._inGameView.ItemDragData;
			ClientItemStack clientItemStack = (itemDragData != null) ? itemDragData.ItemStack : null;
			bool flag = clientItemStack != null;
			if (flag)
			{
				ClientItemBase clientItemBase;
				bool flag2 = !this._inGameView.Items.TryGetValue(clientItemStack.Id, out clientItemBase);
				if (flag2)
				{
					return;
				}
				bool flag3 = sectionId == -5;
				if (flag3)
				{
					bool flag4 = clientItemBase.Utility == null || !clientItemBase.Utility.Usable;
					if (flag4)
					{
						return;
					}
				}
				else
				{
					bool flag5 = !clientItemBase.Consumable;
					if (flag5)
					{
						return;
					}
				}
			}
			ItemSlotSelectorPopover itemSelectorPopover = this._inGameView.InventoryPage.ItemSelectorPopover;
			float scale = this.Desktop.Scale;
			ItemGrid itemGrid = (sectionId == -5) ? this._itemGridLeft : this._itemGridRight;
			float num = (float)slotIndex + 0.5f;
			float num2 = (float)(-(float)this._rectangleAfterPadding.Left) / scale + (float)itemGrid.RectangleAfterPadding.Left / scale - (float)itemSelectorPopover.Anchor.Width.Value / 2f + (float)(itemGrid.Style.SlotSize + itemGrid.Style.SlotSpacing) / 2f;
			float num3 = (float)(-(float)this._rectangleAfterPadding.Top + itemGrid.RectangleAfterPadding.Top) / scale + (float)(itemGrid.Style.SlotSize + itemGrid.Style.SlotSpacing) * num - (float)itemSelectorPopover.Anchor.Height.Value / 2f;
			int activeSlot = (sectionId == -5) ? this._activeUtilitySlot : this._activeConsumableSlot;
			itemSelectorPopover.Setup(sectionId, activeSlot, (int)Math.Round((double)num2), (int)Math.Round((double)num3));
			itemSelectorPopover.Visible = true;
			itemSelectorPopover.Layout(new Rectangle?(this._rectangleAfterPadding), true);
			this.UpdateCompatibleSlotHighlight(true);
		}

		// Token: 0x06003F49 RID: 16201 RVA: 0x000B0394 File Offset: 0x000AE594
		private void OnSpecialSlotMouseExited(int sectionId)
		{
			this._inGameView.HandleItemSlotMouseExited(sectionId, 0);
		}

		// Token: 0x06003F4A RID: 16202 RVA: 0x000B03A8 File Offset: 0x000AE5A8
		public void OnSetActiveUtilitySlot(int activeSlot)
		{
			this._activeUtilitySlot = activeSlot;
			bool flag = !base.IsMounted;
			if (!flag)
			{
				this.UpdateSelectorSlots();
				ItemSlotSelectorPopover itemSelectorPopover = this._inGameView.InventoryPage.ItemSelectorPopover;
				bool flag2 = itemSelectorPopover.InventorySectionId == -5 && itemSelectorPopover.IsMounted;
				if (flag2)
				{
					itemSelectorPopover.SelectedSlot = activeSlot + 1;
				}
			}
		}

		// Token: 0x06003F4B RID: 16203 RVA: 0x000B0408 File Offset: 0x000AE608
		public void OnSetActiveConsumableSlot(int activeSlot)
		{
			this._activeConsumableSlot = activeSlot;
			bool flag = !base.IsMounted;
			if (!flag)
			{
				this.UpdateSelectorSlots();
				ItemSlotSelectorPopover itemSelectorPopover = this._inGameView.InventoryPage.ItemSelectorPopover;
				bool flag2 = itemSelectorPopover.InventorySectionId == -6 && itemSelectorPopover.IsMounted;
				if (flag2)
				{
					itemSelectorPopover.SelectedSlot = activeSlot + 1;
				}
			}
		}

		// Token: 0x06003F4C RID: 16204 RVA: 0x000B0468 File Offset: 0x000AE668
		private void OnArmorSlotMouseEntered(int slotIndex)
		{
			bool isMouseDragging = this.Desktop.IsMouseDragging;
			if (!isMouseDragging)
			{
				this._inGameView.HandleItemSlotMouseEntered(-3, slotIndex);
				this._inGameView.InventoryPage.StoragePanel.UpdateGrid();
				this._inGameView.HotbarComponent.SetupGrid();
			}
		}

		// Token: 0x06003F4D RID: 16205 RVA: 0x000B04BD File Offset: 0x000AE6BD
		private void OnArmorSlotMouseExited(int slotIndex)
		{
			this._inGameView.HandleItemSlotMouseExited(-3, slotIndex);
			this._inGameView.InventoryPage.StoragePanel.UpdateGrid();
			this._inGameView.HotbarComponent.SetupGrid();
		}

		// Token: 0x06003F4E RID: 16206 RVA: 0x000B04F8 File Offset: 0x000AE6F8
		public void UpdateGrid()
		{
			for (int i = 0; i <= 4; i++)
			{
				ItemGrid itemGrid = (i > 1) ? this._itemGridRight : this._itemGridLeft;
				int num = (i > 1) ? (i - 2) : i;
				itemGrid.Slots[num] = new ItemGridSlot(this._inGameView.ArmorStacks[i])
				{
					InventorySlotIndex = new int?(i)
				};
				bool flag = this._inGameView.ArmorStacks[i] == null;
				if (flag)
				{
					ItemGridSlot[] slots = itemGrid.Slots;
					int num2 = num;
					ItemGridSlot itemGridSlot = new ItemGridSlot();
					string str = "InGame/Pages/Inventory/ArmorSlotIcon";
					ItemBase.ItemArmor.ItemArmorSlot itemArmorSlot = i;
					itemGridSlot.Icon = new PatchStyle(str + itemArmorSlot.ToString() + ".png");
					slots[num2] = itemGridSlot;
				}
			}
			int num3 = 5;
			for (int j = 0; j < this._itemGridBottom.Slots.Length; j++)
			{
				this._itemGridBottom.Slots[j] = new ItemGridSlot(this._inGameView.ArmorStacks[j + 5])
				{
					InventorySlotIndex = new int?(j + num3)
				};
				bool flag2 = this._inGameView.ArmorStacks[j + num3] == null;
				if (flag2)
				{
					this._itemGridBottom.Slots[j] = new ItemGridSlot
					{
						Icon = new PatchStyle("InGame/Pages/Inventory/RingSlotIconSpecial.png")
					};
				}
			}
			this._itemGridLeft.Layout(null, true);
			this._itemGridRight.Layout(null, true);
			this._itemGridBottom.Layout(null, true);
			this.UpdateSelectorSlots();
			ItemSlotSelectorPopover itemSelectorPopover = this._inGameView.InventoryPage.ItemSelectorPopover;
			bool isMounted = itemSelectorPopover.IsMounted;
			if (isMounted)
			{
				itemSelectorPopover.SetItemStacks(this._inGameView.GetItemStacks(itemSelectorPopover.InventorySectionId));
			}
		}

		// Token: 0x06003F4F RID: 16207 RVA: 0x000B06D8 File Offset: 0x000AE8D8
		public void UpdateCompatibleSlotHighlight(bool updateGrid = true)
		{
			AppInGame.ItemSelector compatibleItemSelectorItemInteraction = this._compatibleItemSelectorItemInteraction;
			bool isMounted = this._inGameView.InventoryPage.ItemSelectorPopover.IsMounted;
			if (isMounted)
			{
				this._compatibleItemSelectorItemInteraction = ((this._inGameView.InventoryPage.ItemSelectorPopover.InventorySectionId == -5) ? AppInGame.ItemSelector.Utility : AppInGame.ItemSelector.Consumable);
			}
			else
			{
				ItemGrid.ItemDragData itemDragData = this._inGameView.ItemDragData;
				ClientItemStack clientItemStack = (itemDragData != null) ? itemDragData.ItemStack : null;
				bool flag = clientItemStack == null && this._inGameView.HoveredItemSlot != null;
				if (flag)
				{
					ClientItemStack[] itemStacks = this._inGameView.GetItemStacks(this._inGameView.HoveredItemSlot.InventorySectionId);
					clientItemStack = itemStacks[this._inGameView.HoveredItemSlot.SlotId];
				}
				ClientItemBase clientItemBase;
				bool flag2 = clientItemStack != null && this._inGameView.Items.TryGetValue(clientItemStack.Id, out clientItemBase);
				if (flag2)
				{
					bool consumable = clientItemBase.Consumable;
					if (consumable)
					{
						this._compatibleItemSelectorItemInteraction = AppInGame.ItemSelector.Consumable;
					}
					else
					{
						bool flag3 = clientItemBase.Utility != null && clientItemBase.Utility.Usable;
						if (flag3)
						{
							this._compatibleItemSelectorItemInteraction = AppInGame.ItemSelector.Utility;
						}
						else
						{
							this._compatibleItemSelectorItemInteraction = AppInGame.ItemSelector.None;
						}
					}
				}
				else
				{
					this._compatibleItemSelectorItemInteraction = AppInGame.ItemSelector.None;
				}
			}
			bool flag4 = updateGrid && compatibleItemSelectorItemInteraction != this._compatibleItemSelectorItemInteraction;
			if (flag4)
			{
				this.UpdateSelectorSlots();
				this.SetupSpecialSlotBackdrop();
			}
		}

		// Token: 0x06003F50 RID: 16208 RVA: 0x000B0834 File Offset: 0x000AEA34
		private void SetupSpecialSlotBackdrop()
		{
			bool flag = !this._inGameView.InventoryPage.ItemSelectorPopover.IsMounted;
			if (flag)
			{
				this._specialSlotBackdrop.Visible = false;
			}
			else
			{
				ItemGrid itemGrid = (this._compatibleItemSelectorItemInteraction == AppInGame.ItemSelector.Utility) ? this._itemGridLeft : this._itemGridRight;
				float scale = this.Desktop.Scale;
				int num = itemGrid.Style.SlotSize + itemGrid.Style.SlotSpacing;
				int num2 = this._specialSlotBackdrop.Anchor.Width.Value / 2;
				int num3 = this._specialSlotBackdrop.Anchor.Height.Value / 2;
				float num4 = (float)(itemGrid.RectangleAfterPadding.Left - this._specialSlotBackdrop.Parent.RectangleAfterPadding.Left) / scale + (float)(num / 2) - (float)num2;
				double a = (double)((float)(itemGrid.RectangleAfterPadding.Top - this._specialSlotBackdrop.Parent.RectangleAfterPadding.Top) / scale) + (double)num * 2.5 - (double)num3;
				this._specialSlotBackdrop.Anchor.Left = new int?((int)Math.Round((double)num4));
				this._specialSlotBackdrop.Anchor.Top = new int?((int)Math.Round(a));
				this._specialSlotBackdrop.Visible = true;
				this._specialSlotBackdrop.Layout(new Rectangle?(this._specialSlotBackdrop.Parent.RectangleAfterPadding), true);
			}
		}

		// Token: 0x06003F51 RID: 16209 RVA: 0x000B09C4 File Offset: 0x000AEBC4
		private void UpdateSelectorSlots()
		{
			bool flag = this._activeConsumableSlot > -1 && this._inGameView.ConsumableStacks[this._activeConsumableSlot] != null;
			if (flag)
			{
				this._itemGridRight.Slots[2] = new ItemGridSlot(this._inGameView.ConsumableStacks[this._activeConsumableSlot])
				{
					SkipItemQualityBackground = true,
					Background = ((this._compatibleItemSelectorItemInteraction == AppInGame.ItemSelector.Consumable) ? this._specialSlotCompatibleBackground : this._specialSlotBackground)
				};
			}
			else
			{
				this._itemGridRight.Slots[2] = new ItemGridSlot
				{
					Icon = new PatchStyle("InGame/Pages/Inventory/ConsumableSlotIcon.png"),
					Background = ((this._compatibleItemSelectorItemInteraction == AppInGame.ItemSelector.Consumable) ? this._specialSlotCompatibleBackground : this._specialSlotBackground)
				};
			}
			bool flag2 = this._activeUtilitySlot > -1 && this._inGameView.UtilityStacks[this._activeUtilitySlot] != null;
			if (flag2)
			{
				this._itemGridLeft.Slots[2] = new ItemGridSlot(this._inGameView.UtilityStacks[this._activeUtilitySlot])
				{
					SkipItemQualityBackground = true,
					Background = ((this._compatibleItemSelectorItemInteraction == AppInGame.ItemSelector.Utility) ? this._specialSlotCompatibleBackground : this._specialSlotBackground)
				};
			}
			else
			{
				this._itemGridLeft.Slots[2] = new ItemGridSlot
				{
					Icon = new PatchStyle("InGame/Pages/Inventory/UtilitySlotIcon.png"),
					Background = ((this._compatibleItemSelectorItemInteraction == AppInGame.ItemSelector.Utility) ? this._specialSlotCompatibleBackground : this._specialSlotBackground)
				};
			}
			this._itemGridRight.Layout(null, true);
			this._itemGridLeft.Layout(null, true);
		}

		// Token: 0x06003F52 RID: 16210 RVA: 0x000B0B64 File Offset: 0x000AED64
		public void OnHealthChanged(ClientEntityStatValue health)
		{
			this._statsHealth.Text = string.Format("{0}/{1}", MathHelper.Round(health.Value), MathHelper.Round(health.Max));
			this._statsHealth.Layout(null, true);
		}

		// Token: 0x06003F53 RID: 16211 RVA: 0x000B0BC0 File Offset: 0x000AEDC0
		public void OnManaChanged(ClientEntityStatValue mana)
		{
			this._statsMana.Text = string.Format("{0}/{1}", MathHelper.Round(mana.Value), MathHelper.Round(mana.Max));
			this._statsMana.Layout(null, true);
		}

		// Token: 0x06003F54 RID: 16212 RVA: 0x000B0C1C File Offset: 0x000AEE1C
		private void UpdateInputBindings(bool doLayout = true)
		{
			this._utilitySlotInputBinding.Text = this._inGameView.Interface.App.Settings.InputBindings.ShowUtilitySlotSelector.BoundInputLabel;
			if (doLayout)
			{
				this._utilitySlotInputBinding.Layout(null, true);
			}
			this._consumableSlotInputBinding.Text = this._inGameView.Interface.App.Settings.InputBindings.ShowConsumableSlotSelector.BoundInputLabel;
			if (doLayout)
			{
				this._consumableSlotInputBinding.Layout(null, true);
			}
		}

		// Token: 0x06003F55 RID: 16213 RVA: 0x000B0CC0 File Offset: 0x000AEEC0
		public void UpdateCharacterVisibility(bool doLayout = true)
		{
			this._previewContainer.Visible = (this.Desktop.GetLayer(2) == null);
			bool flag = this._previewContainer.Visible && doLayout;
			if (flag)
			{
				this._previewContainer.Layout(new Rectangle?(this._previewContainer.Parent.RectangleAfterPadding), true);
			}
		}

		// Token: 0x06003F56 RID: 16214 RVA: 0x000B0D1C File Offset: 0x000AEF1C
		public void OnItemSlotSelectorClosed()
		{
			this.UpdateCompatibleSlotHighlight(false);
			this.UpdateSelectorSlots();
			this._specialSlotBackdrop.Visible = false;
			this._itemGridBottom.RefreshMouseOver(false);
			this._inGameView.InventoryPage.StoragePanel.UpdateGrid();
			this._inGameView.HotbarComponent.SetupGrid();
		}

		// Token: 0x06003F57 RID: 16215 RVA: 0x000B0D7A File Offset: 0x000AEF7A
		[CompilerGenerated]
		internal static int <Build>g__TranslateArmorSlotIndex|27_0(int index, int offset)
		{
			return ((index >= 2) ? (index + 3) : index) + offset;
		}

		// Token: 0x04001DF6 RID: 7670
		private Label _nameLabel;

		// Token: 0x04001DF7 RID: 7671
		private CharacterPreviewComponent _previewContainer;

		// Token: 0x04001DF8 RID: 7672
		private ItemGrid _itemGridLeft;

		// Token: 0x04001DF9 RID: 7673
		private ItemGrid _itemGridRight;

		// Token: 0x04001DFA RID: 7674
		private ItemGrid _itemGridBottom;

		// Token: 0x04001DFB RID: 7675
		private Group _specialSlotBackdrop;

		// Token: 0x04001DFC RID: 7676
		private Label _statsHealth;

		// Token: 0x04001DFD RID: 7677
		private Label _statsHealthGain;

		// Token: 0x04001DFE RID: 7678
		private Label _statsMana;

		// Token: 0x04001DFF RID: 7679
		private Label _statsManaGain;

		// Token: 0x04001E00 RID: 7680
		private Label _statsArmorRating;

		// Token: 0x04001E01 RID: 7681
		private Label _statsMeleePower;

		// Token: 0x04001E02 RID: 7682
		private Label _statsRangedPower;

		// Token: 0x04001E03 RID: 7683
		private Label _statsMagicPower;

		// Token: 0x04001E04 RID: 7684
		private Label _utilitySlotInputBinding;

		// Token: 0x04001E05 RID: 7685
		private Label _consumableSlotInputBinding;

		// Token: 0x04001E06 RID: 7686
		private PatchStyle _specialSlotBackground;

		// Token: 0x04001E07 RID: 7687
		private PatchStyle _specialSlotCompatibleBackground;

		// Token: 0x04001E08 RID: 7688
		private int _activeUtilitySlot = -1;

		// Token: 0x04001E09 RID: 7689
		private int _activeConsumableSlot = -1;

		// Token: 0x04001E0A RID: 7690
		private AppInGame.ItemSelector _compatibleItemSelectorItemInteraction;
	}
}
