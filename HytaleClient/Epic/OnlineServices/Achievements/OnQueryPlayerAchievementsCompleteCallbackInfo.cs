using System;

namespace Epic.OnlineServices.Achievements
{
	// Token: 0x02000750 RID: 1872
	public struct OnQueryPlayerAchievementsCompleteCallbackInfo : ICallbackInfo
	{
		// Token: 0x17000EA4 RID: 3748
		// (get) Token: 0x06003095 RID: 12437 RVA: 0x00048285 File Offset: 0x00046485
		// (set) Token: 0x06003096 RID: 12438 RVA: 0x0004828D File Offset: 0x0004648D
		public Result ResultCode { get; set; }

		// Token: 0x17000EA5 RID: 3749
		// (get) Token: 0x06003097 RID: 12439 RVA: 0x00048296 File Offset: 0x00046496
		// (set) Token: 0x06003098 RID: 12440 RVA: 0x0004829E File Offset: 0x0004649E
		public object ClientData { get; set; }

		// Token: 0x17000EA6 RID: 3750
		// (get) Token: 0x06003099 RID: 12441 RVA: 0x000482A7 File Offset: 0x000464A7
		// (set) Token: 0x0600309A RID: 12442 RVA: 0x000482AF File Offset: 0x000464AF
		public ProductUserId TargetUserId { get; set; }

		// Token: 0x17000EA7 RID: 3751
		// (get) Token: 0x0600309B RID: 12443 RVA: 0x000482B8 File Offset: 0x000464B8
		// (set) Token: 0x0600309C RID: 12444 RVA: 0x000482C0 File Offset: 0x000464C0
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x0600309D RID: 12445 RVA: 0x000482CC File Offset: 0x000464CC
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x0600309E RID: 12446 RVA: 0x000482E9 File Offset: 0x000464E9
		internal void Set(ref OnQueryPlayerAchievementsCompleteCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.TargetUserId = other.TargetUserId;
			this.LocalUserId = other.LocalUserId;
		}
	}
}
