using System;
using HytaleClient.Interface.Common;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;

namespace HytaleClient.Interface.InGame.Overlays
{
	// Token: 0x020008AC RID: 2220
	internal class InGameMenuOverlay : InterfaceComponent
	{
		// Token: 0x06004046 RID: 16454 RVA: 0x000B8819 File Offset: 0x000B6A19
		public InGameMenuOverlay(InGameView inGameView) : base(inGameView.Interface, null)
		{
			this.InGameView = inGameView;
		}

		// Token: 0x06004047 RID: 16455 RVA: 0x000B8838 File Offset: 0x000B6A38
		public void Build()
		{
			bool areSettingsOpen = this._areSettingsOpen;
			if (areSettingsOpen)
			{
				this._settingsContainer.Remove(this.InGameView.Interface.SettingsComponent);
			}
			base.Clear();
			Document document;
			this.Interface.TryGetDocument("InGame/Overlays/MenuOverlay.ui", out document);
			UIFragment uifragment = document.Instantiate(this.Desktop, this);
			uifragment.Get<TextButton>("ReturnToGame").Activating = delegate()
			{
				this.InGameView.InGame.TryClosePageOrOverlay();
			};
			uifragment.Get<TextButton>("OpenSettings").Activating = new Action(this.ShowSettings);
			uifragment.Get<TextButton>("BackToMainMenu").Activating = delegate()
			{
				this.InGameView.InGame.RequestExit(false);
			};
			uifragment.Get<TextButton>("BackToDesktop").Activating = delegate()
			{
				this.InGameView.InGame.RequestExit(true);
			};
			this._settingsOverlay = uifragment.Get<Group>("SettingsOverlay");
			this._settingsOverlay.Find<Group>("SettingsBackButtonContainer").Add(new BackButton(this.Interface, new Action(this.Dismiss)), -1);
			this._settingsContainer = uifragment.Get<Group>("SettingsContainer");
			bool areSettingsOpen2 = this._areSettingsOpen;
			if (areSettingsOpen2)
			{
				this._settingsContainer.Add(this.InGameView.Interface.SettingsComponent, -1);
			}
			else
			{
				base.Remove(this._settingsOverlay);
			}
			bool isMounted = base.IsMounted;
			if (isMounted)
			{
				this.Interface.SocialBar.SetContainer(base.Find<Group>("SocialBarContainer"));
			}
		}

		// Token: 0x06004048 RID: 16456 RVA: 0x000B89B7 File Offset: 0x000B6BB7
		protected override void OnMounted()
		{
			this.InGameView.InGame.SetSceneBlurEnabled(true);
			this.Interface.SocialBar.SetContainer(base.Find<Group>("SocialBarContainer"));
		}

		// Token: 0x06004049 RID: 16457 RVA: 0x000B89E8 File Offset: 0x000B6BE8
		protected override void OnUnmounted()
		{
			this.InGameView.InGame.SetSceneBlurEnabled(false);
		}

		// Token: 0x0600404A RID: 16458 RVA: 0x000B8A00 File Offset: 0x000B6C00
		protected internal override void Dismiss()
		{
			bool areSettingsOpen = this._areSettingsOpen;
			if (areSettingsOpen)
			{
				this.CloseSettings();
			}
			else
			{
				this.InGameView.InGame.TryClosePageOrOverlay();
			}
		}

		// Token: 0x0600404B RID: 16459 RVA: 0x000B8A34 File Offset: 0x000B6C34
		public void ShowSettings()
		{
			this._areSettingsOpen = true;
			this._settingsContainer.Add(this.InGameView.Interface.SettingsComponent, -1);
			base.Add(this._settingsOverlay, -1);
			base.Layout(null, true);
		}

		// Token: 0x0600404C RID: 16460 RVA: 0x000B8A88 File Offset: 0x000B6C88
		private void CloseSettings()
		{
			this._areSettingsOpen = false;
			this._settingsContainer.Remove(this.InGameView.Interface.SettingsComponent);
			base.Remove(this._settingsOverlay);
			base.Layout(null, true);
		}

		// Token: 0x04001EA4 RID: 7844
		public readonly InGameView InGameView;

		// Token: 0x04001EA5 RID: 7845
		private Group _settingsOverlay;

		// Token: 0x04001EA6 RID: 7846
		private Group _settingsContainer;

		// Token: 0x04001EA7 RID: 7847
		private bool _areSettingsOpen = false;
	}
}
