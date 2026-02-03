using System;
using System.IO;
using HytaleClient.AssetEditor.Interface.Editor;
using HytaleClient.AssetEditor.Interface.MainMenu;
using HytaleClient.Interface;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Utils;

namespace HytaleClient.AssetEditor.Interface
{
	// Token: 0x02000B95 RID: 2965
	internal class AssetEditorInterface : BaseInterface
	{
		// Token: 0x06005B96 RID: 23446 RVA: 0x001CA7EC File Offset: 0x001C89EC
		public AssetEditorInterface(AssetEditorApp app, bool isDevMode) : base(app.Engine, app.Fonts, app.CoUIManager, Path.Combine(Paths.EditorData, "Interface"), isDevMode)
		{
			this.App = app;
			this.AssetEditor = new AssetEditorOverlay(this, this.Desktop);
			this.MainMenuView = new AssetEditorMainMenuView(this);
			this.StartupView = new AssetEditorStartupView(this.Desktop, null);
			this.Notifications = new ToastNotifications(this.Desktop, null);
			this.SettingsModal = new SettingsModal(this);
		}

		// Token: 0x06005B97 RID: 23447 RVA: 0x001CA878 File Offset: 0x001C8A78
		public void OnWindowFocusChanged()
		{
			this.AssetEditor.OnWindowFocusChanged();
		}

		// Token: 0x06005B98 RID: 23448 RVA: 0x001CA888 File Offset: 0x001C8A88
		public void OnAppStageChanged()
		{
			Element element;
			switch (this.App.Stage)
			{
			case AssetEditorApp.AppStage.Startup:
				element = this.StartupView;
				break;
			case AssetEditorApp.AppStage.MainMenu:
				element = this.MainMenuView;
				break;
			case AssetEditorApp.AppStage.Editor:
				element = this.AssetEditor;
				break;
			default:
				throw new NotSupportedException();
			}
			bool flag = this._currentView == element;
			if (!flag)
			{
				this.Desktop.ClearAllLayers();
				this.Desktop.SetLayer(0, element);
				Element parent = this.Notifications.Parent;
				if (parent != null)
				{
					parent.Remove(this.Notifications);
				}
				element.Add(this.Notifications, -1);
				this._currentView = element;
				this._currentView.Layout(null, true);
			}
		}

		// Token: 0x06005B99 RID: 23449 RVA: 0x001CA94D File Offset: 0x001C8B4D
		protected override void Build()
		{
			this.AssetEditor.Build();
			this.MainMenuView.Build();
			this.SettingsModal.Build();
		}

		// Token: 0x04003943 RID: 14659
		public readonly AssetEditorOverlay AssetEditor;

		// Token: 0x04003944 RID: 14660
		public readonly AssetEditorStartupView StartupView;

		// Token: 0x04003945 RID: 14661
		public readonly AssetEditorMainMenuView MainMenuView;

		// Token: 0x04003946 RID: 14662
		public readonly SettingsModal SettingsModal;

		// Token: 0x04003947 RID: 14663
		public readonly AssetEditorApp App;

		// Token: 0x04003948 RID: 14664
		public readonly ToastNotifications Notifications;

		// Token: 0x04003949 RID: 14665
		private Element _currentView;
	}
}
