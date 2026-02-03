using System;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x02000144 RID: 324
	public struct SendInviteCallbackInfo : ICallbackInfo
	{
		// Token: 0x17000215 RID: 533
		// (get) Token: 0x060009CF RID: 2511 RVA: 0x0000D9D2 File Offset: 0x0000BBD2
		// (set) Token: 0x060009D0 RID: 2512 RVA: 0x0000D9DA File Offset: 0x0000BBDA
		public Result ResultCode { get; set; }

		// Token: 0x17000216 RID: 534
		// (get) Token: 0x060009D1 RID: 2513 RVA: 0x0000D9E3 File Offset: 0x0000BBE3
		// (set) Token: 0x060009D2 RID: 2514 RVA: 0x0000D9EB File Offset: 0x0000BBEB
		public object ClientData { get; set; }

		// Token: 0x060009D3 RID: 2515 RVA: 0x0000D9F4 File Offset: 0x0000BBF4
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x060009D4 RID: 2516 RVA: 0x0000DA11 File Offset: 0x0000BC11
		internal void Set(ref SendInviteCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
		}
	}
}
