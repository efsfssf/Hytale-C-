using System;

namespace Epic.OnlineServices.RTC
{
	// Token: 0x020001B5 RID: 437
	public struct DisconnectedCallbackInfo : ICallbackInfo
	{
		// Token: 0x17000301 RID: 769
		// (get) Token: 0x06000CA1 RID: 3233 RVA: 0x000127D4 File Offset: 0x000109D4
		// (set) Token: 0x06000CA2 RID: 3234 RVA: 0x000127DC File Offset: 0x000109DC
		public Result ResultCode { get; set; }

		// Token: 0x17000302 RID: 770
		// (get) Token: 0x06000CA3 RID: 3235 RVA: 0x000127E5 File Offset: 0x000109E5
		// (set) Token: 0x06000CA4 RID: 3236 RVA: 0x000127ED File Offset: 0x000109ED
		public object ClientData { get; set; }

		// Token: 0x17000303 RID: 771
		// (get) Token: 0x06000CA5 RID: 3237 RVA: 0x000127F6 File Offset: 0x000109F6
		// (set) Token: 0x06000CA6 RID: 3238 RVA: 0x000127FE File Offset: 0x000109FE
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x17000304 RID: 772
		// (get) Token: 0x06000CA7 RID: 3239 RVA: 0x00012807 File Offset: 0x00010A07
		// (set) Token: 0x06000CA8 RID: 3240 RVA: 0x0001280F File Offset: 0x00010A0F
		public Utf8String RoomName { get; set; }

		// Token: 0x06000CA9 RID: 3241 RVA: 0x00012818 File Offset: 0x00010A18
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x06000CAA RID: 3242 RVA: 0x00012835 File Offset: 0x00010A35
		internal void Set(ref DisconnectedCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.RoomName = other.RoomName;
		}
	}
}
