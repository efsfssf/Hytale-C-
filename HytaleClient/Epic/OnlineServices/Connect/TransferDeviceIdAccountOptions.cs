using System;

namespace Epic.OnlineServices.Connect
{
	// Token: 0x0200061D RID: 1565
	public struct TransferDeviceIdAccountOptions
	{
		// Token: 0x17000BC1 RID: 3009
		// (get) Token: 0x06002876 RID: 10358 RVA: 0x0003B4EE File Offset: 0x000396EE
		// (set) Token: 0x06002877 RID: 10359 RVA: 0x0003B4F6 File Offset: 0x000396F6
		public ProductUserId PrimaryLocalUserId { get; set; }

		// Token: 0x17000BC2 RID: 3010
		// (get) Token: 0x06002878 RID: 10360 RVA: 0x0003B4FF File Offset: 0x000396FF
		// (set) Token: 0x06002879 RID: 10361 RVA: 0x0003B507 File Offset: 0x00039707
		public ProductUserId LocalDeviceUserId { get; set; }

		// Token: 0x17000BC3 RID: 3011
		// (get) Token: 0x0600287A RID: 10362 RVA: 0x0003B510 File Offset: 0x00039710
		// (set) Token: 0x0600287B RID: 10363 RVA: 0x0003B518 File Offset: 0x00039718
		public ProductUserId ProductUserIdToPreserve { get; set; }
	}
}
