using System;

namespace Epic.OnlineServices.Stats
{
	// Token: 0x020000CC RID: 204
	public struct IngestStatOptions
	{
		// Token: 0x1700016F RID: 367
		// (get) Token: 0x06000774 RID: 1908 RVA: 0x0000ACD3 File Offset: 0x00008ED3
		// (set) Token: 0x06000775 RID: 1909 RVA: 0x0000ACDB File Offset: 0x00008EDB
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x06000776 RID: 1910 RVA: 0x0000ACE4 File Offset: 0x00008EE4
		// (set) Token: 0x06000777 RID: 1911 RVA: 0x0000ACEC File Offset: 0x00008EEC
		public IngestData[] Stats { get; set; }

		// Token: 0x17000171 RID: 369
		// (get) Token: 0x06000778 RID: 1912 RVA: 0x0000ACF5 File Offset: 0x00008EF5
		// (set) Token: 0x06000779 RID: 1913 RVA: 0x0000ACFD File Offset: 0x00008EFD
		public ProductUserId TargetUserId { get; set; }
	}
}
