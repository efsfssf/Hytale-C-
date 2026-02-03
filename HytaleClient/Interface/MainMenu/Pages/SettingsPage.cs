using System;
using HytaleClient.Application;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;

namespace HytaleClient.Interface.MainMenu.Pages
{
	// Token: 0x02000820 RID: 2080
	internal class SettingsPage : InterfaceComponent
	{
		// Token: 0x060039DD RID: 14813 RVA: 0x0007FF3E File Offset: 0x0007E13E
		public SettingsPage(MainMenuView mainMenuView) : base(mainMenuView.Interface, null)
		{
			this.MainMenuView = mainMenuView;
		}

		// Token: 0x060039DE RID: 14814 RVA: 0x0007FF58 File Offset: 0x0007E158
		public void Build()
		{
			bool isMounted = base.IsMounted;
			if (isMounted)
			{
				this._container.Remove(this.MainMenuView.Interface.SettingsComponent);
			}
			base.Clear();
			Document document;
			this.Interface.TryGetDocument("MainMenu/SettingsPage.ui", out document);
			UIFragment uifragment = document.Instantiate(this.Desktop, this);
			this._container = uifragment.Get<Group>("Container");
			bool isMounted2 = base.IsMounted;
			if (isMounted2)
			{
				this._container.Add(this.MainMenuView.Interface.SettingsComponent, -1);
			}
		}

		// Token: 0x060039DF RID: 14815 RVA: 0x0007FFEC File Offset: 0x0007E1EC
		protected override void OnMounted()
		{
			this.MainMenuView.ShowTopBar(true);
			this._container.Add(this.MainMenuView.Interface.SettingsComponent, -1);
		}

		// Token: 0x060039E0 RID: 14816 RVA: 0x00080019 File Offset: 0x0007E219
		protected override void OnUnmounted()
		{
			this._container.Remove(this.MainMenuView.Interface.SettingsComponent);
		}

		// Token: 0x060039E1 RID: 14817 RVA: 0x00080038 File Offset: 0x0007E238
		protected internal override void Dismiss()
		{
			this.Interface.App.Settings.Save();
			this.Interface.App.MainMenu.Open(AppMainMenu.MainMenuPage.Home);
		}

		// Token: 0x040019D1 RID: 6609
		public readonly MainMenuView MainMenuView;

		// Token: 0x040019D2 RID: 6610
		private Group _container;
	}
}
