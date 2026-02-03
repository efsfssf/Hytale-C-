using System;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x02000381 RID: 897
	public struct CreateLobbyOptions
	{
		// Token: 0x17000697 RID: 1687
		// (get) Token: 0x06001801 RID: 6145 RVA: 0x00023232 File Offset: 0x00021432
		// (set) Token: 0x06001802 RID: 6146 RVA: 0x0002323A File Offset: 0x0002143A
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x17000698 RID: 1688
		// (get) Token: 0x06001803 RID: 6147 RVA: 0x00023243 File Offset: 0x00021443
		// (set) Token: 0x06001804 RID: 6148 RVA: 0x0002324B File Offset: 0x0002144B
		public uint MaxLobbyMembers { get; set; }

		// Token: 0x17000699 RID: 1689
		// (get) Token: 0x06001805 RID: 6149 RVA: 0x00023254 File Offset: 0x00021454
		// (set) Token: 0x06001806 RID: 6150 RVA: 0x0002325C File Offset: 0x0002145C
		public LobbyPermissionLevel PermissionLevel { get; set; }

		// Token: 0x1700069A RID: 1690
		// (get) Token: 0x06001807 RID: 6151 RVA: 0x00023265 File Offset: 0x00021465
		// (set) Token: 0x06001808 RID: 6152 RVA: 0x0002326D File Offset: 0x0002146D
		public bool PresenceEnabled { get; set; }

		// Token: 0x1700069B RID: 1691
		// (get) Token: 0x06001809 RID: 6153 RVA: 0x00023276 File Offset: 0x00021476
		// (set) Token: 0x0600180A RID: 6154 RVA: 0x0002327E File Offset: 0x0002147E
		public bool AllowInvites { get; set; }

		// Token: 0x1700069C RID: 1692
		// (get) Token: 0x0600180B RID: 6155 RVA: 0x00023287 File Offset: 0x00021487
		// (set) Token: 0x0600180C RID: 6156 RVA: 0x0002328F File Offset: 0x0002148F
		public Utf8String BucketId { get; set; }

		// Token: 0x1700069D RID: 1693
		// (get) Token: 0x0600180D RID: 6157 RVA: 0x00023298 File Offset: 0x00021498
		// (set) Token: 0x0600180E RID: 6158 RVA: 0x000232A0 File Offset: 0x000214A0
		public bool DisableHostMigration { get; set; }

		// Token: 0x1700069E RID: 1694
		// (get) Token: 0x0600180F RID: 6159 RVA: 0x000232A9 File Offset: 0x000214A9
		// (set) Token: 0x06001810 RID: 6160 RVA: 0x000232B1 File Offset: 0x000214B1
		public bool EnableRTCRoom { get; set; }

		// Token: 0x1700069F RID: 1695
		// (get) Token: 0x06001811 RID: 6161 RVA: 0x000232BA File Offset: 0x000214BA
		// (set) Token: 0x06001812 RID: 6162 RVA: 0x000232C2 File Offset: 0x000214C2
		public LocalRTCOptions? LocalRTCOptions { get; set; }

		// Token: 0x170006A0 RID: 1696
		// (get) Token: 0x06001813 RID: 6163 RVA: 0x000232CB File Offset: 0x000214CB
		// (set) Token: 0x06001814 RID: 6164 RVA: 0x000232D3 File Offset: 0x000214D3
		public Utf8String LobbyId { get; set; }

		// Token: 0x170006A1 RID: 1697
		// (get) Token: 0x06001815 RID: 6165 RVA: 0x000232DC File Offset: 0x000214DC
		// (set) Token: 0x06001816 RID: 6166 RVA: 0x000232E4 File Offset: 0x000214E4
		public bool EnableJoinById { get; set; }

		// Token: 0x170006A2 RID: 1698
		// (get) Token: 0x06001817 RID: 6167 RVA: 0x000232ED File Offset: 0x000214ED
		// (set) Token: 0x06001818 RID: 6168 RVA: 0x000232F5 File Offset: 0x000214F5
		public bool RejoinAfterKickRequiresInvite { get; set; }

		// Token: 0x170006A3 RID: 1699
		// (get) Token: 0x06001819 RID: 6169 RVA: 0x000232FE File Offset: 0x000214FE
		// (set) Token: 0x0600181A RID: 6170 RVA: 0x00023306 File Offset: 0x00021506
		public uint[] AllowedPlatformIds { get; set; }

		// Token: 0x170006A4 RID: 1700
		// (get) Token: 0x0600181B RID: 6171 RVA: 0x0002330F File Offset: 0x0002150F
		// (set) Token: 0x0600181C RID: 6172 RVA: 0x00023317 File Offset: 0x00021517
		public bool CrossplayOptOut { get; set; }

		// Token: 0x170006A5 RID: 1701
		// (get) Token: 0x0600181D RID: 6173 RVA: 0x00023320 File Offset: 0x00021520
		// (set) Token: 0x0600181E RID: 6174 RVA: 0x00023328 File Offset: 0x00021528
		public LobbyRTCRoomJoinActionType RTCRoomJoinActionType { get; set; }
	}
}
