using System;

namespace Epic.OnlineServices.P2P
{
	// Token: 0x020007A8 RID: 1960
	public struct SetPacketQueueSizeOptions
	{
		// Token: 0x17000F67 RID: 3943
		// (get) Token: 0x060032D6 RID: 13014 RVA: 0x0004BCF8 File Offset: 0x00049EF8
		// (set) Token: 0x060032D7 RID: 13015 RVA: 0x0004BD00 File Offset: 0x00049F00
		public ulong IncomingPacketQueueMaxSizeBytes { get; set; }

		// Token: 0x17000F68 RID: 3944
		// (get) Token: 0x060032D8 RID: 13016 RVA: 0x0004BD09 File Offset: 0x00049F09
		// (set) Token: 0x060032D9 RID: 13017 RVA: 0x0004BD11 File Offset: 0x00049F11
		public ulong OutgoingPacketQueueMaxSizeBytes { get; set; }
	}
}
