using System;

namespace Epic.OnlineServices.P2P
{
	// Token: 0x02000771 RID: 1905
	public struct AddNotifyPeerConnectionEstablishedOptions
	{
		// Token: 0x17000EF9 RID: 3833
		// (get) Token: 0x06003194 RID: 12692 RVA: 0x0004A392 File Offset: 0x00048592
		// (set) Token: 0x06003195 RID: 12693 RVA: 0x0004A39A File Offset: 0x0004859A
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x17000EFA RID: 3834
		// (get) Token: 0x06003196 RID: 12694 RVA: 0x0004A3A3 File Offset: 0x000485A3
		// (set) Token: 0x06003197 RID: 12695 RVA: 0x0004A3AB File Offset: 0x000485AB
		public SocketId? SocketId { get; set; }
	}
}
