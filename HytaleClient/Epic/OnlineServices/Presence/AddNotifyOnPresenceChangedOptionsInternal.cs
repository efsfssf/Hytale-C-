using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Presence
{
	// Token: 0x020002BB RID: 699
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct AddNotifyOnPresenceChangedOptionsInternal : ISettable<AddNotifyOnPresenceChangedOptions>, IDisposable
	{
		// Token: 0x06001354 RID: 4948 RVA: 0x0001C1FC File Offset: 0x0001A3FC
		public void Set(ref AddNotifyOnPresenceChangedOptions other)
		{
			this.m_ApiVersion = 1;
		}

		// Token: 0x06001355 RID: 4949 RVA: 0x0001C208 File Offset: 0x0001A408
		public void Set(ref AddNotifyOnPresenceChangedOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
			}
		}

		// Token: 0x06001356 RID: 4950 RVA: 0x0001C229 File Offset: 0x0001A429
		public void Dispose()
		{
		}

		// Token: 0x04000878 RID: 2168
		private int m_ApiVersion;
	}
}
