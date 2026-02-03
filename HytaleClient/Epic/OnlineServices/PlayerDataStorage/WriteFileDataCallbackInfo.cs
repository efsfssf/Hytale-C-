using System;

namespace Epic.OnlineServices.PlayerDataStorage
{
	// Token: 0x02000328 RID: 808
	public struct WriteFileDataCallbackInfo : ICallbackInfo
	{
		// Token: 0x170005FC RID: 1532
		// (get) Token: 0x0600161C RID: 5660 RVA: 0x00020367 File Offset: 0x0001E567
		// (set) Token: 0x0600161D RID: 5661 RVA: 0x0002036F File Offset: 0x0001E56F
		public object ClientData { get; set; }

		// Token: 0x170005FD RID: 1533
		// (get) Token: 0x0600161E RID: 5662 RVA: 0x00020378 File Offset: 0x0001E578
		// (set) Token: 0x0600161F RID: 5663 RVA: 0x00020380 File Offset: 0x0001E580
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x170005FE RID: 1534
		// (get) Token: 0x06001620 RID: 5664 RVA: 0x00020389 File Offset: 0x0001E589
		// (set) Token: 0x06001621 RID: 5665 RVA: 0x00020391 File Offset: 0x0001E591
		public Utf8String Filename { get; set; }

		// Token: 0x170005FF RID: 1535
		// (get) Token: 0x06001622 RID: 5666 RVA: 0x0002039A File Offset: 0x0001E59A
		// (set) Token: 0x06001623 RID: 5667 RVA: 0x000203A2 File Offset: 0x0001E5A2
		public uint DataBufferLengthBytes { get; set; }

		// Token: 0x06001624 RID: 5668 RVA: 0x000203AC File Offset: 0x0001E5AC
		public Result? GetResultCode()
		{
			return null;
		}

		// Token: 0x06001625 RID: 5669 RVA: 0x000203C7 File Offset: 0x0001E5C7
		internal void Set(ref WriteFileDataCallbackInfoInternal other)
		{
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.Filename = other.Filename;
			this.DataBufferLengthBytes = other.DataBufferLengthBytes;
		}
	}
}
