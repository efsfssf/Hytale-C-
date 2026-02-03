using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Friends
{
	// Token: 0x020004FE RID: 1278
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct SendInviteOptionsInternal : ISettable<SendInviteOptions>, IDisposable
	{
		// Token: 0x17000970 RID: 2416
		// (set) Token: 0x0600212C RID: 8492 RVA: 0x00030899 File Offset: 0x0002EA99
		public EpicAccountId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x17000971 RID: 2417
		// (set) Token: 0x0600212D RID: 8493 RVA: 0x000308A9 File Offset: 0x0002EAA9
		public EpicAccountId TargetUserId
		{
			set
			{
				Helper.Set(value, ref this.m_TargetUserId);
			}
		}

		// Token: 0x0600212E RID: 8494 RVA: 0x000308B9 File Offset: 0x0002EAB9
		public void Set(ref SendInviteOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
			this.TargetUserId = other.TargetUserId;
		}

		// Token: 0x0600212F RID: 8495 RVA: 0x000308E0 File Offset: 0x0002EAE0
		public void Set(ref SendInviteOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
				this.TargetUserId = other.Value.TargetUserId;
			}
		}

		// Token: 0x06002130 RID: 8496 RVA: 0x0003092B File Offset: 0x0002EB2B
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_TargetUserId);
		}

		// Token: 0x04000E5B RID: 3675
		private int m_ApiVersion;

		// Token: 0x04000E5C RID: 3676
		private IntPtr m_LocalUserId;

		// Token: 0x04000E5D RID: 3677
		private IntPtr m_TargetUserId;
	}
}
