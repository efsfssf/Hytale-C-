using System;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x0200013E RID: 318
	public struct RegisterPlayersOptions
	{
		// Token: 0x17000208 RID: 520
		// (get) Token: 0x060009AE RID: 2478 RVA: 0x0000D6DF File Offset: 0x0000B8DF
		// (set) Token: 0x060009AF RID: 2479 RVA: 0x0000D6E7 File Offset: 0x0000B8E7
		public Utf8String SessionName { get; set; }

		// Token: 0x17000209 RID: 521
		// (get) Token: 0x060009B0 RID: 2480 RVA: 0x0000D6F0 File Offset: 0x0000B8F0
		// (set) Token: 0x060009B1 RID: 2481 RVA: 0x0000D6F8 File Offset: 0x0000B8F8
		public ProductUserId[] PlayersToRegister { get; set; }
	}
}
