using System;

namespace Epic.OnlineServices.Presence
{
	// Token: 0x020002C4 RID: 708
	public struct HasPresenceOptions
	{
		// Token: 0x17000531 RID: 1329
		// (get) Token: 0x0600137C RID: 4988 RVA: 0x0001C58A File Offset: 0x0001A78A
		// (set) Token: 0x0600137D RID: 4989 RVA: 0x0001C592 File Offset: 0x0001A792
		public EpicAccountId LocalUserId { get; set; }

		// Token: 0x17000532 RID: 1330
		// (get) Token: 0x0600137E RID: 4990 RVA: 0x0001C59B File Offset: 0x0001A79B
		// (set) Token: 0x0600137F RID: 4991 RVA: 0x0001C5A3 File Offset: 0x0001A7A3
		public EpicAccountId TargetUserId { get; set; }
	}
}
