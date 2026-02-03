using System;
using System.Collections.Generic;
using System.Linq;
using HytaleClient.Data.Items;
using HytaleClient.Graphics;
using HytaleClient.Interface.UI;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Interface.UI.Styles;
using Newtonsoft.Json.Linq;

namespace HytaleClient.Interface.InGame.Pages.InventoryPanels
{
	// Token: 0x02000898 RID: 2200
	internal class DiagramCraftingPanel : WindowPanel
	{
		// Token: 0x170010B4 RID: 4276
		// (get) Token: 0x06003F66 RID: 16230 RVA: 0x000B126C File Offset: 0x000AF46C
		// (set) Token: 0x06003F67 RID: 16231 RVA: 0x000B1274 File Offset: 0x000AF474
		public HashSet<int>[] InventoryHints { get; private set; }

		// Token: 0x170010B5 RID: 4277
		// (get) Token: 0x06003F68 RID: 16232 RVA: 0x000B127D File Offset: 0x000AF47D
		private ClientCraftingItemCategory SelectedItemCategory
		{
			get
			{
				return Enumerable.First<ClientCraftingItemCategory>(Enumerable.First<ClientCraftingCategory>(this._categories, (ClientCraftingCategory category) => category.Id == this._selectedCategory.Item1).ItemCategories, (ClientCraftingItemCategory category) => category.Id == this._selectedCategory.Item2);
			}
		}

		// Token: 0x06003F69 RID: 16233 RVA: 0x000B12AC File Offset: 0x000AF4AC
		public DiagramCraftingPanel(InGameView inGameView, Element parent = null) : base(inGameView, parent)
		{
		}

		// Token: 0x06003F6A RID: 16234 RVA: 0x000B12B8 File Offset: 0x000AF4B8
		public void Build()
		{
			base.Clear();
			Document document;
			this.Interface.TryGetDocument("InGame/Pages/Inventory/DiagramCraftingPanel.ui", out document);
			this._activeItemGridSlotBackground = document.ResolveNamedValue<PatchStyle>(this.Desktop.Provider, "SlotPatchActive");
			this._validItemGridSlotBackground = document.ResolveNamedValue<PatchStyle>(this.Desktop.Provider, "SlotPatchValid");
			this._invalidItemGridSlotBackground = document.ResolveNamedValue<PatchStyle>(this.Desktop.Provider, "SlotPatchInvalid");
			this._lockedSlotIcon = document.ResolveNamedValue<PatchStyle>(this.Desktop.Provider, "SlotLockedIcon");
			this._unlockedSlotIcon = document.ResolveNamedValue<PatchStyle>(this.Desktop.Provider, "SlotUnlockedIcon");
			this._itemCategoryButtonPatchStyle = document.ResolveNamedValue<PatchStyle>(this.Desktop.Provider, "ItemCategoryButtonBackground");
			this._itemCategoryButtonActivePatchStyle = document.ResolveNamedValue<PatchStyle>(this.Desktop.Provider, "ItemCategoryButtonActiveBackground");
			this._itemCategoryButtonAnchor = document.ResolveNamedValue<Anchor>(this.Desktop.Provider, "ItemCategoryButtonAnchor");
			UIFragment uifragment = document.Instantiate(this.Desktop, this);
			this._titleLabel = uifragment.Get<Label>("TitleLabel");
			this._categoryLabel = uifragment.Get<Label>("CategoryLabel");
			this._categoryTabs = uifragment.Get<TabNavigation>("Categories");
			this._categoryTabs.SelectedTabChanged = delegate()
			{
				ClientCraftingCategory clientCraftingCategory = Enumerable.First<ClientCraftingCategory>(this._categories, (ClientCraftingCategory c) => c.Id == this._categoryTabs.SelectedTab);
				this.SelectCategory(clientCraftingCategory.Id, clientCraftingCategory.ItemCategories[0].Id);
			};
			this._itemCategoriesContainer = uifragment.Get<Group>("ItemCategories");
			this._itemDiagramGroup = uifragment.Get<Group>("ItemDiagram");
			this._craftButton = uifragment.Get<TextButton>("CraftButton");
			this._craftButton.Activating = delegate()
			{
				this._inGameView.InventoryPage.SendWindowAction(this._inventoryWindow.Id, "craftItem", null);
			};
			this._progressBar = uifragment.Get<ProgressBar>("ProgressBar");
			this._nameLabel = uifragment.Get<Label>("PanelTitle");
			this._descriptionLabel = uifragment.Get<Label>("Description");
			this._weaponDamageLabel = uifragment.Get<Label>("WeaponDamage");
			this._weaponDpsLabel = uifragment.Get<Label>("WeaponDPS");
			this._weaponSpeedLabel = uifragment.Get<Label>("WeaponSpeed");
			this._weaponTypeLabel = uifragment.Get<Label>("WeaponType");
			this._weaponRatingLabel = uifragment.Get<Label>("WeaponRating");
			this._inputItemGrid = uifragment.Get<ItemGrid>("InputItemGrid");
			this._inputItemGrid.Slots = new ItemGridSlot[0];
			this._inputItemGrid.SlotMouseEntered = delegate(int index)
			{
				this._inGameView.HandleItemSlotMouseEntered(this._inventoryWindow.Id, index);
				this._hoveredInputSlot = new int?(index);
				this._inGameView.InventoryPage.StoragePanel.UpdateGrid();
				this._inGameView.HotbarComponent.SetupGrid();
				this.UpdateDiagramState();
			};
			this._inputItemGrid.SlotMouseExited = delegate(int index)
			{
				this._inGameView.HandleItemSlotMouseExited(this._inventoryWindow.Id, index);
				this._hoveredInputSlot = null;
				this._inGameView.InventoryPage.StoragePanel.UpdateGrid();
				this._inGameView.HotbarComponent.SetupGrid();
				this.UpdateDiagramState();
			};
			this._outputItemGrid = uifragment.Get<ItemGrid>("OutputItemGrid");
			this._outputItemGrid.Slots = new ItemGridSlot[1];
			uifragment.Get<TextButton>("RecipesButton").Activating = delegate()
			{
				RecipeCataloguePopup recipeCataloguePopup = this._inGameView.InventoryPage.RecipeCataloguePopup;
				RecipeCataloguePopup recipeCataloguePopup2 = recipeCataloguePopup;
				JToken jtoken = this._inventoryWindow.WindowData["id"];
				recipeCataloguePopup2.SetupSelectedBench((jtoken != null) ? jtoken.ToObject<string>() : null);
				this.Desktop.SetLayer(2, recipeCataloguePopup);
			};
		}

		// Token: 0x06003F6B RID: 16235 RVA: 0x000B1578 File Offset: 0x000AF778
		protected override void OnUnmounted()
		{
			bool isMounted = this._inGameView.InventoryPage.RecipeCataloguePopup.IsMounted;
			if (isMounted)
			{
				this.Desktop.ClearLayer(2);
			}
		}

		// Token: 0x06003F6C RID: 16236 RVA: 0x000B15B0 File Offset: 0x000AF7B0
		protected override void Update()
		{
			ClientCraftingItemCategory selectedItemCategory = this.SelectedItemCategory;
			JArray jarray = this._inventoryWindow.WindowData["slots"].ToObject<JArray>();
			int num = -1;
			bool flag = false;
			this._inputItemGrid.Slots = new ItemGridSlot[1 + selectedItemCategory.Slots];
			int i = 0;
			while (i < this._inputItemGrid.Slots.Length)
			{
				ClientItemStack clientItemStack = this._inventoryWindow.Inventory[i];
				bool flag2 = clientItemStack != null;
				if (flag2)
				{
					num = i;
					bool flag3 = i >= jarray.Count;
					if (!flag3)
					{
						JToken jtoken = jarray[i];
						JToken jtoken2 = jtoken["requiredAmount"];
						int? num2 = (jtoken2 != null) ? new int?(jtoken2.ToObject<int>()) : null;
						int? num3;
						int quantity;
						bool flag4;
						if (num2 != null && num2.GetValueOrDefault() != -1)
						{
							num3 = num2;
							quantity = clientItemStack.Quantity;
							flag4 = !(num3.GetValueOrDefault() <= quantity & num3 != null);
						}
						else
						{
							flag4 = true;
						}
						bool flag5 = flag4;
						if (flag5)
						{
							flag = true;
						}
						ItemGridSlot[] slots = this._inputItemGrid.Slots;
						int num4 = i;
						ItemGridSlot itemGridSlot = new ItemGridSlot();
						itemGridSlot.ItemStack = clientItemStack;
						if (num2 == null || num2.GetValueOrDefault() == -1)
						{
							goto IL_14B;
						}
						num3 = num2;
						quantity = clientItemStack.Quantity;
						if (!(num3.GetValueOrDefault() <= quantity & num3 != null))
						{
							goto IL_14B;
						}
						PatchStyle background = this._validItemGridSlotBackground;
						IL_159:
						itemGridSlot.Background = background;
						slots[num4] = itemGridSlot;
						goto IL_1C8;
						IL_14B:
						background = this._invalidItemGridSlotBackground;
						goto IL_159;
					}
				}
				else
				{
					bool flag6 = num + 1 == i && !flag;
					if (flag6)
					{
						this._inputItemGrid.Slots[i] = new ItemGridSlot
						{
							Icon = this._unlockedSlotIcon,
							Background = this._activeItemGridSlotBackground
						};
					}
					else
					{
						this._inputItemGrid.Slots[i] = new ItemGridSlot
						{
							Icon = this._lockedSlotIcon
						};
					}
				}
				IL_1C9:
				i++;
				continue;
				IL_1C8:
				goto IL_1C9;
			}
			this._inputItemGrid.Anchor.Width = new int?(this._inputItemGrid.Slots.Length * this._inputItemGrid.Style.SlotSize + (this._inputItemGrid.Slots.Length - 1) * this._inputItemGrid.Style.SlotSpacing);
			this.InventoryHints = new HashSet<int>[this._inputItemGrid.Slots.Length];
			ClientItemStack clientItemStack2 = this._inventoryWindow.Inventory[this._inventoryWindow.Inventory.Length - 1];
			ClientItemBase clientItemBase = null;
			bool flag7 = clientItemStack2 != null && (clientItemStack2.Id == "Unknown" || this._inGameView.Items.TryGetValue(clientItemStack2.Id, out clientItemBase));
			if (flag7)
			{
				bool flag8 = clientItemStack2.Id == "Unknown";
				if (flag8)
				{
					this._nameLabel.Text = "???";
					this._descriptionLabel.Text = "";
					this._outputItemGrid.RenderItemQualityBackground = false;
					this._outputItemGrid.Slots[0] = new ItemGridSlot
					{
						Name = "???",
						ItemStack = new ClientItemStack("???", 1)
					};
				}
				else
				{
					this._nameLabel.Text = this.Desktop.Provider.GetText("items." + clientItemBase.Id + ".name", null, true);
					this._descriptionLabel.Text = (this.Desktop.Provider.GetText("items." + clientItemBase.Id + ".description", null, false) ?? "");
					this._outputItemGrid.Slots[0] = new ItemGridSlot(clientItemStack2);
					this._outputItemGrid.RenderItemQualityBackground = true;
					this._outputItemGrid.Layout(null, true);
				}
			}
			else
			{
				this._nameLabel.Text = "";
				this._descriptionLabel.Text = "";
				this._outputItemGrid.Slots[0] = null;
			}
			bool flag9 = clientItemBase != null && clientItemBase.Weapon != null && clientItemStack2.Id != "Unknown";
			if (flag9)
			{
				this._weaponDamageLabel.Text = "7-13";
				this._weaponDpsLabel.Text = "(9.8 DPS)";
				this._weaponSpeedLabel.Text = "Very Slow";
				this._weaponTypeLabel.Text = "2-Hand Sword";
				this._weaponRatingLabel.Text = "15/15";
			}
			else
			{
				this._weaponDamageLabel.Text = "";
				this._weaponDpsLabel.Text = "";
				this._weaponSpeedLabel.Text = "";
				this._weaponTypeLabel.Text = "";
				this._weaponRatingLabel.Text = "";
			}
			bool flag10 = false;
			JToken jtoken3;
			bool flag11 = this._inventoryWindow.WindowData.TryGetValue("progress", ref jtoken3) && jtoken3.ToObject<float>() < 1f;
			if (flag11)
			{
				flag10 = true;
				this._progressBar.Value = jtoken3.ToObject<float>();
			}
			else
			{
				this._progressBar.Value = 0f;
			}
			bool flag12 = true;
			for (int j = 0; j < ((jarray.Count == 1) ? 1 : (jarray.Count - 1)); j++)
			{
				JObject jobject = jarray[j].ToObject<JObject>();
				this.InventoryHints[j] = new HashSet<int>();
				JToken jtoken4 = jobject["inventoryHints"];
				JArray jarray2 = (jtoken4 != null) ? jtoken4.ToObject<JArray>() : null;
				bool flag13 = jarray2 != null;
				if (flag13)
				{
					foreach (JToken jtoken5 in jarray2)
					{
						this.InventoryHints[j].Add(jtoken5.ToObject<int>());
					}
				}
				JToken jtoken6 = jobject["requiredAmount"];
				int? num5 = (jtoken6 != null) ? new int?(jtoken6.ToObject<int>()) : null;
				bool flag14 = num5 == null || num5.GetValueOrDefault() == -1;
				if (!flag14)
				{
					bool flag15;
					if (this._inventoryWindow.Inventory[j] != null)
					{
						int? num3 = num5;
						int quantity = this._inventoryWindow.Inventory[j].Quantity;
						flag15 = (num3.GetValueOrDefault() > quantity & num3 != null);
					}
					else
					{
						flag15 = true;
					}
					bool flag16 = flag15;
					if (flag16)
					{
						flag12 = false;
					}
				}
			}
			this._craftButton.Disabled = (flag10 || !flag12 || this._outputItemGrid.Slots[0] == null);
			this.UpdateDiagramState();
			base.Layout(null, true);
			bool isHovered = this._inputItemGrid.IsHovered;
			if (isHovered)
			{
				this._inGameView.InventoryPage.StoragePanel.UpdateGrid();
				this._inGameView.HotbarComponent.SetupGrid();
			}
		}

		// Token: 0x06003F6D RID: 16237 RVA: 0x000B1CDC File Offset: 0x000AFEDC
		protected override void Setup()
		{
			this._titleLabel.Text = this.Desktop.Provider.GetText(((string)this._inventoryWindow.WindowData["name"]) ?? "", null, true);
			JArray jarray = this._inventoryWindow.WindowData["categories"].ToObject<JArray>();
			this._categories = new ClientCraftingCategory[jarray.Count];
			for (int i = 0; i < this._categories.Length; i++)
			{
				JArray jarray2 = jarray[i]["itemCategories"].ToObject<JArray>();
				this._categories[i] = new ClientCraftingCategory
				{
					Id = jarray[i]["id"].ToObject<string>(),
					Icon = jarray[i]["icon"].ToObject<string>(),
					ItemCategories = new ClientCraftingItemCategory[jarray2.Count]
				};
				for (int j = 0; j < jarray2.Count; j++)
				{
					this._categories[i].ItemCategories[j] = new ClientCraftingItemCategory
					{
						Id = jarray2[j]["id"].ToObject<string>(),
						Icon = jarray2[j]["icon"].ToObject<string>(),
						Diagram = jarray2[j]["diagram"].ToObject<string>(),
						Slots = jarray2[j]["slots"].ToObject<int>(),
						SpecialSlot = jarray2[j]["specialSlot"].ToObject<bool>()
					};
				}
			}
			this._inputItemGrid.InventorySectionId = new int?(this._inventoryWindow.Id);
			this._inputItemGrid.SlotClicking = delegate(int slotIndex, int button)
			{
				this._inGameView.HandleInventoryClick(this._inventoryWindow.Id, slotIndex, button);
			};
			this._inputItemGrid.SlotDoubleClicking = delegate(int slotIndex)
			{
				this._inGameView.HandleInventoryDoubleClick(this._inventoryWindow.Id, slotIndex);
			};
			this._inputItemGrid.Dropped = delegate(int targetSlotIndex, Element sourceItemGrid, ItemGrid.ItemDragData dragData)
			{
				this._inGameView.HandleInventoryDragEnd(this._inputItemGrid, this._inventoryWindow.Id, targetSlotIndex, sourceItemGrid, dragData);
			};
			this._inputItemGrid.DragCancelled = delegate(int slotIndex, int button)
			{
				this._inGameView.HandleInventoryDropItem(this._inventoryWindow.Id, slotIndex, button);
			};
			bool flag = this._previousSelectedCatgeory != null && this._previousSelectedCatgeory.Item1 == Extensions.Value<string>(this._inventoryWindow.WindowData["id"]) && Enumerable.Any<ClientCraftingCategory>(this._categories, (ClientCraftingCategory cat) => cat.Id == this._previousSelectedCatgeory.Item2) && Enumerable.Any<ClientCraftingItemCategory>(Enumerable.First<ClientCraftingCategory>(this._categories, (ClientCraftingCategory cat) => cat.Id == this._previousSelectedCatgeory.Item2).ItemCategories, (ClientCraftingItemCategory cat) => cat.Id == this._previousSelectedCatgeory.Item3);
			if (flag)
			{
				this.SelectCategory(this._previousSelectedCatgeory.Item2, this._previousSelectedCatgeory.Item3);
			}
			else
			{
				this._previousSelectedCatgeory = Tuple.Create<string, string, string>(Extensions.Value<string>(this._inventoryWindow.WindowData["id"]), this._categories[0].Id, this._categories[0].ItemCategories[0].Id);
				this.SelectCategory(this._categories[0].Id, this._categories[0].ItemCategories[0].Id);
			}
		}

		// Token: 0x06003F6E RID: 16238 RVA: 0x000B203C File Offset: 0x000B023C
		private void BuildCategoryButtons()
		{
			this._categoryLabel.Text = this._selectedCategory.Item1;
			this._categoryLabel.Parent.Layout(null, true);
			this._itemCategoriesContainer.Clear();
			TabNavigation.Tab[] array = new TabNavigation.Tab[this._categories.Length];
			for (int i = 0; i < this._categories.Length; i++)
			{
				ClientCraftingCategory category = this._categories[i];
				TabNavigation.Tab tab = new TabNavigation.Tab
				{
					Id = category.Id
				};
				TextureArea textureArea;
				bool flag = this._inGameView.TryMountAssetTexture(category.Icon, out textureArea);
				if (flag)
				{
					tab.Icon = new PatchStyle(textureArea);
				}
				array[i] = tab;
				bool flag2 = this._selectedCategory.Item1 == category.Id;
				if (flag2)
				{
					ClientCraftingItemCategory[] itemCategories = category.ItemCategories;
					for (int j = 0; j < itemCategories.Length; j++)
					{
						ClientCraftingItemCategory itemCategory = itemCategories[j];
						Button parent = new Button(this.Desktop, this._itemCategoriesContainer)
						{
							Background = ((this._selectedCategory.Item2 == itemCategory.Id) ? this._itemCategoryButtonActivePatchStyle : this._itemCategoryButtonPatchStyle),
							Anchor = this._itemCategoryButtonAnchor,
							Activating = delegate()
							{
								this.SelectCategory(category.Id, itemCategory.Id);
							}
						};
						TextureArea textureArea2;
						bool flag3 = this._inGameView.TryMountAssetTexture(itemCategory.Icon, out textureArea2);
						if (flag3)
						{
							Group group = new Group(this.Desktop, parent);
							group.Background = new PatchStyle(textureArea2);
							group.Anchor = new Anchor
							{
								Width = this._itemCategoryButtonAnchor.Height,
								Height = this._itemCategoryButtonAnchor.Height
							};
						}
					}
				}
			}
			this._categoryTabs.Tabs = array;
			this._categoryTabs.SelectedTab = this._selectedCategory.Item1;
			this._categoryTabs.Layout(null, true);
			this._itemCategoriesContainer.Layout(null, true);
		}

		// Token: 0x06003F6F RID: 16239 RVA: 0x000B22AC File Offset: 0x000B04AC
		private void SelectCategory(string categoryId, string itemCategoryId)
		{
			this._previousSelectedCatgeory = Tuple.Create<string, string, string>(Extensions.Value<string>(this._inventoryWindow.WindowData["id"]), categoryId, itemCategoryId);
			this._selectedCategory = Tuple.Create<string, string>(categoryId, itemCategoryId);
			this.BuildCategoryButtons();
			this._itemDiagramGroup.Clear();
			for (int i = 0; i < 3; i++)
			{
				Group group = new Group(this.Desktop, this._itemDiagramGroup);
				group.Anchor = new Anchor
				{
					Width = new int?(415),
					Height = new int?(145)
				};
				group.Background = new PatchStyle(string.Format("InGame/Pages/Inventory/Diagram/Sword_{0}_Default.png", i + 1));
			}
			this.Update();
			InventoryPage inventoryPage = this._inGameView.InventoryPage;
			int id = this._inventoryWindow.Id;
			string action = "updateCategory";
			JObject jobject = new JObject();
			jobject.Add("category", categoryId);
			jobject.Add("itemCategory", itemCategoryId);
			inventoryPage.SendWindowAction(id, action, jobject);
		}

		// Token: 0x06003F70 RID: 16240 RVA: 0x000B23C4 File Offset: 0x000B05C4
		private void UpdateDiagramState()
		{
			for (int i = 0; i < 3; i++)
			{
				string arg = "Default";
				bool flag = this._inputItemGrid.Slots.Length > i && this._inputItemGrid.Slots[i].Background == this._validItemGridSlotBackground;
				if (flag)
				{
					arg = "Valid";
				}
				else
				{
					int? hoveredInputSlot = this._hoveredInputSlot;
					int num = i;
					bool flag2 = hoveredInputSlot.GetValueOrDefault() == num & hoveredInputSlot != null;
					if (flag2)
					{
						arg = "Hovered";
					}
				}
				Element element = this._itemDiagramGroup.Children[i];
				element.Background = new PatchStyle(string.Format("InGame/Pages/Inventory/Diagram/Sword_{0}_{1}.png", i + 1, arg));
				element.Layout(null, true);
			}
		}

		// Token: 0x06003F71 RID: 16241 RVA: 0x000B2497 File Offset: 0x000B0697
		public void OnSetStacks()
		{
			this.Update();
		}

		// Token: 0x06003F72 RID: 16242 RVA: 0x000B24A0 File Offset: 0x000B06A0
		public void ResetState()
		{
			this._previousSelectedCatgeory = null;
		}

		// Token: 0x04001E0F RID: 7695
		private Label _titleLabel;

		// Token: 0x04001E10 RID: 7696
		private Label _categoryLabel;

		// Token: 0x04001E11 RID: 7697
		private TabNavigation _categoryTabs;

		// Token: 0x04001E12 RID: 7698
		private Group _itemCategoriesContainer;

		// Token: 0x04001E13 RID: 7699
		private Group _itemDiagramGroup;

		// Token: 0x04001E14 RID: 7700
		private PatchStyle _itemCategoryButtonPatchStyle;

		// Token: 0x04001E15 RID: 7701
		private PatchStyle _itemCategoryButtonActivePatchStyle;

		// Token: 0x04001E16 RID: 7702
		private Anchor _itemCategoryButtonAnchor;

		// Token: 0x04001E17 RID: 7703
		private ItemGrid _inputItemGrid;

		// Token: 0x04001E18 RID: 7704
		private ItemGrid _outputItemGrid;

		// Token: 0x04001E19 RID: 7705
		private Label _nameLabel;

		// Token: 0x04001E1A RID: 7706
		private Label _descriptionLabel;

		// Token: 0x04001E1B RID: 7707
		private Label _weaponDamageLabel;

		// Token: 0x04001E1C RID: 7708
		private Label _weaponDpsLabel;

		// Token: 0x04001E1D RID: 7709
		private Label _weaponSpeedLabel;

		// Token: 0x04001E1E RID: 7710
		private Label _weaponTypeLabel;

		// Token: 0x04001E1F RID: 7711
		private Label _weaponRatingLabel;

		// Token: 0x04001E20 RID: 7712
		private ProgressBar _progressBar;

		// Token: 0x04001E21 RID: 7713
		private TextButton _craftButton;

		// Token: 0x04001E22 RID: 7714
		private ClientCraftingCategory[] _categories;

		// Token: 0x04001E23 RID: 7715
		private Tuple<string, string> _selectedCategory;

		// Token: 0x04001E25 RID: 7717
		private int? _hoveredInputSlot;

		// Token: 0x04001E26 RID: 7718
		private PatchStyle _validItemGridSlotBackground;

		// Token: 0x04001E27 RID: 7719
		private PatchStyle _invalidItemGridSlotBackground;

		// Token: 0x04001E28 RID: 7720
		private PatchStyle _activeItemGridSlotBackground;

		// Token: 0x04001E29 RID: 7721
		private PatchStyle _lockedSlotIcon;

		// Token: 0x04001E2A RID: 7722
		private PatchStyle _unlockedSlotIcon;

		// Token: 0x04001E2B RID: 7723
		private Tuple<string, string, string> _previousSelectedCatgeory;
	}
}
