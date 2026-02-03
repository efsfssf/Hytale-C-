using System;

namespace Epic.OnlineServices.UserInfo
{
	// Token: 0x0200003E RID: 62
	public struct QueryUserInfoByDisplayNameCallbackInfo : ICallbackInfo
	{
		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060003E4 RID: 996 RVA: 0x000056D8 File Offset: 0x000038D8
		// (set) Token: 0x060003E5 RID: 997 RVA: 0x000056E0 File Offset: 0x000038E0
		public Result ResultCode { get; set; }

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x060003E6 RID: 998 RVA: 0x000056E9 File Offset: 0x000038E9
		// (set) Token: 0x060003E7 RID: 999 RVA: 0x000056F1 File Offset: 0x000038F1
		public object ClientData { get; set; }

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x060003E8 RID: 1000 RVA: 0x000056FA File Offset: 0x000038FA
		// (set) Token: 0x060003E9 RID: 1001 RVA: 0x00005702 File Offset: 0x00003902
		public EpicAccountId LocalUserId { get; set; }

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x060003EA RID: 1002 RVA: 0x0000570B File Offset: 0x0000390B
		// (set) Token: 0x060003EB RID: 1003 RVA: 0x00005713 File Offset: 0x00003913
		public EpicAccountId TargetUserId { get; set; }

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x060003EC RID: 1004 RVA: 0x0000571C File Offset: 0x0000391C
		// (set) Token: 0x060003ED RID: 1005 RVA: 0x00005724 File Offset: 0x00003924
		public Utf8String DisplayName { get; set; }

		// Token: 0x060003EE RID: 1006 RVA: 0x00005730 File Offset: 0x00003930
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x060003EF RID: 1007 RVA: 0x00005750 File Offset: 0x00003950
		internal void Set(ref QueryUserInfoByDisplayNameCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.TargetUserId = other.TargetUserId;
			this.DisplayName = other.DisplayName;
		}
	}
}
