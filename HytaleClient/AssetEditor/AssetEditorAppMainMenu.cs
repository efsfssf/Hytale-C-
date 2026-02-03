using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using HytaleClient.AssetEditor.Data;
using HytaleClient.AssetEditor.Networking;
using HytaleClient.Interface.Messages;
using HytaleClient.Networking;
using HytaleClient.Protocol;
using HytaleClient.Utils;
using Newtonsoft.Json;
using NLog;

namespace HytaleClient.AssetEditor
{
	// Token: 0x02000B8C RID: 2956
	internal class AssetEditorAppMainMenu
	{
		// Token: 0x17001398 RID: 5016
		// (get) Token: 0x06005B1F RID: 23327 RVA: 0x001C73C5 File Offset: 0x001C55C5
		public bool IsConnectingToServer
		{
			get
			{
				return this._connection != null;
			}
		}

		// Token: 0x17001399 RID: 5017
		// (get) Token: 0x06005B20 RID: 23328 RVA: 0x001C73D0 File Offset: 0x001C55D0
		public ConnectionToServer Connection
		{
			get
			{
				return this._connection;
			}
		}

		// Token: 0x1700139A RID: 5018
		// (get) Token: 0x06005B21 RID: 23329 RVA: 0x001C73D8 File Offset: 0x001C55D8
		// (set) Token: 0x06005B22 RID: 23330 RVA: 0x001C73E0 File Offset: 0x001C55E0
		public string ConnectionErrorMessage { get; private set; }

		// Token: 0x1700139B RID: 5019
		// (get) Token: 0x06005B23 RID: 23331 RVA: 0x001C73E9 File Offset: 0x001C55E9
		// (set) Token: 0x06005B24 RID: 23332 RVA: 0x001C73F1 File Offset: 0x001C55F1
		public bool DisplayDisconnectPopup { get; private set; }

		// Token: 0x1700139C RID: 5020
		// (get) Token: 0x06005B25 RID: 23333 RVA: 0x001C73FA File Offset: 0x001C55FA
		// (set) Token: 0x06005B26 RID: 23334 RVA: 0x001C7402 File Offset: 0x001C5602
		public AssetEditorAppMainMenu.ConnectionStages ConnectionStage { get; private set; }

		// Token: 0x1700139D RID: 5021
		// (get) Token: 0x06005B27 RID: 23335 RVA: 0x001C740B File Offset: 0x001C560B
		public IReadOnlyList<AssetEditorAppMainMenu.Server> Servers
		{
			get
			{
				return this._servers;
			}
		}

		// Token: 0x1700139E RID: 5022
		// (get) Token: 0x06005B28 RID: 23336 RVA: 0x001C7413 File Offset: 0x001C5613
		private string ServersFilePath
		{
			get
			{
				return Path.Combine(Paths.UserData, "Servers.json");
			}
		}

		// Token: 0x06005B29 RID: 23337 RVA: 0x001C7424 File Offset: 0x001C5624
		public AssetEditorAppMainMenu(AssetEditorApp app)
		{
			this._app = app;
			this.LoadServers();
		}

		// Token: 0x06005B2A RID: 23338 RVA: 0x001C7448 File Offset: 0x001C5648
		private void LoadServers()
		{
			try
			{
				string text = File.ReadAllText(this.ServersFilePath, Encoding.UTF8);
				this._servers = JsonConvert.DeserializeObject<List<AssetEditorAppMainMenu.Server>>(text);
			}
			catch (FileNotFoundException ex)
			{
				this._servers.Clear();
			}
			catch (Exception exception)
			{
				AssetEditorAppMainMenu.Logger.Error(exception, "Failed to load server list.");
				this._servers.Clear();
			}
			this._servers.Sort(AssetEditorAppMainMenu.ServerSortComparison);
			bool flag = this._app.Stage == AssetEditorApp.AppStage.MainMenu;
			if (flag)
			{
				this._app.Interface.MainMenuView.BuildServerList(true);
			}
		}

		// Token: 0x06005B2B RID: 23339 RVA: 0x001C7504 File Offset: 0x001C5704
		private bool TrySaveServers()
		{
			string serversFilePath = this.ServersFilePath;
			try
			{
				string text = JsonConvert.SerializeObject(this.Servers, 1);
				File.WriteAllText(serversFilePath + ".new", text);
				bool flag = File.Exists(serversFilePath);
				if (flag)
				{
					File.Replace(serversFilePath + ".new", serversFilePath, serversFilePath + ".bak");
				}
				else
				{
					File.Move(serversFilePath + ".new", serversFilePath);
				}
			}
			catch (Exception exception)
			{
				AssetEditorAppMainMenu.Logger.Error(exception, "Failed to save server list to " + serversFilePath);
				return false;
			}
			return true;
		}

		// Token: 0x06005B2C RID: 23340 RVA: 0x001C75AC File Offset: 0x001C57AC
		public void Open()
		{
			this._app.SetStage(AssetEditorApp.AppStage.MainMenu);
		}

		// Token: 0x06005B2D RID: 23341 RVA: 0x001C75BC File Offset: 0x001C57BC
		public void OpenWithDisconnectPopup(string hostname, int port)
		{
			this.DisplayDisconnectPopup = true;
			this._previousServerHostname = hostname;
			this._previousServerPort = port;
			this._app.Interface.MainMenuView.UpdateDisconnectPopup();
			this._app.SetStage(AssetEditorApp.AppStage.MainMenu);
		}

		// Token: 0x06005B2E RID: 23342 RVA: 0x001C75F8 File Offset: 0x001C57F8
		public void CloseDisconnectPopup()
		{
			this.DisplayDisconnectPopup = false;
			this._app.Interface.MainMenuView.UpdateDisconnectPopup();
		}

		// Token: 0x06005B2F RID: 23343 RVA: 0x001C7619 File Offset: 0x001C5819
		public void Reconnect()
		{
			this.ConnectToServer(this._previousServerHostname, this._previousServerPort);
		}

		// Token: 0x06005B30 RID: 23344 RVA: 0x001C7630 File Offset: 0x001C5830
		public void ConnectToServer(string host, int port)
		{
			Debug.Assert(this._connection == null);
			AssetEditorAppMainMenu.Logger.Info<string, int>("Connecting to server {0}:{1}", host, port);
			this.ConnectionErrorMessage = null;
			this.ConnectionStage = AssetEditorAppMainMenu.ConnectionStages.Connecting;
			this._connection = new ConnectionToServer(this._app.Engine, host, port, new Action<Exception>(this.OnConnected), new Action<Exception>(this.OnDisconnected));
			this._app.Interface.MainMenuView.UpdateConnectionStatus(true);
		}

		// Token: 0x06005B31 RID: 23345 RVA: 0x001C76B8 File Offset: 0x001C58B8
		public void ConnectToServer(int index)
		{
			AssetEditorAppMainMenu.Server server = this.Servers[index];
			server.DateLastJoined = DateTime.Now;
			this.TrySaveServers();
			this.ConnectToServer(server.Hostname, server.Port);
		}

		// Token: 0x06005B32 RID: 23346 RVA: 0x001C76F8 File Offset: 0x001C58F8
		private void OnConnected(Exception exception)
		{
			bool flag = exception != null;
			if (flag)
			{
				this.ConnectionErrorMessage = "ui.assetEditor.mainMenu.connection.error.failedToEstablishConnection";
				this._connection.OnDisconnected = null;
				this._connection = null;
				AssetEditorPacketHandler packetHandler = this._packetHandler;
				if (packetHandler != null)
				{
					packetHandler.Dispose();
				}
				this._packetHandler = null;
				this._app.Interface.MainMenuView.UpdateConnectionStatus(true);
			}
			else
			{
				AssetEditorAppMainMenu.Logger.Info("Connection established!");
				this.ConnectionStage = AssetEditorAppMainMenu.ConnectionStages.Authenticating;
				this._packetHandler = new AssetEditorPacketHandler(this._app, this._connection);
				this._connection.OnPacketReceived = new Action<byte[], int>(this._packetHandler.Receive);
				ConnectionMode connectionMode = 6;
				bool isInsecure = this._app.AuthManager.Settings.IsInsecure;
				if (isInsecure)
				{
					connectionMode = 7;
				}
				this._connection.SendPacket(new Connect("f4c63561b2d2f5120b4c81ad1b8544e396088277d88f650aea892b6f0cb113f", 1643968234458L, connectionMode, this._app.Settings.Language ?? Language.SystemLanguage));
				bool flag2 = connectionMode == 7;
				if (flag2)
				{
					this._connection.SendPacket(new SetUsername(this._app.AuthManager.Settings.Username));
				}
				else
				{
					bool flag3 = this._app.AuthManager.CertPathBytes == null;
					if (flag3)
					{
						throw new Exception("Attempted to execute an online-mode handshake while not authenticated!");
					}
					Auth1 packet = new Auth1((sbyte[])this._app.AuthManager.CertPathBytes);
					this._connection.SendPacket(packet);
				}
				this._app.Interface.MainMenuView.UpdateConnectionStatus(true);
			}
		}

		// Token: 0x06005B33 RID: 23347 RVA: 0x001C78A0 File Offset: 0x001C5AA0
		public void OnAuthenticated()
		{
			Debug.Assert(ThreadHelper.IsMainThread());
			AssetEditorAppMainMenu.Logger.Info("User is fully authenticated!");
			ConnectionToServer connection = this._connection;
			AssetEditorPacketHandler packetHandler = this._packetHandler;
			this._connection.OnDisconnected = null;
			this._connection = null;
			this._packetHandler = null;
			string assetPathToOpen = this.AssetPathToOpen;
			AssetIdReference assetIdToOpen = this.AssetIdToOpen;
			this._app.Interface.MainMenuView.UpdateConnectionStatus(true);
			this._app.Editor.OpenAssetEditor(connection, packetHandler);
			bool flag = assetPathToOpen != null;
			if (flag)
			{
				this._app.Editor.OpenAsset(assetPathToOpen);
			}
			else
			{
				bool flag2 = assetIdToOpen.Type != null;
				if (flag2)
				{
					this._app.Editor.OpenAsset(assetIdToOpen);
				}
			}
		}

		// Token: 0x06005B34 RID: 23348 RVA: 0x001C796C File Offset: 0x001C5B6C
		private void OnDisconnected(Exception exception)
		{
			bool flag = this._connection == null;
			if (!flag)
			{
				AssetEditorAppMainMenu.Logger.Error(exception, "Disconnected from server");
				this._connection.OnDisconnected = null;
				this._connection = null;
				AssetEditorPacketHandler packetHandler = this._packetHandler;
				if (packetHandler != null)
				{
					packetHandler.Dispose();
				}
				this._packetHandler = null;
				bool flag2 = this.ServerDisconnectReason != null;
				if (flag2)
				{
					this.ConnectionErrorMessage = this.ServerDisconnectReason;
				}
				else
				{
					bool flag3 = exception != null;
					if (flag3)
					{
						this.ConnectionErrorMessage = "ui.assetEditor.mainMenu.connection.error.failedAuthentication";
					}
				}
				this._app.Interface.MainMenuView.UpdateConnectionStatus(true);
			}
		}

		// Token: 0x06005B35 RID: 23349 RVA: 0x001C7A14 File Offset: 0x001C5C14
		public void CancelConnection()
		{
			bool flag = this._connection == null;
			if (!flag)
			{
				this._connection.OnDisconnected = null;
				this._connection.SendPacketImmediate(new Disconnect("Player abort", 0));
				this._connection.Close();
				this._connection = null;
				AssetEditorPacketHandler packetHandler = this._packetHandler;
				if (packetHandler != null)
				{
					packetHandler.Dispose();
				}
				this._packetHandler = null;
				this._app.Interface.MainMenuView.UpdateConnectionStatus(true);
			}
		}

		// Token: 0x06005B36 RID: 23350 RVA: 0x001C7A98 File Offset: 0x001C5C98
		public void CleanUp()
		{
			this._servers.Sort(AssetEditorAppMainMenu.ServerSortComparison);
			this.AssetIdToOpen = AssetIdReference.None;
			this.AssetPathToOpen = null;
			this.ServerDisconnectReason = null;
			this.DisplayDisconnectPopup = false;
			this._previousServerHostname = null;
			this._previousServerPort = 0;
			this.ConnectionErrorMessage = null;
			this.CancelConnection();
		}

		// Token: 0x06005B37 RID: 23351 RVA: 0x001C7AF8 File Offset: 0x001C5CF8
		private void SaveServers()
		{
			bool flag = this.TrySaveServers();
			if (!flag)
			{
				this._app.Interface.Notifications.AddNotification(2, FormattedMessage.FromMessageId("ui.assetEditor.mainMenu.errors.failedToSaveServerList", null));
			}
		}

		// Token: 0x06005B38 RID: 23352 RVA: 0x001C7B34 File Offset: 0x001C5D34
		public void AddServer(AssetEditorAppMainMenu.Server server)
		{
			this._servers.Add(server);
			this._servers.Sort(AssetEditorAppMainMenu.ServerSortComparison);
			this.SaveServers();
			this._app.Interface.MainMenuView.BuildServerList(true);
		}

		// Token: 0x06005B39 RID: 23353 RVA: 0x001C7B74 File Offset: 0x001C5D74
		public void UpdateServer(int index, string name, string hostname, int port)
		{
			AssetEditorAppMainMenu.Server server = this.Servers[index];
			server.Name = name;
			server.Hostname = hostname;
			server.Port = port;
			this._servers.Sort(AssetEditorAppMainMenu.ServerSortComparison);
			this.SaveServers();
			this._app.Interface.MainMenuView.BuildServerList(true);
		}

		// Token: 0x06005B3A RID: 23354 RVA: 0x001C7BD4 File Offset: 0x001C5DD4
		public void RemoveServer(int index)
		{
			this._servers.RemoveAt(index);
			this.SaveServers();
			this._app.Interface.MainMenuView.BuildServerList(true);
		}

		// Token: 0x04003911 RID: 14609
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x04003912 RID: 14610
		private static readonly Comparison<AssetEditorAppMainMenu.Server> ServerSortComparison = (AssetEditorAppMainMenu.Server a, AssetEditorAppMainMenu.Server b) => b.DateLastJoined.CompareTo(a.DateLastJoined);

		// Token: 0x04003913 RID: 14611
		private readonly AssetEditorApp _app;

		// Token: 0x04003914 RID: 14612
		private AssetEditorPacketHandler _packetHandler;

		// Token: 0x04003915 RID: 14613
		private ConnectionToServer _connection;

		// Token: 0x04003918 RID: 14616
		public string ServerDisconnectReason;

		// Token: 0x04003919 RID: 14617
		private string _previousServerHostname;

		// Token: 0x0400391A RID: 14618
		private int _previousServerPort;

		// Token: 0x0400391B RID: 14619
		public string AssetPathToOpen;

		// Token: 0x0400391C RID: 14620
		public AssetIdReference AssetIdToOpen;

		// Token: 0x0400391E RID: 14622
		private List<AssetEditorAppMainMenu.Server> _servers = new List<AssetEditorAppMainMenu.Server>();

		// Token: 0x02000F7F RID: 3967
		public enum ConnectionStages
		{
			// Token: 0x04004B38 RID: 19256
			Connecting,
			// Token: 0x04004B39 RID: 19257
			Authenticating
		}

		// Token: 0x02000F80 RID: 3968
		public class Server
		{
			// Token: 0x04004B3A RID: 19258
			public string Name;

			// Token: 0x04004B3B RID: 19259
			public string Hostname;

			// Token: 0x04004B3C RID: 19260
			public int Port;

			// Token: 0x04004B3D RID: 19261
			public DateTime DateLastJoined;
		}
	}
}
