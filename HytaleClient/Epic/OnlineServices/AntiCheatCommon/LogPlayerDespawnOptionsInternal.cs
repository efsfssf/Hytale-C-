using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatCommon
{
	// Token: 0x020006AE RID: 1710
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct LogPlayerDespawnOptionsInternal : ISettable<LogPlayerDespawnOptions>, IDisposable
	{
		// Token: 0x17000CF9 RID: 3321
		// (set) Token: 0x06002C14 RID: 11284 RVA: 0x00041216 File Offset: 0x0003F416
		public IntPtr DespawnedPlayerHandle
		{
			set
			{
				this.m_DespawnedPlayerHandle = value;
			}
		}

		// Token: 0x06002C15 RID: 11285 RVA: 0x00041220 File Offset: 0x0003F420
		public void Set(ref LogPlayerDespawnOptions other)
		{
			this.m_ApiVersion = 1;
			this.DespawnedPlayerHandle = other.DespawnedPlayerHandle;
		}

		// Token: 0x06002C16 RID: 11286 RVA: 0x00041238 File Offset: 0x0003F438
		public void Set(ref LogPlayerDespawnOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.DespawnedPlayerHandle = other.Value.DespawnedPlayerHandle;
			}
		}

		// Token: 0x06002C17 RID: 11287 RVA: 0x0004126E File Offset: 0x0003F46E
		public void Dispose()
		{
			Helper.Dispose(ref this.m_DespawnedPlayerHandle);
		}

		// Token: 0x04001354 RID: 4948
		private int m_ApiVersion;

		// Token: 0x04001355 RID: 4949
		private IntPtr m_DespawnedPlayerHandle;
	}
}
