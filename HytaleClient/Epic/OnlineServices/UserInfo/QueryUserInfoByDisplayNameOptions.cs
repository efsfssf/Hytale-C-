using System;

namespace Epic.OnlineServices.UserInfo
{
	// Token: 0x02000040 RID: 64
	public struct QueryUserInfoByDisplayNameOptions
	{
		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060003FF RID: 1023 RVA: 0x000059C4 File Offset: 0x00003BC4
		// (set) Token: 0x06000400 RID: 1024 RVA: 0x000059CC File Offset: 0x00003BCC
		public EpicAccountId LocalUserId { get; set; }

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x06000401 RID: 1025 RVA: 0x000059D5 File Offset: 0x00003BD5
		// (set) Token: 0x06000402 RID: 1026 RVA: 0x000059DD File Offset: 0x00003BDD
		public Utf8String DisplayName { get; set; }
	}
}
