using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Friends
{
	// Token: 0x020004FA RID: 1274
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct RejectInviteOptionsInternal : ISettable<RejectInviteOptions>, IDisposable
	{
		// Token: 0x17000963 RID: 2403
		// (set) Token: 0x0600210C RID: 8460 RVA: 0x0003057D File Offset: 0x0002E77D
		public EpicAccountId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x17000964 RID: 2404
		// (set) Token: 0x0600210D RID: 8461 RVA: 0x0003058D File Offset: 0x0002E78D
		public EpicAccountId TargetUserId
		{
			set
			{
				Helper.Set(value, ref this.m_TargetUserId);
			}
		}

		// Token: 0x0600210E RID: 8462 RVA: 0x0003059D File Offset: 0x0002E79D
		public void Set(ref RejectInviteOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
			this.TargetUserId = other.TargetUserId;
		}

		// Token: 0x0600210F RID: 8463 RVA: 0x000305C4 File Offset: 0x0002E7C4
		public void Set(ref RejectInviteOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
				this.TargetUserId = other.Value.TargetUserId;
			}
		}

		// Token: 0x06002110 RID: 8464 RVA: 0x0003060F File Offset: 0x0002E80F
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_TargetUserId);
		}

		// Token: 0x04000E4E RID: 3662
		private int m_ApiVersion;

		// Token: 0x04000E4F RID: 3663
		private IntPtr m_LocalUserId;

		// Token: 0x04000E50 RID: 3664
		private IntPtr m_TargetUserId;
	}
}
