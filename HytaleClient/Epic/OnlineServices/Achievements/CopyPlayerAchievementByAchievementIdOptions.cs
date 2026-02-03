using System;

namespace Epic.OnlineServices.Achievements
{
	// Token: 0x02000730 RID: 1840
	public struct CopyPlayerAchievementByAchievementIdOptions
	{
		// Token: 0x17000E4D RID: 3661
		// (get) Token: 0x06002FA5 RID: 12197 RVA: 0x00046C9D File Offset: 0x00044E9D
		// (set) Token: 0x06002FA6 RID: 12198 RVA: 0x00046CA5 File Offset: 0x00044EA5
		public ProductUserId TargetUserId { get; set; }

		// Token: 0x17000E4E RID: 3662
		// (get) Token: 0x06002FA7 RID: 12199 RVA: 0x00046CAE File Offset: 0x00044EAE
		// (set) Token: 0x06002FA8 RID: 12200 RVA: 0x00046CB6 File Offset: 0x00044EB6
		public Utf8String AchievementId { get; set; }

		// Token: 0x17000E4F RID: 3663
		// (get) Token: 0x06002FA9 RID: 12201 RVA: 0x00046CBF File Offset: 0x00044EBF
		// (set) Token: 0x06002FAA RID: 12202 RVA: 0x00046CC7 File Offset: 0x00044EC7
		public ProductUserId LocalUserId { get; set; }
	}
}
