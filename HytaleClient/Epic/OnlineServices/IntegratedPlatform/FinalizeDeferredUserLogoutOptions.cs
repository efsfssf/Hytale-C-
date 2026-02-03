using System;

namespace Epic.OnlineServices.IntegratedPlatform
{
	// Token: 0x020004B7 RID: 1207
	public struct FinalizeDeferredUserLogoutOptions
	{
		// Token: 0x170008E6 RID: 2278
		// (get) Token: 0x06001F6C RID: 8044 RVA: 0x0002DFBC File Offset: 0x0002C1BC
		// (set) Token: 0x06001F6D RID: 8045 RVA: 0x0002DFC4 File Offset: 0x0002C1C4
		public Utf8String PlatformType { get; set; }

		// Token: 0x170008E7 RID: 2279
		// (get) Token: 0x06001F6E RID: 8046 RVA: 0x0002DFCD File Offset: 0x0002C1CD
		// (set) Token: 0x06001F6F RID: 8047 RVA: 0x0002DFD5 File Offset: 0x0002C1D5
		public Utf8String LocalPlatformUserId { get; set; }

		// Token: 0x170008E8 RID: 2280
		// (get) Token: 0x06001F70 RID: 8048 RVA: 0x0002DFDE File Offset: 0x0002C1DE
		// (set) Token: 0x06001F71 RID: 8049 RVA: 0x0002DFE6 File Offset: 0x0002C1E6
		public LoginStatus ExpectedLoginStatus { get; set; }
	}
}
