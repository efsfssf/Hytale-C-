using System;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x0200018F RID: 399
	public struct UnregisterPlayersOptions
	{
		// Token: 0x170002A7 RID: 679
		// (get) Token: 0x06000BB1 RID: 2993 RVA: 0x0001119E File Offset: 0x0000F39E
		// (set) Token: 0x06000BB2 RID: 2994 RVA: 0x000111A6 File Offset: 0x0000F3A6
		public Utf8String SessionName { get; set; }

		// Token: 0x170002A8 RID: 680
		// (get) Token: 0x06000BB3 RID: 2995 RVA: 0x000111AF File Offset: 0x0000F3AF
		// (set) Token: 0x06000BB4 RID: 2996 RVA: 0x000111B7 File Offset: 0x0000F3B7
		public ProductUserId[] PlayersToUnregister { get; set; }
	}
}
