using System;

namespace Epic.OnlineServices.PlayerDataStorage
{
	// Token: 0x020002EB RID: 747
	public struct CopyFileMetadataAtIndexOptions
	{
		// Token: 0x1700057D RID: 1405
		// (get) Token: 0x06001478 RID: 5240 RVA: 0x0001DE16 File Offset: 0x0001C016
		// (set) Token: 0x06001479 RID: 5241 RVA: 0x0001DE1E File Offset: 0x0001C01E
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x1700057E RID: 1406
		// (get) Token: 0x0600147A RID: 5242 RVA: 0x0001DE27 File Offset: 0x0001C027
		// (set) Token: 0x0600147B RID: 5243 RVA: 0x0001DE2F File Offset: 0x0001C02F
		public uint Index { get; set; }
	}
}
