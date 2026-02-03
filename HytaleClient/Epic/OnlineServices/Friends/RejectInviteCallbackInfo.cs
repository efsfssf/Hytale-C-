using System;

namespace Epic.OnlineServices.Friends
{
	// Token: 0x020004F7 RID: 1271
	public struct RejectInviteCallbackInfo : ICallbackInfo
	{
		// Token: 0x17000958 RID: 2392
		// (get) Token: 0x060020F1 RID: 8433 RVA: 0x0003030D File Offset: 0x0002E50D
		// (set) Token: 0x060020F2 RID: 8434 RVA: 0x00030315 File Offset: 0x0002E515
		public Result ResultCode { get; set; }

		// Token: 0x17000959 RID: 2393
		// (get) Token: 0x060020F3 RID: 8435 RVA: 0x0003031E File Offset: 0x0002E51E
		// (set) Token: 0x060020F4 RID: 8436 RVA: 0x00030326 File Offset: 0x0002E526
		public object ClientData { get; set; }

		// Token: 0x1700095A RID: 2394
		// (get) Token: 0x060020F5 RID: 8437 RVA: 0x0003032F File Offset: 0x0002E52F
		// (set) Token: 0x060020F6 RID: 8438 RVA: 0x00030337 File Offset: 0x0002E537
		public EpicAccountId LocalUserId { get; set; }

		// Token: 0x1700095B RID: 2395
		// (get) Token: 0x060020F7 RID: 8439 RVA: 0x00030340 File Offset: 0x0002E540
		// (set) Token: 0x060020F8 RID: 8440 RVA: 0x00030348 File Offset: 0x0002E548
		public EpicAccountId TargetUserId { get; set; }

		// Token: 0x060020F9 RID: 8441 RVA: 0x00030354 File Offset: 0x0002E554
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x060020FA RID: 8442 RVA: 0x00030371 File Offset: 0x0002E571
		internal void Set(ref RejectInviteCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.TargetUserId = other.TargetUserId;
		}
	}
}
