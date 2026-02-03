using System;

namespace Epic.OnlineServices.P2P
{
	// Token: 0x02000779 RID: 1913
	public struct CloseConnectionOptions
	{
		// Token: 0x17000F0B RID: 3851
		// (get) Token: 0x060031BB RID: 12731 RVA: 0x0004A71F File Offset: 0x0004891F
		// (set) Token: 0x060031BC RID: 12732 RVA: 0x0004A727 File Offset: 0x00048927
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x17000F0C RID: 3852
		// (get) Token: 0x060031BD RID: 12733 RVA: 0x0004A730 File Offset: 0x00048930
		// (set) Token: 0x060031BE RID: 12734 RVA: 0x0004A738 File Offset: 0x00048938
		public ProductUserId RemoteUserId { get; set; }

		// Token: 0x17000F0D RID: 3853
		// (get) Token: 0x060031BF RID: 12735 RVA: 0x0004A741 File Offset: 0x00048941
		// (set) Token: 0x060031C0 RID: 12736 RVA: 0x0004A749 File Offset: 0x00048949
		public SocketId? SocketId { get; set; }
	}
}
