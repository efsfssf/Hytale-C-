using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Auth
{
	// Token: 0x02000668 RID: 1640
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct VerifyIdTokenOptionsInternal : ISettable<VerifyIdTokenOptions>, IDisposable
	{
		// Token: 0x17000C85 RID: 3205
		// (set) Token: 0x06002AB7 RID: 10935 RVA: 0x0003ED59 File Offset: 0x0003CF59
		public IdToken? IdToken
		{
			set
			{
				Helper.Set<IdToken, IdTokenInternal>(ref value, ref this.m_IdToken);
			}
		}

		// Token: 0x06002AB8 RID: 10936 RVA: 0x0003ED6A File Offset: 0x0003CF6A
		public void Set(ref VerifyIdTokenOptions other)
		{
			this.m_ApiVersion = 1;
			this.IdToken = other.IdToken;
		}

		// Token: 0x06002AB9 RID: 10937 RVA: 0x0003ED84 File Offset: 0x0003CF84
		public void Set(ref VerifyIdTokenOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.IdToken = other.Value.IdToken;
			}
		}

		// Token: 0x06002ABA RID: 10938 RVA: 0x0003EDBA File Offset: 0x0003CFBA
		public void Dispose()
		{
			Helper.Dispose(ref this.m_IdToken);
		}

		// Token: 0x04001250 RID: 4688
		private int m_ApiVersion;

		// Token: 0x04001251 RID: 4689
		private IntPtr m_IdToken;
	}
}
