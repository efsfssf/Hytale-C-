using System;

namespace Epic.OnlineServices.Sanctions
{
	// Token: 0x0200019B RID: 411
	public struct CreatePlayerSanctionAppealCallbackInfo : ICallbackInfo
	{
		// Token: 0x170002BC RID: 700
		// (get) Token: 0x06000BEE RID: 3054 RVA: 0x0001167A File Offset: 0x0000F87A
		// (set) Token: 0x06000BEF RID: 3055 RVA: 0x00011682 File Offset: 0x0000F882
		public Result ResultCode { get; set; }

		// Token: 0x170002BD RID: 701
		// (get) Token: 0x06000BF0 RID: 3056 RVA: 0x0001168B File Offset: 0x0000F88B
		// (set) Token: 0x06000BF1 RID: 3057 RVA: 0x00011693 File Offset: 0x0000F893
		public object ClientData { get; set; }

		// Token: 0x170002BE RID: 702
		// (get) Token: 0x06000BF2 RID: 3058 RVA: 0x0001169C File Offset: 0x0000F89C
		// (set) Token: 0x06000BF3 RID: 3059 RVA: 0x000116A4 File Offset: 0x0000F8A4
		public Utf8String ReferenceId { get; set; }

		// Token: 0x06000BF4 RID: 3060 RVA: 0x000116B0 File Offset: 0x0000F8B0
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x06000BF5 RID: 3061 RVA: 0x000116CD File Offset: 0x0000F8CD
		internal void Set(ref CreatePlayerSanctionAppealCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.ReferenceId = other.ReferenceId;
		}
	}
}
