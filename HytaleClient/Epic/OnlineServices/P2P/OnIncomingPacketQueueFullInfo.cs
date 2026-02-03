using System;

namespace Epic.OnlineServices.P2P
{
	// Token: 0x0200078F RID: 1935
	public struct OnIncomingPacketQueueFullInfo : ICallbackInfo
	{
		// Token: 0x17000F1E RID: 3870
		// (get) Token: 0x06003203 RID: 12803 RVA: 0x0004AC33 File Offset: 0x00048E33
		// (set) Token: 0x06003204 RID: 12804 RVA: 0x0004AC3B File Offset: 0x00048E3B
		public object ClientData { get; set; }

		// Token: 0x17000F1F RID: 3871
		// (get) Token: 0x06003205 RID: 12805 RVA: 0x0004AC44 File Offset: 0x00048E44
		// (set) Token: 0x06003206 RID: 12806 RVA: 0x0004AC4C File Offset: 0x00048E4C
		public ulong PacketQueueMaxSizeBytes { get; set; }

		// Token: 0x17000F20 RID: 3872
		// (get) Token: 0x06003207 RID: 12807 RVA: 0x0004AC55 File Offset: 0x00048E55
		// (set) Token: 0x06003208 RID: 12808 RVA: 0x0004AC5D File Offset: 0x00048E5D
		public ulong PacketQueueCurrentSizeBytes { get; set; }

		// Token: 0x17000F21 RID: 3873
		// (get) Token: 0x06003209 RID: 12809 RVA: 0x0004AC66 File Offset: 0x00048E66
		// (set) Token: 0x0600320A RID: 12810 RVA: 0x0004AC6E File Offset: 0x00048E6E
		public ProductUserId OverflowPacketLocalUserId { get; set; }

		// Token: 0x17000F22 RID: 3874
		// (get) Token: 0x0600320B RID: 12811 RVA: 0x0004AC77 File Offset: 0x00048E77
		// (set) Token: 0x0600320C RID: 12812 RVA: 0x0004AC7F File Offset: 0x00048E7F
		public byte OverflowPacketChannel { get; set; }

		// Token: 0x17000F23 RID: 3875
		// (get) Token: 0x0600320D RID: 12813 RVA: 0x0004AC88 File Offset: 0x00048E88
		// (set) Token: 0x0600320E RID: 12814 RVA: 0x0004AC90 File Offset: 0x00048E90
		public uint OverflowPacketSizeBytes { get; set; }

		// Token: 0x0600320F RID: 12815 RVA: 0x0004AC9C File Offset: 0x00048E9C
		public Result? GetResultCode()
		{
			return null;
		}

		// Token: 0x06003210 RID: 12816 RVA: 0x0004ACB8 File Offset: 0x00048EB8
		internal void Set(ref OnIncomingPacketQueueFullInfoInternal other)
		{
			this.ClientData = other.ClientData;
			this.PacketQueueMaxSizeBytes = other.PacketQueueMaxSizeBytes;
			this.PacketQueueCurrentSizeBytes = other.PacketQueueCurrentSizeBytes;
			this.OverflowPacketLocalUserId = other.OverflowPacketLocalUserId;
			this.OverflowPacketChannel = other.OverflowPacketChannel;
			this.OverflowPacketSizeBytes = other.OverflowPacketSizeBytes;
		}
	}
}
