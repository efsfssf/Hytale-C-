using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x020003C2 RID: 962
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct LobbyDetailsGetAttributeCountOptionsInternal : ISettable<LobbyDetailsGetAttributeCountOptions>, IDisposable
	{
		// Token: 0x060019C4 RID: 6596 RVA: 0x00025DED File Offset: 0x00023FED
		public void Set(ref LobbyDetailsGetAttributeCountOptions other)
		{
			this.m_ApiVersion = 1;
		}

		// Token: 0x060019C5 RID: 6597 RVA: 0x00025DF8 File Offset: 0x00023FF8
		public void Set(ref LobbyDetailsGetAttributeCountOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
			}
		}

		// Token: 0x060019C6 RID: 6598 RVA: 0x00025E19 File Offset: 0x00024019
		public void Dispose()
		{
		}

		// Token: 0x04000B71 RID: 2929
		private int m_ApiVersion;
	}
}
