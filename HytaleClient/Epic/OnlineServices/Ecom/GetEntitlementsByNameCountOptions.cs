using System;

namespace Epic.OnlineServices.Ecom
{
	// Token: 0x0200052A RID: 1322
	public struct GetEntitlementsByNameCountOptions
	{
		// Token: 0x17000A13 RID: 2579
		// (get) Token: 0x060022C0 RID: 8896 RVA: 0x000337B7 File Offset: 0x000319B7
		// (set) Token: 0x060022C1 RID: 8897 RVA: 0x000337BF File Offset: 0x000319BF
		public EpicAccountId LocalUserId { get; set; }

		// Token: 0x17000A14 RID: 2580
		// (get) Token: 0x060022C2 RID: 8898 RVA: 0x000337C8 File Offset: 0x000319C8
		// (set) Token: 0x060022C3 RID: 8899 RVA: 0x000337D0 File Offset: 0x000319D0
		public Utf8String EntitlementName { get; set; }
	}
}
