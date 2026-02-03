using System;

namespace Epic.OnlineServices.UserInfo
{
	// Token: 0x02000030 RID: 48
	public struct CopyUserInfoOptions
	{
		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060003A2 RID: 930 RVA: 0x000052E7 File Offset: 0x000034E7
		// (set) Token: 0x060003A3 RID: 931 RVA: 0x000052EF File Offset: 0x000034EF
		public EpicAccountId LocalUserId { get; set; }

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060003A4 RID: 932 RVA: 0x000052F8 File Offset: 0x000034F8
		// (set) Token: 0x060003A5 RID: 933 RVA: 0x00005300 File Offset: 0x00003500
		public EpicAccountId TargetUserId { get; set; }
	}
}
