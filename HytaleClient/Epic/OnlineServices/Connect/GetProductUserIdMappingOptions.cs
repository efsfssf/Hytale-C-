using System;

namespace Epic.OnlineServices.Connect
{
	// Token: 0x020005E7 RID: 1511
	public struct GetProductUserIdMappingOptions
	{
		// Token: 0x17000B6C RID: 2924
		// (get) Token: 0x06002738 RID: 10040 RVA: 0x0003A0D9 File Offset: 0x000382D9
		// (set) Token: 0x06002739 RID: 10041 RVA: 0x0003A0E1 File Offset: 0x000382E1
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x17000B6D RID: 2925
		// (get) Token: 0x0600273A RID: 10042 RVA: 0x0003A0EA File Offset: 0x000382EA
		// (set) Token: 0x0600273B RID: 10043 RVA: 0x0003A0F2 File Offset: 0x000382F2
		public ExternalAccountType AccountIdType { get; set; }

		// Token: 0x17000B6E RID: 2926
		// (get) Token: 0x0600273C RID: 10044 RVA: 0x0003A0FB File Offset: 0x000382FB
		// (set) Token: 0x0600273D RID: 10045 RVA: 0x0003A103 File Offset: 0x00038303
		public ProductUserId TargetProductUserId { get; set; }
	}
}
