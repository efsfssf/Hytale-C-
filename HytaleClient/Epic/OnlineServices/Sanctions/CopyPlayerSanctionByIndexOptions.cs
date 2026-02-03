using System;

namespace Epic.OnlineServices.Sanctions
{
	// Token: 0x02000197 RID: 407
	public struct CopyPlayerSanctionByIndexOptions
	{
		// Token: 0x170002B8 RID: 696
		// (get) Token: 0x06000BDD RID: 3037 RVA: 0x000115BD File Offset: 0x0000F7BD
		// (set) Token: 0x06000BDE RID: 3038 RVA: 0x000115C5 File Offset: 0x0000F7C5
		public ProductUserId TargetUserId { get; set; }

		// Token: 0x170002B9 RID: 697
		// (get) Token: 0x06000BDF RID: 3039 RVA: 0x000115CE File Offset: 0x0000F7CE
		// (set) Token: 0x06000BE0 RID: 3040 RVA: 0x000115D6 File Offset: 0x0000F7D6
		public uint SanctionIndex { get; set; }
	}
}
