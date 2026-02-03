using System;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x02000191 RID: 401
	public struct UpdateSessionCallbackInfo : ICallbackInfo
	{
		// Token: 0x170002AB RID: 683
		// (get) Token: 0x06000BBA RID: 3002 RVA: 0x00011272 File Offset: 0x0000F472
		// (set) Token: 0x06000BBB RID: 3003 RVA: 0x0001127A File Offset: 0x0000F47A
		public Result ResultCode { get; set; }

		// Token: 0x170002AC RID: 684
		// (get) Token: 0x06000BBC RID: 3004 RVA: 0x00011283 File Offset: 0x0000F483
		// (set) Token: 0x06000BBD RID: 3005 RVA: 0x0001128B File Offset: 0x0000F48B
		public object ClientData { get; set; }

		// Token: 0x170002AD RID: 685
		// (get) Token: 0x06000BBE RID: 3006 RVA: 0x00011294 File Offset: 0x0000F494
		// (set) Token: 0x06000BBF RID: 3007 RVA: 0x0001129C File Offset: 0x0000F49C
		public Utf8String SessionName { get; set; }

		// Token: 0x170002AE RID: 686
		// (get) Token: 0x06000BC0 RID: 3008 RVA: 0x000112A5 File Offset: 0x0000F4A5
		// (set) Token: 0x06000BC1 RID: 3009 RVA: 0x000112AD File Offset: 0x0000F4AD
		public Utf8String SessionId { get; set; }

		// Token: 0x06000BC2 RID: 3010 RVA: 0x000112B8 File Offset: 0x0000F4B8
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x06000BC3 RID: 3011 RVA: 0x000112D5 File Offset: 0x0000F4D5
		internal void Set(ref UpdateSessionCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.SessionName = other.SessionName;
			this.SessionId = other.SessionId;
		}
	}
}
