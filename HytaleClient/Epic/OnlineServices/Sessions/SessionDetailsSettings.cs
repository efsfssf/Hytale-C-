using System;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x02000158 RID: 344
	public struct SessionDetailsSettings
	{
		// Token: 0x17000241 RID: 577
		// (get) Token: 0x06000A4C RID: 2636 RVA: 0x0000E6E3 File Offset: 0x0000C8E3
		// (set) Token: 0x06000A4D RID: 2637 RVA: 0x0000E6EB File Offset: 0x0000C8EB
		public Utf8String BucketId { get; set; }

		// Token: 0x17000242 RID: 578
		// (get) Token: 0x06000A4E RID: 2638 RVA: 0x0000E6F4 File Offset: 0x0000C8F4
		// (set) Token: 0x06000A4F RID: 2639 RVA: 0x0000E6FC File Offset: 0x0000C8FC
		public uint NumPublicConnections { get; set; }

		// Token: 0x17000243 RID: 579
		// (get) Token: 0x06000A50 RID: 2640 RVA: 0x0000E705 File Offset: 0x0000C905
		// (set) Token: 0x06000A51 RID: 2641 RVA: 0x0000E70D File Offset: 0x0000C90D
		public bool AllowJoinInProgress { get; set; }

		// Token: 0x17000244 RID: 580
		// (get) Token: 0x06000A52 RID: 2642 RVA: 0x0000E716 File Offset: 0x0000C916
		// (set) Token: 0x06000A53 RID: 2643 RVA: 0x0000E71E File Offset: 0x0000C91E
		public OnlineSessionPermissionLevel PermissionLevel { get; set; }

		// Token: 0x17000245 RID: 581
		// (get) Token: 0x06000A54 RID: 2644 RVA: 0x0000E727 File Offset: 0x0000C927
		// (set) Token: 0x06000A55 RID: 2645 RVA: 0x0000E72F File Offset: 0x0000C92F
		public bool InvitesAllowed { get; set; }

		// Token: 0x17000246 RID: 582
		// (get) Token: 0x06000A56 RID: 2646 RVA: 0x0000E738 File Offset: 0x0000C938
		// (set) Token: 0x06000A57 RID: 2647 RVA: 0x0000E740 File Offset: 0x0000C940
		public bool SanctionsEnabled { get; set; }

		// Token: 0x17000247 RID: 583
		// (get) Token: 0x06000A58 RID: 2648 RVA: 0x0000E749 File Offset: 0x0000C949
		// (set) Token: 0x06000A59 RID: 2649 RVA: 0x0000E751 File Offset: 0x0000C951
		public uint[] AllowedPlatformIds { get; set; }

		// Token: 0x06000A5A RID: 2650 RVA: 0x0000E75C File Offset: 0x0000C95C
		internal void Set(ref SessionDetailsSettingsInternal other)
		{
			this.BucketId = other.BucketId;
			this.NumPublicConnections = other.NumPublicConnections;
			this.AllowJoinInProgress = other.AllowJoinInProgress;
			this.PermissionLevel = other.PermissionLevel;
			this.InvitesAllowed = other.InvitesAllowed;
			this.SanctionsEnabled = other.SanctionsEnabled;
			this.AllowedPlatformIds = other.AllowedPlatformIds;
		}
	}
}
