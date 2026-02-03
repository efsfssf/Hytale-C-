using System;

namespace Epic.OnlineServices.Presence
{
	// Token: 0x020002C2 RID: 706
	public struct GetJoinInfoOptions
	{
		// Token: 0x1700052D RID: 1325
		// (get) Token: 0x06001373 RID: 4979 RVA: 0x0001C4BC File Offset: 0x0001A6BC
		// (set) Token: 0x06001374 RID: 4980 RVA: 0x0001C4C4 File Offset: 0x0001A6C4
		public EpicAccountId LocalUserId { get; set; }

		// Token: 0x1700052E RID: 1326
		// (get) Token: 0x06001375 RID: 4981 RVA: 0x0001C4CD File Offset: 0x0001A6CD
		// (set) Token: 0x06001376 RID: 4982 RVA: 0x0001C4D5 File Offset: 0x0001A6D5
		public EpicAccountId TargetUserId { get; set; }
	}
}
