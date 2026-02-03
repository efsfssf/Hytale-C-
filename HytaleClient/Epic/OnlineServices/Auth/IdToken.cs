using System;

namespace Epic.OnlineServices.Auth
{
	// Token: 0x0200063A RID: 1594
	public struct IdToken
	{
		// Token: 0x17000C04 RID: 3076
		// (get) Token: 0x06002945 RID: 10565 RVA: 0x0003CD69 File Offset: 0x0003AF69
		// (set) Token: 0x06002946 RID: 10566 RVA: 0x0003CD71 File Offset: 0x0003AF71
		public EpicAccountId AccountId { get; set; }

		// Token: 0x17000C05 RID: 3077
		// (get) Token: 0x06002947 RID: 10567 RVA: 0x0003CD7A File Offset: 0x0003AF7A
		// (set) Token: 0x06002948 RID: 10568 RVA: 0x0003CD82 File Offset: 0x0003AF82
		public Utf8String JsonWebToken { get; set; }

		// Token: 0x06002949 RID: 10569 RVA: 0x0003CD8B File Offset: 0x0003AF8B
		internal void Set(ref IdTokenInternal other)
		{
			this.AccountId = other.AccountId;
			this.JsonWebToken = other.JsonWebToken;
		}
	}
}
