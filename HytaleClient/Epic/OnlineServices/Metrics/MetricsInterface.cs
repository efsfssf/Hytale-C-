using System;

namespace Epic.OnlineServices.Metrics
{
	// Token: 0x02000356 RID: 854
	public sealed class MetricsInterface : Handle
	{
		// Token: 0x0600175E RID: 5982 RVA: 0x000222E8 File Offset: 0x000204E8
		public MetricsInterface()
		{
		}

		// Token: 0x0600175F RID: 5983 RVA: 0x000222F2 File Offset: 0x000204F2
		public MetricsInterface(IntPtr innerHandle) : base(innerHandle)
		{
		}

		// Token: 0x06001760 RID: 5984 RVA: 0x00022300 File Offset: 0x00020500
		public Result BeginPlayerSession(ref BeginPlayerSessionOptions options)
		{
			BeginPlayerSessionOptionsInternal beginPlayerSessionOptionsInternal = default(BeginPlayerSessionOptionsInternal);
			beginPlayerSessionOptionsInternal.Set(ref options);
			Result result = Bindings.EOS_Metrics_BeginPlayerSession(base.InnerHandle, ref beginPlayerSessionOptionsInternal);
			Helper.Dispose<BeginPlayerSessionOptionsInternal>(ref beginPlayerSessionOptionsInternal);
			return result;
		}

		// Token: 0x06001761 RID: 5985 RVA: 0x0002233C File Offset: 0x0002053C
		public Result EndPlayerSession(ref EndPlayerSessionOptions options)
		{
			EndPlayerSessionOptionsInternal endPlayerSessionOptionsInternal = default(EndPlayerSessionOptionsInternal);
			endPlayerSessionOptionsInternal.Set(ref options);
			Result result = Bindings.EOS_Metrics_EndPlayerSession(base.InnerHandle, ref endPlayerSessionOptionsInternal);
			Helper.Dispose<EndPlayerSessionOptionsInternal>(ref endPlayerSessionOptionsInternal);
			return result;
		}

		// Token: 0x04000A2B RID: 2603
		public const int BeginplayersessionApiLatest = 1;

		// Token: 0x04000A2C RID: 2604
		public const int EndplayersessionApiLatest = 1;
	}
}
