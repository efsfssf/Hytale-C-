using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Auth
{
	// Token: 0x0200064C RID: 1612
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct LogoutOptionsInternal : ISettable<LogoutOptions>, IDisposable
	{
		// Token: 0x17000C3F RID: 3135
		// (set) Token: 0x060029D4 RID: 10708 RVA: 0x0003DBAF File Offset: 0x0003BDAF
		public EpicAccountId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x060029D5 RID: 10709 RVA: 0x0003DBBF File Offset: 0x0003BDBF
		public void Set(ref LogoutOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
		}

		// Token: 0x060029D6 RID: 10710 RVA: 0x0003DBD8 File Offset: 0x0003BDD8
		public void Set(ref LogoutOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
			}
		}

		// Token: 0x060029D7 RID: 10711 RVA: 0x0003DC0E File Offset: 0x0003BE0E
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
		}

		// Token: 0x04001208 RID: 4616
		private int m_ApiVersion;

		// Token: 0x04001209 RID: 4617
		private IntPtr m_LocalUserId;
	}
}
