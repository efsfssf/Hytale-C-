using System;
using System.IO;
using HytaleClient.AssetEditor.Data;
using HytaleClient.AssetEditor.Interface.Editor;
using HytaleClient.AssetEditor.Utils;
using HytaleClient.Interface.Messages;
using HytaleClient.Interface.UI;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Math;

namespace HytaleClient.AssetEditor.Interface.Modals
{
	// Token: 0x02000BA2 RID: 2978
	internal class RenameModal : Element
	{
		// Token: 0x06005C2D RID: 23597 RVA: 0x001CF8EC File Offset: 0x001CDAEC
		public RenameModal(AssetEditorOverlay assetEditorOverlay) : base(assetEditorOverlay.Desktop, null)
		{
			this._assetEditorOverlay = assetEditorOverlay;
		}

		// Token: 0x06005C2E RID: 23598 RVA: 0x001CF904 File Offset: 0x001CDB04
		public void Build()
		{
			base.Clear();
			Document document;
			this.Desktop.Provider.TryGetDocument("AssetEditor/RenameModal.ui", out document);
			UIFragment uifragment = document.Instantiate(this.Desktop, this);
			uifragment.Get<TextButton>("SaveButton").Activating = new Action(this.Validate);
			uifragment.Get<TextButton>("CancelButton").Activating = new Action(this.Dismiss);
			this._container = uifragment.Get<Group>("Container");
			this._title = uifragment.Get<Label>("Title");
			this._newIdInput = uifragment.Get<TextField>("Input");
			this._errorLabel = uifragment.Get<Label>("ErrorMessage");
			this._applyChangesLocallyContainer = uifragment.Get<Group>("ApplyChangesLocallyContainer");
			this._applyChangesLocally = uifragment.Get<CheckBox>("ApplyChangesLocally");
		}

		// Token: 0x06005C2F RID: 23599 RVA: 0x001CF9E0 File Offset: 0x001CDBE0
		public void OpenForAsset(AssetReference assetReference, bool displayApplyLocalChangeCheckBox = false)
		{
			this._assetReference = assetReference;
			this._directoryPath = null;
			this._errorLabel.Visible = false;
			this._applyChangesLocallyContainer.Visible = displayApplyLocalChangeCheckBox;
			this._title.Text = this.Desktop.Provider.GetText("ui.assetEditor.renameModal.titleAsset", null, true);
			this._newIdInput.PlaceholderText = this.Desktop.Provider.GetText("ui.assetEditor.renameModal.newId", null, true);
			this.Desktop.SetLayer(4, this);
			this.Desktop.FocusElement(this._newIdInput, true);
			this._newIdInput.Value = this._assetEditorOverlay.GetAssetIdFromReference(assetReference);
			this._newIdInput.SelectAll();
		}

		// Token: 0x06005C30 RID: 23600 RVA: 0x001CFAA4 File Offset: 0x001CDCA4
		public void OpenForDirectory(string path, bool displayApplyLocalChangeCheckBox = false)
		{
			this._directoryPath = path;
			this._assetReference = AssetReference.None;
			this._errorLabel.Visible = false;
			this._applyChangesLocallyContainer.Visible = displayApplyLocalChangeCheckBox;
			this._title.Text = this.Desktop.Provider.GetText("ui.assetEditor.renameModal.titleFolder", null, true);
			this._newIdInput.PlaceholderText = this.Desktop.Provider.GetText("ui.assetEditor.renameModal.newName", null, true);
			this.Desktop.SetLayer(4, this);
			this.Desktop.FocusElement(this._newIdInput, true);
			this._newIdInput.Value = Path.GetFileName(path);
			this._newIdInput.SelectAll();
		}

		// Token: 0x06005C31 RID: 23601 RVA: 0x001CFB64 File Offset: 0x001CDD64
		protected override void OnMouseButtonUp(MouseButtonEvent evt, bool activate)
		{
			base.OnMouseButtonUp(evt, activate);
			bool flag = activate && !this._container.AnchoredRectangle.Contains(this.Desktop.MousePosition);
			if (flag)
			{
				this.Dismiss();
			}
		}

		// Token: 0x06005C32 RID: 23602 RVA: 0x001CFBB0 File Offset: 0x001CDDB0
		private void ValidateAssetId()
		{
			string text = this._newIdInput.Value.Trim();
			string message;
			bool flag = !this._assetEditorOverlay.ValidateAssetId(text, out message);
			if (flag)
			{
				this.ShowError(message);
			}
			else
			{
				AssetTypeConfig assetTypeConfig = this._assetEditorOverlay.AssetTypeRegistry.AssetTypes[this._assetReference.Type];
				bool flag2 = assetTypeConfig.AssetTree == AssetTreeFolder.Cosmetics;
				string text2;
				if (flag2)
				{
					text2 = assetTypeConfig.Path + "#" + text;
				}
				else
				{
					string[] array = this._assetReference.FilePath.Split(new char[]
					{
						'/'
					});
					string path = string.Join("/", array, 0, array.Length - 1);
					text2 = AssetPathUtils.CombinePaths(path, text + assetTypeConfig.FileExtension);
				}
				string path2 = text2.ToLowerInvariant();
				AssetFile assetFile;
				bool flag3 = this._assetEditorOverlay.Assets.TryGetAsset(path2, out assetFile, true);
				if (flag3)
				{
					this.ShowError(this.Desktop.Provider.GetText("ui.assetEditor.createAssetModal.errors.existingId", null, true));
				}
				else
				{
					this._assetEditorOverlay.Backend.RenameAsset(this._assetReference, text2, this._applyChangesLocallyContainer.Visible && this._applyChangesLocally.Value);
					this.Desktop.ClearLayer(4);
				}
			}
		}

		// Token: 0x06005C33 RID: 23603 RVA: 0x001CFD08 File Offset: 0x001CDF08
		private void ValidateDirectoryName()
		{
			string text = this._newIdInput.Value.Trim();
			bool flag = text.Contains("/") || text.Contains("\\");
			if (flag)
			{
				this.ShowError(this.Desktop.Provider.GetText("ui.assetEditor.errors.directoryInvalidCharacters", null, true));
			}
			else
			{
				string text2 = Path.GetDirectoryName(this._directoryPath) + "/" + text;
				string path2 = text2.ToLowerInvariant();
				AssetFile assetFile;
				bool flag2 = this._assetEditorOverlay.Assets.TryGetAsset(path2, out assetFile, true);
				if (flag2)
				{
					this.ShowError(this.Desktop.Provider.GetText("ui.assetEditor.errors.renameDirectoryExists", null, true));
				}
				else
				{
					this._assetEditorOverlay.Backend.RenameDirectory(this._directoryPath, text2, this._applyChangesLocally.Visible && this._applyChangesLocally.Value, delegate(string path, FormattedMessage error)
					{
						bool flag3 = error != null;
						if (flag3)
						{
							this._assetEditorOverlay.ToastNotifications.AddNotification(2, error);
						}
						else
						{
							this._assetEditorOverlay.ToastNotifications.AddNotification(1, this.Desktop.Provider.GetText("ui.assetEditor.messages.directoryCreated", null, true));
						}
					});
					this.Desktop.ClearLayer(4);
				}
			}
		}

		// Token: 0x06005C34 RID: 23604 RVA: 0x001CFE10 File Offset: 0x001CE010
		protected internal override void Validate()
		{
			bool flag = this._directoryPath != null;
			if (flag)
			{
				this.ValidateDirectoryName();
			}
			else
			{
				this.ValidateAssetId();
			}
		}

		// Token: 0x06005C35 RID: 23605 RVA: 0x001CFE40 File Offset: 0x001CE040
		private void ShowError(string message)
		{
			this._errorLabel.Text = message;
			this._errorLabel.Visible = true;
			base.Layout(null, true);
		}

		// Token: 0x06005C36 RID: 23606 RVA: 0x001CFE79 File Offset: 0x001CE079
		protected internal override void Dismiss()
		{
			this.Desktop.ClearLayer(4);
		}

		// Token: 0x06005C37 RID: 23607 RVA: 0x001CFE89 File Offset: 0x001CE089
		public override Element HitTest(Point position)
		{
			return base.HitTest(position) ?? this;
		}

		// Token: 0x040039B4 RID: 14772
		private AssetReference _assetReference;

		// Token: 0x040039B5 RID: 14773
		private string _directoryPath;

		// Token: 0x040039B6 RID: 14774
		private TextField _newIdInput;

		// Token: 0x040039B7 RID: 14775
		private Label _title;

		// Token: 0x040039B8 RID: 14776
		private Label _errorLabel;

		// Token: 0x040039B9 RID: 14777
		private Group _container;

		// Token: 0x040039BA RID: 14778
		private Group _applyChangesLocallyContainer;

		// Token: 0x040039BB RID: 14779
		private CheckBox _applyChangesLocally;

		// Token: 0x040039BC RID: 14780
		private readonly AssetEditorOverlay _assetEditorOverlay;
	}
}
