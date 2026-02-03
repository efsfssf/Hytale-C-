using System;

namespace Epic.OnlineServices.Achievements
{
	// Token: 0x02000754 RID: 1876
	public struct OnUnlockAchievementsCompleteCallbackInfo : ICallbackInfo
	{
		// Token: 0x17000EAD RID: 3757
		// (get) Token: 0x060030B4 RID: 12468 RVA: 0x000484D3 File Offset: 0x000466D3
		// (set) Token: 0x060030B5 RID: 12469 RVA: 0x000484DB File Offset: 0x000466DB
		public Result ResultCode { get; set; }

		// Token: 0x17000EAE RID: 3758
		// (get) Token: 0x060030B6 RID: 12470 RVA: 0x000484E4 File Offset: 0x000466E4
		// (set) Token: 0x060030B7 RID: 12471 RVA: 0x000484EC File Offset: 0x000466EC
		public object ClientData { get; set; }

		// Token: 0x17000EAF RID: 3759
		// (get) Token: 0x060030B8 RID: 12472 RVA: 0x000484F5 File Offset: 0x000466F5
		// (set) Token: 0x060030B9 RID: 12473 RVA: 0x000484FD File Offset: 0x000466FD
		public ProductUserId UserId { get; set; }

		// Token: 0x17000EB0 RID: 3760
		// (get) Token: 0x060030BA RID: 12474 RVA: 0x00048506 File Offset: 0x00046706
		// (set) Token: 0x060030BB RID: 12475 RVA: 0x0004850E File Offset: 0x0004670E
		public uint AchievementsCount { get; set; }

		// Token: 0x060030BC RID: 12476 RVA: 0x00048518 File Offset: 0x00046718
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x060030BD RID: 12477 RVA: 0x00048535 File Offset: 0x00046735
		internal void Set(ref OnUnlockAchievementsCompleteCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.UserId = other.UserId;
			this.AchievementsCount = other.AchievementsCount;
		}
	}
}
