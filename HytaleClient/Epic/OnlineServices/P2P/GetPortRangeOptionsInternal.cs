using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.P2P
{
	// Token: 0x02000784 RID: 1924
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct GetPortRangeOptionsInternal : ISettable<GetPortRangeOptions>, IDisposable
	{
		// Token: 0x060031D6 RID: 12758 RVA: 0x0004A968 File Offset: 0x00048B68
		public void Set(ref GetPortRangeOptions other)
		{
			this.m_ApiVersion = 1;
		}

		// Token: 0x060031D7 RID: 12759 RVA: 0x0004A974 File Offset: 0x00048B74
		public void Set(ref GetPortRangeOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
			}
		}

		// Token: 0x060031D8 RID: 12760 RVA: 0x0004A995 File Offset: 0x00048B95
		public void Dispose()
		{
		}

		// Token: 0x0400166D RID: 5741
		private int m_ApiVersion;
	}
}
