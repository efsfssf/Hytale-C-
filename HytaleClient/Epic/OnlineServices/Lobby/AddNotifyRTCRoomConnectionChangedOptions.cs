using System;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x0200036F RID: 879
	public struct AddNotifyRTCRoomConnectionChangedOptions
	{
		// Token: 0x17000673 RID: 1651
		// (get) Token: 0x06001797 RID: 6039 RVA: 0x00022718 File Offset: 0x00020918
		// (set) Token: 0x06001798 RID: 6040 RVA: 0x00022720 File Offset: 0x00020920
		internal Utf8String LobbyId_DEPRECATED { get; set; }

		// Token: 0x17000674 RID: 1652
		// (get) Token: 0x06001799 RID: 6041 RVA: 0x00022729 File Offset: 0x00020929
		// (set) Token: 0x0600179A RID: 6042 RVA: 0x00022731 File Offset: 0x00020931
		internal ProductUserId LocalUserId_DEPRECATED { get; set; }
	}
}
