using System;
using System.Runtime.InteropServices;

namespace HytaleClient.Utils
{
	// Token: 0x020007D7 RID: 2007
	internal class WindowsUtils
	{
		// Token: 0x06003455 RID: 13397 RVA: 0x00054378 File Offset: 0x00052578
		public static int SetApplicationUserModelId(string appId)
		{
			Version version = Environment.OSVersion.Version;
			bool flag = version.Major > 6 || (version.Major == 6 && version.Minor >= 1);
			int result;
			if (flag)
			{
				result = WindowsUtils.NativeMethods.SetCurrentProcessExplicitAppUserModelID(appId);
			}
			else
			{
				result = -1;
			}
			return result;
		}

		// Token: 0x04001779 RID: 6009
		public const int ResultOk = 0;

		// Token: 0x02000C1D RID: 3101
		private static class NativeMethods
		{
			// Token: 0x06006297 RID: 25239
			[DllImport("shell32.dll")]
			internal static extern int SetCurrentProcessExplicitAppUserModelID([MarshalAs(UnmanagedType.LPWStr)] string appID);
		}
	}
}
