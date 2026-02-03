using System;

namespace Epic.OnlineServices.P2P
{
	// Token: 0x02000793 RID: 1939
	public struct OnPeerConnectionEstablishedInfo : ICallbackInfo
	{
		// Token: 0x17000F2B RID: 3883
		// (get) Token: 0x0600322A RID: 12842 RVA: 0x0004AF48 File Offset: 0x00049148
		// (set) Token: 0x0600322B RID: 12843 RVA: 0x0004AF50 File Offset: 0x00049150
		public object ClientData { get; set; }

		// Token: 0x17000F2C RID: 3884
		// (get) Token: 0x0600322C RID: 12844 RVA: 0x0004AF59 File Offset: 0x00049159
		// (set) Token: 0x0600322D RID: 12845 RVA: 0x0004AF61 File Offset: 0x00049161
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x17000F2D RID: 3885
		// (get) Token: 0x0600322E RID: 12846 RVA: 0x0004AF6A File Offset: 0x0004916A
		// (set) Token: 0x0600322F RID: 12847 RVA: 0x0004AF72 File Offset: 0x00049172
		public ProductUserId RemoteUserId { get; set; }

		// Token: 0x17000F2E RID: 3886
		// (get) Token: 0x06003230 RID: 12848 RVA: 0x0004AF7B File Offset: 0x0004917B
		// (set) Token: 0x06003231 RID: 12849 RVA: 0x0004AF83 File Offset: 0x00049183
		public SocketId? SocketId { get; set; }

		// Token: 0x17000F2F RID: 3887
		// (get) Token: 0x06003232 RID: 12850 RVA: 0x0004AF8C File Offset: 0x0004918C
		// (set) Token: 0x06003233 RID: 12851 RVA: 0x0004AF94 File Offset: 0x00049194
		public ConnectionEstablishedType ConnectionType { get; set; }

		// Token: 0x17000F30 RID: 3888
		// (get) Token: 0x06003234 RID: 12852 RVA: 0x0004AF9D File Offset: 0x0004919D
		// (set) Token: 0x06003235 RID: 12853 RVA: 0x0004AFA5 File Offset: 0x000491A5
		public NetworkConnectionType NetworkType { get; set; }

		// Token: 0x06003236 RID: 12854 RVA: 0x0004AFB0 File Offset: 0x000491B0
		public Result? GetResultCode()
		{
			return null;
		}

		// Token: 0x06003237 RID: 12855 RVA: 0x0004AFCC File Offset: 0x000491CC
		internal void Set(ref OnPeerConnectionEstablishedInfoInternal other)
		{
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.RemoteUserId = other.RemoteUserId;
			this.SocketId = other.SocketId;
			this.ConnectionType = other.ConnectionType;
			this.NetworkType = other.NetworkType;
		}
	}
}
