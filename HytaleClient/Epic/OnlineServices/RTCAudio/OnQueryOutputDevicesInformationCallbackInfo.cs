using System;

namespace Epic.OnlineServices.RTCAudio
{
	// Token: 0x02000236 RID: 566
	public struct OnQueryOutputDevicesInformationCallbackInfo : ICallbackInfo
	{
		// Token: 0x1700040C RID: 1036
		// (get) Token: 0x06000FEA RID: 4074 RVA: 0x00017045 File Offset: 0x00015245
		// (set) Token: 0x06000FEB RID: 4075 RVA: 0x0001704D File Offset: 0x0001524D
		public Result ResultCode { get; set; }

		// Token: 0x1700040D RID: 1037
		// (get) Token: 0x06000FEC RID: 4076 RVA: 0x00017056 File Offset: 0x00015256
		// (set) Token: 0x06000FED RID: 4077 RVA: 0x0001705E File Offset: 0x0001525E
		public object ClientData { get; set; }

		// Token: 0x06000FEE RID: 4078 RVA: 0x00017068 File Offset: 0x00015268
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x06000FEF RID: 4079 RVA: 0x00017085 File Offset: 0x00015285
		internal void Set(ref OnQueryOutputDevicesInformationCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
		}
	}
}
