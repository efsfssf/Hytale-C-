using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x0200016E RID: 366
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct SessionModificationSetJoinInProgressAllowedOptionsInternal : ISettable<SessionModificationSetJoinInProgressAllowedOptions>, IDisposable
	{
		// Token: 0x1700027D RID: 637
		// (set) Token: 0x06000AEF RID: 2799 RVA: 0x0000F866 File Offset: 0x0000DA66
		public bool AllowJoinInProgress
		{
			set
			{
				Helper.Set(value, ref this.m_AllowJoinInProgress);
			}
		}

		// Token: 0x06000AF0 RID: 2800 RVA: 0x0000F876 File Offset: 0x0000DA76
		public void Set(ref SessionModificationSetJoinInProgressAllowedOptions other)
		{
			this.m_ApiVersion = 1;
			this.AllowJoinInProgress = other.AllowJoinInProgress;
		}

		// Token: 0x06000AF1 RID: 2801 RVA: 0x0000F890 File Offset: 0x0000DA90
		public void Set(ref SessionModificationSetJoinInProgressAllowedOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.AllowJoinInProgress = other.Value.AllowJoinInProgress;
			}
		}

		// Token: 0x06000AF2 RID: 2802 RVA: 0x0000F8C6 File Offset: 0x0000DAC6
		public void Dispose()
		{
		}

		// Token: 0x040004FC RID: 1276
		private int m_ApiVersion;

		// Token: 0x040004FD RID: 1277
		private int m_AllowJoinInProgress;
	}
}
