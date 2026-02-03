using System;

namespace Epic.OnlineServices.UserInfo
{
	// Token: 0x0200004A RID: 74
	public struct UserInfoData
	{
		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000453 RID: 1107 RVA: 0x00006206 File Offset: 0x00004406
		// (set) Token: 0x06000454 RID: 1108 RVA: 0x0000620E File Offset: 0x0000440E
		public EpicAccountId UserId { get; set; }

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000455 RID: 1109 RVA: 0x00006217 File Offset: 0x00004417
		// (set) Token: 0x06000456 RID: 1110 RVA: 0x0000621F File Offset: 0x0000441F
		public Utf8String Country { get; set; }

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x06000457 RID: 1111 RVA: 0x00006228 File Offset: 0x00004428
		// (set) Token: 0x06000458 RID: 1112 RVA: 0x00006230 File Offset: 0x00004430
		public Utf8String DisplayName { get; set; }

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x06000459 RID: 1113 RVA: 0x00006239 File Offset: 0x00004439
		// (set) Token: 0x0600045A RID: 1114 RVA: 0x00006241 File Offset: 0x00004441
		public Utf8String PreferredLanguage { get; set; }

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x0600045B RID: 1115 RVA: 0x0000624A File Offset: 0x0000444A
		// (set) Token: 0x0600045C RID: 1116 RVA: 0x00006252 File Offset: 0x00004452
		public Utf8String Nickname { get; set; }

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x0600045D RID: 1117 RVA: 0x0000625B File Offset: 0x0000445B
		// (set) Token: 0x0600045E RID: 1118 RVA: 0x00006263 File Offset: 0x00004463
		public Utf8String DisplayNameSanitized { get; set; }

		// Token: 0x0600045F RID: 1119 RVA: 0x0000626C File Offset: 0x0000446C
		internal void Set(ref UserInfoDataInternal other)
		{
			this.UserId = other.UserId;
			this.Country = other.Country;
			this.DisplayName = other.DisplayName;
			this.PreferredLanguage = other.PreferredLanguage;
			this.Nickname = other.Nickname;
			this.DisplayNameSanitized = other.DisplayNameSanitized;
		}
	}
}
