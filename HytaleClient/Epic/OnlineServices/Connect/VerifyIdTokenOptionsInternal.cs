using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Connect
{
	// Token: 0x02000628 RID: 1576
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct VerifyIdTokenOptionsInternal : ISettable<VerifyIdTokenOptions>, IDisposable
	{
		// Token: 0x17000BEE RID: 3054
		// (set) Token: 0x060028E1 RID: 10465 RVA: 0x0003C001 File Offset: 0x0003A201
		public IdToken? IdToken
		{
			set
			{
				Helper.Set<IdToken, IdTokenInternal>(ref value, ref this.m_IdToken);
			}
		}

		// Token: 0x060028E2 RID: 10466 RVA: 0x0003C012 File Offset: 0x0003A212
		public void Set(ref VerifyIdTokenOptions other)
		{
			this.m_ApiVersion = 1;
			this.IdToken = other.IdToken;
		}

		// Token: 0x060028E3 RID: 10467 RVA: 0x0003C02C File Offset: 0x0003A22C
		public void Set(ref VerifyIdTokenOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.IdToken = other.Value.IdToken;
			}
		}

		// Token: 0x060028E4 RID: 10468 RVA: 0x0003C062 File Offset: 0x0003A262
		public void Dispose()
		{
			Helper.Dispose(ref this.m_IdToken);
		}

		// Token: 0x04001188 RID: 4488
		private int m_ApiVersion;

		// Token: 0x04001189 RID: 4489
		private IntPtr m_IdToken;
	}
}
