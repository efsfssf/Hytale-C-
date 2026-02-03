using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using NLog;
using SDL2;

namespace HytaleClient.Utils
{
	// Token: 0x020007BE RID: 1982
	internal static class CrashHandler
	{
		// Token: 0x06003361 RID: 13153 RVA: 0x0004F34C File Offset: 0x0004D54C
		public static void Hook()
		{
			bool flag = !CrashHandler._isHooked;
			if (flag)
			{
				CrashHandler._isHooked = true;
				AppDomain.CurrentDomain.UnhandledException += delegate(object sender, UnhandledExceptionEventArgs e)
				{
					CrashHandler.Crash(e.ExceptionObject as Exception);
				};
			}
		}

		// Token: 0x06003362 RID: 13154 RVA: 0x0004F398 File Offset: 0x0004D598
		private static void Crash(Exception crashException)
		{
			bool isCrashing = CrashHandler.IsCrashing;
			if (!isCrashing)
			{
				CrashHandler.IsCrashing = true;
				Exception ex = crashException;
				StringBuilder stringBuilder = new StringBuilder();
				while (ex != null)
				{
					stringBuilder.AppendLine(ex.ToString());
					stringBuilder.AppendLine("--------------------");
					ex = ex.InnerException;
				}
				CrashHandler.Logger.Error<StringBuilder>(stringBuilder);
				StringBuilder stringBuilder2 = new StringBuilder("A critical error occured:");
				stringBuilder2.AppendLine();
				stringBuilder2.AppendLine(crashException.Message);
				stringBuilder2.AppendLine();
				bool flag = LogWriter.LogPath != null;
				if (flag)
				{
					stringBuilder2.AppendLine("A log was saved at:");
					stringBuilder2.AppendLine(LogWriter.LogPath);
				}
				SDL.SDL_ShowSimpleMessageBox(SDL.SDL_MessageBoxFlags.SDL_MESSAGEBOX_ERROR, "Hytale has crashed", stringBuilder2.ToString(), IntPtr.Zero);
				bool flag2 = LogWriter.LogPath != null;
				if (flag2)
				{
					Process.Start(Path.GetDirectoryName(LogWriter.LogPath));
				}
				Environment.Exit(1);
			}
		}

		// Token: 0x04001712 RID: 5906
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x04001713 RID: 5907
		public static bool IsCrashing;

		// Token: 0x04001714 RID: 5908
		private static bool _isHooked;
	}
}
