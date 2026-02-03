using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom
{
	// Token: 0x02000554 RID: 1364
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct QueryEntitlementsOptionsInternal : ISettable<QueryEntitlementsOptions>, IDisposable
	{
		// Token: 0x17000A46 RID: 2630
		// (set) Token: 0x06002382 RID: 9090 RVA: 0x000342FA File Offset: 0x000324FA
		public EpicAccountId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x17000A47 RID: 2631
		// (set) Token: 0x06002383 RID: 9091 RVA: 0x0003430A File Offset: 0x0003250A
		public Utf8String[] EntitlementNames
		{
			set
			{
				Helper.Set<Utf8String>(value, ref this.m_EntitlementNames, out this.m_EntitlementNameCount);
			}
		}

		// Token: 0x17000A48 RID: 2632
		// (set) Token: 0x06002384 RID: 9092 RVA: 0x00034320 File Offset: 0x00032520
		public bool IncludeRedeemed
		{
			set
			{
				Helper.Set(value, ref this.m_IncludeRedeemed);
			}
		}

		// Token: 0x17000A49 RID: 2633
		// (set) Token: 0x06002385 RID: 9093 RVA: 0x00034330 File Offset: 0x00032530
		public Utf8String OverrideCatalogNamespace
		{
			set
			{
				Helper.Set(value, ref this.m_OverrideCatalogNamespace);
			}
		}

		// Token: 0x06002386 RID: 9094 RVA: 0x00034340 File Offset: 0x00032540
		public void Set(ref QueryEntitlementsOptions other)
		{
			this.m_ApiVersion = 3;
			this.LocalUserId = other.LocalUserId;
			this.EntitlementNames = other.EntitlementNames;
			this.IncludeRedeemed = other.IncludeRedeemed;
			this.OverrideCatalogNamespace = other.OverrideCatalogNamespace;
		}

		// Token: 0x06002387 RID: 9095 RVA: 0x00034380 File Offset: 0x00032580
		public void Set(ref QueryEntitlementsOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 3;
				this.LocalUserId = other.Value.LocalUserId;
				this.EntitlementNames = other.Value.EntitlementNames;
				this.IncludeRedeemed = other.Value.IncludeRedeemed;
				this.OverrideCatalogNamespace = other.Value.OverrideCatalogNamespace;
			}
		}

		// Token: 0x06002388 RID: 9096 RVA: 0x000343F5 File Offset: 0x000325F5
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_EntitlementNames);
			Helper.Dispose(ref this.m_OverrideCatalogNamespace);
		}

		// Token: 0x04000F8F RID: 3983
		private int m_ApiVersion;

		// Token: 0x04000F90 RID: 3984
		private IntPtr m_LocalUserId;

		// Token: 0x04000F91 RID: 3985
		private IntPtr m_EntitlementNames;

		// Token: 0x04000F92 RID: 3986
		private uint m_EntitlementNameCount;

		// Token: 0x04000F93 RID: 3987
		private int m_IncludeRedeemed;

		// Token: 0x04000F94 RID: 3988
		private IntPtr m_OverrideCatalogNamespace;
	}
}
