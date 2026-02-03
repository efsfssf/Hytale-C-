using System;

namespace Epic.OnlineServices.Connect
{
	// Token: 0x020005EB RID: 1515
	public struct LinkAccountCallbackInfo : ICallbackInfo
	{
		// Token: 0x17000B76 RID: 2934
		// (get) Token: 0x06002751 RID: 10065 RVA: 0x0003A328 File Offset: 0x00038528
		// (set) Token: 0x06002752 RID: 10066 RVA: 0x0003A330 File Offset: 0x00038530
		public Result ResultCode { get; set; }

		// Token: 0x17000B77 RID: 2935
		// (get) Token: 0x06002753 RID: 10067 RVA: 0x0003A339 File Offset: 0x00038539
		// (set) Token: 0x06002754 RID: 10068 RVA: 0x0003A341 File Offset: 0x00038541
		public object ClientData { get; set; }

		// Token: 0x17000B78 RID: 2936
		// (get) Token: 0x06002755 RID: 10069 RVA: 0x0003A34A File Offset: 0x0003854A
		// (set) Token: 0x06002756 RID: 10070 RVA: 0x0003A352 File Offset: 0x00038552
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x06002757 RID: 10071 RVA: 0x0003A35C File Offset: 0x0003855C
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x06002758 RID: 10072 RVA: 0x0003A379 File Offset: 0x00038579
		internal void Set(ref LinkAccountCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
		}
	}
}
