using System;

namespace Epic.OnlineServices.UserInfo
{
	// Token: 0x0200002C RID: 44
	public struct CopyExternalUserInfoByAccountTypeOptions
	{
		// Token: 0x1700002C RID: 44
		// (get) Token: 0x0600038A RID: 906 RVA: 0x000050CF File Offset: 0x000032CF
		// (set) Token: 0x0600038B RID: 907 RVA: 0x000050D7 File Offset: 0x000032D7
		public EpicAccountId LocalUserId { get; set; }

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x0600038C RID: 908 RVA: 0x000050E0 File Offset: 0x000032E0
		// (set) Token: 0x0600038D RID: 909 RVA: 0x000050E8 File Offset: 0x000032E8
		public EpicAccountId TargetUserId { get; set; }

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x0600038E RID: 910 RVA: 0x000050F1 File Offset: 0x000032F1
		// (set) Token: 0x0600038F RID: 911 RVA: 0x000050F9 File Offset: 0x000032F9
		public ExternalAccountType AccountType { get; set; }
	}
}
