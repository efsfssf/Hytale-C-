using System;

namespace Epic.OnlineServices.Connect
{
	// Token: 0x020005E9 RID: 1513
	public struct IdToken
	{
		// Token: 0x17000B72 RID: 2930
		// (get) Token: 0x06002744 RID: 10052 RVA: 0x0003A1E3 File Offset: 0x000383E3
		// (set) Token: 0x06002745 RID: 10053 RVA: 0x0003A1EB File Offset: 0x000383EB
		public ProductUserId ProductUserId { get; set; }

		// Token: 0x17000B73 RID: 2931
		// (get) Token: 0x06002746 RID: 10054 RVA: 0x0003A1F4 File Offset: 0x000383F4
		// (set) Token: 0x06002747 RID: 10055 RVA: 0x0003A1FC File Offset: 0x000383FC
		public Utf8String JsonWebToken { get; set; }

		// Token: 0x06002748 RID: 10056 RVA: 0x0003A205 File Offset: 0x00038405
		internal void Set(ref IdTokenInternal other)
		{
			this.ProductUserId = other.ProductUserId;
			this.JsonWebToken = other.JsonWebToken;
		}
	}
}
