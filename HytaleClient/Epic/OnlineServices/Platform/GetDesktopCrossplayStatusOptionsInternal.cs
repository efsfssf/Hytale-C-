using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Platform
{
	// Token: 0x0200070F RID: 1807
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct GetDesktopCrossplayStatusOptionsInternal : ISettable<GetDesktopCrossplayStatusOptions>, IDisposable
	{
		// Token: 0x06002E9B RID: 11931 RVA: 0x00045039 File Offset: 0x00043239
		public void Set(ref GetDesktopCrossplayStatusOptions other)
		{
			this.m_ApiVersion = 1;
		}

		// Token: 0x06002E9C RID: 11932 RVA: 0x00045044 File Offset: 0x00043244
		public void Set(ref GetDesktopCrossplayStatusOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
			}
		}

		// Token: 0x06002E9D RID: 11933 RVA: 0x00045065 File Offset: 0x00043265
		public void Dispose()
		{
		}

		// Token: 0x040014A3 RID: 5283
		private int m_ApiVersion;
	}
}
