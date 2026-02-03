using System;
using System.Collections.Generic;
using HytaleClient.Application;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Math;

namespace HytaleClient.Interface.MainMenu
{
	// Token: 0x0200081B RID: 2075
	internal class TopBarComponent : InterfaceComponent
	{
		// Token: 0x1700100C RID: 4108
		// (get) Token: 0x06003984 RID: 14724 RVA: 0x0007C5F8 File Offset: 0x0007A7F8
		// (set) Token: 0x06003985 RID: 14725 RVA: 0x0007C600 File Offset: 0x0007A800
		public Element SelectedMarker { get; private set; }

		// Token: 0x1700100D RID: 4109
		// (get) Token: 0x06003986 RID: 14726 RVA: 0x0007C609 File Offset: 0x0007A809
		// (set) Token: 0x06003987 RID: 14727 RVA: 0x0007C611 File Offset: 0x0007A811
		public Element HoverMarker { get; private set; }

		// Token: 0x06003988 RID: 14728 RVA: 0x0007C61A File Offset: 0x0007A81A
		public TopBarComponent(MainMenuView mainMenuView) : base(mainMenuView.Interface, mainMenuView)
		{
			this.MainMenuView = mainMenuView;
		}

		// Token: 0x06003989 RID: 14729 RVA: 0x0007C640 File Offset: 0x0007A840
		public void Build()
		{
			TopBarComponent.<>c__DisplayClass16_0 CS$<>8__locals1 = new TopBarComponent.<>c__DisplayClass16_0();
			CS$<>8__locals1.<>4__this = this;
			base.Clear();
			Document document;
			this.Interface.TryGetDocument("MainMenu/TopBar/TopBar.ui", out document);
			this._buttonStyle = document.ResolveNamedValue<TextButton.TextButtonStyle>(this.Desktop.Provider, "ButtonStyle");
			this._buttonSelectedStyle = document.ResolveNamedValue<TextButton.TextButtonStyle>(this.Desktop.Provider, "ButtonSelectedStyle");
			CS$<>8__locals1.fragment = document.Instantiate(this.Desktop, this);
			CS$<>8__locals1.app = this.Interface.App;
			CS$<>8__locals1.fragment.Get<Button>("BackButton").Activating = delegate()
			{
				CS$<>8__locals1.app.MainMenu.Open(AppMainMenu.MainMenuPage.Home);
			};
			this.SelectedMarker = CS$<>8__locals1.fragment.Get<Element>("SelectedMarker");
			this.HoverMarker = CS$<>8__locals1.fragment.Get<Element>("HoverMarker");
			this._buttonMapping.Clear();
			CS$<>8__locals1.<Build>g__MakeButton|1("NavigationItemAdventure", AppMainMenu.MainMenuPage.Adventure);
			CS$<>8__locals1.<Build>g__MakeButton|1("NavigationItemMinigames", AppMainMenu.MainMenuPage.Minigames);
			CS$<>8__locals1.<Build>g__MakeButton|1("NavigationItemServers", AppMainMenu.MainMenuPage.Servers);
			CS$<>8__locals1.<Build>g__MakeButton|1("NavigationItemSharedSinglePlayer", AppMainMenu.MainMenuPage.SharedSinglePlayer);
			CS$<>8__locals1.<Build>g__MakeButton|1("NavigationItemAvatar", AppMainMenu.MainMenuPage.MyAvatar);
			CS$<>8__locals1.<Build>g__MakeButton|1("NavigationItemSettings", AppMainMenu.MainMenuPage.Settings);
			bool isMounted = base.IsMounted;
			if (isMounted)
			{
				this.Interface.SocialBar.SetContainer(base.Find<Group>("SocialBarContainer"));
				base.Layout(null, true);
				this._mustInitializeMarker = true;
				this.OnPageChanged();
			}
		}

		// Token: 0x0600398A RID: 14730 RVA: 0x0007C7C4 File Offset: 0x0007A9C4
		private void OpenPage(AppMainMenu.MainMenuPage page, bool confirmed = false)
		{
			bool flag = !confirmed && this.MainMenuView.MainMenu.HasUnsavedSkinChanges();
			if (flag)
			{
				this.MainMenuView.OpenUnsavedCharacterChangesModal(delegate
				{
					this.OpenPage(page, true);
				});
			}
			else
			{
				this.MainMenuView.MainMenu.Open(page);
			}
		}

		// Token: 0x0600398B RID: 14731 RVA: 0x0007C832 File Offset: 0x0007AA32
		protected override void OnMounted()
		{
			this.Interface.SocialBar.SetContainer(base.Find<Group>("SocialBarContainer"));
			this.Desktop.RegisterAnimationCallback(new Action<float>(this.Animate));
			this._mustInitializeMarker = true;
		}

		// Token: 0x0600398C RID: 14732 RVA: 0x0007C870 File Offset: 0x0007AA70
		protected override void OnUnmounted()
		{
			this.Desktop.UnregisterAnimationCallback(new Action<float>(this.Animate));
		}

		// Token: 0x0600398D RID: 14733 RVA: 0x0007C88C File Offset: 0x0007AA8C
		public void OnPageChanged()
		{
			App app = this.Interface.App;
			bool flag = !this._buttonMapping.ContainsKey(app.MainMenu.CurrentPage);
			if (!flag)
			{
				foreach (TextButton textButton in this._buttonMapping.Values)
				{
					textButton.Style = this._buttonStyle;
				}
				TextButton textButton2 = this._buttonMapping[app.MainMenu.CurrentPage];
				textButton2.Style = this._buttonSelectedStyle;
				Rectangle anchoredRectangle = textButton2.AnchoredRectangle;
				this._selectedNavigationItemMarkerTargetPosition = (int)((float)anchoredRectangle.Left / this.Desktop.Scale - (float)this.SelectedMarker.AnchoredRectangle.Width / this.Desktop.Scale / 2f + (float)anchoredRectangle.Width / 2f / this.Desktop.Scale) - (int)((float)this.SelectedMarker.Parent.AnchoredRectangle.Left / this.Desktop.Scale);
				bool mustInitializeMarker = this._mustInitializeMarker;
				if (mustInitializeMarker)
				{
					this.SelectedMarker.Anchor.Left = new int?(this._selectedNavigationItemMarkerLerpPosition = this._selectedNavigationItemMarkerTargetPosition);
					this._mustInitializeMarker = false;
				}
				base.Layout(null, true);
			}
		}

		// Token: 0x0600398E RID: 14734 RVA: 0x0007CA18 File Offset: 0x0007AC18
		public void UpdateHoveredNavigationItem(TextButton topBarButtonComponent)
		{
			bool flag = topBarButtonComponent == null;
			if (flag)
			{
				this.HoverMarker.Visible = false;
			}
			else
			{
				int num = (int)((float)topBarButtonComponent.AnchoredRectangle.Left / this.Desktop.Scale);
				int num2 = (int)((float)topBarButtonComponent.AnchoredRectangle.Width / this.Desktop.Scale / 2f);
				int num3 = (int)((float)this.HoverMarker.Parent.AnchoredRectangle.Left / this.Desktop.Scale);
				int num4 = this.HoverMarker.Anchor.Width.Value / 2;
				this.HoverMarker.Anchor.Left = new int?(num - num4 + num2 - num3);
				this.HoverMarker.Visible = true;
				base.Layout(null, true);
			}
		}

		// Token: 0x0600398F RID: 14735 RVA: 0x0007CB00 File Offset: 0x0007AD00
		private void Animate(float deltaTime)
		{
			bool flag = this._selectedNavigationItemMarkerLerpPosition != this._selectedNavigationItemMarkerTargetPosition;
			if (flag)
			{
				this._selectedNavigationItemMarkerLerpPosition = (int)MathHelper.Lerp((float)this._selectedNavigationItemMarkerLerpPosition, (float)this._selectedNavigationItemMarkerTargetPosition, Math.Min(deltaTime * 10f, 1f));
				this.SelectedMarker.Anchor.Left = new int?(this._selectedNavigationItemMarkerLerpPosition);
				this.SelectedMarker.Layout(null, true);
			}
		}

		// Token: 0x0400196B RID: 6507
		public readonly MainMenuView MainMenuView;

		// Token: 0x0400196E RID: 6510
		private int _selectedNavigationItemMarkerTargetPosition;

		// Token: 0x0400196F RID: 6511
		private int _selectedNavigationItemMarkerLerpPosition;

		// Token: 0x04001970 RID: 6512
		private bool _mustInitializeMarker;

		// Token: 0x04001971 RID: 6513
		private TextButton.TextButtonStyle _buttonStyle;

		// Token: 0x04001972 RID: 6514
		private TextButton.TextButtonStyle _buttonSelectedStyle;

		// Token: 0x04001973 RID: 6515
		private Dictionary<AppMainMenu.MainMenuPage, TextButton> _buttonMapping = new Dictionary<AppMainMenu.MainMenuPage, TextButton>();
	}
}
