using System;

namespace Epic.OnlineServices.TitleStorage
{
	// Token: 0x0200009F RID: 159
	public struct FileMetadata
	{
		// Token: 0x1700010B RID: 267
		// (get) Token: 0x06000639 RID: 1593 RVA: 0x00009001 File Offset: 0x00007201
		// (set) Token: 0x0600063A RID: 1594 RVA: 0x00009009 File Offset: 0x00007209
		public uint FileSizeBytes { get; set; }

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x0600063B RID: 1595 RVA: 0x00009012 File Offset: 0x00007212
		// (set) Token: 0x0600063C RID: 1596 RVA: 0x0000901A File Offset: 0x0000721A
		public Utf8String MD5Hash { get; set; }

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x0600063D RID: 1597 RVA: 0x00009023 File Offset: 0x00007223
		// (set) Token: 0x0600063E RID: 1598 RVA: 0x0000902B File Offset: 0x0000722B
		public Utf8String Filename { get; set; }

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x0600063F RID: 1599 RVA: 0x00009034 File Offset: 0x00007234
		// (set) Token: 0x06000640 RID: 1600 RVA: 0x0000903C File Offset: 0x0000723C
		public uint UnencryptedDataSizeBytes { get; set; }

		// Token: 0x06000641 RID: 1601 RVA: 0x00009045 File Offset: 0x00007245
		internal void Set(ref FileMetadataInternal other)
		{
			this.FileSizeBytes = other.FileSizeBytes;
			this.MD5Hash = other.MD5Hash;
			this.Filename = other.Filename;
			this.UnencryptedDataSizeBytes = other.UnencryptedDataSizeBytes;
		}
	}
}
