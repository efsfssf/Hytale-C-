using System;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x0200010E RID: 270
	public struct JoinSessionAcceptedCallbackInfo : ICallbackInfo
	{
		// Token: 0x170001DB RID: 475
		// (get) Token: 0x060008BA RID: 2234 RVA: 0x0000CBBE File Offset: 0x0000ADBE
		// (set) Token: 0x060008BB RID: 2235 RVA: 0x0000CBC6 File Offset: 0x0000ADC6
		public object ClientData { get; set; }

		// Token: 0x170001DC RID: 476
		// (get) Token: 0x060008BC RID: 2236 RVA: 0x0000CBCF File Offset: 0x0000ADCF
		// (set) Token: 0x060008BD RID: 2237 RVA: 0x0000CBD7 File Offset: 0x0000ADD7
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x170001DD RID: 477
		// (get) Token: 0x060008BE RID: 2238 RVA: 0x0000CBE0 File Offset: 0x0000ADE0
		// (set) Token: 0x060008BF RID: 2239 RVA: 0x0000CBE8 File Offset: 0x0000ADE8
		public ulong UiEventId { get; set; }

		// Token: 0x060008C0 RID: 2240 RVA: 0x0000CBF4 File Offset: 0x0000ADF4
		public Result? GetResultCode()
		{
			return null;
		}

		// Token: 0x060008C1 RID: 2241 RVA: 0x0000CC0F File Offset: 0x0000AE0F
		internal void Set(ref JoinSessionAcceptedCallbackInfoInternal other)
		{
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.UiEventId = other.UiEventId;
		}
	}
}
