using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using HytaleClient.Application;
using HytaleClient.Utils;
using NLog;

namespace HytaleClient.Networking
{
	// Token: 0x020007DC RID: 2012
	internal class SingleplayerServer
	{
		// Token: 0x06003518 RID: 13592 RVA: 0x0005E8A4 File Offset: 0x0005CAA4
		public SingleplayerServer(App app, string directoryName, Action<string, float> onProgress, Action onReady, Action onShutdown)
		{
			this._app = app;
			this._onProgress = onProgress;
			this._onReady = onReady;
			this._onShutdown = onShutdown;
			string str = Paths.TrimBackslash(Path.Combine(Paths.Saves, directoryName));
			string str2 = Paths.TrimBackslash(Paths.BuiltInAssets);
			string str3 = Paths.TrimBackslash(Paths.Server);
			this.Port = SingleplayerServer.FindFreeTcpPort();
			string text = Path.Combine(Paths.UserData, "Server");
			Directory.CreateDirectory(text);
			List<string> list = new List<string>();
			list.Add("-jar \"" + str3 + "\"");
			list.Add(string.Format("--client-pid {0}", Process.GetCurrentProcess().Id));
			list.Add(string.Format("--bind {0}", this.Port));
			list.Add("--assets=\"" + str2 + "\"");
			list.Add("--singleplayer");
			list.Add(string.Format("--ownerUuid=\"{0}\"", this._app.AuthManager.GetPlayerUuid()));
			list.Add("--owner=\"" + this._app.Username + "\"");
			list.Add("--universe=\"" + str + "\"");
			bool flag = !this._app.AuthManager.Settings.IsInsecure;
			if (flag)
			{
				string text2 = Path.Combine(text, ".spPrivateKey");
				string text3 = Path.Combine(text, ".spCert");
				this._app.AuthManager.WritePemDataSp(text2, text3);
				list.Add("--spkey=\"" + text2 + "\"");
				list.Add("--spcert=\"" + text3 + "\"");
			}
			list.AddRange(OptionsHelper.CustomServerArgumentList);
			SingleplayerServer.Logger.Info<string, List<string>>("Starting server: {0} {1}", Paths.Java, list);
			this.Process = Process.Start(new ProcessStartInfo
			{
				FileName = Paths.Java,
				Arguments = string.Join(" ", list),
				UseShellExecute = false,
				CreateNoWindow = true,
				RedirectStandardOutput = true,
				RedirectStandardError = true,
				RedirectStandardInput = true,
				WorkingDirectory = text
			});
			Debug.Assert(this.Process != null, "Process != null");
			this.Process.Exited += this.OnServerProcessExit;
			this.Process.EnableRaisingEvents = true;
			this.Process.OutputDataReceived += new DataReceivedEventHandler(this.OnOutputDataReceived);
			this.Process.ErrorDataReceived += new DataReceivedEventHandler(this.OnErrorDataReceived);
			this.Process.BeginOutputReadLine();
			this.Process.BeginErrorReadLine();
			AppDomain.CurrentDomain.ProcessExit += this.OnClientProcessExit;
		}

		// Token: 0x06003519 RID: 13593 RVA: 0x0005EBB4 File Offset: 0x0005CDB4
		private void OnOutputDataReceived(object sender, DataReceivedEventArgs e)
		{
			string data = e.Data;
			bool flag = data == null;
			if (!flag)
			{
				bool flag2 = data.StartsWith("-=|");
				if (flag2)
				{
					string[] array = data.Split(new char[]
					{
						'|'
					});
					string phase = array[1];
					this._lastProgress.Restart();
					bool flag3 = phase == "Shutdown";
					if (flag3)
					{
						this.ShutdownMessage = array[2];
					}
					else
					{
						float progress = float.Parse(array[2], CultureInfo.InvariantCulture);
						this._app.Engine.RunOnMainThread(this._app.Engine, delegate
						{
							Action<string, float> onProgress = this._onProgress;
							if (onProgress != null)
							{
								onProgress(phase, progress);
							}
						}, false, false);
					}
				}
				else
				{
					bool flag4 = data.Contains(">> Singleplayer Ready <<") && !this._isStopping;
					if (flag4)
					{
						SingleplayerServer.Logger.Info("Singleplayer server is ready. Starting to connect...");
						this._app.Engine.RunOnMainThread(this._app.Engine, delegate
						{
							Action onReady = this._onReady;
							if (onReady != null)
							{
								onReady();
							}
						}, false, false);
					}
				}
			}
		}

		// Token: 0x0600351A RID: 13594 RVA: 0x0005ECE4 File Offset: 0x0005CEE4
		private void OnErrorDataReceived(object sender, DataReceivedEventArgs e)
		{
			string data = e.Data;
			bool flag = data == null;
			if (!flag)
			{
				bool flag2 = data.Contains("Failed to shutdown correctly dumping!");
				if (flag2)
				{
					this._isDumping = true;
				}
				SingleplayerServer.Logger.Warn("ERROR - {0}", data);
			}
		}

		// Token: 0x0600351B RID: 13595 RVA: 0x0005ED2C File Offset: 0x0005CF2C
		public void Close()
		{
			Debug.Assert(ThreadHelper.IsMainThread());
			bool flag = !this._isStopping && !this.Process.HasExited;
			if (flag)
			{
				this.StopServer();
			}
			this._onProgress = null;
			this._onReady = null;
		}

		// Token: 0x0600351C RID: 13596 RVA: 0x0005ED78 File Offset: 0x0005CF78
		private void OnClientProcessExit(object sender, EventArgs eventArgs)
		{
			SingleplayerServer.Logger.Info("Client process exit");
			bool flag = this._isStopping || this.Process.HasExited;
			if (!flag)
			{
				this.StopServer();
			}
		}

		// Token: 0x0600351D RID: 13597 RVA: 0x0005EDBC File Offset: 0x0005CFBC
		private void OnServerProcessExit(object sender, EventArgs eventArgs)
		{
			Debug.Assert(this.Process.HasExited);
			SingleplayerServer.Logger.Info("Server process exited with code {0}", this.Process.ExitCode);
			AppDomain.CurrentDomain.ProcessExit -= this.OnClientProcessExit;
			AffinityHelper.SetupDefaultAffinity();
			this._app.Engine.RunOnMainThread(this._app.Engine, this._onShutdown, true, false);
		}

		// Token: 0x0600351E RID: 13598 RVA: 0x0005EE38 File Offset: 0x0005D038
		private void StopServer()
		{
			Debug.Assert(!this._isStopping, "StopServer method already got called");
			SingleplayerServer.Logger.Info("Stopping server...");
			this._isStopping = true;
			AppDomain.CurrentDomain.ProcessExit -= this.OnClientProcessExit;
			this.Process.StandardInput.WriteLine("stop");
			this._lastProgress.Restart();
			ThreadPool.QueueUserWorkItem(delegate(object _)
			{
				bool flag = false;
				int num = 10000;
				while (!this.Process.WaitForExit(1000))
				{
					bool flag2 = this._lastProgress.ElapsedMilliseconds > (long)num;
					if (flag2)
					{
						bool flag3 = !flag;
						if (!flag3)
						{
							SingleplayerServer.Logger.Warn("Failed to stop server cleanly!");
							this.Process.Kill();
							return;
						}
						flag = true;
						this.Process.StandardInput.WriteLine("dump");
						this._app.Engine.RunOnMainThread(this._app.Engine, delegate
						{
							Action<string, float> onProgress = this._onProgress;
							if (onProgress != null)
							{
								onProgress("Server seems frozen. Dumping", 0f);
							}
						}, false, false);
						num = 20000;
						this._lastProgress.Restart();
					}
				}
				SingleplayerServer.Logger.Info("Stopped server, exit code: {0}", this.Process.ExitCode);
			});
		}

		// Token: 0x0600351F RID: 13599 RVA: 0x0005EEBC File Offset: 0x0005D0BC
		private static int FindFreeTcpPort()
		{
			TcpListener tcpListener = new TcpListener(IPAddress.Loopback, 0);
			tcpListener.Start();
			int port = ((IPEndPoint)tcpListener.LocalEndpoint).Port;
			tcpListener.Stop();
			return port;
		}

		// Token: 0x040017CD RID: 6093
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x040017CE RID: 6094
		public readonly Process Process;

		// Token: 0x040017CF RID: 6095
		public readonly int Port;

		// Token: 0x040017D0 RID: 6096
		private readonly App _app;

		// Token: 0x040017D1 RID: 6097
		private Action<string, float> _onProgress;

		// Token: 0x040017D2 RID: 6098
		private Action _onReady;

		// Token: 0x040017D3 RID: 6099
		private readonly Action _onShutdown;

		// Token: 0x040017D4 RID: 6100
		private bool _isDumping;

		// Token: 0x040017D5 RID: 6101
		private bool _isStopping;

		// Token: 0x040017D6 RID: 6102
		public volatile string ShutdownMessage = null;

		// Token: 0x040017D7 RID: 6103
		private Stopwatch _lastProgress = Stopwatch.StartNew();
	}
}
