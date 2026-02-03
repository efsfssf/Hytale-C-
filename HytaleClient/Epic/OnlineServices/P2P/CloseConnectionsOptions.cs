using System;

namespace Epic.OnlineServices.P2P
{
	// Token: 0x0200077B RID: 1915
	public struct CloseConnectionsOptions
	{
		// Token: 0x17000F11 RID: 3857
		// (get) Token: 0x060031C7 RID: 12743 RVA: 0x0004A83B File Offset: 0x00048A3B
		// (set) Token: 0x060031C8 RID: 12744 RVA: 0x0004A843 File Offset: 0x00048A43
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x17000F12 RID: 3858
		// (get) Token: 0x060031C9 RID: 12745 RVA: 0x0004A84C File Offset: 0x00048A4C
		// (set) Token: 0x060031CA RID: 12746 RVA: 0x0004A854 File Offset: 0x00048A54
		public SocketId? SocketId { get; set; }
	}
}
