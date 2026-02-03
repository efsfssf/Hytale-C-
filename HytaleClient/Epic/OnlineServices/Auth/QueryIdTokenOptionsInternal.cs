using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Auth
{
	// Token: 0x02000662 RID: 1634
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct QueryIdTokenOptionsInternal : ISettable<QueryIdTokenOptions>, IDisposable
	{
		// Token: 0x17000C53 RID: 3155
		// (set) Token: 0x06002A48 RID: 10824 RVA: 0x0003E0B1 File Offset: 0x0003C2B1
		public EpicAccountId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x17000C54 RID: 3156
		// (set) Token: 0x06002A49 RID: 10825 RVA: 0x0003E0C1 File Offset: 0x0003C2C1
		public EpicAccountId TargetAccountId
		{
			set
			{
				Helper.Set(value, ref this.m_TargetAccountId);
			}
		}

		// Token: 0x06002A4A RID: 10826 RVA: 0x0003E0D1 File Offset: 0x0003C2D1
		public void Set(ref QueryIdTokenOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
			this.TargetAccountId = other.TargetAccountId;
		}

		// Token: 0x06002A4B RID: 10827 RVA: 0x0003E0F8 File Offset: 0x0003C2F8
		public void Set(ref QueryIdTokenOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
				this.TargetAccountId = other.Value.TargetAccountId;
			}
		}

		// Token: 0x06002A4C RID: 10828 RVA: 0x0003E143 File Offset: 0x0003C343
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_TargetAccountId);
		}

		// Token: 0x0400121D RID: 4637
		private int m_ApiVersion;

		// Token: 0x0400121E RID: 4638
		private IntPtr m_LocalUserId;

		// Token: 0x0400121F RID: 4639
		private IntPtr m_TargetAccountId;
	}
}
