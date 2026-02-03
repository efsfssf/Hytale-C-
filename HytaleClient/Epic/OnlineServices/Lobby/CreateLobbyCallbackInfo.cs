using System;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x0200037F RID: 895
	public struct CreateLobbyCallbackInfo : ICallbackInfo
	{
		// Token: 0x17000690 RID: 1680
		// (get) Token: 0x060017EE RID: 6126 RVA: 0x00023062 File Offset: 0x00021262
		// (set) Token: 0x060017EF RID: 6127 RVA: 0x0002306A File Offset: 0x0002126A
		public Result ResultCode { get; set; }

		// Token: 0x17000691 RID: 1681
		// (get) Token: 0x060017F0 RID: 6128 RVA: 0x00023073 File Offset: 0x00021273
		// (set) Token: 0x060017F1 RID: 6129 RVA: 0x0002307B File Offset: 0x0002127B
		public object ClientData { get; set; }

		// Token: 0x17000692 RID: 1682
		// (get) Token: 0x060017F2 RID: 6130 RVA: 0x00023084 File Offset: 0x00021284
		// (set) Token: 0x060017F3 RID: 6131 RVA: 0x0002308C File Offset: 0x0002128C
		public Utf8String LobbyId { get; set; }

		// Token: 0x060017F4 RID: 6132 RVA: 0x00023098 File Offset: 0x00021298
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x060017F5 RID: 6133 RVA: 0x000230B5 File Offset: 0x000212B5
		internal void Set(ref CreateLobbyCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LobbyId = other.LobbyId;
		}
	}
}
