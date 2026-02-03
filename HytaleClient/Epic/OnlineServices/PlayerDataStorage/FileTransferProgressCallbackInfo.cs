using System;

namespace Epic.OnlineServices.PlayerDataStorage
{
	// Token: 0x020002FD RID: 765
	public struct FileTransferProgressCallbackInfo : ICallbackInfo
	{
		// Token: 0x170005B0 RID: 1456
		// (get) Token: 0x060014F7 RID: 5367 RVA: 0x0001EA0B File Offset: 0x0001CC0B
		// (set) Token: 0x060014F8 RID: 5368 RVA: 0x0001EA13 File Offset: 0x0001CC13
		public object ClientData { get; set; }

		// Token: 0x170005B1 RID: 1457
		// (get) Token: 0x060014F9 RID: 5369 RVA: 0x0001EA1C File Offset: 0x0001CC1C
		// (set) Token: 0x060014FA RID: 5370 RVA: 0x0001EA24 File Offset: 0x0001CC24
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x170005B2 RID: 1458
		// (get) Token: 0x060014FB RID: 5371 RVA: 0x0001EA2D File Offset: 0x0001CC2D
		// (set) Token: 0x060014FC RID: 5372 RVA: 0x0001EA35 File Offset: 0x0001CC35
		public Utf8String Filename { get; set; }

		// Token: 0x170005B3 RID: 1459
		// (get) Token: 0x060014FD RID: 5373 RVA: 0x0001EA3E File Offset: 0x0001CC3E
		// (set) Token: 0x060014FE RID: 5374 RVA: 0x0001EA46 File Offset: 0x0001CC46
		public uint BytesTransferred { get; set; }

		// Token: 0x170005B4 RID: 1460
		// (get) Token: 0x060014FF RID: 5375 RVA: 0x0001EA4F File Offset: 0x0001CC4F
		// (set) Token: 0x06001500 RID: 5376 RVA: 0x0001EA57 File Offset: 0x0001CC57
		public uint TotalFileSizeBytes { get; set; }

		// Token: 0x06001501 RID: 5377 RVA: 0x0001EA60 File Offset: 0x0001CC60
		public Result? GetResultCode()
		{
			return null;
		}

		// Token: 0x06001502 RID: 5378 RVA: 0x0001EA7C File Offset: 0x0001CC7C
		internal void Set(ref FileTransferProgressCallbackInfoInternal other)
		{
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.Filename = other.Filename;
			this.BytesTransferred = other.BytesTransferred;
			this.TotalFileSizeBytes = other.TotalFileSizeBytes;
		}
	}
}
