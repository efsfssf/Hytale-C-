using System;

namespace Epic.OnlineServices.TitleStorage
{
	// Token: 0x0200009B RID: 155
	public struct DeleteCacheCallbackInfo : ICallbackInfo
	{
		// Token: 0x17000102 RID: 258
		// (get) Token: 0x06000620 RID: 1568 RVA: 0x00008DB2 File Offset: 0x00006FB2
		// (set) Token: 0x06000621 RID: 1569 RVA: 0x00008DBA File Offset: 0x00006FBA
		public Result ResultCode { get; set; }

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x06000622 RID: 1570 RVA: 0x00008DC3 File Offset: 0x00006FC3
		// (set) Token: 0x06000623 RID: 1571 RVA: 0x00008DCB File Offset: 0x00006FCB
		public object ClientData { get; set; }

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x06000624 RID: 1572 RVA: 0x00008DD4 File Offset: 0x00006FD4
		// (set) Token: 0x06000625 RID: 1573 RVA: 0x00008DDC File Offset: 0x00006FDC
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x06000626 RID: 1574 RVA: 0x00008DE8 File Offset: 0x00006FE8
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x06000627 RID: 1575 RVA: 0x00008E05 File Offset: 0x00007005
		internal void Set(ref DeleteCacheCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
		}
	}
}
