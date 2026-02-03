using System;

namespace Epic.OnlineServices.P2P
{
	// Token: 0x02000797 RID: 1943
	public struct OnPeerConnectionInterruptedInfo : ICallbackInfo
	{
		// Token: 0x17000F38 RID: 3896
		// (get) Token: 0x06003251 RID: 12881 RVA: 0x0004B294 File Offset: 0x00049494
		// (set) Token: 0x06003252 RID: 12882 RVA: 0x0004B29C File Offset: 0x0004949C
		public object ClientData { get; set; }

		// Token: 0x17000F39 RID: 3897
		// (get) Token: 0x06003253 RID: 12883 RVA: 0x0004B2A5 File Offset: 0x000494A5
		// (set) Token: 0x06003254 RID: 12884 RVA: 0x0004B2AD File Offset: 0x000494AD
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x17000F3A RID: 3898
		// (get) Token: 0x06003255 RID: 12885 RVA: 0x0004B2B6 File Offset: 0x000494B6
		// (set) Token: 0x06003256 RID: 12886 RVA: 0x0004B2BE File Offset: 0x000494BE
		public ProductUserId RemoteUserId { get; set; }

		// Token: 0x17000F3B RID: 3899
		// (get) Token: 0x06003257 RID: 12887 RVA: 0x0004B2C7 File Offset: 0x000494C7
		// (set) Token: 0x06003258 RID: 12888 RVA: 0x0004B2CF File Offset: 0x000494CF
		public SocketId? SocketId { get; set; }

		// Token: 0x06003259 RID: 12889 RVA: 0x0004B2D8 File Offset: 0x000494D8
		public Result? GetResultCode()
		{
			return null;
		}

		// Token: 0x0600325A RID: 12890 RVA: 0x0004B2F3 File Offset: 0x000494F3
		internal void Set(ref OnPeerConnectionInterruptedInfoInternal other)
		{
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.RemoteUserId = other.RemoteUserId;
			this.SocketId = other.SocketId;
		}
	}
}
