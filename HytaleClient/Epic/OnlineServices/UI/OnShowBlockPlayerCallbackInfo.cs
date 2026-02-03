using System;

namespace Epic.OnlineServices.UI
{
	// Token: 0x02000070 RID: 112
	public struct OnShowBlockPlayerCallbackInfo : ICallbackInfo
	{
		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x060004FF RID: 1279 RVA: 0x000071D6 File Offset: 0x000053D6
		// (set) Token: 0x06000500 RID: 1280 RVA: 0x000071DE File Offset: 0x000053DE
		public Result ResultCode { get; set; }

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x06000501 RID: 1281 RVA: 0x000071E7 File Offset: 0x000053E7
		// (set) Token: 0x06000502 RID: 1282 RVA: 0x000071EF File Offset: 0x000053EF
		public object ClientData { get; set; }

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x06000503 RID: 1283 RVA: 0x000071F8 File Offset: 0x000053F8
		// (set) Token: 0x06000504 RID: 1284 RVA: 0x00007200 File Offset: 0x00005400
		public EpicAccountId LocalUserId { get; set; }

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x06000505 RID: 1285 RVA: 0x00007209 File Offset: 0x00005409
		// (set) Token: 0x06000506 RID: 1286 RVA: 0x00007211 File Offset: 0x00005411
		public EpicAccountId TargetUserId { get; set; }

		// Token: 0x06000507 RID: 1287 RVA: 0x0000721C File Offset: 0x0000541C
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x06000508 RID: 1288 RVA: 0x00007239 File Offset: 0x00005439
		internal void Set(ref OnShowBlockPlayerCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.TargetUserId = other.TargetUserId;
		}
	}
}
