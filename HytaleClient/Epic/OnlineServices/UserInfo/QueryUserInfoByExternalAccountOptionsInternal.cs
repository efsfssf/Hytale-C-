using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UserInfo
{
	// Token: 0x02000045 RID: 69
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct QueryUserInfoByExternalAccountOptionsInternal : ISettable<QueryUserInfoByExternalAccountOptions>, IDisposable
	{
		// Token: 0x17000067 RID: 103
		// (set) Token: 0x0600042D RID: 1069 RVA: 0x00005E13 File Offset: 0x00004013
		public EpicAccountId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x17000068 RID: 104
		// (set) Token: 0x0600042E RID: 1070 RVA: 0x00005E23 File Offset: 0x00004023
		public Utf8String ExternalAccountId
		{
			set
			{
				Helper.Set(value, ref this.m_ExternalAccountId);
			}
		}

		// Token: 0x17000069 RID: 105
		// (set) Token: 0x0600042F RID: 1071 RVA: 0x00005E33 File Offset: 0x00004033
		public ExternalAccountType AccountType
		{
			set
			{
				this.m_AccountType = value;
			}
		}

		// Token: 0x06000430 RID: 1072 RVA: 0x00005E3D File Offset: 0x0000403D
		public void Set(ref QueryUserInfoByExternalAccountOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
			this.ExternalAccountId = other.ExternalAccountId;
			this.AccountType = other.AccountType;
		}

		// Token: 0x06000431 RID: 1073 RVA: 0x00005E70 File Offset: 0x00004070
		public void Set(ref QueryUserInfoByExternalAccountOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
				this.ExternalAccountId = other.Value.ExternalAccountId;
				this.AccountType = other.Value.AccountType;
			}
		}

		// Token: 0x06000432 RID: 1074 RVA: 0x00005ED0 File Offset: 0x000040D0
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_ExternalAccountId);
		}

		// Token: 0x040001AA RID: 426
		private int m_ApiVersion;

		// Token: 0x040001AB RID: 427
		private IntPtr m_LocalUserId;

		// Token: 0x040001AC RID: 428
		private IntPtr m_ExternalAccountId;

		// Token: 0x040001AD RID: 429
		private ExternalAccountType m_AccountType;
	}
}
