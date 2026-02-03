using System;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x0200044D RID: 1101
	public struct SendLobbyNativeInviteRequestedCallbackInfo : ICallbackInfo
	{
		// Token: 0x1700081F RID: 2079
		// (get) Token: 0x06001CF8 RID: 7416 RVA: 0x0002A5C3 File Offset: 0x000287C3
		// (set) Token: 0x06001CF9 RID: 7417 RVA: 0x0002A5CB File Offset: 0x000287CB
		public object ClientData { get; set; }

		// Token: 0x17000820 RID: 2080
		// (get) Token: 0x06001CFA RID: 7418 RVA: 0x0002A5D4 File Offset: 0x000287D4
		// (set) Token: 0x06001CFB RID: 7419 RVA: 0x0002A5DC File Offset: 0x000287DC
		public ulong UiEventId { get; set; }

		// Token: 0x17000821 RID: 2081
		// (get) Token: 0x06001CFC RID: 7420 RVA: 0x0002A5E5 File Offset: 0x000287E5
		// (set) Token: 0x06001CFD RID: 7421 RVA: 0x0002A5ED File Offset: 0x000287ED
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x17000822 RID: 2082
		// (get) Token: 0x06001CFE RID: 7422 RVA: 0x0002A5F6 File Offset: 0x000287F6
		// (set) Token: 0x06001CFF RID: 7423 RVA: 0x0002A5FE File Offset: 0x000287FE
		public Utf8String TargetNativeAccountType { get; set; }

		// Token: 0x17000823 RID: 2083
		// (get) Token: 0x06001D00 RID: 7424 RVA: 0x0002A607 File Offset: 0x00028807
		// (set) Token: 0x06001D01 RID: 7425 RVA: 0x0002A60F File Offset: 0x0002880F
		public Utf8String TargetUserNativeAccountId { get; set; }

		// Token: 0x17000824 RID: 2084
		// (get) Token: 0x06001D02 RID: 7426 RVA: 0x0002A618 File Offset: 0x00028818
		// (set) Token: 0x06001D03 RID: 7427 RVA: 0x0002A620 File Offset: 0x00028820
		public Utf8String LobbyId { get; set; }

		// Token: 0x06001D04 RID: 7428 RVA: 0x0002A62C File Offset: 0x0002882C
		public Result? GetResultCode()
		{
			return null;
		}

		// Token: 0x06001D05 RID: 7429 RVA: 0x0002A648 File Offset: 0x00028848
		internal void Set(ref SendLobbyNativeInviteRequestedCallbackInfoInternal other)
		{
			this.ClientData = other.ClientData;
			this.UiEventId = other.UiEventId;
			this.LocalUserId = other.LocalUserId;
			this.TargetNativeAccountType = other.TargetNativeAccountType;
			this.TargetUserNativeAccountId = other.TargetUserNativeAccountId;
			this.LobbyId = other.LobbyId;
		}
	}
}
