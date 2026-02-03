using System;
using HytaleClient.AssetEditor.Interface.Elements;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;

namespace HytaleClient.AssetEditor.Interface.MainMenu
{
	// Token: 0x02000BA4 RID: 2980
	internal class ServerModal : BaseModal
	{
		// Token: 0x06005C4B RID: 23627 RVA: 0x001D079A File Offset: 0x001CE99A
		public ServerModal(AssetEditorInterface @interface) : base(@interface, "MainMenu/ServerModal.ui")
		{
		}

		// Token: 0x06005C4C RID: 23628 RVA: 0x001D07AC File Offset: 0x001CE9AC
		protected override void BuildModal(Document doc, UIFragment fragment)
		{
			fragment.Get<TextButton>("SaveButton").Activating = new Action(this.Validate);
			fragment.Get<TextButton>("CancelButton").Activating = new Action(this.Dismiss);
			this._nameInput = fragment.Get<TextField>("NameInput");
			this._hostnameInput = fragment.Get<TextField>("HostnameInput");
			this._portInput = fragment.Get<NumberField>("PortInput");
			this._errorLabel = fragment.Get<Label>("ErrorMessage");
			this._saveButton = fragment.Get<TextButton>("SaveButton");
		}

		// Token: 0x06005C4D RID: 23629 RVA: 0x001D084C File Offset: 0x001CEA4C
		public void Open()
		{
			this._serverIndex = -1;
			this._errorLabel.Visible = false;
			this._title.Text = this.Desktop.Provider.GetText("ui.assetEditor.mainMenu.serverModal.titleAdd", null, true);
			this._saveButton.Text = this.Desktop.Provider.GetText("ui.general.add", null, true);
			this._nameInput.Value = "";
			this._hostnameInput.Value = "";
			this._portInput.Value = 5520m;
			base.OpenInLayer();
			this.Desktop.FocusElement(this._nameInput, true);
		}

		// Token: 0x06005C4E RID: 23630 RVA: 0x001D0908 File Offset: 0x001CEB08
		public void Open(int serverIndex, AssetEditorAppMainMenu.Server server)
		{
			this._serverIndex = serverIndex;
			this._errorLabel.Visible = false;
			this._title.Text = this.Desktop.Provider.GetText("ui.assetEditor.mainMenu.serverModal.titleUpdate", null, true);
			this._saveButton.Text = this.Desktop.Provider.GetText("ui.general.save", null, true);
			this._nameInput.Value = server.Name;
			this._hostnameInput.Value = server.Hostname;
			this._portInput.Value = server.Port;
			base.OpenInLayer();
			this.Desktop.FocusElement(this._nameInput, true);
		}

		// Token: 0x06005C4F RID: 23631 RVA: 0x001D09C8 File Offset: 0x001CEBC8
		private void ShowError(string message)
		{
			this._errorLabel.Text = message;
			this._errorLabel.Visible = true;
			base.Layout(null, true);
		}

		// Token: 0x06005C50 RID: 23632 RVA: 0x001D0A04 File Offset: 0x001CEC04
		protected internal override void Validate()
		{
			string text = this._nameInput.Value.Trim();
			string text2 = this._hostnameInput.Value.Trim();
			int port = (int)this._portInput.Value;
			bool flag = text == string.Empty;
			if (flag)
			{
				this.ShowError(this.Desktop.Provider.GetText("ui.assetEditor.mainMenu.serverModal.errors.enterName", null, true));
			}
			else
			{
				bool flag2 = text2 == string.Empty;
				if (flag2)
				{
					this.ShowError(this.Desktop.Provider.GetText("ui.assetEditor.mainMenu.serverModal.errors.enterHostname", null, true));
				}
				else
				{
					string b = text.ToLowerInvariant();
					int count = this._interface.App.MainMenu.Servers.Count;
					for (int i = 0; i < count; i++)
					{
						AssetEditorAppMainMenu.Server server = this._interface.App.MainMenu.Servers[i];
						bool flag3 = server.Name.ToLowerInvariant() != b;
						if (!flag3)
						{
							bool flag4 = i == this._serverIndex;
							if (!flag4)
							{
								this.ShowError(this.Desktop.Provider.GetText("ui.assetEditor.mainMenu.serverModal.errors.nameAlreadyInUse", null, true));
								return;
							}
						}
					}
					bool flag5 = this._serverIndex != -1;
					if (flag5)
					{
						this._interface.App.MainMenu.UpdateServer(this._serverIndex, text, text2, port);
					}
					else
					{
						this._interface.App.MainMenu.AddServer(new AssetEditorAppMainMenu.Server
						{
							DateLastJoined = DateTime.Now,
							Name = text,
							Hostname = text2,
							Port = port
						});
					}
					this.Dismiss();
				}
			}
		}

		// Token: 0x040039CF RID: 14799
		private TextField _nameInput;

		// Token: 0x040039D0 RID: 14800
		private TextField _hostnameInput;

		// Token: 0x040039D1 RID: 14801
		private NumberField _portInput;

		// Token: 0x040039D2 RID: 14802
		private Label _errorLabel;

		// Token: 0x040039D3 RID: 14803
		private TextButton _saveButton;

		// Token: 0x040039D4 RID: 14804
		private int _serverIndex;
	}
}
