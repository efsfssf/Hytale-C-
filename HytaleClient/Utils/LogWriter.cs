using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using NLog;
using NLog.Config;
using NLog.Layouts;
using NLog.Targets;
using SDL2;

namespace HytaleClient.Utils
{
	// Token: 0x020007C5 RID: 1989
	internal static class LogWriter
	{
		// Token: 0x17000F7E RID: 3966
		// (get) Token: 0x06003386 RID: 13190 RVA: 0x00050336 File Offset: 0x0004E536
		// (set) Token: 0x06003387 RID: 13191 RVA: 0x0005033D File Offset: 0x0004E53D
		public static string LogPath { get; private set; }

		// Token: 0x06003389 RID: 13193 RVA: 0x0005035C File Offset: 0x0004E55C
		public static void Start()
		{
			bool flag = !Debugger.IsAttached;
			if (flag)
			{
				Trace.Listeners.Add(new ConsoleTraceListener());
			}
			string text = OptionsHelper.LogFileOverride;
			bool flag2 = text == null;
			if (flag2)
			{
				bool flag3 = !Directory.Exists(LogWriter.LogsFolderPath);
				if (flag3)
				{
					Directory.CreateDirectory(LogWriter.LogsFolderPath);
				}
				else
				{
					LogWriter.CleanLogDirectory(LogWriter.LogsFolderPath);
				}
				text = Paths.EnsureUniqueFilename(Path.Combine(LogWriter.LogsFolderPath, DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss")), OptionsHelper.LaunchEditor ? "_editor.log" : "_client.log");
			}
			else
			{
				bool flag4 = File.Exists(text);
				if (flag4)
				{
					Console.WriteLine("Specified log file already exists at '" + text + "', deleting");
					File.Delete(text);
				}
			}
			Console.WriteLine("Set log path to " + text);
			try
			{
				Trace.Listeners.Add(new LogWriter.HytaleLogListener());
				Trace.AutoFlush = true;
				Layout layout = Layout.FromString("${longdate}|${level:uppercase=true}|${logger}|${message} ${exception:format=tostring,Data:separator=\n}");
				LoggingConfiguration loggingConfiguration = new LoggingConfiguration();
				loggingConfiguration.AddRule(LogLevel.Info, LogLevel.Fatal, new ConsoleTarget("logconsole")
				{
					Encoding = Encoding.UTF8,
					Layout = layout
				}, "*");
				loggingConfiguration.AddRule(LogLevel.Debug, LogLevel.Fatal, new FileTarget("logfile")
				{
					FileName = text,
					Encoding = Encoding.UTF8,
					Layout = layout,
					KeepFileOpen = true,
					OpenFileCacheTimeout = 30
				}, "*");
				bool isAttached = Debugger.IsAttached;
				if (isAttached)
				{
					loggingConfiguration.AddRule(LogLevel.Debug, LogLevel.Fatal, new DebuggerTarget("debugger")
					{
						Layout = layout
					}, "*");
				}
				LogManager.Configuration = loggingConfiguration;
				LogWriter._logger = LogManager.GetCurrentClassLogger();
			}
			catch (Exception ex)
			{
				string title = "Failed to setup logging.";
				string message = "Could not setup log at " + text + ".\n" + ex.Message;
				SDL.SDL_ShowSimpleMessageBox(SDL.SDL_MessageBoxFlags.SDL_MESSAGEBOX_ERROR, title, message, IntPtr.Zero);
				return;
			}
			Debug.WriteLine("Log started.");
			LogWriter.LogPath = text;
		}

		// Token: 0x0600338A RID: 13194 RVA: 0x000505A0 File Offset: 0x0004E7A0
		private static void CleanLogDirectory(string dir)
		{
			try
			{
				string[] files = Directory.GetFiles(dir);
				bool flag = files.Length >= 10;
				if (flag)
				{
					Array.Sort<string>(files, StringComparer.Create(CultureInfo.InvariantCulture, true));
					for (int i = 0; i <= files.Length - 10; i++)
					{
						File.Delete(files[i]);
					}
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x04001722 RID: 5922
		private static Logger _logger;

		// Token: 0x04001723 RID: 5923
		private const int MaxLogs = 10;

		// Token: 0x04001725 RID: 5925
		private static readonly string LogsFolderPath = Path.Combine(Paths.UserData, "Logs");

		// Token: 0x02000C15 RID: 3093
		private class HytaleLogListener : TraceListener
		{
			// Token: 0x06006270 RID: 25200 RVA: 0x0020653F File Offset: 0x0020473F
			public override void Write(string message)
			{
				LogWriter._logger.Info(message);
			}

			// Token: 0x06006271 RID: 25201 RVA: 0x0020654E File Offset: 0x0020474E
			public override void WriteLine(string message)
			{
				LogWriter._logger.Info(message);
			}

			// Token: 0x06006272 RID: 25202 RVA: 0x00206560 File Offset: 0x00204760
			public override void Fail(string message, string detailMessage)
			{
				bool flag = BuildInfo.Platform == Platform.Windows;
				if (flag)
				{
					base.Fail(message, detailMessage);
				}
				else
				{
					StackTrace stackTrace = new StackTrace(true);
					string text = "";
					bool flag2 = !string.IsNullOrEmpty(message);
					if (flag2)
					{
						text += message;
						bool flag3 = !string.IsNullOrEmpty(detailMessage);
						if (flag3)
						{
							text = text + ": '" + detailMessage + "'";
						}
						text += "\n";
					}
					else
					{
						bool flag4 = !string.IsNullOrEmpty(detailMessage);
						if (flag4)
						{
							text += detailMessage;
							text += "\n";
						}
					}
					text += stackTrace.ToString();
					SDL.SDL_MessageBoxData sdl_MessageBoxData = new SDL.SDL_MessageBoxData
					{
						flags = SDL.SDL_MessageBoxFlags.SDL_MESSAGEBOX_ERROR,
						title = "Fail",
						message = text,
						numbuttons = 2,
						buttons = new SDL.SDL_MessageBoxButtonData[]
						{
							new SDL.SDL_MessageBoxButtonData
							{
								flags = SDL.SDL_MessageBoxButtonFlags.SDL_MESSAGEBOX_BUTTON_ESCAPEKEY_DEFAULT,
								buttonid = 0,
								text = "Ignore"
							},
							new SDL.SDL_MessageBoxButtonData
							{
								flags = SDL.SDL_MessageBoxButtonFlags.SDL_MESSAGEBOX_BUTTON_RETURNKEY_DEFAULT,
								buttonid = 1,
								text = "Abort"
							}
						}
					};
					int num;
					SDL.SDL_ShowMessageBox(ref sdl_MessageBoxData, out num);
					bool flag5 = num == 1;
					if (flag5)
					{
						throw new Exception(text);
					}
					LogWriter._logger.Error("Fail: " + text);
				}
			}
		}
	}
}
