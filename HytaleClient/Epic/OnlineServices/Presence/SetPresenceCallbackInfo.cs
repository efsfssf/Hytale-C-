using System;

namespace Epic.OnlineServices.Presence
{
	// Token: 0x020002E4 RID: 740
	public struct SetPresenceCallbackInfo : ICallbackInfo
	{
		// Token: 0x17000572 RID: 1394
		// (get) Token: 0x06001454 RID: 5204 RVA: 0x0001DB7A File Offset: 0x0001BD7A
		// (set) Token: 0x06001455 RID: 5205 RVA: 0x0001DB82 File Offset: 0x0001BD82
		public Result ResultCode { get; set; }

		// Token: 0x17000573 RID: 1395
		// (get) Token: 0x06001456 RID: 5206 RVA: 0x0001DB8B File Offset: 0x0001BD8B
		// (set) Token: 0x06001457 RID: 5207 RVA: 0x0001DB93 File Offset: 0x0001BD93
		public object ClientData { get; set; }

		// Token: 0x17000574 RID: 1396
		// (get) Token: 0x06001458 RID: 5208 RVA: 0x0001DB9C File Offset: 0x0001BD9C
		// (set) Token: 0x06001459 RID: 5209 RVA: 0x0001DBA4 File Offset: 0x0001BDA4
		public EpicAccountId LocalUserId { get; set; }

		// Token: 0x0600145A RID: 5210 RVA: 0x0001DBB0 File Offset: 0x0001BDB0
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x0600145B RID: 5211 RVA: 0x0001DBCD File Offset: 0x0001BDCD
		internal void Set(ref SetPresenceCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
		}
	}
}
