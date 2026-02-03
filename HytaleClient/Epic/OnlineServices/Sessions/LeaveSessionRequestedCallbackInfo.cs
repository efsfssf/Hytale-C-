using System;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x02000114 RID: 276
	public struct LeaveSessionRequestedCallbackInfo : ICallbackInfo
	{
		// Token: 0x170001EF RID: 495
		// (get) Token: 0x060008EB RID: 2283 RVA: 0x0000D040 File Offset: 0x0000B240
		// (set) Token: 0x060008EC RID: 2284 RVA: 0x0000D048 File Offset: 0x0000B248
		public object ClientData { get; set; }

		// Token: 0x170001F0 RID: 496
		// (get) Token: 0x060008ED RID: 2285 RVA: 0x0000D051 File Offset: 0x0000B251
		// (set) Token: 0x060008EE RID: 2286 RVA: 0x0000D059 File Offset: 0x0000B259
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x170001F1 RID: 497
		// (get) Token: 0x060008EF RID: 2287 RVA: 0x0000D062 File Offset: 0x0000B262
		// (set) Token: 0x060008F0 RID: 2288 RVA: 0x0000D06A File Offset: 0x0000B26A
		public Utf8String SessionName { get; set; }

		// Token: 0x060008F1 RID: 2289 RVA: 0x0000D074 File Offset: 0x0000B274
		public Result? GetResultCode()
		{
			return null;
		}

		// Token: 0x060008F2 RID: 2290 RVA: 0x0000D08F File Offset: 0x0000B28F
		internal void Set(ref LeaveSessionRequestedCallbackInfoInternal other)
		{
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.SessionName = other.SessionName;
		}
	}
}
