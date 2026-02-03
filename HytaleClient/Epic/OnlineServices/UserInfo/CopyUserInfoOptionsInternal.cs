using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UserInfo
{
	// Token: 0x02000031 RID: 49
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct CopyUserInfoOptionsInternal : ISettable<CopyUserInfoOptions>, IDisposable
	{
		// Token: 0x1700003A RID: 58
		// (set) Token: 0x060003A6 RID: 934 RVA: 0x00005309 File Offset: 0x00003509
		public EpicAccountId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x1700003B RID: 59
		// (set) Token: 0x060003A7 RID: 935 RVA: 0x00005319 File Offset: 0x00003519
		public EpicAccountId TargetUserId
		{
			set
			{
				Helper.Set(value, ref this.m_TargetUserId);
			}
		}

		// Token: 0x060003A8 RID: 936 RVA: 0x00005329 File Offset: 0x00003529
		public void Set(ref CopyUserInfoOptions other)
		{
			this.m_ApiVersion = 3;
			this.LocalUserId = other.LocalUserId;
			this.TargetUserId = other.TargetUserId;
		}

		// Token: 0x060003A9 RID: 937 RVA: 0x00005350 File Offset: 0x00003550
		public void Set(ref CopyUserInfoOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 3;
				this.LocalUserId = other.Value.LocalUserId;
				this.TargetUserId = other.Value.TargetUserId;
			}
		}

		// Token: 0x060003AA RID: 938 RVA: 0x0000539B File Offset: 0x0000359B
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_TargetUserId);
		}

		// Token: 0x0400017A RID: 378
		private int m_ApiVersion;

		// Token: 0x0400017B RID: 379
		private IntPtr m_LocalUserId;

		// Token: 0x0400017C RID: 380
		private IntPtr m_TargetUserId;
	}
}
