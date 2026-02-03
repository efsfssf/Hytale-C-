using System;

namespace Epic.OnlineServices.P2P
{
	// Token: 0x0200076F RID: 1903
	public struct AddNotifyPeerConnectionClosedOptions
	{
		// Token: 0x17000EF5 RID: 3829
		// (get) Token: 0x0600318B RID: 12683 RVA: 0x0004A2C4 File Offset: 0x000484C4
		// (set) Token: 0x0600318C RID: 12684 RVA: 0x0004A2CC File Offset: 0x000484CC
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x17000EF6 RID: 3830
		// (get) Token: 0x0600318D RID: 12685 RVA: 0x0004A2D5 File Offset: 0x000484D5
		// (set) Token: 0x0600318E RID: 12686 RVA: 0x0004A2DD File Offset: 0x000484DD
		public SocketId? SocketId { get; set; }
	}
}
