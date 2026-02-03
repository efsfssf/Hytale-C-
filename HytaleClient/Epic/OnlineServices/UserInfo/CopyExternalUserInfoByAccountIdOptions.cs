using System;

namespace Epic.OnlineServices.UserInfo
{
	// Token: 0x0200002A RID: 42
	public struct CopyExternalUserInfoByAccountIdOptions
	{
		// Token: 0x17000026 RID: 38
		// (get) Token: 0x0600037E RID: 894 RVA: 0x00004FB3 File Offset: 0x000031B3
		// (set) Token: 0x0600037F RID: 895 RVA: 0x00004FBB File Offset: 0x000031BB
		public EpicAccountId LocalUserId { get; set; }

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000380 RID: 896 RVA: 0x00004FC4 File Offset: 0x000031C4
		// (set) Token: 0x06000381 RID: 897 RVA: 0x00004FCC File Offset: 0x000031CC
		public EpicAccountId TargetUserId { get; set; }

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000382 RID: 898 RVA: 0x00004FD5 File Offset: 0x000031D5
		// (set) Token: 0x06000383 RID: 899 RVA: 0x00004FDD File Offset: 0x000031DD
		public Utf8String AccountId { get; set; }
	}
}
