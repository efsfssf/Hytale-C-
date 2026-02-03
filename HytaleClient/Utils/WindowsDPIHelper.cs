using System;
using System.Runtime.InteropServices;
using HytaleClient.Math;

namespace HytaleClient.Utils
{
	// Token: 0x020007D6 RID: 2006
	public static class WindowsDPIHelper
	{
		// Token: 0x0600344D RID: 13389
		[DllImport("user32.dll", SetLastError = true)]
		private static extern bool SetProcessDpiAwarenessContext(int dpiFlag);

		// Token: 0x0600344E RID: 13390
		[DllImport("SHCore.dll", SetLastError = true)]
		private static extern bool SetProcessDpiAwareness(WindowsDPIHelper.PROCESS_DPI_AWARENESS awareness);

		// Token: 0x0600344F RID: 13391
		[DllImport("user32.dll")]
		private static extern bool SetProcessDPIAware();

		// Token: 0x06003450 RID: 13392 RVA: 0x000542A0 File Offset: 0x000524A0
		public static bool TryEnableDpiAwareness()
		{
			try
			{
				return WindowsDPIHelper.SetProcessDpiAwarenessContext(34);
			}
			catch
			{
			}
			try
			{
				return WindowsDPIHelper.SetProcessDpiAwareness(WindowsDPIHelper.PROCESS_DPI_AWARENESS.Process_Per_Monitor_DPI_Aware);
			}
			catch
			{
			}
			try
			{
				return WindowsDPIHelper.SetProcessDPIAware();
			}
			catch
			{
			}
			return false;
		}

		// Token: 0x06003451 RID: 13393
		[DllImport("user32.dll")]
		private static extern uint GetDpiForWindow(IntPtr hwnd);

		// Token: 0x06003452 RID: 13394
		[DllImport("user32.dll")]
		private static extern IntPtr MonitorFromPoint([In] Point pt, [In] uint dwFlags);

		// Token: 0x06003453 RID: 13395
		[DllImport("shcore.dll")]
		private static extern IntPtr GetDpiForMonitor([In] IntPtr hmonitor, [In] WindowsDPIHelper.DpiType dpiType, out uint dpiX, out uint dpiY);

		// Token: 0x06003454 RID: 13396 RVA: 0x0005430C File Offset: 0x0005250C
		public static bool TryGetDpiForWindow(IntPtr hwnd, out uint dpi)
		{
			try
			{
				dpi = WindowsDPIHelper.GetDpiForWindow(hwnd);
				return true;
			}
			catch
			{
			}
			try
			{
				IntPtr hmonitor = WindowsDPIHelper.MonitorFromPoint(new Point(0, 0), 2U);
				uint num;
				WindowsDPIHelper.GetDpiForMonitor(hmonitor, WindowsDPIHelper.DpiType.Effective, out dpi, out num);
				return dpi > 0U;
			}
			catch
			{
			}
			dpi = 0U;
			return false;
		}

		// Token: 0x04001777 RID: 6007
		public const int ReferenceDPI = 96;

		// Token: 0x04001778 RID: 6008
		private const int MONITOR_DEFAULTTONEAREST = 2;

		// Token: 0x02000C1A RID: 3098
		private enum PROCESS_DPI_AWARENESS
		{
			// Token: 0x04003D92 RID: 15762
			Process_DPI_Unaware,
			// Token: 0x04003D93 RID: 15763
			Process_System_DPI_Aware,
			// Token: 0x04003D94 RID: 15764
			Process_Per_Monitor_DPI_Aware
		}

		// Token: 0x02000C1B RID: 3099
		private enum DPI_AWARENESS_CONTEXT
		{
			// Token: 0x04003D96 RID: 15766
			DPI_AWARENESS_CONTEXT_UNAWARE = 16,
			// Token: 0x04003D97 RID: 15767
			DPI_AWARENESS_CONTEXT_SYSTEM_AWARE,
			// Token: 0x04003D98 RID: 15768
			DPI_AWARENESS_CONTEXT_PER_MONITOR_AWARE,
			// Token: 0x04003D99 RID: 15769
			DPI_AWARENESS_CONTEXT_PER_MONITOR_AWARE_V2 = 34
		}

		// Token: 0x02000C1C RID: 3100
		private enum DpiType
		{
			// Token: 0x04003D9B RID: 15771
			Effective,
			// Token: 0x04003D9C RID: 15772
			Angular,
			// Token: 0x04003D9D RID: 15773
			Raw
		}
	}
}
