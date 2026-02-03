using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x02000370 RID: 880
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct AddNotifyRTCRoomConnectionChangedOptionsInternal : ISettable<AddNotifyRTCRoomConnectionChangedOptions>, IDisposable
	{
		// Token: 0x17000675 RID: 1653
		// (set) Token: 0x0600179B RID: 6043 RVA: 0x0002273A File Offset: 0x0002093A
		public Utf8String LobbyId_DEPRECATED
		{
			set
			{
				Helper.Set(value, ref this.m_LobbyId_DEPRECATED);
			}
		}

		// Token: 0x17000676 RID: 1654
		// (set) Token: 0x0600179C RID: 6044 RVA: 0x0002274A File Offset: 0x0002094A
		public ProductUserId LocalUserId_DEPRECATED
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId_DEPRECATED);
			}
		}

		// Token: 0x0600179D RID: 6045 RVA: 0x0002275A File Offset: 0x0002095A
		public void Set(ref AddNotifyRTCRoomConnectionChangedOptions other)
		{
			this.m_ApiVersion = 2;
			this.LobbyId_DEPRECATED = other.LobbyId_DEPRECATED;
			this.LocalUserId_DEPRECATED = other.LocalUserId_DEPRECATED;
		}

		// Token: 0x0600179E RID: 6046 RVA: 0x00022780 File Offset: 0x00020980
		public void Set(ref AddNotifyRTCRoomConnectionChangedOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 2;
				this.LobbyId_DEPRECATED = other.Value.LobbyId_DEPRECATED;
				this.LocalUserId_DEPRECATED = other.Value.LocalUserId_DEPRECATED;
			}
		}

		// Token: 0x0600179F RID: 6047 RVA: 0x000227CB File Offset: 0x000209CB
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LobbyId_DEPRECATED);
			Helper.Dispose(ref this.m_LocalUserId_DEPRECATED);
		}

		// Token: 0x04000A6C RID: 2668
		private int m_ApiVersion;

		// Token: 0x04000A6D RID: 2669
		private IntPtr m_LobbyId_DEPRECATED;

		// Token: 0x04000A6E RID: 2670
		private IntPtr m_LocalUserId_DEPRECATED;
	}
}
