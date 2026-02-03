using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Hypixel.ProtoPlus;
using HytaleClient.Core;
using HytaleClient.Protocol;
using HytaleClient.Utils;
using NLog;

namespace HytaleClient.Networking
{
	// Token: 0x020007D9 RID: 2009
	internal class ConnectionToServer
	{
		// Token: 0x06003463 RID: 13411 RVA: 0x00054894 File Offset: 0x00052A94
		public ConnectionToServer(Engine engine, string hostname, int port, Action<Exception> onConnected, Action<Exception> onDisconnected)
		{
			ConnectionToServer <>4__this = this;
			this._engine = engine;
			this.Hostname = hostname;
			this.Port = port;
			this._onConnected = onConnected;
			this.OnDisconnected = onDisconnected;
			this._connectAndReceiveThread = new Thread(delegate()
			{
				<>4__this.ConnectAndReceiveThreadStart(hostname, port);
			})
			{
				Name = "ConnectAndReceive",
				IsBackground = true
			};
			this._connectAndReceiveThread.Start();
			this._sendThread = new Thread(new ThreadStart(this.SendPackets))
			{
				Name = "SocketSend",
				IsBackground = true
			};
			this._sendThread.Start();
		}

		// Token: 0x06003464 RID: 13412 RVA: 0x00054988 File Offset: 0x00052B88
		private void ConnectAndReceiveThreadStart(string hostname, int port)
		{
			try
			{
				IPAddress[] hostAddresses = Dns.GetHostAddresses(hostname);
				bool flag = hostAddresses.Length == 0;
				if (flag)
				{
					throw new Exception(string.Format("Failed to find any addresses for: {0}:{1}", hostname, port));
				}
				Exception ex = null;
				foreach (IPAddress ipaddress in hostAddresses)
				{
					try
					{
						this._socket = new Socket(ipaddress.AddressFamily, 1, 6)
						{
							NoDelay = true,
							LingerState = new LingerOption(true, 1)
						};
						this._socket.Connect(ipaddress, port);
						break;
					}
					catch (SocketException ex2)
					{
						ex = ex2;
						Socket socket = this._socket;
						if (socket != null)
						{
							socket.Dispose();
						}
						this._socket = null;
					}
				}
				bool flag2 = this._socket == null;
				if (flag2)
				{
					throw ex ?? new Exception("Failed to find host address for $" + hostname);
				}
			}
			catch (ThreadAbortException)
			{
				return;
			}
			catch (Exception ex3)
			{
				Exception exception2 = ex3;
				Exception exception = exception2;
				ConnectionToServer.Logger.Error(exception, "Failed to connect to {0}:{1}", new object[]
				{
					hostname,
					port
				});
				bool flag3 = this._onConnected != null;
				if (flag3)
				{
					this._engine.RunOnMainThread(this._engine, delegate
					{
						Action<Exception> onConnected = this._onConnected;
						if (onConnected != null)
						{
							onConnected(exception);
						}
					}, false, false);
				}
				return;
			}
			bool flag4 = this._onConnected != null;
			if (flag4)
			{
				this._engine.RunOnMainThread(this._engine, delegate
				{
					Action<Exception> onConnected = this._onConnected;
					if (onConnected != null)
					{
						onConnected(null);
					}
				}, false, false);
			}
			byte[] array2 = new byte[65557];
			int num = 0;
			byte[] array3 = new byte[array2.Length + 4];
			ConnectionToServer.SocketBufferStream socketBufferStream = new ConnectionToServer.SocketBufferStream
			{
				Buffer = new byte[4]
			};
			ConnectionToServer.SocketBufferStream socketBufferStream2 = new ConnectionToServer.SocketBufferStream();
			Exception wrapperException;
			for (;;)
			{
				int num2 = 0;
				try
				{
					num2 = this._socket.Receive(array3);
				}
				catch (SocketException)
				{
				}
				bool flag5 = num2 == 0;
				if (flag5)
				{
					break;
				}
				int num4;
				for (int j = 0; j < num2; j += num4)
				{
					int num3 = 0;
					num4 = 0;
					bool flag6 = socketBufferStream2.Buffer == null;
					if (flag6)
					{
						num3 = Math.Min(num2 - j, 4 - socketBufferStream.Offset);
						Buffer.BlockCopy(array3, j, socketBufferStream.Buffer, socketBufferStream.Offset, num3);
						socketBufferStream.Offset += num3;
						bool flag7 = socketBufferStream.Offset == 4;
						if (flag7)
						{
							num = BitConverter.ToInt32(socketBufferStream.Buffer, 0);
							socketBufferStream.Offset = 0;
							Interlocked.Add(ref this.ReceivedPacketLength, num);
							bool flag8 = num <= array2.Length;
							if (flag8)
							{
								socketBufferStream2.Buffer = array2;
							}
							else
							{
								ConnectionToServer.Logger.Warn<int, int>("Received a packet with a payload of {0} bytes (bigger than {1}). If this happens often, the default size should be adjusted", num, array2.Length);
								socketBufferStream2.Buffer = new byte[num];
							}
						}
					}
					j += num3;
					bool flag9 = socketBufferStream2.Buffer != null;
					if (flag9)
					{
						num4 = Math.Min(num2 - j, num - socketBufferStream2.Offset);
						Buffer.BlockCopy(array3, j, socketBufferStream2.Buffer, socketBufferStream2.Offset, num4);
						socketBufferStream2.Offset += num4;
						bool flag10 = socketBufferStream2.Offset == num;
						if (flag10)
						{
							try
							{
								this.OnPacketReceived(socketBufferStream2.Buffer, num);
							}
							catch (Exception innerException)
							{
								Exception wrapperException = new Exception("Failed to deserialize packet: ", innerException);
								bool flag11 = this.OnDisconnected != null;
								if (flag11)
								{
									this._engine.RunOnMainThread(this._engine, delegate
									{
										Action<Exception> onDisconnected = this.OnDisconnected;
										if (onDisconnected != null)
										{
											onDisconnected(wrapperException);
										}
									}, false, false);
								}
								return;
							}
							socketBufferStream2.Buffer = null;
							socketBufferStream2.Offset = 0;
						}
					}
				}
			}
			wrapperException = new Exception("Failed to receive frame from server (zero bytes received)");
			bool flag12 = this.OnDisconnected != null;
			if (flag12)
			{
				this._engine.RunOnMainThread(this._engine, delegate
				{
					Action<Exception> onDisconnected = this.OnDisconnected;
					if (onDisconnected != null)
					{
						onDisconnected(wrapperException);
					}
				}, false, false);
			}
		}

		// Token: 0x06003465 RID: 13413 RVA: 0x00054DF4 File Offset: 0x00052FF4
		private void SendPackets()
		{
			int num = 0;
			Stopwatch stopwatch = new Stopwatch();
			MemoryStream memoryStream = new MemoryStream();
			ProtoBinaryWriter protoBinaryWriter = new ProtoBinaryWriter(memoryStream);
			for (;;)
			{
				stopwatch.Restart();
				for (;;)
				{
					ProtoPacket protoPacket;
					bool flag = this._sendQueue.TryDequeue(out protoPacket);
					if (!flag)
					{
						break;
					}
					int num2 = (int)memoryStream.Position;
					protoBinaryWriter.Write(0);
					protoPacket.WritePacket(protoBinaryWriter);
					int num3 = (int)memoryStream.Position;
					int num4 = num3 - num2 - 4;
					bool flag2 = num4 <= 0;
					if (flag2)
					{
						goto Block_2;
					}
					memoryStream.Position = (long)num2;
					protoBinaryWriter.Write(num4);
					memoryStream.Position = (long)num3;
					int num5 = num4 + 4;
					num += num5;
					ref ConnectionToServer.PacketStat ptr = ref this.PacketStats[protoPacket.GetId()];
					bool flag3 = ptr.Name == null;
					if (flag3)
					{
						ptr.Name = protoPacket.GetType().Name;
					}
					ptr.AddSentSize((long)num4);
				}
				bool flag4 = num > 0;
				if (flag4)
				{
					try
					{
						Interlocked.Add(ref this.SentPacketLength, num);
						this._socket.Send(memoryStream.GetBuffer(), num, 0);
						num = 0;
					}
					catch (SocketException value)
					{
						ConnectionToServer.Logger.Error<SocketException>(value);
					}
				}
				memoryStream.SetLength(0L);
				int num6 = 30 - (int)stopwatch.ElapsedMilliseconds;
				bool flag5 = num6 > 0;
				if (flag5)
				{
					this._sendTriggerEvent.WaitOne(num6);
				}
			}
			Block_2:
			throw new ArgumentException("Packet length can't be 0");
		}

		// Token: 0x06003466 RID: 13414 RVA: 0x00054F7C File Offset: 0x0005317C
		public void Close()
		{
			Debug.Assert(ThreadHelper.IsMainThread());
			bool flag = this._connectAndReceiveThread != null;
			if (flag)
			{
				this._connectAndReceiveThread.Abort();
				this._connectAndReceiveThread = null;
			}
			bool flag2 = this._sendThread != null;
			if (flag2)
			{
				this._sendThread.Abort();
				this._sendThread = null;
			}
			Socket socket = this._socket;
			if (socket != null)
			{
				socket.Close();
			}
			this._socket = null;
		}

		// Token: 0x06003467 RID: 13415 RVA: 0x00054FF2 File Offset: 0x000531F2
		public void TriggerSend()
		{
			this._sendTriggerEvent.Set();
		}

		// Token: 0x06003468 RID: 13416 RVA: 0x00055004 File Offset: 0x00053204
		public void SendPacket(ProtoPacket packet)
		{
			bool flag = packet == null;
			if (flag)
			{
				throw new ArgumentException("Packet can't be null");
			}
			this._sendQueue.Enqueue(packet);
		}

		// Token: 0x06003469 RID: 13417 RVA: 0x00055034 File Offset: 0x00053234
		public void SendPacketImmediate(ProtoPacket packet)
		{
			bool flag = packet == null;
			if (flag)
			{
				throw new ArgumentException("Packet can't be null");
			}
			bool flag2 = this._socket == null || !this._socket.Connected;
			if (!flag2)
			{
				using (MemoryStream memoryStream = new MemoryStream())
				{
					using (ProtoBinaryWriter protoBinaryWriter = new ProtoBinaryWriter(memoryStream))
					{
						protoBinaryWriter.Write(0);
						packet.WritePacket(protoBinaryWriter);
						bool flag3 = memoryStream.Length <= 4L;
						if (flag3)
						{
							throw new ArgumentException("Packet length can't be 0");
						}
						protoBinaryWriter.Seek(0, SeekOrigin.Begin);
						long num = memoryStream.Length - 4L;
						protoBinaryWriter.Write((int)num);
						protoBinaryWriter.Seek(0, SeekOrigin.End);
						byte[] array = memoryStream.ToArray();
						try
						{
							bool flag4 = this._socket == null || !this._socket.Connected;
							if (!flag4)
							{
								Interlocked.Add(ref this.SentPacketLength, array.Length);
								this._socket.Send(array, array.Length, 0);
								ref ConnectionToServer.PacketStat ptr = ref this.PacketStats[packet.GetId()];
								bool flag5 = ptr.Name == null;
								if (flag5)
								{
									ptr.Name = packet.GetType().Name;
								}
								ptr.AddSentSize(num);
							}
						}
						catch (SocketException ex)
						{
							SocketException exception2 = ex;
							SocketException exception = exception2;
							this._engine.RunOnMainThread(this._engine, delegate
							{
								Action<Exception> onDisconnected = this.OnDisconnected;
								if (onDisconnected != null)
								{
									onDisconnected(exception);
								}
							}, true, false);
						}
					}
				}
			}
		}

		// Token: 0x0600346A RID: 13418 RVA: 0x00055214 File Offset: 0x00053414
		public void ResetPacketStats()
		{
			this.PacketStats = new ConnectionToServer.PacketStat[243];
		}

		// Token: 0x04001784 RID: 6020
		private const int MaxSendInterval = 30;

		// Token: 0x04001785 RID: 6021
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x04001786 RID: 6022
		private readonly Engine _engine;

		// Token: 0x04001787 RID: 6023
		public readonly string Hostname;

		// Token: 0x04001788 RID: 6024
		public readonly int Port;

		// Token: 0x04001789 RID: 6025
		private readonly Action<Exception> _onConnected;

		// Token: 0x0400178A RID: 6026
		public Action<byte[], int> OnPacketReceived;

		// Token: 0x0400178B RID: 6027
		public Action<Exception> OnDisconnected;

		// Token: 0x0400178C RID: 6028
		public int SentPacketLength;

		// Token: 0x0400178D RID: 6029
		public int ReceivedPacketLength;

		// Token: 0x0400178E RID: 6030
		public ReferralConnect Referral;

		// Token: 0x0400178F RID: 6031
		private Socket _socket;

		// Token: 0x04001790 RID: 6032
		private Thread _connectAndReceiveThread;

		// Token: 0x04001791 RID: 6033
		private Thread _sendThread;

		// Token: 0x04001792 RID: 6034
		private ConcurrentQueue<ProtoPacket> _sendQueue = new ConcurrentQueue<ProtoPacket>();

		// Token: 0x04001793 RID: 6035
		private AutoResetEvent _sendTriggerEvent = new AutoResetEvent(true);

		// Token: 0x04001794 RID: 6036
		public ConnectionToServer.PacketStat[] PacketStats = new ConnectionToServer.PacketStat[243];

		// Token: 0x02000C22 RID: 3106
		private class SocketBufferStream
		{
			// Token: 0x04003DA5 RID: 15781
			public int Offset;

			// Token: 0x04003DA6 RID: 15782
			public byte[] Buffer;
		}

		// Token: 0x02000C23 RID: 3107
		public struct PacketStat
		{
			// Token: 0x0600629F RID: 25247 RVA: 0x00206B02 File Offset: 0x00204D02
			public void AddReceivedTime(long elapsed)
			{
				this.ReceivedCount += 1L;
				this.ReceivedTotalElapsed += elapsed;
			}

			// Token: 0x060062A0 RID: 25248 RVA: 0x00206B22 File Offset: 0x00204D22
			public void AddSentSize(long size)
			{
				this.SentCount += 1L;
				this.SentTotalSize += size;
			}

			// Token: 0x04003DA7 RID: 15783
			public string Name;

			// Token: 0x04003DA8 RID: 15784
			public long ReceivedCount;

			// Token: 0x04003DA9 RID: 15785
			public long ReceivedTotalElapsed;

			// Token: 0x04003DAA RID: 15786
			public long SentCount;

			// Token: 0x04003DAB RID: 15787
			public long SentTotalSize;
		}
	}
}
