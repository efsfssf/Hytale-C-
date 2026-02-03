using System;

namespace Epic.OnlineServices.RTC
{
	// Token: 0x020001B3 RID: 435
	public struct BlockParticipantOptions
	{
		// Token: 0x170002F9 RID: 761
		// (get) Token: 0x06000C92 RID: 3218 RVA: 0x00012674 File Offset: 0x00010874
		// (set) Token: 0x06000C93 RID: 3219 RVA: 0x0001267C File Offset: 0x0001087C
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x170002FA RID: 762
		// (get) Token: 0x06000C94 RID: 3220 RVA: 0x00012685 File Offset: 0x00010885
		// (set) Token: 0x06000C95 RID: 3221 RVA: 0x0001268D File Offset: 0x0001088D
		public Utf8String RoomName { get; set; }

		// Token: 0x170002FB RID: 763
		// (get) Token: 0x06000C96 RID: 3222 RVA: 0x00012696 File Offset: 0x00010896
		// (set) Token: 0x06000C97 RID: 3223 RVA: 0x0001269E File Offset: 0x0001089E
		public ProductUserId ParticipantId { get; set; }

		// Token: 0x170002FC RID: 764
		// (get) Token: 0x06000C98 RID: 3224 RVA: 0x000126A7 File Offset: 0x000108A7
		// (set) Token: 0x06000C99 RID: 3225 RVA: 0x000126AF File Offset: 0x000108AF
		public bool Blocked { get; set; }
	}
}
