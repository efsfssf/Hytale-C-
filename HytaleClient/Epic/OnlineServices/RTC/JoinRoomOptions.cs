using System;

namespace Epic.OnlineServices.RTC
{
	// Token: 0x020001BA RID: 442
	public struct JoinRoomOptions
	{
		// Token: 0x17000315 RID: 789
		// (get) Token: 0x06000CD3 RID: 3283 RVA: 0x00012D14 File Offset: 0x00010F14
		// (set) Token: 0x06000CD4 RID: 3284 RVA: 0x00012D1C File Offset: 0x00010F1C
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x17000316 RID: 790
		// (get) Token: 0x06000CD5 RID: 3285 RVA: 0x00012D25 File Offset: 0x00010F25
		// (set) Token: 0x06000CD6 RID: 3286 RVA: 0x00012D2D File Offset: 0x00010F2D
		public Utf8String RoomName { get; set; }

		// Token: 0x17000317 RID: 791
		// (get) Token: 0x06000CD7 RID: 3287 RVA: 0x00012D36 File Offset: 0x00010F36
		// (set) Token: 0x06000CD8 RID: 3288 RVA: 0x00012D3E File Offset: 0x00010F3E
		public Utf8String ClientBaseUrl { get; set; }

		// Token: 0x17000318 RID: 792
		// (get) Token: 0x06000CD9 RID: 3289 RVA: 0x00012D47 File Offset: 0x00010F47
		// (set) Token: 0x06000CDA RID: 3290 RVA: 0x00012D4F File Offset: 0x00010F4F
		public Utf8String ParticipantToken { get; set; }

		// Token: 0x17000319 RID: 793
		// (get) Token: 0x06000CDB RID: 3291 RVA: 0x00012D58 File Offset: 0x00010F58
		// (set) Token: 0x06000CDC RID: 3292 RVA: 0x00012D60 File Offset: 0x00010F60
		public ProductUserId ParticipantId { get; set; }

		// Token: 0x1700031A RID: 794
		// (get) Token: 0x06000CDD RID: 3293 RVA: 0x00012D69 File Offset: 0x00010F69
		// (set) Token: 0x06000CDE RID: 3294 RVA: 0x00012D71 File Offset: 0x00010F71
		public JoinRoomFlags Flags { get; set; }

		// Token: 0x1700031B RID: 795
		// (get) Token: 0x06000CDF RID: 3295 RVA: 0x00012D7A File Offset: 0x00010F7A
		// (set) Token: 0x06000CE0 RID: 3296 RVA: 0x00012D82 File Offset: 0x00010F82
		public bool ManualAudioInputEnabled { get; set; }

		// Token: 0x1700031C RID: 796
		// (get) Token: 0x06000CE1 RID: 3297 RVA: 0x00012D8B File Offset: 0x00010F8B
		// (set) Token: 0x06000CE2 RID: 3298 RVA: 0x00012D93 File Offset: 0x00010F93
		public bool ManualAudioOutputEnabled { get; set; }
	}
}
