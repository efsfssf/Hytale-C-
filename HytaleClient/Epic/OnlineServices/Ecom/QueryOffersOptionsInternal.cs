using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom
{
	// Token: 0x0200055C RID: 1372
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct QueryOffersOptionsInternal : ISettable<QueryOffersOptions>, IDisposable
	{
		// Token: 0x17000A60 RID: 2656
		// (set) Token: 0x060023C0 RID: 9152 RVA: 0x0003492C File Offset: 0x00032B2C
		public EpicAccountId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x17000A61 RID: 2657
		// (set) Token: 0x060023C1 RID: 9153 RVA: 0x0003493C File Offset: 0x00032B3C
		public Utf8String OverrideCatalogNamespace
		{
			set
			{
				Helper.Set(value, ref this.m_OverrideCatalogNamespace);
			}
		}

		// Token: 0x060023C2 RID: 9154 RVA: 0x0003494C File Offset: 0x00032B4C
		public void Set(ref QueryOffersOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
			this.OverrideCatalogNamespace = other.OverrideCatalogNamespace;
		}

		// Token: 0x060023C3 RID: 9155 RVA: 0x00034970 File Offset: 0x00032B70
		public void Set(ref QueryOffersOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
				this.OverrideCatalogNamespace = other.Value.OverrideCatalogNamespace;
			}
		}

		// Token: 0x060023C4 RID: 9156 RVA: 0x000349BB File Offset: 0x00032BBB
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_OverrideCatalogNamespace);
		}

		// Token: 0x04000FAB RID: 4011
		private int m_ApiVersion;

		// Token: 0x04000FAC RID: 4012
		private IntPtr m_LocalUserId;

		// Token: 0x04000FAD RID: 4013
		private IntPtr m_OverrideCatalogNamespace;
	}
}
