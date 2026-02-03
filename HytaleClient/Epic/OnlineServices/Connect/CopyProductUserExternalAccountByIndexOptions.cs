using System;

namespace Epic.OnlineServices.Connect
{
	// Token: 0x020005CF RID: 1487
	public struct CopyProductUserExternalAccountByIndexOptions
	{
		// Token: 0x17000B3B RID: 2875
		// (get) Token: 0x060026B1 RID: 9905 RVA: 0x00039492 File Offset: 0x00037692
		// (set) Token: 0x060026B2 RID: 9906 RVA: 0x0003949A File Offset: 0x0003769A
		public ProductUserId TargetUserId { get; set; }

		// Token: 0x17000B3C RID: 2876
		// (get) Token: 0x060026B3 RID: 9907 RVA: 0x000394A3 File Offset: 0x000376A3
		// (set) Token: 0x060026B4 RID: 9908 RVA: 0x000394AB File Offset: 0x000376AB
		public uint ExternalAccountInfoIndex { get; set; }
	}
}
