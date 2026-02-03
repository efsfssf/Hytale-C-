using System;
using HytaleClient.AssetEditor.Backends;
using HytaleClient.AssetEditor.Data;
using HytaleClient.AssetEditor.Interface.Editor;
using HytaleClient.AssetEditor.Networking;
using HytaleClient.Networking;
using HytaleClient.Protocol;
using NLog;

namespace HytaleClient.AssetEditor
{
	// Token: 0x02000B8A RID: 2954
	internal class AssetEditorAppEditor
	{
		// Token: 0x17001393 RID: 5011
		// (get) Token: 0x06005AFC RID: 23292 RVA: 0x001C6EC4 File Offset: 0x001C50C4
		// (set) Token: 0x06005AFD RID: 23293 RVA: 0x001C6ECC File Offset: 0x001C50CC
		public AssetEditorBackend Backend { get; private set; }

		// Token: 0x17001394 RID: 5012
		// (get) Token: 0x06005AFE RID: 23294 RVA: 0x001C6ED5 File Offset: 0x001C50D5
		public ServerAssetEditorBackend ServerBackend
		{
			get
			{
				return this.Backend as ServerAssetEditorBackend;
			}
		}

		// Token: 0x17001395 RID: 5013
		// (get) Token: 0x06005AFF RID: 23295 RVA: 0x001C6EE2 File Offset: 0x001C50E2
		// (set) Token: 0x06005B00 RID: 23296 RVA: 0x001C6EEA File Offset: 0x001C50EA
		public Model ModelPreview { get; private set; }

		// Token: 0x17001396 RID: 5014
		// (get) Token: 0x06005B01 RID: 23297 RVA: 0x001C6EF3 File Offset: 0x001C50F3
		// (set) Token: 0x06005B02 RID: 23298 RVA: 0x001C6EFB File Offset: 0x001C50FB
		public BlockType BlockPreview { get; private set; }

		// Token: 0x17001397 RID: 5015
		// (get) Token: 0x06005B03 RID: 23299 RVA: 0x001C6F04 File Offset: 0x001C5104
		// (set) Token: 0x06005B04 RID: 23300 RVA: 0x001C6F0C File Offset: 0x001C510C
		public AssetEditorPreviewCameraSettings PreviewCameraSettings { get; private set; }

		// Token: 0x06005B05 RID: 23301 RVA: 0x001C6F15 File Offset: 0x001C5115
		public AssetEditorAppEditor(AssetEditorApp app)
		{
			this._app = app;
			this.GameTime = new GameTimeState(app);
		}

		// Token: 0x06005B06 RID: 23302 RVA: 0x001C6F34 File Offset: 0x001C5134
		public void OpenCosmeticsEditor()
		{
			AssetEditorOverlay assetEditor = this._app.Interface.AssetEditor;
			this.Backend = new LocalAssetEditorBackend(assetEditor, new AssetTreeFolder[]
			{
				AssetTreeFolder.Cosmetics
			});
			assetEditor.SetupBackend(this.Backend);
			this._app.SetStage(AssetEditorApp.AppStage.Editor);
		}

		// Token: 0x06005B07 RID: 23303 RVA: 0x001C6F84 File Offset: 0x001C5184
		public void OpenAssetEditor(ConnectionToServer connectionToServer, AssetEditorPacketHandler packetHandler)
		{
			this.Backend = new ServerAssetEditorBackend(this._app.Interface.AssetEditor, connectionToServer, packetHandler);
			this._app.Interface.AssetEditor.SetupBackend(this.Backend);
			this._app.SetStage(AssetEditorApp.AppStage.Editor);
		}

		// Token: 0x06005B08 RID: 23304 RVA: 0x001C6FD9 File Offset: 0x001C51D9
		public void CloseEditor()
		{
			this._app.SetStage(AssetEditorApp.AppStage.MainMenu);
		}

		// Token: 0x06005B09 RID: 23305 RVA: 0x001C6FE9 File Offset: 0x001C51E9
		public void OpenAsset(string assetPath)
		{
			this._app.Interface.AssetEditor.OpenExistingAsset(assetPath, true);
		}

		// Token: 0x06005B0A RID: 23306 RVA: 0x001C7004 File Offset: 0x001C5204
		public void OpenAsset(AssetIdReference assetReference)
		{
			this._app.Interface.AssetEditor.OpenExistingAssetById(assetReference, true);
		}

		// Token: 0x06005B0B RID: 23307 RVA: 0x001C7020 File Offset: 0x001C5220
		public void OnAssetEditorPathChanged()
		{
			AssetEditorOverlay assetEditor = this._app.Interface.AssetEditor;
			bool flag = !assetEditor.IsBackendInitialized;
			if (!flag)
			{
				assetEditor.FinishWork();
				AssetEditorBackend backend = this.Backend;
				AssetEditorBackend assetEditorBackend = backend;
				if (!(assetEditorBackend is LocalAssetEditorBackend))
				{
					ServerAssetEditorBackend serverAssetEditorBackend = assetEditorBackend as ServerAssetEditorBackend;
					if (serverAssetEditorBackend != null)
					{
						serverAssetEditorBackend.OnLocalAssetsDirectoryPathChanged();
					}
				}
				else
				{
					this._app.MainMenu.Open();
					this._app.Editor.OpenCosmeticsEditor();
				}
			}
		}

		// Token: 0x06005B0C RID: 23308 RVA: 0x001C70A4 File Offset: 0x001C52A4
		public void CleanUp()
		{
			AssetEditorBackend backend = this.Backend;
			if (backend != null)
			{
				backend.Dispose();
			}
			this.Backend = null;
			AssetEditorOverlay assetEditor = this._app.Interface.AssetEditor;
			assetEditor.FinishWork();
			assetEditor.CleanupWebViews();
			assetEditor.Reset();
			this.GameTime.Cleanup();
			this.ModelPreview = null;
			this.BlockPreview = null;
		}

		// Token: 0x06005B0D RID: 23309 RVA: 0x001C7110 File Offset: 0x001C5310
		public void SetModelPreview(Model model, AssetEditorPreviewCameraSettings camera)
		{
			this.ModelPreview = model;
			this.BlockPreview = null;
			bool flag = camera != null;
			if (flag)
			{
				this.PreviewCameraSettings = camera;
			}
			this._app.Interface.AssetEditor.UpdateModelPreview();
		}

		// Token: 0x06005B0E RID: 23310 RVA: 0x001C7154 File Offset: 0x001C5354
		public void SetBlockPreview(BlockType blockType, AssetEditorPreviewCameraSettings camera)
		{
			this.BlockPreview = blockType;
			this.ModelPreview = null;
			bool flag = camera != null;
			if (flag)
			{
				this.PreviewCameraSettings = camera;
			}
			this._app.Interface.AssetEditor.UpdateModelPreview();
		}

		// Token: 0x06005B0F RID: 23311 RVA: 0x001C7198 File Offset: 0x001C5398
		public void ClearPreview(bool updateUi = true)
		{
			this.BlockPreview = null;
			this.ModelPreview = null;
			this.PreviewCameraSettings = null;
			if (updateUi)
			{
				this._app.Interface.AssetEditor.UpdateModelPreview();
			}
		}

		// Token: 0x06005B10 RID: 23312 RVA: 0x001C71D9 File Offset: 0x001C53D9
		public void ShowIpcServerConnectionPrompt(string serverName)
		{
			this._app.Interface.AssetEditor.OpenIpcOpenEditorConfirmationModal(serverName);
		}

		// Token: 0x04003907 RID: 14599
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x04003908 RID: 14600
		private readonly AssetEditorApp _app;

		// Token: 0x04003909 RID: 14601
		public readonly GameTimeState GameTime;
	}
}
