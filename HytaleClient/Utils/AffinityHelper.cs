using System;
using System.Diagnostics;
using System.Threading;
using HytaleClient.Math;
using NLog;

namespace HytaleClient.Utils
{
	// Token: 0x020007AF RID: 1967
	internal static class AffinityHelper
	{
		// Token: 0x060032F4 RID: 13044 RVA: 0x0004BF50 File Offset: 0x0004A150
		public static void Setup()
		{
			Thread.CurrentThread.Priority = ThreadPriority.AboveNormal;
			bool flag = OptionsHelper.DisableAffinity || !AffinityHelper.Enabled;
			if (!flag)
			{
				AffinityHelper.SetupDefaultAffinity();
			}
		}

		// Token: 0x060032F5 RID: 13045 RVA: 0x0004BF88 File Offset: 0x0004A188
		public static void SetupDefaultAffinity()
		{
			bool flag = OptionsHelper.DisableAffinity || !AffinityHelper.Enabled;
			if (!flag)
			{
				AffinityHelper.Logger.Info("Setup Default Affinity");
				Process.GetCurrentProcess().ProcessorAffinity = AffinityHelper.DefaultClientAffinity;
			}
		}

		// Token: 0x060032F6 RID: 13046 RVA: 0x0004BFD0 File Offset: 0x0004A1D0
		public static void SetupSingleplayerAffinity(Process serverProcess)
		{
			bool flag = OptionsHelper.DisableAffinity || !AffinityHelper.Enabled;
			if (!flag)
			{
				AffinityHelper.Logger.Info("Setup Singleplayer Affinity");
				Process.GetCurrentProcess().ProcessorAffinity = AffinityHelper.SingleplayerClientAffinity;
				serverProcess.ProcessorAffinity = AffinityHelper.SingleplayerServerAffinity;
			}
		}

		// Token: 0x040016DC RID: 5852
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x040016DD RID: 5853
		private const float SingleplayerClientAffinityRatio = 0.43f;

		// Token: 0x040016DE RID: 5854
		private static readonly IntPtr DefaultClientAffinity = (IntPtr)(1L << Environment.ProcessorCount) - 1;

		// Token: 0x040016DF RID: 5855
		private static readonly int SingleplayerClientProcessorCount = MathHelper.Round((float)Environment.ProcessorCount * 0.43f);

		// Token: 0x040016E0 RID: 5856
		private static readonly int SingleplayerServerProcessorCount = MathHelper.Round((float)Environment.ProcessorCount * 0.57f);

		// Token: 0x040016E1 RID: 5857
		private static readonly IntPtr SingleplayerClientAffinity = (IntPtr)((1L << AffinityHelper.SingleplayerClientProcessorCount) - 1L);

		// Token: 0x040016E2 RID: 5858
		private static readonly IntPtr SingleplayerServerAffinity = (IntPtr)((1L << AffinityHelper.SingleplayerServerProcessorCount) - 1L << AffinityHelper.SingleplayerClientProcessorCount);

		// Token: 0x040016E3 RID: 5859
		private static readonly bool Enabled = Environment.ProcessorCount <= 8;
	}
}
