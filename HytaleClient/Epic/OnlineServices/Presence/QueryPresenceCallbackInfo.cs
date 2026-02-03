using System;

namespace Epic.OnlineServices.Presence
{
	// Token: 0x020002E0 RID: 736
	public struct QueryPresenceCallbackInfo : ICallbackInfo
	{
		// Token: 0x17000565 RID: 1381
		// (get) Token: 0x06001434 RID: 5172 RVA: 0x0001D85D File Offset: 0x0001BA5D
		// (set) Token: 0x06001435 RID: 5173 RVA: 0x0001D865 File Offset: 0x0001BA65
		public Result ResultCode { get; set; }

		// Token: 0x17000566 RID: 1382
		// (get) Token: 0x06001436 RID: 5174 RVA: 0x0001D86E File Offset: 0x0001BA6E
		// (set) Token: 0x06001437 RID: 5175 RVA: 0x0001D876 File Offset: 0x0001BA76
		public object ClientData { get; set; }

		// Token: 0x17000567 RID: 1383
		// (get) Token: 0x06001438 RID: 5176 RVA: 0x0001D87F File Offset: 0x0001BA7F
		// (set) Token: 0x06001439 RID: 5177 RVA: 0x0001D887 File Offset: 0x0001BA87
		public EpicAccountId LocalUserId { get; set; }

		// Token: 0x17000568 RID: 1384
		// (get) Token: 0x0600143A RID: 5178 RVA: 0x0001D890 File Offset: 0x0001BA90
		// (set) Token: 0x0600143B RID: 5179 RVA: 0x0001D898 File Offset: 0x0001BA98
		public EpicAccountId TargetUserId { get; set; }

		// Token: 0x0600143C RID: 5180 RVA: 0x0001D8A4 File Offset: 0x0001BAA4
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x0600143D RID: 5181 RVA: 0x0001D8C1 File Offset: 0x0001BAC1
		internal void Set(ref QueryPresenceCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.TargetUserId = other.TargetUserId;
		}
	}
}
