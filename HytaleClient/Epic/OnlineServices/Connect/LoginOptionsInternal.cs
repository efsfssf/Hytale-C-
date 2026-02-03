using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Connect
{
	// Token: 0x020005F2 RID: 1522
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct LoginOptionsInternal : ISettable<LoginOptions>, IDisposable
	{
		// Token: 0x17000B8C RID: 2956
		// (set) Token: 0x06002788 RID: 10120 RVA: 0x0003A831 File Offset: 0x00038A31
		public Credentials? Credentials
		{
			set
			{
				Helper.Set<Credentials, CredentialsInternal>(ref value, ref this.m_Credentials);
			}
		}

		// Token: 0x17000B8D RID: 2957
		// (set) Token: 0x06002789 RID: 10121 RVA: 0x0003A842 File Offset: 0x00038A42
		public UserLoginInfo? UserLoginInfo
		{
			set
			{
				Helper.Set<UserLoginInfo, UserLoginInfoInternal>(ref value, ref this.m_UserLoginInfo);
			}
		}

		// Token: 0x0600278A RID: 10122 RVA: 0x0003A853 File Offset: 0x00038A53
		public void Set(ref LoginOptions other)
		{
			this.m_ApiVersion = 2;
			this.Credentials = other.Credentials;
			this.UserLoginInfo = other.UserLoginInfo;
		}

		// Token: 0x0600278B RID: 10123 RVA: 0x0003A878 File Offset: 0x00038A78
		public void Set(ref LoginOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 2;
				this.Credentials = other.Value.Credentials;
				this.UserLoginInfo = other.Value.UserLoginInfo;
			}
		}

		// Token: 0x0600278C RID: 10124 RVA: 0x0003A8C3 File Offset: 0x00038AC3
		public void Dispose()
		{
			Helper.Dispose(ref this.m_Credentials);
			Helper.Dispose(ref this.m_UserLoginInfo);
		}

		// Token: 0x04001124 RID: 4388
		private int m_ApiVersion;

		// Token: 0x04001125 RID: 4389
		private IntPtr m_Credentials;

		// Token: 0x04001126 RID: 4390
		private IntPtr m_UserLoginInfo;
	}
}
