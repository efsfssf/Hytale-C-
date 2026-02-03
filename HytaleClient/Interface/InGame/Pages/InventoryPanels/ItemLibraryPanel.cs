using System;
using System.Collections.Generic;
using System.Linq;
using HytaleClient.Data.Items;
using HytaleClient.Graphics;
using HytaleClient.Interface.InGame.Hud;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Interface.UI.Styles;
using HytaleClient.Protocol;

namespace HytaleClient.Interface.InGame.Pages.InventoryPanels
{
	// Token: 0x02000899 RID: 2201
	internal class ItemLibraryPanel : Panel
	{
		// Token: 0x170010B6 RID: 4278
		// (get) Token: 0x06003F82 RID: 16258 RVA: 0x000B2727 File Offset: 0x000B0927
		// (set) Token: 0x06003F83 RID: 16259 RVA: 0x000B272F File Offset: 0x000B092F
		public ItemGrid ItemGrid { get; private set; }

		// Token: 0x170010B7 RID: 4279
		// (get) Token: 0x06003F84 RID: 16260 RVA: 0x000B2738 File Offset: 0x000B0938
		// (set) Token: 0x06003F85 RID: 16261 RVA: 0x000B2740 File Offset: 0x000B0940
		public TextField SearchField { get; private set; }

		// Token: 0x170010B8 RID: 4280
		// (get) Token: 0x06003F86 RID: 16262 RVA: 0x000B2749 File Offset: 0x000B0949
		// (set) Token: 0x06003F87 RID: 16263 RVA: 0x000B2751 File Offset: 0x000B0951
		public string HoveredItemId { get; private set; }

		// Token: 0x06003F88 RID: 16264 RVA: 0x000B275A File Offset: 0x000B095A
		public ItemLibraryPanel(InGameView inGameView, Element parent = null) : base(inGameView, parent)
		{
		}

		// Token: 0x06003F89 RID: 16265 RVA: 0x000B2770 File Offset: 0x000B0970
		public void Build()
		{
			base.Clear();
			Document document;
			this.Interface.TryGetDocument("InGame/Pages/Inventory/ItemLibraryPanel.ui", out document);
			UIFragment uifragment = document.Instantiate(this.Desktop, this);
			this._primaryCategoryTabs = uifragment.Get<TabNavigation>("PrimaryCategoryTabs");
			this._primaryCategoryTabs.SelectedTabChanged = delegate()
			{
				this.SearchField.Value = "";
				this.SetSelectedTabs(this._primaryCategoryTabs.SelectedTab, null);
				this.UpdateItemLibrary();
				this.SetupSecondaryCategoryTabs();
				this._secondaryCategoryTabs.Parent.Layout(null, true);
			};
			this._secondaryCategoryTabs = uifragment.Get<TabNavigation>("SecondaryCategoryTabs");
			this._secondaryCategoryTabs.SelectedTabChanged = delegate()
			{
				this.SearchField.Value = "";
				this.SetSelectedTabs(this._primaryCategoryTabs.SelectedTab, this._secondaryCategoryTabs.SelectedTab);
				this.UpdateItemLibrary();
			};
			this.SearchField = uifragment.Get<Group>("Search").Find<TextField>("SearchField");
			this.SearchField.ValueChanged = new Action(this.UpdateItemLibrary);
			this.ItemGrid = uifragment.Get<ItemGrid>("ItemGrid");
			this.ItemGrid.AllowMaxStackDraggableItems = true;
			this.ItemGrid.Slots = new ItemGridSlot[0];
			this.ItemGrid.SlotMouseEntered = delegate(int slotIndex)
			{
				ItemGridSlot itemGridSlot = this.ItemGrid.Slots[slotIndex];
				ClientItemStack clientItemStack = (itemGridSlot != null) ? itemGridSlot.ItemStack : null;
				bool flag4 = clientItemStack == null;
				if (!flag4)
				{
					this.HoveredItemId = clientItemStack.Id;
				}
			};
			this.ItemGrid.SlotMouseExited = delegate(int slotIndex)
			{
				this.HoveredItemId = null;
			};
			this.ItemGrid.SlotClicking = delegate(int slotIndex, int button)
			{
				ItemGridSlot itemGridSlot = this.ItemGrid.Slots[slotIndex];
				ClientItemStack clientItemStack = (itemGridSlot != null) ? itemGridSlot.ItemStack : null;
				bool flag4 = clientItemStack == null;
				if (!flag4)
				{
					string id = clientItemStack.Id;
					bool flag5 = this.Interface.App.InGame.Instance.Chat.IsOpen && this.Desktop.IsShiftKeyDown;
					if (flag5)
					{
						this._inGameView.ChatComponent.InsertItemTag(id);
					}
					else
					{
						bool isShiftKeyDown = this.Desktop.IsShiftKeyDown;
						if (isShiftKeyDown)
						{
							ClientItemBase clientItemBase = this._inGameView.Items[id];
							int quantity = 1;
							bool flag6 = (long)button == 1L;
							if (flag6)
							{
								quantity = clientItemBase.MaxStack;
							}
							else
							{
								bool flag7 = (long)button == 2L;
								if (flag7)
								{
									quantity = (int)Math.Floor((double)((float)clientItemBase.MaxStack / 2f));
								}
							}
							ClientItemStack itemStack = new ClientItemStack(clientItemBase.Id, quantity);
							this._inGameView.InGame.SendSmartGiveCreativeItemPacket(itemStack, 1);
						}
					}
				}
			};
			this.ItemGrid.SlotDoubleClicking = delegate(int slotIndex)
			{
				ItemGridSlot itemGridSlot = this.ItemGrid.Slots[slotIndex];
				ClientItemStack clientItemStack = (itemGridSlot != null) ? itemGridSlot.ItemStack : null;
				bool flag4 = clientItemStack == null;
				if (!flag4)
				{
					string id = clientItemStack.Id;
					ClientItemBase clientItemBase = this._inGameView.Items[id];
					ClientItemStack itemStack = new ClientItemStack(clientItemBase.Id, 1);
					this._inGameView.InGame.SendSmartGiveCreativeItemPacket(itemStack, 0);
				}
			};
			this.ItemGrid.Dropped = delegate(int targetSlotIndex, Element sourceItemGrid, ItemGrid.ItemDragData dragData)
			{
				bool flag4 = dragData.InventorySectionId == null;
				if (!flag4)
				{
					ItemGrid itemGrid = sourceItemGrid as ItemGrid;
					ClientItemStack itemStack;
					if (itemGrid == null)
					{
						BaseItemSlotSelector baseItemSlotSelector = sourceItemGrid as BaseItemSlotSelector;
						if (baseItemSlotSelector == null)
						{
							return;
						}
						itemStack = baseItemSlotSelector.GetItemStack(dragData.ItemGridIndex);
					}
					else
					{
						itemStack = itemGrid.Slots[dragData.ItemGridIndex].ItemStack;
					}
					int quantity = itemStack.Quantity - dragData.ItemStack.Quantity;
					ClientItemStack itemStack2 = new ClientItemStack(itemStack.Id, quantity)
					{
						Metadata = itemStack.Metadata
					};
					this._inGameView.ClearSlotHighlight();
					this.Interface.App.InGame.Instance.BuilderToolsModule.ClearConfiguringTool();
					this._inGameView.InGame.SendSetCreativeItemPacket(dragData.InventorySectionId.Value, dragData.SlotId, itemStack2, true);
				}
			};
			bool flag = this._inGameView.Items != null;
			if (flag)
			{
				this.UpdateItemLibrary();
			}
			ClientItemCategory[] itemCategories = this.Interface.App.InGame.ItemCategories;
			bool flag2 = itemCategories != null && itemCategories.Length != 0;
			if (flag2)
			{
				bool flag3 = this._selectedTabs == null;
				if (flag3)
				{
					this.SetSelectedTabs(this.Interface.App.InGame.ItemCategories[0].Id, null);
				}
				this.SetupPrimaryCategoryTabs();
				this.SetupSecondaryCategoryTabs();
			}
		}

		// Token: 0x06003F8A RID: 16266 RVA: 0x000B2950 File Offset: 0x000B0B50
		public void ResetState()
		{
			this._selectedTabs = null;
			this._idKeywordsMapping = null;
			this._displayMode = 0;
			this._primaryCategoryTabs.Tabs = new TabNavigation.Tab[0];
			this._secondaryCategoryTabs.Tabs = new TabNavigation.Tab[0];
			this.SearchField.Value = "";
		}

		// Token: 0x06003F8B RID: 16267 RVA: 0x000B29A8 File Offset: 0x000B0BA8
		protected override void OnMounted()
		{
			this.UpdateItemLibrary();
		}

		// Token: 0x06003F8C RID: 16268 RVA: 0x000B29B2 File Offset: 0x000B0BB2
		protected override void OnUnmounted()
		{
			this.HoveredItemId = null;
		}

		// Token: 0x06003F8D RID: 16269 RVA: 0x000B29C0 File Offset: 0x000B0BC0
		private void SetSelectedTabs(string primaryTab, string secondaryTab = null)
		{
			ClientItemCategory clientItemCategory = Enumerable.First<ClientItemCategory>(this.Interface.App.InGame.ItemCategories, (ClientItemCategory c) => c.Id == primaryTab);
			ClientItemCategory clientItemCategory2 = (secondaryTab != null) ? Enumerable.First<ClientItemCategory>(clientItemCategory.Children, (ClientItemCategory c) => c.Id == secondaryTab) : clientItemCategory.Children[0];
			this._selectedTabs = Tuple.Create<string, string>(primaryTab, clientItemCategory2.Id);
			this._displayMode = clientItemCategory2.InfoDisplayMode;
		}

		// Token: 0x06003F8E RID: 16270 RVA: 0x000B2A58 File Offset: 0x000B0C58
		public void EnsureValidCategorySelected()
		{
			bool flag = this._selectedTabs == null;
			if (flag)
			{
				this.SetSelectedTabs(this.Interface.App.InGame.ItemCategories[0].Id, null);
			}
			else
			{
				foreach (ClientItemCategory clientItemCategory in this.Interface.App.InGame.ItemCategories)
				{
					bool flag2 = clientItemCategory.Id != this._selectedTabs.Item1;
					if (!flag2)
					{
						foreach (ClientItemCategory clientItemCategory2 in clientItemCategory.Children)
						{
							bool flag3 = clientItemCategory2.Id != this._selectedTabs.Item2;
							if (!flag3)
							{
								return;
							}
						}
						break;
					}
				}
				this.SetSelectedTabs(this.Interface.App.InGame.ItemCategories[0].Id, null);
			}
		}

		// Token: 0x06003F8F RID: 16271 RVA: 0x000B2B54 File Offset: 0x000B0D54
		public void SetupCategories()
		{
			this.SetupPrimaryCategoryTabs();
			this.SetupSecondaryCategoryTabs();
			bool isMounted = base.IsMounted;
			if (isMounted)
			{
				this._primaryCategoryTabs.Layout(null, true);
				this._secondaryCategoryTabs.Parent.Layout(null, true);
			}
		}

		// Token: 0x06003F90 RID: 16272 RVA: 0x000B2BB0 File Offset: 0x000B0DB0
		public void OnItemsUpdated()
		{
			this._idKeywordsMapping = new Dictionary<string, string>(this._inGameView.Items.Count);
			foreach (ClientItemBase clientItemBase in this._inGameView.Items.Values)
			{
				string text = this.Interface.GetText("items." + clientItemBase.Id + ".name", null, false);
				bool flag = text == null;
				if (!flag)
				{
					this._idKeywordsMapping[clientItemBase.Id] = text.ToLowerInvariant() + " " + clientItemBase.Id.ToLowerInvariant();
				}
			}
		}

		// Token: 0x06003F91 RID: 16273 RVA: 0x000B2C80 File Offset: 0x000B0E80
		private void SetupPrimaryCategoryTabs()
		{
			ClientItemCategory[] itemCategories = this.Interface.App.InGame.ItemCategories;
			TabNavigation.Tab[] array = new TabNavigation.Tab[itemCategories.Length];
			for (int i = 0; i < itemCategories.Length; i++)
			{
				ClientItemCategory clientItemCategory = itemCategories[i];
				array[i] = new TabNavigation.Tab
				{
					Id = clientItemCategory.Id
				};
				TextureArea textureArea;
				bool flag = this._inGameView.TryMountAssetTexture(clientItemCategory.Icon, out textureArea);
				if (flag)
				{
					array[i].Icon = new PatchStyle(textureArea);
				}
				TextureArea textureArea2;
				bool flag2 = this._inGameView.TryMountAssetTexture(clientItemCategory.Icon.Replace(".png", "Active.png"), out textureArea2);
				if (flag2)
				{
					array[i].IconSelected = new PatchStyle(textureArea2);
				}
			}
			this._primaryCategoryTabs.Tabs = array;
			this._primaryCategoryTabs.SelectedTab = this._selectedTabs.Item1;
		}

		// Token: 0x06003F92 RID: 16274 RVA: 0x000B2D68 File Offset: 0x000B0F68
		public void UpdateItemLibrary()
		{
			Tuple<string, string> selectedTabs = this._selectedTabs;
			string str = (selectedTabs != null) ? selectedTabs.Item1 : null;
			string str2 = ".";
			Tuple<string, string> selectedTabs2 = this._selectedTabs;
			string text = str + str2 + ((selectedTabs2 != null) ? selectedTabs2.Item2 : null);
			List<ClientItemBase> list = new List<ClientItemBase>();
			string text2 = this.SearchField.Value.Trim().ToLowerInvariant();
			bool flag = text2 != "";
			if (flag)
			{
				string[] array = Enumerable.ToArray<string>(Enumerable.Where<string>(Enumerable.Select<string, string>(text2.Split(new char[]
				{
					' '
				}), (string w) => w.Trim()), (string w) => w != ""));
				foreach (KeyValuePair<string, string> keyValuePair in this._idKeywordsMapping)
				{
					bool flag2 = true;
					foreach (string value in array)
					{
						bool flag3 = keyValuePair.Value.Contains(value);
						if (!flag3)
						{
							flag2 = false;
							break;
						}
					}
					bool flag4 = flag2;
					if (flag4)
					{
						list.Add(this._inGameView.Items[keyValuePair.Key]);
					}
				}
				this.ItemGrid.InfoDisplay = 0;
			}
			else
			{
				foreach (KeyValuePair<string, ClientItemBase> keyValuePair2 in this._inGameView.Items)
				{
					bool flag5 = keyValuePair2.Value.Categories != null && Enumerable.Contains<string>(keyValuePair2.Value.Categories, text);
					if (flag5)
					{
						list.Add(keyValuePair2.Value);
					}
				}
				this.ItemGrid.InfoDisplay = this._displayMode;
			}
			list = Enumerable.ToList<ClientItemBase>(Enumerable.ThenBy<ClientItemBase, string>(Enumerable.OrderBy<ClientItemBase, string>(list, (ClientItemBase item) => item.Set), (ClientItemBase item) => item.Id));
			int itemsPerRow = this.ItemGrid.GetItemsPerRow();
			bool flag6 = list.Count <= 5 * itemsPerRow;
			int num;
			if (flag6)
			{
				num = 5 * itemsPerRow;
			}
			else
			{
				num = list.Count + itemsPerRow - list.Count % itemsPerRow;
			}
			this.ItemGrid.Slots = new ItemGridSlot[num];
			this.ItemGrid.SetItemStacks(Enumerable.ToArray<ClientItemStack>(Enumerable.Select<ClientItemBase, ClientItemStack>(list, (ClientItemBase item) => new ClientItemStack(item.Id, 1))), 0);
			this.ItemGrid.SetScroll(new int?(0), new int?(0));
			bool isMounted = this.ItemGrid.IsMounted;
			if (isMounted)
			{
				this.ItemGrid.Layout(null, true);
			}
		}

		// Token: 0x06003F93 RID: 16275 RVA: 0x000B3098 File Offset: 0x000B1298
		private void SetupSecondaryCategoryTabs()
		{
			ClientItemCategory[] children = Enumerable.First<ClientItemCategory>(this.Interface.App.InGame.ItemCategories, (ClientItemCategory c) => c.Id == this._selectedTabs.Item1).Children;
			TabNavigation.Tab[] array = new TabNavigation.Tab[children.Length];
			for (int i = 0; i < children.Length; i++)
			{
				array[i] = new TabNavigation.Tab
				{
					Id = children[i].Id
				};
				TextureArea textureArea;
				bool flag = this._inGameView.TryMountAssetTexture(children[i].Icon, out textureArea);
				if (flag)
				{
					array[i].Icon = new PatchStyle(textureArea);
				}
			}
			this._secondaryCategoryTabs.Tabs = array;
			this._secondaryCategoryTabs.SelectedTab = this._selectedTabs.Item2;
		}

		// Token: 0x04001E2D RID: 7725
		private TabNavigation _primaryCategoryTabs;

		// Token: 0x04001E2E RID: 7726
		private TabNavigation _secondaryCategoryTabs;

		// Token: 0x04001E30 RID: 7728
		private Tuple<string, string> _selectedTabs;

		// Token: 0x04001E31 RID: 7729
		private ItemGridInfoDisplayMode _displayMode = 0;

		// Token: 0x04001E32 RID: 7730
		private Dictionary<string, string> _idKeywordsMapping;
	}
}
