using System;
using HytaleClient.AssetEditor.Data;
using HytaleClient.AssetEditor.Utils;
using HytaleClient.Interface.UI;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Protocol;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HytaleClient.AssetEditor.Interface.Editor
{
	// Token: 0x02000BB3 RID: 2995
	internal class SourceEditor : Element
	{
		// Token: 0x170013C5 RID: 5061
		// (get) Token: 0x06005DB8 RID: 23992 RVA: 0x001DDBF9 File Offset: 0x001DBDF9
		// (set) Token: 0x06005DB9 RID: 23993 RVA: 0x001DDC01 File Offset: 0x001DBE01
		public bool HasUnsavedChanges { get; private set; }

		// Token: 0x06005DBA RID: 23994 RVA: 0x001DDC0C File Offset: 0x001DBE0C
		public SourceEditor(AssetEditorOverlay assetEditorOverlay) : base(assetEditorOverlay.Desktop, null)
		{
			this._assetEditorOverlay = assetEditorOverlay;
			this._layoutMode = LayoutMode.Top;
			this.FlexWeight = 1;
			this.CodeEditor = new WebCodeEditor(assetEditorOverlay.Interface, assetEditorOverlay.Desktop, null)
			{
				FlexWeight = 1,
				ValueChanged = new Action(this.OnChange)
			};
		}

		// Token: 0x06005DBB RID: 23995 RVA: 0x001DDC70 File Offset: 0x001DBE70
		public void Build()
		{
			base.Clear();
			Document document;
			this.Desktop.Provider.TryGetDocument("AssetEditor/SourceEditor.ui", out document);
			UIFragment uifragment = document.Instantiate(this.Desktop, this);
			this._applyButton = uifragment.Get<TextButton>("ApplyButton");
			this._applyButton.Activating = delegate()
			{
				bool flag = this.ApplyChanges();
				if (flag)
				{
					this._assetEditorOverlay.Backend.SaveUnsavedChanges();
				}
			};
			this._discardButton = uifragment.Get<TextButton>("DiscardButton");
			this._discardButton.Activating = new Action(this.Discard);
			this._currentAssetNameLabel = uifragment.Get<Group>("CurrentAsset").Find<Label>("AssetName");
			this._currentAssetTypeLabel = uifragment.Get<Group>("CurrentAsset").Find<Label>("AssetType");
			this._errorMessageLabel = uifragment.Get<Label>("ErrorMessage");
			base.Add(this.CodeEditor, -1);
		}

		// Token: 0x06005DBC RID: 23996 RVA: 0x001DDD50 File Offset: 0x001DBF50
		protected override void OnUnmounted()
		{
			this.CodeEditor.Value = "";
			this.HasUnsavedChanges = false;
			this._errorMessageLabel.Visible = false;
		}

		// Token: 0x06005DBD RID: 23997 RVA: 0x001DDD7C File Offset: 0x001DBF7C
		public void Setup(string value, WebCodeEditor.EditorLanguage language, AssetReference assetReference)
		{
			this.HasUnsavedChanges = false;
			this._applyButton.Disabled = true;
			this._discardButton.Disabled = true;
			this._errorMessageLabel.Visible = false;
			this._assetReference = assetReference;
			this._currentAssetNameLabel.Text = this._assetEditorOverlay.GetAssetIdFromReference(assetReference);
			this._currentAssetTypeLabel.Text = this._assetEditorOverlay.AssetTypeRegistry.AssetTypes[assetReference.Type].Name;
			this.CodeEditor.Language = language;
			this.CodeEditor.Value = value;
		}

		// Token: 0x06005DBE RID: 23998 RVA: 0x001DDE1C File Offset: 0x001DC01C
		private void OnChange()
		{
			bool flag = !this.HasUnsavedChanges;
			if (flag)
			{
				this.HasUnsavedChanges = true;
				this._applyButton.Disabled = false;
				this._discardButton.Disabled = false;
				this._applyButton.Parent.Layout(null, true);
			}
			bool visible = this._errorMessageLabel.Visible;
			if (visible)
			{
				this._errorMessageLabel.Visible = false;
				base.Layout(null, true);
			}
		}

		// Token: 0x06005DBF RID: 23999 RVA: 0x001DDEA4 File Offset: 0x001DC0A4
		public bool ApplyChanges()
		{
			bool flag = !this.HasUnsavedChanges;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				this._errorMessageLabel.Visible = false;
				JObject jobject = null;
				AssetTypeConfig assetTypeConfig = this._assetEditorOverlay.AssetTypeRegistry.AssetTypes[this._assetEditorOverlay.CurrentAsset.Type];
				bool flag2 = this.CodeEditor.Language == WebCodeEditor.EditorLanguage.Json;
				if (flag2)
				{
					try
					{
						JsonUtils.ValidateJson(this.CodeEditor.Value);
						jobject = JObject.Parse(this.CodeEditor.Value);
					}
					catch (JsonReaderException)
					{
						this._errorMessageLabel.Text = this.Desktop.Provider.GetText("ui.assetEditor.errors.invalidJson", null, true);
						this._errorMessageLabel.Visible = true;
						base.Layout(null, true);
						return false;
					}
					bool flag3 = assetTypeConfig.HasIdField && (string)jobject["Id"] != this._assetEditorOverlay.GetAssetIdFromReference(this._assetEditorOverlay.CurrentAsset);
					if (flag3)
					{
						this._errorMessageLabel.Text = this.Desktop.Provider.GetText("ui.assetEditor.errors.idImmutableInSourceEditor", null, true);
						this._errorMessageLabel.Visible = true;
						base.Layout(null, true);
						return false;
					}
				}
				this.HasUnsavedChanges = false;
				this._applyButton.Disabled = true;
				this._discardButton.Disabled = true;
				base.Layout(null, true);
				bool flag4 = this.CodeEditor.Language == WebCodeEditor.EditorLanguage.Json;
				if (flag4)
				{
					bool flag5 = assetTypeConfig.Schema != null;
					if (flag5)
					{
						SchemaNode schemaNode = this._assetEditorOverlay.ResolveSchemaInCurrentContext(assetTypeConfig.Schema);
						this._assetEditorOverlay.TryResolveTypeSchemaInCurrentContext(jobject, ref schemaNode);
						this._assetEditorOverlay.UpdateJsonAsset(this._assetReference.FilePath, jobject, schemaNode.RebuildCaches);
					}
					else
					{
						this._assetEditorOverlay.UpdateJsonAsset(this._assetReference.FilePath, jobject, new AssetEditorRebuildCaches());
					}
				}
				else
				{
					this._assetEditorOverlay.UpdateTextAsset(this._assetReference.FilePath, this.CodeEditor.Value);
				}
				result = true;
			}
			return result;
		}

		// Token: 0x06005DC0 RID: 24000 RVA: 0x001DE100 File Offset: 0x001DC300
		public void Discard()
		{
			bool flag = !this.HasUnsavedChanges;
			if (!flag)
			{
				this.HasUnsavedChanges = false;
				this._applyButton.Disabled = true;
				this._discardButton.Disabled = true;
				this._errorMessageLabel.Visible = false;
				base.Layout(null, true);
				bool flag2 = this.CodeEditor.Language == WebCodeEditor.EditorLanguage.Json;
				if (flag2)
				{
					this.CodeEditor.Value = ((JObject)this._assetEditorOverlay.TrackedAssets[this._assetReference.FilePath].Data).ToString();
				}
				else
				{
					this.CodeEditor.Value = this._assetEditorOverlay.TrackedAssets[this._assetReference.FilePath].Data.ToString();
				}
			}
		}

		// Token: 0x04003A9F RID: 15007
		private readonly AssetEditorOverlay _assetEditorOverlay;

		// Token: 0x04003AA0 RID: 15008
		public readonly WebCodeEditor CodeEditor;

		// Token: 0x04003AA1 RID: 15009
		private TextButton _applyButton;

		// Token: 0x04003AA2 RID: 15010
		private TextButton _discardButton;

		// Token: 0x04003AA3 RID: 15011
		private Label _errorMessageLabel;

		// Token: 0x04003AA4 RID: 15012
		private Label _currentAssetNameLabel;

		// Token: 0x04003AA5 RID: 15013
		private Label _currentAssetTypeLabel;

		// Token: 0x04003AA7 RID: 15015
		private AssetReference _assetReference;
	}
}
