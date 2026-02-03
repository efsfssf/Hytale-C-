using System;

namespace Epic.OnlineServices.PlayerDataStorage
{
	// Token: 0x0200032A RID: 810
	public struct WriteFileOptions
	{
		// Token: 0x17000605 RID: 1541
		// (get) Token: 0x06001633 RID: 5683 RVA: 0x000205B7 File Offset: 0x0001E7B7
		// (set) Token: 0x06001634 RID: 5684 RVA: 0x000205BF File Offset: 0x0001E7BF
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x17000606 RID: 1542
		// (get) Token: 0x06001635 RID: 5685 RVA: 0x000205C8 File Offset: 0x0001E7C8
		// (set) Token: 0x06001636 RID: 5686 RVA: 0x000205D0 File Offset: 0x0001E7D0
		public Utf8String Filename { get; set; }

		// Token: 0x17000607 RID: 1543
		// (get) Token: 0x06001637 RID: 5687 RVA: 0x000205D9 File Offset: 0x0001E7D9
		// (set) Token: 0x06001638 RID: 5688 RVA: 0x000205E1 File Offset: 0x0001E7E1
		public uint ChunkLengthBytes { get; set; }

		// Token: 0x17000608 RID: 1544
		// (get) Token: 0x06001639 RID: 5689 RVA: 0x000205EA File Offset: 0x0001E7EA
		// (set) Token: 0x0600163A RID: 5690 RVA: 0x000205F2 File Offset: 0x0001E7F2
		public OnWriteFileDataCallback WriteFileDataCallback { get; set; }

		// Token: 0x17000609 RID: 1545
		// (get) Token: 0x0600163B RID: 5691 RVA: 0x000205FB File Offset: 0x0001E7FB
		// (set) Token: 0x0600163C RID: 5692 RVA: 0x00020603 File Offset: 0x0001E803
		public OnFileTransferProgressCallback FileTransferProgressCallback { get; set; }
	}
}
