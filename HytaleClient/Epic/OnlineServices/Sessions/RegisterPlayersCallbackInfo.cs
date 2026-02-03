using System;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x0200013C RID: 316
	public struct RegisterPlayersCallbackInfo : ICallbackInfo
	{
		// Token: 0x170001FF RID: 511
		// (get) Token: 0x06000997 RID: 2455 RVA: 0x0000D479 File Offset: 0x0000B679
		// (set) Token: 0x06000998 RID: 2456 RVA: 0x0000D481 File Offset: 0x0000B681
		public Result ResultCode { get; set; }

		// Token: 0x17000200 RID: 512
		// (get) Token: 0x06000999 RID: 2457 RVA: 0x0000D48A File Offset: 0x0000B68A
		// (set) Token: 0x0600099A RID: 2458 RVA: 0x0000D492 File Offset: 0x0000B692
		public object ClientData { get; set; }

		// Token: 0x17000201 RID: 513
		// (get) Token: 0x0600099B RID: 2459 RVA: 0x0000D49B File Offset: 0x0000B69B
		// (set) Token: 0x0600099C RID: 2460 RVA: 0x0000D4A3 File Offset: 0x0000B6A3
		public ProductUserId[] RegisteredPlayers { get; set; }

		// Token: 0x17000202 RID: 514
		// (get) Token: 0x0600099D RID: 2461 RVA: 0x0000D4AC File Offset: 0x0000B6AC
		// (set) Token: 0x0600099E RID: 2462 RVA: 0x0000D4B4 File Offset: 0x0000B6B4
		public ProductUserId[] SanctionedPlayers { get; set; }

		// Token: 0x0600099F RID: 2463 RVA: 0x0000D4C0 File Offset: 0x0000B6C0
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x060009A0 RID: 2464 RVA: 0x0000D4DD File Offset: 0x0000B6DD
		internal void Set(ref RegisterPlayersCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.RegisteredPlayers = other.RegisteredPlayers;
			this.SanctionedPlayers = other.SanctionedPlayers;
		}
	}
}
