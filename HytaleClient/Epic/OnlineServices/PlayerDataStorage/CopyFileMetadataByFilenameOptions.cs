using System;

namespace Epic.OnlineServices.PlayerDataStorage
{
	// Token: 0x020002ED RID: 749
	public struct CopyFileMetadataByFilenameOptions
	{
		// Token: 0x17000581 RID: 1409
		// (get) Token: 0x06001481 RID: 5249 RVA: 0x0001DED2 File Offset: 0x0001C0D2
		// (set) Token: 0x06001482 RID: 5250 RVA: 0x0001DEDA File Offset: 0x0001C0DA
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x17000582 RID: 1410
		// (get) Token: 0x06001483 RID: 5251 RVA: 0x0001DEE3 File Offset: 0x0001C0E3
		// (set) Token: 0x06001484 RID: 5252 RVA: 0x0001DEEB File Offset: 0x0001C0EB
		public Utf8String Filename { get; set; }
	}
}
