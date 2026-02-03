using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x020000DB RID: 219
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct ActiveSessionCopyInfoOptionsInternal : ISettable<ActiveSessionCopyInfoOptions>, IDisposable
	{
		// Token: 0x060007DD RID: 2013 RVA: 0x0000B721 File Offset: 0x00009921
		public void Set(ref ActiveSessionCopyInfoOptions other)
		{
			this.m_ApiVersion = 1;
		}

		// Token: 0x060007DE RID: 2014 RVA: 0x0000B72C File Offset: 0x0000992C
		public void Set(ref ActiveSessionCopyInfoOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
			}
		}

		// Token: 0x060007DF RID: 2015 RVA: 0x0000B74D File Offset: 0x0000994D
		public void Dispose()
		{
		}

		// Token: 0x040003C8 RID: 968
		private int m_ApiVersion;
	}
}
