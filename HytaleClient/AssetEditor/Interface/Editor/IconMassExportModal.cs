using System;
using System.Collections.Generic;
using System.Threading;
using HytaleClient.AssetEditor.Data;
using HytaleClient.AssetEditor.Interface.Previews;
using HytaleClient.Interface.Messages;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;
using Newtonsoft.Json.Linq;
using NLog;

namespace HytaleClient.AssetEditor.Interface.Editor
{
	// Token: 0x02000BB2 RID: 2994
	internal class IconMassExportModal : Element
	{
		// Token: 0x06005DAA RID: 23978 RVA: 0x001DD5DF File Offset: 0x001DB7DF
		public IconMassExportModal(AssetEditorOverlay assetEditorOverlay) : base(assetEditorOverlay.Desktop, null)
		{
			this._assetEditorOverlay = assetEditorOverlay;
		}

		// Token: 0x06005DAB RID: 23979 RVA: 0x001DD602 File Offset: 0x001DB802
		protected override void OnMounted()
		{
			this.Desktop.RegisterAnimationCallback(new Action<float>(this.Animate));
		}

		// Token: 0x06005DAC RID: 23980 RVA: 0x001DD620 File Offset: 0x001DB820
		protected override void OnUnmounted()
		{
			this.Desktop.UnregisterAnimationCallback(new Action<float>(this.Animate));
			bool isExporting = this._isExporting;
			if (isExporting)
			{
				this.CancelExport();
			}
		}

		// Token: 0x06005DAD RID: 23981 RVA: 0x001DD65C File Offset: 0x001DB85C
		private void Animate(float deltaTime)
		{
			bool flag = !this._isExporting;
			if (!flag)
			{
				this._statusLabel.Text = this.Desktop.Provider.GetText("ui.assetEditor.iconMassExportModal.status", new Dictionary<string, string>
				{
					{
						"completeCount",
						this.Desktop.Provider.FormatNumber(this._completeCount)
					},
					{
						"totalCount",
						this.Desktop.Provider.FormatNumber(this._totalCount)
					},
					{
						"failedCount",
						this.Desktop.Provider.FormatNumber(this._failedCount)
					},
					{
						"skippedCount",
						this.Desktop.Provider.FormatNumber(this._skippedCount)
					},
					{
						"percentage",
						this.Desktop.Provider.FormatNumber((int)((float)this._completeCount / (float)this._totalCount * 100f))
					}
				}, true);
				this._statusLabel.Layout(null, true);
			}
		}

		// Token: 0x06005DAE RID: 23982 RVA: 0x001DD774 File Offset: 0x001DB974
		public void Build()
		{
			base.Clear();
			Document document;
			this.Desktop.Provider.TryGetDocument("AssetEditor/IconMassExportModal.ui", out document);
			UIFragment uifragment = document.Instantiate(this.Desktop, this);
			this._statusLabel = uifragment.Get<Label>("StatusLabel");
			this._startButton = uifragment.Get<TextButton>("StartButton");
			this._startButton.Activating = new Action(this.Validate);
			this._startButton.Disabled = this._isExporting;
			this._closeButton = uifragment.Get<TextButton>("CloseButton");
			this._closeButton.Activating = new Action(this.Dismiss);
			this._scaleField = uifragment.Get<NumberField>("Scale");
			this._scaleField.Value = 64m;
			this._cancelButton = uifragment.Get<TextButton>("CancelButton");
			this._cancelButton.Activating = new Action(this.CancelExport);
		}

		// Token: 0x06005DAF RID: 23983 RVA: 0x001DD86F File Offset: 0x001DBA6F
		protected internal override void Validate()
		{
			this.StartExport();
		}

		// Token: 0x06005DB0 RID: 23984 RVA: 0x001DD878 File Offset: 0x001DBA78
		protected internal override void Dismiss()
		{
			bool isExporting = this._isExporting;
			if (!isExporting)
			{
				this.Desktop.ClearLayer(4);
			}
		}

		// Token: 0x06005DB1 RID: 23985 RVA: 0x001DD8A0 File Offset: 0x001DBAA0
		private void CancelExport()
		{
			bool flag = this._cancellationTokenSource == null;
			if (!flag)
			{
				this._cancellationTokenSource.Cancel();
				this.FinishExport();
			}
		}

		// Token: 0x06005DB2 RID: 23986 RVA: 0x001DD8D0 File Offset: 0x001DBAD0
		private void StartExport()
		{
			bool isExporting = this._isExporting;
			if (!isExporting)
			{
				this._isExporting = true;
				this._cancellationTokenSource = new CancellationTokenSource();
				this._iconSize = (int)this._scaleField.Value;
				this._statusLabel.Text = this.Desktop.Provider.GetText("ui.assetEditor.iconMassExportModal.starting", null, true);
				this._statusLabel.Layout(null, true);
				this._closeButton.Disabled = true;
				this._closeButton.Layout(null, true);
				this._startButton.Disabled = true;
				this._startButton.Layout(null, true);
				this.PrepareQueue();
			}
		}

		// Token: 0x06005DB3 RID: 23987 RVA: 0x001DD99C File Offset: 0x001DBB9C
		private void PrepareQueue()
		{
			string path = this._assetEditorOverlay.AssetTypeRegistry.AssetTypes["Item"].Path;
			List<AssetFile> assets = this._assetEditorOverlay.Assets.GetAssets(AssetTreeFolder.Server);
			int num;
			bool flag = !this._assetEditorOverlay.Assets.TryGetDirectoryIndex(path, out num, false);
			if (flag)
			{
				this.FinishExport();
			}
			else
			{
				for (int i = num + 1; i < assets.Count; i++)
				{
					AssetFile assetFile = assets[i];
					bool flag2 = !assetFile.Path.StartsWith(path + "/");
					if (flag2)
					{
						break;
					}
					bool isDirectory = assetFile.IsDirectory;
					if (!isDirectory)
					{
						this._pathQueue.Enqueue(assetFile.Path);
					}
				}
				this._totalCount = this._pathQueue.Count;
				this._completeCount = 0;
				this._failedCount = 0;
				this._skippedCount = 0;
				this.ExportNextIcon(this._cancellationTokenSource.Token);
			}
		}

		// Token: 0x06005DB4 RID: 23988 RVA: 0x001DDAAC File Offset: 0x001DBCAC
		private void FinishExport()
		{
			this._pathQueue.Clear();
			this._isExporting = false;
			this._statusLabel.Text = "";
			this._startButton.Disabled = false;
			this._closeButton.Disabled = false;
			this._cancellationTokenSource = null;
			bool isMounted = base.IsMounted;
			if (isMounted)
			{
				base.Layout(null, true);
			}
		}

		// Token: 0x06005DB5 RID: 23989 RVA: 0x001DDB18 File Offset: 0x001DBD18
		private void ExportNextIcon(CancellationToken cancellationToken)
		{
			bool isCancellationRequested = cancellationToken.IsCancellationRequested;
			if (!isCancellationRequested)
			{
				bool flag = this._pathQueue.Count == 0;
				if (flag)
				{
					this.FinishExport();
				}
				else
				{
					string path = this._pathQueue.Dequeue();
					this._completeCount++;
					SchemaNode schema = this._assetEditorOverlay.AssetTypeRegistry.AssetTypes["Item"].Schema;
					this._assetEditorOverlay.Backend.FetchJsonAssetWithParents(new AssetReference("Item", path), delegate(Dictionary<string, TrackedAsset> data, FormattedMessage err)
					{
						bool flag2 = err != null;
						if (flag2)
						{
							this._failedCount++;
							this.ExportNextIcon(cancellationToken);
						}
						else
						{
							JObject jobject = (JObject)data[path].Data;
							bool flag3 = jobject["Icon"] == null || jobject["IconProperties"] == null || !((string)jobject["Icon"]).StartsWith("Icons/ItemsGenerated/");
							if (flag3)
							{
								this._skippedCount++;
								IconMassExportModal.Logger.Info("Skipping {0} because it doesn't have a generated icon setup.", path);
								this.ExportNextIcon(cancellationToken);
							}
							else
							{
								this._assetEditorOverlay.ApplyAssetInheritance(schema, jobject, data, schema);
								this.ExportNextIcon(cancellationToken);
							}
						}
					}, false);
				}
			}
		}

		// Token: 0x06005DB6 RID: 23990 RVA: 0x001DDBDC File Offset: 0x001DBDDC
		public void Open()
		{
			this.Desktop.SetLayer(4, this);
		}

		// Token: 0x04003A8F RID: 14991
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x04003A90 RID: 14992
		private Label _statusLabel;

		// Token: 0x04003A91 RID: 14993
		private TextButton _startButton;

		// Token: 0x04003A92 RID: 14994
		private TextButton _cancelButton;

		// Token: 0x04003A93 RID: 14995
		private TextButton _closeButton;

		// Token: 0x04003A94 RID: 14996
		private NumberField _scaleField;

		// Token: 0x04003A95 RID: 14997
		private bool _isExporting;

		// Token: 0x04003A96 RID: 14998
		private readonly AssetEditorOverlay _assetEditorOverlay;

		// Token: 0x04003A97 RID: 14999
		private readonly Queue<string> _pathQueue = new Queue<string>();

		// Token: 0x04003A98 RID: 15000
		private int _totalCount;

		// Token: 0x04003A99 RID: 15001
		private int _completeCount;

		// Token: 0x04003A9A RID: 15002
		private int _failedCount;

		// Token: 0x04003A9B RID: 15003
		private int _skippedCount;

		// Token: 0x04003A9C RID: 15004
		private AssetPreview _modelPreview;

		// Token: 0x04003A9D RID: 15005
		private CancellationTokenSource _cancellationTokenSource;

		// Token: 0x04003A9E RID: 15006
		private int _iconSize;
	}
}
