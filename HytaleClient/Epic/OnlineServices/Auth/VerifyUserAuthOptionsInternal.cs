using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Auth
{
	// Token: 0x0200066C RID: 1644
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct VerifyUserAuthOptionsInternal : ISettable<VerifyUserAuthOptions>, IDisposable
	{
		// Token: 0x17000C8C RID: 3212
		// (set) Token: 0x06002ACC RID: 10956 RVA: 0x0003EF2E File Offset: 0x0003D12E
		public Token? AuthToken
		{
			set
			{
				Helper.Set<Token, TokenInternal>(ref value, ref this.m_AuthToken);
			}
		}

		// Token: 0x06002ACD RID: 10957 RVA: 0x0003EF3F File Offset: 0x0003D13F
		public void Set(ref VerifyUserAuthOptions other)
		{
			this.m_ApiVersion = 1;
			this.AuthToken = other.AuthToken;
		}

		// Token: 0x06002ACE RID: 10958 RVA: 0x0003EF58 File Offset: 0x0003D158
		public void Set(ref VerifyUserAuthOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.AuthToken = other.Value.AuthToken;
			}
		}

		// Token: 0x06002ACF RID: 10959 RVA: 0x0003EF8E File Offset: 0x0003D18E
		public void Dispose()
		{
			Helper.Dispose(ref this.m_AuthToken);
		}

		// Token: 0x04001257 RID: 4695
		private int m_ApiVersion;

		// Token: 0x04001258 RID: 4696
		private IntPtr m_AuthToken;
	}
}
