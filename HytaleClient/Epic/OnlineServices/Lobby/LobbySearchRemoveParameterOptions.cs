using System;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x020003FB RID: 1019
	public struct LobbySearchRemoveParameterOptions
	{
		// Token: 0x170007C9 RID: 1993
		// (get) Token: 0x06001B5C RID: 7004 RVA: 0x00029164 File Offset: 0x00027364
		// (set) Token: 0x06001B5D RID: 7005 RVA: 0x0002916C File Offset: 0x0002736C
		public Utf8String Key { get; set; }

		// Token: 0x170007CA RID: 1994
		// (get) Token: 0x06001B5E RID: 7006 RVA: 0x00029175 File Offset: 0x00027375
		// (set) Token: 0x06001B5F RID: 7007 RVA: 0x0002917D File Offset: 0x0002737D
		public ComparisonOp ComparisonOp { get; set; }
	}
}
