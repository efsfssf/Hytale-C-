using System;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x02000397 RID: 919
	public struct JoinLobbyAcceptedCallbackInfo : ICallbackInfo
	{
		// Token: 0x170006E5 RID: 1765
		// (get) Token: 0x060018A3 RID: 6307 RVA: 0x000240EA File Offset: 0x000222EA
		// (set) Token: 0x060018A4 RID: 6308 RVA: 0x000240F2 File Offset: 0x000222F2
		public object ClientData { get; set; }

		// Token: 0x170006E6 RID: 1766
		// (get) Token: 0x060018A5 RID: 6309 RVA: 0x000240FB File Offset: 0x000222FB
		// (set) Token: 0x060018A6 RID: 6310 RVA: 0x00024103 File Offset: 0x00022303
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x170006E7 RID: 1767
		// (get) Token: 0x060018A7 RID: 6311 RVA: 0x0002410C File Offset: 0x0002230C
		// (set) Token: 0x060018A8 RID: 6312 RVA: 0x00024114 File Offset: 0x00022314
		public ulong UiEventId { get; set; }

		// Token: 0x060018A9 RID: 6313 RVA: 0x00024120 File Offset: 0x00022320
		public Result? GetResultCode()
		{
			return null;
		}

		// Token: 0x060018AA RID: 6314 RVA: 0x0002413B File Offset: 0x0002233B
		internal void Set(ref JoinLobbyAcceptedCallbackInfoInternal other)
		{
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.UiEventId = other.UiEventId;
		}
	}
}
