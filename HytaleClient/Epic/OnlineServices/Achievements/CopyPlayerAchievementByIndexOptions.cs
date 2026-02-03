using System;

namespace Epic.OnlineServices.Achievements
{
	// Token: 0x02000732 RID: 1842
	public struct CopyPlayerAchievementByIndexOptions
	{
		// Token: 0x17000E53 RID: 3667
		// (get) Token: 0x06002FB1 RID: 12209 RVA: 0x00046DBB File Offset: 0x00044FBB
		// (set) Token: 0x06002FB2 RID: 12210 RVA: 0x00046DC3 File Offset: 0x00044FC3
		public ProductUserId TargetUserId { get; set; }

		// Token: 0x17000E54 RID: 3668
		// (get) Token: 0x06002FB3 RID: 12211 RVA: 0x00046DCC File Offset: 0x00044FCC
		// (set) Token: 0x06002FB4 RID: 12212 RVA: 0x00046DD4 File Offset: 0x00044FD4
		public uint AchievementIndex { get; set; }

		// Token: 0x17000E55 RID: 3669
		// (get) Token: 0x06002FB5 RID: 12213 RVA: 0x00046DDD File Offset: 0x00044FDD
		// (set) Token: 0x06002FB6 RID: 12214 RVA: 0x00046DE5 File Offset: 0x00044FE5
		public ProductUserId LocalUserId { get; set; }
	}
}
