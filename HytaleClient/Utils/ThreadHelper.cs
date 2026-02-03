using System;
using System.Threading;

namespace HytaleClient.Utils
{
	// Token: 0x020007D3 RID: 2003
	internal static class ThreadHelper
	{
		// Token: 0x06003439 RID: 13369 RVA: 0x00053CD0 File Offset: 0x00051ED0
		static ThreadHelper()
		{
			Thread currentThread = Thread.CurrentThread;
			currentThread.Name = "MainThread";
			ThreadHelper.MainThreadId = currentThread.ManagedThreadId;
		}

		// Token: 0x0600343A RID: 13370 RVA: 0x00053CFB File Offset: 0x00051EFB
		public static void Initialize()
		{
		}

		// Token: 0x0600343B RID: 13371 RVA: 0x00053CFE File Offset: 0x00051EFE
		public static bool IsMainThread()
		{
			return Thread.CurrentThread.ManagedThreadId == ThreadHelper.MainThreadId;
		}

		// Token: 0x0600343C RID: 13372 RVA: 0x00053D11 File Offset: 0x00051F11
		public static bool IsOnThread(Thread thread)
		{
			return Thread.CurrentThread.ManagedThreadId == thread.ManagedThreadId;
		}

		// Token: 0x0400176B RID: 5995
		private static readonly int MainThreadId;
	}
}
