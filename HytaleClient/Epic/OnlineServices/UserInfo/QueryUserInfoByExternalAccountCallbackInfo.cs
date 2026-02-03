using System;

namespace Epic.OnlineServices.UserInfo
{
	// Token: 0x02000042 RID: 66
	public struct QueryUserInfoByExternalAccountCallbackInfo : ICallbackInfo
	{
		// Token: 0x17000057 RID: 87
		// (get) Token: 0x06000408 RID: 1032 RVA: 0x00005A92 File Offset: 0x00003C92
		// (set) Token: 0x06000409 RID: 1033 RVA: 0x00005A9A File Offset: 0x00003C9A
		public Result ResultCode { get; set; }

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x0600040A RID: 1034 RVA: 0x00005AA3 File Offset: 0x00003CA3
		// (set) Token: 0x0600040B RID: 1035 RVA: 0x00005AAB File Offset: 0x00003CAB
		public object ClientData { get; set; }

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x0600040C RID: 1036 RVA: 0x00005AB4 File Offset: 0x00003CB4
		// (set) Token: 0x0600040D RID: 1037 RVA: 0x00005ABC File Offset: 0x00003CBC
		public EpicAccountId LocalUserId { get; set; }

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x0600040E RID: 1038 RVA: 0x00005AC5 File Offset: 0x00003CC5
		// (set) Token: 0x0600040F RID: 1039 RVA: 0x00005ACD File Offset: 0x00003CCD
		public Utf8String ExternalAccountId { get; set; }

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x06000410 RID: 1040 RVA: 0x00005AD6 File Offset: 0x00003CD6
		// (set) Token: 0x06000411 RID: 1041 RVA: 0x00005ADE File Offset: 0x00003CDE
		public ExternalAccountType AccountType { get; set; }

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x06000412 RID: 1042 RVA: 0x00005AE7 File Offset: 0x00003CE7
		// (set) Token: 0x06000413 RID: 1043 RVA: 0x00005AEF File Offset: 0x00003CEF
		public EpicAccountId TargetUserId { get; set; }

		// Token: 0x06000414 RID: 1044 RVA: 0x00005AF8 File Offset: 0x00003CF8
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x06000415 RID: 1045 RVA: 0x00005B18 File Offset: 0x00003D18
		internal void Set(ref QueryUserInfoByExternalAccountCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.ExternalAccountId = other.ExternalAccountId;
			this.AccountType = other.AccountType;
			this.TargetUserId = other.TargetUserId;
		}
	}
}
