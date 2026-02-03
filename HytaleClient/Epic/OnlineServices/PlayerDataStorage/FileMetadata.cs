using System;

namespace Epic.OnlineServices.PlayerDataStorage
{
	// Token: 0x020002FB RID: 763
	public struct FileMetadata
	{
		// Token: 0x170005A6 RID: 1446
		// (get) Token: 0x060014DE RID: 5342 RVA: 0x0001E773 File Offset: 0x0001C973
		// (set) Token: 0x060014DF RID: 5343 RVA: 0x0001E77B File Offset: 0x0001C97B
		public uint FileSizeBytes { get; set; }

		// Token: 0x170005A7 RID: 1447
		// (get) Token: 0x060014E0 RID: 5344 RVA: 0x0001E784 File Offset: 0x0001C984
		// (set) Token: 0x060014E1 RID: 5345 RVA: 0x0001E78C File Offset: 0x0001C98C
		public Utf8String MD5Hash { get; set; }

		// Token: 0x170005A8 RID: 1448
		// (get) Token: 0x060014E2 RID: 5346 RVA: 0x0001E795 File Offset: 0x0001C995
		// (set) Token: 0x060014E3 RID: 5347 RVA: 0x0001E79D File Offset: 0x0001C99D
		public Utf8String Filename { get; set; }

		// Token: 0x170005A9 RID: 1449
		// (get) Token: 0x060014E4 RID: 5348 RVA: 0x0001E7A6 File Offset: 0x0001C9A6
		// (set) Token: 0x060014E5 RID: 5349 RVA: 0x0001E7AE File Offset: 0x0001C9AE
		public DateTimeOffset? LastModifiedTime { get; set; }

		// Token: 0x170005AA RID: 1450
		// (get) Token: 0x060014E6 RID: 5350 RVA: 0x0001E7B7 File Offset: 0x0001C9B7
		// (set) Token: 0x060014E7 RID: 5351 RVA: 0x0001E7BF File Offset: 0x0001C9BF
		public uint UnencryptedDataSizeBytes { get; set; }

		// Token: 0x060014E8 RID: 5352 RVA: 0x0001E7C8 File Offset: 0x0001C9C8
		internal void Set(ref FileMetadataInternal other)
		{
			this.FileSizeBytes = other.FileSizeBytes;
			this.MD5Hash = other.MD5Hash;
			this.Filename = other.Filename;
			this.LastModifiedTime = other.LastModifiedTime;
			this.UnencryptedDataSizeBytes = other.UnencryptedDataSizeBytes;
		}
	}
}
