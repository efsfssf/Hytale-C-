using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Hypixel.ProtoPlus;
using HytaleClient.Core;
using HytaleClient.Interface.Messages;
using HytaleClient.Net.Protocol;
using HytaleClient.Protocol;
using HytaleClient.Utils;
using Newtonsoft.Json.Linq;
using NLog;

namespace HytaleClient.Networking
{
	// Token: 0x020007D8 RID: 2008
	internal abstract class BasePacketHandler : Disposable
	{
		// Token: 0x06003457 RID: 13399 RVA: 0x000543D4 File Offset: 0x000525D4
		public int AddPendingCallback<T>(Disposable disposable, Action<FailureReply, T> callback) where T : ProtoPacket
		{
			bool flag = this._pendingCallbacks.Count > 1000;
			if (flag)
			{
				bool flag2 = (DateTime.Now - this._lastCallbackWarning).TotalSeconds > 5.0;
				if (flag2)
				{
					this._lastCallbackWarning = DateTime.Now;
					BasePacketHandler.Logger.Warn("There are currently more than 1000 pending packet callbacks. Removing oldest callback...");
				}
				int num = Enumerable.First<int>(this._pendingCallbacks.Keys);
				BasePacketHandler.PendingCallback pendingCallback;
				this._pendingCallbacks.TryRemove(num, ref pendingCallback);
				pendingCallback.Callback(new FailureReply(num, BsonHelper.ToBson(JToken.FromObject(FormattedMessage.FromMessageId("ui.general.callback.cancelled", null)))), null);
			}
			int num2 = Interlocked.Add(ref this._lastCallbackToken, 1);
			this._pendingCallbacks[num2] = new BasePacketHandler.PendingCallback
			{
				Callback = delegate(FailureReply err, ProtoPacket res)
				{
					callback(err, (T)((object)res));
				},
				Disposable = disposable
			};
			return num2;
		}

		// Token: 0x06003458 RID: 13400 RVA: 0x000544DC File Offset: 0x000526DC
		protected void CallPendingCallback(int token, ProtoPacket responsePacket, FailureReply failurePacket)
		{
			BasePacketHandler.PendingCallback pendingCallback;
			bool flag = this._pendingCallbacks.TryRemove(token, ref pendingCallback);
			if (flag)
			{
				bool disposed = pendingCallback.Disposable.Disposed;
				if (!disposed)
				{
					Action<FailureReply, ProtoPacket> callback = pendingCallback.Callback;
					if (callback != null)
					{
						callback(failurePacket, responsePacket);
					}
				}
			}
		}

		// Token: 0x06003459 RID: 13401 RVA: 0x00054524 File Offset: 0x00052724
		protected BasePacketHandler(Engine engine, ConnectionToServer connection)
		{
			this._engine = engine;
			this._connection = connection;
			this._threadCancellationToken = this._threadCancellationTokenSource.Token;
			this._thread = new Thread(new ThreadStart(this.ProcessPacketsThreadStart))
			{
				Name = "BackgroundPacketHandler",
				IsBackground = true
			};
			this._thread.Start();
		}

		// Token: 0x17000FA8 RID: 4008
		// (get) Token: 0x0600345A RID: 13402 RVA: 0x000545B0 File Offset: 0x000527B0
		public bool IsOnThread
		{
			get
			{
				return ThreadHelper.IsOnThread(this._thread);
			}
		}

		// Token: 0x0600345B RID: 13403 RVA: 0x000545BD File Offset: 0x000527BD
		protected override void DoDispose()
		{
			Debug.Assert(ThreadHelper.IsMainThread());
			this._threadCancellationTokenSource.Cancel();
			this._thread.Join();
			this._threadCancellationTokenSource.Dispose();
		}

		// Token: 0x0600345C RID: 13404 RVA: 0x000545F0 File Offset: 0x000527F0
		public void Receive(byte[] buffer, int payloadLength)
		{
			using (ProtoBinaryReader protoBinaryReader = ProtoBinaryReader.Create(buffer, payloadLength))
			{
				this.Receive(PacketReader.ReadPacket(protoBinaryReader));
			}
		}

		// Token: 0x0600345D RID: 13405 RVA: 0x00054634 File Offset: 0x00052834
		private void Receive(ProtoPacket packet)
		{
			bool flag = packet.GetId() == 1;
			if (flag)
			{
				string reason = ((Disconnect)packet).Reason;
				this.SetDisconnectReason(reason);
			}
			else
			{
				bool flag2 = packet.GetId() == 2;
				if (flag2)
				{
					Ping ping = (Ping)packet;
					DateTime utcNow = DateTime.UtcNow;
					this.ProcessPingPacket(ping);
					this._connection.SendPacket(new Pong(ping.Id, TimeHelper.DateTimeToInstantData(utcNow), 0, (short)this._packets.Count));
				}
				this._packets.Add(packet, this._threadCancellationToken);
			}
		}

		// Token: 0x0600345E RID: 13406 RVA: 0x000546CC File Offset: 0x000528CC
		private void ProcessPacketsThreadStart()
		{
			Debug.Assert(ThreadHelper.IsOnThread(this._thread));
			Stopwatch stopwatch = Stopwatch.StartNew();
			ProtoPacket protoPacket = null;
			try
			{
				while (!this._threadCancellationToken.IsCancellationRequested)
				{
					try
					{
						protoPacket = this._packets.Take(this._threadCancellationToken);
					}
					catch (OperationCanceledException)
					{
						break;
					}
					stopwatch.Restart();
					this.ProcessPacket(protoPacket);
					ref ConnectionToServer.PacketStat ptr = ref this._connection.PacketStats[protoPacket.GetId()];
					bool flag = ptr.Name == null;
					if (flag)
					{
						ptr.Name = protoPacket.GetType().Name;
					}
					ptr.AddReceivedTime(stopwatch.ElapsedTicks);
				}
			}
			catch (Exception ex)
			{
				BasePacketHandler.<>c__DisplayClass19_0 CS$<>8__locals1 = new BasePacketHandler.<>c__DisplayClass19_0();
				CS$<>8__locals1.<>4__this = this;
				Exception e = ex;
				CS$<>8__locals1.e = e;
				BasePacketHandler.Logger.Error(CS$<>8__locals1.e, "Exception when handling packet {0} {1}:", new object[]
				{
					(protoPacket != null) ? new int?(protoPacket.GetId()) : null,
					(protoPacket != null) ? protoPacket.GetType().Name : null
				});
				string reason = string.Format("Exception when handling packet {0} {1}: {2}", protoPacket.GetId(), protoPacket.GetType().Name, CS$<>8__locals1.e.Message);
				this._engine.RunOnMainThread(this._engine, delegate
				{
					CS$<>8__locals1.<>4__this.SetDisconnectReason(reason);
					CS$<>8__locals1.<>4__this._connection.SendPacketImmediate(new Disconnect(reason, 1));
					CS$<>8__locals1.<>4__this._connection.Close();
					CS$<>8__locals1.<>4__this._connection.OnDisconnected(CS$<>8__locals1.e);
				}, true, false);
			}
		}

		// Token: 0x0600345F RID: 13407 RVA: 0x00054880 File Offset: 0x00052A80
		protected virtual void SetDisconnectReason(string reason)
		{
		}

		// Token: 0x06003460 RID: 13408 RVA: 0x00054883 File Offset: 0x00052A83
		protected virtual void ProcessPingPacket(Ping packet)
		{
		}

		// Token: 0x06003461 RID: 13409
		protected abstract void ProcessPacket(ProtoPacket packet);

		// Token: 0x0400177A RID: 6010
		private ConcurrentDictionary<int, BasePacketHandler.PendingCallback> _pendingCallbacks = new ConcurrentDictionary<int, BasePacketHandler.PendingCallback>();

		// Token: 0x0400177B RID: 6011
		private int _lastCallbackToken;

		// Token: 0x0400177C RID: 6012
		private DateTime _lastCallbackWarning;

		// Token: 0x0400177D RID: 6013
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x0400177E RID: 6014
		private readonly Engine _engine;

		// Token: 0x0400177F RID: 6015
		protected readonly ConnectionToServer _connection;

		// Token: 0x04001780 RID: 6016
		private readonly BlockingCollection<ProtoPacket> _packets = new BlockingCollection<ProtoPacket>();

		// Token: 0x04001781 RID: 6017
		protected readonly Thread _thread;

		// Token: 0x04001782 RID: 6018
		private readonly CancellationTokenSource _threadCancellationTokenSource = new CancellationTokenSource();

		// Token: 0x04001783 RID: 6019
		private readonly CancellationToken _threadCancellationToken;

		// Token: 0x02000C1E RID: 3102
		private class PendingCallback
		{
			// Token: 0x04003D9E RID: 15774
			public Action<FailureReply, ProtoPacket> Callback;

			// Token: 0x04003D9F RID: 15775
			public Disposable Disposable;
		}
	}
}
