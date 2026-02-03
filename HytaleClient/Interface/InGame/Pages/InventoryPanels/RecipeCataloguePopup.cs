using System;
using System.Collections.Generic;
using HytaleClient.Data.Items;
using HytaleClient.Graphics;
using HytaleClient.Interface.UI;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Interface.UI.Styles;

namespace HytaleClient.Interface.InGame.Pages.InventoryPanels
{
	// Token: 0x0200089E RID: 2206
	internal class RecipeCataloguePopup : InterfaceComponent
	{
		// Token: 0x06003FD3 RID: 16339 RVA: 0x000B51C0 File Offset: 0x000B33C0
		public RecipeCataloguePopup(InGameView inGameView, Element parent) : base(inGameView.Interface, parent)
		{
			this._inGameView = inGameView;
		}

		// Token: 0x06003FD4 RID: 16340 RVA: 0x000B51E8 File Offset: 0x000B33E8
		public void Build()
		{
			base.Clear();
			Document document;
			this.Interface.TryGetDocument("InGame/Pages/Inventory/RecipeCataloguePopup.ui", out document);
			UIFragment uifragment = document.Instantiate(this.Desktop, this);
			uifragment.Get<Button>("CloseButton").Activating = new Action(this.Dismiss);
			this._benchesGroup = uifragment.Get<Group>("Benches");
			this._benchCategoriesGroup = uifragment.Get<Group>("BenchCategories");
			this._itemCategoriesGroup = uifragment.Get<Group>("ItemCategories");
			this._recipesGroup = uifragment.Get<Group>("Recipes");
			this._categoriesSeparator = uifragment.Get<Group>("CategoriesSeparator");
			this._itemPreviewComponent = uifragment.Get<ItemPreviewComponent>("ItemPreview");
			this._itemNameLabel = uifragment.Get<Label>("ItemName");
			this._itemDescriptionLabel = uifragment.Get<Label>("ItemDescription");
			this._ingredientsGroup = uifragment.Get<Group>("Ingredients");
		}

		// Token: 0x06003FD5 RID: 16341 RVA: 0x000B52D4 File Offset: 0x000B34D4
		protected override void OnMounted()
		{
			this.UpdateLists();
			bool isMounted = this._inGameView.InventoryPage.BlockInfoPanel.IsMounted;
			if (isMounted)
			{
				this._inGameView.InventoryPage.BlockInfoPanel.UpdatePreview();
			}
			bool isMounted2 = this._inGameView.InventoryPage.BasicCraftingPanel.IsMounted;
			if (isMounted2)
			{
				this._inGameView.InventoryPage.BasicCraftingPanel.UpdateItemPreview();
			}
			bool isMounted3 = this._inGameView.InventoryPage.StructuralCraftingPanel.IsMounted;
			if (isMounted3)
			{
				this._inGameView.InventoryPage.StructuralCraftingPanel.UpdateItemPreview();
			}
			bool isMounted4 = this._inGameView.InventoryPage.CharacterPanel.IsMounted;
			if (isMounted4)
			{
				this._inGameView.InventoryPage.CharacterPanel.UpdateCharacterVisibility(true);
			}
		}

		// Token: 0x06003FD6 RID: 16342 RVA: 0x000B53A8 File Offset: 0x000B35A8
		protected override void OnUnmounted()
		{
			bool flag = !this._inGameView.IsMounted;
			if (!flag)
			{
				bool isMounted = this._inGameView.InventoryPage.BlockInfoPanel.IsMounted;
				if (isMounted)
				{
					this._inGameView.InventoryPage.BlockInfoPanel.UpdatePreview();
				}
				bool isMounted2 = this._inGameView.InventoryPage.BasicCraftingPanel.IsMounted;
				if (isMounted2)
				{
					this._inGameView.InventoryPage.BasicCraftingPanel.UpdateItemPreview();
				}
				bool isMounted3 = this._inGameView.InventoryPage.StructuralCraftingPanel.IsMounted;
				if (isMounted3)
				{
					this._inGameView.InventoryPage.StructuralCraftingPanel.UpdateItemPreview();
				}
				bool isMounted4 = this._inGameView.InventoryPage.CharacterPanel.IsMounted;
				if (isMounted4)
				{
					this._inGameView.InventoryPage.CharacterPanel.UpdateCharacterVisibility(true);
				}
			}
		}

		// Token: 0x06003FD7 RID: 16343 RVA: 0x000B548C File Offset: 0x000B368C
		protected internal override void Dismiss()
		{
			this.Desktop.ClearLayer(2);
		}

		// Token: 0x06003FD8 RID: 16344 RVA: 0x000B549C File Offset: 0x000B369C
		public void SetupSelectedBench(string benchId)
		{
			this._selection = Tuple.Create<string, string, string, string>(benchId, null, null, null);
		}

		// Token: 0x06003FD9 RID: 16345 RVA: 0x000B54B0 File Offset: 0x000B36B0
		private void UpdateLists()
		{
			RecipeCataloguePopup.<>c__DisplayClass17_0 CS$<>8__locals1 = new RecipeCataloguePopup.<>c__DisplayClass17_0();
			CS$<>8__locals1.<>4__this = this;
			this._benchesGroup.Clear();
			this._benchCategoriesGroup.Clear();
			this._itemCategoriesGroup.Clear();
			this._recipesGroup.Clear();
			Dictionary<string, ClientItemBase> items = this._inGameView.Items;
			HashSet<string> hashSet = new HashSet<string>();
			HashSet<string> hashSet2 = new HashSet<string>();
			HashSet<string> hashSet3 = new HashSet<string>();
			this.Interface.TryGetDocument("InGame/Pages/Inventory/RecipeCatalogueNavigationButton.ui", out CS$<>8__locals1.navigationButtonDoc);
			this.Interface.TryGetDocument("InGame/Pages/Inventory/RecipeCatalogueNavigationButtonSelected.ui", out CS$<>8__locals1.navigationButtonSelectedDoc);
			this.Interface.TryGetDocument("InGame/Pages/Inventory/RecipeCatalogueRecipeButton.ui", out CS$<>8__locals1.recipeButtonDoc);
			CS$<>8__locals1.recipeButtonItemSlotSize = CS$<>8__locals1.recipeButtonDoc.ResolveNamedValue<int>(this.Interface, "ItemSlotSize");
			CS$<>8__locals1.recipeButtonStyleSelected = CS$<>8__locals1.recipeButtonDoc.ResolveNamedValue<Button.ButtonStyle>(this.Interface, "StyleSelected");
			CS$<>8__locals1.itemGridStyle = CS$<>8__locals1.recipeButtonDoc.ResolveNamedValue<ItemGrid.ItemGridStyle>(this.Interface, "RecipeItemGridStyle");
			foreach (ClientItemBase clientItemBase in items.Values)
			{
				ClientItemCraftingRecipe recipe = clientItemBase.Recipe;
				bool flag = ((recipe != null) ? recipe.BenchRequirement : null) == null;
				if (!flag)
				{
					bool flag2 = clientItemBase.Recipe.KnowledgeRequired && !this._inGameView.InventoryPage.KnownCraftingRecipes.ContainsKey(clientItemBase.Id);
					if (!flag2)
					{
						bool flag3 = false;
						foreach (ClientItemCraftingRecipe.ClientBenchRequirement clientBenchRequirement in clientItemBase.Recipe.BenchRequirement)
						{
							bool flag4 = clientBenchRequirement.Type == 1;
							if (!flag4)
							{
								bool flag5 = this._selection.Item1 == null;
								if (flag5)
								{
									this._selection = Tuple.Create<string, string, string, string>(clientBenchRequirement.Id, null, null, null);
								}
								bool flag6 = !hashSet.Contains(clientBenchRequirement.Id);
								if (flag6)
								{
									hashSet.Add(clientBenchRequirement.Id);
									CS$<>8__locals1.<UpdateLists>g__MakeTextButton|0(this._benchesGroup, clientBenchRequirement.Id, this._selection.Item1 == clientBenchRequirement.Id, clientBenchRequirement.Id, null, null, null);
								}
								bool flag7 = clientBenchRequirement.Id != this._selection.Item1;
								if (!flag7)
								{
									bool flag8 = clientBenchRequirement.Categories == null;
									if (flag8)
									{
										bool flag9 = this._selection.Item2 == null;
										if (flag9)
										{
											this._selection = Tuple.Create<string, string, string, string>(clientBenchRequirement.Id, "All", null, null);
										}
										bool flag10 = !hashSet2.Contains("All");
										if (flag10)
										{
											hashSet2.Add("All");
											CS$<>8__locals1.<UpdateLists>g__MakeTextButton|0(this._benchCategoriesGroup, "All", this._selection.Item2 == "All", clientBenchRequirement.Id, "All", null, null);
										}
										bool flag11 = this._selection.Item2 != "All";
										if (!flag11)
										{
											bool flag12 = !flag3;
											if (flag12)
											{
												bool flag13 = this._selection.Item4 == null;
												if (flag13)
												{
													this._selection = Tuple.Create<string, string, string, string>(clientBenchRequirement.Id, "All", null, clientItemBase.Id);
												}
												flag3 = true;
												CS$<>8__locals1.<UpdateLists>g__MakeRecipeButton|1(clientItemBase, clientBenchRequirement.Id, "All", null);
											}
										}
									}
									else
									{
										foreach (string text in clientBenchRequirement.Categories)
										{
											string[] array = text.Split(new char[]
											{
												'.'
											});
											string text2 = array[0];
											bool flag14 = this._selection.Item2 == null;
											if (flag14)
											{
												this._selection = Tuple.Create<string, string, string, string>(clientBenchRequirement.Id, text2, null, null);
											}
											bool flag15 = !hashSet2.Contains(text2);
											if (flag15)
											{
												hashSet2.Add(text2);
												CS$<>8__locals1.<UpdateLists>g__MakeTextButton|0(this._benchCategoriesGroup, text2, this._selection.Item2 == text2, clientBenchRequirement.Id, text2, null, null);
											}
											bool flag16 = text2 != this._selection.Item2;
											if (!flag16)
											{
												bool flag17 = array.Length > 1;
												if (flag17)
												{
													string text3 = array[1];
													bool flag18 = this._selection.Item3 == null;
													if (flag18)
													{
														this._selection = Tuple.Create<string, string, string, string>(clientBenchRequirement.Id, text2, text3, null);
													}
													bool flag19 = !hashSet3.Contains(text3);
													if (flag19)
													{
														hashSet3.Add(text3);
														CS$<>8__locals1.<UpdateLists>g__MakeTextButton|0(this._itemCategoriesGroup, text3, this._selection.Item3 == text3, clientBenchRequirement.Id, text2, text3, null);
													}
													bool flag20 = text3 != this._selection.Item3;
													if (flag20)
													{
														goto IL_53D;
													}
												}
												bool flag21 = !flag3;
												if (flag21)
												{
													bool flag22 = this._selection.Item4 == null;
													if (flag22)
													{
														this._selection = Tuple.Create<string, string, string, string>(clientBenchRequirement.Id, text2, (array.Length > 1) ? array[1] : null, clientItemBase.Id);
													}
													flag3 = true;
													CS$<>8__locals1.<UpdateLists>g__MakeRecipeButton|1(clientItemBase, clientBenchRequirement.Id, text2, (array.Length > 1) ? array[1] : null);
												}
											}
											IL_53D:;
										}
									}
								}
							}
						}
					}
				}
			}
			bool flag23 = this._recipesGroup.Children.Count > 0;
			if (flag23)
			{
				this._recipesGroup.Children[this._recipesGroup.Children.Count - 1].Anchor.Bottom = new int?(0);
			}
			this._categoriesSeparator.Visible = (this._itemCategoriesGroup.Children.Count > 0);
			this.UpdateItemPanel();
		}

		// Token: 0x06003FDA RID: 16346 RVA: 0x000B5AC8 File Offset: 0x000B3CC8
		private void UpdateItemPanel()
		{
			ClientItemBase clientItemBase = this._inGameView.Items[this._selection.Item4];
			this._itemNameLabel.Text = this.Desktop.Provider.GetText("items." + clientItemBase.Id + ".name", null, true);
			this._itemNameLabel.Style = this._itemNameLabel.Style.Clone();
			this._itemNameLabel.Style.TextColor = this._inGameView.InGame.Instance.ServerSettings.ItemQualities[clientItemBase.QualityIndex].TextColor;
			this._itemDescriptionLabel.Text = (this.Desktop.Provider.GetText("items." + clientItemBase.Id + ".description", null, false) ?? "");
			this._itemPreviewComponent.SetItemId(clientItemBase.Id);
			this._ingredientsGroup.Clear();
			Group root = null;
			Document document;
			this.Interface.TryGetDocument("InGame/Pages/Inventory/RecipeCatalogueIngredient.ui", out document);
			Document document2;
			this.Interface.TryGetDocument("InGame/Pages/Inventory/RecipeCatalogueSeparator.ui", out document2);
			for (int i = 0; i < clientItemBase.Recipe.Input.Length; i++)
			{
				bool flag = i % 2 == 0;
				if (flag)
				{
					bool flag2 = i > 0;
					if (flag2)
					{
						document2.Instantiate(this.Desktop, this._ingredientsGroup);
					}
					root = new Group(this.Desktop, this._ingredientsGroup)
					{
						LayoutMode = LayoutMode.Center
					};
				}
				ClientItemCraftingRecipe.ClientCraftingMaterial clientCraftingMaterial = clientItemBase.Recipe.Input[i];
				UIFragment uifragment = document.Instantiate(this.Desktop, root);
				uifragment.Get<Label>("Name").Text = ((clientCraftingMaterial.ItemId != null) ? this.Interface.GetText("items." + clientCraftingMaterial.ItemId + ".name", null, true) : this.Interface.GetText("resourceTypes." + clientCraftingMaterial.ResourceTypeId + ".name", null, true));
				uifragment.Get<Label>("Quantity").Text = "x" + clientCraftingMaterial.Quantity.ToString();
				ItemGrid itemGrid = new ItemGrid(this.Desktop, uifragment.Get<Group>("Container"));
				itemGrid.SlotsPerRow = 1;
				itemGrid.Style = this._inGameView.DefaultItemGridStyle.Clone();
				itemGrid.Style.SlotBackground = null;
				itemGrid.Anchor.Width = new int?(itemGrid.Style.SlotSize);
				itemGrid.RenderItemQualityBackground = false;
				itemGrid.Slots = new ItemGridSlot[1];
				bool flag3 = clientCraftingMaterial.ItemId == null;
				if (flag3)
				{
					PatchStyle icon = null;
					ClientResourceType clientResourceType;
					TextureArea textureArea;
					bool flag4 = this._inGameView.InventoryPage.ResourceTypes.TryGetValue(clientCraftingMaterial.ResourceTypeId, out clientResourceType) && this._inGameView.TryMountAssetTexture(clientResourceType.Icon, out textureArea);
					if (flag4)
					{
						icon = new PatchStyle(textureArea);
					}
					itemGrid.Slots[0] = new ItemGridSlot
					{
						Name = this.Desktop.Provider.GetText("ui.items.resourceTypeTooltip.name", new Dictionary<string, string>
						{
							{
								"name",
								this.Desktop.Provider.GetText("resourceTypes." + clientCraftingMaterial.ResourceTypeId + ".name", null, true)
							}
						}, true),
						Description = this.Desktop.Provider.GetText("resourceTypes." + clientCraftingMaterial.ResourceTypeId + ".description", null, false),
						Icon = icon
					};
				}
				else
				{
					itemGrid.Slots[0] = new ItemGridSlot(new ClientItemStack(clientCraftingMaterial.ItemId, 1));
					itemGrid.Layout(null, true);
				}
			}
		}

		// Token: 0x04001E56 RID: 7766
		private InGameView _inGameView;

		// Token: 0x04001E57 RID: 7767
		private Group _benchesGroup;

		// Token: 0x04001E58 RID: 7768
		private Group _benchCategoriesGroup;

		// Token: 0x04001E59 RID: 7769
		private Group _itemCategoriesGroup;

		// Token: 0x04001E5A RID: 7770
		private Group _recipesGroup;

		// Token: 0x04001E5B RID: 7771
		private ItemPreviewComponent _itemPreviewComponent;

		// Token: 0x04001E5C RID: 7772
		private Label _itemNameLabel;

		// Token: 0x04001E5D RID: 7773
		private Label _itemDescriptionLabel;

		// Token: 0x04001E5E RID: 7774
		private Group _ingredientsGroup;

		// Token: 0x04001E5F RID: 7775
		private Group _categoriesSeparator;

		// Token: 0x04001E60 RID: 7776
		private Tuple<string, string, string, string> _selection = Tuple.Create<string, string, string, string>(null, null, null, null);
	}
}
