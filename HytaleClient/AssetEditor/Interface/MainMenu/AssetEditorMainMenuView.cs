using System;
using System.Collections.Generic;
using HytaleClient.AssetEditor.Interface.Modals;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;

namespace HytaleClient.AssetEditor.Interface.MainMenu
{
	// Token: 0x02000BA3 RID: 2979
	internal class AssetEditorMainMenuView : Group
	{
		// Token: 0x06005C39 RID: 23609 RVA: 0x001CFEF0 File Offset: 0x001CE0F0
		public AssetEditorMainMenuView(AssetEditorInterface @interface) : base(@interface.Desktop, null)
		{
			this.Interface = @interface;
			this._serverModal = new ServerModal(@interface);
			this._confirmationModal = new ConfirmationModal(this.Desktop, null);
		}

		// Token: 0x06005C3A RID: 23610 RVA: 0x001CFF30 File Offset: 0x001CE130
		public void Build()
		{
			base.Clear();
			Document document;
			this.Interface.TryGetDocument("MainMenu/ServerButton.ui", out document);
			this._buttonStyle = document.ResolveNamedValue<TextButton.TextButtonStyle>(this.Interface, "Style");
			this._buttonStyleSelected = document.ResolveNamedValue<TextButton.TextButtonStyle>(this.Interface, "StyleSelected");
			Document document2;
			this.Interface.TryGetDocument("MainMenu/MainMenu.ui", out document2);
			UIFragment uifragment = document2.Instantiate(this.Desktop, this);
			this._cosmeticsEditorButton = uifragment.Get<TextButton>("CosmeticsEditorButton");
			this._cosmeticsEditorButton.Activating = delegate()
			{
				this.Interface.App.Editor.OpenCosmeticsEditor();
			};
			this._serverList = uifragment.Get<Group>("ServerList");
			this._connectionStatusLabel = uifragment.Get<Label>("ConnectionStatusLabel");
			this._connectionStatusLabel.Visible = false;
			this._connectionErrorLabel = uifragment.Get<Label>("ConnectionErrorLabel");
			this._connectionErrorLabel.Visible = false;
			this._cancelConnectionButton = uifragment.Get<TextButton>("CancelConnectionButton");
			this._cancelConnectionButton.Visible = false;
			this._cancelConnectionButton.Activating = delegate()
			{
				this.Interface.App.MainMenu.CancelConnection();
			};
			this._disconnectPopup = uifragment.Get<Group>("DisconnectPopup");
			this._disconnectPopup.Visible = false;
			this._disconnectReason = uifragment.Get<Label>("DisconnectReason");
			this._reconnectButton = uifragment.Get<TextButton>("ReconnectButton");
			this._reconnectButton.Activating = delegate()
			{
				bool isConnectingToServer = this.Interface.App.MainMenu.IsConnectingToServer;
				if (!isConnectingToServer)
				{
					this.Interface.App.MainMenu.Reconnect();
				}
			};
			this._closeDisconnectPopupButton = uifragment.Get<TextButton>("CloseDisconnectPopupButton");
			this._closeDisconnectPopupButton.Activating = delegate()
			{
				this.Interface.App.MainMenu.CloseDisconnectPopup();
			};
			uifragment.Get<TextButton>("SettingsButton").Activating = delegate()
			{
				this.Interface.SettingsModal.Open();
			};
			this._addServerButton = uifragment.Get<TextButton>("AddServerButton");
			this._addServerButton.Activating = delegate()
			{
				this._serverModal.Open();
			};
			this._editServerButton = uifragment.Get<TextButton>("EditServerButton");
			this._editServerButton.Visible = false;
			this._editServerButton.Activating = delegate()
			{
				this._serverModal.Open(this._selectedServerIndex, this.Interface.App.MainMenu.Servers[this._selectedServerIndex]);
			};
			this._removeServerButton = uifragment.Get<TextButton>("RemoveServerButton");
			this._removeServerButton.Visible = false;
			this._removeServerButton.Activating = delegate()
			{
				string text = this.Desktop.Provider.GetText("ui.assetEditor.mainMenu.removeServerModal.title", new Dictionary<string, string>
				{
					{
						"serverName",
						this.Interface.App.MainMenu.Servers[this._selectedServerIndex].Name
					}
				}, true);
				this._confirmationModal.Open(text, "", delegate
				{
					this.Interface.App.MainMenu.RemoveServer(this._selectedServerIndex);
				}, null, this.Desktop.Provider.GetText("ui.general.remove", null, true), null, false);
			};
			uifragment.Get<Element>("CosmeticsEditorButton").Visible = true;
			uifragment.Get<Element>("CosmeticsEditorButtonSeparator").Visible = true;
			this.BuildServerList(false);
			this._serverModal.Build();
			this._confirmationModal.Build();
		}

		// Token: 0x06005C3B RID: 23611 RVA: 0x001D01C4 File Offset: 0x001CE3C4
		protected override void OnMounted()
		{
			bool hasMarkupError = this.Interface.HasMarkupError;
			if (!hasMarkupError)
			{
				this.BuildServerList(false);
				this.UpdateEditButtons(false);
			}
		}

		// Token: 0x06005C3C RID: 23612 RVA: 0x001D01F3 File Offset: 0x001CE3F3
		protected override void OnUnmounted()
		{
			this.UpdateDisconnectPopup();
			this.UpdateConnectionStatus(true);
			this._selectedServerIndex = -1;
		}

		// Token: 0x06005C3D RID: 23613 RVA: 0x001D020C File Offset: 0x001CE40C
		public void BuildServerList(bool doLayout = true)
		{
			this._selectedServerIndex = -1;
			Document document;
			this.Interface.TryGetDocument("MainMenu/ServerButton.ui", out document);
			IReadOnlyList<AssetEditorAppMainMenu.Server> servers = this.Interface.App.MainMenu.Servers;
			this._serverList.Clear();
			for (int i = 0; i < servers.Count; i++)
			{
				int index = i;
				AssetEditorAppMainMenu.Server server = servers[index];
				UIFragment uifragment = document.Instantiate(this.Desktop, this._serverList);
				TextButton button = uifragment.Get<TextButton>("Button");
				button.Text = server.Name;
				button.Activating = delegate()
				{
					this.OnServerButtonActivating(button, index, false);
				};
				button.DoubleClicking = delegate()
				{
					this.OnServerButtonActivating(button, index, true);
					this.Interface.App.MainMenu.ConnectToServer(index);
				};
			}
			this.UpdateEditButtons(false);
			if (doLayout)
			{
				base.Layout(null, true);
			}
		}

		// Token: 0x06005C3E RID: 23614 RVA: 0x001D0320 File Offset: 0x001CE520
		private void OnServerButtonActivating(TextButton button, int index, bool forceSelect)
		{
			bool flag = this._selectedServerIndex != -1;
			if (flag)
			{
				TextButton textButton = (TextButton)this._serverList.Children[this._selectedServerIndex];
				textButton.Style = this._buttonStyle;
				textButton.Layout(null, true);
			}
			bool flag2 = !forceSelect && this._selectedServerIndex == index;
			if (flag2)
			{
				this._selectedServerIndex = -1;
				this.UpdateEditButtons(true);
			}
			else
			{
				this._selectedServerIndex = index;
				button.Style = this._buttonStyleSelected;
				button.Layout(null, true);
				this.UpdateEditButtons(true);
			}
		}

		// Token: 0x06005C3F RID: 23615 RVA: 0x001D03CC File Offset: 0x001CE5CC
		private void UpdateEditButtons(bool doLayout = true)
		{
			this._editServerButton.Visible = (this._selectedServerIndex != -1);
			this._removeServerButton.Visible = (this._selectedServerIndex != -1);
			if (doLayout)
			{
				base.Layout(null, true);
			}
		}

		// Token: 0x06005C40 RID: 23616 RVA: 0x001D0420 File Offset: 0x001CE620
		public void UpdateDisconnectPopup()
		{
			AssetEditorAppMainMenu mainMenu = this.Interface.App.MainMenu;
			this._disconnectPopup.Visible = mainMenu.DisplayDisconnectPopup;
			bool displayDisconnectPopup = mainMenu.DisplayDisconnectPopup;
			if (displayDisconnectPopup)
			{
				this._disconnectReason.Text = (mainMenu.ServerDisconnectReason ?? "");
			}
			bool isMounted = base.IsMounted;
			if (isMounted)
			{
				base.Layout(null, true);
			}
		}

		// Token: 0x06005C41 RID: 23617 RVA: 0x001D0494 File Offset: 0x001CE694
		public void UpdateConnectionStatus(bool doLayout = true)
		{
			AssetEditorAppMainMenu mainMenu = this.Interface.App.MainMenu;
			bool isConnectingToServer = mainMenu.IsConnectingToServer;
			string connectionErrorMessage = mainMenu.ConnectionErrorMessage;
			this._cosmeticsEditorButton.Disabled = isConnectingToServer;
			foreach (Element element in this._serverList.Children)
			{
				TextButton textButton = element as TextButton;
				bool flag = textButton == null;
				if (!flag)
				{
					textButton.Disabled = isConnectingToServer;
				}
			}
			this._connectionStatusLabel.Visible = isConnectingToServer;
			this._connectionErrorLabel.Visible = (!isConnectingToServer && connectionErrorMessage != null);
			this._cancelConnectionButton.Visible = isConnectingToServer;
			bool flag2 = isConnectingToServer;
			if (flag2)
			{
				bool flag3 = mainMenu.ConnectionStage == AssetEditorAppMainMenu.ConnectionStages.Authenticating;
				if (flag3)
				{
					this._connectionStatusLabel.Text = this.Desktop.Provider.GetText("ui.assetEditor.mainMenu.connection.authenticating", null, true);
				}
				else
				{
					this._connectionStatusLabel.Text = this.Desktop.Provider.GetText("ui.assetEditor.mainMenu.connection.connecting", null, true);
				}
			}
			else
			{
				bool flag4 = connectionErrorMessage != null;
				if (flag4)
				{
					this._connectionErrorLabel.Text = this.Desktop.Provider.GetText(connectionErrorMessage, null, true);
				}
			}
			if (doLayout)
			{
				base.Layout(null, true);
			}
		}

		// Token: 0x040039BD RID: 14781
		public readonly AssetEditorInterface Interface;

		// Token: 0x040039BE RID: 14782
		private TextButton _cosmeticsEditorButton;

		// Token: 0x040039BF RID: 14783
		private Label _connectionStatusLabel;

		// Token: 0x040039C0 RID: 14784
		private Label _connectionErrorLabel;

		// Token: 0x040039C1 RID: 14785
		private TextButton _cancelConnectionButton;

		// Token: 0x040039C2 RID: 14786
		private TextButton _closeDisconnectPopupButton;

		// Token: 0x040039C3 RID: 14787
		private TextButton _reconnectButton;

		// Token: 0x040039C4 RID: 14788
		private Group _disconnectPopup;

		// Token: 0x040039C5 RID: 14789
		private Label _disconnectReason;

		// Token: 0x040039C6 RID: 14790
		private Group _serverList;

		// Token: 0x040039C7 RID: 14791
		private TextButton _addServerButton;

		// Token: 0x040039C8 RID: 14792
		private TextButton _editServerButton;

		// Token: 0x040039C9 RID: 14793
		private TextButton _removeServerButton;

		// Token: 0x040039CA RID: 14794
		private TextButton.TextButtonStyle _buttonStyle;

		// Token: 0x040039CB RID: 14795
		private TextButton.TextButtonStyle _buttonStyleSelected;

		// Token: 0x040039CC RID: 14796
		private int _selectedServerIndex = -1;

		// Token: 0x040039CD RID: 14797
		private readonly ServerModal _serverModal;

		// Token: 0x040039CE RID: 14798
		private readonly ConfirmationModal _confirmationModal;
	}
}
