using System;
using HytaleClient.Data.Items;
using HytaleClient.Graphics;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Interface.UI.Styles;
using Newtonsoft.Json.Linq;

namespace HytaleClient.Interface.InGame.Pages.InventoryPanels
{
	// Token: 0x0200089D RID: 2205
	internal class ProcessingPanel : WindowPanel
	{
		// Token: 0x170010BA RID: 4282
		// (get) Token: 0x06003FB9 RID: 16313 RVA: 0x000B4088 File Offset: 0x000B2288
		// (set) Token: 0x06003FBA RID: 16314 RVA: 0x000B4090 File Offset: 0x000B2290
		public bool[] CompatibleInputSlots { get; private set; }

		// Token: 0x170010BB RID: 4283
		// (get) Token: 0x06003FBB RID: 16315 RVA: 0x000B4099 File Offset: 0x000B2299
		public int FuelSlotCount
		{
			get
			{
				return this._fuelItemGrid.Slots.Length;
			}
		}

		// Token: 0x06003FBC RID: 16316 RVA: 0x000B40A8 File Offset: 0x000B22A8
		public ProcessingPanel(InGameView inGameView, Element parent = null) : base(inGameView, parent)
		{
		}

		// Token: 0x06003FBD RID: 16317 RVA: 0x000B40B4 File Offset: 0x000B22B4
		public void Build()
		{
			base.Clear();
			Document document;
			this.Interface.TryGetDocument("InGame/Pages/Inventory/ProcessingPanel.ui", out document);
			this._offButtonStyle = document.ResolveNamedValue<TextButton.TextButtonStyle>(this.Interface, "OffButtonStyle");
			this._onButtonStyle = document.ResolveNamedValue<TextButton.TextButtonStyle>(this.Interface, "OnButtonStyle");
			this._fuelSlotActiveOverlay = document.ResolveNamedValue<PatchStyle>(this.Interface, "FuelSlotActiveOverlay");
			this._inputSlotActiveOverlay = document.ResolveNamedValue<PatchStyle>(this.Interface, "InputSlotActiveOverlay");
			UIFragment uifragment = document.Instantiate(this.Desktop, this);
			this._titleLabel = uifragment.Get<Label>("TitleLabel");
			this._fuelContainer = uifragment.Get<Group>("FuelContainer");
			this._fuelInputContainer = uifragment.Get<Group>("FuelInputContainer");
			this._inputProcessingBars = uifragment.Get<Group>("InputProcessingBars");
			this._fuelItemGrid = uifragment.Get<ItemGrid>("FuelItemGrid");
			this._fuelItemGrid.Slots = new ItemGridSlot[1];
			this._fuelItemGrid.SlotClicking = delegate(int slotIndex, int button)
			{
				this._inGameView.HandleInventoryClick(this._inventoryWindow.Id, slotIndex, button);
			};
			this._fuelItemGrid.SlotDoubleClicking = delegate(int slotIndex)
			{
				this._inGameView.HandleInventoryDoubleClick(this._inventoryWindow.Id, slotIndex);
			};
			this._fuelItemGrid.Dropped = delegate(int targetSlotIndex, Element sourceItemGrid, ItemGrid.ItemDragData dragData)
			{
				this._inGameView.HandleInventoryDragEnd(this._fuelItemGrid, this._inventoryWindow.Id, targetSlotIndex, sourceItemGrid, dragData);
			};
			this._fuelItemGrid.DragCancelled = delegate(int slotIndex, int button)
			{
				this._inGameView.HandleInventoryDropItem(this._inventoryWindow.Id, slotIndex, button);
			};
			this._fuelItemGrid.SlotMouseEntered = new Action<int>(this.OnFuelSlotMouseEntered);
			this._fuelItemGrid.SlotMouseExited = new Action<int>(this.OnFuelSlotMouseExited);
			this._inputItemGrid = uifragment.Get<ItemGrid>("InputItemGrid");
			this._inputItemGrid.Slots = new ItemGridSlot[0];
			this._inputItemGrid.SlotClicking = delegate(int slotIndex, int button)
			{
				this._inGameView.HandleInventoryClick(this._inventoryWindow.Id, slotIndex + this._fuelItemGrid.Slots.Length, button);
			};
			this._inputItemGrid.SlotDoubleClicking = delegate(int slotIndex)
			{
				this._inGameView.HandleInventoryDoubleClick(this._inventoryWindow.Id, slotIndex + this._fuelItemGrid.Slots.Length);
			};
			this._inputItemGrid.Dropped = delegate(int targetSlotIndex, Element sourceItemGrid, ItemGrid.ItemDragData dragData)
			{
				this._inGameView.HandleInventoryDragEnd(this._inputItemGrid, this._inventoryWindow.Id, targetSlotIndex + this._fuelItemGrid.Slots.Length, sourceItemGrid, dragData);
			};
			this._inputItemGrid.DragCancelled = delegate(int slotIndex, int button)
			{
				this._inGameView.HandleInventoryDropItem(this._inventoryWindow.Id, slotIndex + this._fuelItemGrid.Slots.Length, button);
			};
			this._inputItemGrid.SlotMouseEntered = new Action<int>(this.OnInputSlotMouseEntered);
			this._inputItemGrid.SlotMouseExited = new Action<int>(this.OnInputSlotMouseExited);
			this._outputItemGrid = uifragment.Get<ItemGrid>("OutputItemGrid");
			this._outputItemGrid.Slots = new ItemGridSlot[0];
			this._outputItemGrid.SlotClicking = delegate(int slotIndex, int button)
			{
				this._inGameView.HandleInventoryClick(this._inventoryWindow.Id, slotIndex + this._fuelItemGrid.Slots.Length + this._inputItemGrid.Slots.Length, button);
			};
			this._outputItemGrid.SlotDoubleClicking = delegate(int slotIndex)
			{
				this._inGameView.HandleInventoryDoubleClick(this._inventoryWindow.Id, slotIndex + this._fuelItemGrid.Slots.Length + this._inputItemGrid.Slots.Length);
			};
			this._outputItemGrid.Dropped = delegate(int targetSlotIndex, Element sourceItemGrid, ItemGrid.ItemDragData dragData)
			{
				this._inGameView.HandleInventoryDragEnd(this._outputItemGrid, this._inventoryWindow.Id, targetSlotIndex + this._fuelItemGrid.Slots.Length + this._inputItemGrid.Slots.Length, sourceItemGrid, dragData);
			};
			this._outputItemGrid.DragCancelled = delegate(int slotIndex, int button)
			{
				this._inGameView.HandleInventoryDropItem(this._inventoryWindow.Id, slotIndex + this._fuelItemGrid.Slots.Length + this._inputItemGrid.Slots.Length, button);
			};
			this._descriptiveLabel = uifragment.Get<Group>("DescriptiveLabel").Find<Label>("PanelTitle");
			this._onOffButton = uifragment.Get<TextButton>("OnOffButton");
			this._onOffButton.Activating = delegate()
			{
				this.SetOn(!this._isOn, true);
			};
			this._onOffButton.Text = this.Desktop.Provider.GetText("ui.windows.processing.turn" + (this._isOn ? "Off" : "On"), null, true);
			this._onOffButton.Style = (this._isOn ? this._onButtonStyle : this._offButtonStyle);
		}

		// Token: 0x06003FBE RID: 16318 RVA: 0x000B4400 File Offset: 0x000B2600
		public void OnSetStacks()
		{
			this.Update();
		}

		// Token: 0x06003FBF RID: 16319 RVA: 0x000B440C File Offset: 0x000B260C
		private void OnFuelSlotMouseEntered(int slotIndex)
		{
			this._inGameView.HandleItemSlotMouseEntered(this._inventoryWindow.Id, slotIndex);
			this._inGameView.InventoryPage.StoragePanel.UpdateGrid();
			this._inGameView.HotbarComponent.SetupGrid();
		}

		// Token: 0x06003FC0 RID: 16320 RVA: 0x000B445C File Offset: 0x000B265C
		private void OnInputSlotMouseEntered(int slotIndex)
		{
			this._inGameView.HandleItemSlotMouseEntered(this._inventoryWindow.Id, slotIndex + this.FuelSlotCount);
			JArray jarray = (JArray)this._inventoryWindow.WindowData["inventoryHints"];
			this.CompatibleInputSlots = new bool[this._inGameView.StorageStacks.Length + this._inGameView.HotbarStacks.Length];
			bool flag = jarray != null;
			if (flag)
			{
				foreach (JToken jtoken in jarray)
				{
					this.CompatibleInputSlots[(int)jtoken] = true;
				}
			}
			this._inGameView.InventoryPage.StoragePanel.UpdateGrid();
			this._inGameView.HotbarComponent.SetupGrid();
		}

		// Token: 0x06003FC1 RID: 16321 RVA: 0x000B4540 File Offset: 0x000B2740
		private void OnFuelSlotMouseExited(int slotIndex)
		{
			this._inGameView.HandleItemSlotMouseExited(this._inventoryWindow.Id, slotIndex);
			this._inGameView.InventoryPage.StoragePanel.UpdateGrid();
			this._inGameView.HotbarComponent.SetupGrid();
		}

		// Token: 0x06003FC2 RID: 16322 RVA: 0x000B4590 File Offset: 0x000B2790
		private void OnInputSlotMouseExited(int slotIndex)
		{
			this._inGameView.HandleItemSlotMouseExited(this._inventoryWindow.Id, slotIndex + this.FuelSlotCount);
			this._inGameView.InventoryPage.StoragePanel.UpdateGrid();
			this._inGameView.HotbarComponent.SetupGrid();
		}

		// Token: 0x06003FC3 RID: 16323 RVA: 0x000B45E4 File Offset: 0x000B27E4
		protected override void Setup()
		{
			this._titleLabel.Text = this.Desktop.Provider.GetText(((string)this._inventoryWindow.WindowData["name"]) ?? "", null, true);
			JArray jarray = (JArray)this._inventoryWindow.WindowData["fuel"];
			this._fuelContainer.Visible = (jarray != null);
			this._fuelItemGrid.InventorySectionId = new int?(this._inventoryWindow.Id);
			this._inputItemGrid.InventorySectionId = new int?(this._inventoryWindow.Id);
			this._outputItemGrid.InventorySectionId = new int?(this._inventoryWindow.Id);
			JArray jarray2 = (JArray)this._inventoryWindow.WindowData["input"];
			this._inputSlotIcons = new PatchStyle[jarray2.Count];
			for (int i = 0; i < this._inputSlotIcons.Length; i++)
			{
				string text = (string)jarray2[i]["icon"];
				TextureArea textureArea;
				bool flag = text != null && this._inGameView.TryMountAssetTexture(text, out textureArea);
				if (flag)
				{
					this._inputSlotIcons[i] = new PatchStyle(textureArea);
				}
				else
				{
					this._inputSlotIcons[i] = null;
				}
			}
			this._fuelSlotIcons = new PatchStyle[(jarray != null) ? jarray.Count : 0];
			for (int j = 0; j < this._fuelSlotIcons.Length; j++)
			{
				string text2 = (string)jarray[j]["icon"];
				TextureArea textureArea2;
				bool flag2 = text2 != null && this._inGameView.TryMountAssetTexture(text2, out textureArea2);
				if (flag2)
				{
					this._fuelSlotIcons[j] = new PatchStyle(textureArea2);
				}
				else
				{
					this._fuelSlotIcons[j] = null;
				}
			}
			int count = jarray2.Count;
			int num = (int)this._inventoryWindow.WindowData["outputSlotsCount"];
			this._inputItemGrid.Slots = new ItemGridSlot[count];
			this._inputItemGrid.Parent.Anchor.Height = this._inputItemGrid.Style.SlotSize * count + this._inputItemGrid.Style.SlotSpacing * (count - 1) + this._inputItemGrid.Parent.Padding.Vertical + this._inputItemGrid.Padding.Vertical;
			this._inputProcessingBars.Clear();
			Document document;
			this.Interface.TryGetDocument("InGame/Pages/Inventory/ProcessingBar.ui", out document);
			for (int k = 0; k < count; k++)
			{
				Group group = document.Instantiate(this.Desktop, this._inputProcessingBars).Get<Group>("ProcessingBarContainer");
				group.Anchor.Top = new int?(k * (this._inputItemGrid.Style.SlotSize + this._inputItemGrid.Style.SlotSpacing) - 1);
			}
			this._outputItemGrid.SlotsPerRow = ((num > 3) ? 2 : 1);
			this._outputItemGrid.Slots = new ItemGridSlot[num];
			this._outputItemGrid.Parent.Anchor.Width = this._outputItemGrid.Style.SlotSize * this._outputItemGrid.SlotsPerRow + this._outputItemGrid.Style.SlotSpacing * (this._outputItemGrid.SlotsPerRow - 1) + this._outputItemGrid.Padding.Vertical;
			int num2 = (int)Math.Ceiling((double)((float)num / (float)this._outputItemGrid.SlotsPerRow));
			this._outputItemGrid.Parent.Anchor.Height = this._outputItemGrid.Style.SlotSize * num2 + this._outputItemGrid.Style.SlotSpacing * (num2 - 1) + this._outputItemGrid.Padding.Vertical;
			int num3 = (jarray != null) ? jarray.Count : 0;
			this._fuelInputContainer.Anchor.Width = new int?(this._fuelItemGrid.Style.SlotSize * num3);
			this._fuelItemGrid.Slots = new ItemGridSlot[num3];
			this._isOn = (bool)this._inventoryWindow.WindowData["active"];
			this._onOffButton.Text = this.Desktop.Provider.GetText("ui.windows.processing.turn" + (this._isOn ? "Off" : "On"), null, true);
			this._onOffButton.Style = (this._isOn ? this._onButtonStyle : this._offButtonStyle);
			this._descriptiveLabel.Text = (this.Desktop.Provider.GetText(string.Format("items.{0}.bench.descriptiveLabel", this._inventoryWindow.WindowData["blockItemId"]), null, false) ?? this.Desktop.Provider.GetText("ui.windows.processing.descriptiveLabel", null, true));
			this.Update();
		}

		// Token: 0x06003FC4 RID: 16324 RVA: 0x000B4BB0 File Offset: 0x000B2DB0
		protected override void Update()
		{
			bool flag = (bool)this._inventoryWindow.WindowData["active"] != this._isOn;
			if (flag)
			{
				this.SetOn((bool)this._inventoryWindow.WindowData["active"], false);
			}
			float num = this._inventoryWindow.WindowData["progress"].ToObject<float>();
			for (int i = 0; i < this._inputItemGrid.Slots.Length; i++)
			{
				ClientItemStack clientItemStack = this._inventoryWindow.Inventory[i + this._fuelItemGrid.Slots.Length];
				bool flag2 = clientItemStack == null;
				if (flag2)
				{
					this._inputItemGrid.Slots[i] = new ItemGridSlot
					{
						Icon = this._inputSlotIcons[i],
						InventorySlotIndex = new int?(i + this._fuelItemGrid.Slots.Length)
					};
				}
				else
				{
					this._inputItemGrid.Slots[i] = new ItemGridSlot
					{
						ItemStack = clientItemStack,
						InventorySlotIndex = new int?(i + this._fuelItemGrid.Slots.Length),
						Overlay = (((double)num > 0.001 && num < 1f) ? this._inputSlotActiveOverlay : null)
					};
				}
			}
			this._inputItemGrid.Layout(null, true);
			int slotOffset = this._inputItemGrid.Slots.Length + this._fuelItemGrid.Slots.Length;
			this._outputItemGrid.SetItemStacks(this._inventoryWindow.Inventory, slotOffset);
			this._outputItemGrid.Layout(null, true);
			foreach (Element element in this._inputProcessingBars.Children)
			{
				ProgressBar progressBar = element.Find<ProgressBar>("ProcessingBar");
				progressBar.Value = num;
				progressBar.Layout(null, true);
			}
			bool visible = this._fuelContainer.Visible;
			if (visible)
			{
				for (int j = 0; j < this._fuelItemGrid.Slots.Length; j++)
				{
					ClientItemStack clientItemStack2 = this._inventoryWindow.Inventory[j];
					bool flag3 = clientItemStack2 == null && this._fuelSlotIcons[j] != null;
					if (flag3)
					{
						this._fuelItemGrid.Slots[j] = new ItemGridSlot
						{
							Icon = this._fuelSlotIcons[j],
							Overlay = (this._isOn ? this._fuelSlotActiveOverlay : null)
						};
					}
					else
					{
						this._fuelItemGrid.Slots[j] = new ItemGridSlot
						{
							ItemStack = clientItemStack2,
							Overlay = (this._isOn ? this._fuelSlotActiveOverlay : null)
						};
					}
				}
				this._fuelItemGrid.Layout(null, true);
			}
		}

		// Token: 0x06003FC5 RID: 16325 RVA: 0x000B4ED0 File Offset: 0x000B30D0
		private void SetOn(bool isOn, bool sendPacket = true)
		{
			this._isOn = isOn;
			this._onOffButton.Text = this.Desktop.Provider.GetText("ui.windows.processing.turn" + (this._isOn ? "Off" : "On"), null, true);
			this._onOffButton.Style = (this._isOn ? this._onButtonStyle : this._offButtonStyle);
			this._onOffButton.Layout(null, true);
			bool isMounted = this._fuelItemGrid.IsMounted;
			if (isMounted)
			{
				this.Update();
			}
			if (sendPacket)
			{
				InventoryPage inventoryPage = this._inGameView.InventoryPage;
				int id = this._inventoryWindow.Id;
				string action = "setActive";
				JObject jobject = new JObject();
				jobject.Add("state", isOn);
				inventoryPage.SendWindowAction(id, action, jobject);
			}
		}

		// Token: 0x04001E45 RID: 7749
		private Label _titleLabel;

		// Token: 0x04001E46 RID: 7750
		private Label _descriptiveLabel;

		// Token: 0x04001E47 RID: 7751
		private Group _fuelContainer;

		// Token: 0x04001E48 RID: 7752
		private Group _fuelInputContainer;

		// Token: 0x04001E49 RID: 7753
		private Group _inputProcessingBars;

		// Token: 0x04001E4A RID: 7754
		private ItemGrid _fuelItemGrid;

		// Token: 0x04001E4B RID: 7755
		private ItemGrid _inputItemGrid;

		// Token: 0x04001E4C RID: 7756
		private ItemGrid _outputItemGrid;

		// Token: 0x04001E4D RID: 7757
		private TextButton _onOffButton;

		// Token: 0x04001E4E RID: 7758
		private TextButton.TextButtonStyle _offButtonStyle;

		// Token: 0x04001E4F RID: 7759
		private TextButton.TextButtonStyle _onButtonStyle;

		// Token: 0x04001E50 RID: 7760
		private PatchStyle[] _inputSlotIcons;

		// Token: 0x04001E51 RID: 7761
		private PatchStyle[] _fuelSlotIcons;

		// Token: 0x04001E52 RID: 7762
		private bool _isOn;

		// Token: 0x04001E53 RID: 7763
		private PatchStyle _inputSlotActiveOverlay;

		// Token: 0x04001E54 RID: 7764
		private PatchStyle _fuelSlotActiveOverlay;
	}
}
