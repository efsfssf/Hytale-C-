using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x02000170 RID: 368
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct SessionModificationSetMaxPlayersOptionsInternal : ISettable<SessionModificationSetMaxPlayersOptions>, IDisposable
	{
		// Token: 0x1700027F RID: 639
		// (set) Token: 0x06000AF5 RID: 2805 RVA: 0x0000F8DA File Offset: 0x0000DADA
		public uint MaxPlayers
		{
			set
			{
				this.m_MaxPlayers = value;
			}
		}

		// Token: 0x06000AF6 RID: 2806 RVA: 0x0000F8E4 File Offset: 0x0000DAE4
		public void Set(ref SessionModificationSetMaxPlayersOptions other)
		{
			this.m_ApiVersion = 1;
			this.MaxPlayers = other.MaxPlayers;
		}

		// Token: 0x06000AF7 RID: 2807 RVA: 0x0000F8FC File Offset: 0x0000DAFC
		public void Set(ref SessionModificationSetMaxPlayersOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.MaxPlayers = other.Value.MaxPlayers;
			}
		}

		// Token: 0x06000AF8 RID: 2808 RVA: 0x0000F932 File Offset: 0x0000DB32
		public void Dispose()
		{
		}

		// Token: 0x040004FF RID: 1279
		private int m_ApiVersion;

		// Token: 0x04000500 RID: 1280
		private uint m_MaxPlayers;
	}
}
