using System;

namespace Epic.OnlineServices.Ecom
{
	// Token: 0x0200055B RID: 1371
	public struct QueryOffersOptions
	{
		// Token: 0x17000A5E RID: 2654
		// (get) Token: 0x060023BC RID: 9148 RVA: 0x0003490A File Offset: 0x00032B0A
		// (set) Token: 0x060023BD RID: 9149 RVA: 0x00034912 File Offset: 0x00032B12
		public EpicAccountId LocalUserId { get; set; }

		// Token: 0x17000A5F RID: 2655
		// (get) Token: 0x060023BE RID: 9150 RVA: 0x0003491B File Offset: 0x00032B1B
		// (set) Token: 0x060023BF RID: 9151 RVA: 0x00034923 File Offset: 0x00032B23
		public Utf8String OverrideCatalogNamespace { get; set; }
	}
}
