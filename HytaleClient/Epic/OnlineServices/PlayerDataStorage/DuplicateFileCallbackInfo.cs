using System;

namespace Epic.OnlineServices.PlayerDataStorage
{
	// Token: 0x020002F7 RID: 759
	public struct DuplicateFileCallbackInfo : ICallbackInfo
	{
		// Token: 0x17000599 RID: 1433
		// (get) Token: 0x060014BF RID: 5311 RVA: 0x0001E486 File Offset: 0x0001C686
		// (set) Token: 0x060014C0 RID: 5312 RVA: 0x0001E48E File Offset: 0x0001C68E
		public Result ResultCode { get; set; }

		// Token: 0x1700059A RID: 1434
		// (get) Token: 0x060014C1 RID: 5313 RVA: 0x0001E497 File Offset: 0x0001C697
		// (set) Token: 0x060014C2 RID: 5314 RVA: 0x0001E49F File Offset: 0x0001C69F
		public object ClientData { get; set; }

		// Token: 0x1700059B RID: 1435
		// (get) Token: 0x060014C3 RID: 5315 RVA: 0x0001E4A8 File Offset: 0x0001C6A8
		// (set) Token: 0x060014C4 RID: 5316 RVA: 0x0001E4B0 File Offset: 0x0001C6B0
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x060014C5 RID: 5317 RVA: 0x0001E4BC File Offset: 0x0001C6BC
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x060014C6 RID: 5318 RVA: 0x0001E4D9 File Offset: 0x0001C6D9
		internal void Set(ref DuplicateFileCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
		}
	}
}
