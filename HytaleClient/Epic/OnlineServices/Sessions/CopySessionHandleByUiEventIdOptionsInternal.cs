using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x020000F7 RID: 247
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct CopySessionHandleByUiEventIdOptionsInternal : ISettable<CopySessionHandleByUiEventIdOptions>, IDisposable
	{
		// Token: 0x170001AC RID: 428
		// (set) Token: 0x06000847 RID: 2119 RVA: 0x0000C1AE File Offset: 0x0000A3AE
		public ulong UiEventId
		{
			set
			{
				this.m_UiEventId = value;
			}
		}

		// Token: 0x06000848 RID: 2120 RVA: 0x0000C1B8 File Offset: 0x0000A3B8
		public void Set(ref CopySessionHandleByUiEventIdOptions other)
		{
			this.m_ApiVersion = 1;
			this.UiEventId = other.UiEventId;
		}

		// Token: 0x06000849 RID: 2121 RVA: 0x0000C1D0 File Offset: 0x0000A3D0
		public void Set(ref CopySessionHandleByUiEventIdOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.UiEventId = other.Value.UiEventId;
			}
		}

		// Token: 0x0600084A RID: 2122 RVA: 0x0000C206 File Offset: 0x0000A406
		public void Dispose()
		{
		}

		// Token: 0x040003F2 RID: 1010
		private int m_ApiVersion;

		// Token: 0x040003F3 RID: 1011
		private ulong m_UiEventId;
	}
}
