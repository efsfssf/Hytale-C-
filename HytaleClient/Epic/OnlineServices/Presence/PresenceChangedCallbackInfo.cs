using System;

namespace Epic.OnlineServices.Presence
{
	// Token: 0x020002D0 RID: 720
	public struct PresenceChangedCallbackInfo : ICallbackInfo
	{
		// Token: 0x17000552 RID: 1362
		// (get) Token: 0x060013E1 RID: 5089 RVA: 0x0001CE1C File Offset: 0x0001B01C
		// (set) Token: 0x060013E2 RID: 5090 RVA: 0x0001CE24 File Offset: 0x0001B024
		public object ClientData { get; set; }

		// Token: 0x17000553 RID: 1363
		// (get) Token: 0x060013E3 RID: 5091 RVA: 0x0001CE2D File Offset: 0x0001B02D
		// (set) Token: 0x060013E4 RID: 5092 RVA: 0x0001CE35 File Offset: 0x0001B035
		public EpicAccountId LocalUserId { get; set; }

		// Token: 0x17000554 RID: 1364
		// (get) Token: 0x060013E5 RID: 5093 RVA: 0x0001CE3E File Offset: 0x0001B03E
		// (set) Token: 0x060013E6 RID: 5094 RVA: 0x0001CE46 File Offset: 0x0001B046
		public EpicAccountId PresenceUserId { get; set; }

		// Token: 0x060013E7 RID: 5095 RVA: 0x0001CE50 File Offset: 0x0001B050
		public Result? GetResultCode()
		{
			return null;
		}

		// Token: 0x060013E8 RID: 5096 RVA: 0x0001CE6B File Offset: 0x0001B06B
		internal void Set(ref PresenceChangedCallbackInfoInternal other)
		{
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.PresenceUserId = other.PresenceUserId;
		}
	}
}
