using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using HytaleClient.Data.Items;
using HytaleClient.Interface.InGame.Pages.InventoryPanels;
using HytaleClient.Interface.InGame.Pages.InventoryPanels.SelectionCommands;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Interface.UI.Styles;
using HytaleClient.Math;
using HytaleClient.Networking;
using Newtonsoft.Json.Linq;
using SDL2;

namespace HytaleClient.Interface.InGame.Pages
{
	// Token: 0x0200088C RID: 2188
	internal class InventoryPage : InterfaceComponent
	{
		// Token: 0x170010A8 RID: 4264
		// (get) Token: 0x06003E8E RID: 16014 RVA: 0x000A8988 File Offset: 0x000A6B88
		// (set) Token: 0x06003E8F RID: 16015 RVA: 0x000A8990 File Offset: 0x000A6B90
		public SoundStyle OpenSound { get; private set; }

		// Token: 0x170010A9 RID: 4265
		// (get) Token: 0x06003E90 RID: 16016 RVA: 0x000A8999 File Offset: 0x000A6B99
		// (set) Token: 0x06003E91 RID: 16017 RVA: 0x000A89A1 File Offset: 0x000A6BA1
		public SoundStyle CloseSound { get; private set; }

		// Token: 0x170010AA RID: 4266
		// (get) Token: 0x06003E92 RID: 16018 RVA: 0x000A89AA File Offset: 0x000A6BAA
		// (set) Token: 0x06003E93 RID: 16019 RVA: 0x000A89B2 File Offset: 0x000A6BB2
		public ClientCraftingCategory[] FieldcraftCategories { get; private set; }

		// Token: 0x170010AB RID: 4267
		// (get) Token: 0x06003E94 RID: 16020 RVA: 0x000A89BB File Offset: 0x000A6BBB
		public Dictionary<string, ClientItemCraftingRecipe> KnownCraftingRecipes { get; } = new Dictionary<string, ClientItemCraftingRecipe>();

		// Token: 0x170010AC RID: 4268
		// (get) Token: 0x06003E95 RID: 16021 RVA: 0x000A89C3 File Offset: 0x000A6BC3
		// (set) Token: 0x06003E96 RID: 16022 RVA: 0x000A89CB File Offset: 0x000A6BCB
		public Dictionary<string, ClientResourceType> ResourceTypes { get; private set; } = new Dictionary<string, ClientResourceType>();

		// Token: 0x06003E97 RID: 16023 RVA: 0x000A89D4 File Offset: 0x000A6BD4
		public InventoryPage(InGameView inGameView) : base(inGameView.Interface, null)
		{
			this.InGameView = inGameView;
			this.ItemSelectorPopover = new ItemSlotSelectorPopover(inGameView, null);
			this.ContainerPanel = new ContainerPanel(this.InGameView, null)
			{
				Visible = false
			};
			this.BasicCraftingPanel = new BasicCraftingPanel(this.InGameView, null)
			{
				Visible = false
			};
			this.StructuralCraftingPanel = new StructuralCraftingPanel(this.InGameView, null)
			{
				Visible = false
			};
			this.ProcessingPanel = new ProcessingPanel(this.InGameView, null)
			{
				Visible = false
			};
			this.DiagramCraftingPanel = new DiagramCraftingPanel(this.InGameView, null)
			{
				Visible = false
			};
			this.BlockInfoPanel = new BlockInfoPanel(this.InGameView, null)
			{
				Visible = false
			};
			this.CharacterPanel = new CharacterPanel(this.InGameView, null);
			this.ItemLibraryPanel = new ItemLibraryPanel(this.InGameView, null)
			{
				Visible = false
			};
			this.StoragePanel = new StoragePanel(this.InGameView, null);
			this.BuilderToolPanel = new BuilderToolPanel(this.InGameView, null)
			{
				Visible = false
			};
			this.BuilderToolPanel.Visible = false;
			this.SelectionCommandsPanel = new SelectionCommandsPanel(this.InGameView, null)
			{
				Visible = false
			};
			this.SelectionCommandsPanel.Visible = false;
			this.RecipeCataloguePopup = new RecipeCataloguePopup(inGameView, null);
			this.Interface.RegisterForEventFromEngine<ClientCraftingCategory[]>("fieldcraftCategories.initialized", new Action<ClientCraftingCategory[]>(this.OnFieldcraftCategoriesInitialized));
			this.Interface.RegisterForEventFromEngine<ClientCraftingCategory[]>("fieldcraftCategories.added", new Action<ClientCraftingCategory[]>(this.OnFieldcraftCategoriesAdded));
			this.Interface.RegisterForEventFromEngine<PacketHandler.ClientKnownRecipe[]>("crafting.knownRecipesUpdated", new Action<PacketHandler.ClientKnownRecipe[]>(this.OnKnownRecipesUpdated));
			this.Interface.RegisterForEventFromEngine<Dictionary<string, ClientResourceType>>("resourceTypes.initialized", new Action<Dictionary<string, ClientResourceType>>(this.OnResourceTypesInitialized));
			this.Interface.RegisterForEventFromEngine<Dictionary<string, ClientResourceType>>("resourceTypes.added", new Action<Dictionary<string, ClientResourceType>>(this.OnResourceTypesAdded));
			this.Interface.RegisterForEventFromEngine<string[]>("resourceTypes.removed", new Action<string[]>(this.OnResourceTypesRemoved));
		}

		// Token: 0x06003E98 RID: 16024 RVA: 0x000A8C00 File Offset: 0x000A6E00
		public void Build()
		{
			Group group = base.Find<Group>("CenterPanel");
			if (group != null)
			{
				group.Clear();
			}
			Group group2 = base.Find<Group>("Root");
			if (group2 != null)
			{
				group2.Clear();
			}
			base.Clear();
			Document document;
			this.Interface.TryGetDocument("InGame/Pages/Inventory/InventoryPage.ui", out document);
			this.OpenSound = document.ResolveNamedValue<SoundStyle>(this.Desktop.Provider, "OpenSound");
			this.CloseSound = document.ResolveNamedValue<SoundStyle>(this.Desktop.Provider, "CloseSound");
			UIFragment uifragment = document.Instantiate(this.Desktop, this);
			this._hotbarHeight = document.ResolveNamedValue<int>(this.Interface, "HotbarHeight");
			this._panelSpacing = document.ResolveNamedValue<int>(this.Interface, "PanelSpacing");
			Group group3 = uifragment.Get<Group>("Root");
			this.BlockInfoPanel.Build();
			group3.Add(this.BlockInfoPanel, -1);
			this.CharacterPanel.Build();
			group3.Add(this.CharacterPanel, 0);
			this.BuilderToolPanel.Build();
			group3.Add(this.BuilderToolPanel, -1);
			this.SelectionCommandsPanel.Build();
			group3.Add(this.SelectionCommandsPanel, -1);
			Group group4 = uifragment.Get<Group>("CenterPanel");
			this.ContainerPanel.Build();
			group4.Add(this.ContainerPanel, -1);
			this.BasicCraftingPanel.Build();
			group4.Add(this.BasicCraftingPanel, -1);
			this.StructuralCraftingPanel.Build();
			group4.Add(this.StructuralCraftingPanel, -1);
			this.DiagramCraftingPanel.Build();
			group4.Add(this.DiagramCraftingPanel, -1);
			this.ProcessingPanel.Build();
			group4.Add(this.ProcessingPanel, -1);
			this.ItemLibraryPanel.Build();
			group4.Add(this.ItemLibraryPanel, -1);
			this.StoragePanel.Build();
			group4.Add(this.StoragePanel, -1);
			this.RecipeCataloguePopup.Build();
			this.ItemSelectorPopover.Visible = false;
			this.ItemSelectorPopover.Build();
			base.Add(this.ItemSelectorPopover, -1);
			bool isMounted = base.IsMounted;
			if (isMounted)
			{
				this.InGameView.HotbarComponent.OnToggleInventoryOpen();
				base.Add(this.InGameView.HotbarComponent, -1);
				base.Add(this.InGameView.InputBindingsComponent, -1);
				this.SetupWindows();
			}
		}

		// Token: 0x06003E99 RID: 16025 RVA: 0x000A8E84 File Offset: 0x000A7084
		protected override void OnMounted()
		{
			base.OnMounted();
			this.InGameView.InGame.SetSceneBlurEnabled(true);
			this.InGameView.HotbarComponent.UpdateActiveItemNameLabel();
			this.InGameView.HotbarComponent.OnToggleInventoryOpen();
			this.InGameView.HudContainer.Remove(this.InGameView.HotbarComponent);
			this.InGameView.HudContainer.Remove(this.InGameView.InputBindingsComponent);
			base.Add(this.InGameView.HotbarComponent, -1);
			base.Add(this.InGameView.InputBindingsComponent, -1);
			this.SetupWindows();
			base.Layout(null, true);
			this.InGameView.SetupCursorFloatingItem();
		}

		// Token: 0x06003E9A RID: 16026 RVA: 0x000A8F50 File Offset: 0x000A7150
		protected override void OnUnmounted()
		{
			base.OnUnmounted();
			this.InGameView.InGame.SetSceneBlurEnabled(false);
			base.Remove(this.InGameView.HotbarComponent);
			base.Remove(this.InGameView.InputBindingsComponent);
			this.InGameView.HotbarComponent.UpdateActiveItemNameLabel();
			this.InGameView.HotbarComponent.OnToggleInventoryOpen();
			this.InGameView.HudContainer.Add(this.InGameView.HotbarComponent, -1);
			this.InGameView.HudContainer.Add(this.InGameView.InputBindingsComponent, -1);
			this.InGameView.HudContainer.Layout(null, true);
			bool flag = this.InGameView.ItemDragData != null;
			if (flag)
			{
				this.InGameView.SetupDragAndDropItem(null);
			}
		}

		// Token: 0x06003E9B RID: 16027 RVA: 0x000A9034 File Offset: 0x000A7234
		public override Element HitTest(Point position)
		{
			Debug.Assert(base.IsMounted);
			bool flag = !this._anchoredRectangle.Contains(position);
			Element result;
			if (flag)
			{
				result = null;
			}
			else
			{
				result = (base.HitTest(position) ?? this);
			}
			return result;
		}

		// Token: 0x06003E9C RID: 16028 RVA: 0x000A9078 File Offset: 0x000A7278
		protected internal override void OnKeyDown(SDL.SDL_Keycode keycode, int repeat)
		{
			base.OnKeyDown(keycode, repeat);
			bool isShiftKeyDown = this.Desktop.IsShiftKeyDown;
			if (isShiftKeyDown)
			{
				sbyte b = 0;
				while (b < 9)
				{
					bool flag = keycode == SDL.SDL_Keycode.SDLK_1 + (int)b;
					if (flag)
					{
						bool flag2 = this.InGameView.HandleHotbarLoad(b);
						if (flag2)
						{
							return;
						}
						break;
					}
					else
					{
						b += 1;
					}
				}
				bool flag3 = keycode == SDL.SDL_Keycode.SDLK_0;
				if (flag3)
				{
					bool flag4 = this.InGameView.HandleHotbarLoad(9);
					if (flag4)
					{
						return;
					}
				}
			}
			bool flag5 = keycode >= SDL.SDL_Keycode.SDLK_0 && keycode <= SDL.SDL_Keycode.SDLK_9;
			if (flag5)
			{
				InGameView.ClientInventoryPosition hoveredItemSlot = this.InGameView.HoveredItemSlot;
				bool flag6 = hoveredItemSlot == null;
				if (!flag6)
				{
					ClientItemStack clientItemStack = this.InGameView.GetItemStacks(hoveredItemSlot.InventorySectionId)[hoveredItemSlot.SlotId];
					bool flag7 = clientItemStack == null;
					if (!flag7)
					{
						int targetSlotIndex = (keycode == SDL.SDL_Keycode.SDLK_0) ? 9 : (keycode - SDL.SDL_Keycode.SDLK_1);
						this.InGameView.MoveItemStack(hoveredItemSlot.InventorySectionId, hoveredItemSlot.SlotId, -1, targetSlotIndex, clientItemStack);
					}
				}
			}
			else
			{
				bool isMounted = this.ItemLibraryPanel.IsMounted;
				if (isMounted)
				{
					bool flag8 = this.Desktop.IsShortcutKeyDown && keycode == SDL.SDL_Keycode.SDLK_f;
					if (flag8)
					{
						this.Desktop.FocusElement(this.ItemLibraryPanel.SearchField, true);
					}
					else
					{
						bool flag9 = this.Desktop.IsShiftKeyDown && keycode == SDL.SDL_Keycode.SDLK_h;
						if (flag9)
						{
							bool flag10 = this.ItemLibraryPanel.HoveredItemId != null;
							if (flag10)
							{
								this.InGameView.InGame.OpenAssetIdInAssetEditor("Item", this.ItemLibraryPanel.HoveredItemId);
							}
							else
							{
								InGameView.ClientInventoryPosition hoveredItemSlot2 = this.InGameView.HoveredItemSlot;
								bool flag11 = hoveredItemSlot2 == null;
								if (!flag11)
								{
									ClientItemStack clientItemStack2 = this.InGameView.GetItemStacks(hoveredItemSlot2.InventorySectionId)[hoveredItemSlot2.SlotId];
									bool flag12 = clientItemStack2 == null;
									if (!flag12)
									{
										this.InGameView.InGame.OpenAssetIdInAssetEditor("Item", clientItemStack2.Id);
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06003E9D RID: 16029 RVA: 0x000A929C File Offset: 0x000A749C
		public void OnSetStacks()
		{
			bool flag = !base.IsMounted;
			if (!flag)
			{
				this.StoragePanel.UpdateGrid();
				this.CharacterPanel.UpdateGrid();
				bool visible = this.BasicCraftingPanel.Visible;
				if (visible)
				{
					this.BasicCraftingPanel.OnSetStacks();
				}
				bool visible2 = this.StructuralCraftingPanel.Visible;
				if (visible2)
				{
					this.StructuralCraftingPanel.OnSetStacks();
				}
				bool visible3 = this.DiagramCraftingPanel.Visible;
				if (visible3)
				{
					this.DiagramCraftingPanel.OnSetStacks();
				}
				bool visible4 = this.ProcessingPanel.Visible;
				if (visible4)
				{
					this.ProcessingPanel.OnSetStacks();
				}
			}
		}

		// Token: 0x06003E9E RID: 16030 RVA: 0x000A9344 File Offset: 0x000A7544
		public void OnItemsUpdated()
		{
			this.ItemLibraryPanel.OnItemsUpdated();
			bool flag = !base.IsMounted;
			if (!flag)
			{
				bool visible = this.ItemLibraryPanel.Visible;
				if (visible)
				{
					this.ItemLibraryPanel.UpdateItemLibrary();
				}
				this.OnSetStacks();
			}
		}

		// Token: 0x06003E9F RID: 16031 RVA: 0x000A9390 File Offset: 0x000A7590
		public void OnItemIconsUpdated()
		{
			bool flag = !base.IsMounted || this.InGameView.Items == null;
			if (!flag)
			{
				bool visible = this.ItemLibraryPanel.Visible;
				if (visible)
				{
					this.ItemLibraryPanel.UpdateItemLibrary();
				}
				this.OnSetStacks();
			}
		}

		// Token: 0x06003EA0 RID: 16032 RVA: 0x000A93DF File Offset: 0x000A75DF
		private void OnFieldcraftCategoriesInitialized(ClientCraftingCategory[] categories)
		{
			this.FieldcraftCategories = categories;
		}

		// Token: 0x06003EA1 RID: 16033 RVA: 0x000A93EC File Offset: 0x000A75EC
		private void OnFieldcraftCategoriesAdded(ClientCraftingCategory[] categories)
		{
			Dictionary<string, ClientCraftingCategory> dictionary = new Dictionary<string, ClientCraftingCategory>();
			foreach (ClientCraftingCategory clientCraftingCategory in this.FieldcraftCategories)
			{
				dictionary[clientCraftingCategory.Id] = clientCraftingCategory;
			}
			foreach (ClientCraftingCategory clientCraftingCategory2 in categories)
			{
				dictionary[clientCraftingCategory2.Id] = clientCraftingCategory2;
			}
			this.FieldcraftCategories = Enumerable.ToArray<ClientCraftingCategory>(dictionary.Values);
			bool flag = this.BasicCraftingPanel.IsMounted && this.IsFieldcraft;
			if (flag)
			{
				this.BasicCraftingPanel.RefreshWindow();
			}
		}

		// Token: 0x06003EA2 RID: 16034 RVA: 0x000A9490 File Offset: 0x000A7690
		private void OnKnownRecipesUpdated(PacketHandler.ClientKnownRecipe[] knownRecipes)
		{
			this.KnownCraftingRecipes.Clear();
			foreach (PacketHandler.ClientKnownRecipe clientKnownRecipe in knownRecipes)
			{
				this.KnownCraftingRecipes[clientKnownRecipe.ItemId] = clientKnownRecipe.Recipe;
			}
			bool isMounted = this.BasicCraftingPanel.IsMounted;
			if (isMounted)
			{
				this.BasicCraftingPanel.BuildItemList();
			}
		}

		// Token: 0x06003EA3 RID: 16035 RVA: 0x000A94F2 File Offset: 0x000A76F2
		private void OnResourceTypesInitialized(Dictionary<string, ClientResourceType> resourceTypes)
		{
			this.ResourceTypes = resourceTypes;
		}

		// Token: 0x06003EA4 RID: 16036 RVA: 0x000A9500 File Offset: 0x000A7700
		private void OnResourceTypesAdded(Dictionary<string, ClientResourceType> resourceTypes)
		{
			foreach (KeyValuePair<string, ClientResourceType> keyValuePair in resourceTypes)
			{
				this.ResourceTypes[keyValuePair.Key] = keyValuePair.Value;
			}
			this.OnSetStacks();
		}

		// Token: 0x06003EA5 RID: 16037 RVA: 0x000A956C File Offset: 0x000A776C
		private void OnResourceTypesRemoved(string[] resourceTypes)
		{
			foreach (string key in resourceTypes)
			{
				this.ResourceTypes.Remove(key);
			}
			this.OnSetStacks();
		}

		// Token: 0x06003EA6 RID: 16038 RVA: 0x000A95A4 File Offset: 0x000A77A4
		public bool IsItemAtPositionDroppable(Point pos)
		{
			return !this.CharacterPanel.Panel.AnchoredRectangle.Contains(pos) && !this.StoragePanel.AnchoredRectangle.Contains(pos) && (this.GetCurrentContextPanel() == null || !this.GetCurrentContextPanel().AnchoredRectangle.Contains(pos)) && (!this.BuilderToolPanel.Visible || !this.BuilderToolPanel.Panel.AnchoredRectangle.Contains(pos)) && (!this.SelectionCommandsPanel.Visible || !this.SelectionCommandsPanel.Panel.AnchoredRectangle.Contains(pos)) && (!this.BlockInfoPanel.Visible || !this.BlockInfoPanel.Panel.AnchoredRectangle.Contains(pos)) && (!this.InGameView.InGame.IsToolsSettingsModalOpened || !this.InGameView.ToolsSettingsPage.ContainPosition(pos));
		}

		// Token: 0x06003EA7 RID: 16039 RVA: 0x000A96B4 File Offset: 0x000A78B4
		private HytaleClient.Interface.InGame.Pages.InventoryPanels.Panel GetCurrentContextPanel()
		{
			bool flag = this.InGameView.InventoryWindow == null;
			HytaleClient.Interface.InGame.Pages.InventoryPanels.Panel result;
			if (flag)
			{
				result = null;
			}
			else
			{
				switch (this.InGameView.InventoryWindow.WindowType)
				{
				case 0:
					result = this.ContainerPanel;
					break;
				case 1:
					result = ((this.IsFieldcraft && this.InGameView.InGame.Instance.GameMode == 1) ? this.ItemLibraryPanel : this.BasicCraftingPanel);
					break;
				case 2:
					result = this.DiagramCraftingPanel;
					break;
				case 3:
					result = this.StructuralCraftingPanel;
					break;
				case 4:
					result = this.ProcessingPanel;
					break;
				default:
					result = null;
					break;
				}
			}
			return result;
		}

		// Token: 0x06003EA8 RID: 16040 RVA: 0x000A9764 File Offset: 0x000A7964
		public void SetupWindows()
		{
			PacketHandler.InventoryWindow inventoryWindow = this.InGameView.InventoryWindow;
			this.ContainerPanel.Visible = false;
			this.BasicCraftingPanel.Visible = false;
			this.StructuralCraftingPanel.Visible = false;
			this.ProcessingPanel.Visible = false;
			this.DiagramCraftingPanel.Visible = false;
			this.ItemLibraryPanel.Visible = false;
			this.BuilderToolPanel.Visible = false;
			this.BlockInfoPanel.Visible = !this.IsFieldcraft;
			this.BlockInfoPanel.Update();
			bool flag = inventoryWindow == null;
			if (!flag)
			{
				switch (inventoryWindow.WindowType)
				{
				case 0:
					this.ContainerPanel.Visible = true;
					this.ContainerPanel.SetupWindow(inventoryWindow);
					break;
				case 1:
				{
					bool flag2 = this.IsFieldcraft && this.InGameView.InGame.Instance.GameMode == 1;
					if (flag2)
					{
						this.ItemLibraryPanel.Visible = true;
					}
					else
					{
						this.BuilderToolPanel.Visible = false;
						this.BasicCraftingPanel.Visible = true;
						this.BasicCraftingPanel.SetupWindow(inventoryWindow);
					}
					break;
				}
				case 2:
					this.DiagramCraftingPanel.Visible = true;
					this.DiagramCraftingPanel.SetupWindow(inventoryWindow);
					break;
				case 3:
					this.StructuralCraftingPanel.Visible = true;
					this.StructuralCraftingPanel.SetupWindow(inventoryWindow);
					break;
				case 4:
					this.ProcessingPanel.Visible = true;
					this.ProcessingPanel.SetupWindow(inventoryWindow);
					break;
				}
				base.Layout(null, true);
			}
		}

		// Token: 0x06003EA9 RID: 16041 RVA: 0x000A991C File Offset: 0x000A7B1C
		protected override void AfterChildrenLayout()
		{
			bool flag = this.InGameView.InventoryWindow == null;
			if (!flag)
			{
				int num = this.Desktop.UnscaleRound((float)this.StoragePanel.Find<Group>("Panel").AnchoredRectangle.Height);
				int num2 = this.Desktop.UnscaleRound((float)this.GetCurrentContextPanel().Find<Group>("Panel").AnchoredRectangle.Height);
				int num3 = num + num2 + this._panelSpacing + this.StoragePanel.Offset - this._hotbarHeight;
				Group group = this.CharacterPanel.Find<Group>("Panel");
				int num4 = this.Desktop.UnscaleRound((float)group.AnchoredRectangle.Height);
				int num5 = Math.Max(num3 - num4 + this._hotbarHeight, this._hotbarHeight);
				int? bottom = group.Anchor.Bottom;
				int num6 = num5;
				bool flag2 = !(bottom.GetValueOrDefault() == num6 & bottom != null);
				if (flag2)
				{
					group.Anchor.Bottom = new int?(num5);
					group.Layout(null, true);
				}
				bool visible = this.BlockInfoPanel.Visible;
				if (visible)
				{
					Group group2 = this.BlockInfoPanel.Find<Group>("Panel");
					int num7 = this.Desktop.UnscaleRound((float)group2.AnchoredRectangle.Height);
					int num8 = num3 + this._hotbarHeight - num7;
					bottom = group2.Anchor.Bottom;
					num6 = num8;
					bool flag3 = !(bottom.GetValueOrDefault() == num6 & bottom != null);
					if (flag3)
					{
						group2.Anchor.Bottom = new int?(num8);
						group2.Layout(null, true);
					}
				}
				bool visible2 = this.BuilderToolPanel.Visible;
				if (visible2)
				{
					Group group3 = this.BuilderToolPanel.Find<Group>("Panel");
					int num9 = this.Desktop.UnscaleRound((float)group3.AnchoredRectangle.Height);
					int num10 = num3 + this._hotbarHeight - num9;
					bottom = group3.Anchor.Bottom;
					num6 = num10;
					bool flag4 = !(bottom.GetValueOrDefault() == num6 & bottom != null);
					if (flag4)
					{
						group3.Anchor.Bottom = new int?(num10);
						group3.Layout(null, true);
					}
				}
				bool visible3 = this.SelectionCommandsPanel.Visible;
				if (visible3)
				{
					Group group4 = this.SelectionCommandsPanel.Find<Group>("Panel");
					int num11 = this.Desktop.UnscaleRound((float)group4.AnchoredRectangle.Height);
					int num12 = num3 + this._hotbarHeight - num11;
					bottom = group4.Anchor.Bottom;
					num6 = num12;
					bool flag5 = !(bottom.GetValueOrDefault() == num6 & bottom != null);
					if (flag5)
					{
						group4.Anchor.Bottom = new int?(num12);
						group4.Layout(null, true);
					}
				}
			}
		}

		// Token: 0x06003EAA RID: 16042 RVA: 0x000A9C38 File Offset: 0x000A7E38
		public void UpdateWindows()
		{
			WindowPanel windowPanel;
			bool flag;
			if (this.InGameView.InventoryWindow != null)
			{
				windowPanel = (this.GetCurrentContextPanel() as WindowPanel);
				flag = (windowPanel != null);
			}
			else
			{
				flag = false;
			}
			bool flag2 = flag;
			if (flag2)
			{
				windowPanel.UpdateWindow(this.InGameView.InventoryWindow);
			}
		}

		// Token: 0x06003EAB RID: 16043 RVA: 0x000A9C80 File Offset: 0x000A7E80
		public void ResetState()
		{
			this.ItemLibraryPanel.ResetState();
			this.BasicCraftingPanel.ResetState();
			this.StructuralCraftingPanel.ResetState();
			this.DiagramCraftingPanel.ResetState();
			this.FieldcraftCategories = null;
			this.KnownCraftingRecipes.Clear();
			this.ResourceTypes.Clear();
		}

		// Token: 0x06003EAC RID: 16044 RVA: 0x000A9CDE File Offset: 0x000A7EDE
		public void SendWindowAction(int windowId, string action, JObject data)
		{
			this.InGameView.InGame.SendSendWindowActionPacket(windowId, action, (data != null) ? data.ToString() : null);
		}

		// Token: 0x04001D70 RID: 7536
		public readonly InGameView InGameView;

		// Token: 0x04001D73 RID: 7539
		public bool IsFieldcraft;

		// Token: 0x04001D74 RID: 7540
		private int _hotbarHeight;

		// Token: 0x04001D75 RID: 7541
		private int _panelSpacing;

		// Token: 0x04001D76 RID: 7542
		public readonly ProcessingPanel ProcessingPanel;

		// Token: 0x04001D77 RID: 7543
		public readonly DiagramCraftingPanel DiagramCraftingPanel;

		// Token: 0x04001D78 RID: 7544
		public readonly ContainerPanel ContainerPanel;

		// Token: 0x04001D79 RID: 7545
		public readonly BasicCraftingPanel BasicCraftingPanel;

		// Token: 0x04001D7A RID: 7546
		public readonly StructuralCraftingPanel StructuralCraftingPanel;

		// Token: 0x04001D7B RID: 7547
		public readonly BlockInfoPanel BlockInfoPanel;

		// Token: 0x04001D7C RID: 7548
		public readonly CharacterPanel CharacterPanel;

		// Token: 0x04001D7D RID: 7549
		public readonly StoragePanel StoragePanel;

		// Token: 0x04001D7E RID: 7550
		public readonly ItemLibraryPanel ItemLibraryPanel;

		// Token: 0x04001D7F RID: 7551
		public readonly RecipeCataloguePopup RecipeCataloguePopup;

		// Token: 0x04001D80 RID: 7552
		public readonly BuilderToolPanel BuilderToolPanel;

		// Token: 0x04001D81 RID: 7553
		public readonly SelectionCommandsPanel SelectionCommandsPanel;

		// Token: 0x04001D82 RID: 7554
		public readonly ItemSlotSelectorPopover ItemSelectorPopover;
	}
}
