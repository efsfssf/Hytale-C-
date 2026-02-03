using System;

namespace Epic.OnlineServices.KWS
{
	// Token: 0x020004A5 RID: 1189
	public struct QueryPermissionsCallbackInfo : ICallbackInfo
	{
		// Token: 0x170008BF RID: 2239
		// (get) Token: 0x06001F02 RID: 7938 RVA: 0x0002D58C File Offset: 0x0002B78C
		// (set) Token: 0x06001F03 RID: 7939 RVA: 0x0002D594 File Offset: 0x0002B794
		public Result ResultCode { get; set; }

		// Token: 0x170008C0 RID: 2240
		// (get) Token: 0x06001F04 RID: 7940 RVA: 0x0002D59D File Offset: 0x0002B79D
		// (set) Token: 0x06001F05 RID: 7941 RVA: 0x0002D5A5 File Offset: 0x0002B7A5
		public object ClientData { get; set; }

		// Token: 0x170008C1 RID: 2241
		// (get) Token: 0x06001F06 RID: 7942 RVA: 0x0002D5AE File Offset: 0x0002B7AE
		// (set) Token: 0x06001F07 RID: 7943 RVA: 0x0002D5B6 File Offset: 0x0002B7B6
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x170008C2 RID: 2242
		// (get) Token: 0x06001F08 RID: 7944 RVA: 0x0002D5BF File Offset: 0x0002B7BF
		// (set) Token: 0x06001F09 RID: 7945 RVA: 0x0002D5C7 File Offset: 0x0002B7C7
		public Utf8String KWSUserId { get; set; }

		// Token: 0x170008C3 RID: 2243
		// (get) Token: 0x06001F0A RID: 7946 RVA: 0x0002D5D0 File Offset: 0x0002B7D0
		// (set) Token: 0x06001F0B RID: 7947 RVA: 0x0002D5D8 File Offset: 0x0002B7D8
		public Utf8String DateOfBirth { get; set; }

		// Token: 0x170008C4 RID: 2244
		// (get) Token: 0x06001F0C RID: 7948 RVA: 0x0002D5E1 File Offset: 0x0002B7E1
		// (set) Token: 0x06001F0D RID: 7949 RVA: 0x0002D5E9 File Offset: 0x0002B7E9
		public bool IsMinor { get; set; }

		// Token: 0x170008C5 RID: 2245
		// (get) Token: 0x06001F0E RID: 7950 RVA: 0x0002D5F2 File Offset: 0x0002B7F2
		// (set) Token: 0x06001F0F RID: 7951 RVA: 0x0002D5FA File Offset: 0x0002B7FA
		public Utf8String ParentEmail { get; set; }

		// Token: 0x06001F10 RID: 7952 RVA: 0x0002D604 File Offset: 0x0002B804
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x06001F11 RID: 7953 RVA: 0x0002D624 File Offset: 0x0002B824
		internal void Set(ref QueryPermissionsCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.KWSUserId = other.KWSUserId;
			this.DateOfBirth = other.DateOfBirth;
			this.IsMinor = other.IsMinor;
			this.ParentEmail = other.ParentEmail;
		}
	}
}
