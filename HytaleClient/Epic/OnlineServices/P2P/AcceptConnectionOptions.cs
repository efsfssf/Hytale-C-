using System;

namespace Epic.OnlineServices.P2P
{
	// Token: 0x0200076B RID: 1899
	public struct AcceptConnectionOptions
	{
		// Token: 0x17000EEF RID: 3823
		// (get) Token: 0x0600317C RID: 12668 RVA: 0x0004A176 File Offset: 0x00048376
		// (set) Token: 0x0600317D RID: 12669 RVA: 0x0004A17E File Offset: 0x0004837E
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x17000EF0 RID: 3824
		// (get) Token: 0x0600317E RID: 12670 RVA: 0x0004A187 File Offset: 0x00048387
		// (set) Token: 0x0600317F RID: 12671 RVA: 0x0004A18F File Offset: 0x0004838F
		public ProductUserId RemoteUserId { get; set; }

		// Token: 0x17000EF1 RID: 3825
		// (get) Token: 0x06003180 RID: 12672 RVA: 0x0004A198 File Offset: 0x00048398
		// (set) Token: 0x06003181 RID: 12673 RVA: 0x0004A1A0 File Offset: 0x000483A0
		public SocketId? SocketId { get; set; }
	}
}
