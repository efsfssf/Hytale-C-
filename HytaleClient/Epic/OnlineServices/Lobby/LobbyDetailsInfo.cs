using System;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x020003CB RID: 971
	public struct LobbyDetailsInfo
	{
		// Token: 0x1700075B RID: 1883
		// (get) Token: 0x060019D9 RID: 6617 RVA: 0x00025F64 File Offset: 0x00024164
		// (set) Token: 0x060019DA RID: 6618 RVA: 0x00025F6C File Offset: 0x0002416C
		public Utf8String LobbyId { get; set; }

		// Token: 0x1700075C RID: 1884
		// (get) Token: 0x060019DB RID: 6619 RVA: 0x00025F75 File Offset: 0x00024175
		// (set) Token: 0x060019DC RID: 6620 RVA: 0x00025F7D File Offset: 0x0002417D
		public ProductUserId LobbyOwnerUserId { get; set; }

		// Token: 0x1700075D RID: 1885
		// (get) Token: 0x060019DD RID: 6621 RVA: 0x00025F86 File Offset: 0x00024186
		// (set) Token: 0x060019DE RID: 6622 RVA: 0x00025F8E File Offset: 0x0002418E
		public LobbyPermissionLevel PermissionLevel { get; set; }

		// Token: 0x1700075E RID: 1886
		// (get) Token: 0x060019DF RID: 6623 RVA: 0x00025F97 File Offset: 0x00024197
		// (set) Token: 0x060019E0 RID: 6624 RVA: 0x00025F9F File Offset: 0x0002419F
		public uint AvailableSlots { get; set; }

		// Token: 0x1700075F RID: 1887
		// (get) Token: 0x060019E1 RID: 6625 RVA: 0x00025FA8 File Offset: 0x000241A8
		// (set) Token: 0x060019E2 RID: 6626 RVA: 0x00025FB0 File Offset: 0x000241B0
		public uint MaxMembers { get; set; }

		// Token: 0x17000760 RID: 1888
		// (get) Token: 0x060019E3 RID: 6627 RVA: 0x00025FB9 File Offset: 0x000241B9
		// (set) Token: 0x060019E4 RID: 6628 RVA: 0x00025FC1 File Offset: 0x000241C1
		public bool AllowInvites { get; set; }

		// Token: 0x17000761 RID: 1889
		// (get) Token: 0x060019E5 RID: 6629 RVA: 0x00025FCA File Offset: 0x000241CA
		// (set) Token: 0x060019E6 RID: 6630 RVA: 0x00025FD2 File Offset: 0x000241D2
		public Utf8String BucketId { get; set; }

		// Token: 0x17000762 RID: 1890
		// (get) Token: 0x060019E7 RID: 6631 RVA: 0x00025FDB File Offset: 0x000241DB
		// (set) Token: 0x060019E8 RID: 6632 RVA: 0x00025FE3 File Offset: 0x000241E3
		public bool AllowHostMigration { get; set; }

		// Token: 0x17000763 RID: 1891
		// (get) Token: 0x060019E9 RID: 6633 RVA: 0x00025FEC File Offset: 0x000241EC
		// (set) Token: 0x060019EA RID: 6634 RVA: 0x00025FF4 File Offset: 0x000241F4
		public bool RTCRoomEnabled { get; set; }

		// Token: 0x17000764 RID: 1892
		// (get) Token: 0x060019EB RID: 6635 RVA: 0x00025FFD File Offset: 0x000241FD
		// (set) Token: 0x060019EC RID: 6636 RVA: 0x00026005 File Offset: 0x00024205
		public bool AllowJoinById { get; set; }

		// Token: 0x17000765 RID: 1893
		// (get) Token: 0x060019ED RID: 6637 RVA: 0x0002600E File Offset: 0x0002420E
		// (set) Token: 0x060019EE RID: 6638 RVA: 0x00026016 File Offset: 0x00024216
		public bool RejoinAfterKickRequiresInvite { get; set; }

		// Token: 0x17000766 RID: 1894
		// (get) Token: 0x060019EF RID: 6639 RVA: 0x0002601F File Offset: 0x0002421F
		// (set) Token: 0x060019F0 RID: 6640 RVA: 0x00026027 File Offset: 0x00024227
		public bool PresenceEnabled { get; set; }

		// Token: 0x17000767 RID: 1895
		// (get) Token: 0x060019F1 RID: 6641 RVA: 0x00026030 File Offset: 0x00024230
		// (set) Token: 0x060019F2 RID: 6642 RVA: 0x00026038 File Offset: 0x00024238
		public uint[] AllowedPlatformIds { get; set; }

		// Token: 0x060019F3 RID: 6643 RVA: 0x00026044 File Offset: 0x00024244
		internal void Set(ref LobbyDetailsInfoInternal other)
		{
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
	}
}
