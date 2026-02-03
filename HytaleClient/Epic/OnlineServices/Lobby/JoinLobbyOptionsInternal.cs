using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x020003A0 RID: 928
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct JoinLobbyOptionsInternal : ISettable<JoinLobbyOptions>, IDisposable
	{
		// Token: 0x1700070C RID: 1804
		// (set) Token: 0x060018FD RID: 6397 RVA: 0x000248AC File Offset: 0x00022AAC
		public LobbyDetails LobbyDetailsHandle
		{
			set
			{
				Helper.Set(value, ref this.m_LobbyDetailsHandle);
			}
		}

		// Token: 0x1700070D RID: 1805
		// (set) Token: 0x060018FE RID: 6398 RVA: 0x000248BC File Offset: 0x00022ABC
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x1700070E RID: 1806
		// (set) Token: 0x060018FF RID: 6399 RVA: 0x000248CC File Offset: 0x00022ACC
		public bool PresenceEnabled
		{
			set
			{
				Helper.Set(value, ref this.m_PresenceEnabled);
			}
		}

		// Token: 0x1700070F RID: 1807
		// (set) Token: 0x06001900 RID: 6400 RVA: 0x000248DC File Offset: 0x00022ADC
		public LocalRTCOptions? LocalRTCOptions
		{
			set
			{
				Helper.Set<LocalRTCOptions, LocalRTCOptionsInternal>(ref value, ref this.m_LocalRTCOptions);
			}
		}

		// Token: 0x17000710 RID: 1808
		// (set) Token: 0x06001901 RID: 6401 RVA: 0x000248ED File Offset: 0x00022AED
		public bool CrossplayOptOut
		{
			set
			{
				Helper.Set(value, ref this.m_CrossplayOptOut);
			}
		}

		// Token: 0x17000711 RID: 1809
		// (set) Token: 0x06001902 RID: 6402 RVA: 0x000248FD File Offset: 0x00022AFD
		public LobbyRTCRoomJoinActionType RTCRoomJoinActionType
		{
			set
			{
				this.m_RTCRoomJoinActionType = value;
			}
		}

		// Token: 0x06001903 RID: 6403 RVA: 0x00024908 File Offset: 0x00022B08
		public void Set(ref JoinLobbyOptions other)
		{
			this.m_ApiVersion = 5;
			this.LobbyDetailsHandle = other.LobbyDetailsHandle;
			this.LocalUserId = other.LocalUserId;
			this.PresenceEnabled = other.PresenceEnabled;
			this.LocalRTCOptions = other.LocalRTCOptions;
			this.CrossplayOptOut = other.CrossplayOptOut;
			this.RTCRoomJoinActionType = other.RTCRoomJoinActionType;
		}

		// Token: 0x06001904 RID: 6404 RVA: 0x0002496C File Offset: 0x00022B6C
		public void Set(ref JoinLobbyOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 5;
				this.LobbyDetailsHandle = other.Value.LobbyDetailsHandle;
				this.LocalUserId = other.Value.LocalUserId;
				this.PresenceEnabled = other.Value.PresenceEnabled;
				this.LocalRTCOptions = other.Value.LocalRTCOptions;
				this.CrossplayOptOut = other.Value.CrossplayOptOut;
				this.RTCRoomJoinActionType = other.Value.RTCRoomJoinActionType;
			}
		}

		// Token: 0x06001905 RID: 6405 RVA: 0x00024A0E File Offset: 0x00022C0E
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LobbyDetailsHandle);
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_LocalRTCOptions);
		}

		// Token: 0x04000B10 RID: 2832
		private int m_ApiVersion;

		// Token: 0x04000B11 RID: 2833
		private IntPtr m_LobbyDetailsHandle;

		// Token: 0x04000B12 RID: 2834
		private IntPtr m_LocalUserId;

		// Token: 0x04000B13 RID: 2835
		private int m_PresenceEnabled;

		// Token: 0x04000B14 RID: 2836
		private IntPtr m_LocalRTCOptions;

		// Token: 0x04000B15 RID: 2837
		private int m_CrossplayOptOut;

		// Token: 0x04000B16 RID: 2838
		private LobbyRTCRoomJoinActionType m_RTCRoomJoinActionType;
	}
}
