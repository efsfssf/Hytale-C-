using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Hypixel.ProtoPlus;
using HytaleClient.Auth.Proto;
using HytaleClient.AuthHandshake.Proto;
using HytaleClient.Core;
using HytaleClient.Utils;
using NLog;

namespace HytaleClient.Application.Services
{
	// Token: 0x02000BFB RID: 3067
	internal class ServicesClient
	{
		// Token: 0x17001411 RID: 5137
		// (get) Token: 0x060061B1 RID: 25009 RVA: 0x00201DB3 File Offset: 0x001FFFB3
		// (set) Token: 0x060061B2 RID: 25010 RVA: 0x00201DBB File Offset: 0x001FFFBB
		public ServicesAuthState AuthState { get; private set; }

		// Token: 0x060061B3 RID: 25011 RVA: 0x00201DC4 File Offset: 0x001FFFC4
		public ServicesClient(App app, string host, int port, bool secure, string path, ServicesPacketHandler handler, Action onDisconnected)
		{
			this._app = app;
			this._host = host;
			this._port = port;
			this._onDisconnected = onDisconnected;
			this._packetHandler = handler;
			string text = secure ? "wss" : "ws";
			this._uri = string.Format("{0}://{1}:{2}/{3}", new object[]
			{
				text,
				this._host,
				this._port,
				path
			});
			bool disableServices = OptionsHelper.DisableServices;
			if (disableServices)
			{
				ServicesClient.Logger.Info("Refusing to connect to services since they have been disabled");
			}
			else
			{
				bool flag = !this._app.AuthManager.Settings.IsInsecure;
				if (flag)
				{
					this.Connect();
				}
				else
				{
					ServicesClient.Logger.Info("Refusing to connect to services while IsInsecure is true!");
				}
			}
		}

		// Token: 0x060061B4 RID: 25012 RVA: 0x00201E9C File Offset: 0x0020009C
		private void Connect()
		{
			this._connecting = true;
			this._app.Engine.RunOnMainThread(this._app.Engine, delegate
			{
				this._app.Interface.OnServicesStateChanged(HytaleServices.ServiceState.Connecting);
			}, true, false);
			this.AuthState = new ServicesAuthState(this._app.AuthManager, this);
			CancellationTokenSource cancellation = this._connectionCancellationTokenSource = new CancellationTokenSource();
			Action <>9__4;
			Action<Task> <>9__5;
			this._webSocket = new WebSocket(this._uri, delegate(WebSocket socket)
			{
				this._reconnectBackoff = 0;
				this._connecting = false;
				Engine engine = this._app.Engine;
				Disposable engine2 = this._app.Engine;
				Action action;
				if ((action = <>9__4) == null)
				{
					action = (<>9__4 = delegate()
					{
						this._app.Interface.OnServicesStateChanged(HytaleServices.ServiceState.Authenticating);
					});
				}
				engine.RunOnMainThread(engine2, action, false, false);
				ServicesClient.Logger.Info("Connected successfully to services websocket at {0}", this._uri);
			}, delegate(byte[] bytes, WebSocket socket)
			{
				try
				{
					using (ProtoBinaryReader protoBinaryReader = ProtoBinaryReader.Create(bytes))
					{
						ProtoPacket protoPacket = this.AuthState.Authed ? PacketReader.ReadPacket(protoBinaryReader) : PacketReader.ReadPacket(protoBinaryReader);
						ServicesClient.Logger.Info<ProtoPacket>("Received packet from websocket channel: {0}", protoPacket);
						this._packetHandler.Receive(protoPacket, this);
					}
				}
				catch
				{
					ServicesClient.Logger.Info<byte[], WebSocket>("Failed to read packet from websocket: {0}, {0}", bytes, socket);
					this.Close();
				}
			}, delegate(Exception exception, WebSocket socket)
			{
				this._connecting = false;
				this._onDisconnected();
				bool connectionClosed = this._connectionClosed;
				if (!connectionClosed)
				{
					this._reconnectBackoff++;
					bool flag = this._reconnectBackoff > 10;
					if (flag)
					{
						this._reconnectBackoff = 10;
					}
					ServicesClient.Logger.Info<string, int>("Delaying reconnect to {0} by {1} seconds...", this._uri, this._reconnectBackoff - 1);
					Task task2 = Task.Delay(1000 * (this._reconnectBackoff - 1), cancellation.Token);
					Action<Task> continuationAction;
					if ((continuationAction = <>9__5) == null)
					{
						continuationAction = (<>9__5 = delegate(Task task)
						{
							bool flag3 = task.IsCanceled || this._connectionClosed;
							if (!flag3)
							{
								ServicesClient.Logger.Info("Reconnecting to {0}.", this._uri);
								this.Connect();
							}
						});
					}
					task2.ContinueWith(continuationAction, cancellation.Token);
					bool flag2 = exception != null;
					if (flag2)
					{
						ServicesClient.Logger.Error(exception, "Got exception from websocket with message {0} and exception:", new object[]
						{
							exception.Message
						});
					}
					else
					{
						ServicesClient.Logger.Info("Got socket closed");
					}
				}
			});
			ServicesClient.Logger.Info<ServicesEndpoint, string>("Connecting to websocket at {0}: {1}", OptionsHelper.Endpoint, this._uri);
			this._webSocket.ConnectAsync();
		}

		// Token: 0x060061B5 RID: 25013 RVA: 0x00201F70 File Offset: 0x00200170
		public bool IsConnected()
		{
			return this._webSocket != null && this._webSocket.IsAlive();
		}

		// Token: 0x060061B6 RID: 25014 RVA: 0x00201F98 File Offset: 0x00200198
		public void Write(ProtoPacket packet)
		{
			bool flag = packet == null;
			if (flag)
			{
				throw new ArgumentException("Packet can't be null");
			}
			bool flag2 = this._connecting || this._webSocket == null || !this._webSocket.IsAlive();
			if (flag2)
			{
				Logger logger = ServicesClient.Logger;
				string message = "Attempted to write packet out {0} while connecting or not connected: {1} and websocket state: {2}";
				bool connecting = this._connecting;
				WebSocket webSocket = this._webSocket;
				logger.Warn<ProtoPacket, bool, bool?>(message, packet, connecting, (webSocket != null) ? new bool?(webSocket.IsAlive()) : null);
			}
			else
			{
				byte[] array;
				using (MemoryStream memoryStream = new MemoryStream())
				{
					using (ProtoBinaryWriter protoBinaryWriter = new ProtoBinaryWriter(memoryStream))
					{
						packet.WritePacket(protoBinaryWriter);
						array = memoryStream.ToArray();
					}
				}
				try
				{
					this._webSocket.SendMessageAsync(array);
					ServicesClient.Logger.Info<ProtoPacket, WebSocket, int>("Sent packet {0} to {1} with payload length {2}", packet, this._webSocket, array.Length);
				}
				catch (Exception exception)
				{
					ServicesClient.Logger.Error(exception, "Failed to send packet {0} to {1} with payload length {2}", new object[]
					{
						packet,
						this._webSocket,
						array.Length
					});
				}
			}
		}

		// Token: 0x060061B7 RID: 25015 RVA: 0x002020E8 File Offset: 0x002002E8
		public void Close()
		{
			ServicesClient.Logger.Info<ServicesEndpoint, string>("Closing websocket for {0}: {1}", OptionsHelper.Endpoint, this._uri);
			this._connectionClosed = true;
			ServicesPacketHandler packetHandler = this._packetHandler;
			if (packetHandler != null)
			{
				packetHandler.Dispose();
			}
			CancellationTokenSource connectionCancellationTokenSource = this._connectionCancellationTokenSource;
			if (connectionCancellationTokenSource != null)
			{
				connectionCancellationTokenSource.Cancel();
			}
			bool flag = this._connecting || this.IsConnected();
			if (flag)
			{
				this._webSocket.Close();
			}
		}

		// Token: 0x060061B8 RID: 25016 RVA: 0x00202160 File Offset: 0x00200360
		public override string ToString()
		{
			return string.Format("{0}: {1}, {2}: {3}, {4}: {5}, {6}: {7}, {8}: {9}", new object[]
			{
				"_host",
				this._host,
				"_port",
				this._port,
				"_webSocket",
				this._webSocket,
				"_connecting",
				this._connecting,
				"AuthState",
				this.AuthState
			});
		}

		// Token: 0x04003CC4 RID: 15556
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x04003CC6 RID: 15558
		private readonly App _app;

		// Token: 0x04003CC7 RID: 15559
		private readonly string _host;

		// Token: 0x04003CC8 RID: 15560
		private readonly int _port;

		// Token: 0x04003CC9 RID: 15561
		private readonly string _uri;

		// Token: 0x04003CCA RID: 15562
		private WebSocket _webSocket;

		// Token: 0x04003CCB RID: 15563
		private bool _connecting;

		// Token: 0x04003CCC RID: 15564
		private Action _onDisconnected;

		// Token: 0x04003CCD RID: 15565
		private CancellationTokenSource _connectionCancellationTokenSource;

		// Token: 0x04003CCE RID: 15566
		private ServicesPacketHandler _packetHandler;

		// Token: 0x04003CCF RID: 15567
		private int _reconnectBackoff;

		// Token: 0x04003CD0 RID: 15568
		private bool _connectionClosed;
	}
}
