using System;

namespace Epic.OnlineServices.AntiCheatClient
{
	// Token: 0x020006F6 RID: 1782
	public struct ReceiveMessageFromPeerOptions
	{
		// Token: 0x17000DA3 RID: 3491
		// (get) Token: 0x06002DE5 RID: 11749 RVA: 0x00043C02 File Offset: 0x00041E02
		// (set) Token: 0x06002DE6 RID: 11750 RVA: 0x00043C0A File Offset: 0x00041E0A
		public IntPtr PeerHandle { get; set; }

		// Token: 0x17000DA4 RID: 3492
		// (get) Token: 0x06002DE7 RID: 11751 RVA: 0x00043C13 File Offset: 0x00041E13
		// (set) Token: 0x06002DE8 RID: 11752 RVA: 0x00043C1B File Offset: 0x00041E1B
		public ArraySegment<byte> Data { get; set; }
	}
}
