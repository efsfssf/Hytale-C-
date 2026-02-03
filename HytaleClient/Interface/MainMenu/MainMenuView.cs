using System;
using System.Diagnostics;
using HytaleClient.Application;
using HytaleClient.Interface.MainMenu.Pages;
using HytaleClient.Interface.MainMenu.Pages.MyAvatar;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Interface.UI.Styles;
using SDL2;

namespace HytaleClient.Interface.MainMenu
{
	// Token: 0x02000818 RID: 2072
	internal class MainMenuView : InterfaceComponent
	{
		// Token: 0x0600396C RID: 14700 RVA: 0x0007BCA4 File Offset: 0x00079EA4
		public MainMenuView(Interface @interface) : base(@interface, null)
		{
			this.MainMenu = @interface.App.MainMenu;
			this._pageContainer = new Group(this.Desktop, this);
			this.HomePage = new HomePage(this);
			this.AdventurePage = new AdventurePage(this);
			this.MinigamesPage = new MinigamesPage(this);
			this.ServersPage = new ServersPage(this);
			this.MyAvatarPage = new MyAvatarPage(this);
			this.SettingsPage = new SettingsPage(this);
			this.SharedSinglePlayerPage = new SharedSinglePlayerPage(this);
			this.TopBar = new TopBarComponent(this);
		}

		// Token: 0x0600396D RID: 14701 RVA: 0x0007BD40 File Offset: 0x00079F40
		public void Build()
		{
			this.TopBar.Build();
			this.HomePage.Build();
			this.AdventurePage.Build();
			this.MinigamesPage.Build();
			this.ServersPage.Build();
			this.MyAvatarPage.Build();
			this.SettingsPage.Build();
			this.SharedSinglePlayerPage.Build();
			Document document;
			this.Interface.TryGetDocument("MainMenu/Common.ui", out document);
			this._dismissalSound = document.ResolveNamedValue<SoundStyle>(this.Interface, "DismissalSound");
			bool isMounted = base.IsMounted;
			if (isMounted)
			{
				this.OnPageChanged();
			}
		}

		// Token: 0x0600396E RID: 14702 RVA: 0x0007BDE9 File Offset: 0x00079FE9
		protected override void OnUnmounted()
		{
			this.MinigamesPage.DisposeGameTextures();
		}

		// Token: 0x0600396F RID: 14703 RVA: 0x0007BDF8 File Offset: 0x00079FF8
		protected internal override void OnKeyDown(SDL.SDL_Keycode keycode, int repeat)
		{
			bool flag = this.Interface.App.MainMenu.CurrentPage == AppMainMenu.MainMenuPage.Servers && this.Desktop.IsShortcutKeyDown && keycode == SDL.SDL_Keycode.SDLK_f;
			if (flag)
			{
				this.ServersPage.FocusTextSearchInput();
			}
			else
			{
				base.OnKeyDown(keycode, repeat);
			}
		}

		// Token: 0x06003970 RID: 14704 RVA: 0x0007BE50 File Offset: 0x0007A050
		public void ShowTopBar(bool showTopBar)
		{
			this.TopBar.Visible = showTopBar;
			base.Layout(null, true);
		}

		// Token: 0x06003971 RID: 14705 RVA: 0x0007BE7C File Offset: 0x0007A07C
		public void OpenUnsavedCharacterChangesModal(Action onContinue)
		{
			this.Interface.ModalDialog.Setup(new ModalDialog.DialogSetup
			{
				Title = "ui.myAvatar.unsavedChanges.title",
				Text = "ui.myAvatar.unsavedChanges.text",
				ConfirmationText = "ui.general.save",
				CancelText = "ui.general.discardChanges",
				Dismissable = false,
				OnConfirm = delegate()
				{
					this.MainMenu.SaveCharacter();
					onContinue();
				},
				OnCancel = onContinue
			});
			this.Desktop.SetLayer(4, this.Interface.ModalDialog);
		}

		// Token: 0x06003972 RID: 14706 RVA: 0x0007BF20 File Offset: 0x0007A120
		public void OpenAssetEditorMissingPathDialog()
		{
			this.Interface.ModalDialog.Setup(new ModalDialog.DialogSetup
			{
				Title = "ui.mainMenu.assetEditor.errorModal.missingDirectory.title",
				Text = "ui.mainMenu.assetEditor.errorModal.missingDirectory.title",
				ConfirmationText = "ui.mainMenu.assetEditor.errorModal.openSettingsButton",
				Dismissable = true,
				OnConfirm = delegate()
				{
					this.MainMenu.Open(AppMainMenu.MainMenuPage.Settings);
				}
			});
			this.Desktop.SetLayer(4, this.Interface.ModalDialog);
		}

		// Token: 0x06003973 RID: 14707 RVA: 0x0007BF98 File Offset: 0x0007A198
		public void OpenAssetEditorInvalidPathDialog()
		{
			this.Interface.ModalDialog.Setup(new ModalDialog.DialogSetup
			{
				Title = "ui.mainMenu.assetEditor.errorModal.invalidDirectory.title",
				Text = "ui.mainMenu.assetEditor.errorModal.invalidDirectory.text",
				ConfirmationText = "ui.mainMenu.assetEditor.errorModal.openSettingsButton",
				Dismissable = true,
				OnConfirm = delegate()
				{
					this.MainMenu.Open(AppMainMenu.MainMenuPage.Settings);
				}
			});
			this.Desktop.SetLayer(4, this.Interface.ModalDialog);
		}

		// Token: 0x06003974 RID: 14708 RVA: 0x0007C010 File Offset: 0x0007A210
		public void OnPageChanged()
		{
			Debug.Assert(base.IsMounted);
			InterfaceComponent interfaceComponent = null;
			switch (this.Interface.App.MainMenu.CurrentPage)
			{
			case AppMainMenu.MainMenuPage.Home:
				interfaceComponent = this.HomePage;
				break;
			case AppMainMenu.MainMenuPage.Servers:
				interfaceComponent = this.ServersPage;
				break;
			case AppMainMenu.MainMenuPage.Minigames:
				interfaceComponent = this.MinigamesPage;
				break;
			case AppMainMenu.MainMenuPage.Adventure:
				interfaceComponent = this.AdventurePage;
				break;
			case AppMainMenu.MainMenuPage.MyAvatar:
				interfaceComponent = this.MyAvatarPage;
				break;
			case AppMainMenu.MainMenuPage.Settings:
				interfaceComponent = this.SettingsPage;
				break;
			case AppMainMenu.MainMenuPage.SharedSinglePlayer:
				interfaceComponent = this.SharedSinglePlayerPage;
				break;
			}
			bool flag = this._pageContainer.Children.Count == 1 && interfaceComponent == this._pageContainer.Children[0];
			if (!flag)
			{
				this._pageContainer.Clear();
				this._pageContainer.Add(interfaceComponent, -1);
				base.Layout(null, true);
				bool isMounted = this.TopBar.IsMounted;
				if (isMounted)
				{
					this.TopBar.OnPageChanged();
				}
				bool flag2 = this.Interface.App.MainMenu.CurrentPage == AppMainMenu.MainMenuPage.Home || this.Interface.App.MainMenu.CurrentPage == AppMainMenu.MainMenuPage.MyAvatar;
				if (flag2)
				{
					this.Interface.App.MainMenu.ResetCharacters();
				}
			}
		}

		// Token: 0x06003975 RID: 14709 RVA: 0x0007C178 File Offset: 0x0007A378
		protected internal override void Dismiss()
		{
			SoundStyle dismissalSound = this._dismissalSound;
			bool flag = ((dismissalSound != null) ? dismissalSound.SoundPath : null) != null;
			if (flag)
			{
				this.Interface.PlaySound(this._dismissalSound);
			}
			bool flag2 = this.Interface.App.MainMenu.CurrentPage == AppMainMenu.MainMenuPage.Adventure;
			if (flag2)
			{
				this.AdventurePage.Dismiss();
			}
			else
			{
				bool flag3 = this.Interface.App.MainMenu.CurrentPage == AppMainMenu.MainMenuPage.Settings;
				if (flag3)
				{
					this.SettingsPage.Dismiss();
				}
				else
				{
					bool flag4 = this.Interface.App.MainMenu.CurrentPage == AppMainMenu.MainMenuPage.MyAvatar && this.Interface.App.MainMenu.HasUnsavedSkinChanges();
					if (flag4)
					{
						this.OpenUnsavedCharacterChangesModal(delegate
						{
							this.Interface.App.MainMenu.Open(AppMainMenu.MainMenuPage.Home);
						});
					}
					else
					{
						bool flag5 = this.Interface.App.MainMenu.CurrentPage > AppMainMenu.MainMenuPage.Home;
						if (flag5)
						{
							this.Interface.App.MainMenu.Open(AppMainMenu.MainMenuPage.Home);
						}
					}
				}
			}
		}

		// Token: 0x04001958 RID: 6488
		private readonly Group _pageContainer;

		// Token: 0x04001959 RID: 6489
		public readonly HomePage HomePage;

		// Token: 0x0400195A RID: 6490
		public readonly AdventurePage AdventurePage;

		// Token: 0x0400195B RID: 6491
		public readonly MinigamesPage MinigamesPage;

		// Token: 0x0400195C RID: 6492
		public readonly ServersPage ServersPage;

		// Token: 0x0400195D RID: 6493
		public readonly MyAvatarPage MyAvatarPage;

		// Token: 0x0400195E RID: 6494
		public readonly SettingsPage SettingsPage;

		// Token: 0x0400195F RID: 6495
		public readonly SharedSinglePlayerPage SharedSinglePlayerPage;

		// Token: 0x04001960 RID: 6496
		public readonly TopBarComponent TopBar;

		// Token: 0x04001961 RID: 6497
		internal readonly AppMainMenu MainMenu;

		// Token: 0x04001962 RID: 6498
		private SoundStyle _dismissalSound;
	}
}
