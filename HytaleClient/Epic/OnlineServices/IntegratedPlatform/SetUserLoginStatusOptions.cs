using System;

namespace Epic.OnlineServices.IntegratedPlatform
{
	// Token: 0x020004C5 RID: 1221
	public struct SetUserLoginStatusOptions
	{
		// Token: 0x170008F4 RID: 2292
		// (get) Token: 0x06001FAE RID: 8110 RVA: 0x0002E58D File Offset: 0x0002C78D
		// (set) Token: 0x06001FAF RID: 8111 RVA: 0x0002E595 File Offset: 0x0002C795
		public Utf8String PlatformType { get; set; }

		// Token: 0x170008F5 RID: 2293
		// (get) Token: 0x06001FB0 RID: 8112 RVA: 0x0002E59E File Offset: 0x0002C79E
		// (set) Token: 0x06001FB1 RID: 8113 RVA: 0x0002E5A6 File Offset: 0x0002C7A6
		public Utf8String LocalPlatformUserId { get; set; }

		// Token: 0x170008F6 RID: 2294
		// (get) Token: 0x06001FB2 RID: 8114 RVA: 0x0002E5AF File Offset: 0x0002C7AF
		// (set) Token: 0x06001FB3 RID: 8115 RVA: 0x0002E5B7 File Offset: 0x0002C7B7
		public LoginStatus CurrentLoginStatus { get; set; }
	}
}
