using System;

namespace Epic.OnlineServices.Auth
{
	// Token: 0x02000634 RID: 1588
	public struct Credentials
	{
		// Token: 0x17000BF3 RID: 3059
		// (get) Token: 0x06002917 RID: 10519 RVA: 0x0003C8FC File Offset: 0x0003AAFC
		// (set) Token: 0x06002918 RID: 10520 RVA: 0x0003C904 File Offset: 0x0003AB04
		public Utf8String Id { get; set; }

		// Token: 0x17000BF4 RID: 3060
		// (get) Token: 0x06002919 RID: 10521 RVA: 0x0003C90D File Offset: 0x0003AB0D
		// (set) Token: 0x0600291A RID: 10522 RVA: 0x0003C915 File Offset: 0x0003AB15
		public Utf8String Token { get; set; }

		// Token: 0x17000BF5 RID: 3061
		// (get) Token: 0x0600291B RID: 10523 RVA: 0x0003C91E File Offset: 0x0003AB1E
		// (set) Token: 0x0600291C RID: 10524 RVA: 0x0003C926 File Offset: 0x0003AB26
		public LoginCredentialType Type { get; set; }

		// Token: 0x17000BF6 RID: 3062
		// (get) Token: 0x0600291D RID: 10525 RVA: 0x0003C92F File Offset: 0x0003AB2F
		// (set) Token: 0x0600291E RID: 10526 RVA: 0x0003C937 File Offset: 0x0003AB37
		public IntPtr SystemAuthCredentialsOptions { get; set; }

		// Token: 0x17000BF7 RID: 3063
		// (get) Token: 0x0600291F RID: 10527 RVA: 0x0003C940 File Offset: 0x0003AB40
		// (set) Token: 0x06002920 RID: 10528 RVA: 0x0003C948 File Offset: 0x0003AB48
		public ExternalCredentialType ExternalType { get; set; }

		// Token: 0x06002921 RID: 10529 RVA: 0x0003C954 File Offset: 0x0003AB54
		internal void Set(ref CredentialsInternal other)
		{
			this.Id = other.Id;
			this.Token = other.Token;
			this.Type = other.Type;
			this.SystemAuthCredentialsOptions = other.SystemAuthCredentialsOptions;
			this.ExternalType = other.ExternalType;
		}
	}
}
