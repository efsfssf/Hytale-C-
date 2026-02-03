using System;

namespace Epic.OnlineServices.Presence
{
	// Token: 0x020002C8 RID: 712
	public struct JoinGameAcceptedCallbackInfo : ICallbackInfo
	{
		// Token: 0x17000547 RID: 1351
		// (get) Token: 0x060013AE RID: 5038 RVA: 0x0001CB34 File Offset: 0x0001AD34
		// (set) Token: 0x060013AF RID: 5039 RVA: 0x0001CB3C File Offset: 0x0001AD3C
		public object ClientData { get; set; }

		// Token: 0x17000548 RID: 1352
		// (get) Token: 0x060013B0 RID: 5040 RVA: 0x0001CB45 File Offset: 0x0001AD45
		// (set) Token: 0x060013B1 RID: 5041 RVA: 0x0001CB4D File Offset: 0x0001AD4D
		public Utf8String JoinInfo { get; set; }

		// Token: 0x17000549 RID: 1353
		// (get) Token: 0x060013B2 RID: 5042 RVA: 0x0001CB56 File Offset: 0x0001AD56
		// (set) Token: 0x060013B3 RID: 5043 RVA: 0x0001CB5E File Offset: 0x0001AD5E
		public EpicAccountId LocalUserId { get; set; }

		// Token: 0x1700054A RID: 1354
		// (get) Token: 0x060013B4 RID: 5044 RVA: 0x0001CB67 File Offset: 0x0001AD67
		// (set) Token: 0x060013B5 RID: 5045 RVA: 0x0001CB6F File Offset: 0x0001AD6F
		public EpicAccountId TargetUserId { get; set; }

		// Token: 0x1700054B RID: 1355
		// (get) Token: 0x060013B6 RID: 5046 RVA: 0x0001CB78 File Offset: 0x0001AD78
		// (set) Token: 0x060013B7 RID: 5047 RVA: 0x0001CB80 File Offset: 0x0001AD80
		public ulong UiEventId { get; set; }

		// Token: 0x060013B8 RID: 5048 RVA: 0x0001CB8C File Offset: 0x0001AD8C
		public Result? GetResultCode()
		{
			return null;
		}

		// Token: 0x060013B9 RID: 5049 RVA: 0x0001CBA8 File Offset: 0x0001ADA8
		internal void Set(ref JoinGameAcceptedCallbackInfoInternal other)
		{
			this.ClientData = other.ClientData;
			this.JoinInfo = other.JoinInfo;
			this.LocalUserId = other.LocalUserId;
			this.TargetUserId = other.TargetUserId;
			this.UiEventId = other.UiEventId;
		}
	}
}
