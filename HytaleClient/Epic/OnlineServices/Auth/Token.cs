using System;

namespace Epic.OnlineServices.Auth
{
	// Token: 0x02000663 RID: 1635
	public struct Token
	{
		// Token: 0x17000C55 RID: 3157
		// (get) Token: 0x06002A4D RID: 10829 RVA: 0x0003E15E File Offset: 0x0003C35E
		// (set) Token: 0x06002A4E RID: 10830 RVA: 0x0003E166 File Offset: 0x0003C366
		public Utf8String App { get; set; }

		// Token: 0x17000C56 RID: 3158
		// (get) Token: 0x06002A4F RID: 10831 RVA: 0x0003E16F File Offset: 0x0003C36F
		// (set) Token: 0x06002A50 RID: 10832 RVA: 0x0003E177 File Offset: 0x0003C377
		public Utf8String ClientId { get; set; }

		// Token: 0x17000C57 RID: 3159
		// (get) Token: 0x06002A51 RID: 10833 RVA: 0x0003E180 File Offset: 0x0003C380
		// (set) Token: 0x06002A52 RID: 10834 RVA: 0x0003E188 File Offset: 0x0003C388
		public EpicAccountId AccountId { get; set; }

		// Token: 0x17000C58 RID: 3160
		// (get) Token: 0x06002A53 RID: 10835 RVA: 0x0003E191 File Offset: 0x0003C391
		// (set) Token: 0x06002A54 RID: 10836 RVA: 0x0003E199 File Offset: 0x0003C399
		public Utf8String AccessToken { get; set; }

		// Token: 0x17000C59 RID: 3161
		// (get) Token: 0x06002A55 RID: 10837 RVA: 0x0003E1A2 File Offset: 0x0003C3A2
		// (set) Token: 0x06002A56 RID: 10838 RVA: 0x0003E1AA File Offset: 0x0003C3AA
		public double ExpiresIn { get; set; }

		// Token: 0x17000C5A RID: 3162
		// (get) Token: 0x06002A57 RID: 10839 RVA: 0x0003E1B3 File Offset: 0x0003C3B3
		// (set) Token: 0x06002A58 RID: 10840 RVA: 0x0003E1BB File Offset: 0x0003C3BB
		public Utf8String ExpiresAt { get; set; }

		// Token: 0x17000C5B RID: 3163
		// (get) Token: 0x06002A59 RID: 10841 RVA: 0x0003E1C4 File Offset: 0x0003C3C4
		// (set) Token: 0x06002A5A RID: 10842 RVA: 0x0003E1CC File Offset: 0x0003C3CC
		public AuthTokenType AuthType { get; set; }

		// Token: 0x17000C5C RID: 3164
		// (get) Token: 0x06002A5B RID: 10843 RVA: 0x0003E1D5 File Offset: 0x0003C3D5
		// (set) Token: 0x06002A5C RID: 10844 RVA: 0x0003E1DD File Offset: 0x0003C3DD
		public Utf8String RefreshToken { get; set; }

		// Token: 0x17000C5D RID: 3165
		// (get) Token: 0x06002A5D RID: 10845 RVA: 0x0003E1E6 File Offset: 0x0003C3E6
		// (set) Token: 0x06002A5E RID: 10846 RVA: 0x0003E1EE File Offset: 0x0003C3EE
		public double RefreshExpiresIn { get; set; }

		// Token: 0x17000C5E RID: 3166
		// (get) Token: 0x06002A5F RID: 10847 RVA: 0x0003E1F7 File Offset: 0x0003C3F7
		// (set) Token: 0x06002A60 RID: 10848 RVA: 0x0003E1FF File Offset: 0x0003C3FF
		public Utf8String RefreshExpiresAt { get; set; }

		// Token: 0x06002A61 RID: 10849 RVA: 0x0003E208 File Offset: 0x0003C408
		internal void Set(ref TokenInternal other)
		{
			this.App = other.App;
			this.ClientId = other.ClientId;
			this.AccountId = other.AccountId;
			this.AccessToken = other.AccessToken;
			this.ExpiresIn = other.ExpiresIn;
			this.ExpiresAt = other.ExpiresAt;
			this.AuthType = other.AuthType;
			this.RefreshToken = other.RefreshToken;
			this.RefreshExpiresIn = other.RefreshExpiresIn;
			this.RefreshExpiresAt = other.RefreshExpiresAt;
		}
	}
}
