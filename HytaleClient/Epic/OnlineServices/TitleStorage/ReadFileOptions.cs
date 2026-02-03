using System;

namespace Epic.OnlineServices.TitleStorage
{
	// Token: 0x020000BD RID: 189
	public struct ReadFileOptions
	{
		// Token: 0x1700014E RID: 334
		// (get) Token: 0x06000711 RID: 1809 RVA: 0x0000A0A8 File Offset: 0x000082A8
		// (set) Token: 0x06000712 RID: 1810 RVA: 0x0000A0B0 File Offset: 0x000082B0
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x06000713 RID: 1811 RVA: 0x0000A0B9 File Offset: 0x000082B9
		// (set) Token: 0x06000714 RID: 1812 RVA: 0x0000A0C1 File Offset: 0x000082C1
		public Utf8String Filename { get; set; }

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x06000715 RID: 1813 RVA: 0x0000A0CA File Offset: 0x000082CA
		// (set) Token: 0x06000716 RID: 1814 RVA: 0x0000A0D2 File Offset: 0x000082D2
		public uint ReadChunkLengthBytes { get; set; }

		// Token: 0x17000151 RID: 337
		// (get) Token: 0x06000717 RID: 1815 RVA: 0x0000A0DB File Offset: 0x000082DB
		// (set) Token: 0x06000718 RID: 1816 RVA: 0x0000A0E3 File Offset: 0x000082E3
		public OnReadFileDataCallback ReadFileDataCallback { get; set; }

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x06000719 RID: 1817 RVA: 0x0000A0EC File Offset: 0x000082EC
		// (set) Token: 0x0600071A RID: 1818 RVA: 0x0000A0F4 File Offset: 0x000082F4
		public OnFileTransferProgressCallback FileTransferProgressCallback { get; set; }
	}
}
