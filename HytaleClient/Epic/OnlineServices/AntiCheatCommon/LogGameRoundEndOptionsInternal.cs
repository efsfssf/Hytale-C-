using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatCommon
{
	// Token: 0x020006AA RID: 1706
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct LogGameRoundEndOptionsInternal : ISettable<LogGameRoundEndOptions>, IDisposable
	{
		// Token: 0x17000CED RID: 3309
		// (set) Token: 0x06002BFC RID: 11260 RVA: 0x00041007 File Offset: 0x0003F207
		public uint WinningTeamId
		{
			set
			{
				this.m_WinningTeamId = value;
			}
		}

		// Token: 0x06002BFD RID: 11261 RVA: 0x00041011 File Offset: 0x0003F211
		public void Set(ref LogGameRoundEndOptions other)
		{
			this.m_ApiVersion = 1;
			this.WinningTeamId = other.WinningTeamId;
		}

		// Token: 0x06002BFE RID: 11262 RVA: 0x00041028 File Offset: 0x0003F228
		public void Set(ref LogGameRoundEndOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.WinningTeamId = other.Value.WinningTeamId;
			}
		}

		// Token: 0x06002BFF RID: 11263 RVA: 0x0004105E File Offset: 0x0003F25E
		public void Dispose()
		{
		}

		// Token: 0x04001346 RID: 4934
		private int m_ApiVersion;

		// Token: 0x04001347 RID: 4935
		private uint m_WinningTeamId;
	}
}
