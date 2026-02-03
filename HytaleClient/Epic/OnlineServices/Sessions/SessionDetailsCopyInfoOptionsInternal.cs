using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x0200014F RID: 335
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct SessionDetailsCopyInfoOptionsInternal : ISettable<SessionDetailsCopyInfoOptions>, IDisposable
	{
		// Token: 0x06000A1D RID: 2589 RVA: 0x0000E254 File Offset: 0x0000C454
		public void Set(ref SessionDetailsCopyInfoOptions other)
		{
			this.m_ApiVersion = 1;
		}

		// Token: 0x06000A1E RID: 2590 RVA: 0x0000E260 File Offset: 0x0000C460
		public void Set(ref SessionDetailsCopyInfoOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
			}
		}

		// Token: 0x06000A1F RID: 2591 RVA: 0x0000E281 File Offset: 0x0000C481
		public void Dispose()
		{
		}

		// Token: 0x04000498 RID: 1176
		private int m_ApiVersion;
	}
}
