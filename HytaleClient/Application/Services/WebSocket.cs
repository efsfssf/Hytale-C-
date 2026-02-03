using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.WebSockets;
using System.Net.WebSockets.Managed;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HytaleClient.Utils;
using NLog;

namespace HytaleClient.Application.Services
{
	// Token: 0x02000BFE RID: 3070
	public class WebSocket
	{
		// Token: 0x060061EC RID: 25068 RVA: 0x002042A8 File Offset: 0x002024A8
		public WebSocket(string uri, Action<WebSocket> onConnected, Action<byte[], WebSocket> onMessage, Action<Exception, WebSocket> onDisconnected)
		{
			this._onConnected = onConnected;
			this._onMessage = onMessage;
			this._onDisconnected = onDisconnected;
			this._uri = uri;
			this._cancellationTokenSource = new CancellationTokenSource();
			this._threadCancellationToken = this._cancellationTokenSource.Token;
		}

		// Token: 0x060061ED RID: 25069 RVA: 0x002042F8 File Offset: 0x002024F8
		public void Close()
		{
			bool flag = this._clientWebSocket != null && this._clientWebSocket.State == 2;
			if (flag)
			{
				try
				{
					this._clientWebSocket.CloseAsync(1000, "Client closed the connection", CancellationToken.None).GetAwaiter().GetResult();
					Task.Run(delegate()
					{
						Action<Exception, WebSocket> onDisconnected = this._onDisconnected;
						if (onDisconnected != null)
						{
							onDisconnected(null, this);
						}
					});
				}
				catch (Exception exception)
				{
					WebSocket.Logger.Error(exception, "Failed to close WebSocket:");
				}
			}
			this._cancellationTokenSource.Cancel();
			this._cancellationTokenSource.Dispose();
			Thread thread = this._thread;
			if (thread != null)
			{
				thread.Join();
			}
			WebSocket clientWebSocket = this._clientWebSocket;
			if (clientWebSocket != null)
			{
				clientWebSocket.Dispose();
			}
		}

		// Token: 0x060061EE RID: 25070 RVA: 0x002043C8 File Offset: 0x002025C8
		public void SendMessageAsync(byte[] bytes)
		{
			bool flag = this._clientWebSocket == null || this._clientWebSocket.State != 2;
			if (flag)
			{
				throw new Exception("Connection is not open.");
			}
			this._clientWebSocket.SendAsync(new ArraySegment<byte>(bytes), 1, true, this._threadCancellationToken).ContinueWith(delegate(Task t)
			{
				bool flag2 = t.IsFaulted || t.IsCanceled;
				bool flag3 = flag2;
				if (flag3)
				{
					bool flag4 = t.Exception != null;
					if (flag4)
					{
						WebSocket.Logger.Error(t.Exception, "Unable to send message via websocket");
					}
					else
					{
						WebSocket.Logger.Error("Unable to send message via websocket because operation was canceled");
					}
				}
			});
		}

		// Token: 0x060061EF RID: 25071 RVA: 0x00204440 File Offset: 0x00202640
		public bool IsAlive()
		{
			return this._clientWebSocket != null && this._clientWebSocket.State == 2;
		}

		// Token: 0x060061F0 RID: 25072 RVA: 0x0020445C File Offset: 0x0020265C
		public void ConnectAsync()
		{
			ServicePointManager.SecurityProtocol = 3072;
			this._clientWebSocket = SystemClientWebSocket.CreateClientWebSocket();
			this.InitializeWebsocketOptions();
			try
			{
				SystemClientWebSocket.ConnectAsync(this._clientWebSocket, new Uri(this._uri), CancellationToken.None).ContinueWith(delegate(Task t)
				{
					bool flag = t.IsFaulted || this._clientWebSocket.State != 2;
					if (flag)
					{
						WebSocket.Logger.Error("Unable to connect");
						bool flag2 = t.Exception != null;
						if (flag2)
						{
							Action<Exception, WebSocket> onDisconnected = this._onDisconnected;
							if (onDisconnected != null)
							{
								onDisconnected(t.Exception, this);
							}
						}
						else
						{
							Action<Exception, WebSocket> onDisconnected2 = this._onDisconnected;
							if (onDisconnected2 != null)
							{
								onDisconnected2(null, this);
							}
						}
					}
					else
					{
						this.OnOpen();
					}
				});
			}
			catch (Exception exception)
			{
				WebSocket.Logger.Error(exception, "Error while connecting");
			}
		}

		// Token: 0x060061F1 RID: 25073 RVA: 0x002044E0 File Offset: 0x002026E0
		private void InitializeWebsocketOptions()
		{
			bool managedWebSocketRequired = SystemClientWebSocket.ManagedWebSocketRequired;
			if (managedWebSocketRequired)
			{
				ClientWebSocket clientWebSocket = this._clientWebSocket as ClientWebSocket;
				WebSocket.Logger.Info("Using managed websocket");
				clientWebSocket.Options.KeepAliveInterval = TimeSpan.FromSeconds(1.0);
				clientWebSocket.Options.SetRequestHeader("hytale-services-auth-version", "f4c63561b2d2f5120b4c81ad1b8544e396088277d88f650aea892b6f0cb113f");
				clientWebSocket.Options.SetRequestHeader("hytale-services-auth-compiletime", 1643968234458L.ToString());
				clientWebSocket.Options.SetRequestHeader("hytale-services-client-version", "9d8ae0bf16cd64d040f9b8199273a8d174fe9809e896daca052b49e4ee79f");
				clientWebSocket.Options.SetRequestHeader("hytale-services-client-compiletime", 1551618657161L.ToString());
			}
			else
			{
				ClientWebSocket clientWebSocket2 = this._clientWebSocket as ClientWebSocket;
				clientWebSocket2.Options.KeepAliveInterval = TimeSpan.FromSeconds(1.0);
				clientWebSocket2.Options.SetRequestHeader("hytale-services-auth-version", "f4c63561b2d2f5120b4c81ad1b8544e396088277d88f650aea892b6f0cb113f");
				clientWebSocket2.Options.SetRequestHeader("hytale-services-auth-compiletime", 1643968234458L.ToString());
				clientWebSocket2.Options.SetRequestHeader("hytale-services-client-version", "9d8ae0bf16cd64d040f9b8199273a8d174fe9809e896daca052b49e4ee79f");
				clientWebSocket2.Options.SetRequestHeader("hytale-services-client-compiletime", 1551618657161L.ToString());
			}
		}

		// Token: 0x060061F2 RID: 25074 RVA: 0x00204640 File Offset: 0x00202840
		private void OnOpen()
		{
			bool flag = this._onConnected != null;
			if (flag)
			{
				Task.Run(delegate()
				{
					this._onConnected(this);
				});
			}
			this.StartListen();
		}

		// Token: 0x060061F3 RID: 25075 RVA: 0x00204676 File Offset: 0x00202876
		private void StartListen()
		{
			this._thread = new Thread(new ThreadStart(this.BackgroundThreadStart))
			{
				Name = "WebsocketThread",
				IsBackground = true
			};
			this._thread.Start();
		}

		// Token: 0x060061F4 RID: 25076 RVA: 0x002046B0 File Offset: 0x002028B0
		private void BackgroundThreadStart()
		{
			Debug.Assert(ThreadHelper.IsOnThread(this._thread));
			WebSocket.Logger.Info("Websocket starts listening");
			ArraySegment<byte> arraySegment = WebSocket.CreateClientBuffer(1024, 16);
			using (MemoryStream memoryStream = new MemoryStream())
			{
				while (!this._threadCancellationToken.IsCancellationRequested)
				{
					bool flag = this._clientWebSocket.CloseStatus != null || this._clientWebSocket.State != 2;
					if (flag)
					{
						Exception arg = (this._clientWebSocket.CloseStatus != null && this._clientWebSocket.CloseStatus.Value == 1000) ? null : new Exception(string.Format("Closed with CloseStatus {0}, reason {1}, current state : {2}", this._clientWebSocket.CloseStatus, this._clientWebSocket.CloseStatusDescription, this._clientWebSocket.State));
						Action<Exception, WebSocket> onDisconnected = this._onDisconnected;
						if (onDisconnected != null)
						{
							onDisconnected(arg, this);
						}
						break;
					}
					try
					{
						WebSocketReceiveResult result;
						do
						{
							result = this._clientWebSocket.ReceiveAsync(arraySegment, this._threadCancellationToken).GetAwaiter().GetResult();
							memoryStream.Write(arraySegment.Array, arraySegment.Offset, result.Count);
						}
						while (!result.EndOfMessage);
						memoryStream.Seek(0L, SeekOrigin.Begin);
						WebSocketMessageType messageType = result.MessageType;
						WebSocketMessageType webSocketMessageType = messageType;
						if (webSocketMessageType != 1)
						{
							if (webSocketMessageType != 2)
							{
								WebSocket.Logger.Error<WebSocketMessageType, string>("Received illegal message {0} : {1}", result.MessageType, Encoding.UTF8.GetString(memoryStream.ToArray()));
							}
							else
							{
								WebSocket.Logger.Info("Close message received");
								this._clientWebSocket.CloseOutputAsync(1000, string.Empty, CancellationToken.None);
							}
						}
						else
						{
							this._onMessage(memoryStream.ToArray(), this);
						}
					}
					catch (OperationCanceledException)
					{
						WebSocket.Logger.Info("Receive had been canceled");
						break;
					}
					catch (Exception ex)
					{
						WebSocket.Logger.Error(ex, "Exception during reception");
						Action<Exception, WebSocket> onDisconnected2 = this._onDisconnected;
						if (onDisconnected2 != null)
						{
							onDisconnected2(ex, this);
						}
						break;
					}
					memoryStream.SetLength(0L);
				}
			}
			WebSocket.Logger.Info("Websocket stopped listening");
		}

		// Token: 0x04003CDF RID: 15583
		private const int ReceiveBufferSize = 1024;

		// Token: 0x04003CE0 RID: 15584
		private const int SendBufferSize = 16;

		// Token: 0x04003CE1 RID: 15585
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x04003CE2 RID: 15586
		private readonly CancellationTokenSource _cancellationTokenSource;

		// Token: 0x04003CE3 RID: 15587
		private readonly CancellationToken _threadCancellationToken;

		// Token: 0x04003CE4 RID: 15588
		private readonly string _uri;

		// Token: 0x04003CE5 RID: 15589
		private readonly Action<WebSocket> _onConnected;

		// Token: 0x04003CE6 RID: 15590
		private readonly Action<byte[], WebSocket> _onMessage;

		// Token: 0x04003CE7 RID: 15591
		private readonly Action<Exception, WebSocket> _onDisconnected;

		// Token: 0x04003CE8 RID: 15592
		private WebSocket _clientWebSocket;

		// Token: 0x04003CE9 RID: 15593
		private Thread _thread;
	}
}
