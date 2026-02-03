using System;

namespace Epic.OnlineServices.Friends
{
	// Token: 0x020004E7 RID: 1255
	public struct OnBlockedUsersUpdateInfo : ICallbackInfo
	{
		// Token: 0x1700093B RID: 2363
		// (get) Token: 0x06002086 RID: 8326 RVA: 0x0002FB9A File Offset: 0x0002DD9A
		// (set) Token: 0x06002087 RID: 8327 RVA: 0x0002FBA2 File Offset: 0x0002DDA2
		public object ClientData { get; set; }

		// Token: 0x1700093C RID: 2364
		// (get) Token: 0x06002088 RID: 8328 RVA: 0x0002FBAB File Offset: 0x0002DDAB
		// (set) Token: 0x06002089 RID: 8329 RVA: 0x0002FBB3 File Offset: 0x0002DDB3
		public EpicAccountId LocalUserId { get; set; }

		// Token: 0x1700093D RID: 2365
		// (get) Token: 0x0600208A RID: 8330 RVA: 0x0002FBBC File Offset: 0x0002DDBC
		// (set) Token: 0x0600208B RID: 8331 RVA: 0x0002FBC4 File Offset: 0x0002DDC4
		public EpicAccountId TargetUserId { get; set; }

		// Token: 0x1700093E RID: 2366
		// (get) Token: 0x0600208C RID: 8332 RVA: 0x0002FBCD File Offset: 0x0002DDCD
		// (set) Token: 0x0600208D RID: 8333 RVA: 0x0002FBD5 File Offset: 0x0002DDD5
		public bool Blocked { get; set; }

		// Token: 0x0600208E RID: 8334 RVA: 0x0002FBE0 File Offset: 0x0002DDE0
		public Result? GetResultCode()
		{
			return null;
		}

		// Token: 0x0600208F RID: 8335 RVA: 0x0002FBFB File Offset: 0x0002DDFB
		internal void Set(ref OnBlockedUsersUpdateInfoInternal other)
		{
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.TargetUserId = other.TargetUserId;
			this.Blocked = other.Blocked;
		}
	}
}
