using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio
{
	// Token: 0x0200026F RID: 623
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct UnregisterPlatformUserOptionsInternal : ISettable<UnregisterPlatformUserOptions>, IDisposable
	{
		// Token: 0x17000465 RID: 1125
		// (set) Token: 0x0600114A RID: 4426 RVA: 0x0001924E File Offset: 0x0001744E
		public Utf8String PlatformUserId
		{
			set
			{
				Helper.Set(value, ref this.m_PlatformUserId);
			}
		}

		// Token: 0x0600114B RID: 4427 RVA: 0x0001925E File Offset: 0x0001745E
		public void Set(ref UnregisterPlatformUserOptions other)
		{
			this.m_ApiVersion = 1;
			this.PlatformUserId = other.PlatformUserId;
		}

		// Token: 0x0600114C RID: 4428 RVA: 0x00019278 File Offset: 0x00017478
		public void Set(ref UnregisterPlatformUserOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.PlatformUserId = other.Value.PlatformUserId;
			}
		}

		// Token: 0x0600114D RID: 4429 RVA: 0x000192AE File Offset: 0x000174AE
		public void Dispose()
		{
			Helper.Dispose(ref this.m_PlatformUserId);
		}

		// Token: 0x04000799 RID: 1945
		private int m_ApiVersion;

		// Token: 0x0400079A RID: 1946
		private IntPtr m_PlatformUserId;
	}
}
