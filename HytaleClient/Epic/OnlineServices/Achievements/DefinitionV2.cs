using System;

namespace Epic.OnlineServices.Achievements
{
	// Token: 0x0200073A RID: 1850
	public struct DefinitionV2
	{
		// Token: 0x17000E77 RID: 3703
		// (get) Token: 0x06003000 RID: 12288 RVA: 0x00047640 File Offset: 0x00045840
		// (set) Token: 0x06003001 RID: 12289 RVA: 0x00047648 File Offset: 0x00045848
		public Utf8String AchievementId { get; set; }

		// Token: 0x17000E78 RID: 3704
		// (get) Token: 0x06003002 RID: 12290 RVA: 0x00047651 File Offset: 0x00045851
		// (set) Token: 0x06003003 RID: 12291 RVA: 0x00047659 File Offset: 0x00045859
		public Utf8String UnlockedDisplayName { get; set; }

		// Token: 0x17000E79 RID: 3705
		// (get) Token: 0x06003004 RID: 12292 RVA: 0x00047662 File Offset: 0x00045862
		// (set) Token: 0x06003005 RID: 12293 RVA: 0x0004766A File Offset: 0x0004586A
		public Utf8String UnlockedDescription { get; set; }

		// Token: 0x17000E7A RID: 3706
		// (get) Token: 0x06003006 RID: 12294 RVA: 0x00047673 File Offset: 0x00045873
		// (set) Token: 0x06003007 RID: 12295 RVA: 0x0004767B File Offset: 0x0004587B
		public Utf8String LockedDisplayName { get; set; }

		// Token: 0x17000E7B RID: 3707
		// (get) Token: 0x06003008 RID: 12296 RVA: 0x00047684 File Offset: 0x00045884
		// (set) Token: 0x06003009 RID: 12297 RVA: 0x0004768C File Offset: 0x0004588C
		public Utf8String LockedDescription { get; set; }

		// Token: 0x17000E7C RID: 3708
		// (get) Token: 0x0600300A RID: 12298 RVA: 0x00047695 File Offset: 0x00045895
		// (set) Token: 0x0600300B RID: 12299 RVA: 0x0004769D File Offset: 0x0004589D
		public Utf8String FlavorText { get; set; }

		// Token: 0x17000E7D RID: 3709
		// (get) Token: 0x0600300C RID: 12300 RVA: 0x000476A6 File Offset: 0x000458A6
		// (set) Token: 0x0600300D RID: 12301 RVA: 0x000476AE File Offset: 0x000458AE
		public Utf8String UnlockedIconURL { get; set; }

		// Token: 0x17000E7E RID: 3710
		// (get) Token: 0x0600300E RID: 12302 RVA: 0x000476B7 File Offset: 0x000458B7
		// (set) Token: 0x0600300F RID: 12303 RVA: 0x000476BF File Offset: 0x000458BF
		public Utf8String LockedIconURL { get; set; }

		// Token: 0x17000E7F RID: 3711
		// (get) Token: 0x06003010 RID: 12304 RVA: 0x000476C8 File Offset: 0x000458C8
		// (set) Token: 0x06003011 RID: 12305 RVA: 0x000476D0 File Offset: 0x000458D0
		public bool IsHidden { get; set; }

		// Token: 0x17000E80 RID: 3712
		// (get) Token: 0x06003012 RID: 12306 RVA: 0x000476D9 File Offset: 0x000458D9
		// (set) Token: 0x06003013 RID: 12307 RVA: 0x000476E1 File Offset: 0x000458E1
		public StatThresholds[] StatThresholds { get; set; }

		// Token: 0x06003014 RID: 12308 RVA: 0x000476EC File Offset: 0x000458EC
		internal void Set(ref DefinitionV2Internal other)
		{
			this.AchievementId = other.AchievementId;
			this.UnlockedDisplayName = other.UnlockedDisplayName;
			this.UnlockedDescription = other.UnlockedDescription;
			this.LockedDisplayName = other.LockedDisplayName;
			this.LockedDescription = other.LockedDescription;
			this.FlavorText = other.FlavorText;
			this.UnlockedIconURL = other.UnlockedIconURL;
			this.LockedIconURL = other.LockedIconURL;
			this.IsHidden = other.IsHidden;
			this.StatThresholds = other.StatThresholds;
		}
	}
}
