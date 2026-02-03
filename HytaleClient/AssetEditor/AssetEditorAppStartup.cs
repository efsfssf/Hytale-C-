using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using HytaleClient.AssetEditor.Data;
using HytaleClient.Utils;
using NLog;

namespace HytaleClient.AssetEditor
{
	// Token: 0x02000B8B RID: 2955
	internal class AssetEditorAppStartup
	{
		// Token: 0x06005B12 RID: 23314 RVA: 0x001C71FF File Offset: 0x001C53FF
		public AssetEditorAppStartup(AssetEditorApp app)
		{
			this._app = app;
		}

		// Token: 0x06005B13 RID: 23315 RVA: 0x001C721B File Offset: 0x001C541B
		public void StartFromCosmeticsEditor()
		{
			this.Load(delegate
			{
				this._app.Editor.OpenCosmeticsEditor();
			});
		}

		// Token: 0x06005B14 RID: 23316 RVA: 0x001C7231 File Offset: 0x001C5431
		public void StartFromMainMenu()
		{
			this.Load(delegate
			{
				this._app.SetStage(AssetEditorApp.AppStage.MainMenu);
				this._app.Engine.Window.Show();
			});
		}

		// Token: 0x06005B15 RID: 23317 RVA: 0x001C7248 File Offset: 0x001C5448
		public void StartFromCosmeticsEditorWithPath(string assetFile)
		{
			this.Load(delegate
			{
				this._app.Editor.OpenCosmeticsEditor();
				this._app.Editor.OpenAsset(assetFile);
			});
		}

		// Token: 0x06005B16 RID: 23318 RVA: 0x001C7280 File Offset: 0x001C5480
		public void StartFromCosmeticsEditorWithId(string assetType, string assetId)
		{
			this.Load(delegate
			{
				this._app.Editor.OpenCosmeticsEditor();
				this._app.Editor.OpenAsset(new AssetIdReference(assetType, assetId));
			});
		}

		// Token: 0x06005B17 RID: 23319 RVA: 0x001C72BC File Offset: 0x001C54BC
		public void StartFromAssetEditor(string address)
		{
			this.Load(delegate
			{
				this._app.SetStage(AssetEditorApp.AppStage.MainMenu);
				bool isConnectingToServer = this._app.MainMenu.IsConnectingToServer;
				if (!isConnectingToServer)
				{
					string host;
					int port;
					string argument;
					bool flag = HostnameHelper.TryParseHostname(address, 5520, out host, out port, out argument);
					if (flag)
					{
						this._app.MainMenu.ConnectToServer(host, port);
					}
					else
					{
						AssetEditorAppStartup.Logger.Warn<string, string>("Invalid address '{0}': {1}", address, argument);
					}
				}
			});
		}

		// Token: 0x06005B18 RID: 23320 RVA: 0x001C72F1 File Offset: 0x001C54F1
		public void StartFromAssetEditorWithPath(string address, string assetFile)
		{
			this._app.MainMenu.AssetPathToOpen = assetFile;
			this.StartFromAssetEditor(address);
		}

		// Token: 0x06005B19 RID: 23321 RVA: 0x001C730D File Offset: 0x001C550D
		public void StartFromAssetEditorWithId(string address, string assetType, string assetId)
		{
			this._app.MainMenu.AssetIdToOpen = new AssetIdReference(assetType, assetId);
			this.StartFromAssetEditor(address);
		}

		// Token: 0x06005B1A RID: 23322 RVA: 0x001C7330 File Offset: 0x001C5530
		[DebuggerStepThrough]
		private void Load(Action onLoaded)
		{
			AssetEditorAppStartup.<Load>d__11 <Load>d__ = new AssetEditorAppStartup.<Load>d__11();
			<Load>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<Load>d__.<>4__this = this;
			<Load>d__.onLoaded = onLoaded;
			<Load>d__.<>1__state = -1;
			<Load>d__.<>t__builder.Start<AssetEditorAppStartup.<Load>d__11>(ref <Load>d__);
		}

		// Token: 0x06005B1B RID: 23323 RVA: 0x001C7370 File Offset: 0x001C5570
		public void CleanUp()
		{
			this._startupLoadingCancelTokenSource.Cancel();
		}

		// Token: 0x0400390E RID: 14606
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x0400390F RID: 14607
		private readonly AssetEditorApp _app;

		// Token: 0x04003910 RID: 14608
		private readonly CancellationTokenSource _startupLoadingCancelTokenSource = new CancellationTokenSource();
	}
}
