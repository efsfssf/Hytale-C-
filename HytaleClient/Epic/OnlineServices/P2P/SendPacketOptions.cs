using System;

namespace Epic.OnlineServices.P2P
{
	// Token: 0x020007A7 RID: 1959
	public struct SendPacketOptions
	{
		// Token: 0x17000F5F RID: 3935
		// (get) Token: 0x060032C6 RID: 12998 RVA: 0x0004BC70 File Offset: 0x00049E70
		// (set) Token: 0x060032C7 RID: 12999 RVA: 0x0004BC78 File Offset: 0x00049E78
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x17000F60 RID: 3936
		// (get) Token: 0x060032C8 RID: 13000 RVA: 0x0004BC81 File Offset: 0x00049E81
		// (set) Token: 0x060032C9 RID: 13001 RVA: 0x0004BC89 File Offset: 0x00049E89
		public ProductUserId RemoteUserId { get; set; }

		// Token: 0x17000F61 RID: 3937
		// (get) Token: 0x060032CA RID: 13002 RVA: 0x0004BC92 File Offset: 0x00049E92
		// (set) Token: 0x060032CB RID: 13003 RVA: 0x0004BC9A File Offset: 0x00049E9A
		public SocketId? SocketId { get; set; }

		// Token: 0x17000F62 RID: 3938
		// (get) Token: 0x060032CC RID: 13004 RVA: 0x0004BCA3 File Offset: 0x00049EA3
		// (set) Token: 0x060032CD RID: 13005 RVA: 0x0004BCAB File Offset: 0x00049EAB
		public byte Channel { get; set; }

		// Token: 0x17000F63 RID: 3939
		// (get) Token: 0x060032CE RID: 13006 RVA: 0x0004BCB4 File Offset: 0x00049EB4
		// (set) Token: 0x060032CF RID: 13007 RVA: 0x0004BCBC File Offset: 0x00049EBC
		public ArraySegment<byte> Data { get; set; }

		// Token: 0x17000F64 RID: 3940
		// (get) Token: 0x060032D0 RID: 13008 RVA: 0x0004BCC5 File Offset: 0x00049EC5
		// (set) Token: 0x060032D1 RID: 13009 RVA: 0x0004BCCD File Offset: 0x00049ECD
		public bool AllowDelayedDelivery { get; set; }

		// Token: 0x17000F65 RID: 3941
		// (get) Token: 0x060032D2 RID: 13010 RVA: 0x0004BCD6 File Offset: 0x00049ED6
		// (set) Token: 0x060032D3 RID: 13011 RVA: 0x0004BCDE File Offset: 0x00049EDE
		public PacketReliability Reliability { get; set; }

		// Token: 0x17000F66 RID: 3942
		// (get) Token: 0x060032D4 RID: 13012 RVA: 0x0004BCE7 File Offset: 0x00049EE7
		// (set) Token: 0x060032D5 RID: 13013 RVA: 0x0004BCEF File Offset: 0x00049EEF
		public bool DisableAutoAcceptConnection { get; set; }
	}
}
