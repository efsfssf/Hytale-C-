using System;

namespace Epic.OnlineServices.Auth
{
	// Token: 0x02000673 RID: 1651
	public struct IOSLoginOptions
	{
		// Token: 0x17000C9D RID: 3229
		// (get) Token: 0x06002B01 RID: 11009 RVA: 0x0003F40F File Offset: 0x0003D60F
		// (set) Token: 0x06002B02 RID: 11010 RVA: 0x0003F417 File Offset: 0x0003D617
		public IOSCredentials? Credentials { get; set; }

		// Token: 0x17000C9E RID: 3230
		// (get) Token: 0x06002B03 RID: 11011 RVA: 0x0003F420 File Offset: 0x0003D620
		// (set) Token: 0x06002B04 RID: 11012 RVA: 0x0003F428 File Offset: 0x0003D628
		public AuthScopeFlags ScopeFlags { get; set; }

		// Token: 0x17000C9F RID: 3231
		// (get) Token: 0x06002B05 RID: 11013 RVA: 0x0003F431 File Offset: 0x0003D631
		// (set) Token: 0x06002B06 RID: 11014 RVA: 0x0003F439 File Offset: 0x0003D639
		public LoginFlags LoginFlags { get; set; }
	}
}
