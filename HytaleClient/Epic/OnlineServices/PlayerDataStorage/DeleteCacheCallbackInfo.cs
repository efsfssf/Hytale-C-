using System;

namespace Epic.OnlineServices.PlayerDataStorage
{
	// Token: 0x020002EF RID: 751
	public struct DeleteCacheCallbackInfo : ICallbackInfo
	{
		// Token: 0x17000585 RID: 1413
		// (get) Token: 0x0600148A RID: 5258 RVA: 0x0001DF9E File Offset: 0x0001C19E
		// (set) Token: 0x0600148B RID: 5259 RVA: 0x0001DFA6 File Offset: 0x0001C1A6
		public Result ResultCode { get; set; }

		// Token: 0x17000586 RID: 1414
		// (get) Token: 0x0600148C RID: 5260 RVA: 0x0001DFAF File Offset: 0x0001C1AF
		// (set) Token: 0x0600148D RID: 5261 RVA: 0x0001DFB7 File Offset: 0x0001C1B7
		public object ClientData { get; set; }

		// Token: 0x17000587 RID: 1415
		// (get) Token: 0x0600148E RID: 5262 RVA: 0x0001DFC0 File Offset: 0x0001C1C0
		// (set) Token: 0x0600148F RID: 5263 RVA: 0x0001DFC8 File Offset: 0x0001C1C8
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x06001490 RID: 5264 RVA: 0x0001DFD4 File Offset: 0x0001C1D4
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x06001491 RID: 5265 RVA: 0x0001DFF1 File Offset: 0x0001C1F1
		internal void Set(ref DeleteCacheCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
		}
	}
}
