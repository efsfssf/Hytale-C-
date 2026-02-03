using System;

namespace Epic.OnlineServices.UserInfo
{
	// Token: 0x02000048 RID: 72
	public struct QueryUserInfoOptions
	{
		// Token: 0x17000073 RID: 115
		// (get) Token: 0x0600044A RID: 1098 RVA: 0x00006137 File Offset: 0x00004337
		// (set) Token: 0x0600044B RID: 1099 RVA: 0x0000613F File Offset: 0x0000433F
		public EpicAccountId LocalUserId { get; set; }

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x0600044C RID: 1100 RVA: 0x00006148 File Offset: 0x00004348
		// (set) Token: 0x0600044D RID: 1101 RVA: 0x00006150 File Offset: 0x00004350
		public EpicAccountId TargetUserId { get; set; }
	}
}
