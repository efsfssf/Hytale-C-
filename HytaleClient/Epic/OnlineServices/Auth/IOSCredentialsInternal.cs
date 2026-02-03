using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Auth
{
	// Token: 0x02000670 RID: 1648
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct IOSCredentialsInternal : IGettable<IOSCredentials>, ISettable<IOSCredentials>, IDisposable
	{
		// Token: 0x17000C92 RID: 3218
		// (get) Token: 0x06002AE3 RID: 10979 RVA: 0x0003F044 File Offset: 0x0003D244
		// (set) Token: 0x06002AE4 RID: 10980 RVA: 0x0003F065 File Offset: 0x0003D265
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

		// Token: 0x17000C93 RID: 3219
		// (get) Token: 0x06002AE5 RID: 10981 RVA: 0x0003F078 File Offset: 0x0003D278
		// (set) Token: 0x06002AE6 RID: 10982 RVA: 0x0003F099 File Offset: 0x0003D299
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

		// Token: 0x17000C94 RID: 3220
		// (get) Token: 0x06002AE7 RID: 10983 RVA: 0x0003F0AC File Offset: 0x0003D2AC
		// (set) Token: 0x06002AE8 RID: 10984 RVA: 0x0003F0C4 File Offset: 0x0003D2C4
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

		// Token: 0x17000C95 RID: 3221
		// (get) Token: 0x06002AE9 RID: 10985 RVA: 0x0003F0D0 File Offset: 0x0003D2D0
		// (set) Token: 0x06002AEA RID: 10986 RVA: 0x0003F0F1 File Offset: 0x0003D2F1
		public IOSCredentialsSystemAuthCredentialsOptions? SystemAuthCredentialsOptions
		{
			get
			{
				IOSCredentialsSystemAuthCredentialsOptions? result;
				Helper.Get<IOSCredentialsSystemAuthCredentialsOptionsInternal, IOSCredentialsSystemAuthCredentialsOptions>(this.m_SystemAuthCredentialsOptions, out result);
				return result;
			}
			set
			{
				Helper.Set<IOSCredentialsSystemAuthCredentialsOptions, IOSCredentialsSystemAuthCredentialsOptionsInternal>(ref value, ref this.m_SystemAuthCredentialsOptions);
			}
		}

		// Token: 0x17000C96 RID: 3222
		// (get) Token: 0x06002AEB RID: 10987 RVA: 0x0003F104 File Offset: 0x0003D304
		// (set) Token: 0x06002AEC RID: 10988 RVA: 0x0003F11C File Offset: 0x0003D31C
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

		// Token: 0x06002AED RID: 10989 RVA: 0x0003F128 File Offset: 0x0003D328
		public void Set(ref IOSCredentials other)
		{
			this.m_ApiVersion = 4;
			this.Id = other.Id;
			this.Token = other.Token;
			this.Type = other.Type;
			this.SystemAuthCredentialsOptions = other.SystemAuthCredentialsOptions;
			this.ExternalType = other.ExternalType;
		}

		// Token: 0x06002AEE RID: 10990 RVA: 0x0003F180 File Offset: 0x0003D380
		public void Set(ref IOSCredentials? other)
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

		// Token: 0x06002AEF RID: 10991 RVA: 0x0003F20A File Offset: 0x0003D40A
		public void Dispose()
		{
			Helper.Dispose(ref this.m_Id);
			Helper.Dispose(ref this.m_Token);
			Helper.Dispose(ref this.m_SystemAuthCredentialsOptions);
		}

		// Token: 0x06002AF0 RID: 10992 RVA: 0x0003F231 File Offset: 0x0003D431
		public void Get(out IOSCredentials output)
		{
			output = default(IOSCredentials);
			output.Set(ref this);
		}

		// Token: 0x0400125E RID: 4702
		private int m_ApiVersion;

		// Token: 0x0400125F RID: 4703
		private IntPtr m_Id;

		// Token: 0x04001260 RID: 4704
		private IntPtr m_Token;

		// Token: 0x04001261 RID: 4705
		private LoginCredentialType m_Type;

		// Token: 0x04001262 RID: 4706
		private IntPtr m_SystemAuthCredentialsOptions;

		// Token: 0x04001263 RID: 4707
		private ExternalCredentialType m_ExternalType;
	}
}
