using System;
using System.Collections.Generic;
using HytaleClient.InGame.Modules.WorldMap;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Interface.UI.Styles;
using SDL2;

namespace HytaleClient.Interface.InGame.Pages
{
	// Token: 0x0200088D RID: 2189
	internal class MapPage : InterfaceComponent
	{
		// Token: 0x170010AD RID: 4269
		// (get) Token: 0x06003EAD RID: 16045 RVA: 0x000A9D00 File Offset: 0x000A7F00
		// (set) Token: 0x06003EAE RID: 16046 RVA: 0x000A9D08 File Offset: 0x000A7F08
		public SoundStyle OpenSound { get; private set; }

		// Token: 0x170010AE RID: 4270
		// (get) Token: 0x06003EAF RID: 16047 RVA: 0x000A9D11 File Offset: 0x000A7F11
		// (set) Token: 0x06003EB0 RID: 16048 RVA: 0x000A9D19 File Offset: 0x000A7F19
		public SoundStyle CloseSound { get; private set; }

		// Token: 0x06003EB1 RID: 16049 RVA: 0x000A9D24 File Offset: 0x000A7F24
		public MapPage(InGameView inGameView) : base(inGameView.Interface, null)
		{
			this._inGameView = inGameView;
			this.Interface.RegisterForEventFromEngine<string, string>("worldMap.setHighlightedBiome", new Action<string, string>(this.OnSetHighlightedBiome));
			this.Interface.RegisterForEventFromEngine("worldMap.hideContextMenu", new Action(this.OnHideContextMenu));
		}

		// Token: 0x06003EB2 RID: 16050 RVA: 0x000A9D84 File Offset: 0x000A7F84
		public void Build()
		{
			base.Clear();
			Label label = new Label(this.Desktop, this);
			label.Anchor.Width = new int?(500);
			label.Anchor.Height = new int?(30);
			label.Anchor.Right = new int?(50);
			label.Anchor.Bottom = new int?(150);
			label.Visible = false;
			this._zoneName = label;
			Label label2 = new Label(this.Desktop, this);
			label2.Anchor.Width = new int?(500);
			label2.Anchor.Height = new int?(30);
			label2.Anchor.Right = new int?(50);
			label2.Anchor.Bottom = new int?(120);
			label2.Visible = false;
			this._biomeName = label2;
			Document document;
			this.Interface.TryGetDocument("Common.ui", out document);
			PopupMenuLayerStyle style = document.ResolveNamedValue<PopupMenuLayerStyle>(this.Desktop.Provider, "DefaultPopupMenuLayerStyle");
			this._popup = new PopupMenuLayer(this.Desktop, null)
			{
				Style = style
			};
			Document document2;
			this.Interface.TryGetDocument("InGame/Pages/MapPage.ui", out document2);
			this.OpenSound = document2.ResolveNamedValue<SoundStyle>(this.Desktop.Provider, "OpenSound");
			this.CloseSound = document2.ResolveNamedValue<SoundStyle>(this.Desktop.Provider, "CloseSound");
			document2.TryResolveNamedValue<SoundStyle>(this.Desktop.Provider, "TeleportSound", out this._teleportSound);
			document2.TryResolveNamedValue<SoundStyle>(this.Desktop.Provider, "SelectLocationSound", out this._selectLocationSound);
			document2.TryResolveNamedValue<SoundStyle>(this.Desktop.Provider, "DeselectLocationSound", out this._deselectLocationSound);
			document2.TryResolveNamedValue<SoundStyle>(this.Desktop.Provider, "PlaceMarkerSound", out this._placeMarkerSound);
		}

		// Token: 0x06003EB3 RID: 16051 RVA: 0x000A9F6C File Offset: 0x000A816C
		protected List<PopupMenuItem> BuildMenuItems(bool isSelectedMarker)
		{
			List<PopupMenuItem> list = new List<PopupMenuItem>();
			if (isSelectedMarker)
			{
				list.Add(new PopupMenuItem(this.Desktop.Provider.GetText("ui.map.deselectLocation", null, true), new Action(this.OnPopupDeselectLocation), null, this._deselectLocationSound));
			}
			else
			{
				list.Add(new PopupMenuItem(this.Desktop.Provider.GetText("ui.map.selectLocation", null, true), new Action(this.OnPopupSelectLocation), null, this._selectLocationSound));
			}
			bool flag = this._inGameView.InGame.Instance.GameMode == 1;
			if (flag)
			{
				list.Add(new PopupMenuItem(this.Desktop.Provider.GetText("ui.map.teleport", null, true), new Action(this.OnPopupTeleport), null, this._teleportSound));
			}
			return list;
		}

		// Token: 0x06003EB4 RID: 16052 RVA: 0x000AA050 File Offset: 0x000A8250
		protected override void ApplyStyles()
		{
			base.ApplyStyles();
			this._zoneName.Style = new LabelStyle
			{
				FontSize = 20f,
				RenderBold = true,
				HorizontalAlignment = LabelStyle.LabelAlignment.End
			};
			this._biomeName.Style = new LabelStyle
			{
				FontSize = 20f,
				HorizontalAlignment = LabelStyle.LabelAlignment.End
			};
		}

		// Token: 0x06003EB5 RID: 16053 RVA: 0x000AA0B0 File Offset: 0x000A82B0
		protected override void OnMounted()
		{
			this._inGameView.InGame.SetSceneBlurEnabled(true);
			this._inGameView.InGame.Instance.WorldMapModule.SetVisible(true);
			this._zoneName.Visible = true;
			this._inGameView.HudContainer.Remove(this._inGameView.InputBindingsComponent);
			base.Add(this._inGameView.InputBindingsComponent, -1);
		}

		// Token: 0x06003EB6 RID: 16054 RVA: 0x000AA128 File Offset: 0x000A8328
		protected override void OnUnmounted()
		{
			this._inGameView.InGame.SetSceneBlurEnabled(false);
			this._inGameView.InGame.Instance.WorldMapModule.SetVisible(false);
			this._zoneName.Visible = false;
			this._popup.Close();
			base.Remove(this._inGameView.InputBindingsComponent);
			this._inGameView.HudContainer.Add(this._inGameView.InputBindingsComponent, -1);
			this._inGameView.HudContainer.Layout(null, true);
		}

		// Token: 0x06003EB7 RID: 16055 RVA: 0x000AA1C8 File Offset: 0x000A83C8
		protected internal override void OnKeyDown(SDL.SDL_Keycode keycode, int repeat)
		{
			base.OnKeyDown(keycode, repeat);
			bool flag = keycode == SDL.SDL_Keycode.SDLK_LSHIFT;
			if (flag)
			{
				this._biomeName.Visible = true;
				base.Layout(null, true);
			}
		}

		// Token: 0x06003EB8 RID: 16056 RVA: 0x000AA20C File Offset: 0x000A840C
		protected internal override void OnKeyUp(SDL.SDL_Keycode keycode)
		{
			bool flag = keycode == SDL.SDL_Keycode.SDLK_LSHIFT;
			if (flag)
			{
				this._biomeName.Visible = false;
			}
		}

		// Token: 0x06003EB9 RID: 16057 RVA: 0x000AA238 File Offset: 0x000A8438
		private void OnSetHighlightedBiome(string zoneName, string biomeName)
		{
			string text = this.Desktop.Provider.GetText("map.zones." + zoneName, null, false) ?? "";
			this._zoneName.Text = text;
			this._zoneName.Visible = (text != string.Empty);
			this._biomeName.Text = biomeName;
			bool isMounted = base.IsMounted;
			if (isMounted)
			{
				base.Layout(null, true);
			}
		}

		// Token: 0x06003EBA RID: 16058 RVA: 0x000AA2BC File Offset: 0x000A84BC
		public void OnMarkerPlaced()
		{
			bool flag = this._placeMarkerSound != null;
			if (flag)
			{
				this.Desktop.Provider.PlaySound(this._placeMarkerSound);
			}
		}

		// Token: 0x06003EBB RID: 16059 RVA: 0x000AA2F0 File Offset: 0x000A84F0
		public void OnShowContextMenu(WorldMapModule.MarkerSelection contextMarker, bool isSelectedMarker)
		{
			this._contextMarker = contextMarker;
			PopupMenuLayer popup = this._popup;
			WorldMapModule.MapMarker mapMarker = contextMarker.MapMarker;
			popup.SetTitle(((mapMarker != null) ? mapMarker.Name : null) ?? string.Format("X: {0}, Y: {1}", contextMarker.Coordinates.X, contextMarker.Coordinates.Y));
			this._popup.SetItems(this.BuildMenuItems(isSelectedMarker));
			this._popup.Open();
		}

		// Token: 0x06003EBC RID: 16060 RVA: 0x000AA370 File Offset: 0x000A8570
		private void OnHideContextMenu()
		{
			this._popup.Close();
		}

		// Token: 0x06003EBD RID: 16061 RVA: 0x000AA37E File Offset: 0x000A857E
		private void OnPopupSelectLocation()
		{
			this._inGameView.InGame.Instance.WorldMapModule.OnSelectContextMarker();
			this._inGameView.CompassComponent.SelectContextMarker(this._contextMarker);
		}

		// Token: 0x06003EBE RID: 16062 RVA: 0x000AA3B3 File Offset: 0x000A85B3
		private void OnPopupDeselectLocation()
		{
			this._inGameView.InGame.Instance.WorldMapModule.OnDeselectContextMarker();
			this._inGameView.CompassComponent.DeselectContextMarker();
		}

		// Token: 0x06003EBF RID: 16063 RVA: 0x000AA3E2 File Offset: 0x000A85E2
		private void OnPopupTeleport()
		{
			this._inGameView.InGame.Instance.WorldMapModule.OnTeleportToContextMarker();
		}

		// Token: 0x04001D86 RID: 7558
		private readonly InGameView _inGameView;

		// Token: 0x04001D87 RID: 7559
		private Label _zoneName;

		// Token: 0x04001D88 RID: 7560
		private Label _biomeName;

		// Token: 0x04001D89 RID: 7561
		private WorldMapModule.MarkerSelection _contextMarker;

		// Token: 0x04001D8A RID: 7562
		private PopupMenuLayer _popup;

		// Token: 0x04001D8B RID: 7563
		private SoundStyle _teleportSound;

		// Token: 0x04001D8C RID: 7564
		private SoundStyle _selectLocationSound;

		// Token: 0x04001D8D RID: 7565
		private SoundStyle _deselectLocationSound;

		// Token: 0x04001D8E RID: 7566
		private SoundStyle _placeMarkerSound;
	}
}
