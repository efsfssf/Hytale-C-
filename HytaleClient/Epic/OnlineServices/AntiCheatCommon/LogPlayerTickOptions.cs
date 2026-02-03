using System;

namespace Epic.OnlineServices.AntiCheatCommon
{
	// Token: 0x020006B5 RID: 1717
	public struct LogPlayerTickOptions
	{
		// Token: 0x17000D2A RID: 3370
		// (get) Token: 0x06002C69 RID: 11369 RVA: 0x000419AE File Offset: 0x0003FBAE
		// (set) Token: 0x06002C6A RID: 11370 RVA: 0x000419B6 File Offset: 0x0003FBB6
		public IntPtr PlayerHandle { get; set; }

		// Token: 0x17000D2B RID: 3371
		// (get) Token: 0x06002C6B RID: 11371 RVA: 0x000419BF File Offset: 0x0003FBBF
		// (set) Token: 0x06002C6C RID: 11372 RVA: 0x000419C7 File Offset: 0x0003FBC7
		public Vec3f? PlayerPosition { get; set; }

		// Token: 0x17000D2C RID: 3372
		// (get) Token: 0x06002C6D RID: 11373 RVA: 0x000419D0 File Offset: 0x0003FBD0
		// (set) Token: 0x06002C6E RID: 11374 RVA: 0x000419D8 File Offset: 0x0003FBD8
		public Quat? PlayerViewRotation { get; set; }

		// Token: 0x17000D2D RID: 3373
		// (get) Token: 0x06002C6F RID: 11375 RVA: 0x000419E1 File Offset: 0x0003FBE1
		// (set) Token: 0x06002C70 RID: 11376 RVA: 0x000419E9 File Offset: 0x0003FBE9
		public bool IsPlayerViewZoomed { get; set; }

		// Token: 0x17000D2E RID: 3374
		// (get) Token: 0x06002C71 RID: 11377 RVA: 0x000419F2 File Offset: 0x0003FBF2
		// (set) Token: 0x06002C72 RID: 11378 RVA: 0x000419FA File Offset: 0x0003FBFA
		public float PlayerHealth { get; set; }

		// Token: 0x17000D2F RID: 3375
		// (get) Token: 0x06002C73 RID: 11379 RVA: 0x00041A03 File Offset: 0x0003FC03
		// (set) Token: 0x06002C74 RID: 11380 RVA: 0x00041A0B File Offset: 0x0003FC0B
		public AntiCheatCommonPlayerMovementState PlayerMovementState { get; set; }

		// Token: 0x17000D30 RID: 3376
		// (get) Token: 0x06002C75 RID: 11381 RVA: 0x00041A14 File Offset: 0x0003FC14
		// (set) Token: 0x06002C76 RID: 11382 RVA: 0x00041A1C File Offset: 0x0003FC1C
		public Vec3f? PlayerViewPosition { get; set; }
	}
}
