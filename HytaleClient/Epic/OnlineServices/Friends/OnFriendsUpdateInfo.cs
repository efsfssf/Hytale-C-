using System;

namespace Epic.OnlineServices.Friends
{
	// Token: 0x020004EB RID: 1259
	public struct OnFriendsUpdateInfo : ICallbackInfo
	{
		// Token: 0x17000944 RID: 2372
		// (get) Token: 0x060020A5 RID: 8357 RVA: 0x0002FDF7 File Offset: 0x0002DFF7
		// (set) Token: 0x060020A6 RID: 8358 RVA: 0x0002FDFF File Offset: 0x0002DFFF
		public object ClientData { get; set; }

		// Token: 0x17000945 RID: 2373
		// (get) Token: 0x060020A7 RID: 8359 RVA: 0x0002FE08 File Offset: 0x0002E008
		// (set) Token: 0x060020A8 RID: 8360 RVA: 0x0002FE10 File Offset: 0x0002E010
		public EpicAccountId LocalUserId { get; set; }

		// Token: 0x17000946 RID: 2374
		// (get) Token: 0x060020A9 RID: 8361 RVA: 0x0002FE19 File Offset: 0x0002E019
		// (set) Token: 0x060020AA RID: 8362 RVA: 0x0002FE21 File Offset: 0x0002E021
		public EpicAccountId TargetUserId { get; set; }

		// Token: 0x17000947 RID: 2375
		// (get) Token: 0x060020AB RID: 8363 RVA: 0x0002FE2A File Offset: 0x0002E02A
		// (set) Token: 0x060020AC RID: 8364 RVA: 0x0002FE32 File Offset: 0x0002E032
		public FriendsStatus PreviousStatus { get; set; }

		// Token: 0x17000948 RID: 2376
		// (get) Token: 0x060020AD RID: 8365 RVA: 0x0002FE3B File Offset: 0x0002E03B
		// (set) Token: 0x060020AE RID: 8366 RVA: 0x0002FE43 File Offset: 0x0002E043
		public FriendsStatus CurrentStatus { get; set; }

		// Token: 0x060020AF RID: 8367 RVA: 0x0002FE4C File Offset: 0x0002E04C
		public Result? GetResultCode()
		{
			return null;
		}

		// Token: 0x060020B0 RID: 8368 RVA: 0x0002FE68 File Offset: 0x0002E068
		internal void Set(ref OnFriendsUpdateInfoInternal other)
		{
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.TargetUserId = other.TargetUserId;
			this.PreviousStatus = other.PreviousStatus;
			this.CurrentStatus = other.CurrentStatus;
		}
	}
}
