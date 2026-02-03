using System;

namespace Epic.OnlineServices.Presence
{
	// Token: 0x020002E8 RID: 744
	public struct SetPresenceOptions
	{
		// Token: 0x17000579 RID: 1401
		// (get) Token: 0x0600146F RID: 5231 RVA: 0x0001DD4A File Offset: 0x0001BF4A
		// (set) Token: 0x06001470 RID: 5232 RVA: 0x0001DD52 File Offset: 0x0001BF52
		public EpicAccountId LocalUserId { get; set; }

		// Token: 0x1700057A RID: 1402
		// (get) Token: 0x06001471 RID: 5233 RVA: 0x0001DD5B File Offset: 0x0001BF5B
		// (set) Token: 0x06001472 RID: 5234 RVA: 0x0001DD63 File Offset: 0x0001BF63
		public PresenceModification PresenceModificationHandle { get; set; }
	}
}
