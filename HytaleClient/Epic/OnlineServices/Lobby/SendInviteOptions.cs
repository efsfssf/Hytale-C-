using System;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x0200044B RID: 1099
	public struct SendInviteOptions
	{
		// Token: 0x17000819 RID: 2073
		// (get) Token: 0x06001CEC RID: 7404 RVA: 0x0002A4A6 File Offset: 0x000286A6
		// (set) Token: 0x06001CED RID: 7405 RVA: 0x0002A4AE File Offset: 0x000286AE
		public Utf8String LobbyId { get; set; }

		// Token: 0x1700081A RID: 2074
		// (get) Token: 0x06001CEE RID: 7406 RVA: 0x0002A4B7 File Offset: 0x000286B7
		// (set) Token: 0x06001CEF RID: 7407 RVA: 0x0002A4BF File Offset: 0x000286BF
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x1700081B RID: 2075
		// (get) Token: 0x06001CF0 RID: 7408 RVA: 0x0002A4C8 File Offset: 0x000286C8
		// (set) Token: 0x06001CF1 RID: 7409 RVA: 0x0002A4D0 File Offset: 0x000286D0
		public ProductUserId TargetUserId { get; set; }
	}
}
