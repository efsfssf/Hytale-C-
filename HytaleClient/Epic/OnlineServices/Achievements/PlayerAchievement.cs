using System;

namespace Epic.OnlineServices.Achievements
{
	// Token: 0x02000756 RID: 1878
	public struct PlayerAchievement
	{
		// Token: 0x17000EB6 RID: 3766
		// (get) Token: 0x060030CB RID: 12491 RVA: 0x00048707 File Offset: 0x00046907
		// (set) Token: 0x060030CC RID: 12492 RVA: 0x0004870F File Offset: 0x0004690F
		public Utf8String AchievementId { get; set; }

		// Token: 0x17000EB7 RID: 3767
		// (get) Token: 0x060030CD RID: 12493 RVA: 0x00048718 File Offset: 0x00046918
		// (set) Token: 0x060030CE RID: 12494 RVA: 0x00048720 File Offset: 0x00046920
		public double Progress { get; set; }

		// Token: 0x17000EB8 RID: 3768
		// (get) Token: 0x060030CF RID: 12495 RVA: 0x00048729 File Offset: 0x00046929
		// (set) Token: 0x060030D0 RID: 12496 RVA: 0x00048731 File Offset: 0x00046931
		public DateTimeOffset? UnlockTime { get; set; }

		// Token: 0x17000EB9 RID: 3769
		// (get) Token: 0x060030D1 RID: 12497 RVA: 0x0004873A File Offset: 0x0004693A
		// (set) Token: 0x060030D2 RID: 12498 RVA: 0x00048742 File Offset: 0x00046942
		public PlayerStatInfo[] StatInfo { get; set; }

		// Token: 0x17000EBA RID: 3770
		// (get) Token: 0x060030D3 RID: 12499 RVA: 0x0004874B File Offset: 0x0004694B
		// (set) Token: 0x060030D4 RID: 12500 RVA: 0x00048753 File Offset: 0x00046953
		public Utf8String DisplayName { get; set; }

		// Token: 0x17000EBB RID: 3771
		// (get) Token: 0x060030D5 RID: 12501 RVA: 0x0004875C File Offset: 0x0004695C
		// (set) Token: 0x060030D6 RID: 12502 RVA: 0x00048764 File Offset: 0x00046964
		public Utf8String Description { get; set; }

		// Token: 0x17000EBC RID: 3772
		// (get) Token: 0x060030D7 RID: 12503 RVA: 0x0004876D File Offset: 0x0004696D
		// (set) Token: 0x060030D8 RID: 12504 RVA: 0x00048775 File Offset: 0x00046975
		public Utf8String IconURL { get; set; }

		// Token: 0x17000EBD RID: 3773
		// (get) Token: 0x060030D9 RID: 12505 RVA: 0x0004877E File Offset: 0x0004697E
		// (set) Token: 0x060030DA RID: 12506 RVA: 0x00048786 File Offset: 0x00046986
		public Utf8String FlavorText { get; set; }

		// Token: 0x060030DB RID: 12507 RVA: 0x00048790 File Offset: 0x00046990
		internal void Set(ref PlayerAchievementInternal other)
		{
			this.AchievementId = other.AchievementId;
			this.Progress = other.Progress;
			this.UnlockTime = other.UnlockTime;
			this.StatInfo = other.StatInfo;
			this.DisplayName = other.DisplayName;
			this.Description = other.Description;
			this.IconURL = other.IconURL;
			this.FlavorText = other.FlavorText;
		}
	}
}
