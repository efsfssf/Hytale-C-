using System;

namespace Epic.OnlineServices.Friends
{
	// Token: 0x020004E1 RID: 1249
	public struct GetStatusOptions
	{
		// Token: 0x17000937 RID: 2359
		// (get) Token: 0x0600206D RID: 8301 RVA: 0x0002FACD File Offset: 0x0002DCCD
		// (set) Token: 0x0600206E RID: 8302 RVA: 0x0002FAD5 File Offset: 0x0002DCD5
		public EpicAccountId LocalUserId { get; set; }

		// Token: 0x17000938 RID: 2360
		// (get) Token: 0x0600206F RID: 8303 RVA: 0x0002FADE File Offset: 0x0002DCDE
		// (set) Token: 0x06002070 RID: 8304 RVA: 0x0002FAE6 File Offset: 0x0002DCE6
		public EpicAccountId TargetUserId { get; set; }
	}
}
