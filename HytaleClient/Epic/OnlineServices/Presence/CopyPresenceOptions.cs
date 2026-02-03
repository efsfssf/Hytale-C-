using System;

namespace Epic.OnlineServices.Presence
{
	// Token: 0x020002BC RID: 700
	public struct CopyPresenceOptions
	{
		// Token: 0x17000523 RID: 1315
		// (get) Token: 0x06001357 RID: 4951 RVA: 0x0001C22C File Offset: 0x0001A42C
		// (set) Token: 0x06001358 RID: 4952 RVA: 0x0001C234 File Offset: 0x0001A434
		public EpicAccountId LocalUserId { get; set; }

		// Token: 0x17000524 RID: 1316
		// (get) Token: 0x06001359 RID: 4953 RVA: 0x0001C23D File Offset: 0x0001A43D
		// (set) Token: 0x0600135A RID: 4954 RVA: 0x0001C245 File Offset: 0x0001A445
		public EpicAccountId TargetUserId { get; set; }
	}
}
