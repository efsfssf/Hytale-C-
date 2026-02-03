using System;

namespace Epic.OnlineServices.P2P
{
	// Token: 0x02000777 RID: 1911
	public struct ClearPacketQueueOptions
	{
		// Token: 0x17000F05 RID: 3845
		// (get) Token: 0x060031AF RID: 12719 RVA: 0x0004A602 File Offset: 0x00048802
		// (set) Token: 0x060031B0 RID: 12720 RVA: 0x0004A60A File Offset: 0x0004880A
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x17000F06 RID: 3846
		// (get) Token: 0x060031B1 RID: 12721 RVA: 0x0004A613 File Offset: 0x00048813
		// (set) Token: 0x060031B2 RID: 12722 RVA: 0x0004A61B File Offset: 0x0004881B
		public ProductUserId RemoteUserId { get; set; }

		// Token: 0x17000F07 RID: 3847
		// (get) Token: 0x060031B3 RID: 12723 RVA: 0x0004A624 File Offset: 0x00048824
		// (set) Token: 0x060031B4 RID: 12724 RVA: 0x0004A62C File Offset: 0x0004882C
		public SocketId? SocketId { get; set; }
	}
}
