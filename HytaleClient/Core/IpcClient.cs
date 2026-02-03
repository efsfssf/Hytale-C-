using System;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;

namespace HytaleClient.Core
{
	// Token: 0x02000B7F RID: 2943
	public class IpcClient : Disposable
	{
		// Token: 0x06005A7B RID: 23163 RVA: 0x001C28C2 File Offset: 0x001C0AC2
		private IpcClient()
		{
		}

		// Token: 0x06005A7C RID: 23164 RVA: 0x001C28D8 File Offset: 0x001C0AD8
		private IpcClient(Action<string, JObject> commandReceived)
		{
			this._commandReceived = commandReceived;
			this._thread = new Thread(new ThreadStart(this.ProcessInputThreadStart));
			this._thread.IsBackground = true;
			this._thread.Name = "IpcClient";
			this._thread.Start();
		}

		// Token: 0x06005A7D RID: 23165 RVA: 0x001C2940 File Offset: 0x001C0B40
		private void ProcessInputThreadStart()
		{
			while (!this._cancellationTokenSource.Token.IsCancellationRequested)
			{
				string text = Console.In.ReadLine();
				bool flag = text == null;
				if (flag)
				{
					break;
				}
				text = text.Trim();
				bool flag2 = !text.StartsWith("ipc:");
				if (!flag2)
				{
					string arg;
					JObject arg2;
					try
					{
						string text2 = text.Substring("ipc:".Length).Trim();
						JObject jobject = JObject.Parse(text2);
						arg = (string)jobject["Command"];
						arg2 = (jobject.ContainsKey("Data") ? ((JObject)jobject["Data"]) : null);
					}
					catch (Exception exception)
					{
						IpcClient.Logger.Warn(exception, "Failed to parse ipc command {0}", new object[]
						{
							text
						});
						continue;
					}
					try
					{
						this._commandReceived(arg, arg2);
					}
					catch (Exception exception2)
					{
						IpcClient.Logger.Error(exception2, "IPC message handler threw an exception");
					}
				}
			}
		}

		// Token: 0x06005A7E RID: 23166 RVA: 0x001C2A70 File Offset: 0x001C0C70
		public void SendCommand(string command, JObject data)
		{
			JObject jobject = new JObject();
			jobject.Add("Command", command);
			jobject.Add("Data", data);
			JObject jobject2 = jobject;
			Console.Out.WriteLine("ipc:" + jobject2.ToString(0, Array.Empty<JsonConverter>()));
		}

		// Token: 0x06005A7F RID: 23167 RVA: 0x001C2AC4 File Offset: 0x001C0CC4
		protected override void DoDispose()
		{
			this._cancellationTokenSource.Cancel();
			this._cancellationTokenSource.Dispose();
			Thread thread = this._thread;
			if (thread != null)
			{
				thread.Interrupt();
			}
		}

		// Token: 0x06005A80 RID: 23168 RVA: 0x001C2AF4 File Offset: 0x001C0CF4
		public static IpcClient CreateWriteOnlyClient()
		{
			return new IpcClient();
		}

		// Token: 0x06005A81 RID: 23169 RVA: 0x001C2B0C File Offset: 0x001C0D0C
		public static IpcClient CreateReadWriteClient(Action<string, JObject> commandReceived)
		{
			return new IpcClient(commandReceived);
		}

		// Token: 0x04003886 RID: 14470
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x04003887 RID: 14471
		private const string MessagePrefix = "ipc:";

		// Token: 0x04003888 RID: 14472
		private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

		// Token: 0x04003889 RID: 14473
		private readonly Action<string, JObject> _commandReceived;

		// Token: 0x0400388A RID: 14474
		private Thread _thread;
	}
}
