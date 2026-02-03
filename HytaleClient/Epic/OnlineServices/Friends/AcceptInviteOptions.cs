using System;

namespace Epic.OnlineServices.Friends
{
	// Token: 0x020004D1 RID: 1233
	public struct AcceptInviteOptions
	{
		// Token: 0x17000927 RID: 2343
		// (get) Token: 0x0600202B RID: 8235 RVA: 0x0002F273 File Offset: 0x0002D473
		// (set) Token: 0x0600202C RID: 8236 RVA: 0x0002F27B File Offset: 0x0002D47B
		public EpicAccountId LocalUserId { get; set; }

		// Token: 0x17000928 RID: 2344
		// (get) Token: 0x0600202D RID: 8237 RVA: 0x0002F284 File Offset: 0x0002D484
		// (set) Token: 0x0600202E RID: 8238 RVA: 0x0002F28C File Offset: 0x0002D48C
		public EpicAccountId TargetUserId { get; set; }
	}
}
