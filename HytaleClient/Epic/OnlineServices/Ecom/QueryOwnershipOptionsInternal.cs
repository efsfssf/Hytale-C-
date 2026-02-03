using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom
{
	// Token: 0x02000564 RID: 1380
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct QueryOwnershipOptionsInternal : ISettable<QueryOwnershipOptions>, IDisposable
	{
		// Token: 0x17000A7B RID: 2683
		// (set) Token: 0x06002402 RID: 9218 RVA: 0x00034F96 File Offset: 0x00033196
		public EpicAccountId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x17000A7C RID: 2684
		// (set) Token: 0x06002403 RID: 9219 RVA: 0x00034FA6 File Offset: 0x000331A6
		public Utf8String[] CatalogItemIds
		{
			set
			{
				Helper.Set<Utf8String>(value, ref this.m_CatalogItemIds, out this.m_CatalogItemIdCount);
			}
		}

		// Token: 0x17000A7D RID: 2685
		// (set) Token: 0x06002404 RID: 9220 RVA: 0x00034FBC File Offset: 0x000331BC
		public Utf8String CatalogNamespace
		{
			set
			{
				Helper.Set(value, ref this.m_CatalogNamespace);
			}
		}

		// Token: 0x06002405 RID: 9221 RVA: 0x00034FCC File Offset: 0x000331CC
		public void Set(ref QueryOwnershipOptions other)
		{
			this.m_ApiVersion = 2;
			this.LocalUserId = other.LocalUserId;
			this.CatalogItemIds = other.CatalogItemIds;
			this.CatalogNamespace = other.CatalogNamespace;
		}

		// Token: 0x06002406 RID: 9222 RVA: 0x00035000 File Offset: 0x00033200
		public void Set(ref QueryOwnershipOptions? other)
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

		// Token: 0x06002407 RID: 9223 RVA: 0x00035060 File Offset: 0x00033260
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_CatalogItemIds);
			Helper.Dispose(ref this.m_CatalogNamespace);
		}

		// Token: 0x04000FC9 RID: 4041
		private int m_ApiVersion;

		// Token: 0x04000FCA RID: 4042
		private IntPtr m_LocalUserId;

		// Token: 0x04000FCB RID: 4043
		private IntPtr m_CatalogItemIds;

		// Token: 0x04000FCC RID: 4044
		private uint m_CatalogItemIdCount;

		// Token: 0x04000FCD RID: 4045
		private IntPtr m_CatalogNamespace;
	}
}
