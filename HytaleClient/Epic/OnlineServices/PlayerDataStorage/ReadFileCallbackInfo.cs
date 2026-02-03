using System;

namespace Epic.OnlineServices.PlayerDataStorage
{
	// Token: 0x0200031F RID: 799
	public struct ReadFileCallbackInfo : ICallbackInfo
	{
		// Token: 0x170005D3 RID: 1491
		// (get) Token: 0x060015BD RID: 5565 RVA: 0x0001F916 File Offset: 0x0001DB16
		// (set) Token: 0x060015BE RID: 5566 RVA: 0x0001F91E File Offset: 0x0001DB1E
		public Result ResultCode { get; set; }

		// Token: 0x170005D4 RID: 1492
		// (get) Token: 0x060015BF RID: 5567 RVA: 0x0001F927 File Offset: 0x0001DB27
		// (set) Token: 0x060015C0 RID: 5568 RVA: 0x0001F92F File Offset: 0x0001DB2F
		public object ClientData { get; set; }

		// Token: 0x170005D5 RID: 1493
		// (get) Token: 0x060015C1 RID: 5569 RVA: 0x0001F938 File Offset: 0x0001DB38
		// (set) Token: 0x060015C2 RID: 5570 RVA: 0x0001F940 File Offset: 0x0001DB40
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x170005D6 RID: 1494
		// (get) Token: 0x060015C3 RID: 5571 RVA: 0x0001F949 File Offset: 0x0001DB49
		// (set) Token: 0x060015C4 RID: 5572 RVA: 0x0001F951 File Offset: 0x0001DB51
		public Utf8String Filename { get; set; }

		// Token: 0x060015C5 RID: 5573 RVA: 0x0001F95C File Offset: 0x0001DB5C
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x060015C6 RID: 5574 RVA: 0x0001F979 File Offset: 0x0001DB79
		internal void Set(ref ReadFileCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.Filename = other.Filename;
		}
	}
}
