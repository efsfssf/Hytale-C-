using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x0200037C RID: 892
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct CopyLobbyDetailsHandleByUiEventIdOptionsInternal : ISettable<CopyLobbyDetailsHandleByUiEventIdOptions>, IDisposable
	{
		// Token: 0x1700068B RID: 1675
		// (set) Token: 0x060017E1 RID: 6113 RVA: 0x00022F3A File Offset: 0x0002113A
		public ulong UiEventId
		{
			set
			{
				this.m_UiEventId = value;
			}
		}

		// Token: 0x060017E2 RID: 6114 RVA: 0x00022F44 File Offset: 0x00021144
		public void Set(ref CopyLobbyDetailsHandleByUiEventIdOptions other)
		{
			this.m_ApiVersion = 1;
			this.UiEventId = other.UiEventId;
		}

		// Token: 0x060017E3 RID: 6115 RVA: 0x00022F5C File Offset: 0x0002115C
		public void Set(ref CopyLobbyDetailsHandleByUiEventIdOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.UiEventId = other.Value.UiEventId;
			}
		}

		// Token: 0x060017E4 RID: 6116 RVA: 0x00022F92 File Offset: 0x00021192
		public void Dispose()
		{
		}

		// Token: 0x04000A88 RID: 2696
		private int m_ApiVersion;

		// Token: 0x04000A89 RID: 2697
		private ulong m_UiEventId;
	}
}
