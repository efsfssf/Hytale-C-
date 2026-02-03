using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatCommon
{
	// Token: 0x020006B2 RID: 1714
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct LogPlayerSpawnOptionsInternal : ISettable<LogPlayerSpawnOptions>, IDisposable
	{
		// Token: 0x17000D01 RID: 3329
		// (set) Token: 0x06002C27 RID: 11303 RVA: 0x00041371 File Offset: 0x0003F571
		public IntPtr SpawnedPlayerHandle
		{
			set
			{
				this.m_SpawnedPlayerHandle = value;
			}
		}

		// Token: 0x17000D02 RID: 3330
		// (set) Token: 0x06002C28 RID: 11304 RVA: 0x0004137B File Offset: 0x0003F57B
		public uint TeamId
		{
			set
			{
				this.m_TeamId = value;
			}
		}

		// Token: 0x17000D03 RID: 3331
		// (set) Token: 0x06002C29 RID: 11305 RVA: 0x00041385 File Offset: 0x0003F585
		public uint CharacterId
		{
			set
			{
				this.m_CharacterId = value;
			}
		}

		// Token: 0x06002C2A RID: 11306 RVA: 0x0004138F File Offset: 0x0003F58F
		public void Set(ref LogPlayerSpawnOptions other)
		{
			this.m_ApiVersion = 1;
			this.SpawnedPlayerHandle = other.SpawnedPlayerHandle;
			this.TeamId = other.TeamId;
			this.CharacterId = other.CharacterId;
		}

		// Token: 0x06002C2B RID: 11307 RVA: 0x000413C0 File Offset: 0x0003F5C0
		public void Set(ref LogPlayerSpawnOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.SpawnedPlayerHandle = other.Value.SpawnedPlayerHandle;
				this.TeamId = other.Value.TeamId;
				this.CharacterId = other.Value.CharacterId;
			}
		}

		// Token: 0x06002C2C RID: 11308 RVA: 0x00041420 File Offset: 0x0003F620
		public void Dispose()
		{
			Helper.Dispose(ref this.m_SpawnedPlayerHandle);
		}

		// Token: 0x0400135E RID: 4958
		private int m_ApiVersion;

		// Token: 0x0400135F RID: 4959
		private IntPtr m_SpawnedPlayerHandle;

		// Token: 0x04001360 RID: 4960
		private uint m_TeamId;

		// Token: 0x04001361 RID: 4961
		private uint m_CharacterId;
	}
}
