using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Auth
{
	// Token: 0x02000635 RID: 1589
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct CredentialsInternal : IGettable<Credentials>, ISettable<Credentials>, IDisposable
	{
		// Token: 0x17000BF8 RID: 3064
		// (get) Token: 0x06002922 RID: 10530 RVA: 0x0003C9A4 File Offset: 0x0003ABA4
		// (set) Token: 0x06002923 RID: 10531 RVA: 0x0003C9C5 File Offset: 0x0003ABC5
		public Utf8String Id
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_Id, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_Id);
			}
		}

		// Token: 0x17000BF9 RID: 3065
		// (get) Token: 0x06002924 RID: 10532 RVA: 0x0003C9D8 File Offset: 0x0003ABD8
		// (set) Token: 0x06002925 RID: 10533 RVA: 0x0003C9F9 File Offset: 0x0003ABF9
		public Utf8String Token
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_Token, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_Token);
			}
		}

		// Token: 0x17000BFA RID: 3066
		// (get) Token: 0x06002926 RID: 10534 RVA: 0x0003CA0C File Offset: 0x0003AC0C
		// (set) Token: 0x06002927 RID: 10535 RVA: 0x0003CA24 File Offset: 0x0003AC24
		public LoginCredentialType Type
		{
			get
			{
				return this.m_Type;
			}
			set
			{
				this.m_Type = value;
			}
		}

		// Token: 0x17000BFB RID: 3067
		// (get) Token: 0x06002928 RID: 10536 RVA: 0x0003CA30 File Offset: 0x0003AC30
		// (set) Token: 0x06002929 RID: 10537 RVA: 0x0003CA48 File Offset: 0x0003AC48
		public IntPtr SystemAuthCredentialsOptions
		{
			get
			{
				return this.m_SystemAuthCredentialsOptions;
			}
			set
			{
				this.m_SystemAuthCredentialsOptions = value;
			}
		}

		// Token: 0x17000BFC RID: 3068
		// (get) Token: 0x0600292A RID: 10538 RVA: 0x0003CA54 File Offset: 0x0003AC54
		// (set) Token: 0x0600292B RID: 10539 RVA: 0x0003CA6C File Offset: 0x0003AC6C
		public ExternalCredentialType ExternalType
		{
			get
			{
				return this.m_ExternalType;
			}
			set
			{
				this.m_ExternalType = value;
			}
		}

		// Token: 0x0600292C RID: 10540 RVA: 0x0003CA78 File Offset: 0x0003AC78
		public void Set(ref Credentials other)
		{
			this.m_ApiVersion = 4;
			this.Id = other.Id;
			this.Token = other.Token;
			this.Type = other.Type;
			this.SystemAuthCredentialsOptions = other.SystemAuthCredentialsOptions;
			this.ExternalType = other.ExternalType;
		}

		// Token: 0x0600292D RID: 10541 RVA: 0x0003CAD0 File Offset: 0x0003ACD0
		public void Set(ref Credentials? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 4;
				this.Id = other.Value.Id;
				this.Token = other.Value.Token;
				this.Type = other.Value.Type;
				this.SystemAuthCredentialsOptions = other.Value.SystemAuthCredentialsOptions;
				this.ExternalType = other.Value.ExternalType;
			}
		}

		// Token: 0x0600292E RID: 10542 RVA: 0x0003CB5A File Offset: 0x0003AD5A
		public void Dispose()
		{
			Helper.Dispose(ref this.m_Id);
			Helper.Dispose(ref this.m_Token);
			Helper.Dispose(ref this.m_SystemAuthCredentialsOptions);
		}

		// Token: 0x0600292F RID: 10543 RVA: 0x0003CB81 File Offset: 0x0003AD81
		public void Get(out Credentials output)
		{
			output = default(Credentials);
			output.Set(ref this);
		}

		// Token: 0x040011B2 RID: 4530
		private int m_ApiVersion;

		// Token: 0x040011B3 RID: 4531
		private IntPtr m_Id;

		// Token: 0x040011B4 RID: 4532
		private IntPtr m_Token;

		// Token: 0x040011B5 RID: 4533
		private LoginCredentialType m_Type;

		// Token: 0x040011B6 RID: 4534
		private IntPtr m_SystemAuthCredentialsOptions;

		// Token: 0x040011B7 RID: 4535
		private ExternalCredentialType m_ExternalType;
	}
}
