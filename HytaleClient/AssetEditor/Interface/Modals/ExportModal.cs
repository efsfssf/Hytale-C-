using System;
using System.Collections.Generic;
using System.IO;
using HytaleClient.AssetEditor.Backends;
using HytaleClient.AssetEditor.Data;
using HytaleClient.AssetEditor.Interface.Editor;
using HytaleClient.Graphics;
using HytaleClient.Interface.UI;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Interface.UI.Styles;
using HytaleClient.Math;
using HytaleClient.Protocol;
using NLog;

namespace HytaleClient.AssetEditor.Interface.Modals
{
	// Token: 0x02000BA0 RID: 2976
	internal class ExportModal : Element
	{
		// Token: 0x06005C0D RID: 23565 RVA: 0x001CEC38 File Offset: 0x001CCE38
		public ExportModal(AssetEditorOverlay assetEditorOverlay) : base(assetEditorOverlay.Desktop, null)
		{
			this._assetEditorOverlay = assetEditorOverlay;
		}

		// Token: 0x06005C0E RID: 23566 RVA: 0x001CEC5C File Offset: 0x001CCE5C
		public void Build()
		{
			base.Clear();
			Document document;
			this.Desktop.Provider.TryGetDocument("AssetEditor/ExportModal.ui", out document);
			UIFragment uifragment = document.Instantiate(this.Desktop, this);
			this._container = uifragment.Get<Group>("Container");
			this._entriesContainer = uifragment.Get<Group>("Entries");
			this._discardCheckBox = uifragment.Get<CheckBox>("DiscardCheckBox");
			this._exportButton = uifragment.Get<TextButton>("ExportButton");
			this._exportButton.Activating = new Action(this.Validate);
			uifragment.Get<TextButton>("CancelButton").Activating = new Action(this.Dismiss);
			uifragment.Get<TextButton>("SelectAllButton").Activating = new Action(this.OnActivateSelectAll);
			uifragment.Get<TextButton>("DeselectAllButton").Activating = new Action(this.OnActivateDeselectAll);
			bool isMounted = base.IsMounted;
			if (isMounted)
			{
				this.Setup();
			}
		}

		// Token: 0x06005C0F RID: 23567 RVA: 0x001CED59 File Offset: 0x001CCF59
		protected override void OnMounted()
		{
			this._assetEditorOverlay.AttachNotifications(this);
			this._assetEditorOverlay.Backend.FetchLastModifiedAssets();
			this._assetEditorOverlay.Backend.UpdateSubscriptionToModifiedAssetsUpdates(true);
		}

		// Token: 0x06005C10 RID: 23568 RVA: 0x001CED8C File Offset: 0x001CCF8C
		protected override void OnUnmounted()
		{
			AssetEditorBackend backend = this._assetEditorOverlay.Backend;
			if (backend != null)
			{
				backend.UpdateSubscriptionToModifiedAssetsUpdates(false);
			}
			this._assetEditorOverlay.ReparentNotifications();
		}

		// Token: 0x06005C11 RID: 23569 RVA: 0x001CEDB3 File Offset: 0x001CCFB3
		public override Element HitTest(Point position)
		{
			return base.HitTest(position) ?? this;
		}

		// Token: 0x06005C12 RID: 23570 RVA: 0x001CEDC4 File Offset: 0x001CCFC4
		protected override void OnMouseButtonUp(MouseButtonEvent evt, bool activate)
		{
			base.OnMouseButtonUp(evt, activate);
			bool flag = activate && !this._container.AnchoredRectangle.Contains(this.Desktop.MousePosition);
			if (flag)
			{
				this.Dismiss();
			}
		}

		// Token: 0x06005C13 RID: 23571 RVA: 0x001CEE10 File Offset: 0x001CD010
		private void OnActivateDeselectAll()
		{
			this._selectedAssets.Clear();
			foreach (Element element in this._entriesContainer.Children)
			{
				ExportModal.ExportModalEntry exportModalEntry = (ExportModal.ExportModalEntry)element;
				exportModalEntry.OnStateChanged();
			}
			this.UpdateExportButtonState();
		}

		// Token: 0x06005C14 RID: 23572 RVA: 0x001CEE84 File Offset: 0x001CD084
		private void OnActivateSelectAll()
		{
			this._selectedAssets.Clear();
			foreach (Element element in this._entriesContainer.Children)
			{
				ExportModal.ExportModalEntry exportModalEntry = (ExportModal.ExportModalEntry)element;
				this._selectedAssets.Add(exportModalEntry.Asset.FilePath);
				exportModalEntry.OnStateChanged();
			}
			this.UpdateExportButtonState();
		}

		// Token: 0x06005C15 RID: 23573 RVA: 0x001CEF0C File Offset: 0x001CD10C
		protected internal override void Dismiss()
		{
			this.Desktop.ClearLayer(4);
		}

		// Token: 0x06005C16 RID: 23574 RVA: 0x001CEF1C File Offset: 0x001CD11C
		protected internal override void Validate()
		{
			bool flag = this._assetEditorOverlay.Backend.IsExportingAssets || this._selectedAssets.Count == 0;
			if (!flag)
			{
				List<AssetReference> list = new List<AssetReference>();
				foreach (string filePath in this._selectedAssets)
				{
					string type;
					this._assetEditorOverlay.AssetTypeRegistry.TryGetAssetTypeFromPath(filePath, out type);
					list.Add(new AssetReference(type, filePath));
				}
				bool value = this._discardCheckBox.Value;
				if (value)
				{
					this._assetEditorOverlay.Backend.ExportAndDiscardAssets(list);
				}
				else
				{
					this._assetEditorOverlay.Backend.ExportAssets(list, null);
				}
			}
		}

		// Token: 0x06005C17 RID: 23575 RVA: 0x001CF000 File Offset: 0x001CD200
		public void UpdateExportButtonState()
		{
			this._exportButton.Disabled = (this._assetEditorOverlay.Backend.IsExportingAssets || this._selectedAssets.Count == 0);
			bool isMounted = base.IsMounted;
			if (isMounted)
			{
				this._exportButton.Layout(null, true);
			}
		}

		// Token: 0x06005C18 RID: 23576 RVA: 0x001CF05C File Offset: 0x001CD25C
		public void ResetState()
		{
			this._entriesContainer.Clear();
			this._selectedAssets.Clear();
			this._exportButton.Disabled = false;
		}

		// Token: 0x06005C19 RID: 23577 RVA: 0x001CF084 File Offset: 0x001CD284
		public void Setup()
		{
			this._entriesContainer.Clear();
			this._selectedAssets.Clear();
			foreach (AssetEditorLastModifiedAssets.AssetInfo assetInfo in this._assetEditorOverlay.Backend.GetLastModifiedAssets())
			{
				string text;
				AssetTypeConfig assetTypeConfig;
				bool flag = !this._assetEditorOverlay.AssetTypeRegistry.TryGetAssetTypeFromPath(assetInfo.Path, out text) || !this._assetEditorOverlay.AssetTypeRegistry.AssetTypes.TryGetValue(text, out assetTypeConfig);
				if (flag)
				{
					ExportModal.Logger.Info("Asset type for file in export list not found: {0}", assetInfo.Path);
				}
				else
				{
					new ExportModal.ExportModalEntry(this._entriesContainer, this, new AssetReference(text, assetInfo.Path), assetTypeConfig, assetInfo);
				}
			}
			this.UpdateExportButtonState();
			bool isMounted = base.IsMounted;
			if (isMounted)
			{
				this._entriesContainer.Layout(null, true);
			}
		}

		// Token: 0x06005C1A RID: 23578 RVA: 0x001CF170 File Offset: 0x001CD370
		public void Open()
		{
			this.Setup();
			this.Desktop.SetLayer(4, this);
		}

		// Token: 0x040039A6 RID: 14758
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x040039A7 RID: 14759
		private readonly AssetEditorOverlay _assetEditorOverlay;

		// Token: 0x040039A8 RID: 14760
		private Group _container;

		// Token: 0x040039A9 RID: 14761
		private Group _entriesContainer;

		// Token: 0x040039AA RID: 14762
		private TextButton _exportButton;

		// Token: 0x040039AB RID: 14763
		private CheckBox _discardCheckBox;

		// Token: 0x040039AC RID: 14764
		private HashSet<string> _selectedAssets = new HashSet<string>();

		// Token: 0x02000FA5 RID: 4005
		private class ExportModalEntry : Element
		{
			// Token: 0x06006958 RID: 26968 RVA: 0x0021D8C4 File Offset: 0x0021BAC4
			public ExportModalEntry(Element parent, ExportModal exportModal, AssetReference asset, AssetTypeConfig assetTypeConfig, AssetEditorLastModifiedAssets.AssetInfo assetInfo) : base(parent.Desktop, parent)
			{
				this._exportModal = exportModal;
				this.Asset = asset;
				Document document;
				this.Desktop.Provider.TryGetDocument("AssetEditor/ExportEntry.ui", out document);
				this._defaultStyle = document.ResolveNamedValue<Button.ButtonStyle>(this.Desktop.Provider, "DefaultStyle");
				this._selectedStyle = document.ResolveNamedValue<Button.ButtonStyle>(this.Desktop.Provider, "SelectedStyle");
				UIFragment uifragment = document.Instantiate(this.Desktop, this);
				Label label = uifragment.Get<Label>("NameLabel");
				this._button = uifragment.Get<Button>("Button");
				this._button.Activating = new Action(this.OnButtonActivating);
				this._button.DoubleClicking = new Action(this.OnButtonDoubleClicking);
				this._button.Style = (this._exportModal._selectedAssets.Contains(this.Asset.FilePath) ? this._selectedStyle : this._defaultStyle);
				uifragment.Get<Label>("UsernameLabel").Text = assetInfo.LastModificationUsername;
				uifragment.Get<Label>("DateLabel").Text = this.Desktop.Provider.FormatRelativeTime(DateTimeOffset.FromUnixTimeMilliseconds(assetInfo.LastModificationDate).LocalDateTime);
				uifragment.Get<TextButton>("DiscardButton").Activating = delegate()
				{
					exportModal._assetEditorOverlay.Backend.DiscardChanges(new TimestampedAssetReference(asset.FilePath, null));
				};
				bool isDeleted = assetInfo.IsDeleted;
				UInt32Color value;
				if (isDeleted)
				{
					value = document.ResolveNamedValue<UInt32Color>(this.Desktop.Provider, "DeletedColor");
					this._button.TooltipText = assetTypeConfig.Name + " - " + this.Desktop.Provider.GetText("ui.assetEditor.exportModal.tooltips.deleted", null, true);
				}
				else
				{
					bool isNew = assetInfo.IsNew;
					if (isNew)
					{
						value = document.ResolveNamedValue<UInt32Color>(this.Desktop.Provider, "NewColor");
						this._button.TooltipText = assetTypeConfig.Name + " - " + this.Desktop.Provider.GetText("ui.assetEditor.exportModal.tooltips.new", null, true);
					}
					else
					{
						bool flag = assetInfo.OldPath != null;
						if (flag)
						{
							value = document.ResolveNamedValue<UInt32Color>(this.Desktop.Provider, "RenamedColor");
							this._button.TooltipText = assetTypeConfig.Name + " - " + this.Desktop.Provider.GetText("ui.assetEditor.exportModal.tooltips.renamed", new Dictionary<string, string>
							{
								{
									"oldPath",
									assetInfo.OldPath
								}
							}, true);
						}
						else
						{
							value = document.ResolveNamedValue<UInt32Color>(this.Desktop.Provider, "DefaultColor");
							this._button.TooltipText = assetTypeConfig.Name + " - " + this.Desktop.Provider.GetText("ui.assetEditor.exportModal.tooltips.changed", null, true);
						}
					}
				}
				List<Label.LabelSpan> list = new List<Label.LabelSpan>();
				string fileName = Path.GetFileName(asset.FilePath);
				string extension = Path.GetExtension(asset.FilePath);
				string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(asset.FilePath);
				list.Add(new Label.LabelSpan
				{
					Color = new UInt32Color?(value),
					Text = asset.FilePath.Substring(0, asset.FilePath.Length - fileName.Length)
				});
				list.Add(new Label.LabelSpan
				{
					Color = new UInt32Color?(value),
					IsBold = true,
					Text = fileNameWithoutExtension
				});
				list.Add(new Label.LabelSpan
				{
					Color = new UInt32Color?(value),
					Text = extension
				});
				label.TextSpans = list;
				uifragment.Get<Group>("Icon").Background = assetTypeConfig.Icon;
				AssetDiagnostics assetDiagnostics;
				bool flag2 = exportModal._assetEditorOverlay.Diagnostics.TryGetValue(asset.FilePath, out assetDiagnostics);
				if (flag2)
				{
					bool flag3 = assetDiagnostics.Errors != null && assetDiagnostics.Errors.Length != 0;
					if (flag3)
					{
						this._hasDiagnostics = true;
						Group group = uifragment.Get<Group>("DiagnosticsIcon");
						group.Visible = true;
						group.Background = new PatchStyle("AssetEditor/ErrorIcon.png");
					}
					else
					{
						bool flag4 = assetDiagnostics.Warnings != null && assetDiagnostics.Warnings.Length != 0;
						if (flag4)
						{
							this._hasDiagnostics = true;
							Group group2 = uifragment.Get<Group>("DiagnosticsIcon");
							group2.Visible = true;
							group2.Background = new PatchStyle("AssetEditor/WarningIcon.png");
						}
					}
				}
			}

			// Token: 0x06006959 RID: 26969 RVA: 0x0021DD94 File Offset: 0x0021BF94
			public void OnStateChanged()
			{
				this._button.Style = (this._exportModal._selectedAssets.Contains(this.Asset.FilePath) ? this._selectedStyle : this._defaultStyle);
				this._button.Layout(null, true);
			}

			// Token: 0x0600695A RID: 26970 RVA: 0x0021DDF0 File Offset: 0x0021BFF0
			private void OnButtonActivating()
			{
				bool flag = !this._exportModal._selectedAssets.Contains(this.Asset.FilePath);
				if (flag)
				{
					this._exportModal._selectedAssets.Add(this.Asset.FilePath);
				}
				else
				{
					this._exportModal._selectedAssets.Remove(this.Asset.FilePath);
				}
				this.OnStateChanged();
				this._exportModal.UpdateExportButtonState();
			}

			// Token: 0x0600695B RID: 26971 RVA: 0x0021DE6D File Offset: 0x0021C06D
			private void OnButtonDoubleClicking()
			{
				this._exportModal._assetEditorOverlay.OpenExistingAsset(this.Asset, true);
			}

			// Token: 0x0600695C RID: 26972 RVA: 0x0021DE88 File Offset: 0x0021C088
			protected override void OnMouseEnter()
			{
				bool flag = !this._hasDiagnostics;
				if (!flag)
				{
					AssetEditorOverlay assetEditorOverlay = this._exportModal._assetEditorOverlay;
					AssetDiagnostics assetDiagnostics;
					bool flag2 = !assetEditorOverlay.Diagnostics.TryGetValue(this.Asset.FilePath, out assetDiagnostics);
					if (!flag2)
					{
						List<Label.LabelSpan> list = new List<Label.LabelSpan>();
						bool flag3 = assetDiagnostics.Errors != null && assetDiagnostics.Errors.Length != 0;
						if (flag3)
						{
							list.Add(new Label.LabelSpan
							{
								Text = this.Desktop.Provider.GetText("ui.assetEditor.diagnosticsTooltip.errors", null, true),
								IsBold = true
							});
							foreach (AssetDiagnosticMessage assetDiagnosticMessage in assetDiagnostics.Errors)
							{
								list.Add(new Label.LabelSpan
								{
									Text = "\n- " + assetDiagnosticMessage.Property.ToString() + ": " + assetDiagnosticMessage.Message
								});
							}
						}
						bool flag4 = assetDiagnostics.Warnings != null && assetDiagnostics.Warnings.Length != 0;
						if (flag4)
						{
							bool flag5 = list.Count > 0;
							if (flag5)
							{
								list.Add(new Label.LabelSpan
								{
									Text = "\n\n"
								});
							}
							list.Add(new Label.LabelSpan
							{
								Text = this.Desktop.Provider.GetText("ui.assetEditor.diagnosticsTooltip.warnings", null, true),
								IsBold = true
							});
							foreach (AssetDiagnosticMessage assetDiagnosticMessage2 in assetDiagnostics.Warnings)
							{
								list.Add(new Label.LabelSpan
								{
									Text = "\n- " + assetDiagnosticMessage2.Property.ToString() + ": " + assetDiagnosticMessage2.Message
								});
							}
						}
						TextTooltipLayer textTooltipLayer = assetEditorOverlay.TextTooltipLayer;
						textTooltipLayer.TextSpans = list;
						textTooltipLayer.Start(false);
					}
				}
			}

			// Token: 0x0600695D RID: 26973 RVA: 0x0021E089 File Offset: 0x0021C289
			protected override void OnMouseLeave()
			{
				this._exportModal._assetEditorOverlay.TextTooltipLayer.Stop();
			}

			// Token: 0x0600695E RID: 26974 RVA: 0x0021E0A2 File Offset: 0x0021C2A2
			public override Element HitTest(Point position)
			{
				return this._anchoredRectangle.Contains(position) ? (base.HitTest(position) ?? this) : null;
			}

			// Token: 0x04004B95 RID: 19349
			private readonly ExportModal _exportModal;

			// Token: 0x04004B96 RID: 19350
			private readonly bool _hasDiagnostics;

			// Token: 0x04004B97 RID: 19351
			private readonly Button _button;

			// Token: 0x04004B98 RID: 19352
			private Button.ButtonStyle _defaultStyle;

			// Token: 0x04004B99 RID: 19353
			private Button.ButtonStyle _selectedStyle;

			// Token: 0x04004B9A RID: 19354
			public readonly AssetReference Asset;
		}
	}
}
