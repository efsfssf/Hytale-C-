using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x020003E5 RID: 997
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct LobbyModificationSetAllowedPlatformIdsOptionsInternal : ISettable<LobbyModificationSetAllowedPlatformIdsOptions>, IDisposable
	{
		// Token: 0x170007B7 RID: 1975
		// (set) Token: 0x06001B0E RID: 6926 RVA: 0x00028A56 File Offset: 0x00026C56
		public uint[] AllowedPlatformIds
		{
			set
			{
				Helper.Set<uint>(value, ref this.m_AllowedPlatformIds, out this.m_AllowedPlatformIdsCount);
			}
		}

		// Token: 0x06001B0F RID: 6927 RVA: 0x00028A6C File Offset: 0x00026C6C
		public void Set(ref LobbyModificationSetAllowedPlatformIdsOptions other)
		{
			this.m_ApiVersion = 1;
			this.AllowedPlatformIds = other.AllowedPlatformIds;
		}

		// Token: 0x06001B10 RID: 6928 RVA: 0x00028A84 File Offset: 0x00026C84
		public void Set(ref LobbyModificationSetAllowedPlatformIdsOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.AllowedPlatformIds = other.Value.AllowedPlatformIds;
			}
		}

		// Token: 0x06001B11 RID: 6929 RVA: 0x00028ABA File Offset: 0x00026CBA
		public void Dispose()
		{
			Helper.Dispose(ref this.m_AllowedPlatformIds);
		}

		// Token: 0x04000C1B RID: 3099
		private int m_ApiVersion;

		// Token: 0x04000C1C RID: 3100
		private IntPtr m_AllowedPlatformIds;

		// Token: 0x04000C1D RID: 3101
		private uint m_AllowedPlatformIdsCount;
	}
}
