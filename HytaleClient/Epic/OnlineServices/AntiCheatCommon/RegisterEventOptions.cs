using System;

namespace Epic.OnlineServices.AntiCheatCommon
{
	// Token: 0x020006C5 RID: 1733
	public struct RegisterEventOptions
	{
		// Token: 0x17000D6F RID: 3439
		// (get) Token: 0x06002D09 RID: 11529 RVA: 0x000428E3 File Offset: 0x00040AE3
		// (set) Token: 0x06002D0A RID: 11530 RVA: 0x000428EB File Offset: 0x00040AEB
		public uint EventId { get; set; }

		// Token: 0x17000D70 RID: 3440
		// (get) Token: 0x06002D0B RID: 11531 RVA: 0x000428F4 File Offset: 0x00040AF4
		// (set) Token: 0x06002D0C RID: 11532 RVA: 0x000428FC File Offset: 0x00040AFC
		public Utf8String EventName { get; set; }

		// Token: 0x17000D71 RID: 3441
		// (get) Token: 0x06002D0D RID: 11533 RVA: 0x00042905 File Offset: 0x00040B05
		// (set) Token: 0x06002D0E RID: 11534 RVA: 0x0004290D File Offset: 0x00040B0D
		public AntiCheatCommonEventType EventType { get; set; }

		// Token: 0x17000D72 RID: 3442
		// (get) Token: 0x06002D0F RID: 11535 RVA: 0x00042916 File Offset: 0x00040B16
		// (set) Token: 0x06002D10 RID: 11536 RVA: 0x0004291E File Offset: 0x00040B1E
		public RegisterEventParamDef[] ParamDefs { get; set; }
	}
}
