using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x020000DD RID: 221
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct ActiveSessionGetRegisteredPlayerByIndexOptionsInternal : ISettable<ActiveSessionGetRegisteredPlayerByIndexOptions>, IDisposable
	{
		// Token: 0x17000191 RID: 401
		// (set) Token: 0x060007E2 RID: 2018 RVA: 0x0000B761 File Offset: 0x00009961
		public uint PlayerIndex
		{
			set
			{
				this.m_PlayerIndex = value;
			}
		}

		// Token: 0x060007E3 RID: 2019 RVA: 0x0000B76B File Offset: 0x0000996B
		public void Set(ref ActiveSessionGetRegisteredPlayerByIndexOptions other)
		{
			this.m_ApiVersion = 1;
			this.PlayerIndex = other.PlayerIndex;
		}

		// Token: 0x060007E4 RID: 2020 RVA: 0x0000B784 File Offset: 0x00009984
		public void Set(ref ActiveSessionGetRegisteredPlayerByIndexOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.PlayerIndex = other.Value.PlayerIndex;
			}
		}

		// Token: 0x060007E5 RID: 2021 RVA: 0x0000B7BA File Offset: 0x000099BA
		public void Dispose()
		{
		}

		// Token: 0x040003CA RID: 970
		private int m_ApiVersion;

		// Token: 0x040003CB RID: 971
		private uint m_PlayerIndex;
	}
}
