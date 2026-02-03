using System;

namespace Epic.OnlineServices.AntiCheatCommon
{
	// Token: 0x020006C9 RID: 1737
	public struct SetClientDetailsOptions
	{
		// Token: 0x17000D7B RID: 3451
		// (get) Token: 0x06002D25 RID: 11557 RVA: 0x00042B49 File Offset: 0x00040D49
		// (set) Token: 0x06002D26 RID: 11558 RVA: 0x00042B51 File Offset: 0x00040D51
		public IntPtr ClientHandle { get; set; }

		// Token: 0x17000D7C RID: 3452
		// (get) Token: 0x06002D27 RID: 11559 RVA: 0x00042B5A File Offset: 0x00040D5A
		// (set) Token: 0x06002D28 RID: 11560 RVA: 0x00042B62 File Offset: 0x00040D62
		public AntiCheatCommonClientFlags ClientFlags { get; set; }

		// Token: 0x17000D7D RID: 3453
		// (get) Token: 0x06002D29 RID: 11561 RVA: 0x00042B6B File Offset: 0x00040D6B
		// (set) Token: 0x06002D2A RID: 11562 RVA: 0x00042B73 File Offset: 0x00040D73
		public AntiCheatCommonClientInput ClientInputMethod { get; set; }
	}
}
