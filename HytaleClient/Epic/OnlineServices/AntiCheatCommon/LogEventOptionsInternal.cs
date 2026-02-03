using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatCommon
{
	// Token: 0x020006A4 RID: 1700
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct LogEventOptionsInternal : ISettable<LogEventOptions>, IDisposable
	{
		// Token: 0x17000CD4 RID: 3284
		// (set) Token: 0x06002BB6 RID: 11190 RVA: 0x00040571 File Offset: 0x0003E771
		public IntPtr ClientHandle
		{
			set
			{
				this.m_ClientHandle = value;
			}
		}

		// Token: 0x17000CD5 RID: 3285
		// (set) Token: 0x06002BB7 RID: 11191 RVA: 0x0004057B File Offset: 0x0003E77B
		public uint EventId
		{
			set
			{
				this.m_EventId = value;
			}
		}

		// Token: 0x17000CD6 RID: 3286
		// (set) Token: 0x06002BB8 RID: 11192 RVA: 0x00040585 File Offset: 0x0003E785
		public LogEventParamPair[] Params
		{
			set
			{
				Helper.Set<LogEventParamPair, LogEventParamPairInternal>(ref value, ref this.m_Params, out this.m_ParamsCount);
			}
		}

		// Token: 0x06002BB9 RID: 11193 RVA: 0x0004059C File Offset: 0x0003E79C
		public void Set(ref LogEventOptions other)
		{
			this.m_ApiVersion = 1;
			this.ClientHandle = other.ClientHandle;
			this.EventId = other.EventId;
			this.Params = other.Params;
		}

		// Token: 0x06002BBA RID: 11194 RVA: 0x000405D0 File Offset: 0x0003E7D0
		public void Set(ref LogEventOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.ClientHandle = other.Value.ClientHandle;
				this.EventId = other.Value.EventId;
				this.Params = other.Value.Params;
			}
		}

		// Token: 0x06002BBB RID: 11195 RVA: 0x00040630 File Offset: 0x0003E830
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientHandle);
			Helper.Dispose(ref this.m_Params);
		}

		// Token: 0x0400132A RID: 4906
		private int m_ApiVersion;

		// Token: 0x0400132B RID: 4907
		private IntPtr m_ClientHandle;

		// Token: 0x0400132C RID: 4908
		private uint m_EventId;

		// Token: 0x0400132D RID: 4909
		private uint m_ParamsCount;

		// Token: 0x0400132E RID: 4910
		private IntPtr m_Params;
	}
}
