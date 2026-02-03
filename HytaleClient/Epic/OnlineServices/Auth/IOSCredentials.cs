using System;

namespace Epic.OnlineServices.Auth
{
	// Token: 0x0200066F RID: 1647
	public struct IOSCredentials
	{
		// Token: 0x17000C8D RID: 3213
		// (get) Token: 0x06002AD8 RID: 10968 RVA: 0x0003EF9D File Offset: 0x0003D19D
		// (set) Token: 0x06002AD9 RID: 10969 RVA: 0x0003EFA5 File Offset: 0x0003D1A5
		public Utf8String Id { get; set; }

		// Token: 0x17000C8E RID: 3214
		// (get) Token: 0x06002ADA RID: 10970 RVA: 0x0003EFAE File Offset: 0x0003D1AE
		// (set) Token: 0x06002ADB RID: 10971 RVA: 0x0003EFB6 File Offset: 0x0003D1B6
		public Utf8String Token { get; set; }

		// Token: 0x17000C8F RID: 3215
		// (get) Token: 0x06002ADC RID: 10972 RVA: 0x0003EFBF File Offset: 0x0003D1BF
		// (set) Token: 0x06002ADD RID: 10973 RVA: 0x0003EFC7 File Offset: 0x0003D1C7
		public LoginCredentialType Type { get; set; }

		// Token: 0x17000C90 RID: 3216
		// (get) Token: 0x06002ADE RID: 10974 RVA: 0x0003EFD0 File Offset: 0x0003D1D0
		// (set) Token: 0x06002ADF RID: 10975 RVA: 0x0003EFD8 File Offset: 0x0003D1D8
		public IOSCredentialsSystemAuthCredentialsOptions? SystemAuthCredentialsOptions { get; set; }

		// Token: 0x17000C91 RID: 3217
		// (get) Token: 0x06002AE0 RID: 10976 RVA: 0x0003EFE1 File Offset: 0x0003D1E1
		// (set) Token: 0x06002AE1 RID: 10977 RVA: 0x0003EFE9 File Offset: 0x0003D1E9
		public ExternalCredentialType ExternalType { get; set; }

		// Token: 0x06002AE2 RID: 10978 RVA: 0x0003EFF4 File Offset: 0x0003D1F4
		internal void Set(ref IOSCredentialsInternal other)
		{
			this.Id = other.Id;
			this.Token = other.Token;
			this.Type = other.Type;
			this.SystemAuthCredentialsOptions = other.SystemAuthCredentialsOptions;
			this.ExternalType = other.ExternalType;
		}
	}
}
