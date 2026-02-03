using System;

namespace Epic.OnlineServices.UserInfo
{
	// Token: 0x02000032 RID: 50
	public struct ExternalUserInfo
	{
		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060003AB RID: 939 RVA: 0x000053B6 File Offset: 0x000035B6
		// (set) Token: 0x060003AC RID: 940 RVA: 0x000053BE File Offset: 0x000035BE
		public ExternalAccountType AccountType { get; set; }

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060003AD RID: 941 RVA: 0x000053C7 File Offset: 0x000035C7
		// (set) Token: 0x060003AE RID: 942 RVA: 0x000053CF File Offset: 0x000035CF
		public Utf8String AccountId { get; set; }

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060003AF RID: 943 RVA: 0x000053D8 File Offset: 0x000035D8
		// (set) Token: 0x060003B0 RID: 944 RVA: 0x000053E0 File Offset: 0x000035E0
		public Utf8String DisplayName { get; set; }

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060003B1 RID: 945 RVA: 0x000053E9 File Offset: 0x000035E9
		// (set) Token: 0x060003B2 RID: 946 RVA: 0x000053F1 File Offset: 0x000035F1
		public Utf8String DisplayNameSanitized { get; set; }

		// Token: 0x060003B3 RID: 947 RVA: 0x000053FA File Offset: 0x000035FA
		internal void Set(ref ExternalUserInfoInternal other)
		{
			this.AccountType = other.AccountType;
			this.AccountId = other.AccountId;
			this.DisplayName = other.DisplayName;
			this.DisplayNameSanitized = other.DisplayNameSanitized;
		}
	}
}
