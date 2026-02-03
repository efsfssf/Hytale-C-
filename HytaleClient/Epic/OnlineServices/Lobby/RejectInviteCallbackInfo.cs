using System;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x02000443 RID: 1091
	public struct RejectInviteCallbackInfo : ICallbackInfo
	{
		// Token: 0x170007FC RID: 2044
		// (get) Token: 0x06001CA2 RID: 7330 RVA: 0x00029D65 File Offset: 0x00027F65
		// (set) Token: 0x06001CA3 RID: 7331 RVA: 0x00029D6D File Offset: 0x00027F6D
		public Result ResultCode { get; set; }

		// Token: 0x170007FD RID: 2045
		// (get) Token: 0x06001CA4 RID: 7332 RVA: 0x00029D76 File Offset: 0x00027F76
		// (set) Token: 0x06001CA5 RID: 7333 RVA: 0x00029D7E File Offset: 0x00027F7E
		public object ClientData { get; set; }

		// Token: 0x170007FE RID: 2046
		// (get) Token: 0x06001CA6 RID: 7334 RVA: 0x00029D87 File Offset: 0x00027F87
		// (set) Token: 0x06001CA7 RID: 7335 RVA: 0x00029D8F File Offset: 0x00027F8F
		public Utf8String InviteId { get; set; }

		// Token: 0x06001CA8 RID: 7336 RVA: 0x00029D98 File Offset: 0x00027F98
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x06001CA9 RID: 7337 RVA: 0x00029DB5 File Offset: 0x00027FB5
		internal void Set(ref RejectInviteCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.InviteId = other.InviteId;
		}
	}
}
