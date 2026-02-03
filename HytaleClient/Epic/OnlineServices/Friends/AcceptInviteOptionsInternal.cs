using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Friends
{
	// Token: 0x020004D2 RID: 1234
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct AcceptInviteOptionsInternal : ISettable<AcceptInviteOptions>, IDisposable
	{
		// Token: 0x17000929 RID: 2345
		// (set) Token: 0x0600202F RID: 8239 RVA: 0x0002F295 File Offset: 0x0002D495
		public EpicAccountId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x1700092A RID: 2346
		// (set) Token: 0x06002030 RID: 8240 RVA: 0x0002F2A5 File Offset: 0x0002D4A5
		public EpicAccountId TargetUserId
		{
			set
			{
				Helper.Set(value, ref this.m_TargetUserId);
			}
		}

		// Token: 0x06002031 RID: 8241 RVA: 0x0002F2B5 File Offset: 0x0002D4B5
		public void Set(ref AcceptInviteOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
			this.TargetUserId = other.TargetUserId;
		}

		// Token: 0x06002032 RID: 8242 RVA: 0x0002F2DC File Offset: 0x0002D4DC
		public void Set(ref AcceptInviteOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
				this.TargetUserId = other.Value.TargetUserId;
			}
		}

		// Token: 0x06002033 RID: 8243 RVA: 0x0002F327 File Offset: 0x0002D527
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_TargetUserId);
		}

		// Token: 0x04000DFF RID: 3583
		private int m_ApiVersion;

		// Token: 0x04000E00 RID: 3584
		private IntPtr m_LocalUserId;

		// Token: 0x04000E01 RID: 3585
		private IntPtr m_TargetUserId;
	}
}
