using System;

namespace Epic.OnlineServices.TitleStorage
{
	// Token: 0x020000A1 RID: 161
	public struct FileTransferProgressCallbackInfo : ICallbackInfo
	{
		// Token: 0x17000113 RID: 275
		// (get) Token: 0x0600064E RID: 1614 RVA: 0x0000920A File Offset: 0x0000740A
		// (set) Token: 0x0600064F RID: 1615 RVA: 0x00009212 File Offset: 0x00007412
		public object ClientData { get; set; }

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x06000650 RID: 1616 RVA: 0x0000921B File Offset: 0x0000741B
		// (set) Token: 0x06000651 RID: 1617 RVA: 0x00009223 File Offset: 0x00007423
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x06000652 RID: 1618 RVA: 0x0000922C File Offset: 0x0000742C
		// (set) Token: 0x06000653 RID: 1619 RVA: 0x00009234 File Offset: 0x00007434
		public Utf8String Filename { get; set; }

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x06000654 RID: 1620 RVA: 0x0000923D File Offset: 0x0000743D
		// (set) Token: 0x06000655 RID: 1621 RVA: 0x00009245 File Offset: 0x00007445
		public uint BytesTransferred { get; set; }

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x06000656 RID: 1622 RVA: 0x0000924E File Offset: 0x0000744E
		// (set) Token: 0x06000657 RID: 1623 RVA: 0x00009256 File Offset: 0x00007456
		public uint TotalFileSizeBytes { get; set; }

		// Token: 0x06000658 RID: 1624 RVA: 0x00009260 File Offset: 0x00007460
		public Result? GetResultCode()
		{
			return null;
		}

		// Token: 0x06000659 RID: 1625 RVA: 0x0000927C File Offset: 0x0000747C
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
