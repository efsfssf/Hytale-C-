using System;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x02000401 RID: 1025
	public struct LobbySearchSetParameterOptions
	{
		// Token: 0x170007D1 RID: 2001
		// (get) Token: 0x06001B71 RID: 7025 RVA: 0x00029309 File Offset: 0x00027509
		// (set) Token: 0x06001B72 RID: 7026 RVA: 0x00029311 File Offset: 0x00027511
		public AttributeData? Parameter { get; set; }

		// Token: 0x170007D2 RID: 2002
		// (get) Token: 0x06001B73 RID: 7027 RVA: 0x0002931A File Offset: 0x0002751A
		// (set) Token: 0x06001B74 RID: 7028 RVA: 0x00029322 File Offset: 0x00027522
		public ComparisonOp ComparisonOp { get; set; }
	}
}
