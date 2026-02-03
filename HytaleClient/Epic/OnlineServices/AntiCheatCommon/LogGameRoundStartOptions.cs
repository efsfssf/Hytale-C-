using System;

namespace Epic.OnlineServices.AntiCheatCommon
{
	// Token: 0x020006AB RID: 1707
	public struct LogGameRoundStartOptions
	{
		// Token: 0x17000CEE RID: 3310
		// (get) Token: 0x06002C00 RID: 11264 RVA: 0x00041061 File Offset: 0x0003F261
		// (set) Token: 0x06002C01 RID: 11265 RVA: 0x00041069 File Offset: 0x0003F269
		public Utf8String SessionIdentifier { get; set; }

		// Token: 0x17000CEF RID: 3311
		// (get) Token: 0x06002C02 RID: 11266 RVA: 0x00041072 File Offset: 0x0003F272
		// (set) Token: 0x06002C03 RID: 11267 RVA: 0x0004107A File Offset: 0x0003F27A
		public Utf8String LevelName { get; set; }

		// Token: 0x17000CF0 RID: 3312
		// (get) Token: 0x06002C04 RID: 11268 RVA: 0x00041083 File Offset: 0x0003F283
		// (set) Token: 0x06002C05 RID: 11269 RVA: 0x0004108B File Offset: 0x0003F28B
		public Utf8String ModeName { get; set; }

		// Token: 0x17000CF1 RID: 3313
		// (get) Token: 0x06002C06 RID: 11270 RVA: 0x00041094 File Offset: 0x0003F294
		// (set) Token: 0x06002C07 RID: 11271 RVA: 0x0004109C File Offset: 0x0003F29C
		public uint RoundTimeSeconds { get; set; }

		// Token: 0x17000CF2 RID: 3314
		// (get) Token: 0x06002C08 RID: 11272 RVA: 0x000410A5 File Offset: 0x0003F2A5
		// (set) Token: 0x06002C09 RID: 11273 RVA: 0x000410AD File Offset: 0x0003F2AD
		public AntiCheatCommonGameRoundCompetitionType CompetitionType { get; set; }
	}
}
