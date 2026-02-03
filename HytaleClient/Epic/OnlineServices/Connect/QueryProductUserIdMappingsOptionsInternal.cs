using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Connect
{
	// Token: 0x0200061A RID: 1562
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct QueryProductUserIdMappingsOptionsInternal : ISettable<QueryProductUserIdMappingsOptions>, IDisposable
	{
		// Token: 0x17000BB7 RID: 2999
		// (set) Token: 0x0600285D RID: 10333 RVA: 0x0003B241 File Offset: 0x00039441
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x17000BB8 RID: 3000
		// (set) Token: 0x0600285E RID: 10334 RVA: 0x0003B251 File Offset: 0x00039451
		public ExternalAccountType AccountIdType_DEPRECATED
		{
			set
			{
				this.m_AccountIdType_DEPRECATED = value;
			}
		}

		// Token: 0x17000BB9 RID: 3001
		// (set) Token: 0x0600285F RID: 10335 RVA: 0x0003B25B File Offset: 0x0003945B
		public ProductUserId[] ProductUserIds
		{
			set
			{
				Helper.Set<ProductUserId>(value, ref this.m_ProductUserIds, out this.m_ProductUserIdCount);
			}
		}

		// Token: 0x06002860 RID: 10336 RVA: 0x0003B271 File Offset: 0x00039471
		public void Set(ref QueryProductUserIdMappingsOptions other)
		{
			this.m_ApiVersion = 2;
			this.LocalUserId = other.LocalUserId;
			this.AccountIdType_DEPRECATED = other.AccountIdType_DEPRECATED;
			this.ProductUserIds = other.ProductUserIds;
		}

		// Token: 0x06002861 RID: 10337 RVA: 0x0003B2A4 File Offset: 0x000394A4
		public void Set(ref QueryProductUserIdMappingsOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 2;
				this.LocalUserId = other.Value.LocalUserId;
				this.AccountIdType_DEPRECATED = other.Value.AccountIdType_DEPRECATED;
				this.ProductUserIds = other.Value.ProductUserIds;
			}
		}

		// Token: 0x06002862 RID: 10338 RVA: 0x0003B304 File Offset: 0x00039504
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_ProductUserIds);
		}

		// Token: 0x0400114F RID: 4431
		private int m_ApiVersion;

		// Token: 0x04001150 RID: 4432
		private IntPtr m_LocalUserId;

		// Token: 0x04001151 RID: 4433
		private ExternalAccountType m_AccountIdType_DEPRECATED;

		// Token: 0x04001152 RID: 4434
		private IntPtr m_ProductUserIds;

		// Token: 0x04001153 RID: 4435
		private uint m_ProductUserIdCount;
	}
}
