using System;
using HytaleClient.Application;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Utils;

namespace HytaleClient.Interface.MainMenu.Pages
{
	// Token: 0x0200081D RID: 2077
	internal class HomePage : InterfaceComponent
	{
		// Token: 0x060039AD RID: 14765 RVA: 0x0007E3AE File Offset: 0x0007C5AE
		public HomePage(MainMenuView mainMenuView) : base(mainMenuView.Interface, null)
		{
			this.MainMenuView = mainMenuView;
		}

		// Token: 0x060039AE RID: 14766 RVA: 0x0007E3C8 File Offset: 0x0007C5C8
		public void Build()
		{
			HomePage.<>c__DisplayClass2_0 CS$<>8__locals1 = new HomePage.<>c__DisplayClass2_0();
			CS$<>8__locals1.<>4__this = this;
			base.Clear();
			Document document;
			this.Interface.TryGetDocument("MainMenu/HomePage.ui", out document);
			CS$<>8__locals1.fragment = document.Instantiate(this.Desktop, this);
			CS$<>8__locals1.<Build>g__SetupButton|0("AdventureButton", delegate
			{
				CS$<>8__locals1.<>4__this.Interface.App.MainMenu.Open(AppMainMenu.MainMenuPage.Adventure);
			}, 15);
			CS$<>8__locals1.<Build>g__SetupButton|0("MinigamesButton", delegate
			{
				CS$<>8__locals1.<>4__this.Interface.App.MainMenu.Open(AppMainMenu.MainMenuPage.Minigames);
			}, 15);
			CS$<>8__locals1.<Build>g__SetupButton|0("ServersButton", delegate
			{
				CS$<>8__locals1.<>4__this.Interface.App.MainMenu.Open(AppMainMenu.MainMenuPage.Servers);
			}, 15);
			CS$<>8__locals1.<Build>g__SetupButton|0("SharedSinglePlayerButton", delegate
			{
				CS$<>8__locals1.<>4__this.Interface.App.MainMenu.Open(AppMainMenu.MainMenuPage.SharedSinglePlayer);
			}, 15);
			CS$<>8__locals1.<Build>g__SetupButton|0("AvatarButton", delegate
			{
				CS$<>8__locals1.<>4__this.Interface.App.MainMenu.Open(AppMainMenu.MainMenuPage.MyAvatar);
			}, 15);
			CS$<>8__locals1.<Build>g__SetupButton|0("SettingsButton", delegate
			{
				CS$<>8__locals1.<>4__this.Interface.App.MainMenu.Open(AppMainMenu.MainMenuPage.Settings);
			}, 9);
			CS$<>8__locals1.<Build>g__SetupButton|0("QuitButton", new Action(this.OnQuit), 9);
			CS$<>8__locals1.fragment.Get<Label>("Version").Text = ((BuildInfo.RevisionId != null) ? (BuildInfo.Version + " Rev. " + BuildInfo.RevisionId) : BuildInfo.Version);
			bool isMounted = base.IsMounted;
			if (isMounted)
			{
				this.Interface.SocialBar.SetContainer(base.Find<Group>("SocialBarContainer"));
			}
		}

		// Token: 0x060039AF RID: 14767 RVA: 0x0007E525 File Offset: 0x0007C725
		protected override void OnMounted()
		{
			this.MainMenuView.ShowTopBar(false);
			this.Interface.SocialBar.SetContainer(base.Find<Group>("SocialBarContainer"));
		}

		// Token: 0x060039B0 RID: 14768 RVA: 0x0007E551 File Offset: 0x0007C751
		private void OnQuit()
		{
			Interface.Logger.Info("User closed game from user interface");
			this.Interface.App.Exit();
		}

		// Token: 0x0400199B RID: 6555
		public readonly MainMenuView MainMenuView;
	}
}
