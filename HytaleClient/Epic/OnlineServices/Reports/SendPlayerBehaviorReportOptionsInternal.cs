using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Reports
{
	// Token: 0x020002A4 RID: 676
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct SendPlayerBehaviorReportOptionsInternal : ISettable<SendPlayerBehaviorReportOptions>, IDisposable
	{
		// Token: 0x17000502 RID: 1282
		// (set) Token: 0x060012E6 RID: 4838 RVA: 0x0001B82E File Offset: 0x00019A2E
		public ProductUserId ReporterUserId
		{
			set
			{
				Helper.Set(value, ref this.m_ReporterUserId);
			}
		}

		// Token: 0x17000503 RID: 1283
		// (set) Token: 0x060012E7 RID: 4839 RVA: 0x0001B83E File Offset: 0x00019A3E
		public ProductUserId ReportedUserId
		{
			set
			{
				Helper.Set(value, ref this.m_ReportedUserId);
			}
		}

		// Token: 0x17000504 RID: 1284
		// (set) Token: 0x060012E8 RID: 4840 RVA: 0x0001B84E File Offset: 0x00019A4E
		public PlayerReportsCategory Category
		{
			set
			{
				this.m_Category = value;
			}
		}

		// Token: 0x17000505 RID: 1285
		// (set) Token: 0x060012E9 RID: 4841 RVA: 0x0001B858 File Offset: 0x00019A58
		public Utf8String Message
		{
			set
			{
				Helper.Set(value, ref this.m_Message);
			}
		}

		// Token: 0x17000506 RID: 1286
		// (set) Token: 0x060012EA RID: 4842 RVA: 0x0001B868 File Offset: 0x00019A68
		public Utf8String Context
		{
			set
			{
				Helper.Set(value, ref this.m_Context);
			}
		}

		// Token: 0x060012EB RID: 4843 RVA: 0x0001B878 File Offset: 0x00019A78
		public void Set(ref SendPlayerBehaviorReportOptions other)
		{
			this.m_ApiVersion = 2;
			this.ReporterUserId = other.ReporterUserId;
			this.ReportedUserId = other.ReportedUserId;
			this.Category = other.Category;
			this.Message = other.Message;
			this.Context = other.Context;
		}

		// Token: 0x060012EC RID: 4844 RVA: 0x0001B8D0 File Offset: 0x00019AD0
		public void Set(ref SendPlayerBehaviorReportOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 2;
				this.ReporterUserId = other.Value.ReporterUserId;
				this.ReportedUserId = other.Value.ReportedUserId;
				this.Category = other.Value.Category;
				this.Message = other.Value.Message;
				this.Context = other.Value.Context;
			}
		}

		// Token: 0x060012ED RID: 4845 RVA: 0x0001B95A File Offset: 0x00019B5A
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ReporterUserId);
			Helper.Dispose(ref this.m_ReportedUserId);
			Helper.Dispose(ref this.m_Message);
			Helper.Dispose(ref this.m_Context);
		}

		// Token: 0x0400084C RID: 2124
		private int m_ApiVersion;

		// Token: 0x0400084D RID: 2125
		private IntPtr m_ReporterUserId;

		// Token: 0x0400084E RID: 2126
		private IntPtr m_ReportedUserId;

		// Token: 0x0400084F RID: 2127
		private PlayerReportsCategory m_Category;

		// Token: 0x04000850 RID: 2128
		private IntPtr m_Message;

		// Token: 0x04000851 RID: 2129
		private IntPtr m_Context;
	}
}
