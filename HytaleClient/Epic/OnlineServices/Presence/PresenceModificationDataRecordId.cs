using System;

namespace Epic.OnlineServices.Presence
{
	// Token: 0x020002D4 RID: 724
	public struct PresenceModificationDataRecordId
	{
		// Token: 0x17000559 RID: 1369
		// (get) Token: 0x0600140D RID: 5133 RVA: 0x0001D526 File Offset: 0x0001B726
		// (set) Token: 0x0600140E RID: 5134 RVA: 0x0001D52E File Offset: 0x0001B72E
		public Utf8String Key { get; set; }

		// Token: 0x0600140F RID: 5135 RVA: 0x0001D537 File Offset: 0x0001B737
		internal void Set(ref PresenceModificationDataRecordIdInternal other)
		{
			this.Key = other.Key;
		}
	}
}
