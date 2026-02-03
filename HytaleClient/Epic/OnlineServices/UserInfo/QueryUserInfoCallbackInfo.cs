using System;

namespace Epic.OnlineServices.UserInfo
{
	// Token: 0x02000046 RID: 70
	public struct QueryUserInfoCallbackInfo : ICallbackInfo
	{
		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000433 RID: 1075 RVA: 0x00005EEB File Offset: 0x000040EB
		// (set) Token: 0x06000434 RID: 1076 RVA: 0x00005EF3 File Offset: 0x000040F3
		public Result ResultCode { get; set; }

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000435 RID: 1077 RVA: 0x00005EFC File Offset: 0x000040FC
		// (set) Token: 0x06000436 RID: 1078 RVA: 0x00005F04 File Offset: 0x00004104
		public object ClientData { get; set; }

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000437 RID: 1079 RVA: 0x00005F0D File Offset: 0x0000410D
		// (set) Token: 0x06000438 RID: 1080 RVA: 0x00005F15 File Offset: 0x00004115
		public EpicAccountId LocalUserId { get; set; }

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x06000439 RID: 1081 RVA: 0x00005F1E File Offset: 0x0000411E
		// (set) Token: 0x0600043A RID: 1082 RVA: 0x00005F26 File Offset: 0x00004126
		public EpicAccountId TargetUserId { get; set; }

		// Token: 0x0600043B RID: 1083 RVA: 0x00005F30 File Offset: 0x00004130
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x0600043C RID: 1084 RVA: 0x00005F4D File Offset: 0x0000414D
		internal void Set(ref QueryUserInfoCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.TargetUserId = other.TargetUserId;
		}
	}
}
