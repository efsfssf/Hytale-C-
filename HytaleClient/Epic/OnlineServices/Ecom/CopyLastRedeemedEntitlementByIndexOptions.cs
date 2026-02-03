using System;

namespace Epic.OnlineServices.Ecom
{
	// Token: 0x02000518 RID: 1304
	public struct CopyLastRedeemedEntitlementByIndexOptions
	{
		// Token: 0x170009E7 RID: 2535
		// (get) Token: 0x06002236 RID: 8758 RVA: 0x000323B3 File Offset: 0x000305B3
		// (set) Token: 0x06002237 RID: 8759 RVA: 0x000323BB File Offset: 0x000305BB
		public EpicAccountId LocalUserId { get; set; }

		// Token: 0x170009E8 RID: 2536
		// (get) Token: 0x06002238 RID: 8760 RVA: 0x000323C4 File Offset: 0x000305C4
		// (set) Token: 0x06002239 RID: 8761 RVA: 0x000323CC File Offset: 0x000305CC
		public uint RedeemedEntitlementIndex { get; set; }
	}
}
