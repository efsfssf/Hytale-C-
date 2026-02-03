using System;

namespace Epic.OnlineServices.UI
{
	// Token: 0x0200008E RID: 142
	public struct ShowNativeProfileCallbackInfo : ICallbackInfo
	{
		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x060005C1 RID: 1473 RVA: 0x000080B5 File Offset: 0x000062B5
		// (set) Token: 0x060005C2 RID: 1474 RVA: 0x000080BD File Offset: 0x000062BD
		public Result ResultCode { get; set; }

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x060005C3 RID: 1475 RVA: 0x000080C6 File Offset: 0x000062C6
		// (set) Token: 0x060005C4 RID: 1476 RVA: 0x000080CE File Offset: 0x000062CE
		public object ClientData { get; set; }

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x060005C5 RID: 1477 RVA: 0x000080D7 File Offset: 0x000062D7
		// (set) Token: 0x060005C6 RID: 1478 RVA: 0x000080DF File Offset: 0x000062DF
		public EpicAccountId LocalUserId { get; set; }

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x060005C7 RID: 1479 RVA: 0x000080E8 File Offset: 0x000062E8
		// (set) Token: 0x060005C8 RID: 1480 RVA: 0x000080F0 File Offset: 0x000062F0
		public EpicAccountId TargetUserId { get; set; }

		// Token: 0x060005C9 RID: 1481 RVA: 0x000080FC File Offset: 0x000062FC
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x060005CA RID: 1482 RVA: 0x00008119 File Offset: 0x00006319
		internal void Set(ref ShowNativeProfileCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.TargetUserId = other.TargetUserId;
		}
	}
}
