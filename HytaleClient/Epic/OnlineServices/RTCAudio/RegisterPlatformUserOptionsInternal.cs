using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio
{
	// Token: 0x0200025D RID: 605
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct RegisterPlatformUserOptionsInternal : ISettable<RegisterPlatformUserOptions>, IDisposable
	{
		// Token: 0x17000443 RID: 1091
		// (set) Token: 0x060010CB RID: 4299 RVA: 0x00017ED6 File Offset: 0x000160D6
		public Utf8String PlatformUserId
		{
			set
			{
				Helper.Set(value, ref this.m_PlatformUserId);
			}
		}

		// Token: 0x060010CC RID: 4300 RVA: 0x00017EE6 File Offset: 0x000160E6
		public void Set(ref RegisterPlatformUserOptions other)
		{
			this.m_ApiVersion = 1;
			this.PlatformUserId = other.PlatformUserId;
		}

		// Token: 0x060010CD RID: 4301 RVA: 0x00017F00 File Offset: 0x00016100
		public void Set(ref RegisterPlatformUserOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.PlatformUserId = other.Value.PlatformUserId;
			}
		}

		// Token: 0x060010CE RID: 4302 RVA: 0x00017F36 File Offset: 0x00016136
		public void Dispose()
		{
			Helper.Dispose(ref this.m_PlatformUserId);
		}

		// Token: 0x0400073D RID: 1853
		private int m_ApiVersion;

		// Token: 0x0400073E RID: 1854
		private IntPtr m_PlatformUserId;
	}
}
