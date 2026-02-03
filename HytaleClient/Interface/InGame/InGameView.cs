using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using HytaleClient.Application;
using HytaleClient.Core;
using HytaleClient.Data.Entities;
using HytaleClient.Data.EntityStats;
using HytaleClient.Data.Items;
using HytaleClient.Graphics;
using HytaleClient.InGame.Modules;
using HytaleClient.InGame.Modules.Entities;
using HytaleClient.Interface.InGame.Hud;
using HytaleClient.Interface.InGame.Hud.Abilities;
using HytaleClient.Interface.InGame.Hud.StatusEffects;
using HytaleClient.Interface.InGame.Overlays;
using HytaleClient.Interface.InGame.Pages;
using HytaleClient.Interface.InGame.Pages.InventoryPanels;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Math;
using HytaleClient.Networking;
using HytaleClient.Protocol;
using Newtonsoft.Json.Linq;
using SDL2;

namespace HytaleClient.Interface.InGame
{
	// Token: 0x02000885 RID: 2181
	internal class InGameView : InterfaceComponent
	{
		// Token: 0x17001091 RID: 4241
		// (get) Token: 0x06003DC2 RID: 15810 RVA: 0x000A0FE0 File Offset: 0x0009F1E0
		// (set) Token: 0x06003DC3 RID: 15811 RVA: 0x000A0FE8 File Offset: 0x0009F1E8
		public Dictionary<string, ClientItemBase> Items { get; private set; }

		// Token: 0x17001092 RID: 4242
		// (get) Token: 0x06003DC4 RID: 15812 RVA: 0x000A0FF1 File Offset: 0x0009F1F1
		// (set) Token: 0x06003DC5 RID: 15813 RVA: 0x000A0FF9 File Offset: 0x0009F1F9
		public string Motd { get; private set; }

		// Token: 0x17001093 RID: 4243
		// (get) Token: 0x06003DC6 RID: 15814 RVA: 0x000A1002 File Offset: 0x0009F202
		// (set) Token: 0x06003DC7 RID: 15815 RVA: 0x000A100A File Offset: 0x0009F20A
		public string ServerName { get; private set; }

		// Token: 0x17001094 RID: 4244
		// (get) Token: 0x06003DC8 RID: 15816 RVA: 0x000A1013 File Offset: 0x0009F213
		// (set) Token: 0x06003DC9 RID: 15817 RVA: 0x000A101B File Offset: 0x0009F21B
		public int MaxPlayers { get; private set; }

		// Token: 0x06003DCA RID: 15818 RVA: 0x000A1024 File Offset: 0x0009F224
		public InGameView(Interface @interface) : base(@interface, null)
		{
			this.InGame = this.Interface.App.InGame;
			this._customUIMarkupErrorOverlay = new CustomMarkupErrorOverlay(this);
			this.EntityUIContainer = new EntityUIContainer(this.Desktop, this);
			this.HudContainer = new Group(this.Desktop, this);
			this.PageContainer = new Group(this.Desktop, this);
			this.ToolsSettingsPageContainer = new Group(this.Desktop, this);
			this.CustomHud = new CustomHud(this);
			this.UtilitySlotSelector = new UtilitySlotSelector(this);
			this.ConsumableSlotSelector = new ConsumableSlotSelector(this);
			this.BuilderToolsMaterialSlotSelector = new BuilderToolsMaterialSlotSelector(this);
			this.HotbarComponent = new HotbarComponent(this);
			this.StaminaPanelComponent = new StaminaPanelComponent(this);
			this.AmmoIndicator = new AmmoIndicator(this);
			this.HealthComponent = new HealthComponent(this);
			this.OxygenComponent = new OxygenComponent(this);
			this.NotificationFeedComponent = new NotificationFeedComponent(this);
			this.KillFeedComponent = new KillFeedComponent(this);
			this.ReticleComponent = new ReticleComponent(this);
			this.StatusEffectsHudComponent = new StatusEffectsHudComponent(this);
			this.AbilitiesHudComponent = new AbilitiesHudComponent(this);
			this.InputBindingsComponent = new InputBindingsComponent(this);
			this.CompassComponent = new CompassComponent(this);
			this.BuilderToolsLegend = new BuilderToolsLegend(this);
			this.SpeedometerComponent = new SpeedometerComponent(this);
			this.MovementIndicator = new MovementIndicator(this);
			this.ObjectivePanelComponent = new ObjectivePanelComponent(this);
			this.EventTitleComponent = new EventTitleComponent(this);
			this.ChatComponent = new ChatComponent(this);
			this.PlayerListComponent = new PlayerListComponent(this);
			this.InventoryPage = new InventoryPage(this);
			this.MapPage = new MapPage(this);
			this.ToolsSettingsPage = new ToolsSettingsPage(this, null);
			this.MediaBrowserPage = new MediaBrowserPage(this);
			this.CustomPage = new CustomPage(this);
			this.InGameMenuOverlay = new InGameMenuOverlay(this);
			this.MachinimaEditorOverlay = new MachinimaEditorOverlay(this);
			this.ConfirmQuitToDesktopOverlay = new ConfirmQuitOverlay(this);
			this.ItemQuantityPopup = new ItemQuantityPopup(this);
			this.DebugStressComponent = new DebugStressComponent(this);
			this.Interface.RegisterForEventFromEngine<Dictionary<string, ClientItemBase>>("items.initialized", new Action<Dictionary<string, ClientItemBase>>(this.OnItemsInitialized));
			this.Interface.RegisterForEventFromEngine<Dictionary<string, ClientItemBase>>("items.added", new Action<Dictionary<string, ClientItemBase>>(this.OnItemsAdded));
			this.Interface.RegisterForEventFromEngine<string[]>("items.removed", new Action<string[]>(this.OnItemsRemoved));
			this.Interface.RegisterForEventFromEngine<ClientItemStack[], ClientItemStack[], ClientItemStack[], ClientItemStack[], ClientItemStack[]>("inventory.setAll", new Action<ClientItemStack[], ClientItemStack[], ClientItemStack[], ClientItemStack[], ClientItemStack[]>(this.OnInventorySetAll));
			this.Interface.RegisterForEventFromEngine<PacketHandler.InventoryWindow>("inventory.windows.open", new Action<PacketHandler.InventoryWindow>(this.OnWindowOpen));
			this.Interface.RegisterForEventFromEngine<PacketHandler.InventoryWindow>("inventory.windows.update", new Action<PacketHandler.InventoryWindow>(this.OnWindowUpdate));
			this.Interface.RegisterForEventFromEngine<int>("inventory.windows.close", new Action<int>(this.OnWindowClose));
			this.Interface.RegisterForEventFromEngine<SortType>("inventory.setAutosortType", new Action<SortType>(this.OnSetAutosortType));
			this.Interface.RegisterForEventFromEngine<int>("game.setActiveHotbarSlot", new Action<int>(this.OnSetActiveHotbarSlot));
			this.Interface.RegisterForEventFromEngine<int>("game.setActiveUtilitySlot", new Action<int>(this.OnSetActiveUtilitySlot));
			this.Interface.RegisterForEventFromEngine<int>("game.setActiveConsumableSlot", new Action<int>(this.OnSetActiveConsumableSlot));
			this.Interface.RegisterForEventFromEngine<int>("game.setActiveToolsSlot", new Action<int>(this.OnSetActiveToolsSlot));
			this.Interface.RegisterForEventFromEngine<int, ClientEntityStatValue, float?>("game.entityStats.set", new Action<int, ClientEntityStatValue, float?>(this.OnStatChanged));
			this.Interface.RegisterForEventFromEngine("inventory.dropItemBindingDown", new Action(this.OnDropItemBindingDown));
			this.Interface.RegisterForEventFromEngine("inventory.dropItemBindingUp", new Action(this.OnDropItemBindingUp));
			this.Interface.RegisterForEventFromEngine<string[]>("assets.updated", new Action<string[]>(this.OnAssetsUpdated));
			this.Interface.RegisterForEventFromEngine<HudComponent[]>("hud.visibleComponentsUpdated", new Action<HudComponent[]>(this.OnVisibleHudComponentsUpdated));
			this.Interface.RegisterForEventFromEngine("settings.inputBindingsUpdated", new Action(this.InputBindingsUpdated));
		}

		// Token: 0x06003DCB RID: 15819 RVA: 0x000A1474 File Offset: 0x0009F674
		public void Build()
		{
			Document document;
			this.Interface.TryGetDocument("InGame/Common.ui", out document);
			this.DefaultItemSlotsPerRow = document.ResolveNamedValue<int>(this.Interface, "DefaultItemSlotsPerRow");
			this.DefaultItemGridStyle = document.ResolveNamedValue<ItemGrid.ItemGridStyle>(this.Interface, "DefaultItemGridStyle");
			this._cursorFloatingItem = new FloatingItemComponent(this, null);
			this.SetupCursorFloatingItem();
			this.EntityUIContainer.Build();
			this.CustomHud.Build();
			this.UtilitySlotSelector.Build();
			this.ConsumableSlotSelector.Build();
			this.BuilderToolsMaterialSlotSelector.Build();
			this.HotbarComponent.Build();
			this.StaminaPanelComponent.Build();
			this.AmmoIndicator.Build();
			this.HealthComponent.Build();
			this.OxygenComponent.Build();
			this.NotificationFeedComponent.Build();
			this.KillFeedComponent.Build();
			this.ReticleComponent.Build();
			this.StatusEffectsHudComponent.Build();
			this.AbilitiesHudComponent.Build();
			this.InputBindingsComponent.Build();
			this.ObjectivePanelComponent.Build();
			this.EventTitleComponent.Build();
			this.ChatComponent.Build();
			this.PlayerListComponent.Build();
			this.BuilderToolsLegend.Build();
			this.SpeedometerComponent.Build();
			this.MovementIndicator.Build();
			this.UpdateHudWidgetVisibility(false);
			this.InGameMenuOverlay.Build();
			this.MachinimaEditorOverlay.Build();
			this.ConfirmQuitToDesktopOverlay.Build();
			this.InventoryPage.Build();
			this.MapPage.Build();
			this.ToolsSettingsPage.Build();
			this.MediaBrowserPage.Build();
			this.CustomPage.Build();
			this.ItemQuantityPopup.Build();
			this.DebugStressComponent.Visible = false;
		}

		// Token: 0x06003DCC RID: 15820 RVA: 0x000A1670 File Offset: 0x0009F870
		protected override void OnMounted()
		{
			bool hasCustomUIMarkupError = this._hasCustomUIMarkupError;
			if (hasCustomUIMarkupError)
			{
				this.Desktop.SetLayer(1, this._customUIMarkupErrorOverlay);
			}
			bool flag = this.InGame.Instance.GameMode != 1;
			if (!flag)
			{
				this.BuilderToolsLegend.UpdateInputHints(true, false);
				bool flag2 = !this.InGame.Instance.BuilderToolsModule.TrySelectHotbarActiveTool();
				if (!flag2)
				{
					this.UpdateBuilderToolsLegendVisibility(false);
				}
			}
		}

		// Token: 0x06003DCD RID: 15821 RVA: 0x000A16EC File Offset: 0x0009F8EC
		public void DisplayMarkupError(string message, TextParserSpan span)
		{
			this._hasCustomUIMarkupError = true;
			this._customUIMarkupErrorOverlay.Setup(message, span);
			bool isMounted = base.IsMounted;
			if (isMounted)
			{
				bool isMounted2 = this._customUIMarkupErrorOverlay.IsMounted;
				if (isMounted2)
				{
					this._customUIMarkupErrorOverlay.Layout(null, true);
				}
				else
				{
					this.Desktop.SetLayer(1, this._customUIMarkupErrorOverlay);
				}
			}
		}

		// Token: 0x06003DCE RID: 15822 RVA: 0x000A1758 File Offset: 0x0009F958
		public void ClearMarkupError()
		{
			bool flag = !this._hasCustomUIMarkupError;
			if (!flag)
			{
				this._hasCustomUIMarkupError = false;
				this._customUIMarkupErrorOverlay.Clear();
				bool isMounted = this._customUIMarkupErrorOverlay.IsMounted;
				if (isMounted)
				{
					this.Desktop.ClearLayer(1);
				}
			}
		}

		// Token: 0x06003DCF RID: 15823 RVA: 0x000A17A4 File Offset: 0x0009F9A4
		protected override void OnUnmounted()
		{
			Debug.Assert(this._mountedAssetTextureAreasByLocalPath.Count == 0);
			this.Interface.InGameCustomUIProvider.DisposeTextures();
			Texture itemIconAtlasTexture = this.ItemIconAtlasTexture;
			if (itemIconAtlasTexture != null)
			{
				itemIconAtlasTexture.Dispose();
			}
			this.ItemIconAtlasTexture = null;
			bool isDropItemBindingDown = this._isDropItemBindingDown;
			if (isDropItemBindingDown)
			{
				this._isDropItemBindingDown = false;
				this.Desktop.UnregisterAnimationCallback(new Action<float>(this.CheckForItemsToDrop));
			}
		}

		// Token: 0x06003DD0 RID: 15824 RVA: 0x000A181C File Offset: 0x0009FA1C
		public void RegisterEntityUIDrawTasks(ref Matrix transformationMatrix, Entity entity, float distanceToCamera)
		{
			this.EntityUIContainer.RegisterDrawTasksForEntity(ref transformationMatrix, entity, distanceToCamera);
		}

		// Token: 0x06003DD1 RID: 15825 RVA: 0x000A1830 File Offset: 0x0009FA30
		protected internal override void OnKeyDown(SDL.SDL_Keycode keycode, int repeat)
		{
			base.OnKeyDown(keycode, repeat);
			bool flag = this.InGame.CurrentOverlay > AppInGame.InGameOverlay.None;
			if (!flag)
			{
				Page currentPage = this.InGame.CurrentPage;
				Page page = currentPage;
				if (page - 1 > 1)
				{
					if (page == 6)
					{
						this.MapPage.OnKeyDown(keycode, repeat);
					}
				}
				else
				{
					this.InventoryPage.OnKeyDown(keycode, repeat);
				}
			}
		}

		// Token: 0x06003DD2 RID: 15826 RVA: 0x000A1898 File Offset: 0x0009FA98
		protected internal override void OnKeyUp(SDL.SDL_Keycode keycode)
		{
			bool flag = this.InGame.CurrentOverlay > AppInGame.InGameOverlay.None;
			if (!flag)
			{
				bool flag2 = this.InGame.CurrentPage == 6;
				if (flag2)
				{
					this.MapPage.OnKeyUp(keycode);
				}
			}
		}

		// Token: 0x06003DD3 RID: 15827 RVA: 0x000A18DC File Offset: 0x0009FADC
		public void OnCharacterControllerTicked(ClientMovementStates movementStates)
		{
			this.MovementIndicator.Update(movementStates);
		}

		// Token: 0x06003DD4 RID: 15828 RVA: 0x000A18EC File Offset: 0x0009FAEC
		protected internal override void Dismiss()
		{
			bool isOpen = this.InGame.Instance.Chat.IsOpen;
			if (isOpen)
			{
				this.InGame.Instance.Chat.Close();
			}
			else
			{
				bool flag = this.InGame.CurrentOverlay == AppInGame.InGameOverlay.None && this.InGame.CurrentPage == null && !this.PlayerListComponent.Visible && !this.InGame.IsToolsSettingsModalOpened;
				if (flag)
				{
					this.InGame.SetCurrentOverlay(AppInGame.InGameOverlay.InGameMenu);
				}
				else
				{
					this.InGame.TryClosePageOrOverlay();
					this.InGame.CloseToolsSettingsModal();
				}
			}
		}

		// Token: 0x06003DD5 RID: 15829 RVA: 0x000A1990 File Offset: 0x0009FB90
		public void OnReset(bool isStayingConnected)
		{
			bool hasMarkupError = this.Interface.HasMarkupError;
			if (!hasMarkupError)
			{
				foreach (TextureArea textureArea in this._mountedAssetTextureAreasByLocalPath.Values)
				{
					textureArea.Texture.Dispose();
				}
				this._mountedAssetTextureAreasByLocalPath.Clear();
				this._mountedTextureAssetPaths.Clear();
				Texture itemIconAtlasTexture = this.ItemIconAtlasTexture;
				if (itemIconAtlasTexture != null)
				{
					itemIconAtlasTexture.Dispose();
				}
				this.ItemIconAtlasTexture = null;
				this.PageContainer.Clear();
				this.CustomHud.ResetState();
				this.UtilitySlotSelector.ResetState();
				this.ConsumableSlotSelector.ResetState();
				this.BuilderToolsMaterialSlotSelector.ResetState();
				this.ChatComponent.ResetState();
				this.KillFeedComponent.ResetState();
				this.StaminaPanelComponent.ResetState();
				this.AmmoIndicator.ResetState();
				this.HealthComponent.ResetState();
				this.OxygenComponent.ResetState();
				this.NotificationFeedComponent.ResetState();
				this.ReticleComponent.ResetState(isStayingConnected);
				this.InputBindingsComponent.ResetState();
				this.CompassComponent.ResetState();
				this.ObjectivePanelComponent.ResetState();
				this.EventTitleComponent.ResetState();
				this.HotbarComponent.ResetState();
				this.ChatComponent.ResetState();
				this.PlayerListComponent.ResetState();
				this.BuilderToolsLegend.ResetState();
				this.Motd = null;
				this.ServerName = null;
				this.MaxPlayers = 0;
				this.Players.Clear();
				bool isMounted = this.PlayerListComponent.IsMounted;
				if (isMounted)
				{
					this.PlayerListComponent.UpdateList();
					this.PlayerListComponent.UpdateServerDetails();
				}
				this._visibleHudComponents = new HudComponent[0];
				this.UpdateHudWidgetVisibility(false);
				this.Items = null;
				this.NextPreviewId = 100;
				this.HoveredItemSlot = null;
				this.StorageStacks = null;
				this.ArmorStacks = null;
				this.HotbarStacks = null;
				this.UtilityStacks = null;
				this.ConsumableStacks = null;
				this.InventoryWindow = null;
				bool flag = this.Desktop.IsMouseDragging && this.ItemDragData != null;
				if (flag)
				{
					this.Desktop.CancelMouseDrag();
				}
				this.SetupDragAndDropItem(null);
				this.InventoryPage.ResetState();
				this.MediaBrowserPage.ResetState();
				this.CustomPage.ResetState();
				bool flag2 = this.Desktop.GetLayer(3) != null;
				if (flag2)
				{
					this.Desktop.ClearLayer(3);
				}
				bool flag3 = base.IsMounted && isStayingConnected;
				if (flag3)
				{
					base.Layout(null, true);
				}
			}
		}

		// Token: 0x06003DD6 RID: 15830 RVA: 0x000A1C74 File Offset: 0x0009FE74
		public void PlayPageCloseSound(Page closedPage)
		{
			if (closedPage != 2)
			{
				if (closedPage == 6)
				{
					this.Interface.PlaySound(this.MapPage.CloseSound);
				}
			}
			else
			{
				this.Interface.PlaySound(this.InventoryPage.CloseSound);
			}
		}

		// Token: 0x06003DD7 RID: 15831 RVA: 0x000A1CC4 File Offset: 0x0009FEC4
		public void PlayPageOpenSound(Page openedPage)
		{
			if (openedPage != 2)
			{
				if (openedPage == 6)
				{
					this.Interface.PlaySound(this.MapPage.OpenSound);
				}
			}
			else
			{
				this.Interface.PlaySound(this.InventoryPage.OpenSound);
			}
		}

		// Token: 0x06003DD8 RID: 15832 RVA: 0x000A1D14 File Offset: 0x0009FF14
		public void CloseToolsSettingsModal()
		{
			this.ToolsSettingsPageContainer.Clear();
			this.ReparentChat(false);
			this.UpdateHudWidgetVisibility(true);
			this.InputBindingsComponent.UpdateBindings(true);
			this.HotbarComponent.OnPageChanged();
			base.Layout(null, true);
		}

		// Token: 0x06003DD9 RID: 15833 RVA: 0x000A1D68 File Offset: 0x0009FF68
		public void OpenToolsSettingsPage()
		{
			this.ToolsSettingsPageContainer.Clear();
			this.ToolsSettingsPageContainer.Add(this.ToolsSettingsPage, -1);
			this.ReparentChat(false);
			this.UpdateHudWidgetVisibility(true);
			this.InputBindingsComponent.UpdateBindings(true);
			this.HotbarComponent.OnPageChanged();
			this.InventoryPage.SelectionCommandsPanel.Visible = false;
			this.InventoryPage.Layout(null, true);
			this.InventoryPage.BuilderToolPanel.Visible = false;
			this.InventoryPage.BuilderToolPanel.Layout(null, true);
			base.Layout(null, true);
		}

		// Token: 0x06003DDA RID: 15834 RVA: 0x000A1E24 File Offset: 0x000A0024
		public void OnPageChanged()
		{
			bool flag = this.InventoryWindow != null;
			if (flag)
			{
				this.CloseAllInventoryWindows();
			}
			InterfaceComponent child = null;
			switch (this.InGame.CurrentPage)
			{
			case 1:
				child = this.InventoryPage;
				break;
			case 2:
				child = this.InventoryPage;
				break;
			case 3:
				child = this.MediaBrowserPage;
				break;
			case 5:
				child = this.ToolsSettingsPage;
				break;
			case 6:
				child = this.MapPage;
				break;
			case 7:
				child = this.CustomPage;
				break;
			}
			this.PageContainer.Clear();
			bool flag2 = this.InGame.CurrentPage > 0;
			if (flag2)
			{
				this.PageContainer.Add(child, -1);
			}
			this.ReparentChat(false);
			bool flag3 = this.InGame.CurrentPage == 2;
			if (flag3)
			{
				this.InGame.SendOpenInventoryPacket();
				PacketHandler.InventoryWindow inventoryWindow = new PacketHandler.InventoryWindow();
				inventoryWindow.Id = 0;
				inventoryWindow.WindowType = 1;
				JObject jobject = new JObject();
				jobject.Add("id", "Fieldcraft");
				jobject.Add("name", "ui.windows.fieldcraft");
				jobject.Add("type", 0);
				inventoryWindow.WindowData = jobject;
				this.InventoryWindow = inventoryWindow;
				this.InventoryPage.IsFieldcraft = true;
				this.InventoryPage.SetupWindows();
			}
			else
			{
				bool flag4 = this.InGame.CurrentPage == 7;
				if (flag4)
				{
					this.Desktop.FocusElement(this.CustomPage, true);
				}
			}
			bool isMounted = this.PlayerListComponent.IsMounted;
			if (isMounted)
			{
				this.InGame.SetPlayerListVisible(false);
			}
			bool flag5 = this.InGame.Instance.GameMode == 1 && (this.InGame.CurrentPage == null || this.InGame.CurrentPage == 2);
			if (flag5)
			{
				this.InGame.Instance.BuilderToolsModule.TrySelectActiveTool();
			}
			this.UpdateHudWidgetVisibility(true);
			this.InputBindingsComponent.UpdateBindings(true);
			this.HotbarComponent.OnPageChanged();
			base.Layout(null, true);
		}

		// Token: 0x06003DDB RID: 15835 RVA: 0x000A2058 File Offset: 0x000A0258
		public void OnOverlayChanged()
		{
			Element layer = null;
			switch (this.InGame.CurrentOverlay)
			{
			case AppInGame.InGameOverlay.InGameMenu:
				layer = this.InGameMenuOverlay;
				break;
			case AppInGame.InGameOverlay.MachinimaEditor:
				layer = this.MachinimaEditorOverlay;
				break;
			case AppInGame.InGameOverlay.ConfirmQuit:
				layer = this.ConfirmQuitToDesktopOverlay;
				break;
			}
			bool flag = this.Desktop.GetLayer(3) != null;
			if (flag)
			{
				this.Desktop.ClearLayer(3);
			}
			bool flag2 = this.InGame.CurrentOverlay > AppInGame.InGameOverlay.None;
			if (flag2)
			{
				this.Desktop.SetLayer(3, layer);
			}
			else
			{
				bool flag3 = this.InGame.CurrentPage == 7;
				if (flag3)
				{
					this.Desktop.FocusElement(this.CustomPage, true);
				}
			}
			this.UpdateReticleVisibility(true);
			this.UpdateBuilderToolsLegendVisibility(true);
			bool isMounted = this.PlayerListComponent.IsMounted;
			if (isMounted)
			{
				this.InGame.SetPlayerListVisible(false);
			}
		}

		// Token: 0x17001095 RID: 4245
		// (get) Token: 0x06003DDC RID: 15836 RVA: 0x000A2140 File Offset: 0x000A0340
		public bool HasFocusedElement
		{
			get
			{
				return this.Desktop.FocusedElement != null && (this.Desktop.FocusedElement != this.CustomPage || this.CustomPage.HasPageDesktopFocusedElement);
			}
		}

		// Token: 0x06003DDD RID: 15837 RVA: 0x000A2174 File Offset: 0x000A0374
		public void TryClosePageOrOverlayWithInputBinding()
		{
			bool flag = !this.HasFocusedElement;
			if (flag)
			{
				this.InGame.TryClosePageOrOverlay();
			}
		}

		// Token: 0x06003DDE RID: 15838 RVA: 0x000A219B File Offset: 0x000A039B
		private void OnVisibleHudComponentsUpdated(HudComponent[] components)
		{
			this._visibleHudComponents = components;
			this.UpdateHudWidgetVisibility(true);
		}

		// Token: 0x06003DDF RID: 15839 RVA: 0x000A21B0 File Offset: 0x000A03B0
		private void InputBindingsUpdated()
		{
			bool flag = !base.IsMounted;
			if (!flag)
			{
				this.BuilderToolsLegend.UpdateInputHints(true, true);
			}
		}

		// Token: 0x06003DE0 RID: 15840 RVA: 0x000A21DC File Offset: 0x000A03DC
		public void UpdateBuilderToolsLegendVisibility(bool doLayout = false)
		{
			this.BuilderToolsLegend.Visible = (Enumerable.Contains<HudComponent>(this._visibleHudComponents, 12) && this.InGame.IsHudVisible && this.Interface.App.Stage == App.AppStage.InGame && this.InGame.Instance.GameMode == 1 && this.InGame.Instance.BuilderToolsModule.HasActiveBrush && this.Interface.App.Settings.BuilderToolsSettings.DisplayLegend && this.InGame.CurrentPage == null && this.InGame.CurrentOverlay == AppInGame.InGameOverlay.None);
			bool flag = this.BuilderToolsLegend.IsMounted && doLayout;
			if (flag)
			{
				this.BuilderToolsLegend.Layout(new Rectangle?(this.HudContainer.RectangleAfterPadding), true);
			}
		}

		// Token: 0x06003DE1 RID: 15841 RVA: 0x000A22BC File Offset: 0x000A04BC
		public void UpdateSpeedometerVisibility(bool doLayout = false)
		{
			this.SpeedometerComponent.Visible = (Enumerable.Contains<HudComponent>(this._visibleHudComponents, 13) && this.InGame.IsHudVisible && this.Interface.App.Stage == App.AppStage.InGame && this.InGame.CurrentPage == null && this.InGame.CurrentOverlay == AppInGame.InGameOverlay.None && this.SpeedometerComponent.Enabled);
			bool flag = this.SpeedometerComponent.IsMounted && doLayout;
			if (flag)
			{
				this.SpeedometerComponent.Layout(new Rectangle?(this.HudContainer.RectangleAfterPadding), true);
			}
		}

		// Token: 0x06003DE2 RID: 15842 RVA: 0x000A2360 File Offset: 0x000A0560
		public void UpdateMovementIndicatorVisibility(bool doLayout = false)
		{
			this.MovementIndicator.Visible = (Enumerable.Contains<HudComponent>(this._visibleHudComponents, 14) && this.InGame.IsHudVisible && this.InGame.CurrentPage == null && this.InGame.CurrentOverlay == AppInGame.InGameOverlay.None);
			bool flag = this.MovementIndicator.IsMounted && doLayout;
			if (flag)
			{
				this.MovementIndicator.Layout(new Rectangle?(this.HudContainer.RectangleAfterPadding), true);
			}
		}

		// Token: 0x06003DE3 RID: 15843 RVA: 0x000A23E4 File Offset: 0x000A05E4
		public void UpdateReticleVisibility(bool doLayout = false)
		{
			this.ReticleComponent.Visible = (Enumerable.Contains<HudComponent>(this._visibleHudComponents, 2) && this.InGame.IsHudVisible && this.ReticleComponent.IsReticleVisible && this.InGame.CurrentPage == null && this.InGame.CurrentOverlay == AppInGame.InGameOverlay.None);
			bool flag = this.ReticleComponent.IsMounted && doLayout;
			if (flag)
			{
				this.ReticleComponent.Layout(new Rectangle?(this.HudContainer.RectangleAfterPadding), true);
			}
		}

		// Token: 0x06003DE4 RID: 15844 RVA: 0x000A2474 File Offset: 0x000A0674
		public void OnPlayerListVisibilityChanged()
		{
			this.UpdatePlayerListVisibility(true);
		}

		// Token: 0x06003DE5 RID: 15845 RVA: 0x000A2480 File Offset: 0x000A0680
		public void UpdatePlayerListVisibility(bool doLayout = false)
		{
			this.PlayerListComponent.Visible = (Enumerable.Contains<HudComponent>(this._visibleHudComponents, 8) && this.InGame.IsHudVisible && this.InGame.IsPlayerListVisible && this.InGame.CurrentPage == null && this.InGame.CurrentOverlay == AppInGame.InGameOverlay.None);
			bool flag = this.PlayerListComponent.IsMounted && doLayout;
			if (flag)
			{
				this.PlayerListComponent.Layout(new Rectangle?(this.HudContainer.RectangleAfterPadding), true);
			}
		}

		// Token: 0x06003DE6 RID: 15846 RVA: 0x000A2510 File Offset: 0x000A0710
		public void UpdateUtilitySlotSelectorVisibility(bool doLayout = false)
		{
			this.UtilitySlotSelector.Visible = (Enumerable.Contains<HudComponent>(this._visibleHudComponents, 15) && this.InGame.IsHudVisible && this.InGame.ActiveItemSelector == AppInGame.ItemSelector.Utility);
			bool flag = this.UtilitySlotSelector.IsMounted && doLayout;
			if (flag)
			{
				this.UtilitySlotSelector.Layout(new Rectangle?(this.HudContainer.RectangleAfterPadding), true);
			}
		}

		// Token: 0x06003DE7 RID: 15847 RVA: 0x000A2588 File Offset: 0x000A0788
		public void UpdateConsumableSlotSelectorVisibility(bool doLayout = false)
		{
			this.ConsumableSlotSelector.Visible = (Enumerable.Contains<HudComponent>(this._visibleHudComponents, 16) && this.InGame.IsHudVisible && this.InGame.ActiveItemSelector == AppInGame.ItemSelector.Consumable);
			bool flag = this.ConsumableSlotSelector.IsMounted && doLayout;
			if (flag)
			{
				this.ConsumableSlotSelector.Layout(new Rectangle?(this.HudContainer.RectangleAfterPadding), true);
			}
		}

		// Token: 0x06003DE8 RID: 15848 RVA: 0x000A2600 File Offset: 0x000A0800
		public void UpdateBuilderToolsMaterialSlotSelectorVisibility(bool doLayout = false)
		{
			this.BuilderToolsMaterialSlotSelector.Visible = (Enumerable.Contains<HudComponent>(this._visibleHudComponents, 17) && this.InGame.IsHudVisible && this.InGame.ActiveItemSelector == AppInGame.ItemSelector.BuilderToolsMaterial);
			bool flag = this.BuilderToolsMaterialSlotSelector.IsMounted && doLayout;
			if (flag)
			{
				this.BuilderToolsMaterialSlotSelector.Layout(new Rectangle?(this.HudContainer.RectangleAfterPadding), true);
			}
		}

		// Token: 0x06003DE9 RID: 15849 RVA: 0x000A2678 File Offset: 0x000A0878
		public void UpdateObjectivePanelVisibility(bool doLayout = false)
		{
			this.ObjectivePanelComponent.Visible = (Enumerable.Contains<HudComponent>(this._visibleHudComponents, 11) && this.InGame.IsHudVisible && this.ObjectivePanelComponent.HasObjectives);
			bool flag = this.ObjectivePanelComponent.IsMounted && doLayout;
			if (flag)
			{
				this.ObjectivePanelComponent.Layout(new Rectangle?(this.HudContainer.RectangleAfterPadding), true);
			}
		}

		// Token: 0x06003DEA RID: 15850 RVA: 0x000A26EC File Offset: 0x000A08EC
		public void UpdateStaminaPanelVisibility(bool doLayout = false)
		{
			this.StaminaPanelComponent.Visible = (Enumerable.Contains<HudComponent>(this._visibleHudComponents, 18) && this.InGame.IsHudVisible && this.StaminaPanelComponent.ShouldDisplay);
			bool flag = this.StaminaPanelComponent.IsMounted && doLayout;
			if (flag)
			{
				this.StaminaPanelComponent.Layout(new Rectangle?(this.HudContainer.RectangleAfterPadding), true);
			}
		}

		// Token: 0x06003DEB RID: 15851 RVA: 0x000A2760 File Offset: 0x000A0960
		public void UpdateAmmoIndicatorVisibility(bool doLayout = false)
		{
			this.AmmoIndicator.Visible = (Enumerable.Contains<HudComponent>(this._visibleHudComponents, 19) && this.InGame.IsHudVisible && this.InGame.Instance != null && this.InGame.Instance.GameMode == null && this.InGame.Instance.LocalPlayer != null && this.InGame.Instance.LocalPlayer.ShouldDisplayHudForEntityStat(DefaultEntityStats.Ammo));
			bool flag = this.AmmoIndicator.IsMounted && doLayout;
			if (flag)
			{
				this.AmmoIndicator.Layout(new Rectangle?(this.HudContainer.RectangleAfterPadding), true);
			}
		}

		// Token: 0x06003DEC RID: 15852 RVA: 0x000A2814 File Offset: 0x000A0A14
		public void UpdateHealthVisibility(bool doLayout = false)
		{
			this.HealthComponent.Visible = (Enumerable.Contains<HudComponent>(this._visibleHudComponents, 20) && this.InGame.IsHudVisible && this.InGame.Instance != null && this.InGame.Instance.GameMode == 0);
			bool flag = this.HealthComponent.IsMounted && doLayout;
			if (flag)
			{
				this.HealthComponent.Layout(new Rectangle?(this.HudContainer.RectangleAfterPadding), true);
			}
		}

		// Token: 0x06003DED RID: 15853 RVA: 0x000A28A0 File Offset: 0x000A0AA0
		public void UpdateOxygenVisibility(bool doLayout = false)
		{
			this.OxygenComponent.Visible = (Enumerable.Contains<HudComponent>(this._visibleHudComponents, 21) && this.InGame.IsHudVisible && this.InGame.Instance != null && this.InGame.Instance.GameMode == null && this.OxygenComponent.Display);
			bool flag = this.OxygenComponent.IsMounted && doLayout;
			if (flag)
			{
				this.OxygenComponent.Layout(new Rectangle?(this.HudContainer.RectangleAfterPadding), true);
			}
		}

		// Token: 0x06003DEE RID: 15854 RVA: 0x000A2934 File Offset: 0x000A0B34
		public void UpdateStatusEffectHudVisibility(bool doLayout = false)
		{
			this.StatusEffectsHudComponent.Visible = (this.InGame.IsHudVisible && this.InGame.Instance != null && this.InGame.Instance.GameMode == 0);
			bool flag = this.StatusEffectsHudComponent.IsMounted && doLayout;
			if (flag)
			{
				this.StatusEffectsHudComponent.Layout(new Rectangle?(this.HudContainer.RectangleAfterPadding), true);
			}
		}

		// Token: 0x06003DEF RID: 15855 RVA: 0x000A29B0 File Offset: 0x000A0BB0
		public void UpdateAbilitiesHudVisibility(bool doLayout = false)
		{
			this.AbilitiesHudComponent.Visible = (this.InGame.IsHudVisible && this.InGame.Instance != null && this.InGame.Instance.GameMode == 0);
			bool flag = this.AbilitiesHudComponent.IsMounted && doLayout;
			if (flag)
			{
				this.AbilitiesHudComponent.Layout(new Rectangle?(this.HudContainer.RectangleAfterPadding), true);
			}
		}

		// Token: 0x06003DF0 RID: 15856 RVA: 0x000A2A2C File Offset: 0x000A0C2C
		private void UpdateHudWidgetVisibility(bool doLayout = true)
		{
			bool isHudVisible = this.InGame.IsHudVisible;
			this.EntityUIContainer.Visible = isHudVisible;
			this.HotbarComponent.Visible = (this.HotbarComponent.Parent != this.HudContainer || (Enumerable.Contains<HudComponent>(this._visibleHudComponents, 0) && isHudVisible));
			this.UpdateUtilitySlotSelectorVisibility(false);
			this.UpdateConsumableSlotSelectorVisibility(false);
			this.UpdateBuilderToolsMaterialSlotSelectorVisibility(false);
			this.NotificationFeedComponent.Visible = (Enumerable.Contains<HudComponent>(this._visibleHudComponents, 5) && isHudVisible);
			this.KillFeedComponent.Visible = (Enumerable.Contains<HudComponent>(this._visibleHudComponents, 6) && isHudVisible);
			this.InputBindingsComponent.Visible = (Enumerable.Contains<HudComponent>(this._visibleHudComponents, 7) && isHudVisible);
			this.CompassComponent.Visible = (Enumerable.Contains<HudComponent>(this._visibleHudComponents, 10) && isHudVisible);
			this.UpdateReticleVisibility(false);
			this.UpdateBuilderToolsLegendVisibility(false);
			this.UpdateSpeedometerVisibility(false);
			this.UpdateMovementIndicatorVisibility(false);
			this.UpdateObjectivePanelVisibility(false);
			this.UpdatePlayerListVisibility(false);
			this.UpdateStaminaPanelVisibility(false);
			this.UpdateAmmoIndicatorVisibility(false);
			this.UpdateHealthVisibility(false);
			this.UpdateOxygenVisibility(false);
			this.UpdateStatusEffectHudVisibility(false);
			this.UpdateAbilitiesHudVisibility(false);
			this.ChatComponent.Visible = Enumerable.Contains<HudComponent>(this._visibleHudComponents, 3);
			bool flag = this.EntityUIContainer.IsMounted && doLayout;
			if (flag)
			{
				this.EntityUIContainer.Layout(new Rectangle?(this._rectangleAfterPadding), true);
			}
			bool flag2 = this.HudContainer.IsMounted && doLayout;
			if (flag2)
			{
				this.HudContainer.Layout(new Rectangle?(this._rectangleAfterPadding), true);
			}
			bool flag3 = this.HotbarComponent.IsMounted && this.HotbarComponent.Parent != this.HudContainer;
			if (flag3)
			{
				this.HotbarComponent.Layout(new Rectangle?(this.HotbarComponent.Parent.RectangleAfterPadding), true);
			}
			bool flag4 = this.InputBindingsComponent.IsMounted && this.InputBindingsComponent.Parent != this.HudContainer;
			if (flag4)
			{
				this.InputBindingsComponent.Layout(new Rectangle?(this.HotbarComponent.Parent.RectangleAfterPadding), true);
			}
		}

		// Token: 0x06003DF1 RID: 15857 RVA: 0x000A2C68 File Offset: 0x000A0E68
		public void UpdateDebugStressVisibility()
		{
			this.DebugStressComponent.Visible = !this.DebugStressComponent.Visible;
			this.UpdateHudWidgetVisibility(true);
		}

		// Token: 0x06003DF2 RID: 15858 RVA: 0x000A2C8D File Offset: 0x000A0E8D
		public void OnActiveItemSelectorChanged()
		{
			this.UpdateUtilitySlotSelectorVisibility(true);
			this.UpdateConsumableSlotSelectorVisibility(true);
			this.UpdateBuilderToolsMaterialSlotSelectorVisibility(true);
			this.HotbarComponent.OnToggleItemSlotSelector();
		}

		// Token: 0x06003DF3 RID: 15859 RVA: 0x000A2CB4 File Offset: 0x000A0EB4
		public void OnActiveBuilderToolSelected(bool hasActiveBrush, int? favoriteMaterialsCount)
		{
			this.HighlightSlot(-1, this.InGame.Instance.InventoryModule.HotbarActiveSlot);
			bool flag;
			if (hasActiveBrush)
			{
				int? num = favoriteMaterialsCount;
				int num2 = 0;
				flag = (num.GetValueOrDefault() > num2 & num != null);
			}
			else
			{
				flag = false;
			}
			bool flag2 = flag;
			if (flag2)
			{
				this.BuilderToolsMaterialSlotSelector.SetItemStacks(this.InGame.Instance.BuilderToolsModule.ActiveTool.BrushData.GetFavoriteMaterialStacks());
			}
		}

		// Token: 0x06003DF4 RID: 15860 RVA: 0x000A2D2C File Offset: 0x000A0F2C
		public void OnHudVisibilityChanged()
		{
			this.UpdateHudWidgetVisibility(true);
			this.ChatComponent.OnHudVisibilityChanged();
		}

		// Token: 0x06003DF5 RID: 15861 RVA: 0x000A2D43 File Offset: 0x000A0F43
		public void OnChatOpened(SDL.SDL_Keycode? keyCodeTrigger, bool isCommand)
		{
			this.ReparentChat(true);
			this.ChatComponent.OnOpened(keyCodeTrigger, isCommand);
		}

		// Token: 0x06003DF6 RID: 15862 RVA: 0x000A2D5C File Offset: 0x000A0F5C
		public void OnChatClosed()
		{
			this.ReparentChat(true);
			this.ChatComponent.OnClosed();
			bool flag = this.InGame.CurrentPage == 7;
			if (flag)
			{
				this.Desktop.FocusElement(this.CustomPage, true);
			}
		}

		// Token: 0x06003DF7 RID: 15863 RVA: 0x000A2DA4 File Offset: 0x000A0FA4
		private void ReparentChat(bool doLayout = true)
		{
			bool flag = this.InGame.CurrentPage == null || !this.ChatComponent.IsOpen();
			if (flag)
			{
				bool flag2 = this.ChatComponent.Parent != this.HudContainer;
				if (flag2)
				{
					Element parent = this.ChatComponent.Parent;
					if (parent != null)
					{
						parent.Remove(this.ChatComponent);
					}
					this.HudContainer.Add(this.ChatComponent, this.PlayerListComponent);
					if (doLayout)
					{
						this.HudContainer.Layout(null, true);
					}
				}
			}
			else
			{
				bool flag3 = this.ChatComponent.Parent != this.PageContainer;
				if (flag3)
				{
					Element parent2 = this.ChatComponent.Parent;
					if (parent2 != null)
					{
						parent2.Remove(this.ChatComponent);
					}
					this.PageContainer.Add(this.ChatComponent, -1);
					if (doLayout)
					{
						this.PageContainer.Layout(null, true);
					}
				}
			}
		}

		// Token: 0x06003DF8 RID: 15864 RVA: 0x000A2EB4 File Offset: 0x000A10B4
		public void OnGameModeChanged()
		{
			this.InventoryPage.SetupWindows();
			this.HotbarComponent.UpdateBackgroundVisibility();
			this.UpdateBuilderToolsLegendVisibility(true);
			this.UpdateStaminaPanelVisibility(true);
			this.UpdateAmmoIndicatorVisibility(true);
			this.UpdateHealthVisibility(true);
			this.UpdateOxygenVisibility(true);
			this.UpdateAbilitiesHudVisibility(true);
			this.UpdateStatusEffectHudVisibility(true);
			bool flag = this.InGame.Instance.GameMode == 0;
			if (flag)
			{
				this.ClearSlotHighlight();
			}
		}

		// Token: 0x06003DF9 RID: 15865 RVA: 0x000A2F30 File Offset: 0x000A1130
		private void OnItemsInitialized(Dictionary<string, ClientItemBase> items)
		{
			this.Items = items;
			this.InventoryPage.OnItemsUpdated();
		}

		// Token: 0x06003DFA RID: 15866 RVA: 0x000A2F48 File Offset: 0x000A1148
		private void OnItemsAdded(Dictionary<string, ClientItemBase> items)
		{
			foreach (ClientItemBase clientItemBase in items.Values)
			{
				this.Items[clientItemBase.Id] = clientItemBase;
			}
			this.InventoryPage.OnItemsUpdated();
		}

		// Token: 0x06003DFB RID: 15867 RVA: 0x000A2FB8 File Offset: 0x000A11B8
		private void OnItemsRemoved(string[] itemIds)
		{
			foreach (string key in itemIds)
			{
				this.Items.Remove(key);
			}
			this.InventoryPage.OnItemsUpdated();
			this.UpdateAmmoIndicatorVisibility(true);
		}

		// Token: 0x06003DFC RID: 15868 RVA: 0x000A2FFC File Offset: 0x000A11FC
		public void OnItemIconsUpdated(Texture texture)
		{
			Texture itemIconAtlasTexture = this.ItemIconAtlasTexture;
			if (itemIconAtlasTexture != null)
			{
				itemIconAtlasTexture.Dispose();
			}
			this.ItemIconAtlasTexture = texture;
			this.SetupCursorFloatingItem();
			this.HotbarComponent.OnItemIconsUpdated();
			this.InventoryPage.OnItemIconsUpdated();
		}

		// Token: 0x06003DFD RID: 15869 RVA: 0x000A3038 File Offset: 0x000A1238
		public void OnServerInfoUpdate(string serverName, string motd, int maxPlayers)
		{
			this.ServerName = serverName;
			this.Motd = motd;
			this.MaxPlayers = maxPlayers;
			bool isMounted = this.PlayerListComponent.IsMounted;
			if (isMounted)
			{
				this.PlayerListComponent.UpdateServerDetails();
			}
		}

		// Token: 0x06003DFE RID: 15870 RVA: 0x000A307C File Offset: 0x000A127C
		public void OnPlayerListAdd(PacketHandler.PlayerListPlayer[] players)
		{
			foreach (PacketHandler.PlayerListPlayer playerListPlayer in players)
			{
				this.Players[playerListPlayer.Uuid] = playerListPlayer;
			}
			bool isMounted = this.PlayerListComponent.IsMounted;
			if (isMounted)
			{
				this.PlayerListComponent.UpdateList();
			}
		}

		// Token: 0x06003DFF RID: 15871 RVA: 0x000A30D4 File Offset: 0x000A12D4
		public void OnPlayerListRemove(Guid[] players)
		{
			foreach (Guid key in players)
			{
				this.Players.Remove(key);
			}
			bool isMounted = this.PlayerListComponent.IsMounted;
			if (isMounted)
			{
				this.PlayerListComponent.UpdateList();
			}
		}

		// Token: 0x06003E00 RID: 15872 RVA: 0x000A3128 File Offset: 0x000A1328
		public void OnPlayerListUpdate(Dictionary<Guid, int> players)
		{
			foreach (KeyValuePair<Guid, int> keyValuePair in players)
			{
				PacketHandler.PlayerListPlayer playerListPlayer;
				bool flag = !this.Players.TryGetValue(keyValuePair.Key, out playerListPlayer);
				if (!flag)
				{
					playerListPlayer.Ping = keyValuePair.Value;
				}
			}
			bool isMounted = this.PlayerListComponent.IsMounted;
			if (isMounted)
			{
				this.PlayerListComponent.UpdateList();
			}
		}

		// Token: 0x06003E01 RID: 15873 RVA: 0x000A31C0 File Offset: 0x000A13C0
		public void OnPlayerListClear()
		{
			this.Players.Clear();
			bool isMounted = this.PlayerListComponent.IsMounted;
			if (isMounted)
			{
				this.PlayerListComponent.UpdateList();
			}
		}

		// Token: 0x06003E02 RID: 15874 RVA: 0x000A31F8 File Offset: 0x000A13F8
		private void OnAssetsUpdated(string[] assetNames)
		{
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			foreach (string text in assetNames)
			{
				bool flag4 = this._mountedTextureAssetPaths.Contains(text);
				if (flag4)
				{
					flag = true;
				}
				bool flag5 = text.StartsWith("UI/Custom/");
				if (flag5)
				{
					bool flag6 = text.EndsWith(".ui");
					if (flag6)
					{
						flag2 = true;
					}
					else
					{
						bool flag7 = text.EndsWith(".png");
						if (flag7)
						{
							flag3 = true;
						}
					}
				}
			}
			bool flag8 = flag;
			if (flag8)
			{
				this.ReloadAssetTextures();
			}
			bool flag9 = flag2;
			if (flag9)
			{
				this.ClearMarkupError();
				try
				{
					this.Interface.InGameCustomUIProvider.LoadDocuments();
				}
				catch (TextParser.TextParserException ex)
				{
					this.Interface.InGameCustomUIProvider.ClearDocuments();
					bool diagnosticMode = this.Interface.App.Settings.DiagnosticMode;
					if (!diagnosticMode)
					{
						this.DisconnectWithError("Failed to load updated CustomUI documents", ex);
						return;
					}
					this.DisplayMarkupError(ex.RawMessage, ex.Span);
				}
				catch (Exception exception)
				{
					this.Interface.InGameCustomUIProvider.ClearDocuments();
					this.DisconnectWithError("Failed to load updated CustomUI documents", exception);
					return;
				}
			}
			bool flag10 = flag3;
			if (flag10)
			{
				try
				{
					this.Interface.InGameCustomUIProvider.LoadTextures(this.Desktop.Scale > 1f);
				}
				catch (Exception exception2)
				{
					this.DisconnectWithError("Failed to load updated CustomUI textures", exception2);
				}
			}
		}

		// Token: 0x06003E03 RID: 15875 RVA: 0x000A339C File Offset: 0x000A159C
		public void OnStatChanged(int stat, ClientEntityStatValue value, float? previousValue)
		{
			bool flag = stat == DefaultEntityStats.Health;
			if (flag)
			{
				this.HealthComponent.OnStatChanged(value);
				this.InventoryPage.CharacterPanel.OnHealthChanged(value);
			}
			else
			{
				bool flag2 = stat == DefaultEntityStats.Mana;
				if (flag2)
				{
					this.InventoryPage.CharacterPanel.OnManaChanged(value);
				}
				else
				{
					bool flag3 = stat == DefaultEntityStats.Stamina;
					if (flag3)
					{
						this.StaminaPanelComponent.OnStatChanged(value, previousValue);
					}
					else
					{
						bool flag4 = stat == DefaultEntityStats.SignatureEnergy;
						if (flag4)
						{
							this.AbilitiesHudComponent.OnSignatureEnergyStatChanged(value);
						}
						else
						{
							bool flag5 = stat == DefaultEntityStats.Ammo;
							if (flag5)
							{
								this.AmmoIndicator.OnAmmoChanged(value);
							}
							else
							{
								bool flag6 = stat == DefaultEntityStats.Oxygen;
								if (flag6)
								{
									this.OxygenComponent.OnStatChanged(value);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06003E04 RID: 15876 RVA: 0x000A346B File Offset: 0x000A166B
		public void OnEffectAdded(int effectIndex)
		{
			this.StatusEffectsHudComponent.OnEffectAdded(effectIndex);
			this.StaminaPanelComponent.OnEffectAdded(effectIndex);
		}

		// Token: 0x06003E05 RID: 15877 RVA: 0x000A3488 File Offset: 0x000A1688
		public void OnEffectRemoved(int effectIndex)
		{
			this.StatusEffectsHudComponent.OnEffectRemoved(effectIndex);
			this.StaminaPanelComponent.OnEffectRemoved(effectIndex);
			this.AbilitiesHudComponent.OnEffectRemoved(effectIndex);
		}

		// Token: 0x06003E06 RID: 15878 RVA: 0x000A34B2 File Offset: 0x000A16B2
		public void OnReticleServerEvent(int eventIndex)
		{
			this.ReticleComponent.OnServerEvent(eventIndex);
		}

		// Token: 0x06003E07 RID: 15879 RVA: 0x000A34C2 File Offset: 0x000A16C2
		public void OnReticlesUpdated()
		{
			this.ReticleComponent.UpdateReticleImage();
		}

		// Token: 0x06003E08 RID: 15880 RVA: 0x000A34D4 File Offset: 0x000A16D4
		private bool TryGetServerProvidedAssetHash(string assetPath, out string hash)
		{
			hash = null;
			return this.InGame.Instance != null && this.InGame.Instance.HashesByServerAssetPath.TryGetValue(assetPath, ref hash);
		}

		// Token: 0x06003E09 RID: 15881 RVA: 0x000A3510 File Offset: 0x000A1710
		public bool TryMountAssetTexture(string assetPath, out TextureArea textureArea)
		{
			bool flag = string.IsNullOrEmpty(assetPath);
			bool result;
			if (flag)
			{
				textureArea = null;
				result = false;
			}
			else
			{
				bool flag2 = this.Desktop.Scale > 1f;
				bool flag3 = false;
				int scale = 1;
				string hash = null;
				bool flag4 = flag2;
				if (flag4)
				{
					string text = assetPath.Substring(0, assetPath.Length - ".png".Length) + "@2x.png";
					flag3 = this.TryGetServerProvidedAssetHash(text, out hash);
					bool flag5 = flag3;
					if (flag5)
					{
						scale = 2;
						assetPath = text;
					}
				}
				bool flag6 = !flag3;
				if (flag6)
				{
					flag3 = this.TryGetServerProvidedAssetHash(assetPath, out hash);
				}
				bool flag7 = !flag3 && !flag2;
				if (flag7)
				{
					string text2 = assetPath.Substring(0, assetPath.Length - ".png".Length) + "@2x.png";
					flag3 = this.TryGetServerProvidedAssetHash(text2, out hash);
					bool flag8 = flag3;
					if (flag8)
					{
						scale = 2;
						assetPath = text2;
					}
				}
				bool flag9 = !flag3;
				if (flag9)
				{
					textureArea = null;
					result = false;
				}
				else
				{
					string assetLocalPathUsingHash = AssetManager.GetAssetLocalPathUsingHash(hash);
					bool flag10 = this._mountedAssetTextureAreasByLocalPath.TryGetValue(assetLocalPathUsingHash, out textureArea);
					if (flag10)
					{
						result = true;
					}
					else
					{
						textureArea = ExternalTextureLoader.FromPath(assetLocalPathUsingHash);
						textureArea.Scale = scale;
						this._mountedAssetTextureAreasByLocalPath.Add(assetLocalPathUsingHash, textureArea);
						this._mountedTextureAssetPaths.Add(assetPath);
						result = true;
					}
				}
			}
			return result;
		}

		// Token: 0x06003E0A RID: 15882 RVA: 0x000A3668 File Offset: 0x000A1868
		public void ReloadAssetTextures()
		{
			foreach (TextureArea textureArea in this._mountedAssetTextureAreasByLocalPath.Values)
			{
				textureArea.Texture.Dispose();
			}
			this._mountedAssetTextureAreasByLocalPath.Clear();
			this._mountedTextureAssetPaths.Clear();
			this.NotificationFeedComponent.RebuildFeed();
			this.KillFeedComponent.RebuildFeed();
			this.ReticleComponent.UpdateReticleImage();
			this.CompassComponent.OnAssetsUpdated();
			this.InventoryPage.ItemLibraryPanel.SetupCategories();
		}

		// Token: 0x06003E0B RID: 15883 RVA: 0x000A3724 File Offset: 0x000A1924
		internal void DisconnectWithError(string reason, Exception exception)
		{
			App.AppStage stage = this.Interface.App.Stage;
			App.AppStage appStage = stage;
			if (appStage - App.AppStage.GameLoading > 1)
			{
				Debug.Fail(string.Format("Unexpected game stage: {0}", this.Interface.App.Stage));
			}
			else
			{
				this.InGame.Instance.DisconnectWithReason(reason, exception);
			}
		}

		// Token: 0x06003E0C RID: 15884 RVA: 0x000A3789 File Offset: 0x000A1989
		public void OpenAssetEditorMissingPathDialog()
		{
			this.InGame.SetCurrentOverlay(AppInGame.InGameOverlay.InGameMenu);
			this.InGameMenuOverlay.ShowSettings();
		}

		// Token: 0x17001096 RID: 4246
		// (get) Token: 0x06003E0D RID: 15885 RVA: 0x000A37A5 File Offset: 0x000A19A5
		// (set) Token: 0x06003E0E RID: 15886 RVA: 0x000A37AD File Offset: 0x000A19AD
		public InGameView.ClientInventoryPosition HoveredItemSlot { get; private set; }

		// Token: 0x17001097 RID: 4247
		// (get) Token: 0x06003E0F RID: 15887 RVA: 0x000A37B6 File Offset: 0x000A19B6
		// (set) Token: 0x06003E10 RID: 15888 RVA: 0x000A37BE File Offset: 0x000A19BE
		public InGameView.ClientInventoryPosition HighlightedItemSlot { get; private set; }

		// Token: 0x17001098 RID: 4248
		// (get) Token: 0x06003E11 RID: 15889 RVA: 0x000A37C7 File Offset: 0x000A19C7
		// (set) Token: 0x06003E12 RID: 15890 RVA: 0x000A37CF File Offset: 0x000A19CF
		public ClientItemStack[] StorageStacks { get; private set; }

		// Token: 0x17001099 RID: 4249
		// (get) Token: 0x06003E13 RID: 15891 RVA: 0x000A37D8 File Offset: 0x000A19D8
		// (set) Token: 0x06003E14 RID: 15892 RVA: 0x000A37E0 File Offset: 0x000A19E0
		public ClientItemStack[] ArmorStacks { get; private set; }

		// Token: 0x1700109A RID: 4250
		// (get) Token: 0x06003E15 RID: 15893 RVA: 0x000A37E9 File Offset: 0x000A19E9
		// (set) Token: 0x06003E16 RID: 15894 RVA: 0x000A37F1 File Offset: 0x000A19F1
		public ClientItemStack[] HotbarStacks { get; private set; }

		// Token: 0x1700109B RID: 4251
		// (get) Token: 0x06003E17 RID: 15895 RVA: 0x000A37FA File Offset: 0x000A19FA
		// (set) Token: 0x06003E18 RID: 15896 RVA: 0x000A3802 File Offset: 0x000A1A02
		public ClientItemStack[] UtilityStacks { get; private set; }

		// Token: 0x1700109C RID: 4252
		// (get) Token: 0x06003E19 RID: 15897 RVA: 0x000A380B File Offset: 0x000A1A0B
		// (set) Token: 0x06003E1A RID: 15898 RVA: 0x000A3813 File Offset: 0x000A1A13
		public ClientItemStack[] ConsumableStacks { get; private set; }

		// Token: 0x1700109D RID: 4253
		// (get) Token: 0x06003E1B RID: 15899 RVA: 0x000A381C File Offset: 0x000A1A1C
		// (set) Token: 0x06003E1C RID: 15900 RVA: 0x000A3824 File Offset: 0x000A1A24
		public ItemGrid.ItemDragData ItemDragData { get; private set; }

		// Token: 0x1700109E RID: 4254
		// (get) Token: 0x06003E1D RID: 15901 RVA: 0x000A382D File Offset: 0x000A1A2D
		// (set) Token: 0x06003E1E RID: 15902 RVA: 0x000A3835 File Offset: 0x000A1A35
		public int DefaultItemSlotsPerRow { get; private set; }

		// Token: 0x1700109F RID: 4255
		// (get) Token: 0x06003E1F RID: 15903 RVA: 0x000A383E File Offset: 0x000A1A3E
		// (set) Token: 0x06003E20 RID: 15904 RVA: 0x000A3846 File Offset: 0x000A1A46
		public ItemGrid.ItemGridStyle DefaultItemGridStyle { get; private set; }

		// Token: 0x170010A0 RID: 4256
		// (get) Token: 0x06003E21 RID: 15905 RVA: 0x000A384F File Offset: 0x000A1A4F
		// (set) Token: 0x06003E22 RID: 15906 RVA: 0x000A3857 File Offset: 0x000A1A57
		public Texture ItemIconAtlasTexture { get; private set; }

		// Token: 0x06003E23 RID: 15907 RVA: 0x000A3860 File Offset: 0x000A1A60
		private void OnWindowOpen(PacketHandler.InventoryWindow window)
		{
			PacketHandler.InventoryWindow inventoryWindow = this.InventoryWindow;
			int? num = (inventoryWindow != null) ? new int?(inventoryWindow.Id) : null;
			int id = window.Id;
			bool flag = num.GetValueOrDefault() == id & num != null;
			if (!flag)
			{
				this.InventoryWindow = window;
				this.InventoryPage.IsFieldcraft = false;
				bool isMounted = this.InventoryPage.IsMounted;
				if (isMounted)
				{
					this.InventoryPage.SetupWindows();
				}
			}
		}

		// Token: 0x06003E24 RID: 15908 RVA: 0x000A38E0 File Offset: 0x000A1AE0
		private void OnWindowUpdate(PacketHandler.InventoryWindow window)
		{
			bool flag = this.InventoryWindow == null || this.InventoryWindow.Id != window.Id;
			if (!flag)
			{
				this.InventoryWindow.WindowData = window.WindowData;
				this.InventoryWindow.Inventory = window.Inventory;
				bool isMounted = this.InventoryPage.IsMounted;
				if (isMounted)
				{
					this.InventoryPage.UpdateWindows();
				}
			}
		}

		// Token: 0x06003E25 RID: 15909 RVA: 0x000A3954 File Offset: 0x000A1B54
		private void OnWindowClose(int windowId)
		{
			bool flag = this.InventoryWindow == null || this.InventoryWindow.Id != windowId;
			if (!flag)
			{
				this.InventoryWindow = null;
				bool flag2 = this.InGame.CurrentPage == 2 || this.InGame.CurrentPage == 1;
				if (flag2)
				{
					this.InGame.TryClosePage();
				}
			}
		}

		// Token: 0x06003E26 RID: 15910 RVA: 0x000A39BC File Offset: 0x000A1BBC
		public void CloseAllInventoryWindows()
		{
			bool flag = this.InventoryWindow == null;
			if (!flag)
			{
				this.InGame.SendCloseWindowPacket(this.InventoryWindow.Id);
				this.InventoryWindow = null;
			}
		}

		// Token: 0x06003E27 RID: 15911 RVA: 0x000A39F8 File Offset: 0x000A1BF8
		private void OnDropItemBindingDown()
		{
			bool isDropItemBindingDown = this._isDropItemBindingDown;
			if (!isDropItemBindingDown)
			{
				this._isDropItemBindingDown = true;
				this.Desktop.RegisterAnimationCallback(new Action<float>(this.CheckForItemsToDrop));
			}
		}

		// Token: 0x06003E28 RID: 15912 RVA: 0x000A3A34 File Offset: 0x000A1C34
		private void OnDropItemBindingUp()
		{
			bool flag = !this._isDropItemBindingDown;
			if (!flag)
			{
				this._isDropItemBindingDown = false;
				this.Desktop.UnregisterAnimationCallback(new Action<float>(this.CheckForItemsToDrop));
				this.CheckForItemsToDrop(0f);
			}
		}

		// Token: 0x06003E29 RID: 15913 RVA: 0x000A3A7C File Offset: 0x000A1C7C
		private void CheckForItemsToDrop(float dt)
		{
			bool flag = this.HoveredItemSlot == null;
			if (!flag)
			{
				bool flag2 = this.GetItemStacks(this.HoveredItemSlot.InventorySectionId)[this.HoveredItemSlot.SlotId] == null;
				if (!flag2)
				{
					bool isDropItemBindingDown = this._isDropItemBindingDown;
					if (isDropItemBindingDown)
					{
						this._dropBindingHeldTick += dt;
						bool flag3 = this._dropBindingHeldTick >= 0.5f && !this._hasDroppedStack;
						if (flag3)
						{
							this.DropItemStack(this.HoveredItemSlot.InventorySectionId, this.HoveredItemSlot.SlotId, -1);
							this._hasDroppedStack = true;
						}
					}
					else
					{
						bool flag4 = this._dropBindingHeldTick > 0f && !this._hasDroppedStack;
						if (flag4)
						{
							this.DropItemStack(this.HoveredItemSlot.InventorySectionId, this.HoveredItemSlot.SlotId, 1);
						}
						this._hasDroppedStack = false;
						this._dropBindingHeldTick = 0f;
					}
				}
			}
		}

		// Token: 0x06003E2A RID: 15914 RVA: 0x000A3B78 File Offset: 0x000A1D78
		private void LayoutCursorFloatingItem(float deltaTime)
		{
			int num = -this.DefaultItemGridStyle.SlotSize / 4;
			bool isShiftKeyDown = this.Desktop.IsShiftKeyDown;
			if (isShiftKeyDown)
			{
				bool flag = this.ItemDragData.ItemStack.Quantity > 1;
				if (flag)
				{
					this._visibleDragStack.Quantity = (int)Math.Floor((double)((float)this.ItemDragData.ItemStack.Quantity / 2f));
				}
				else
				{
					this._visibleDragStack.Quantity = 1;
				}
			}
			else
			{
				bool flag2 = (long)this.ItemDragData.PressedMouseButton == 3L;
				if (flag2)
				{
					this._visibleDragStack.Quantity = 1;
				}
				else
				{
					this._visibleDragStack.Quantity = this.ItemDragData.ItemStack.Quantity;
				}
			}
			this._cursorFloatingItem.Anchor.Left = new int?(this.Desktop.UnscaleRound((float)this.Desktop.MousePosition.X) + num);
			this._cursorFloatingItem.Anchor.Top = new int?(this.Desktop.UnscaleRound((float)this.Desktop.MousePosition.Y) + num);
			this._cursorFloatingItem.Layout(new Rectangle?(this._rectangleAfterPadding), true);
			this._cursorFloatingItem.ShowDropIcon = this.InventoryPage.IsItemAtPositionDroppable(this.Desktop.MousePosition);
		}

		// Token: 0x06003E2B RID: 15915 RVA: 0x000A3CDA File Offset: 0x000A1EDA
		private void OnSetActiveHotbarSlot(int activeSlot)
		{
			this.InGame.SetActiveItemSelector(AppInGame.ItemSelector.None);
			this.HotbarComponent.OnSetActiveHotbarSlot(activeSlot);
			this.AbilitiesHudComponent.ShowOrHideHud();
			this.ReticleComponent.OnSetActiveSlot(activeSlot);
		}

		// Token: 0x06003E2C RID: 15916 RVA: 0x000A3D10 File Offset: 0x000A1F10
		private void OnSetActiveUtilitySlot(int activeSlot)
		{
			this.UtilitySlotSelector.SelectedSlot = activeSlot + 1;
			this.HotbarComponent.OnSetActiveUtilitySlot(activeSlot);
			this.InventoryPage.CharacterPanel.OnSetActiveUtilitySlot(activeSlot);
		}

		// Token: 0x06003E2D RID: 15917 RVA: 0x000A3D40 File Offset: 0x000A1F40
		private void OnSetActiveConsumableSlot(int activeSlot)
		{
			this.ConsumableSlotSelector.SelectedSlot = activeSlot + 1;
			this.HotbarComponent.OnSetActiveConsumableSlot(activeSlot);
			this.InventoryPage.CharacterPanel.OnSetActiveConsumableSlot(activeSlot);
		}

		// Token: 0x06003E2E RID: 15918 RVA: 0x000A3D70 File Offset: 0x000A1F70
		private void OnSetActiveToolsSlot(int activeSlot)
		{
		}

		// Token: 0x06003E2F RID: 15919 RVA: 0x000A3D74 File Offset: 0x000A1F74
		public void OnPlayerCharacterItemChanged(ItemChangeType changeType)
		{
			if (changeType != ItemChangeType.Dropped)
			{
				if (changeType == ItemChangeType.SlotChanged)
				{
					AbilitiesHudComponent abilitiesHudComponent = this.AbilitiesHudComponent;
					if (abilitiesHudComponent != null)
					{
						PlayerEntity localPlayer = this.InGame.Instance.LocalPlayer;
						abilitiesHudComponent.OnSignatureEnergyStatChanged((localPlayer != null) ? localPlayer.GetEntityStat(DefaultEntityStats.SignatureEnergy) : null);
					}
				}
			}
			else
			{
				AbilitiesHudComponent abilitiesHudComponent2 = this.AbilitiesHudComponent;
				if (abilitiesHudComponent2 != null)
				{
					abilitiesHudComponent2.OnSignatureEnergyStatChanged(null);
				}
			}
			bool flag = this.InGame.Instance.GameMode == 1;
			if (flag)
			{
				this.UpdateBuilderToolsLegendVisibility(true);
			}
			this.UpdateStaminaPanelVisibility(true);
			this.UpdateAmmoIndicatorVisibility(true);
			this.ReticleComponent.UpdateReticleImage();
		}

		// Token: 0x06003E30 RID: 15920 RVA: 0x000A3E16 File Offset: 0x000A2016
		private void OnSetAutosortType(SortType sortType)
		{
			this.InventoryPage.ContainerPanel.SetSortType(sortType);
			this.InventoryPage.StoragePanel.SetSortType(sortType);
		}

		// Token: 0x06003E31 RID: 15921 RVA: 0x000A3E40 File Offset: 0x000A2040
		private void OnInventorySetAll(ClientItemStack[] storageStacks, ClientItemStack[] armorStacks, ClientItemStack[] hotbarStacks, ClientItemStack[] utilityStacks, ClientItemStack[] consumableStacks)
		{
			this.StorageStacks = storageStacks;
			this.HotbarStacks = hotbarStacks;
			this.ArmorStacks = armorStacks;
			this.UtilityStacks = utilityStacks;
			this.ConsumableStacks = consumableStacks;
			this.SetupCursorFloatingItem();
			this.InventoryPage.OnSetStacks();
			this.HotbarComponent.OnSetStacks();
			this.ReticleComponent.OnSetStacks();
			this.UtilitySlotSelector.SetItemStacks(this.UtilityStacks);
			this.ConsumableSlotSelector.SetItemStacks(this.ConsumableStacks);
			this.UpdateAmmoIndicatorVisibility(true);
		}

		// Token: 0x06003E32 RID: 15922 RVA: 0x000A3ECF File Offset: 0x000A20CF
		public void SetupDragAndDropItem(ItemGrid.ItemDragData itemDragData)
		{
			this.ItemDragData = itemDragData;
			this._visibleDragStack = ((itemDragData == null) ? null : new ClientItemStack(itemDragData.ItemStack.Id, itemDragData.ItemStack.Quantity));
			this.SetupCursorFloatingItem();
		}

		// Token: 0x06003E33 RID: 15923 RVA: 0x000A3F08 File Offset: 0x000A2108
		public void SetupCursorFloatingItem()
		{
			bool flag = this.InGame.CurrentPage != null || this.InGame.IsToolsSettingsModalOpened;
			bool flag2 = this.ItemDragData != null && flag && !this.ItemQuantityPopup.IsMounted;
			if (flag2)
			{
				bool flag3 = this._cursorFloatingItem.Parent == null;
				if (flag3)
				{
					base.Add(this._cursorFloatingItem, -1);
					this.Desktop.RegisterAnimationCallback(new Action<float>(this.LayoutCursorFloatingItem));
				}
				this._cursorFloatingItem.Slot = new ItemGridSlot
				{
					ItemStack = this._visibleDragStack
				};
				this.LayoutCursorFloatingItem(0f);
			}
			else
			{
				bool flag4 = this._cursorFloatingItem.Parent != null;
				if (flag4)
				{
					base.Remove(this._cursorFloatingItem);
					this.Desktop.UnregisterAnimationCallback(new Action<float>(this.LayoutCursorFloatingItem));
				}
			}
			bool isMounted = this.InventoryPage.CharacterPanel.IsMounted;
			if (isMounted)
			{
				this.InventoryPage.CharacterPanel.UpdateCompatibleSlotHighlight(true);
			}
		}

		// Token: 0x06003E34 RID: 15924 RVA: 0x000A4020 File Offset: 0x000A2220
		public TextureArea GetTextureAreaForItemIcon(string icon)
		{
			Dictionary<string, ClientIcon> itemIcons = this.InGame.Instance.ItemLibraryModule.ItemIcons;
			ClientIcon clientIcon;
			bool flag = icon == null || !itemIcons.TryGetValue(icon, out clientIcon);
			TextureArea result;
			if (flag)
			{
				result = null;
			}
			else
			{
				result = new TextureArea(this.ItemIconAtlasTexture, clientIcon.X, clientIcon.Y, clientIcon.Size, clientIcon.Size, 1);
			}
			return result;
		}

		// Token: 0x06003E35 RID: 15925 RVA: 0x000A4088 File Offset: 0x000A2288
		public ClientItemStack[] GetItemStacks(int sectionId)
		{
			switch (sectionId)
			{
			case -6:
				return this.ConsumableStacks;
			case -5:
				return this.UtilityStacks;
			case -3:
				return this.ArmorStacks;
			case -2:
				return this.StorageStacks;
			case -1:
				return this.HotbarStacks;
			}
			PacketHandler.InventoryWindow inventoryWindow = this.InventoryWindow;
			bool flag = inventoryWindow != null && inventoryWindow.Id == sectionId;
			if (!flag)
			{
				throw new Exception("Invalid inventory section id: " + sectionId.ToString());
			}
			return this.InventoryWindow.Inventory;
		}

		// Token: 0x06003E36 RID: 15926 RVA: 0x000A412C File Offset: 0x000A232C
		public ClientItemStack GetHotbarItem(int slot)
		{
			bool flag = this.HotbarStacks == null || slot == -1;
			ClientItemStack result;
			if (flag)
			{
				result = null;
			}
			else
			{
				result = this.HotbarStacks[slot];
			}
			return result;
		}

		// Token: 0x06003E37 RID: 15927 RVA: 0x000A4160 File Offset: 0x000A2360
		public ClientItemStack GetUtilityItem(int slot)
		{
			bool flag = this.UtilityStacks == null || slot == -1;
			ClientItemStack result;
			if (flag)
			{
				result = null;
			}
			else
			{
				result = this.UtilityStacks[slot];
			}
			return result;
		}

		// Token: 0x06003E38 RID: 15928 RVA: 0x000A4194 File Offset: 0x000A2394
		public ClientItemStack GetConsumableItem(int slot)
		{
			bool flag = this.ConsumableStacks == null || slot == -1;
			ClientItemStack result;
			if (flag)
			{
				result = null;
			}
			else
			{
				result = this.ConsumableStacks[slot];
			}
			return result;
		}

		// Token: 0x06003E39 RID: 15929 RVA: 0x000A41C8 File Offset: 0x000A23C8
		public bool checkForSettingBrush(string itemStackName)
		{
			bool flag = this.canSetActiveBrushMaterial();
			bool result;
			if (flag)
			{
				this.InGame.Instance.BuilderToolsModule.setActiveBrushMaterial(itemStackName, this.Desktop.IsShiftKeyDown, this.Desktop.IsAltKeyDown);
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06003E3A RID: 15930 RVA: 0x000A4218 File Offset: 0x000A2418
		public bool canSetActiveBrushMaterial()
		{
			return this.InGame.Instance.GameMode == 1 && this.InGame.Instance.BuilderToolsModule.HasActiveBrush;
		}

		// Token: 0x06003E3B RID: 15931 RVA: 0x000A4258 File Offset: 0x000A2458
		public void HandleInventoryClick(int sectionId, int slotIndex, int button)
		{
			bool flag = this.Interface.App.InGame.CurrentPage != 2 && this.Interface.App.InGame.CurrentPage != 1 && (long)button != 1L;
			if (!flag)
			{
				ClientItemStack clientItemStack = this.GetItemStacks(sectionId)[slotIndex];
				bool flag2 = clientItemStack == null;
				if (flag2)
				{
					this.ClearSlotHighlight();
					this.Interface.App.InGame.Instance.BuilderToolsModule.ClearConfiguringTool();
				}
				else
				{
					bool flag3 = this.canSetActiveBrushMaterial() && (long)button == 2L;
					if (!flag3)
					{
						bool isShiftKeyDown = this.Desktop.IsShiftKeyDown;
						if (isShiftKeyDown)
						{
							bool isOpen = this.Interface.App.InGame.Instance.Chat.IsOpen;
							if (isOpen)
							{
								this.ChatComponent.InsertItemTag(clientItemStack.Id);
							}
							else
							{
								ClientItemStack itemStack = clientItemStack;
								bool flag4 = (long)button != 1L;
								if (flag4)
								{
									int quantity = 1;
									bool flag5 = clientItemStack.Quantity > 1 && (long)button == 2L;
									if (flag5)
									{
										quantity = (int)Math.Floor((double)((float)clientItemStack.Quantity / 2f));
									}
									itemStack = new ClientItemStack(clientItemStack.Id, quantity)
									{
										Metadata = clientItemStack.Metadata
									};
								}
								this.Interface.App.InGame.SendSmartMoveItemStackPacket(itemStack, sectionId, slotIndex, 1);
							}
						}
						bool flag6 = (long)button != 1L;
						if (!flag6)
						{
							bool flag7 = this.InGame.Instance.GameMode == 1 && (sectionId == -2 || sectionId == -1);
							if (flag7)
							{
								bool flag8 = this.Interface.App.InGame.Instance.BuilderToolsModule.TryConfigureTool(sectionId, slotIndex, clientItemStack);
								if (flag8)
								{
									this.HighlightSlot(sectionId, slotIndex);
								}
							}
							bool flag9 = sectionId == -1;
							if (flag9)
							{
								this.Interface.App.InGame.Instance.InventoryModule.SetActiveHotbarSlot(slotIndex, true);
							}
						}
					}
				}
			}
		}

		// Token: 0x06003E3C RID: 15932 RVA: 0x000A4474 File Offset: 0x000A2674
		public void HandleInventoryDoubleClick(int sectionId, int slotIndex)
		{
			bool flag = this.Interface.App.InGame.CurrentPage != 2 && this.Interface.App.InGame.CurrentPage != 1;
			if (!flag)
			{
				ClientItemStack clientItemStack = this.GetItemStacks(sectionId)[slotIndex];
				bool flag2 = clientItemStack == null;
				if (!flag2)
				{
					this.Interface.App.InGame.SendSmartMoveItemStackPacket(clientItemStack, sectionId, slotIndex, 0);
				}
			}
		}

		// Token: 0x06003E3D RID: 15933 RVA: 0x000A44EC File Offset: 0x000A26EC
		public bool HandleHotbarLoad(sbyte hotbarIndex)
		{
			return this.Interface.App.InGame.HandleHotbarLoad(hotbarIndex);
		}

		// Token: 0x06003E3E RID: 15934 RVA: 0x000A4514 File Offset: 0x000A2714
		public void HandleInventoryDragEnd(Element targetElement, int targetSectionId, int targetSlotIndex, Element sourceElement, ItemGrid.ItemDragData dragData)
		{
			bool flag = sourceElement == this.InventoryPage.ItemLibraryPanel.ItemGrid;
			if (flag)
			{
				bool flag2 = this.GetItemStacks(targetSectionId)[targetSlotIndex] != null;
				this.PlayItemMovedSound(targetElement);
				this.InGame.SendSetCreativeItemPacket(targetSectionId, targetSlotIndex, dragData.ItemStack, false);
				bool flag3 = !flag2;
				if (!flag3)
				{
					this.ClearSlotHighlight();
					bool flag4 = this.Interface.App.InGame.Instance.BuilderToolsModule.TryConfigureTool(targetSectionId, targetSlotIndex, dragData.ItemStack);
					if (flag4)
					{
						this.HighlightSlot(targetSectionId, targetSlotIndex);
					}
				}
			}
			else
			{
				bool flag5 = dragData.InventorySectionId == null;
				if (!flag5)
				{
					this.PlayItemMovedSound(targetElement);
					bool flag6 = this.InGame.Instance.GameMode == 1 && (long)dragData.PressedMouseButton == 2L;
					if (flag6)
					{
						this.InGame.SendSetCreativeItemPacket(targetSectionId, targetSlotIndex, dragData.ItemStack, false);
					}
					else
					{
						this.MoveItemStack(dragData.InventorySectionId.Value, dragData.SlotId, targetSectionId, targetSlotIndex, dragData.ItemStack);
					}
					this.ClearSlotHighlight();
					this.Interface.App.InGame.Instance.BuilderToolsModule.ClearConfiguringTool();
				}
			}
		}

		// Token: 0x06003E3F RID: 15935 RVA: 0x000A4668 File Offset: 0x000A2868
		private void PlayItemMovedSound(Element targetElement)
		{
			ItemGrid itemGrid = targetElement as ItemGrid;
			if (itemGrid == null)
			{
				ItemSlotSelectorPopover itemSlotSelectorPopover = targetElement as ItemSlotSelectorPopover;
				if (itemSlotSelectorPopover != null)
				{
					if (itemSlotSelectorPopover.ItemMovedSound != null)
					{
						this.Desktop.Provider.PlaySound(itemSlotSelectorPopover.ItemMovedSound);
					}
				}
			}
			else if (itemGrid.Style.ItemStackMovedSound != null)
			{
				this.Desktop.Provider.PlaySound(itemGrid.Style.ItemStackMovedSound);
			}
		}

		// Token: 0x06003E40 RID: 15936 RVA: 0x000A46E0 File Offset: 0x000A28E0
		public void MoveItemStack(int sourceSectionId, int sourceSlotIndex, int targetSectionId, int targetSlotIndex, ClientItemStack itemStack = null)
		{
			bool flag = itemStack == null;
			if (flag)
			{
				itemStack = this.GetItemStacks(sourceSectionId)[sourceSlotIndex];
			}
			this.InGame.SendMoveItemStackPacket(itemStack, sourceSectionId, sourceSlotIndex, targetSectionId, targetSlotIndex);
		}

		// Token: 0x06003E41 RID: 15937 RVA: 0x000A4718 File Offset: 0x000A2918
		public void HandleInventoryDropItem(int sectionId, int slotIndex, int button)
		{
			bool flag = (!this.InventoryPage.IsMounted && !this.Interface.App.InGame.IsToolsSettingsModalOpened) || !this.InventoryPage.IsItemAtPositionDroppable(this.Desktop.MousePosition);
			if (!flag)
			{
				this.DropItemStack(sectionId, slotIndex, ((long)button == 3L) ? 1 : -1);
				this.ClearSlotHighlight();
				this.Interface.App.InGame.Instance.BuilderToolsModule.ClearConfiguringTool();
			}
		}

		// Token: 0x06003E42 RID: 15938 RVA: 0x000A47A8 File Offset: 0x000A29A8
		public void HandleItemSlotMouseEntered(int sectionId, int slotIndex)
		{
			this._dropBindingHeldTick = 0f;
			this._hasDroppedStack = false;
			this.HoveredItemSlot = new InGameView.ClientInventoryPosition
			{
				InventorySectionId = sectionId,
				SlotId = slotIndex
			};
			bool isMounted = this.InventoryPage.CharacterPanel.IsMounted;
			if (isMounted)
			{
				this.InventoryPage.CharacterPanel.UpdateCompatibleSlotHighlight(true);
			}
		}

		// Token: 0x06003E43 RID: 15939 RVA: 0x000A4808 File Offset: 0x000A2A08
		public void HandleItemSlotMouseExited(int sectionId, int slotIndex)
		{
			InGameView.ClientInventoryPosition hoveredItemSlot = this.HoveredItemSlot;
			bool flag;
			if (hoveredItemSlot != null && hoveredItemSlot.SlotId == slotIndex)
			{
				InGameView.ClientInventoryPosition hoveredItemSlot2 = this.HoveredItemSlot;
				flag = (hoveredItemSlot2 == null || hoveredItemSlot2.InventorySectionId != sectionId);
			}
			else
			{
				flag = true;
			}
			bool flag2 = flag;
			if (!flag2)
			{
				this.HoveredItemSlot = null;
				bool isMounted = this.InventoryPage.CharacterPanel.IsMounted;
				if (isMounted)
				{
					this.InventoryPage.CharacterPanel.UpdateCompatibleSlotHighlight(true);
				}
			}
		}

		// Token: 0x06003E44 RID: 15940 RVA: 0x000A4880 File Offset: 0x000A2A80
		private void DropItemStack(int sectionId, int slotIndex, int quantity = -1)
		{
			ClientItemStack clientItemStack = this.GetItemStacks(sectionId)[slotIndex];
			ClientItemStack itemStack = new ClientItemStack(clientItemStack.Id, (quantity > -1) ? quantity : clientItemStack.Quantity)
			{
				Metadata = clientItemStack.Metadata
			};
			this.InGame.SendDropItemStackPacket(itemStack, sectionId, slotIndex);
		}

		// Token: 0x06003E45 RID: 15941 RVA: 0x000A48CC File Offset: 0x000A2ACC
		public void HighlightSlot(int sectionId, int slotIndex)
		{
			this.InventoryPage.StoragePanel.ClearSlotHighlight();
			this.HotbarComponent.ClearSlotHighlight();
			bool flag = this.HighlightedItemSlot != null && this.HighlightedItemSlot.InventorySectionId == sectionId && this.HighlightedItemSlot.SlotId == slotIndex;
			if (flag)
			{
				this.HighlightedItemSlot = null;
			}
			if (sectionId != -2)
			{
				if (sectionId != -1)
				{
					this.HighlightedItemSlot = new InGameView.ClientInventoryPosition
					{
						InventorySectionId = sectionId,
						SlotId = slotIndex
					};
				}
				else
				{
					this.HotbarComponent.HighlightSlot(slotIndex);
				}
			}
			else
			{
				this.InventoryPage.StoragePanel.HighlightSlot(slotIndex);
			}
		}

		// Token: 0x06003E46 RID: 15942 RVA: 0x000A497A File Offset: 0x000A2B7A
		public void ClearSlotHighlight()
		{
			this.InventoryPage.StoragePanel.ClearSlotHighlight();
			this.HotbarComponent.ClearSlotHighlight();
			this.HighlightedItemSlot = null;
		}

		// Token: 0x06003E47 RID: 15943 RVA: 0x000A49A4 File Offset: 0x000A2BA4
		internal bool IsItemValid(int index, ClientItemBase item, InventorySectionType inventorySectionType)
		{
			Debug.Assert(inventorySectionType == InventorySectionType.Storage || inventorySectionType == InventorySectionType.Hotbar);
			InGameView.ClientInventoryPosition hoveredItemSlot = this.HoveredItemSlot;
			bool flag = hoveredItemSlot == null;
			bool result;
			if (flag)
			{
				result = true;
			}
			else
			{
				bool flag2 = inventorySectionType == InventorySectionType.Storage;
				if (flag2)
				{
					index += this.HotbarStacks.Length;
				}
				bool flag3 = hoveredItemSlot.InventorySectionId >= 0 && this.InventoryWindow != null;
				if (flag3)
				{
					switch (this.InventoryWindow.WindowType)
					{
					case 2:
					{
						bool flag4 = item == null;
						if (flag4)
						{
							return false;
						}
						HashSet<int>[] inventoryHints = this.InventoryPage.DiagramCraftingPanel.InventoryHints;
						bool flag5 = inventoryHints[hoveredItemSlot.SlotId] == null || this.InventoryWindow.Inventory[hoveredItemSlot.SlotId] != null;
						return flag5 || inventoryHints[hoveredItemSlot.SlotId].Contains(index);
					}
					case 3:
					{
						bool flag6 = item == null;
						return !flag6 && this.InventoryPage.StructuralCraftingPanel.CompatibleSlots[index];
					}
					case 4:
					{
						bool flag7 = item == null;
						if (flag7)
						{
							return false;
						}
						bool flag8 = hoveredItemSlot.SlotId >= this.InventoryPage.ProcessingPanel.FuelSlotCount;
						if (flag8)
						{
							return this.InventoryPage.ProcessingPanel.CompatibleInputSlots[index];
						}
						string b = (string)this.InventoryWindow.WindowData["fuel"][hoveredItemSlot.SlotId]["resourceTypeId"];
						bool flag9 = item.ResourceTypes == null;
						if (flag9)
						{
							return false;
						}
						foreach (ClientItemResourceType clientItemResourceType in item.ResourceTypes)
						{
							bool flag10 = clientItemResourceType.Id == b;
							if (flag10)
							{
								return true;
							}
						}
						return false;
					}
					}
				}
				else
				{
					bool flag11 = hoveredItemSlot.InventorySectionId == -3;
					if (flag11)
					{
						bool flag12 = this.ArmorStacks[hoveredItemSlot.SlotId] != null;
						if (flag12)
						{
							return true;
						}
						bool flag13 = item == null;
						return !flag13 && item.Armor != null && item.Armor.ArmorSlot == hoveredItemSlot.SlotId;
					}
					else
					{
						bool flag14 = hoveredItemSlot.InventorySectionId == -5;
						if (flag14)
						{
							bool flag15 = item == null;
							return !flag15 && item.Utility != null && item.Utility.Usable;
						}
						bool flag16 = hoveredItemSlot.InventorySectionId == -6;
						if (flag16)
						{
							bool flag17 = item == null;
							return !flag17 && item.Consumable;
						}
					}
				}
				result = true;
			}
			return result;
		}

		// Token: 0x04001CD3 RID: 7379
		internal readonly AppInGame InGame;

		// Token: 0x04001CD4 RID: 7380
		public int NextPreviewId = 100;

		// Token: 0x04001CD5 RID: 7381
		public readonly EntityUIContainer EntityUIContainer;

		// Token: 0x04001CD6 RID: 7382
		public readonly Group HudContainer;

		// Token: 0x04001CD7 RID: 7383
		public readonly Group PageContainer;

		// Token: 0x04001CD8 RID: 7384
		public readonly Group ToolsSettingsPageContainer;

		// Token: 0x04001CD9 RID: 7385
		public readonly CustomHud CustomHud;

		// Token: 0x04001CDA RID: 7386
		public readonly UtilitySlotSelector UtilitySlotSelector;

		// Token: 0x04001CDB RID: 7387
		public readonly ConsumableSlotSelector ConsumableSlotSelector;

		// Token: 0x04001CDC RID: 7388
		public readonly BuilderToolsMaterialSlotSelector BuilderToolsMaterialSlotSelector;

		// Token: 0x04001CDD RID: 7389
		public readonly HotbarComponent HotbarComponent;

		// Token: 0x04001CDE RID: 7390
		public readonly StaminaPanelComponent StaminaPanelComponent;

		// Token: 0x04001CDF RID: 7391
		public readonly AmmoIndicator AmmoIndicator;

		// Token: 0x04001CE0 RID: 7392
		public readonly HealthComponent HealthComponent;

		// Token: 0x04001CE1 RID: 7393
		public readonly OxygenComponent OxygenComponent;

		// Token: 0x04001CE2 RID: 7394
		public readonly NotificationFeedComponent NotificationFeedComponent;

		// Token: 0x04001CE3 RID: 7395
		public readonly KillFeedComponent KillFeedComponent;

		// Token: 0x04001CE4 RID: 7396
		public readonly ReticleComponent ReticleComponent;

		// Token: 0x04001CE5 RID: 7397
		public readonly StatusEffectsHudComponent StatusEffectsHudComponent;

		// Token: 0x04001CE6 RID: 7398
		public readonly AbilitiesHudComponent AbilitiesHudComponent;

		// Token: 0x04001CE7 RID: 7399
		public readonly InputBindingsComponent InputBindingsComponent;

		// Token: 0x04001CE8 RID: 7400
		public readonly CompassComponent CompassComponent;

		// Token: 0x04001CE9 RID: 7401
		public readonly ObjectivePanelComponent ObjectivePanelComponent;

		// Token: 0x04001CEA RID: 7402
		public readonly EventTitleComponent EventTitleComponent;

		// Token: 0x04001CEB RID: 7403
		public readonly ChatComponent ChatComponent;

		// Token: 0x04001CEC RID: 7404
		public readonly PlayerListComponent PlayerListComponent;

		// Token: 0x04001CED RID: 7405
		public readonly BuilderToolsLegend BuilderToolsLegend;

		// Token: 0x04001CEE RID: 7406
		public readonly SpeedometerComponent SpeedometerComponent;

		// Token: 0x04001CEF RID: 7407
		public readonly MovementIndicator MovementIndicator;

		// Token: 0x04001CF0 RID: 7408
		public readonly DebugStressComponent DebugStressComponent;

		// Token: 0x04001CF1 RID: 7409
		public readonly InventoryPage InventoryPage;

		// Token: 0x04001CF2 RID: 7410
		public readonly MapPage MapPage;

		// Token: 0x04001CF3 RID: 7411
		public readonly ToolsSettingsPage ToolsSettingsPage;

		// Token: 0x04001CF4 RID: 7412
		public readonly MediaBrowserPage MediaBrowserPage;

		// Token: 0x04001CF5 RID: 7413
		public readonly CustomPage CustomPage;

		// Token: 0x04001CF6 RID: 7414
		public readonly InGameMenuOverlay InGameMenuOverlay;

		// Token: 0x04001CF7 RID: 7415
		public readonly MachinimaEditorOverlay MachinimaEditorOverlay;

		// Token: 0x04001CF8 RID: 7416
		public readonly ConfirmQuitOverlay ConfirmQuitToDesktopOverlay;

		// Token: 0x04001CF9 RID: 7417
		public readonly ItemQuantityPopup ItemQuantityPopup;

		// Token: 0x04001CFB RID: 7419
		private readonly HashSet<string> _mountedTextureAssetPaths = new HashSet<string>();

		// Token: 0x04001CFC RID: 7420
		private readonly Dictionary<string, TextureArea> _mountedAssetTextureAreasByLocalPath = new Dictionary<string, TextureArea>();

		// Token: 0x04001D00 RID: 7424
		public readonly Dictionary<Guid, PacketHandler.PlayerListPlayer> Players = new Dictionary<Guid, PacketHandler.PlayerListPlayer>();

		// Token: 0x04001D01 RID: 7425
		private HudComponent[] _visibleHudComponents = new HudComponent[0];

		// Token: 0x04001D02 RID: 7426
		private readonly MarkupErrorOverlay _customUIMarkupErrorOverlay;

		// Token: 0x04001D03 RID: 7427
		private bool _hasCustomUIMarkupError;

		// Token: 0x04001D04 RID: 7428
		public bool Wielding;

		// Token: 0x04001D0C RID: 7436
		public PacketHandler.InventoryWindow InventoryWindow;

		// Token: 0x04001D0E RID: 7438
		private ClientItemStack _visibleDragStack;

		// Token: 0x04001D0F RID: 7439
		private FloatingItemComponent _cursorFloatingItem;

		// Token: 0x04001D10 RID: 7440
		private bool _isDropItemBindingDown;

		// Token: 0x04001D11 RID: 7441
		private float _dropBindingHeldTick;

		// Token: 0x04001D12 RID: 7442
		private bool _hasDroppedStack;

		// Token: 0x02000D57 RID: 3415
		public class ClientInventoryPosition
		{
			// Token: 0x04004195 RID: 16789
			public int InventorySectionId;

			// Token: 0x04004196 RID: 16790
			public int SlotId;
		}
	}
}
