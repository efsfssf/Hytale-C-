using System;

namespace Epic.OnlineServices.Ecom
{
	// Token: 0x0200056B RID: 1387
	public struct RedeemEntitlementsOptions
	{
		// Token: 0x17000A96 RID: 2710
		// (get) Token: 0x06002442 RID: 9282 RVA: 0x0003562B File Offset: 0x0003382B
		// (set) Token: 0x06002443 RID: 9283 RVA: 0x00035633 File Offset: 0x00033833
		public EpicAccountId LocalUserId { get; set; }

		// Token: 0x17000A97 RID: 2711
		// (get) Token: 0x06002444 RID: 9284 RVA: 0x0003563C File Offset: 0x0003383C
		// (set) Token: 0x06002445 RID: 9285 RVA: 0x00035644 File Offset: 0x00033844
		public Utf8String[] EntitlementIds { get; set; }
	}
}
