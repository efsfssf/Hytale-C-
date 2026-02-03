using System;
using System.Diagnostics;
using System.Net.Sockets;
using HytaleClient.Interface;
using HytaleClient.Networking;
using HytaleClient.Protocol;
using HytaleClient.Utils;
using NLog;
using Sentry;

namespace HytaleClient.Application
{
	// Token: 0x02000BE6 RID: 3046
	internal class AppGameLoading
	{
		// Token: 0x170013F6 RID: 5110
		// (get) Token: 0x06006091 RID: 24721 RVA: 0x001F9768 File Offset: 0x001F7968
		// (set) Token: 0x06006092 RID: 24722 RVA: 0x001F9770 File Offset: 0x001F7970
		public AppGameLoading.GameLoadingStage LoadingStage { get; private set; }

		// Token: 0x170013F7 RID: 5111
		// (get) Token: 0x06006093 RID: 24723 RVA: 0x001F9779 File Offset: 0x001F7979
		// (set) Token: 0x06006094 RID: 24724 RVA: 0x001F9781 File Offset: 0x001F7981
		public ConnectionToServer Connection { get; private set; }

		// Token: 0x06006095 RID: 24725 RVA: 0x001F978A File Offset: 0x001F798A
		public AppGameLoading(App app)
		{
			this._app = app;
		}

		// Token: 0x06006096 RID: 24726 RVA: 0x001F979C File Offset: 0x001F799C
		public void Open(string singleplayerWorldName)
		{
			SentrySdk.ConfigureScope(delegate(Scope o)
			{
				o.UnsetTag("Server");
				o.SetTag("World", singleplayerWorldName);
			});
			this.LoadingStage = AppGameLoading.GameLoadingStage.Initial;
			this._app.SetSingleplayerWorldName(singleplayerWorldName);
			this._app.SetStage(App.AppStage.GameLoading);
			this._app.MainMenu.SetPageToReturnTo(AppMainMenu.MainMenuPage.Adventure);
			bool flag = this._app.ShuttingDownSingleplayerServer != null;
			if (flag)
			{
				this._app.Interface.GameLoadingView.SetStatus("Waiting for server to finish shutting down...", 0f);
				this.SetStage(AppGameLoading.GameLoadingStage.WaitingForServerToShutdown);
			}
			else
			{
				this.StartSingleplayerServer();
			}
		}

		// Token: 0x06006097 RID: 24727 RVA: 0x001F984C File Offset: 0x001F7A4C
		public void Open(string hostname, int port, AppMainMenu.MainMenuPage? returnPage = null)
		{
			SentrySdk.ConfigureScope(delegate(Scope o)
			{
				o.UnsetTag("World");
				o.SetTag("Server", string.Format("{0}:{1}", hostname, port));
			});
			this._app.DevTools.Info(string.Format("Connecting to server {0}:{1}", hostname, port));
			this.LoadingStage = AppGameLoading.GameLoadingStage.Initial;
			this.Connection = new ConnectionToServer(this._app.Engine, hostname, port, new Action<Exception>(this.OnConnected), new Action<Exception>(this.OnDisconnectedWithError));
			this._app.SetStage(App.AppStage.GameLoading);
			bool flag = returnPage != null;
			if (flag)
			{
				this._app.MainMenu.SetPageToReturnTo(returnPage.Value);
			}
			this.SetStage(AppGameLoading.GameLoadingStage.Connecting);
			this._app.Interface.GameLoadingView.SetStatus("Connecting...", 0f);
		}

		// Token: 0x06006098 RID: 24728 RVA: 0x001F9948 File Offset: 0x001F7B48
		internal void CleanUp()
		{
			bool flag = this.LoadingStage == AppGameLoading.GameLoadingStage.Complete;
			if (flag)
			{
				Debug.Assert(this.Connection == null);
			}
			else
			{
				bool flag2 = this.Connection != null;
				if (flag2)
				{
					this.Connection.OnDisconnected = null;
					this.Connection.Close();
					this.Connection = null;
				}
				bool flag3 = this._app.SingleplayerServer != null;
				if (flag3)
				{
					this._app.SingleplayerServer.Close();
					this._app.OnSinglePlayerServerShuttingDown();
				}
				this._app.InGame.DisposeAndClearInstance();
				bool flag4 = this._app.Interface.FadeState == BaseInterface.InterfaceFadeState.FadingOut || this._app.Interface.FadeState == BaseInterface.InterfaceFadeState.FadedOut;
				if (flag4)
				{
					this._app.Interface.FadeIn(null, false);
				}
			}
			SentrySdk.ConfigureScope(delegate(Scope o)
			{
				o.UnsetTag("World");
				o.UnsetTag("Server");
			});
		}

		// Token: 0x06006099 RID: 24729 RVA: 0x001F9A54 File Offset: 0x001F7C54
		public void AssertStage(AppGameLoading.GameLoadingStage stage)
		{
			bool flag = this.LoadingStage == stage;
			if (flag)
			{
				return;
			}
			throw new Exception(string.Format("Loading stage is {0} but expected {1}", this.LoadingStage, stage));
		}

		// Token: 0x0600609A RID: 24730 RVA: 0x001F9A94 File Offset: 0x001F7C94
		public void AssertStage(AppGameLoading.GameLoadingStage stage1, AppGameLoading.GameLoadingStage stage2)
		{
			bool flag = this.LoadingStage == stage1 || this.LoadingStage == stage2;
			if (flag)
			{
				return;
			}
			throw new Exception(string.Format("Loading stage is {0} but expected {1} or {2}", this.LoadingStage, stage1, stage2));
		}

		// Token: 0x0600609B RID: 24731 RVA: 0x001F9AE4 File Offset: 0x001F7CE4
		public void SetStage(AppGameLoading.GameLoadingStage stage)
		{
			AppGameLoading.Logger.Info<AppGameLoading.GameLoadingStage, AppGameLoading.GameLoadingStage>("Changing from loading stage {from} to {to}", this.LoadingStage, stage);
			this.LoadingStage = stage;
			bool flag = this.LoadingStage == AppGameLoading.GameLoadingStage.Complete;
			if (flag)
			{
				this.Connection = null;
			}
		}

		// Token: 0x0600609C RID: 24732 RVA: 0x001F9B28 File Offset: 0x001F7D28
		public void StartSingleplayerServer()
		{
			AppGameLoading.<>c__DisplayClass18_0 CS$<>8__locals1 = new AppGameLoading.<>c__DisplayClass18_0();
			CS$<>8__locals1.<>4__this = this;
			this.AssertStage(AppGameLoading.GameLoadingStage.Initial, AppGameLoading.GameLoadingStage.WaitingForServerToShutdown);
			this.SetStage(AppGameLoading.GameLoadingStage.BootingServer);
			Debug.Assert(this._app.SingleplayerServer == null);
			Debug.Assert(this._app.ShuttingDownSingleplayerServer == null);
			this._app.Interface.GameLoadingView.SetStatus("Booting server...", 0f);
			CS$<>8__locals1.server = null;
			try
			{
				CS$<>8__locals1.server = new SingleplayerServer(this._app, this._app.SingleplayerWorldName, new Action<string, float>(CS$<>8__locals1.<StartSingleplayerServer>g__OnSingleplayerServerProgress|1), new Action(CS$<>8__locals1.<StartSingleplayerServer>g__OnSingleplayerServerReady|2), delegate()
				{
					CS$<>8__locals1.<>4__this._app.OnSingleplayerServerShutdown(CS$<>8__locals1.server);
					bool flag = CS$<>8__locals1.<>4__this.LoadingStage == AppGameLoading.GameLoadingStage.BootingServer;
					if (flag)
					{
						CS$<>8__locals1.<>4__this.OnDisconnectedWithError(new Exception("Server failed to boot."));
					}
				});
				this._app.SetSingleplayerServer(CS$<>8__locals1.server);
			}
			catch (Exception value)
			{
				AppGameLoading.Logger.Error<Exception>(value);
				this.Abort();
			}
		}

		// Token: 0x0600609D RID: 24733 RVA: 0x001F9C24 File Offset: 0x001F7E24
		private void OnConnected(Exception exception)
		{
			bool flag = exception != null;
			if (flag)
			{
				string hostname = this.Connection.Hostname;
				int port = this.Connection.Port;
				AppGameLoading.Logger.Error(exception, "Failed to connect");
				this.Connection = null;
				bool flag2 = this._app.SingleplayerServer != null;
				if (flag2)
				{
					this._app.Disconnection.SetReason(this._app.SingleplayerServer.ShutdownMessage);
				}
				bool flag3 = this._app.Disconnection.Reason == null && exception is SocketException;
				if (flag3)
				{
					this._app.Disconnection.SetReason(this._app.Interface.GetText("ui.disconnection.errors.noConnectectionEstablished", null, true));
				}
				this._app.Disconnection.Open(exception.Message, hostname, port);
			}
			else
			{
				this.SetStage(AppGameLoading.GameLoadingStage.Loading);
				this._app.InGame.CreateInstance(this.Connection);
			}
		}

		// Token: 0x0600609E RID: 24734 RVA: 0x001F9D2C File Offset: 0x001F7F2C
		private void OnDisconnectedWithError(Exception exception)
		{
			Debug.Assert(ThreadHelper.IsMainThread());
			Debug.Assert(this.Connection != null || this._app.SingleplayerWorldName != null);
			bool flag = this.LoadingStage == AppGameLoading.GameLoadingStage.Aborted;
			if (!flag)
			{
				AppGameLoading.Logger.Info("Disconnected during loading with error:");
				AppGameLoading.Logger.Error<Exception>(exception);
				this.SetStage(AppGameLoading.GameLoadingStage.Aborted);
				bool flag2 = this._app.SingleplayerWorldName != null;
				if (flag2)
				{
					this._app.Disconnection.Open(exception.Message, null, 0);
				}
				else
				{
					this._app.Disconnection.Open(exception.Message, this.Connection.Hostname, this.Connection.Port);
				}
			}
		}

		// Token: 0x0600609F RID: 24735 RVA: 0x001F9DF4 File Offset: 0x001F7FF4
		public void Abort()
		{
			Debug.Assert(ThreadHelper.IsMainThread());
			bool flag = this.LoadingStage == AppGameLoading.GameLoadingStage.Complete || this._app.Stage != App.AppStage.GameLoading;
			if (!flag)
			{
				bool flag2 = this.Connection != null;
				if (flag2)
				{
					this.Connection.OnDisconnected = null;
					this.Connection.SendPacketImmediate(new Disconnect("Player abort", 0));
					this.Connection.Close();
					this.Connection = null;
				}
				bool flag3 = this._app.SingleplayerServer != null;
				if (flag3)
				{
					this._app.SingleplayerServer.Close();
					this._app.OnSinglePlayerServerShuttingDown();
				}
				this._app.InGame.DisposeAndClearInstance();
				this.SetStage(AppGameLoading.GameLoadingStage.Aborted);
				this._app.MainMenu.Open(this._app.MainMenu.CurrentPage);
			}
		}

		// Token: 0x04003C1D RID: 15389
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x04003C1E RID: 15390
		private readonly App _app;

		// Token: 0x02001026 RID: 4134
		public enum GameLoadingStage
		{
			// Token: 0x04004D39 RID: 19769
			Initial,
			// Token: 0x04004D3A RID: 19770
			WaitingForServerToShutdown,
			// Token: 0x04004D3B RID: 19771
			BootingServer,
			// Token: 0x04004D3C RID: 19772
			Connecting,
			// Token: 0x04004D3D RID: 19773
			Loading,
			// Token: 0x04004D3E RID: 19774
			Aborted,
			// Token: 0x04004D3F RID: 19775
			Complete
		}
	}
}
