using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UserInfo
{
	// Token: 0x0200002D RID: 45
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct CopyExternalUserInfoByAccountTypeOptionsInternal : ISettable<CopyExternalUserInfoByAccountTypeOptions>, IDisposable
	{
		// Token: 0x1700002F RID: 47
		// (set) Token: 0x06000390 RID: 912 RVA: 0x00005102 File Offset: 0x00003302
		public EpicAccountId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x17000030 RID: 48
		// (set) Token: 0x06000391 RID: 913 RVA: 0x00005112 File Offset: 0x00003312
		public EpicAccountId TargetUserId
		{
			set
			{
				Helper.Set(value, ref this.m_TargetUserId);
			}
		}

		// Token: 0x17000031 RID: 49
		// (set) Token: 0x06000392 RID: 914 RVA: 0x00005122 File Offset: 0x00003322
		public ExternalAccountType AccountType
		{
			set
			{
				this.m_AccountType = value;
			}
		}

		// Token: 0x06000393 RID: 915 RVA: 0x0000512C File Offset: 0x0000332C
		public void Set(ref CopyExternalUserInfoByAccountTypeOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
			this.TargetUserId = other.TargetUserId;
			this.AccountType = other.AccountType;
		}

		// Token: 0x06000394 RID: 916 RVA: 0x00005160 File Offset: 0x00003360
		public void Set(ref CopyExternalUserInfoByAccountTypeOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
				this.TargetUserId = other.Value.TargetUserId;
				this.AccountType = other.Value.AccountType;
			}
		}

		// Token: 0x06000395 RID: 917 RVA: 0x000051C0 File Offset: 0x000033C0
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_TargetUserId);
		}

		// Token: 0x0400016D RID: 365
		private int m_ApiVersion;

		// Token: 0x0400016E RID: 366
		private IntPtr m_LocalUserId;

		// Token: 0x0400016F RID: 367
		private IntPtr m_TargetUserId;

		// Token: 0x04000170 RID: 368
		private ExternalAccountType m_AccountType;
	}
}
