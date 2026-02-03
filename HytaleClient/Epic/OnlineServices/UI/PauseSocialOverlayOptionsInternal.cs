using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UI
{
	// Token: 0x0200007B RID: 123
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct PauseSocialOverlayOptionsInternal : ISettable<PauseSocialOverlayOptions>, IDisposable
	{
		// Token: 0x170000B3 RID: 179
		// (set) Token: 0x06000547 RID: 1351 RVA: 0x00007680 File Offset: 0x00005880
		public bool IsPaused
		{
			set
			{
				Helper.Set(value, ref this.m_IsPaused);
			}
		}

		// Token: 0x06000548 RID: 1352 RVA: 0x00007690 File Offset: 0x00005890
		public void Set(ref PauseSocialOverlayOptions other)
		{
			this.m_ApiVersion = 1;
			this.IsPaused = other.IsPaused;
		}

		// Token: 0x06000549 RID: 1353 RVA: 0x000076A8 File Offset: 0x000058A8
		public void Set(ref PauseSocialOverlayOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.IsPaused = other.Value.IsPaused;
			}
		}

		// Token: 0x0600054A RID: 1354 RVA: 0x000076DE File Offset: 0x000058DE
		public void Dispose()
		{
		}

		// Token: 0x04000292 RID: 658
		private int m_ApiVersion;

		// Token: 0x04000293 RID: 659
		private int m_IsPaused;
	}
}
