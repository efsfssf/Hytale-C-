using System;

namespace Epic.OnlineServices.UserInfo
{
	// Token: 0x02000044 RID: 68
	public struct QueryUserInfoByExternalAccountOptions
	{
		// Token: 0x17000064 RID: 100
		// (get) Token: 0x06000427 RID: 1063 RVA: 0x00005DE0 File Offset: 0x00003FE0
		// (set) Token: 0x06000428 RID: 1064 RVA: 0x00005DE8 File Offset: 0x00003FE8
		public EpicAccountId LocalUserId { get; set; }

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x06000429 RID: 1065 RVA: 0x00005DF1 File Offset: 0x00003FF1
		// (set) Token: 0x0600042A RID: 1066 RVA: 0x00005DF9 File Offset: 0x00003FF9
		public Utf8String ExternalAccountId { get; set; }

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x0600042B RID: 1067 RVA: 0x00005E02 File Offset: 0x00004002
		// (set) Token: 0x0600042C RID: 1068 RVA: 0x00005E0A File Offset: 0x0000400A
		public ExternalAccountType AccountType { get; set; }
	}
}
