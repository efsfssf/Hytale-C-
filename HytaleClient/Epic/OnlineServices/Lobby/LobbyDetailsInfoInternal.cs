using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x020003CC RID: 972
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct LobbyDetailsInfoInternal : IGettable<LobbyDetailsInfo>, ISettable<LobbyDetailsInfo>, IDisposable
	{
		// Token: 0x17000768 RID: 1896
		// (get) Token: 0x060019F4 RID: 6644 RVA: 0x000260FC File Offset: 0x000242FC
		// (set) Token: 0x060019F5 RID: 6645 RVA: 0x0002611D File Offset: 0x0002431D
		public Utf8String LobbyId
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_LobbyId, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_LobbyId);
			}
		}

		// Token: 0x17000769 RID: 1897
		// (get) Token: 0x060019F6 RID: 6646 RVA: 0x00026130 File Offset: 0x00024330
		// (set) Token: 0x060019F7 RID: 6647 RVA: 0x00026151 File Offset: 0x00024351
		public ProductUserId LobbyOwnerUserId
		{
			get
			{
				ProductUserId result;
				Helper.Get<ProductUserId>(this.m_LobbyOwnerUserId, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_LobbyOwnerUserId);
			}
		}

		// Token: 0x1700076A RID: 1898
		// (get) Token: 0x060019F8 RID: 6648 RVA: 0x00026164 File Offset: 0x00024364
		// (set) Token: 0x060019F9 RID: 6649 RVA: 0x0002617C File Offset: 0x0002437C
		public LobbyPermissionLevel PermissionLevel
		{
			get
			{
				return this.m_PermissionLevel;
			}
			set
			{
				this.m_PermissionLevel = value;
			}
		}

		// Token: 0x1700076B RID: 1899
		// (get) Token: 0x060019FA RID: 6650 RVA: 0x00026188 File Offset: 0x00024388
		// (set) Token: 0x060019FB RID: 6651 RVA: 0x000261A0 File Offset: 0x000243A0
		public uint AvailableSlots
		{
			get
			{
				return this.m_AvailableSlots;
			}
			set
			{
				this.m_AvailableSlots = value;
			}
		}

		// Token: 0x1700076C RID: 1900
		// (get) Token: 0x060019FC RID: 6652 RVA: 0x000261AC File Offset: 0x000243AC
		// (set) Token: 0x060019FD RID: 6653 RVA: 0x000261C4 File Offset: 0x000243C4
		public uint MaxMembers
		{
			get
			{
				return this.m_MaxMembers;
			}
			set
			{
				this.m_MaxMembers = value;
			}
		}

		// Token: 0x1700076D RID: 1901
		// (get) Token: 0x060019FE RID: 6654 RVA: 0x000261D0 File Offset: 0x000243D0
		// (set) Token: 0x060019FF RID: 6655 RVA: 0x000261F1 File Offset: 0x000243F1
		public bool AllowInvites
		{
			get
			{
				bool result;
				Helper.Get(this.m_AllowInvites, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_AllowInvites);
			}
		}

		// Token: 0x1700076E RID: 1902
		// (get) Token: 0x06001A00 RID: 6656 RVA: 0x00026204 File Offset: 0x00024404
		// (set) Token: 0x06001A01 RID: 6657 RVA: 0x00026225 File Offset: 0x00024425
		public Utf8String BucketId
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_BucketId, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_BucketId);
			}
		}

		// Token: 0x1700076F RID: 1903
		// (get) Token: 0x06001A02 RID: 6658 RVA: 0x00026238 File Offset: 0x00024438
		// (set) Token: 0x06001A03 RID: 6659 RVA: 0x00026259 File Offset: 0x00024459
		public bool AllowHostMigration
		{
			get
			{
				bool result;
				Helper.Get(this.m_AllowHostMigration, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_AllowHostMigration);
			}
		}

		// Token: 0x17000770 RID: 1904
		// (get) Token: 0x06001A04 RID: 6660 RVA: 0x0002626C File Offset: 0x0002446C
		// (set) Token: 0x06001A05 RID: 6661 RVA: 0x0002628D File Offset: 0x0002448D
		public bool RTCRoomEnabled
		{
			get
			{
				bool result;
				Helper.Get(this.m_RTCRoomEnabled, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_RTCRoomEnabled);
			}
		}

		// Token: 0x17000771 RID: 1905
		// (get) Token: 0x06001A06 RID: 6662 RVA: 0x000262A0 File Offset: 0x000244A0
		// (set) Token: 0x06001A07 RID: 6663 RVA: 0x000262C1 File Offset: 0x000244C1
		public bool AllowJoinById
		{
			get
			{
				bool result;
				Helper.Get(this.m_AllowJoinById, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_AllowJoinById);
			}
		}

		// Token: 0x17000772 RID: 1906
		// (get) Token: 0x06001A08 RID: 6664 RVA: 0x000262D4 File Offset: 0x000244D4
		// (set) Token: 0x06001A09 RID: 6665 RVA: 0x000262F5 File Offset: 0x000244F5
		public bool RejoinAfterKickRequiresInvite
		{
			get
			{
				bool result;
				Helper.Get(this.m_RejoinAfterKickRequiresInvite, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_RejoinAfterKickRequiresInvite);
			}
		}

		// Token: 0x17000773 RID: 1907
		// (get) Token: 0x06001A0A RID: 6666 RVA: 0x00026308 File Offset: 0x00024508
		// (set) Token: 0x06001A0B RID: 6667 RVA: 0x00026329 File Offset: 0x00024529
		public bool PresenceEnabled
		{
			get
			{
				bool result;
				Helper.Get(this.m_PresenceEnabled, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_PresenceEnabled);
			}
		}

		// Token: 0x17000774 RID: 1908
		// (get) Token: 0x06001A0C RID: 6668 RVA: 0x0002633C File Offset: 0x0002453C
		// (set) Token: 0x06001A0D RID: 6669 RVA: 0x00026363 File Offset: 0x00024563
		public uint[] AllowedPlatformIds
		{
			get
			{
				uint[] result;
				Helper.Get<uint>(this.m_AllowedPlatformIds, out result, this.m_AllowedPlatformIdsCount);
				return result;
			}
			set
			{
				Helper.Set<uint>(value, ref this.m_AllowedPlatformIds, out this.m_AllowedPlatformIdsCount);
			}
		}

		// Token: 0x06001A0E RID: 6670 RVA: 0x0002637C File Offset: 0x0002457C
		public void Set(ref LobbyDetailsInfo other)
		{
			this.m_ApiVersion = 3;
			this.LobbyId = other.LobbyId;
			this.LobbyOwnerUserId = other.LobbyOwnerUserId;
			this.PermissionLevel = other.PermissionLevel;
			this.AvailableSlots = other.AvailableSlots;
			this.MaxMembers = other.MaxMembers;
			this.AllowInvites = other.AllowInvites;
			this.BucketId = other.BucketId;
			this.AllowHostMigration = other.AllowHostMigration;
			this.RTCRoomEnabled = other.RTCRoomEnabled;
			this.AllowJoinById = other.AllowJoinById;
			this.RejoinAfterKickRequiresInvite = other.RejoinAfterKickRequiresInvite;
			this.PresenceEnabled = other.PresenceEnabled;
			this.AllowedPlatformIds = other.AllowedPlatformIds;
		}

		// Token: 0x06001A0F RID: 6671 RVA: 0x0002643C File Offset: 0x0002463C
		public void Set(ref LobbyDetailsInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 3;
				this.LobbyId = other.Value.LobbyId;
				this.LobbyOwnerUserId = other.Value.LobbyOwnerUserId;
				this.PermissionLevel = other.Value.PermissionLevel;
				this.AvailableSlots = other.Value.AvailableSlots;
				this.MaxMembers = other.Value.MaxMembers;
				this.AllowInvites = other.Value.AllowInvites;
				this.BucketId = other.Value.BucketId;
				this.AllowHostMigration = other.Value.AllowHostMigration;
				this.RTCRoomEnabled = other.Value.RTCRoomEnabled;
				this.AllowJoinById = other.Value.AllowJoinById;
				this.RejoinAfterKickRequiresInvite = other.Value.RejoinAfterKickRequiresInvite;
				this.PresenceEnabled = other.Value.PresenceEnabled;
				this.AllowedPlatformIds = other.Value.AllowedPlatformIds;
			}
		}

		// Token: 0x06001A10 RID: 6672 RVA: 0x00026571 File Offset: 0x00024771
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LobbyId);
			Helper.Dispose(ref this.m_LobbyOwnerUserId);
			Helper.Dispose(ref this.m_BucketId);
			Helper.Dispose(ref this.m_AllowedPlatformIds);
		}

		// Token: 0x06001A11 RID: 6673 RVA: 0x000265A4 File Offset: 0x000247A4
		public void Get(out LobbyDetailsInfo output)
		{
			output = default(LobbyDetailsInfo);
			output.Set(ref this);
		}

		// Token: 0x04000B87 RID: 2951
		private int m_ApiVersion;

		// Token: 0x04000B88 RID: 2952
		private IntPtr m_LobbyId;

		// Token: 0x04000B89 RID: 2953
		private IntPtr m_LobbyOwnerUserId;

		// Token: 0x04000B8A RID: 2954
		private LobbyPermissionLevel m_PermissionLevel;

		// Token: 0x04000B8B RID: 2955
		private uint m_AvailableSlots;

		// Token: 0x04000B8C RID: 2956
		private uint m_MaxMembers;

		// Token: 0x04000B8D RID: 2957
		private int m_AllowInvites;

		// Token: 0x04000B8E RID: 2958
		private IntPtr m_BucketId;

		// Token: 0x04000B8F RID: 2959
		private int m_AllowHostMigration;

		// Token: 0x04000B90 RID: 2960
		private int m_RTCRoomEnabled;

		// Token: 0x04000B91 RID: 2961
		private int m_AllowJoinById;

		// Token: 0x04000B92 RID: 2962
		private int m_RejoinAfterKickRequiresInvite;

		// Token: 0x04000B93 RID: 2963
		private int m_PresenceEnabled;

		// Token: 0x04000B94 RID: 2964
		private IntPtr m_AllowedPlatformIds;

		// Token: 0x04000B95 RID: 2965
		private uint m_AllowedPlatformIdsCount;
	}
}
