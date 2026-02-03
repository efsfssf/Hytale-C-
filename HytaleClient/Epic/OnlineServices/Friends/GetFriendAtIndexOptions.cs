using System;

namespace Epic.OnlineServices.Friends
{
	// Token: 0x020004DD RID: 1245
	public struct GetFriendAtIndexOptions
	{
		// Token: 0x17000931 RID: 2353
		// (get) Token: 0x0600205E RID: 8286 RVA: 0x0002F991 File Offset: 0x0002DB91
		// (set) Token: 0x0600205F RID: 8287 RVA: 0x0002F999 File Offset: 0x0002DB99
		public EpicAccountId LocalUserId { get; set; }

		// Token: 0x17000932 RID: 2354
		// (get) Token: 0x06002060 RID: 8288 RVA: 0x0002F9A2 File Offset: 0x0002DBA2
		// (set) Token: 0x06002061 RID: 8289 RVA: 0x0002F9AA File Offset: 0x0002DBAA
		public int Index { get; set; }
	}
}
