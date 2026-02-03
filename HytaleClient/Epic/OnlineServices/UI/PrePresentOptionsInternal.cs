using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UI
{
	// Token: 0x0200007D RID: 125
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct PrePresentOptionsInternal : ISettable<PrePresentOptions>, IDisposable
	{
		// Token: 0x170000B5 RID: 181
		// (set) Token: 0x0600054D RID: 1357 RVA: 0x000076F2 File Offset: 0x000058F2
		public IntPtr PlatformSpecificData
		{
			set
			{
				this.m_PlatformSpecificData = value;
			}
		}

		// Token: 0x0600054E RID: 1358 RVA: 0x000076FC File Offset: 0x000058FC
		public void Set(ref PrePresentOptions other)
		{
			this.m_ApiVersion = 1;
			this.PlatformSpecificData = other.PlatformSpecificData;
		}

		// Token: 0x0600054F RID: 1359 RVA: 0x00007714 File Offset: 0x00005914
		public void Set(ref PrePresentOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.PlatformSpecificData = other.Value.PlatformSpecificData;
			}
		}

		// Token: 0x06000550 RID: 1360 RVA: 0x0000774A File Offset: 0x0000594A
		public void Dispose()
		{
			Helper.Dispose(ref this.m_PlatformSpecificData);
		}

		// Token: 0x04000295 RID: 661
		private int m_ApiVersion;

		// Token: 0x04000296 RID: 662
		private IntPtr m_PlatformSpecificData;
	}
}
