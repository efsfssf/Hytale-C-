using System;
using System.Collections.Generic;
using System.Linq;
using HytaleClient.Data.Items;
using HytaleClient.Graphics;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Interface.UI.Styles;
using Newtonsoft.Json.Linq;

namespace HytaleClient.Interface.InGame.Pages.InventoryPanels
{
	// Token: 0x02000892 RID: 2194
	internal class BasicCraftingPanel : WindowPanel
	{
		// Token: 0x06003EED RID: 16109 RVA: 0x000AC45A File Offset: 0x000AA65A
		public BasicCraftingPanel(InGameView inGameView, Element parent = null) : base(inGameView, parent)
		{
		}

		// Token: 0x06003EEE RID: 16110 RVA: 0x000AC468 File Offset: 0x000AA668
		public void Build()
		{
			base.Clear();
			bool hasMarkupError = this.Interface.HasMarkupError;
			if (!hasMarkupError)
			{
				Document document;
				this.Interface.TryGetDocument("InGame/Pages/Inventory/BasicCraftingPanel.ui", out document);
				this._itemGridStyle = document.ResolveNamedValue<ItemGrid.ItemGridStyle>(this.Desktop.Provider, "ItemGridStyle");
				this._slotBackground = document.ResolveNamedValue<PatchStyle>(this.Desktop.Provider, "SlotBackground");
				this._slotSelectedBackground = document.ResolveNamedValue<PatchStyle>(this.Desktop.Provider, "SlotSelectedBackground");
				this._slotUncraftableBackground = document.ResolveNamedValue<PatchStyle>(this.Desktop.Provider, "SlotUncraftableBackground");
				this._slotUncraftableSelectedBackground = document.ResolveNamedValue<PatchStyle>(this.Desktop.Provider, "SlotUncraftableSelectedBackground");
				this._slotSelectedOverlay = document.ResolveNamedValue<PatchStyle>(this.Desktop.Provider, "SlotSelectedOverlay");
				UIFragment uifragment = document.Instantiate(this.Desktop, this);
				this._titleLabel = uifragment.Get<Label>("TitleLabel");
				this._categoryLabel = uifragment.Get<Label>("CategoryLabel");
				this._itemPreview = uifragment.Get<ItemPreviewComponent>("ItemPreview");
				this._searchField = uifragment.Get<Group>("Search").Find<TextField>("SearchField");
				this._searchField.ValueChanged = delegate()
				{
					this.BuildItemList();
					this.SetupItemList();
				};
				this._categoryTabs = uifragment.Get<TabNavigation>("Categories");
				this._categoryTabs.SelectedTabChanged = delegate()
				{
					this._previousSelectedCatgeory = Tuple.Create<string, string>(Extensions.Value<string>(this._inventoryWindow.WindowData["id"]), this._categoryTabs.SelectedTab);
					this.OnCategoryChanged();
					this._selectedItem = null;
					this._searchField.Value = "";
					this._itemList.SetScroll(new int?(0), new int?(0));
					this.BuildItemList();
					this.UpdateItemInfo();
				};
				this._itemList = uifragment.Get<ItemGrid>("Items");
				this._itemList.Style.SlotBackground = this._slotBackground;
				this._itemList.Style.ItemStackMouseDownSound = null;
				this._itemList.AreItemsDraggable = false;
				this._itemList.RenderItemQualityBackground = false;
				this._itemList.Slots = new ItemGridSlot[0];
				this._itemList.SlotClicking = delegate(int slotIndex, int button)
				{
					string id = this._itemList.Slots[slotIndex].ItemStack.Id;
					bool flag = this.Interface.App.InGame.Instance.Chat.IsOpen && this.Desktop.IsShiftKeyDown;
					if (flag)
					{
						this._inGameView.ChatComponent.InsertItemTag(id);
					}
					else
					{
						this._selectedItem = id;
						this.BuildItemList();
						this.UpdateItemInfo();
					}
				};
				this._progressBar = uifragment.Get<ProgressBar>("ProgressBar");
				this._craft1Button = uifragment.Get<TextButton>("Craft1Button");
				this._craft1Button.Activating = delegate()
				{
					this.Craft(1);
				};
				this._craft10Button = uifragment.Get<TextButton>("Craft10Button");
				this._craft10Button.Activating = delegate()
				{
					this.Craft(10);
				};
				this._craftAllButton = uifragment.Get<TextButton>("CraftAllButton");
				this._craftAllButton.Activating = delegate()
				{
					this.Craft(-1);
				};
				this._itemName = uifragment.Get<Group>("ItemName").Find<Label>("PanelTitle");
				this._itemRecipePanel = uifragment.Get<Group>("ItemRecipe");
				this._itemInfoPanel = uifragment.Get<Group>("ItemInfo");
				uifragment.Get<TextButton>("RecipesButton").Activating = delegate()
				{
					RecipeCataloguePopup recipeCataloguePopup = this._inGameView.InventoryPage.RecipeCataloguePopup;
					RecipeCataloguePopup recipeCataloguePopup2 = recipeCataloguePopup;
					JToken jtoken = this._inventoryWindow.WindowData["id"];
					recipeCataloguePopup2.SetupSelectedBench((jtoken != null) ? jtoken.ToObject<string>() : null);
					this.Desktop.SetLayer(2, recipeCataloguePopup);
				};
			}
		}

		// Token: 0x06003EEF RID: 16111 RVA: 0x000AC744 File Offset: 0x000AA944
		protected override void OnUnmounted()
		{
			bool isMounted = this._inGameView.InventoryPage.RecipeCataloguePopup.IsMounted;
			if (isMounted)
			{
				this.Desktop.ClearLayer(2);
			}
		}

		// Token: 0x06003EF0 RID: 16112 RVA: 0x000AC77C File Offset: 0x000AA97C
		protected override void Setup()
		{
			this._titleLabel.Text = this.Desktop.Provider.GetText(((string)this._inventoryWindow.WindowData["name"]) ?? "", null, true);
			bool flag = this._inGameView.InventoryPage.IsFieldcraft && this._inGameView.InGame.Instance.GameMode == 0;
			if (flag)
			{
				this._categories = Enumerable.ToArray<ClientCraftingCategory>(this._inGameView.InventoryPage.FieldcraftCategories);
			}
			else
			{
				JArray jarray = this._inventoryWindow.WindowData["categories"].ToObject<JArray>();
				this._categories = new ClientCraftingCategory[jarray.Count];
				for (int i = 0; i < this._categories.Length; i++)
				{
					this._categories[i] = new ClientCraftingCategory
					{
						Id = jarray[i]["id"].ToObject<string>(),
						Icon = jarray[i]["icon"].ToObject<string>()
					};
				}
			}
			this._searchField.Value = "";
			TabNavigation.Tab[] array = new TabNavigation.Tab[this._categories.Length];
			for (int j = 0; j < array.Length; j++)
			{
				TabNavigation.Tab tab = new TabNavigation.Tab
				{
					Id = this._categories[j].Id
				};
				TextureArea textureArea;
				bool flag2 = this._inGameView.TryMountAssetTexture(this._categories[j].Icon, out textureArea);
				if (flag2)
				{
					tab.Icon = new PatchStyle(textureArea);
				}
				array[j] = tab;
			}
			this._categoryTabs.Visible = (this._categories.Length > 1);
			this._categoryTabs.Tabs = array;
			bool flag3 = this._previousSelectedCatgeory != null && this._previousSelectedCatgeory.Item1 == Extensions.Value<string>(this._inventoryWindow.WindowData["id"]) && Enumerable.Any<ClientCraftingCategory>(this._categories, (ClientCraftingCategory cat) => cat.Id == this._previousSelectedCatgeory.Item2);
			if (flag3)
			{
				this._categoryTabs.SelectedTab = this._previousSelectedCatgeory.Item2;
			}
			else
			{
				this._categoryTabs.SelectedTab = this._categories[0].Id;
				this._previousSelectedCatgeory = Tuple.Create<string, string>(Extensions.Value<string>(this._inventoryWindow.WindowData["id"]), this._categoryTabs.SelectedTab);
			}
			this._selectedItem = null;
			this.OnCategoryChanged();
			this.BuildItemList();
			this.UpdateItemInfo();
		}

		// Token: 0x06003EF1 RID: 16113 RVA: 0x000ACA30 File Offset: 0x000AAC30
		private void Craft(int quantity = -1)
		{
			bool flag = quantity == -1;
			if (flag)
			{
				quantity = Enumerable.First<BasicCraftingPanel.CraftableItem>(this._craftableItems, (BasicCraftingPanel.CraftableItem item) => item.ItemId == this._selectedItem).CraftableAmount;
			}
			InventoryPage inventoryPage = this._inGameView.InventoryPage;
			int id = this._inventoryWindow.Id;
			string action = "craftItem";
			JObject jobject = new JObject();
			jobject.Add("itemId", this._selectedItem);
			jobject.Add("quantity", quantity);
			inventoryPage.SendWindowAction(id, action, jobject);
		}

		// Token: 0x06003EF2 RID: 16114 RVA: 0x000ACAB8 File Offset: 0x000AACB8
		public void OnSetStacks()
		{
			this.BuildItemList();
			bool flag = this._selectedItem != null;
			if (flag)
			{
				this.UpdateItemInfo();
			}
		}

		// Token: 0x06003EF3 RID: 16115 RVA: 0x000ACAE4 File Offset: 0x000AACE4
		private void OnCategoryChanged()
		{
			this._categoryLabel.Text = this._categoryTabs.SelectedTab;
			this._categoryLabel.Parent.Layout(null, true);
		}

		// Token: 0x06003EF4 RID: 16116 RVA: 0x000ACB24 File Offset: 0x000AAD24
		public void BuildItemList()
		{
			string b = this._inventoryWindow.WindowData["id"].ToObject<string>();
			List<BasicCraftingPanel.CraftableItem> list = new List<BasicCraftingPanel.CraftableItem>();
			Dictionary<string, ClientItemBase>.KeyCollection keys = this._inGameView.Items.Keys;
			string text = this._searchField.Value.Trim().ToLower();
			string[] array;
			if (!(text != ""))
			{
				array = null;
			}
			else
			{
				array = Enumerable.ToArray<string>(Enumerable.Where<string>(Enumerable.Select<string, string>(text.Split(new char[]
				{
					' '
				}), (string w) => w.Trim()), (string w) => w != ""));
			}
			string[] array2 = array;
			foreach (string key in keys)
			{
				ClientItemBase clientItemBase = this._inGameView.Items[key];
				bool flag = clientItemBase.Recipe == null || clientItemBase.Recipe.BenchRequirement == null;
				if (!flag)
				{
					bool flag2 = false;
					foreach (ClientItemCraftingRecipe.ClientBenchRequirement clientBenchRequirement in clientItemBase.Recipe.BenchRequirement)
					{
						bool flag3 = clientBenchRequirement.Id == b && clientBenchRequirement.Type == 0;
						if (flag3)
						{
							bool flag4 = array2 != null;
							if (flag4)
							{
								string[] array3 = Enumerable.ToArray<string>(Enumerable.Where<string>(Enumerable.Select<string, string>(text.Split(new char[]
								{
									' '
								}), (string w) => w.Trim()), (string w) => w != ""));
								string text2 = this.Desktop.Provider.GetText("items." + clientItemBase.Id + ".name", null, true).ToLowerInvariant();
								flag2 = true;
								foreach (string value in array3)
								{
									bool flag5 = text2.Contains(value);
									if (!flag5)
									{
										flag2 = false;
										break;
									}
								}
								break;
							}
							bool flag6 = this._categoryTabs.SelectedTab == "All";
							if (flag6)
							{
								flag2 = true;
								break;
							}
							bool flag7 = clientBenchRequirement.Categories != null;
							if (flag7)
							{
								foreach (string a in clientBenchRequirement.Categories)
								{
									bool flag8 = a == this._categoryTabs.SelectedTab;
									if (flag8)
									{
										flag2 = true;
										break;
									}
								}
							}
						}
						bool flag9 = flag2;
						if (flag9)
						{
							break;
						}
					}
					bool flag10 = !flag2;
					if (!flag10)
					{
						BasicCraftingPanel.CraftableItem item2;
						bool flag11 = !this.TryGetCraftableItem(clientItemBase, out item2);
						if (!flag11)
						{
							list.Add(item2);
						}
					}
				}
			}
			this._craftableItems = Enumerable.ToArray<BasicCraftingPanel.CraftableItem>(Enumerable.ThenBy<BasicCraftingPanel.CraftableItem, string>(Enumerable.ThenBy<BasicCraftingPanel.CraftableItem, string>(Enumerable.ThenBy<BasicCraftingPanel.CraftableItem, int>(Enumerable.OrderByDescending<BasicCraftingPanel.CraftableItem, bool>(list, (BasicCraftingPanel.CraftableItem item) => item.CraftableAmount > 0), (BasicCraftingPanel.CraftableItem item) => item.ItemLevel), (BasicCraftingPanel.CraftableItem item) => item.Set), (BasicCraftingPanel.CraftableItem item) => item.Name));
			bool flag12 = this._selectedItem == null && this._craftableItems.Length != 0;
			if (flag12)
			{
				this._selectedItem = this._craftableItems[0].ItemId;
			}
			this.SetupItemList();
		}

		// Token: 0x06003EF5 RID: 16117 RVA: 0x000ACF2C File Offset: 0x000AB12C
		private bool TryGetCraftableItem(ClientItemBase item, out BasicCraftingPanel.CraftableItem craftableItem)
		{
			bool flag = item.Recipe == null || item.Recipe.Output == null || item.Recipe.Output.Length == 0 || (item.Recipe.KnowledgeRequired && !this._inGameView.InventoryPage.KnownCraftingRecipes.ContainsKey(item.Id));
			bool result;
			if (flag)
			{
				craftableItem = null;
				result = false;
			}
			else
			{
				List<BasicCraftingPanel.CraftingIngredient> list = new List<BasicCraftingPanel.CraftingIngredient>();
				int num = int.MaxValue;
				bool flag2 = item.Recipe.Input != null;
				if (flag2)
				{
					foreach (ClientItemCraftingRecipe.ClientCraftingMaterial clientCraftingMaterial in item.Recipe.Input)
					{
						bool flag3 = clientCraftingMaterial.Quantity <= 0;
						if (!flag3)
						{
							BasicCraftingPanel.CraftingIngredient item2 = new BasicCraftingPanel.CraftingIngredient
							{
								Needs = clientCraftingMaterial.Quantity,
								Has = this.CountMaterial(clientCraftingMaterial),
								ItemId = clientCraftingMaterial.ItemId,
								ResourceTypeId = clientCraftingMaterial.ResourceTypeId
							};
							int num2 = (int)Math.Floor((double)((float)this.CountMaterial(clientCraftingMaterial) / (float)clientCraftingMaterial.Quantity));
							bool flag4 = num == -1 || num2 < num;
							if (flag4)
							{
								num = num2;
							}
							list.Add(item2);
						}
					}
				}
				bool flag5 = list.Count == 0;
				if (flag5)
				{
					num = 1;
				}
				craftableItem = new BasicCraftingPanel.CraftableItem
				{
					ItemId = item.Id,
					Name = this.Desktop.Provider.GetText("items." + item.Id + ".name", null, true),
					ItemLevel = item.ItemLevel,
					OutputItemId = item.Recipe.Output[0].ItemId,
					OutputQuantity = item.Recipe.Output[0].Quantity,
					Set = item.Set,
					CraftableAmount = num,
					Ingredients = list
				};
				result = true;
			}
			return result;
		}

		// Token: 0x06003EF6 RID: 16118 RVA: 0x000AD12C File Offset: 0x000AB32C
		private void SetupItemList()
		{
			BasicCraftingPanel.CraftableItem[] craftableItems = this._craftableItems;
			this._itemList.Slots = new ItemGridSlot[craftableItems.Length];
			for (int i = 0; i < craftableItems.Length; i++)
			{
				BasicCraftingPanel.CraftableItem craftableItem = craftableItems[i];
				this._itemList.Slots[i] = new ItemGridSlot(new ClientItemStack(craftableItem.ItemId, craftableItem.OutputQuantity))
				{
					IsItemIncompatible = (craftableItem.CraftableAmount == 0),
					Background = ((this._selectedItem != craftableItem.ItemId) ? this._slotUncraftableBackground : ((craftableItem.CraftableAmount == 0) ? this._slotUncraftableSelectedBackground : this._slotSelectedBackground)),
					Overlay = ((this._selectedItem == craftableItem.ItemId) ? this._slotSelectedOverlay : null),
					IsActivatable = (this._selectedItem != craftableItem.ItemId)
				};
			}
			this._itemList.Layout(null, true);
		}

		// Token: 0x06003EF7 RID: 16119 RVA: 0x000AD22C File Offset: 0x000AB42C
		private void UpdateItemInfo()
		{
			this._itemRecipePanel.Clear();
			ClientItemBase item;
			BasicCraftingPanel.CraftableItem craftableItem;
			bool flag = this._selectedItem != null && this._inGameView.Items.TryGetValue(this._selectedItem, out item) && this.TryGetCraftableItem(item, out craftableItem);
			if (flag)
			{
				Document document;
				this.Interface.TryGetDocument("InGame/Pages/Inventory/BasicCraftingIngredient.ui", out document);
				PatchStyle patchStyle = document.ResolveNamedValue<PatchStyle>(this.Desktop.Provider, "SlotInvalidBackground");
				PatchStyle patchStyle2 = document.ResolveNamedValue<PatchStyle>(this.Desktop.Provider, "SlotValidBackground");
				UInt32Color uint32Color = document.ResolveNamedValue<UInt32Color>(this.Interface, "ValidColor");
				UInt32Color uint32Color2 = document.ResolveNamedValue<UInt32Color>(this.Interface, "InvalidColor");
				ItemGrid.ItemGridStyle itemGridStyle = this._itemGridStyle.Clone();
				itemGridStyle.SlotBackground = null;
				for (int i = 0; i < craftableItem.Ingredients.Count; i++)
				{
					BasicCraftingPanel.CraftingIngredient craftingIngredient = craftableItem.Ingredients[i];
					UIFragment uifragment = document.Instantiate(this.Desktop, this._itemRecipePanel);
					uifragment.Get<Group>("Container").Anchor.Top = new int?(itemGridStyle.SlotSize * i + itemGridStyle.SlotSpacing * i);
					uifragment.Get<Group>("SlotBackground").Background = ((craftingIngredient.Has >= craftingIngredient.Needs) ? patchStyle2 : patchStyle);
					uifragment.Get<Label>("NameLabel").Text = this.Desktop.Provider.GetText((craftingIngredient.ItemId != null) ? ("items." + craftingIngredient.ItemId + ".name") : ("resourceTypes." + craftingIngredient.ResourceTypeId + ".name"), null, true);
					Label label = uifragment.Get<Label>("QuantityLabel");
					label.Style = label.Style.Clone();
					label.Style.TextColor = ((craftingIngredient.Has >= craftingIngredient.Needs) ? uint32Color : uint32Color2);
					label.Text = string.Format("{0}/{1}", craftingIngredient.Has, craftingIngredient.Needs);
				}
				ItemGrid itemGrid = new ItemGrid(this.Desktop, this._itemRecipePanel)
				{
					SlotsPerRow = 1,
					Style = itemGridStyle,
					RenderItemQualityBackground = false,
					AreItemsDraggable = false,
					Slots = new ItemGridSlot[craftableItem.Ingredients.Count]
				};
				for (int j = 0; j < craftableItem.Ingredients.Count; j++)
				{
					BasicCraftingPanel.CraftingIngredient craftingIngredient2 = craftableItem.Ingredients[j];
					bool flag2 = craftingIngredient2.ItemId == null;
					if (flag2)
					{
						PatchStyle icon = null;
						ClientResourceType clientResourceType;
						TextureArea textureArea;
						bool flag3 = this._inGameView.InventoryPage.ResourceTypes.TryGetValue(craftingIngredient2.ResourceTypeId, out clientResourceType) && this._inGameView.TryMountAssetTexture(clientResourceType.Icon, out textureArea);
						if (flag3)
						{
							icon = new PatchStyle(textureArea);
						}
						itemGrid.Slots[j] = new ItemGridSlot
						{
							Name = this.Desktop.Provider.GetText("ui.items.resourceTypeTooltip.name", new Dictionary<string, string>
							{
								{
									"name",
									this.Desktop.Provider.GetText("resourceTypes." + craftingIngredient2.ResourceTypeId + ".name", null, true)
								}
							}, true),
							Description = this.Desktop.Provider.GetText("resourceTypes." + craftingIngredient2.ResourceTypeId + ".description", null, false),
							Icon = icon
						};
					}
					else
					{
						itemGrid.Slots[j] = new ItemGridSlot(new ClientItemStack(craftingIngredient2.ItemId, 1));
					}
				}
				itemGrid.Layout(null, true);
				this._itemRecipePanel.Layout(null, true);
				this._itemName.Text = this.Desktop.Provider.GetText("items." + this._selectedItem + ".name", null, true);
				this._craft1Button.Disabled = (craftableItem.CraftableAmount < 1);
				this._craft10Button.Disabled = (craftableItem.CraftableAmount < 10);
				this._craftAllButton.Disabled = (craftableItem.CraftableAmount < 1);
			}
			else
			{
				this._itemName.Text = "";
				this._craft1Button.Disabled = true;
				this._craft10Button.Disabled = true;
				this._craftAllButton.Disabled = true;
			}
			this.UpdateItemPreview();
			this._itemInfoPanel.Layout(null, true);
		}

		// Token: 0x06003EF8 RID: 16120 RVA: 0x000AD6F8 File Offset: 0x000AB8F8
		public void UpdateItemPreview()
		{
			bool flag = this._selectedItem != null && this.Desktop.GetLayer(2) == null;
			if (flag)
			{
				this._itemPreview.SetItemId(this._selectedItem);
			}
			else
			{
				this._itemPreview.SetItemId(null);
			}
		}

		// Token: 0x06003EF9 RID: 16121 RVA: 0x000AD74C File Offset: 0x000AB94C
		private int CountMaterial(ClientItemCraftingRecipe.ClientCraftingMaterial material)
		{
			return (material.ResourceTypeId != null) ? this.CountResourceType(material.ResourceTypeId) : this.CountItem(material.ItemId);
		}

		// Token: 0x06003EFA RID: 16122 RVA: 0x000AD780 File Offset: 0x000AB980
		private int CountItem(string itemId)
		{
			int num = 0;
			foreach (ClientItemStack clientItemStack in this._inGameView.StorageStacks)
			{
				bool flag = ((clientItemStack != null) ? clientItemStack.Id : null) == itemId;
				if (flag)
				{
					num += clientItemStack.Quantity;
				}
			}
			foreach (ClientItemStack clientItemStack2 in this._inGameView.HotbarStacks)
			{
				bool flag2 = ((clientItemStack2 != null) ? clientItemStack2.Id : null) == itemId;
				if (flag2)
				{
					num += clientItemStack2.Quantity;
				}
			}
			return num;
		}

		// Token: 0x06003EFB RID: 16123 RVA: 0x000AD82C File Offset: 0x000ABA2C
		private int CountResourceType(string resourceTypeId)
		{
			int num = 0;
			foreach (ClientItemStack clientItemStack in this._inGameView.StorageStacks)
			{
				ClientItemBase clientItemBase;
				bool flag = clientItemStack == null || !this._inGameView.Items.TryGetValue(clientItemStack.Id, out clientItemBase) || clientItemBase.ResourceTypes == null;
				if (!flag)
				{
					foreach (ClientItemResourceType clientItemResourceType in clientItemBase.ResourceTypes)
					{
						bool flag2 = clientItemResourceType.Id == resourceTypeId;
						if (flag2)
						{
							num += clientItemResourceType.Quantity * clientItemStack.Quantity;
						}
					}
				}
			}
			foreach (ClientItemStack clientItemStack2 in this._inGameView.HotbarStacks)
			{
				ClientItemBase clientItemBase2;
				bool flag3 = clientItemStack2 == null || !this._inGameView.Items.TryGetValue(clientItemStack2.Id, out clientItemBase2) || clientItemBase2.ResourceTypes == null;
				if (!flag3)
				{
					foreach (ClientItemResourceType clientItemResourceType2 in clientItemBase2.ResourceTypes)
					{
						bool flag4 = clientItemResourceType2.Id == resourceTypeId;
						if (flag4)
						{
							num += clientItemResourceType2.Quantity * clientItemStack2.Quantity;
						}
					}
				}
			}
			return num;
		}

		// Token: 0x06003EFC RID: 16124 RVA: 0x000AD999 File Offset: 0x000ABB99
		public void ResetState()
		{
			this._previousSelectedCatgeory = null;
		}

		// Token: 0x04001DB4 RID: 7604
		private Label _titleLabel;

		// Token: 0x04001DB5 RID: 7605
		private Label _categoryLabel;

		// Token: 0x04001DB6 RID: 7606
		private TextField _searchField;

		// Token: 0x04001DB7 RID: 7607
		private ItemGrid _itemList;

		// Token: 0x04001DB8 RID: 7608
		private TabNavigation _categoryTabs;

		// Token: 0x04001DB9 RID: 7609
		private ClientCraftingCategory[] _categories;

		// Token: 0x04001DBA RID: 7610
		private string _selectedItem;

		// Token: 0x04001DBB RID: 7611
		private Label _itemName;

		// Token: 0x04001DBC RID: 7612
		private ItemPreviewComponent _itemPreview;

		// Token: 0x04001DBD RID: 7613
		private Group _itemRecipePanel;

		// Token: 0x04001DBE RID: 7614
		private Group _itemInfoPanel;

		// Token: 0x04001DBF RID: 7615
		private TextButton _craft1Button;

		// Token: 0x04001DC0 RID: 7616
		private TextButton _craft10Button;

		// Token: 0x04001DC1 RID: 7617
		private TextButton _craftAllButton;

		// Token: 0x04001DC2 RID: 7618
		private BasicCraftingPanel.CraftableItem[] _craftableItems;

		// Token: 0x04001DC3 RID: 7619
		private ProgressBar _progressBar;

		// Token: 0x04001DC4 RID: 7620
		private ItemGrid.ItemGridStyle _itemGridStyle;

		// Token: 0x04001DC5 RID: 7621
		private PatchStyle _slotBackground;

		// Token: 0x04001DC6 RID: 7622
		private PatchStyle _slotSelectedBackground;

		// Token: 0x04001DC7 RID: 7623
		private PatchStyle _slotUncraftableBackground;

		// Token: 0x04001DC8 RID: 7624
		private PatchStyle _slotUncraftableSelectedBackground;

		// Token: 0x04001DC9 RID: 7625
		private PatchStyle _slotSelectedOverlay;

		// Token: 0x04001DCA RID: 7626
		private Tuple<string, string> _previousSelectedCatgeory;

		// Token: 0x02000D62 RID: 3426
		private class CraftableItem
		{
			// Token: 0x040041BE RID: 16830
			public string ItemId;

			// Token: 0x040041BF RID: 16831
			public string Name;

			// Token: 0x040041C0 RID: 16832
			public int ItemLevel;

			// Token: 0x040041C1 RID: 16833
			public string OutputItemId;

			// Token: 0x040041C2 RID: 16834
			public int OutputQuantity;

			// Token: 0x040041C3 RID: 16835
			public string Set;

			// Token: 0x040041C4 RID: 16836
			public int CraftableAmount;

			// Token: 0x040041C5 RID: 16837
			public List<BasicCraftingPanel.CraftingIngredient> Ingredients;
		}

		// Token: 0x02000D63 RID: 3427
		private class CraftingIngredient
		{
			// Token: 0x040041C6 RID: 16838
			public int Needs;

			// Token: 0x040041C7 RID: 16839
			public int Has;

			// Token: 0x040041C8 RID: 16840
			public string ItemId;

			// Token: 0x040041C9 RID: 16841
			public string ResourceTypeId;
		}
	}
}
