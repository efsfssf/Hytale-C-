using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using HytaleClient.Application;
using HytaleClient.Data;
using HytaleClient.Graphics;
using HytaleClient.Interface.Common;
using HytaleClient.Interface.UI;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Interface.UI.Styles;
using HytaleClient.Math;
using HytaleClient.Protocol;
using HytaleClient.Utils;
using NLog;

namespace HytaleClient.Interface.MainMenu.Pages
{
	// Token: 0x0200081C RID: 2076
	internal class AdventurePage : InterfaceComponent
	{
		// Token: 0x06003990 RID: 14736 RVA: 0x0007CB84 File Offset: 0x0007AD84
		public AdventurePage(MainMenuView mainMenuView) : base(mainMenuView.Interface, null)
		{
			this.MainMenuView = mainMenuView;
			this._errorDialogSetup = new ModalDialog.DialogSetup
			{
				Cancellable = false
			};
		}

		// Token: 0x06003991 RID: 14737 RVA: 0x0007CBD8 File Offset: 0x0007ADD8
		public void Build()
		{
			base.Clear();
			this._currentPane = null;
			Document document;
			this.Interface.TryGetDocument("MainMenu/Adventure/AdventurePage.ui", out document);
			UIFragment uifragment = document.Instantiate(this.Desktop, this);
			Document document2;
			this.Interface.TryGetDocument("MainMenu/Adventure/WorldTile.ui", out document2);
			this._worldsPerRow = document2.ResolveNamedValue<int>(this.Desktop.Provider, "WorldsPerRow");
			this._tileWidth = document2.ResolveNamedValue<int>(this.Desktop.Provider, "TileWidth");
			this._tileHeight = document2.ResolveNamedValue<int>(this.Desktop.Provider, "TileHeight");
			this._tileSpacing = document2.ResolveNamedValue<int>(this.Desktop.Provider, "TileSpacing");
			this._emptyTileBackground = document2.ResolveNamedValue<PatchStyle>(this.Desktop.Provider, "EmptyTileBackground");
			Document document3;
			this.Interface.TryGetDocument("MainMenu/Adventure/WorldList.ui", out document3);
			UIFragment uifragment2 = document3.Instantiate(this.Desktop, null);
			this._worldListPane = uifragment2.Get<Group>("WorldList");
			this._worldsContainer = uifragment2.Get<Group>("WorldsContainer");
			uifragment2.Get<TextButton>("NewWorldButton").Activating = new Action(this.ShowWorldCreationPane);
			Document document4;
			this.Interface.TryGetDocument("MainMenu/Adventure/WorldCreation.ui", out document4);
			this._adventureTileButtonStyle = document4.ResolveNamedValue<Button.ButtonStyle>(this.Interface, "AdventureTileButtonStyle");
			this._creativeTileButtonStyle = document4.ResolveNamedValue<Button.ButtonStyle>(this.Interface, "CreativeTileButtonStyle");
			this._adventureTileButtonSelectedStyle = document4.ResolveNamedValue<Button.ButtonStyle>(this.Interface, "AdventureTileButtonSelectedStyle");
			this._creativeTileButtonSelectedStyle = document4.ResolveNamedValue<Button.ButtonStyle>(this.Interface, "CreativeTileButtonSelectedStyle");
			this._adventureIconPath = document4.ResolveNamedValue<UIPath>(this.Interface, "AdventureIconPath").Value;
			this._adventureIconSelectedPath = document4.ResolveNamedValue<UIPath>(this.Interface, "AdventureIconSelectedPath").Value;
			UIFragment uifragment3 = document4.Instantiate(this.Desktop, null);
			this._worldCreationPane = uifragment3.Get<Group>("WorldCreation");
			this._adventureTile = uifragment3.Get<Button>("AdventureTile");
			this._adventureTile.DoubleClicking = delegate()
			{
				this.SetGameMode(0);
				this._adventureTile.Layout(null, true);
				this._creativeTile.Layout(null, true);
				this.SaveWorld();
			};
			this._adventureTile.Activating = delegate()
			{
				this.SetGameMode(0);
				this._adventureTile.Layout(null, true);
				this._creativeTile.Layout(null, true);
			};
			this._creativeTile = uifragment3.Get<Button>("CreativeTile");
			this._creativeTile.DoubleClicking = delegate()
			{
				this.SetGameMode(1);
				this._adventureTile.Layout(null, true);
				this._creativeTile.Layout(null, true);
				this.SaveWorld();
			};
			this._creativeTile.Activating = delegate()
			{
				this.SetGameMode(1);
				this._adventureTile.Layout(null, true);
				this._creativeTile.Layout(null, true);
			};
			this._creativeTile.RightClicking = delegate()
			{
				this._worldNameSettingInput.Value = this._worldNameInput.Value;
				this._worldNameSettingInput.PlaceholderText = this._worldNameInput.PlaceholderText;
				this._worldDirectoryOptions.Visible = false;
				this._saveWorldSettingsButton.Visible = false;
				this._createWorldSettingsButton.Visible = true;
				this._creativeOptions.Visible = true;
				this._flatWorldSettingCheckbox.Parent.Visible = true;
				this.SetGameMode(1);
				this.SetPane(this._worldSettingsPane);
			};
			this._creativeModeSettingsSummary = uifragment3.Get<Label>("CreativeModeSettingsSummary");
			this._worldNameInput = uifragment3.Get<TextField>("WorldNameInput");
			uifragment3.Get<TextButton>("CreateWorldButton").Activating = new Action(this.OnSaveWorld);
			Document document5;
			this.Interface.TryGetDocument("MainMenu/Adventure/WorldSettings.ui", out document5);
			UIFragment uifragment4 = document5.Instantiate(this.Desktop, null);
			this._worldSettingsPane = uifragment4.Get<Group>("WorldSettings");
			this._saveWorldSettingsButton = uifragment4.Get<TextButton>("SaveWorldSettingsButton");
			this._saveWorldSettingsButton.Activating = new Action(this.OnSaveWorld);
			this._createWorldSettingsButton = uifragment4.Get<TextButton>("CreateWorldSettingsButton");
			this._createWorldSettingsButton.Activating = new Action(this.OnSaveWorld);
			this._worldNameSettingInput = uifragment4.Get<TextField>("WorldNameSettingInput");
			this._creativeOptions = uifragment4.Get<Group>("CreativeModeOptions");
			this._worldDirectoryOptions = uifragment4.Get<Group>("WorldDirectoryOptions");
			this._worldSettingsImage = uifragment4.Get<Group>("WorldSettingsImage");
			this._worldSettingsModeName = uifragment4.Get<Label>("WorldSettingsModeName");
			this._worldSettingsModeDescription = uifragment4.Get<Label>("WorldSettingsModeDescription");
			this._flatWorldSettingCheckbox = uifragment4.Get<CheckBox>("FlatWorldSettingCheckBox");
			this._npcsSettingCheckbox = uifragment4.Get<CheckBox>("NpcsSettingCheckBox");
			uifragment4.Get<Button>("BackButton").Activating = delegate()
			{
				this.SetPane((this._editingWorld != null) ? this._worldListPane : this._worldCreationPane);
			};
			uifragment4.Get<TextButton>("OpenWorldDirectoryButton").Activating = delegate()
			{
				this.Interface.App.MainMenu.OpenSingleplayerWorldFolder(this._editingWorld);
			};
			uifragment4.Get<TextButton>("DeleteWorldButton").Activating = new Action(this.OnShowDeleteWorldPopup);
			this._backButton = new BackButton(this.Interface, new Action(this.Dismiss));
			uifragment.Get<Group>("BackButtonContainer").Add(this._backButton, -1);
			this.SetPane(this._worldListPane);
			bool isMounted = base.IsMounted;
			if (isMounted)
			{
				this.ShowWorldListPane();
			}
		}

		// Token: 0x06003992 RID: 14738 RVA: 0x0007D082 File Offset: 0x0007B282
		protected override void OnMounted()
		{
			this.MainMenuView.ShowTopBar(true);
			this.Interface.App.MainMenu.GatherWorldList();
			this.ShowWorldListPane();
		}

		// Token: 0x06003993 RID: 14739 RVA: 0x0007D0AF File Offset: 0x0007B2AF
		protected override void OnUnmounted()
		{
			this._worldPreviewElements.Clear();
			this.ClearLoadedWorldPreviewImages();
			this.SetPane(this._worldListPane);
		}

		// Token: 0x06003994 RID: 14740 RVA: 0x0007D0D4 File Offset: 0x0007B2D4
		private void SetGameMode(GameMode gameMode)
		{
			this._gameMode = gameMode;
			this._creativeTile.Style = ((gameMode == 1) ? this._creativeTileButtonSelectedStyle : this._creativeTileButtonStyle);
			this._creativeTile.Find<Group>("Effect").Visible = (gameMode == 1);
			this._adventureTile.Style = ((gameMode == null) ? this._adventureTileButtonSelectedStyle : this._adventureTileButtonStyle);
			this._adventureTile.Find<Group>("Effect").Visible = (gameMode == 0);
			this._adventureTile.Find<Group>("Icon").Background = new PatchStyle((gameMode == null) ? this._adventureIconSelectedPath : this._adventureIconPath);
			this._worldSettingsModeName.Text = "/ " + this.Desktop.Provider.GetText("ui.general.gamemodes." + gameMode.ToString().ToLower(), null, true) + " World";
			this._worldSettingsModeDescription.Text = this.Desktop.Provider.GetText("ui.mainMenu.adventure." + gameMode.ToString().ToLower() + "ModeDescription", null, true);
			this._worldSettingsImage.Background = new PatchStyle(string.Format("MainMenu/Adventure/{0}ModeImage.png", gameMode));
		}

		// Token: 0x06003995 RID: 14741 RVA: 0x0007D22C File Offset: 0x0007B42C
		private void SetPane(Group pane)
		{
			bool flag = this._currentPane == pane;
			if (!flag)
			{
				bool flag2 = this._currentPane != null;
				if (flag2)
				{
					base.Remove(this._currentPane);
				}
				this._currentPane = pane;
				bool flag3 = this._currentPane == this._worldCreationPane;
				if (flag3)
				{
					this._creativeModeSettingsSummary.Text = this.Desktop.Provider.GetText(this._flatWorldSettingCheckbox.Value ? "ui.mainMenu.adventure.flatWorld" : "ui.mainMenu.adventure.randomWorld", null, true) + " / " + this.Desktop.Provider.GetText(this._npcsSettingCheckbox.Value ? "ui.mainMenu.adventure.npcSpawning" : "ui.mainMenu.adventure.noNpcSpawning", null, true);
				}
				base.Add(pane, -1);
				bool isMounted = pane.IsMounted;
				if (isMounted)
				{
					pane.Layout(new Rectangle?(base.RectangleAfterPadding), true);
					this.Desktop.RefreshHover();
				}
				this._backButton.Visible = (this._currentPane != this._worldListPane);
				bool isMounted2 = this._backButton.IsMounted;
				if (isMounted2)
				{
					this._backButton.Layout(new Rectangle?(this._backButton.Parent.RectangleAfterPadding), true);
				}
			}
		}

		// Token: 0x06003996 RID: 14742 RVA: 0x0007D371 File Offset: 0x0007B571
		private void OnSaveWorld()
		{
			this.SaveWorld();
		}

		// Token: 0x06003997 RID: 14743 RVA: 0x0007D37C File Offset: 0x0007B57C
		private void SaveWorld()
		{
			bool flag = this._currentPane == this._worldCreationPane;
			string text;
			if (flag)
			{
				text = this._worldNameInput.Value.Trim();
				bool flag2 = text == "";
				if (flag2)
				{
					text = this._worldNameInput.PlaceholderText.Trim();
				}
			}
			else
			{
				text = this._worldNameSettingInput.Value.Trim();
				bool flag3 = text == "";
				if (flag3)
				{
					text = this._worldNameSettingInput.PlaceholderText.Trim();
				}
			}
			bool flag4 = text == "";
			if (!flag4)
			{
				bool flag5 = this._editingWorld == null;
				if (flag5)
				{
					AppMainMenu.WorldOptions options = new AppMainMenu.WorldOptions
					{
						Name = text,
						FlatWorld = (this._gameMode != null && this._flatWorldSettingCheckbox.Value),
						NpcSpawning = (this._gameMode == null || this._npcsSettingCheckbox.Value),
						GameMode = this._gameMode
					};
					string text2;
					bool flag6 = !this.Interface.App.MainMenu.TryCreateSingleplayerWorld(options, out text2);
					if (flag6)
					{
						this._errorDialogSetup.Title = "ui.general.error";
						this._errorDialogSetup.Text = text2;
						this.Interface.ModalDialog.Setup(this._errorDialogSetup);
						this.Desktop.SetLayer(4, this.Interface.ModalDialog);
					}
				}
				else
				{
					AppMainMenu.WorldOptions options2 = new AppMainMenu.WorldOptions
					{
						Name = text,
						NpcSpawning = this._npcsSettingCheckbox.Value,
						GameMode = this._gameMode
					};
					string text3;
					bool flag7 = !this.Interface.App.MainMenu.TryUpdateSingleplayerWorldOptions(this._editingWorld, options2, out text3);
					if (flag7)
					{
						this._errorDialogSetup.Title = "ui.general.error";
						this._errorDialogSetup.Text = text3;
						this.Interface.ModalDialog.Setup(this._errorDialogSetup);
						this.Desktop.SetLayer(4, this.Interface.ModalDialog);
					}
					else
					{
						this.Interface.App.MainMenu.GatherWorldList();
						this.ShowWorldListPane();
					}
				}
			}
		}

		// Token: 0x06003998 RID: 14744 RVA: 0x0007D5BA File Offset: 0x0007B7BA
		private void JoinWorld(string directoryName)
		{
			this.Interface.App.MainMenu.JoinSingleplayerWorld(directoryName);
		}

		// Token: 0x06003999 RID: 14745 RVA: 0x0007D5D4 File Offset: 0x0007B7D4
		private void ShowWorldListPane()
		{
			bool hasMarkupError = this.Interface.HasMarkupError;
			if (!hasMarkupError)
			{
				this._worldPreviewElements.Clear();
				bool flag = this.Interface.App.MainMenu.Worlds.Count == 0;
				if (flag)
				{
					this.ShowWorldCreationPane();
				}
				else
				{
					this.SetPane(this._worldListPane);
					this.ClearLoadedWorldPreviewImages();
					this._worldsContainer.Clear();
					List<AppMainMenu.World> worlds = this.Interface.App.MainMenu.Worlds;
					this._tileCount = Math.Max(worlds.Count, this._worldsPerRow * 2);
					bool flag2 = this._tileCount > this._worldsPerRow * 2;
					if (flag2)
					{
						int num = this._tileCount % this._worldsPerRow;
						bool flag3 = num > 0;
						if (flag3)
						{
							this._tileCount += this._worldsPerRow - num;
						}
					}
					for (int i = 0; i < this._tileCount; i++)
					{
						bool flag4 = i >= worlds.Count;
						if (flag4)
						{
							Group group = this.<ShowWorldListPane>g__MakeTileContainer|48_0(i);
							group.Background = this._emptyTileBackground;
						}
						else
						{
							AppMainMenu.World world = worlds[i];
							Group root = this.<ShowWorldListPane>g__MakeTileContainer|48_0(i);
							Document document;
							this.Interface.TryGetDocument("MainMenu/Adventure/WorldTile.ui", out document);
							UIFragment uifragment = document.Instantiate(this.Desktop, root);
							Button button = uifragment.Get<Button>("Button");
							button.Activating = delegate()
							{
								this.JoinWorld(world.Path);
							};
							button.RightClicking = delegate()
							{
								this.OnEditWorldButtonActivate(world.Path);
							};
							string text = Path.Combine(Paths.Saves, world.Path, "preview.png");
							Element element = uifragment.Get<Element>("Image");
							this._worldPreviewElements[world.Path] = element;
							int num2;
							int num3;
							bool flag5 = File.Exists(text) && Image.TryGetPngDimensions(text, out num2, out num3);
							if (flag5)
							{
								TextureArea textureArea = ExternalTextureLoader.FromPath(text);
								this._worldPreviewTextures.Add(text, textureArea.Texture);
								bool flag6 = this._tileHeight > this._tileWidth;
								if (flag6)
								{
									int num4 = this._tileHeight / this._tileHeight * num2;
									int y = (num3 - num4) / 2;
									element.Background = new PatchStyle(textureArea)
									{
										Area = new Rectangle?(new Rectangle(0, y, num2, num4)),
										Color = UInt32Color.White
									};
								}
								else
								{
									int num5 = this._tileWidth / this._tileHeight * num3;
									int x = (num2 - num5) / 2;
									element.Background = new PatchStyle(textureArea)
									{
										Area = new Rectangle?(new Rectangle(x, 0, num5, num3)),
										Color = UInt32Color.White
									};
								}
							}
							uifragment.Get<Label>("Name").Text = world.Options.Name;
							uifragment.Get<Label>("LastWriteTime").Text = this.Interface.FormatRelativeTime(DateTime.Parse(world.LastWriteTime, null, DateTimeStyles.RoundtripKind));
							uifragment.Get<Element>("GameModeIcon").Background = new PatchStyle("MainMenu/Adventure/GameModeIcon" + world.Options.GameMode.ToString() + ".png");
						}
					}
					int num6 = (int)Math.Ceiling((double)((float)this._tileCount / (float)this._worldsPerRow));
					this._worldsContainer.ContentHeight = new int?(num6 * this._tileHeight + (num6 - 1) * this._tileSpacing);
					this._worldsContainer.Layout(null, true);
				}
			}
		}

		// Token: 0x0600399A RID: 14746 RVA: 0x0007D9BC File Offset: 0x0007BBBC
		private void OnShowDeleteWorldPopup()
		{
			string name = this.Interface.App.MainMenu.Worlds.Find((AppMainMenu.World w) => w.Path == this._editingWorld).Options.Name;
			string text = this.Desktop.Provider.GetText("ui.mainMenu.adventure.deleteWorldConfirmation", null, true).Replace("{name}", name);
			this.Interface.ModalDialog.Setup(new ModalDialog.DialogSetup
			{
				Title = "ui.mainMenu.adventure.deleteWorld",
				Text = text,
				ConfirmationText = "ui.general.delete",
				OnConfirm = new Action(this.OnConfirmDeleteWorld)
			});
			this.Desktop.SetLayer(4, this.Interface.ModalDialog);
		}

		// Token: 0x0600399B RID: 14747 RVA: 0x0007DA7C File Offset: 0x0007BC7C
		private void OnConfirmDeleteWorld()
		{
			string text;
			bool flag = !this.Interface.App.MainMenu.TryDeleteSingleplayerWorld(this._editingWorld, out text);
			if (flag)
			{
				this._errorDialogSetup.Title = "ui.general.error";
				this._errorDialogSetup.Text = text;
				this.Interface.ModalDialog.Setup(this._errorDialogSetup);
				this.Desktop.SetLayer(4, this.Interface.ModalDialog);
			}
			else
			{
				this.ShowWorldListPane();
			}
		}

		// Token: 0x0600399C RID: 14748 RVA: 0x0007DB04 File Offset: 0x0007BD04
		private void ShowWorldCreationPane()
		{
			this._editingWorld = null;
			this._worldNameInput.Value = "";
			this._npcsSettingCheckbox.Value = false;
			this._flatWorldSettingCheckbox.Value = false;
			this.SetGameMode(0);
			string text = this.Desktop.Provider.GetText("ui.mainMenu.adventure.defaultWorldName", null, true);
			bool flag = this.Interface.App.MainMenu.Worlds.Count > 0;
			if (flag)
			{
				int num = -1;
				foreach (AppMainMenu.World world in this.Interface.App.MainMenu.Worlds)
				{
					bool flag2 = world.Options.Name.Contains(text);
					if (flag2)
					{
						bool flag3 = world.Options.Name == text;
						if (flag3)
						{
							num = Math.Max(num, 0);
						}
						else
						{
							int val;
							bool flag4 = int.TryParse(world.Options.Name.Replace(text, "").Trim(), out val);
							if (flag4)
							{
								num = Math.Max(num, val);
							}
						}
					}
				}
				bool flag5 = num == -1;
				if (flag5)
				{
					this._worldNameInput.PlaceholderText = text;
				}
				else
				{
					this._worldNameInput.PlaceholderText = text + " " + (num + 1).ToString();
				}
			}
			else
			{
				this._worldNameInput.PlaceholderText = text;
			}
			this.SetPane(this._worldCreationPane);
			this.Desktop.FocusElement(this._worldNameInput, true);
		}

		// Token: 0x0600399D RID: 14749 RVA: 0x0007DCC0 File Offset: 0x0007BEC0
		private void OnEditWorldButtonActivate(string directoryName)
		{
			AppMainMenu.World world = this.Interface.App.MainMenu.Worlds.Find((AppMainMenu.World w) => w.Path == directoryName);
			this._editingWorld = directoryName;
			this._saveWorldSettingsButton.Visible = true;
			this._createWorldSettingsButton.Visible = false;
			this._worldNameSettingInput.Value = world.Options.Name;
			this._worldNameSettingInput.PlaceholderText = "";
			this._worldDirectoryOptions.Visible = true;
			this._creativeOptions.Visible = (world.Options.GameMode == 1);
			this.SetGameMode(world.Options.GameMode);
			this._npcsSettingCheckbox.Value = world.Options.NpcSpawning;
			this._flatWorldSettingCheckbox.Value = world.Options.FlatWorld;
			this._flatWorldSettingCheckbox.Parent.Visible = false;
			this.SetPane(this._worldSettingsPane);
		}

		// Token: 0x0600399E RID: 14750 RVA: 0x0007DDD8 File Offset: 0x0007BFD8
		private void ClearLoadedWorldPreviewImages()
		{
			foreach (Texture texture in this._worldPreviewTextures.Values)
			{
				texture.Dispose();
			}
			this._worldPreviewTextures.Clear();
		}

		// Token: 0x0600399F RID: 14751 RVA: 0x0007DE40 File Offset: 0x0007C040
		public void OnWorldPreviewUpdated(string worldDirectoryName)
		{
			AdventurePage.Logger.Info("Reloading world preview for '{0}'", worldDirectoryName);
			Element element;
			bool flag = !this._worldPreviewElements.TryGetValue(worldDirectoryName, out element);
			if (flag)
			{
				AdventurePage.Logger.Warn("No world preview element found!");
			}
			else
			{
				string text = Path.Combine(Paths.Saves, worldDirectoryName, "preview.png");
				int num;
				int num2;
				bool flag2 = File.Exists(text) && Image.TryGetPngDimensions(text, out num, out num2);
				if (flag2)
				{
					TextureArea textureArea = ExternalTextureLoader.FromPath(text);
					this._worldPreviewTextures.Add(text, textureArea.Texture);
					bool flag3 = this._tileHeight > this._tileWidth;
					if (flag3)
					{
						int num3 = this._tileHeight / this._tileHeight * num;
						int y = (num2 - num3) / 2;
						element.Background = new PatchStyle(textureArea)
						{
							Area = new Rectangle?(new Rectangle(0, y, num, num3)),
							Color = UInt32Color.White
						};
					}
					else
					{
						int num4 = this._tileWidth / this._tileHeight * num2;
						int x = (num - num4) / 2;
						element.Background = new PatchStyle(textureArea)
						{
							Area = new Rectangle?(new Rectangle(x, 0, num4, num2)),
							Color = UInt32Color.White
						};
					}
				}
				else
				{
					AdventurePage.Logger.Warn("Failed to load thumbnail");
				}
			}
		}

		// Token: 0x060039A0 RID: 14752 RVA: 0x0007DF98 File Offset: 0x0007C198
		public void OnFailedToJoinUnknownWorld()
		{
			this._errorDialogSetup.Title = "ui.mainMenu.adventure.worldDoesntExist.title";
			this._errorDialogSetup.Text = "ui.mainMenu.adventure.worldDoesntExist.message";
			this.Interface.ModalDialog.Setup(this._errorDialogSetup);
			this.Desktop.SetLayer(4, this.Interface.ModalDialog);
		}

		// Token: 0x060039A1 RID: 14753 RVA: 0x0007DFF8 File Offset: 0x0007C1F8
		protected internal override void Validate()
		{
			bool flag = this._currentPane == this._worldSettingsPane || this._currentPane == this._worldCreationPane || (this._currentPane == this._worldListPane && this.Interface.App.MainMenu.Worlds.Count == 0);
			if (flag)
			{
				this.SaveWorld();
			}
		}

		// Token: 0x060039A2 RID: 14754 RVA: 0x0007E060 File Offset: 0x0007C260
		protected internal override void Dismiss()
		{
			bool flag = this._currentPane == this._worldSettingsPane;
			if (flag)
			{
				this.SetPane((this._editingWorld == null) ? this._worldCreationPane : this._worldListPane);
			}
			else
			{
				bool flag2 = this._currentPane == this._worldCreationPane && this.Interface.App.MainMenu.Worlds.Count > 0;
				if (flag2)
				{
					this.SetPane(this._worldListPane);
				}
				else
				{
					this.Interface.App.MainMenu.Open(AppMainMenu.MainMenuPage.Home);
				}
			}
		}

		// Token: 0x060039AB RID: 14763 RVA: 0x0007E2FC File Offset: 0x0007C4FC
		[CompilerGenerated]
		private Group <ShowWorldListPane>g__MakeTileContainer|48_0(int index)
		{
			int value = index % this._worldsPerRow * (this._tileWidth + this._tileSpacing);
			int value2 = index / this._worldsPerRow * (this._tileHeight + this._tileSpacing);
			return new Group(this.Desktop, this._worldsContainer)
			{
				Anchor = new Anchor
				{
					Left = new int?(value),
					Top = new int?(value2),
					Width = new int?(this._tileWidth),
					Height = new int?(this._tileHeight)
				}
			};
		}

		// Token: 0x04001974 RID: 6516
		public static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x04001975 RID: 6517
		public readonly MainMenuView MainMenuView;

		// Token: 0x04001976 RID: 6518
		private Group _worldsContainer;

		// Token: 0x04001977 RID: 6519
		private int _worldsPerRow;

		// Token: 0x04001978 RID: 6520
		private int _tileWidth;

		// Token: 0x04001979 RID: 6521
		private int _tileHeight;

		// Token: 0x0400197A RID: 6522
		private int _tileSpacing;

		// Token: 0x0400197B RID: 6523
		private PatchStyle _emptyTileBackground;

		// Token: 0x0400197C RID: 6524
		private readonly Dictionary<string, Texture> _worldPreviewTextures = new Dictionary<string, Texture>();

		// Token: 0x0400197D RID: 6525
		private readonly Dictionary<string, Element> _worldPreviewElements = new Dictionary<string, Element>();

		// Token: 0x0400197E RID: 6526
		private int _tileCount;

		// Token: 0x0400197F RID: 6527
		private Group _worldListPane;

		// Token: 0x04001980 RID: 6528
		private Group _worldCreationPane;

		// Token: 0x04001981 RID: 6529
		private Group _worldSettingsPane;

		// Token: 0x04001982 RID: 6530
		private Group _currentPane;

		// Token: 0x04001983 RID: 6531
		private Button _adventureTile;

		// Token: 0x04001984 RID: 6532
		private Button _creativeTile;

		// Token: 0x04001985 RID: 6533
		private TextField _worldNameInput;

		// Token: 0x04001986 RID: 6534
		private Label _creativeModeSettingsSummary;

		// Token: 0x04001987 RID: 6535
		private Button.ButtonStyle _adventureTileButtonStyle;

		// Token: 0x04001988 RID: 6536
		private Button.ButtonStyle _creativeTileButtonStyle;

		// Token: 0x04001989 RID: 6537
		private Button.ButtonStyle _adventureTileButtonSelectedStyle;

		// Token: 0x0400198A RID: 6538
		private Button.ButtonStyle _creativeTileButtonSelectedStyle;

		// Token: 0x0400198B RID: 6539
		private string _adventureIconPath;

		// Token: 0x0400198C RID: 6540
		private string _adventureIconSelectedPath;

		// Token: 0x0400198D RID: 6541
		private TextField _worldNameSettingInput;

		// Token: 0x0400198E RID: 6542
		private TextButton _saveWorldSettingsButton;

		// Token: 0x0400198F RID: 6543
		private TextButton _createWorldSettingsButton;

		// Token: 0x04001990 RID: 6544
		private CheckBox _flatWorldSettingCheckbox;

		// Token: 0x04001991 RID: 6545
		private CheckBox _npcsSettingCheckbox;

		// Token: 0x04001992 RID: 6546
		private Group _creativeOptions;

		// Token: 0x04001993 RID: 6547
		private Group _worldDirectoryOptions;

		// Token: 0x04001994 RID: 6548
		private Group _worldSettingsImage;

		// Token: 0x04001995 RID: 6549
		private Label _worldSettingsModeName;

		// Token: 0x04001996 RID: 6550
		private Label _worldSettingsModeDescription;

		// Token: 0x04001997 RID: 6551
		private readonly ModalDialog.DialogSetup _errorDialogSetup;

		// Token: 0x04001998 RID: 6552
		private BackButton _backButton;

		// Token: 0x04001999 RID: 6553
		private GameMode _gameMode = 0;

		// Token: 0x0400199A RID: 6554
		private string _editingWorld;
	}
}
