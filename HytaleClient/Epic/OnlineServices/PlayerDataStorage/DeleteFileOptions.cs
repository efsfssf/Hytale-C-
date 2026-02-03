using System;

namespace Epic.OnlineServices.PlayerDataStorage
{
	// Token: 0x020002F5 RID: 757
	public struct DeleteFileOptions
	{
		// Token: 0x17000595 RID: 1429
		// (get) Token: 0x060014B6 RID: 5302 RVA: 0x0001E3BA File Offset: 0x0001C5BA
		// (set) Token: 0x060014B7 RID: 5303 RVA: 0x0001E3C2 File Offset: 0x0001C5C2
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x17000596 RID: 1430
		// (get) Token: 0x060014B8 RID: 5304 RVA: 0x0001E3CB File Offset: 0x0001C5CB
		// (set) Token: 0x060014B9 RID: 5305 RVA: 0x0001E3D3 File Offset: 0x0001C5D3
		public Utf8String Filename { get; set; }
	}
}
