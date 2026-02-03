using System;

namespace Epic.OnlineServices.AntiCheatCommon
{
	// Token: 0x020006BF RID: 1727
	public struct OnClientAuthStatusChangedCallbackInfo : ICallbackInfo
	{
		// Token: 0x17000D59 RID: 3417
		// (get) Token: 0x06002CCE RID: 11470 RVA: 0x00042374 File Offset: 0x00040574
		// (set) Token: 0x06002CCF RID: 11471 RVA: 0x0004237C File Offset: 0x0004057C
		public object ClientData { get; set; }

		// Token: 0x17000D5A RID: 3418
		// (get) Token: 0x06002CD0 RID: 11472 RVA: 0x00042385 File Offset: 0x00040585
		// (set) Token: 0x06002CD1 RID: 11473 RVA: 0x0004238D File Offset: 0x0004058D
		public IntPtr ClientHandle { get; set; }

		// Token: 0x17000D5B RID: 3419
		// (get) Token: 0x06002CD2 RID: 11474 RVA: 0x00042396 File Offset: 0x00040596
		// (set) Token: 0x06002CD3 RID: 11475 RVA: 0x0004239E File Offset: 0x0004059E
		public AntiCheatCommonClientAuthStatus ClientAuthStatus { get; set; }

		// Token: 0x06002CD4 RID: 11476 RVA: 0x000423A8 File Offset: 0x000405A8
		public Result? GetResultCode()
		{
			return null;
		}

		// Token: 0x06002CD5 RID: 11477 RVA: 0x000423C3 File Offset: 0x000405C3
		internal void Set(ref OnClientAuthStatusChangedCallbackInfoInternal other)
		{
			this.ClientData = other.ClientData;
			this.ClientHandle = other.ClientHandle;
			this.ClientAuthStatus = other.ClientAuthStatus;
		}
	}
}
