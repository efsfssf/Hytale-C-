using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Auth
{
	// Token: 0x02000674 RID: 1652
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct IOSLoginOptionsInternal : ISettable<IOSLoginOptions>, IDisposable
	{
		// Token: 0x17000CA0 RID: 3232
		// (set) Token: 0x06002B07 RID: 11015 RVA: 0x0003F442 File Offset: 0x0003D642
		public IOSCredentials? Credentials
		{
			set
			{
				Helper.Set<IOSCredentials, IOSCredentialsInternal>(ref value, ref this.m_Credentials);
			}
		}

		// Token: 0x17000CA1 RID: 3233
		// (set) Token: 0x06002B08 RID: 11016 RVA: 0x0003F453 File Offset: 0x0003D653
		public AuthScopeFlags ScopeFlags
		{
			set
			{
				this.m_ScopeFlags = value;
			}
		}

		// Token: 0x17000CA2 RID: 3234
		// (set) Token: 0x06002B09 RID: 11017 RVA: 0x0003F45D File Offset: 0x0003D65D
		public LoginFlags LoginFlags
		{
			set
			{
				this.m_LoginFlags = value;
			}
		}

		// Token: 0x06002B0A RID: 11018 RVA: 0x0003F467 File Offset: 0x0003D667
		public void Set(ref IOSLoginOptions other)
		{
			this.m_ApiVersion = 3;
			this.Credentials = other.Credentials;
			this.ScopeFlags = other.ScopeFlags;
			this.LoginFlags = other.LoginFlags;
		}

		// Token: 0x06002B0B RID: 11019 RVA: 0x0003F498 File Offset: 0x0003D698
		public void Set(ref IOSLoginOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 3;
				this.Credentials = other.Value.Credentials;
				this.ScopeFlags = other.Value.ScopeFlags;
				this.LoginFlags = other.Value.LoginFlags;
			}
		}

		// Token: 0x06002B0C RID: 11020 RVA: 0x0003F4F8 File Offset: 0x0003D6F8
		public void Dispose()
		{
			Helper.Dispose(ref this.m_Credentials);
		}

		// Token: 0x0400126F RID: 4719
		private int m_ApiVersion;

		// Token: 0x04001270 RID: 4720
		private IntPtr m_Credentials;

		// Token: 0x04001271 RID: 4721
		private AuthScopeFlags m_ScopeFlags;

		// Token: 0x04001272 RID: 4722
		private LoginFlags m_LoginFlags;
	}
}
