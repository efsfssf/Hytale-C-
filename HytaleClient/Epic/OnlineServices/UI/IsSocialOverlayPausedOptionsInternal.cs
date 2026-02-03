using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UI
{
	// Token: 0x02000061 RID: 97
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct IsSocialOverlayPausedOptionsInternal : ISettable<IsSocialOverlayPausedOptions>, IDisposable
	{
		// Token: 0x060004BA RID: 1210 RVA: 0x00006E7D File Offset: 0x0000507D
		public void Set(ref IsSocialOverlayPausedOptions other)
		{
			this.m_ApiVersion = 1;
		}

		// Token: 0x060004BB RID: 1211 RVA: 0x00006E88 File Offset: 0x00005088
		public void Set(ref IsSocialOverlayPausedOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
			}
		}

		// Token: 0x060004BC RID: 1212 RVA: 0x00006EA9 File Offset: 0x000050A9
		public void Dispose()
		{
		}

		// Token: 0x04000201 RID: 513
		private int m_ApiVersion;
	}
}
