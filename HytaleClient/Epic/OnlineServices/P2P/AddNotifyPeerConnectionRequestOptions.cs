using System;

namespace Epic.OnlineServices.P2P
{
	// Token: 0x02000775 RID: 1909
	public struct AddNotifyPeerConnectionRequestOptions
	{
		// Token: 0x17000F01 RID: 3841
		// (get) Token: 0x060031A6 RID: 12710 RVA: 0x0004A532 File Offset: 0x00048732
		// (set) Token: 0x060031A7 RID: 12711 RVA: 0x0004A53A File Offset: 0x0004873A
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x17000F02 RID: 3842
		// (get) Token: 0x060031A8 RID: 12712 RVA: 0x0004A543 File Offset: 0x00048743
		// (set) Token: 0x060031A9 RID: 12713 RVA: 0x0004A54B File Offset: 0x0004874B
		public SocketId? SocketId { get; set; }
	}
}
