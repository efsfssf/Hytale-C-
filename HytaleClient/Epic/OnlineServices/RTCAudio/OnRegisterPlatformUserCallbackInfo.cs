using System;

namespace Epic.OnlineServices.RTCAudio
{
	// Token: 0x0200023A RID: 570
	public struct OnRegisterPlatformUserCallbackInfo : ICallbackInfo
	{
		// Token: 0x17000411 RID: 1041
		// (get) Token: 0x06001001 RID: 4097 RVA: 0x00017199 File Offset: 0x00015399
		// (set) Token: 0x06001002 RID: 4098 RVA: 0x000171A1 File Offset: 0x000153A1
		public Result ResultCode { get; set; }

		// Token: 0x17000412 RID: 1042
		// (get) Token: 0x06001003 RID: 4099 RVA: 0x000171AA File Offset: 0x000153AA
		// (set) Token: 0x06001004 RID: 4100 RVA: 0x000171B2 File Offset: 0x000153B2
		public object ClientData { get; set; }

		// Token: 0x17000413 RID: 1043
		// (get) Token: 0x06001005 RID: 4101 RVA: 0x000171BB File Offset: 0x000153BB
		// (set) Token: 0x06001006 RID: 4102 RVA: 0x000171C3 File Offset: 0x000153C3
		public Utf8String PlatformUserId { get; set; }

		// Token: 0x06001007 RID: 4103 RVA: 0x000171CC File Offset: 0x000153CC
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x06001008 RID: 4104 RVA: 0x000171E9 File Offset: 0x000153E9
		internal void Set(ref OnRegisterPlatformUserCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.PlatformUserId = other.PlatformUserId;
		}
	}
}
