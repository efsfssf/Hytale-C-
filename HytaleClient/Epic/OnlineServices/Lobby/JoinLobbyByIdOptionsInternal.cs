using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x0200039C RID: 924
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct JoinLobbyByIdOptionsInternal : ISettable<JoinLobbyByIdOptions>, IDisposable
	{
		// Token: 0x170006F9 RID: 1785
		// (set) Token: 0x060018D5 RID: 6357 RVA: 0x000244F0 File Offset: 0x000226F0
		public Utf8String LobbyId
		{
			set
			{
				Helper.Set(value, ref this.m_LobbyId);
			}
		}

		// Token: 0x170006FA RID: 1786
		// (set) Token: 0x060018D6 RID: 6358 RVA: 0x00024500 File Offset: 0x00022700
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x170006FB RID: 1787
		// (set) Token: 0x060018D7 RID: 6359 RVA: 0x00024510 File Offset: 0x00022710
		public bool PresenceEnabled
		{
			set
			{
				Helper.Set(value, ref this.m_PresenceEnabled);
			}
		}

		// Token: 0x170006FC RID: 1788
		// (set) Token: 0x060018D8 RID: 6360 RVA: 0x00024520 File Offset: 0x00022720
		public LocalRTCOptions? LocalRTCOptions
		{
			set
			{
				Helper.Set<LocalRTCOptions, LocalRTCOptionsInternal>(ref value, ref this.m_LocalRTCOptions);
			}
		}

		// Token: 0x170006FD RID: 1789
		// (set) Token: 0x060018D9 RID: 6361 RVA: 0x00024531 File Offset: 0x00022731
		public bool CrossplayOptOut
		{
			set
			{
				Helper.Set(value, ref this.m_CrossplayOptOut);
			}
		}

		// Token: 0x170006FE RID: 1790
		// (set) Token: 0x060018DA RID: 6362 RVA: 0x00024541 File Offset: 0x00022741
		public LobbyRTCRoomJoinActionType RTCRoomJoinActionType
		{
			set
			{
				this.m_RTCRoomJoinActionType = value;
			}
		}

		// Token: 0x060018DB RID: 6363 RVA: 0x0002454C File Offset: 0x0002274C
		public void Set(ref JoinLobbyByIdOptions other)
		{
			this.m_ApiVersion = 3;
			this.LobbyId = other.LobbyId;
			this.LocalUserId = other.LocalUserId;
			this.PresenceEnabled = other.PresenceEnabled;
			this.LocalRTCOptions = other.LocalRTCOptions;
			this.CrossplayOptOut = other.CrossplayOptOut;
			this.RTCRoomJoinActionType = other.RTCRoomJoinActionType;
		}

		// Token: 0x060018DC RID: 6364 RVA: 0x000245B0 File Offset: 0x000227B0
		public void Set(ref JoinLobbyByIdOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 3;
				this.LobbyId = other.Value.LobbyId;
				this.LocalUserId = other.Value.LocalUserId;
				this.PresenceEnabled = other.Value.PresenceEnabled;
				this.LocalRTCOptions = other.Value.LocalRTCOptions;
				this.CrossplayOptOut = other.Value.CrossplayOptOut;
				this.RTCRoomJoinActionType = other.Value.RTCRoomJoinActionType;
			}
		}

		// Token: 0x060018DD RID: 6365 RVA: 0x00024652 File Offset: 0x00022852
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LobbyId);
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_LocalRTCOptions);
		}

		// Token: 0x04000AFD RID: 2813
		private int m_ApiVersion;

		// Token: 0x04000AFE RID: 2814
		private IntPtr m_LobbyId;

		// Token: 0x04000AFF RID: 2815
		private IntPtr m_LocalUserId;

		// Token: 0x04000B00 RID: 2816
		private int m_PresenceEnabled;

		// Token: 0x04000B01 RID: 2817
		private IntPtr m_LocalRTCOptions;

		// Token: 0x04000B02 RID: 2818
		private int m_CrossplayOptOut;

		// Token: 0x04000B03 RID: 2819
		private LobbyRTCRoomJoinActionType m_RTCRoomJoinActionType;
	}
}
