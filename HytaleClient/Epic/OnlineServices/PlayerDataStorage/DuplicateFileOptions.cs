using System;

namespace Epic.OnlineServices.PlayerDataStorage
{
	// Token: 0x020002F9 RID: 761
	public struct DuplicateFileOptions
	{
		// Token: 0x170005A0 RID: 1440
		// (get) Token: 0x060014D2 RID: 5330 RVA: 0x0001E656 File Offset: 0x0001C856
		// (set) Token: 0x060014D3 RID: 5331 RVA: 0x0001E65E File Offset: 0x0001C85E
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x170005A1 RID: 1441
		// (get) Token: 0x060014D4 RID: 5332 RVA: 0x0001E667 File Offset: 0x0001C867
		// (set) Token: 0x060014D5 RID: 5333 RVA: 0x0001E66F File Offset: 0x0001C86F
		public Utf8String SourceFilename { get; set; }

		// Token: 0x170005A2 RID: 1442
		// (get) Token: 0x060014D6 RID: 5334 RVA: 0x0001E678 File Offset: 0x0001C878
		// (set) Token: 0x060014D7 RID: 5335 RVA: 0x0001E680 File Offset: 0x0001C880
		public Utf8String DestinationFilename { get; set; }
	}
}
