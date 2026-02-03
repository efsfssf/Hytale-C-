using System;

namespace Epic.OnlineServices.Ecom
{
	// Token: 0x02000503 RID: 1283
	public struct CatalogRelease
	{
		// Token: 0x170009B0 RID: 2480
		// (get) Token: 0x060021B7 RID: 8631 RVA: 0x000317F0 File Offset: 0x0002F9F0
		// (set) Token: 0x060021B8 RID: 8632 RVA: 0x000317F8 File Offset: 0x0002F9F8
		public Utf8String[] CompatibleAppIds { get; set; }

		// Token: 0x170009B1 RID: 2481
		// (get) Token: 0x060021B9 RID: 8633 RVA: 0x00031801 File Offset: 0x0002FA01
		// (set) Token: 0x060021BA RID: 8634 RVA: 0x00031809 File Offset: 0x0002FA09
		public Utf8String[] CompatiblePlatforms { get; set; }

		// Token: 0x170009B2 RID: 2482
		// (get) Token: 0x060021BB RID: 8635 RVA: 0x00031812 File Offset: 0x0002FA12
		// (set) Token: 0x060021BC RID: 8636 RVA: 0x0003181A File Offset: 0x0002FA1A
		public Utf8String ReleaseNote { get; set; }

		// Token: 0x060021BD RID: 8637 RVA: 0x00031823 File Offset: 0x0002FA23
		internal void Set(ref CatalogReleaseInternal other)
		{
			this.CompatibleAppIds = other.CompatibleAppIds;
			this.CompatiblePlatforms = other.CompatiblePlatforms;
			this.ReleaseNote = other.ReleaseNote;
		}
	}
}
