using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UserInfo
{
	// Token: 0x0200002B RID: 43
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct CopyExternalUserInfoByAccountIdOptionsInternal : ISettable<CopyExternalUserInfoByAccountIdOptions>, IDisposable
	{
		// Token: 0x17000029 RID: 41
		// (set) Token: 0x06000384 RID: 900 RVA: 0x00004FE6 File Offset: 0x000031E6
		public EpicAccountId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x1700002A RID: 42
		// (set) Token: 0x06000385 RID: 901 RVA: 0x00004FF6 File Offset: 0x000031F6
		public EpicAccountId TargetUserId
		{
			set
			{
				Helper.Set(value, ref this.m_TargetUserId);
			}
		}

		// Token: 0x1700002B RID: 43
		// (set) Token: 0x06000386 RID: 902 RVA: 0x00005006 File Offset: 0x00003206
		public Utf8String AccountId
		{
			set
			{
				Helper.Set(value, ref this.m_AccountId);
			}
		}

		// Token: 0x06000387 RID: 903 RVA: 0x00005016 File Offset: 0x00003216
		public void Set(ref CopyExternalUserInfoByAccountIdOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
			this.TargetUserId = other.TargetUserId;
			this.AccountId = other.AccountId;
		}

		// Token: 0x06000388 RID: 904 RVA: 0x00005048 File Offset: 0x00003248
		public void Set(ref CopyExternalUserInfoByAccountIdOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
				this.TargetUserId = other.Value.TargetUserId;
				this.AccountId = other.Value.AccountId;
			}
		}

		// Token: 0x06000389 RID: 905 RVA: 0x000050A8 File Offset: 0x000032A8
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_TargetUserId);
			Helper.Dispose(ref this.m_AccountId);
		}

		// Token: 0x04000166 RID: 358
		private int m_ApiVersion;

		// Token: 0x04000167 RID: 359
		private IntPtr m_LocalUserId;

		// Token: 0x04000168 RID: 360
		private IntPtr m_TargetUserId;

		// Token: 0x04000169 RID: 361
		private IntPtr m_AccountId;
	}
}
