using System;
using System.Collections.Generic;
using HytaleClient.Data.Items;
using HytaleClient.Graphics;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Interface.UI.Styles;
using Newtonsoft.Json.Linq;

namespace HytaleClient.Interface.InGame.Pages.InventoryPanels
{
	// Token: 0x020008A0 RID: 2208
	internal class StructuralCraftingPanel : WindowPanel
	{
		// Token: 0x170010BE RID: 4286
		// (get) Token: 0x06003FEF RID: 16367 RVA: 0x000B64C9 File Offset: 0x000B46C9
		// (set) Token: 0x06003FF0 RID: 16368 RVA: 0x000B64D1 File Offset: 0x000B46D1
		public bool[] CompatibleSlots { get; private set; }

		// Token: 0x06003FF1 RID: 16369 RVA: 0x000B64DA File Offset: 0x000B46DA
		public StructuralCraftingPanel(InGameView inGameView, Element parent = null) : base(inGameView, parent)
		{
		}

		// Token: 0x06003FF2 RID: 16370 RVA: 0x000B64F0 File Offset: 0x000B46F0
		public void Build()
		{
			base.Clear();
			bool hasMarkupError = this.Interface.HasMarkupError;
			if (!hasMarkupError)
			{
				Document document;
				this.Interface.TryGetDocument("InGame/Pages/Inventory/StructuralCraftingPanel.ui", out document);
				this._slotBackground = document.ResolveNamedValue<PatchStyle>(this.Desktop.Provider, "SlotBackground");
				this._slotSelectedBackground = document.ResolveNamedValue<PatchStyle>(this.Desktop.Provider, "SlotSelectedBackground");
				this._slotSelectedOverlay = document.ResolveNamedValue<PatchStyle>(this.Desktop.Provider, "SlotSelectedOverlay");
				this._inputSlotIcon = document.ResolveNamedValue<PatchStyle>(this.Desktop.Provider, "InputSlotIcon");
				UIFragment uifragment = document.Instantiate(this.Desktop, this);
				this._titleLabel = uifragment.Get<Label>("TitleLabel");
				this._itemPreview = uifragment.Get<ItemPreviewComponent>("ItemPreview");
				this._categoryPreview = uifragment.Get<Group>("CategoryPreview");
				Document document2;
				this.Interface.TryGetDocument("Common.ui", out document2);
				this._inputItemGrid = new ItemGrid(this.Desktop, uifragment.Get<Group>("InputContainer"))
				{
					SlotsPerRow = 1,
					RenderItemQualityBackground = false,
					Slots = new ItemGridSlot[1],
					SlotMouseEntered = new Action<int>(this.OnInputSlotMouseEntered),
					SlotMouseExited = new Action<int>(this.OnInputSlotMouseExited)
				};
				this._inputItemGrid.Style = this._inGameView.DefaultItemGridStyle.Clone();
				this._inputItemGrid.Style.SlotBackground = null;
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
				this._optionsItemGrid = new ItemGrid(this.Desktop, uifragment.Get<Group>("OptionsContainer"))
				{
					SlotsPerRow = 4,
					Slots = new ItemGridSlot[12],
					AreItemsDraggable = false,
					Style = this._inGameView.DefaultItemGridStyle,
					SlotClicking = delegate(int slotIndex, int button)
					{
						this._selectedSlot = slotIndex;
						this.Update();
						base.Layout(null, true);
						InventoryPage inventoryPage = this._inGameView.InventoryPage;
						int id = this._inventoryWindow.Id;
						string action = "select";
						JObject jobject = new JObject();
						jobject.Add("slot", slotIndex);
						inventoryPage.SendWindowAction(id, action, jobject);
					}
				};
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
					this.Craft(this.GetCraftableQuantity());
				};
				this._itemName = uifragment.Get<Group>("ItemName").Find<Label>("PanelTitle");
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

		// Token: 0x06003FF3 RID: 16371 RVA: 0x000B67F0 File Offset: 0x000B49F0
		protected override void Setup()
		{
			this._titleLabel.Text = this.Desktop.Provider.GetText(((string)this._inventoryWindow.WindowData["name"]) ?? "", null, true);
			this._inputItemGrid.InventorySectionId = new int?(this._inventoryWindow.Id);
			this._inputItemGrid.Slots = new ItemGridSlot[1];
			JArray jarray = (JArray)this._inventoryWindow.WindowData["options"];
			bool flag = jarray.Count > 16;
			if (flag)
			{
				throw new Exception("Amount of input slots cannot exceed 16");
			}
			this._optionIcons = new PatchStyle[jarray.Count];
			this._optionIds = new string[jarray.Count];
			for (int i = 0; i < this._optionIcons.Length; i++)
			{
				string text = (string)jarray[i]["icon"];
				TextureArea textureArea;
				bool flag2 = text != null && this._inGameView.TryMountAssetTexture(text, out textureArea);
				if (flag2)
				{
					this._optionIcons[i] = new PatchStyle(textureArea);
				}
				else
				{
					this._optionIcons[i] = null;
				}
				this._optionIds[i] = (string)jarray[i]["id"];
			}
			this._optionsItemGrid.Slots = new ItemGridSlot[this._optionIcons.Length];
			int num = (int)Math.Ceiling((double)((float)this._optionIcons.Length / (float)this._optionsItemGrid.SlotsPerRow));
			int num2 = (this._optionIcons.Length > this._optionsItemGrid.SlotsPerRow) ? this._optionsItemGrid.SlotsPerRow : this._optionIcons.Length;
			this._optionsItemGrid.Parent.Anchor.Height = new int?(this._optionsItemGrid.Style.SlotSize * num + this._optionsItemGrid.Style.SlotSpacing * (num - 1));
			this._optionsItemGrid.Parent.Anchor.Width = new int?(this._optionsItemGrid.Style.SlotSize * num2 + this._optionsItemGrid.Style.SlotSpacing * (num2 - 1));
			this.Update();
		}

		// Token: 0x06003FF4 RID: 16372 RVA: 0x000B6A40 File Offset: 0x000B4C40
		protected override void Update()
		{
			bool flag = this._inventoryWindow.Inventory[0] == null;
			if (flag)
			{
				this._inputItemGrid.Slots[0] = new ItemGridSlot
				{
					Icon = this._inputSlotIcon
				};
			}
			else
			{
				this._inputItemGrid.Slots[0] = new ItemGridSlot(this._inventoryWindow.Inventory[0]);
			}
			this._inputItemGrid.Layout(null, true);
			bool flag2 = false;
			for (int i = 0; i < this._optionsItemGrid.Slots.Length; i++)
			{
				ClientItemStack clientItemStack = this._inventoryWindow.Inventory[i + 1];
				bool flag3 = clientItemStack != null;
				ItemGridSlot itemGridSlot;
				if (flag3)
				{
					itemGridSlot = (this._optionsItemGrid.Slots[i] = new ItemGridSlot(clientItemStack));
					flag2 = true;
				}
				else
				{
					ItemGridSlot[] slots = this._optionsItemGrid.Slots;
					int num = i;
					ItemGridSlot itemGridSlot2 = new ItemGridSlot();
					itemGridSlot2.Icon = this._optionIcons[i];
					ItemGridSlot itemGridSlot3 = itemGridSlot2;
					slots[num] = itemGridSlot2;
					itemGridSlot = itemGridSlot3;
				}
				bool flag4 = i != this._selectedSlot;
				if (flag4)
				{
					bool flag5 = itemGridSlot.Icon == null;
					if (flag5)
					{
						itemGridSlot.Background = this._slotBackground;
					}
				}
				else
				{
					itemGridSlot.Background = this._slotSelectedBackground;
					itemGridSlot.Overlay = this._slotSelectedOverlay;
				}
			}
			this._optionsItemGrid.Layout(null, true);
			this.UpdateItemPreview();
			ClientItemStack clientItemStack2 = this._inventoryWindow.Inventory[this._selectedSlot + 1];
			string text = (clientItemStack2 != null) ? clientItemStack2.Id : null;
			this._itemName.Text = ((text == null) ? this.Desktop.Provider.GetText(string.Concat(new string[]
			{
				"items.",
				(string)this._inGameView.InventoryWindow.WindowData["blockItemId"],
				".bench.options.",
				this._optionIds[this._selectedSlot],
				".name"
			}), null, true) : this.Desktop.Provider.GetText("items." + text + ".name", null, true));
			this._itemName.Layout(null, true);
			bool flag6 = flag2 && this._inventoryWindow.Inventory[0] != null && text == null && this._optionIcons[this._selectedSlot] != null;
			if (flag6)
			{
				this._categoryPreview.Background = this._optionIcons[this._selectedSlot];
			}
			else
			{
				this._categoryPreview.Background = null;
			}
			this._categoryPreview.Layout(null, true);
			int craftableQuantity = this.GetCraftableQuantity();
			this._craft1Button.Disabled = (craftableQuantity < 1);
			this._craft1Button.Layout(null, true);
			this._craft10Button.Disabled = (craftableQuantity < 10);
			this._craft10Button.Layout(null, true);
			this._craftAllButton.Text = this.Desktop.Provider.GetText("ui.windows.crafting.craftX", new Dictionary<string, string>
			{
				{
					"count",
					this.Desktop.Provider.FormatNumber(craftableQuantity)
				}
			}, true);
			this._craftAllButton.Disabled = (craftableQuantity < 1);
			this._craftAllButton.Layout(null, true);
		}

		// Token: 0x06003FF5 RID: 16373 RVA: 0x000B6DC0 File Offset: 0x000B4FC0
		public void UpdateItemPreview()
		{
			ClientItemStack clientItemStack = this._inventoryWindow.Inventory[this._selectedSlot + 1];
			string text = (clientItemStack != null) ? clientItemStack.Id : null;
			bool flag = text != null && this.Desktop.GetLayer(2) == null;
			if (flag)
			{
				this._itemPreview.SetItemId(text);
			}
			else
			{
				this._itemPreview.SetItemId(null);
			}
		}

		// Token: 0x06003FF6 RID: 16374 RVA: 0x000B6E28 File Offset: 0x000B5028
		private void OnInputSlotMouseEntered(int slotIndex)
		{
			this._inGameView.HandleItemSlotMouseEntered(this._inventoryWindow.Id, slotIndex);
			bool isMouseDragging = this.Desktop.IsMouseDragging;
			if (!isMouseDragging)
			{
				JArray jarray = (JArray)this._inventoryWindow.WindowData["inventoryHints"];
				bool flag = jarray == null;
				if (!flag)
				{
					this.CompatibleSlots = new bool[this._inGameView.StorageStacks.Length + this._inGameView.HotbarStacks.Length];
					foreach (JToken jtoken in jarray)
					{
						this.CompatibleSlots[(int)jtoken] = true;
					}
					this._inGameView.InventoryPage.StoragePanel.UpdateGrid();
					this._inGameView.HotbarComponent.SetupGrid();
				}
			}
		}

		// Token: 0x06003FF7 RID: 16375 RVA: 0x000B6F20 File Offset: 0x000B5120
		private void OnInputSlotMouseExited(int slotIndex)
		{
			this._inGameView.HandleItemSlotMouseExited(this._inventoryWindow.Id, slotIndex);
			this._inGameView.InventoryPage.StoragePanel.UpdateGrid();
			this._inGameView.HotbarComponent.SetupGrid();
		}

		// Token: 0x06003FF8 RID: 16376 RVA: 0x000B6F6D File Offset: 0x000B516D
		public void OnSetStacks()
		{
			this.Update();
		}

		// Token: 0x06003FF9 RID: 16377 RVA: 0x000B6F76 File Offset: 0x000B5176
		private void Craft(int quantity)
		{
			InventoryPage inventoryPage = this._inGameView.InventoryPage;
			int id = this._inventoryWindow.Id;
			string action = "craftItem";
			JObject jobject = new JObject();
			jobject.Add("quantity", quantity);
			inventoryPage.SendWindowAction(id, action, jobject);
		}

		// Token: 0x06003FFA RID: 16378 RVA: 0x000B6FB4 File Offset: 0x000B51B4
		private int GetCraftableQuantity()
		{
			ClientItemStack clientItemStack = this._inventoryWindow.Inventory[this._selectedSlot + 1];
			string text = (clientItemStack != null) ? clientItemStack.Id : null;
			ClientItemBase clientItemBase;
			bool flag = text == null || !this._inGameView.Items.TryGetValue(text, out clientItemBase);
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				ClientItemCraftingRecipe.ClientCraftingMaterial clientCraftingMaterial = clientItemBase.Recipe.Input[0];
				bool flag2 = clientCraftingMaterial.Quantity == 0;
				if (flag2)
				{
					result = 0;
				}
				else
				{
					int num = (clientCraftingMaterial.ResourceTypeId != null) ? this.CountResourceType(clientCraftingMaterial.ResourceTypeId) : this.CountItem(clientCraftingMaterial.ItemId);
					result = (int)Math.Floor((double)((float)num / (float)clientCraftingMaterial.Quantity));
				}
			}
			return result;
		}

		// Token: 0x06003FFB RID: 16379 RVA: 0x000B7068 File Offset: 0x000B5268
		private int CountItem(string itemId)
		{
			ClientItemStack clientItemStack = this._inventoryWindow.Inventory[0];
			return (((clientItemStack != null) ? clientItemStack.Id : null) == itemId) ? clientItemStack.Quantity : 0;
		}

		// Token: 0x06003FFC RID: 16380 RVA: 0x000B70A8 File Offset: 0x000B52A8
		private int CountResourceType(string resourceTypeId)
		{
			ClientItemStack clientItemStack = this._inventoryWindow.Inventory[0];
			ClientItemBase clientItemBase;
			bool flag = clientItemStack == null || !this._inGameView.Items.TryGetValue(clientItemStack.Id, out clientItemBase) || clientItemBase.ResourceTypes == null;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				foreach (ClientItemResourceType clientItemResourceType in clientItemBase.ResourceTypes)
				{
					bool flag2 = clientItemResourceType.Id == resourceTypeId;
					if (flag2)
					{
						return clientItemResourceType.Quantity * clientItemStack.Quantity;
					}
				}
				result = 0;
			}
			return result;
		}

		// Token: 0x06003FFD RID: 16381 RVA: 0x000B7144 File Offset: 0x000B5344
		public void ResetState()
		{
			this._selectedSlot = 0;
			this._inputItemGrid.Slots = new ItemGridSlot[0];
			this._optionsItemGrid.Slots = new ItemGridSlot[0];
			this._itemPreview.SetItemId(null);
			this._itemName.Text = "";
			this._optionIcons = null;
			this._optionIds = null;
			this._craft1Button.Disabled = true;
			this._craft10Button.Disabled = true;
			this._craftAllButton.Disabled = true;
		}

		// Token: 0x04001E68 RID: 7784
		private Label _titleLabel;

		// Token: 0x04001E69 RID: 7785
		private ItemGrid _inputItemGrid;

		// Token: 0x04001E6A RID: 7786
		private ItemGrid _optionsItemGrid;

		// Token: 0x04001E6B RID: 7787
		private Group _categoryPreview;

		// Token: 0x04001E6C RID: 7788
		private Label _itemName;

		// Token: 0x04001E6D RID: 7789
		private ItemPreviewComponent _itemPreview;

		// Token: 0x04001E6E RID: 7790
		private TextButton _craft1Button;

		// Token: 0x04001E6F RID: 7791
		private TextButton _craft10Button;

		// Token: 0x04001E70 RID: 7792
		private TextButton _craftAllButton;

		// Token: 0x04001E71 RID: 7793
		private PatchStyle _slotBackground;

		// Token: 0x04001E72 RID: 7794
		private PatchStyle _slotSelectedBackground;

		// Token: 0x04001E73 RID: 7795
		private PatchStyle _slotSelectedOverlay;

		// Token: 0x04001E74 RID: 7796
		private PatchStyle _inputSlotIcon;

		// Token: 0x04001E75 RID: 7797
		private int _selectedSlot = 0;

		// Token: 0x04001E76 RID: 7798
		private PatchStyle[] _optionIcons;

		// Token: 0x04001E77 RID: 7799
		private string[] _optionIds;
	}
}
