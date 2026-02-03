using System;

namespace Epic.OnlineServices.P2P
{
	// Token: 0x020007A1 RID: 1953
	public struct PacketQueueInfo
	{
		// Token: 0x17000F53 RID: 3923
		// (get) Token: 0x060032A6 RID: 12966 RVA: 0x0004B998 File Offset: 0x00049B98
		// (set) Token: 0x060032A7 RID: 12967 RVA: 0x0004B9A0 File Offset: 0x00049BA0
		public ulong IncomingPacketQueueMaxSizeBytes { get; set; }

		// Token: 0x17000F54 RID: 3924
		// (get) Token: 0x060032A8 RID: 12968 RVA: 0x0004B9A9 File Offset: 0x00049BA9
		// (set) Token: 0x060032A9 RID: 12969 RVA: 0x0004B9B1 File Offset: 0x00049BB1
		public ulong IncomingPacketQueueCurrentSizeBytes { get; set; }

		// Token: 0x17000F55 RID: 3925
		// (get) Token: 0x060032AA RID: 12970 RVA: 0x0004B9BA File Offset: 0x00049BBA
		// (set) Token: 0x060032AB RID: 12971 RVA: 0x0004B9C2 File Offset: 0x00049BC2
		public ulong IncomingPacketQueueCurrentPacketCount { get; set; }

		// Token: 0x17000F56 RID: 3926
		// (get) Token: 0x060032AC RID: 12972 RVA: 0x0004B9CB File Offset: 0x00049BCB
		// (set) Token: 0x060032AD RID: 12973 RVA: 0x0004B9D3 File Offset: 0x00049BD3
		public ulong OutgoingPacketQueueMaxSizeBytes { get; set; }

		// Token: 0x17000F57 RID: 3927
		// (get) Token: 0x060032AE RID: 12974 RVA: 0x0004B9DC File Offset: 0x00049BDC
		// (set) Token: 0x060032AF RID: 12975 RVA: 0x0004B9E4 File Offset: 0x00049BE4
		public ulong OutgoingPacketQueueCurrentSizeBytes { get; set; }

		// Token: 0x17000F58 RID: 3928
		// (get) Token: 0x060032B0 RID: 12976 RVA: 0x0004B9ED File Offset: 0x00049BED
		// (set) Token: 0x060032B1 RID: 12977 RVA: 0x0004B9F5 File Offset: 0x00049BF5
		public ulong OutgoingPacketQueueCurrentPacketCount { get; set; }

		// Token: 0x060032B2 RID: 12978 RVA: 0x0004BA00 File Offset: 0x00049C00
		internal void Set(ref PacketQueueInfoInternal other)
		{
			this.IncomingPacketQueueMaxSizeBytes = other.IncomingPacketQueueMaxSizeBytes;
			this.IncomingPacketQueueCurrentSizeBytes = other.IncomingPacketQueueCurrentSizeBytes;
			this.IncomingPacketQueueCurrentPacketCount = other.IncomingPacketQueueCurrentPacketCount;
			this.OutgoingPacketQueueMaxSizeBytes = other.OutgoingPacketQueueMaxSizeBytes;
			this.OutgoingPacketQueueCurrentSizeBytes = other.OutgoingPacketQueueCurrentSizeBytes;
			this.OutgoingPacketQueueCurrentPacketCount = other.OutgoingPacketQueueCurrentPacketCount;
		}
	}
}
