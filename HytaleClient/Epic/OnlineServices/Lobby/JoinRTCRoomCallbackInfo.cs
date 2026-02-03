using System;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x020003A1 RID: 929
	public struct JoinRTCRoomCallbackInfo : ICallbackInfo
	{
		// Token: 0x17000712 RID: 1810
		// (get) Token: 0x06001906 RID: 6406 RVA: 0x00024A35 File Offset: 0x00022C35
		// (set) Token: 0x06001907 RID: 6407 RVA: 0x00024A3D File Offset: 0x00022C3D
		public Result ResultCode { get; set; }

		// Token: 0x17000713 RID: 1811
		// (get) Token: 0x06001908 RID: 6408 RVA: 0x00024A46 File Offset: 0x00022C46
		// (set) Token: 0x06001909 RID: 6409 RVA: 0x00024A4E File Offset: 0x00022C4E
		public object ClientData { get; set; }

		// Token: 0x17000714 RID: 1812
		// (get) Token: 0x0600190A RID: 6410 RVA: 0x00024A57 File Offset: 0x00022C57
		// (set) Token: 0x0600190B RID: 6411 RVA: 0x00024A5F File Offset: 0x00022C5F
		public Utf8String LobbyId { get; set; }

		// Token: 0x0600190C RID: 6412 RVA: 0x00024A68 File Offset: 0x00022C68
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x0600190D RID: 6413 RVA: 0x00024A85 File Offset: 0x00022C85
		internal void Set(ref JoinRTCRoomCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LobbyId = other.LobbyId;
		}
	}
}
