using System;

namespace Epic.OnlineServices.P2P
{
	// Token: 0x02000773 RID: 1907
	public struct AddNotifyPeerConnectionInterruptedOptions
	{
		// Token: 0x17000EFD RID: 3837
		// (get) Token: 0x0600319D RID: 12701 RVA: 0x0004A462 File Offset: 0x00048662
		// (set) Token: 0x0600319E RID: 12702 RVA: 0x0004A46A File Offset: 0x0004866A
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x17000EFE RID: 3838
		// (get) Token: 0x0600319F RID: 12703 RVA: 0x0004A473 File Offset: 0x00048673
		// (set) Token: 0x060031A0 RID: 12704 RVA: 0x0004A47B File Offset: 0x0004867B
		public SocketId? SocketId { get; set; }
	}
}
