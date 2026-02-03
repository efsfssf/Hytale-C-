using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Auth
{
	// Token: 0x02000646 RID: 1606
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct LoginOptionsInternal : ISettable<LoginOptions>, IDisposable
	{
		// Token: 0x17000C2B RID: 3115
		// (set) Token: 0x060029A2 RID: 10658 RVA: 0x0003D6D3 File Offset: 0x0003B8D3
		public Credentials? Credentials
		{
			set
			{
				Helper.Set<Credentials, CredentialsInternal>(ref value, ref this.m_Credentials);
			}
		}

		// Token: 0x17000C2C RID: 3116
		// (set) Token: 0x060029A3 RID: 10659 RVA: 0x0003D6E4 File Offset: 0x0003B8E4
		public AuthScopeFlags ScopeFlags
		{
			set
			{
				this.m_ScopeFlags = value;
			}
		}

		// Token: 0x17000C2D RID: 3117
		// (set) Token: 0x060029A4 RID: 10660 RVA: 0x0003D6EE File Offset: 0x0003B8EE
		public LoginFlags LoginFlags
		{
			set
			{
				this.m_LoginFlags = value;
			}
		}

		// Token: 0x060029A5 RID: 10661 RVA: 0x0003D6F8 File Offset: 0x0003B8F8
		public void Set(ref LoginOptions other)
		{
			this.m_ApiVersion = 3;
			this.Credentials = other.Credentials;
			this.ScopeFlags = other.ScopeFlags;
			this.LoginFlags = other.LoginFlags;
		}

		// Token: 0x060029A6 RID: 10662 RVA: 0x0003D72C File Offset: 0x0003B92C
		public void Set(ref LoginOptions? other)
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

		// Token: 0x060029A7 RID: 10663 RVA: 0x0003D78C File Offset: 0x0003B98C
		public void Dispose()
		{
			Helper.Dispose(ref this.m_Credentials);
		}

		// Token: 0x040011F5 RID: 4597
		private int m_ApiVersion;

		// Token: 0x040011F6 RID: 4598
		private IntPtr m_Credentials;

		// Token: 0x040011F7 RID: 4599
		private AuthScopeFlags m_ScopeFlags;

		// Token: 0x040011F8 RID: 4600
		private LoginFlags m_LoginFlags;
	}
}
