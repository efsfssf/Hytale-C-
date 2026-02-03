using System;

namespace Epic.OnlineServices.UI
{
	// Token: 0x0200005B RID: 91
	public struct HideFriendsCallbackInfo : ICallbackInfo
	{
		// Token: 0x1700008B RID: 139
		// (get) Token: 0x060004A1 RID: 1185 RVA: 0x00006C30 File Offset: 0x00004E30
		// (set) Token: 0x060004A2 RID: 1186 RVA: 0x00006C38 File Offset: 0x00004E38
		public Result ResultCode { get; set; }

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x060004A3 RID: 1187 RVA: 0x00006C41 File Offset: 0x00004E41
		// (set) Token: 0x060004A4 RID: 1188 RVA: 0x00006C49 File Offset: 0x00004E49
		public object ClientData { get; set; }

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x060004A5 RID: 1189 RVA: 0x00006C52 File Offset: 0x00004E52
		// (set) Token: 0x060004A6 RID: 1190 RVA: 0x00006C5A File Offset: 0x00004E5A
		public EpicAccountId LocalUserId { get; set; }

		// Token: 0x060004A7 RID: 1191 RVA: 0x00006C64 File Offset: 0x00004E64
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x060004A8 RID: 1192 RVA: 0x00006C81 File Offset: 0x00004E81
		internal void Set(ref HideFriendsCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
		}
	}
}
