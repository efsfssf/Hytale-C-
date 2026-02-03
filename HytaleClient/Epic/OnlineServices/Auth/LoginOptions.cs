using System;

namespace Epic.OnlineServices.Auth
{
	// Token: 0x02000645 RID: 1605
	public struct LoginOptions
	{
		// Token: 0x17000C28 RID: 3112
		// (get) Token: 0x0600299C RID: 10652 RVA: 0x0003D6A0 File Offset: 0x0003B8A0
		// (set) Token: 0x0600299D RID: 10653 RVA: 0x0003D6A8 File Offset: 0x0003B8A8
		public Credentials? Credentials { get; set; }

		// Token: 0x17000C29 RID: 3113
		// (get) Token: 0x0600299E RID: 10654 RVA: 0x0003D6B1 File Offset: 0x0003B8B1
		// (set) Token: 0x0600299F RID: 10655 RVA: 0x0003D6B9 File Offset: 0x0003B8B9
		public AuthScopeFlags ScopeFlags { get; set; }

		// Token: 0x17000C2A RID: 3114
		// (get) Token: 0x060029A0 RID: 10656 RVA: 0x0003D6C2 File Offset: 0x0003B8C2
		// (set) Token: 0x060029A1 RID: 10657 RVA: 0x0003D6CA File Offset: 0x0003B8CA
		public LoginFlags LoginFlags { get; set; }
	}
}
