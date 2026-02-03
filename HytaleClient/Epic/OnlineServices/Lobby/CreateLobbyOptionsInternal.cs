using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x02000382 RID: 898
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct CreateLobbyOptionsInternal : ISettable<CreateLobbyOptions>, IDisposable
	{
		// Token: 0x170006A6 RID: 1702
		// (set) Token: 0x0600181F RID: 6175 RVA: 0x00023331 File Offset: 0x00021531
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x170006A7 RID: 1703
		// (set) Token: 0x06001820 RID: 6176 RVA: 0x00023341 File Offset: 0x00021541
		public uint MaxLobbyMembers
		{
			set
			{
				this.m_MaxLobbyMembers = value;
			}
		}

		// Token: 0x170006A8 RID: 1704
		// (set) Token: 0x06001821 RID: 6177 RVA: 0x0002334B File Offset: 0x0002154B
		public LobbyPermissionLevel PermissionLevel
		{
			set
			{
				this.m_PermissionLevel = value;
			}
		}

		// Token: 0x170006A9 RID: 1705
		// (set) Token: 0x06001822 RID: 6178 RVA: 0x00023355 File Offset: 0x00021555
		public bool PresenceEnabled
		{
			set
			{
				Helper.Set(value, ref this.m_PresenceEnabled);
			}
		}

		// Token: 0x170006AA RID: 1706
		// (set) Token: 0x06001823 RID: 6179 RVA: 0x00023365 File Offset: 0x00021565
		public bool AllowInvites
		{
			set
			{
				Helper.Set(value, ref this.m_AllowInvites);
			}
		}

		// Token: 0x170006AB RID: 1707
		// (set) Token: 0x06001824 RID: 6180 RVA: 0x00023375 File Offset: 0x00021575
		public Utf8String BucketId
		{
			set
			{
				Helper.Set(value, ref this.m_BucketId);
			}
		}

		// Token: 0x170006AC RID: 1708
		// (set) Token: 0x06001825 RID: 6181 RVA: 0x00023385 File Offset: 0x00021585
		public bool DisableHostMigration
		{
			set
			{
				Helper.Set(value, ref this.m_DisableHostMigration);
			}
		}

		// Token: 0x170006AD RID: 1709
		// (set) Token: 0x06001826 RID: 6182 RVA: 0x00023395 File Offset: 0x00021595
		public bool EnableRTCRoom
		{
			set
			{
				Helper.Set(value, ref this.m_EnableRTCRoom);
			}
		}

		// Token: 0x170006AE RID: 1710
		// (set) Token: 0x06001827 RID: 6183 RVA: 0x000233A5 File Offset: 0x000215A5
		public LocalRTCOptions? LocalRTCOptions
		{
			set
			{
				Helper.Set<LocalRTCOptions, LocalRTCOptionsInternal>(ref value, ref this.m_LocalRTCOptions);
			}
		}

		// Token: 0x170006AF RID: 1711
		// (set) Token: 0x06001828 RID: 6184 RVA: 0x000233B6 File Offset: 0x000215B6
		public Utf8String LobbyId
		{
			set
			{
				Helper.Set(value, ref this.m_LobbyId);
			}
		}

		// Token: 0x170006B0 RID: 1712
		// (set) Token: 0x06001829 RID: 6185 RVA: 0x000233C6 File Offset: 0x000215C6
		public bool EnableJoinById
		{
			set
			{
				Helper.Set(value, ref this.m_EnableJoinById);
			}
		}

		// Token: 0x170006B1 RID: 1713
		// (set) Token: 0x0600182A RID: 6186 RVA: 0x000233D6 File Offset: 0x000215D6
		public bool RejoinAfterKickRequiresInvite
		{
			set
			{
				Helper.Set(value, ref this.m_RejoinAfterKickRequiresInvite);
			}
		}

		// Token: 0x170006B2 RID: 1714
		// (set) Token: 0x0600182B RID: 6187 RVA: 0x000233E6 File Offset: 0x000215E6
		public uint[] AllowedPlatformIds
		{
			set
			{
				Helper.Set<uint>(value, ref this.m_AllowedPlatformIds, out this.m_AllowedPlatformIdsCount);
			}
		}

		// Token: 0x170006B3 RID: 1715
		// (set) Token: 0x0600182C RID: 6188 RVA: 0x000233FC File Offset: 0x000215FC
		public bool CrossplayOptOut
		{
			set
			{
				Helper.Set(value, ref this.m_CrossplayOptOut);
			}
		}

		// Token: 0x170006B4 RID: 1716
		// (set) Token: 0x0600182D RID: 6189 RVA: 0x0002340C File Offset: 0x0002160C
		public LobbyRTCRoomJoinActionType RTCRoomJoinActionType
		{
			set
			{
				this.m_RTCRoomJoinActionType = value;
			}
		}

		// Token: 0x0600182E RID: 6190 RVA: 0x00023418 File Offset: 0x00021618
		public void Set(ref CreateLobbyOptions other)
		{
			this.m_ApiVersion = 10;
			this.LocalUserId = other.LocalUserId;
			this.MaxLobbyMembers = other.MaxLobbyMembers;
			this.PermissionLevel = other.PermissionLevel;
			this.PresenceEnabled = other.PresenceEnabled;
			this.AllowInvites = other.AllowInvites;
			this.BucketId = other.BucketId;
			this.DisableHostMigration = other.DisableHostMigration;
			this.EnableRTCRoom = other.EnableRTCRoom;
			this.LocalRTCOptions = other.LocalRTCOptions;
			this.LobbyId = other.LobbyId;
			this.EnableJoinById = other.EnableJoinById;
			this.RejoinAfterKickRequiresInvite = other.RejoinAfterKickRequiresInvite;
			this.AllowedPlatformIds = other.AllowedPlatformIds;
			this.CrossplayOptOut = other.CrossplayOptOut;
			this.RTCRoomJoinActionType = other.RTCRoomJoinActionType;
		}

		// Token: 0x0600182F RID: 6191 RVA: 0x000234F4 File Offset: 0x000216F4
		public void Set(ref CreateLobbyOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 10;
				this.LocalUserId = other.Value.LocalUserId;
				this.MaxLobbyMembers = other.Value.MaxLobbyMembers;
				this.PermissionLevel = other.Value.PermissionLevel;
				this.PresenceEnabled = other.Value.PresenceEnabled;
				this.AllowInvites = other.Value.AllowInvites;
				this.BucketId = other.Value.BucketId;
				this.DisableHostMigration = other.Value.DisableHostMigration;
				this.EnableRTCRoom = other.Value.EnableRTCRoom;
				this.LocalRTCOptions = other.Value.LocalRTCOptions;
				this.LobbyId = other.Value.LobbyId;
				this.EnableJoinById = other.Value.EnableJoinById;
				this.RejoinAfterKickRequiresInvite = other.Value.RejoinAfterKickRequiresInvite;
				this.AllowedPlatformIds = other.Value.AllowedPlatformIds;
				this.CrossplayOptOut = other.Value.CrossplayOptOut;
				this.RTCRoomJoinActionType = other.Value.RTCRoomJoinActionType;
			}
		}

		// Token: 0x06001830 RID: 6192 RVA: 0x00023654 File Offset: 0x00021854
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_BucketId);
			Helper.Dispose(ref this.m_LocalRTCOptions);
			Helper.Dispose(ref this.m_LobbyId);
			Helper.Dispose(ref this.m_AllowedPlatformIds);
		}

		// Token: 0x04000AA4 RID: 2724
		private int m_ApiVersion;

		// Token: 0x04000AA5 RID: 2725
		private IntPtr m_LocalUserId;

		// Token: 0x04000AA6 RID: 2726
		private uint m_MaxLobbyMembers;

		// Token: 0x04000AA7 RID: 2727
		private LobbyPermissionLevel m_PermissionLevel;

		// Token: 0x04000AA8 RID: 2728
		private int m_PresenceEnabled;

		// Token: 0x04000AA9 RID: 2729
		private int m_AllowInvites;

		// Token: 0x04000AAA RID: 2730
		private IntPtr m_BucketId;

		// Token: 0x04000AAB RID: 2731
		private int m_DisableHostMigration;

		// Token: 0x04000AAC RID: 2732
		private int m_EnableRTCRoom;

		// Token: 0x04000AAD RID: 2733
		private IntPtr m_LocalRTCOptions;

		// Token: 0x04000AAE RID: 2734
		private IntPtr m_LobbyId;

		// Token: 0x04000AAF RID: 2735
		private int m_EnableJoinById;

		// Token: 0x04000AB0 RID: 2736
		private int m_RejoinAfterKickRequiresInvite;

		// Token: 0x04000AB1 RID: 2737
		private IntPtr m_AllowedPlatformIds;

		// Token: 0x04000AB2 RID: 2738
		private uint m_AllowedPlatformIdsCount;

		// Token: 0x04000AB3 RID: 2739
		private int m_CrossplayOptOut;

		// Token: 0x04000AB4 RID: 2740
		private LobbyRTCRoomJoinActionType m_RTCRoomJoinActionType;
	}
}
