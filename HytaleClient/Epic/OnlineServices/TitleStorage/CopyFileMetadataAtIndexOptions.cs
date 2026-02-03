using System;

namespace Epic.OnlineServices.TitleStorage
{
	// Token: 0x02000097 RID: 151
	public struct CopyFileMetadataAtIndexOptions
	{
		// Token: 0x170000FA RID: 250
		// (get) Token: 0x0600060E RID: 1550 RVA: 0x00008C29 File Offset: 0x00006E29
		// (set) Token: 0x0600060F RID: 1551 RVA: 0x00008C31 File Offset: 0x00006E31
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x06000610 RID: 1552 RVA: 0x00008C3A File Offset: 0x00006E3A
		// (set) Token: 0x06000611 RID: 1553 RVA: 0x00008C42 File Offset: 0x00006E42
		public uint Index { get; set; }
	}
}
