using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Auth
{
	// Token: 0x02000633 RID: 1587
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct CopyUserAuthTokenOptionsInternal : ISettable<CopyUserAuthTokenOptions>, IDisposable
	{
		// Token: 0x06002914 RID: 10516 RVA: 0x0003C8CD File Offset: 0x0003AACD
		public void Set(ref CopyUserAuthTokenOptions other)
		{
			this.m_ApiVersion = 1;
		}

		// Token: 0x06002915 RID: 10517 RVA: 0x0003C8D8 File Offset: 0x0003AAD8
		public void Set(ref CopyUserAuthTokenOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
			}
		}

		// Token: 0x06002916 RID: 10518 RVA: 0x0003C8F9 File Offset: 0x0003AAF9
		public void Dispose()
		{
		}

		// Token: 0x040011AC RID: 4524
		private int m_ApiVersion;
	}
}
