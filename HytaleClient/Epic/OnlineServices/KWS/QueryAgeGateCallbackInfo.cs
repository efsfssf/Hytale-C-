using System;

namespace Epic.OnlineServices.KWS
{
	// Token: 0x020004A1 RID: 1185
	public struct QueryAgeGateCallbackInfo : ICallbackInfo
	{
		// Token: 0x170008B6 RID: 2230
		// (get) Token: 0x06001EE8 RID: 7912 RVA: 0x0002D328 File Offset: 0x0002B528
		// (set) Token: 0x06001EE9 RID: 7913 RVA: 0x0002D330 File Offset: 0x0002B530
		public Result ResultCode { get; set; }

		// Token: 0x170008B7 RID: 2231
		// (get) Token: 0x06001EEA RID: 7914 RVA: 0x0002D339 File Offset: 0x0002B539
		// (set) Token: 0x06001EEB RID: 7915 RVA: 0x0002D341 File Offset: 0x0002B541
		public object ClientData { get; set; }

		// Token: 0x170008B8 RID: 2232
		// (get) Token: 0x06001EEC RID: 7916 RVA: 0x0002D34A File Offset: 0x0002B54A
		// (set) Token: 0x06001EED RID: 7917 RVA: 0x0002D352 File Offset: 0x0002B552
		public Utf8String CountryCode { get; set; }

		// Token: 0x170008B9 RID: 2233
		// (get) Token: 0x06001EEE RID: 7918 RVA: 0x0002D35B File Offset: 0x0002B55B
		// (set) Token: 0x06001EEF RID: 7919 RVA: 0x0002D363 File Offset: 0x0002B563
		public uint AgeOfConsent { get; set; }

		// Token: 0x06001EF0 RID: 7920 RVA: 0x0002D36C File Offset: 0x0002B56C
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x06001EF1 RID: 7921 RVA: 0x0002D389 File Offset: 0x0002B589
		internal void Set(ref QueryAgeGateCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.CountryCode = other.CountryCode;
			this.AgeOfConsent = other.AgeOfConsent;
		}
	}
}
