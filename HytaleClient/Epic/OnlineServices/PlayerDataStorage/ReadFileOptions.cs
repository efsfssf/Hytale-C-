using System;

namespace Epic.OnlineServices.PlayerDataStorage
{
	// Token: 0x02000323 RID: 803
	public struct ReadFileOptions
	{
		// Token: 0x170005E9 RID: 1513
		// (get) Token: 0x060015F3 RID: 5619 RVA: 0x0001FECC File Offset: 0x0001E0CC
		// (set) Token: 0x060015F4 RID: 5620 RVA: 0x0001FED4 File Offset: 0x0001E0D4
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x170005EA RID: 1514
		// (get) Token: 0x060015F5 RID: 5621 RVA: 0x0001FEDD File Offset: 0x0001E0DD
		// (set) Token: 0x060015F6 RID: 5622 RVA: 0x0001FEE5 File Offset: 0x0001E0E5
		public Utf8String Filename { get; set; }

		// Token: 0x170005EB RID: 1515
		// (get) Token: 0x060015F7 RID: 5623 RVA: 0x0001FEEE File Offset: 0x0001E0EE
		// (set) Token: 0x060015F8 RID: 5624 RVA: 0x0001FEF6 File Offset: 0x0001E0F6
		public uint ReadChunkLengthBytes { get; set; }

		// Token: 0x170005EC RID: 1516
		// (get) Token: 0x060015F9 RID: 5625 RVA: 0x0001FEFF File Offset: 0x0001E0FF
		// (set) Token: 0x060015FA RID: 5626 RVA: 0x0001FF07 File Offset: 0x0001E107
		public OnReadFileDataCallback ReadFileDataCallback { get; set; }

		// Token: 0x170005ED RID: 1517
		// (get) Token: 0x060015FB RID: 5627 RVA: 0x0001FF10 File Offset: 0x0001E110
		// (set) Token: 0x060015FC RID: 5628 RVA: 0x0001FF18 File Offset: 0x0001E118
		public OnFileTransferProgressCallback FileTransferProgressCallback { get; set; }
	}
}
