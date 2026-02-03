using System;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x020003AF RID: 943
	public struct LeaveRTCRoomCallbackInfo : ICallbackInfo
	{
		// Token: 0x1700073E RID: 1854
		// (get) Token: 0x06001973 RID: 6515 RVA: 0x00025492 File Offset: 0x00023692
		// (set) Token: 0x06001974 RID: 6516 RVA: 0x0002549A File Offset: 0x0002369A
		public Result ResultCode { get; set; }

		// Token: 0x1700073F RID: 1855
		// (get) Token: 0x06001975 RID: 6517 RVA: 0x000254A3 File Offset: 0x000236A3
		// (set) Token: 0x06001976 RID: 6518 RVA: 0x000254AB File Offset: 0x000236AB
		public object ClientData { get; set; }

		// Token: 0x17000740 RID: 1856
		// (get) Token: 0x06001977 RID: 6519 RVA: 0x000254B4 File Offset: 0x000236B4
		// (set) Token: 0x06001978 RID: 6520 RVA: 0x000254BC File Offset: 0x000236BC
		public Utf8String LobbyId { get; set; }

		// Token: 0x06001979 RID: 6521 RVA: 0x000254C8 File Offset: 0x000236C8
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x0600197A RID: 6522 RVA: 0x000254E5 File Offset: 0x000236E5
		internal void Set(ref LeaveRTCRoomCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LobbyId = other.LobbyId;
		}
	}
}
