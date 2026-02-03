using System;

namespace Epic.OnlineServices.RTCAdmin
{
	// Token: 0x02000288 RID: 648
	public struct KickCompleteCallbackInfo : ICallbackInfo
	{
		// Token: 0x170004CB RID: 1227
		// (get) Token: 0x06001231 RID: 4657 RVA: 0x0001A916 File Offset: 0x00018B16
		// (set) Token: 0x06001232 RID: 4658 RVA: 0x0001A91E File Offset: 0x00018B1E
		public Result ResultCode { get; set; }

		// Token: 0x170004CC RID: 1228
		// (get) Token: 0x06001233 RID: 4659 RVA: 0x0001A927 File Offset: 0x00018B27
		// (set) Token: 0x06001234 RID: 4660 RVA: 0x0001A92F File Offset: 0x00018B2F
		public object ClientData { get; set; }

		// Token: 0x06001235 RID: 4661 RVA: 0x0001A938 File Offset: 0x00018B38
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x06001236 RID: 4662 RVA: 0x0001A955 File Offset: 0x00018B55
		internal void Set(ref KickCompleteCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
		}
	}
}
