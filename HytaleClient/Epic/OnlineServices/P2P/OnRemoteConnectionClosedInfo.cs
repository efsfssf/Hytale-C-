using System;

namespace Epic.OnlineServices.P2P
{
	// Token: 0x0200079F RID: 1951
	public struct OnRemoteConnectionClosedInfo : ICallbackInfo
	{
		// Token: 0x17000F48 RID: 3912
		// (get) Token: 0x0600328B RID: 12939 RVA: 0x0004B6B2 File Offset: 0x000498B2
		// (set) Token: 0x0600328C RID: 12940 RVA: 0x0004B6BA File Offset: 0x000498BA
		public object ClientData { get; set; }

		// Token: 0x17000F49 RID: 3913
		// (get) Token: 0x0600328D RID: 12941 RVA: 0x0004B6C3 File Offset: 0x000498C3
		// (set) Token: 0x0600328E RID: 12942 RVA: 0x0004B6CB File Offset: 0x000498CB
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x17000F4A RID: 3914
		// (get) Token: 0x0600328F RID: 12943 RVA: 0x0004B6D4 File Offset: 0x000498D4
		// (set) Token: 0x06003290 RID: 12944 RVA: 0x0004B6DC File Offset: 0x000498DC
		public ProductUserId RemoteUserId { get; set; }

		// Token: 0x17000F4B RID: 3915
		// (get) Token: 0x06003291 RID: 12945 RVA: 0x0004B6E5 File Offset: 0x000498E5
		// (set) Token: 0x06003292 RID: 12946 RVA: 0x0004B6ED File Offset: 0x000498ED
		public SocketId? SocketId { get; set; }

		// Token: 0x17000F4C RID: 3916
		// (get) Token: 0x06003293 RID: 12947 RVA: 0x0004B6F6 File Offset: 0x000498F6
		// (set) Token: 0x06003294 RID: 12948 RVA: 0x0004B6FE File Offset: 0x000498FE
		public ConnectionClosedReason Reason { get; set; }

		// Token: 0x06003295 RID: 12949 RVA: 0x0004B708 File Offset: 0x00049908
		public Result? GetResultCode()
		{
			return null;
		}

		// Token: 0x06003296 RID: 12950 RVA: 0x0004B724 File Offset: 0x00049924
		internal void Set(ref OnRemoteConnectionClosedInfoInternal other)
		{
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.RemoteUserId = other.RemoteUserId;
			this.SocketId = other.SocketId;
			this.Reason = other.Reason;
		}
	}
}
