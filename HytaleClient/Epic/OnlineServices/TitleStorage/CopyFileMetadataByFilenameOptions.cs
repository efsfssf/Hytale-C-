using System;

namespace Epic.OnlineServices.TitleStorage
{
	// Token: 0x02000099 RID: 153
	public struct CopyFileMetadataByFilenameOptions
	{
		// Token: 0x170000FE RID: 254
		// (get) Token: 0x06000617 RID: 1559 RVA: 0x00008CE6 File Offset: 0x00006EE6
		// (set) Token: 0x06000618 RID: 1560 RVA: 0x00008CEE File Offset: 0x00006EEE
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x06000619 RID: 1561 RVA: 0x00008CF7 File Offset: 0x00006EF7
		// (set) Token: 0x0600061A RID: 1562 RVA: 0x00008CFF File Offset: 0x00006EFF
		public Utf8String Filename { get; set; }
	}
}
