using System;

namespace Epic.OnlineServices.Achievements
{
	// Token: 0x02000738 RID: 1848
	public struct Definition
	{
		// Token: 0x17000E61 RID: 3681
		// (get) Token: 0x06002FCF RID: 12239 RVA: 0x00047052 File Offset: 0x00045252
		// (set) Token: 0x06002FD0 RID: 12240 RVA: 0x0004705A File Offset: 0x0004525A
		public Utf8String AchievementId { get; set; }

		// Token: 0x17000E62 RID: 3682
		// (get) Token: 0x06002FD1 RID: 12241 RVA: 0x00047063 File Offset: 0x00045263
		// (set) Token: 0x06002FD2 RID: 12242 RVA: 0x0004706B File Offset: 0x0004526B
		public Utf8String DisplayName { get; set; }

		// Token: 0x17000E63 RID: 3683
		// (get) Token: 0x06002FD3 RID: 12243 RVA: 0x00047074 File Offset: 0x00045274
		// (set) Token: 0x06002FD4 RID: 12244 RVA: 0x0004707C File Offset: 0x0004527C
		public Utf8String Description { get; set; }

		// Token: 0x17000E64 RID: 3684
		// (get) Token: 0x06002FD5 RID: 12245 RVA: 0x00047085 File Offset: 0x00045285
		// (set) Token: 0x06002FD6 RID: 12246 RVA: 0x0004708D File Offset: 0x0004528D
		public Utf8String LockedDisplayName { get; set; }

		// Token: 0x17000E65 RID: 3685
		// (get) Token: 0x06002FD7 RID: 12247 RVA: 0x00047096 File Offset: 0x00045296
		// (set) Token: 0x06002FD8 RID: 12248 RVA: 0x0004709E File Offset: 0x0004529E
		public Utf8String LockedDescription { get; set; }

		// Token: 0x17000E66 RID: 3686
		// (get) Token: 0x06002FD9 RID: 12249 RVA: 0x000470A7 File Offset: 0x000452A7
		// (set) Token: 0x06002FDA RID: 12250 RVA: 0x000470AF File Offset: 0x000452AF
		public Utf8String HiddenDescription { get; set; }

		// Token: 0x17000E67 RID: 3687
		// (get) Token: 0x06002FDB RID: 12251 RVA: 0x000470B8 File Offset: 0x000452B8
		// (set) Token: 0x06002FDC RID: 12252 RVA: 0x000470C0 File Offset: 0x000452C0
		public Utf8String CompletionDescription { get; set; }

		// Token: 0x17000E68 RID: 3688
		// (get) Token: 0x06002FDD RID: 12253 RVA: 0x000470C9 File Offset: 0x000452C9
		// (set) Token: 0x06002FDE RID: 12254 RVA: 0x000470D1 File Offset: 0x000452D1
		public Utf8String UnlockedIconId { get; set; }

		// Token: 0x17000E69 RID: 3689
		// (get) Token: 0x06002FDF RID: 12255 RVA: 0x000470DA File Offset: 0x000452DA
		// (set) Token: 0x06002FE0 RID: 12256 RVA: 0x000470E2 File Offset: 0x000452E2
		public Utf8String LockedIconId { get; set; }

		// Token: 0x17000E6A RID: 3690
		// (get) Token: 0x06002FE1 RID: 12257 RVA: 0x000470EB File Offset: 0x000452EB
		// (set) Token: 0x06002FE2 RID: 12258 RVA: 0x000470F3 File Offset: 0x000452F3
		public bool IsHidden { get; set; }

		// Token: 0x17000E6B RID: 3691
		// (get) Token: 0x06002FE3 RID: 12259 RVA: 0x000470FC File Offset: 0x000452FC
		// (set) Token: 0x06002FE4 RID: 12260 RVA: 0x00047104 File Offset: 0x00045304
		public StatThresholds[] StatThresholds { get; set; }

		// Token: 0x06002FE5 RID: 12261 RVA: 0x00047110 File Offset: 0x00045310
		internal void Set(ref DefinitionInternal other)
		{
			this.AchievementId = other.AchievementId;
			this.DisplayName = other.DisplayName;
			this.Description = other.Description;
			this.LockedDisplayName = other.LockedDisplayName;
			this.LockedDescription = other.LockedDescription;
			this.HiddenDescription = other.HiddenDescription;
			this.CompletionDescription = other.CompletionDescription;
			this.UnlockedIconId = other.UnlockedIconId;
			this.LockedIconId = other.LockedIconId;
			this.IsHidden = other.IsHidden;
			this.StatThresholds = other.StatThresholds;
		}
	}
}
