using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x020003A4 RID: 932
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct JoinRTCRoomOptionsInternal : ISettable<JoinRTCRoomOptions>, IDisposable
	{
		// Token: 0x1700071C RID: 1820
		// (set) Token: 0x0600191F RID: 6431 RVA: 0x00024C35 File Offset: 0x00022E35
		public Utf8String LobbyId
		{
			set
			{
				Helper.Set(value, ref this.m_LobbyId);
			}
		}

		// Token: 0x1700071D RID: 1821
		// (set) Token: 0x06001920 RID: 6432 RVA: 0x00024C45 File Offset: 0x00022E45
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x1700071E RID: 1822
		// (set) Token: 0x06001921 RID: 6433 RVA: 0x00024C55 File Offset: 0x00022E55
		public LocalRTCOptions? LocalRTCOptions
		{
			set
			{
				Helper.Set<LocalRTCOptions, LocalRTCOptionsInternal>(ref value, ref this.m_LocalRTCOptions);
			}
		}

		// Token: 0x06001922 RID: 6434 RVA: 0x00024C66 File Offset: 0x00022E66
		public void Set(ref JoinRTCRoomOptions other)
		{
			this.m_ApiVersion = 1;
			this.LobbyId = other.LobbyId;
			this.LocalUserId = other.LocalUserId;
			this.LocalRTCOptions = other.LocalRTCOptions;
		}

		// Token: 0x06001923 RID: 6435 RVA: 0x00024C98 File Offset: 0x00022E98
		public void Set(ref JoinRTCRoomOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LobbyId = other.Value.LobbyId;
				this.LocalUserId = other.Value.LocalUserId;
				this.LocalRTCOptions = other.Value.LocalRTCOptions;
			}
		}

		// Token: 0x06001924 RID: 6436 RVA: 0x00024CF8 File Offset: 0x00022EF8
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LobbyId);
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_LocalRTCOptions);
		}

		// Token: 0x04000B20 RID: 2848
		private int m_ApiVersion;

		// Token: 0x04000B21 RID: 2849
		private IntPtr m_LobbyId;

		// Token: 0x04000B22 RID: 2850
		private IntPtr m_LocalUserId;

		// Token: 0x04000B23 RID: 2851
		private IntPtr m_LocalRTCOptions;
	}
}
