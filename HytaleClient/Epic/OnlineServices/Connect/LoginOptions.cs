using System;

namespace Epic.OnlineServices.Connect
{
	// Token: 0x020005F1 RID: 1521
	public struct LoginOptions
	{
		// Token: 0x17000B8A RID: 2954
		// (get) Token: 0x06002784 RID: 10116 RVA: 0x0003A80F File Offset: 0x00038A0F
		// (set) Token: 0x06002785 RID: 10117 RVA: 0x0003A817 File Offset: 0x00038A17
		public Credentials? Credentials { get; set; }

		// Token: 0x17000B8B RID: 2955
		// (get) Token: 0x06002786 RID: 10118 RVA: 0x0003A820 File Offset: 0x00038A20
		// (set) Token: 0x06002787 RID: 10119 RVA: 0x0003A828 File Offset: 0x00038A28
		public UserLoginInfo? UserLoginInfo { get; set; }
	}
}
