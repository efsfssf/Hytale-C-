using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom
{
	// Token: 0x02000568 RID: 1384
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct QueryOwnershipTokenOptionsInternal : ISettable<QueryOwnershipTokenOptions>, IDisposable
	{
		// Token: 0x17000A8A RID: 2698
		// (set) Token: 0x06002425 RID: 9253 RVA: 0x00035306 File Offset: 0x00033506
		public EpicAccountId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x17000A8B RID: 2699
		// (set) Token: 0x06002426 RID: 9254 RVA: 0x00035316 File Offset: 0x00033516
		public Utf8String[] CatalogItemIds
		{
			set
			{
				Helper.Set<Utf8String>(value, ref this.m_CatalogItemIds, out this.m_CatalogItemIdCount);
			}
		}

		// Token: 0x17000A8C RID: 2700
		// (set) Token: 0x06002427 RID: 9255 RVA: 0x0003532C File Offset: 0x0003352C
		public Utf8String CatalogNamespace
		{
			set
			{
				Helper.Set(value, ref this.m_CatalogNamespace);
			}
		}

		// Token: 0x06002428 RID: 9256 RVA: 0x0003533C File Offset: 0x0003353C
		public void Set(ref QueryOwnershipTokenOptions other)
		{
			this.m_ApiVersion = 2;
			this.LocalUserId = other.LocalUserId;
			this.CatalogItemIds = other.CatalogItemIds;
			this.CatalogNamespace = other.CatalogNamespace;
		}

		// Token: 0x06002429 RID: 9257 RVA: 0x00035370 File Offset: 0x00033570
		public void Set(ref QueryOwnershipTokenOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 2;
				this.LocalUserId = other.Value.LocalUserId;
				this.CatalogItemIds = other.Value.CatalogItemIds;
				this.CatalogNamespace = other.Value.CatalogNamespace;
			}
		}

		// Token: 0x0600242A RID: 9258 RVA: 0x000353D0 File Offset: 0x000335D0
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_CatalogItemIds);
			Helper.Dispose(ref this.m_CatalogNamespace);
		}

		// Token: 0x04000FD9 RID: 4057
		private int m_ApiVersion;

		// Token: 0x04000FDA RID: 4058
		private IntPtr m_LocalUserId;

		// Token: 0x04000FDB RID: 4059
		private IntPtr m_CatalogItemIds;

		// Token: 0x04000FDC RID: 4060
		private uint m_CatalogItemIdCount;

		// Token: 0x04000FDD RID: 4061
		private IntPtr m_CatalogNamespace;
	}
}
