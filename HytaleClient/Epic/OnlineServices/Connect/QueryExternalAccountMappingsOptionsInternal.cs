using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Connect
{
	// Token: 0x02000616 RID: 1558
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct QueryExternalAccountMappingsOptionsInternal : ISettable<QueryExternalAccountMappingsOptions>, IDisposable
	{
		// Token: 0x17000BAA RID: 2986
		// (set) Token: 0x0600283E RID: 10302 RVA: 0x0003AF61 File Offset: 0x00039161
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x17000BAB RID: 2987
		// (set) Token: 0x0600283F RID: 10303 RVA: 0x0003AF71 File Offset: 0x00039171
		public ExternalAccountType AccountIdType
		{
			set
			{
				this.m_AccountIdType = value;
			}
		}

		// Token: 0x17000BAC RID: 2988
		// (set) Token: 0x06002840 RID: 10304 RVA: 0x0003AF7B File Offset: 0x0003917B
		public Utf8String[] ExternalAccountIds
		{
			set
			{
				Helper.Set<Utf8String>(value, ref this.m_ExternalAccountIds, true, out this.m_ExternalAccountIdCount);
			}
		}

		// Token: 0x06002841 RID: 10305 RVA: 0x0003AF92 File Offset: 0x00039192
		public void Set(ref QueryExternalAccountMappingsOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
			this.AccountIdType = other.AccountIdType;
			this.ExternalAccountIds = other.ExternalAccountIds;
		}

		// Token: 0x06002842 RID: 10306 RVA: 0x0003AFC4 File Offset: 0x000391C4
		public void Set(ref QueryExternalAccountMappingsOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
				this.AccountIdType = other.Value.AccountIdType;
				this.ExternalAccountIds = other.Value.ExternalAccountIds;
			}
		}

		// Token: 0x06002843 RID: 10307 RVA: 0x0003B024 File Offset: 0x00039224
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_ExternalAccountIds);
		}

		// Token: 0x04001141 RID: 4417
		private int m_ApiVersion;

		// Token: 0x04001142 RID: 4418
		private IntPtr m_LocalUserId;

		// Token: 0x04001143 RID: 4419
		private ExternalAccountType m_AccountIdType;

		// Token: 0x04001144 RID: 4420
		private IntPtr m_ExternalAccountIds;

		// Token: 0x04001145 RID: 4421
		private uint m_ExternalAccountIdCount;
	}
}
