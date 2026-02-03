using System;

namespace Epic.OnlineServices.Ecom
{
	// Token: 0x02000561 RID: 1377
	public struct QueryOwnershipCallbackInfo : ICallbackInfo
	{
		// Token: 0x17000A6F RID: 2671
		// (get) Token: 0x060023E5 RID: 9189 RVA: 0x00034D06 File Offset: 0x00032F06
		// (set) Token: 0x060023E6 RID: 9190 RVA: 0x00034D0E File Offset: 0x00032F0E
		public Result ResultCode { get; set; }

		// Token: 0x17000A70 RID: 2672
		// (get) Token: 0x060023E7 RID: 9191 RVA: 0x00034D17 File Offset: 0x00032F17
		// (set) Token: 0x060023E8 RID: 9192 RVA: 0x00034D1F File Offset: 0x00032F1F
		public object ClientData { get; set; }

		// Token: 0x17000A71 RID: 2673
		// (get) Token: 0x060023E9 RID: 9193 RVA: 0x00034D28 File Offset: 0x00032F28
		// (set) Token: 0x060023EA RID: 9194 RVA: 0x00034D30 File Offset: 0x00032F30
		public EpicAccountId LocalUserId { get; set; }

		// Token: 0x17000A72 RID: 2674
		// (get) Token: 0x060023EB RID: 9195 RVA: 0x00034D39 File Offset: 0x00032F39
		// (set) Token: 0x060023EC RID: 9196 RVA: 0x00034D41 File Offset: 0x00032F41
		public ItemOwnership[] ItemOwnership { get; set; }

		// Token: 0x060023ED RID: 9197 RVA: 0x00034D4C File Offset: 0x00032F4C
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x060023EE RID: 9198 RVA: 0x00034D69 File Offset: 0x00032F69
		internal void Set(ref QueryOwnershipCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.ItemOwnership = other.ItemOwnership;
		}
	}
}
