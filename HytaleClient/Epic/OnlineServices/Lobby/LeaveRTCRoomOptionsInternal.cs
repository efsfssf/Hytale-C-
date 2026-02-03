using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x020003B2 RID: 946
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct LeaveRTCRoomOptionsInternal : ISettable<LeaveRTCRoomOptions>, IDisposable
	{
		// Token: 0x17000747 RID: 1863
		// (set) Token: 0x0600198A RID: 6538 RVA: 0x00025684 File Offset: 0x00023884
		public Utf8String LobbyId
		{
			set
			{
				Helper.Set(value, ref this.m_LobbyId);
			}
		}

		// Token: 0x17000748 RID: 1864
		// (set) Token: 0x0600198B RID: 6539 RVA: 0x00025694 File Offset: 0x00023894
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x0600198C RID: 6540 RVA: 0x000256A4 File Offset: 0x000238A4
		public void Set(ref LeaveRTCRoomOptions other)
		{
			this.m_ApiVersion = 1;
			this.LobbyId = other.LobbyId;
			this.LocalUserId = other.LocalUserId;
		}

		// Token: 0x0600198D RID: 6541 RVA: 0x000256C8 File Offset: 0x000238C8
		public void Set(ref LeaveRTCRoomOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LobbyId = other.Value.LobbyId;
				this.LocalUserId = other.Value.LocalUserId;
			}
		}

		// Token: 0x0600198E RID: 6542 RVA: 0x00025713 File Offset: 0x00023913
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LobbyId);
			Helper.Dispose(ref this.m_LocalUserId);
		}

		// Token: 0x04000B4A RID: 2890
		private int m_ApiVersion;

		// Token: 0x04000B4B RID: 2891
		private IntPtr m_LobbyId;

		// Token: 0x04000B4C RID: 2892
		private IntPtr m_LocalUserId;
	}
}
