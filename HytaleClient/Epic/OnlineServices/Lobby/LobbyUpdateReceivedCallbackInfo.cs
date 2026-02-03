using System;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x02000405 RID: 1029
	public struct LobbyUpdateReceivedCallbackInfo : ICallbackInfo
	{
		// Token: 0x170007D7 RID: 2007
		// (get) Token: 0x06001B80 RID: 7040 RVA: 0x00029445 File Offset: 0x00027645
		// (set) Token: 0x06001B81 RID: 7041 RVA: 0x0002944D File Offset: 0x0002764D
		public object ClientData { get; set; }

		// Token: 0x170007D8 RID: 2008
		// (get) Token: 0x06001B82 RID: 7042 RVA: 0x00029456 File Offset: 0x00027656
		// (set) Token: 0x06001B83 RID: 7043 RVA: 0x0002945E File Offset: 0x0002765E
		public Utf8String LobbyId { get; set; }

		// Token: 0x06001B84 RID: 7044 RVA: 0x00029468 File Offset: 0x00027668
		public Result? GetResultCode()
		{
			return null;
		}

		// Token: 0x06001B85 RID: 7045 RVA: 0x00029483 File Offset: 0x00027683
		internal void Set(ref LobbyUpdateReceivedCallbackInfoInternal other)
		{
			this.ClientData = other.ClientData;
			this.LobbyId = other.LobbyId;
		}
	}
}
