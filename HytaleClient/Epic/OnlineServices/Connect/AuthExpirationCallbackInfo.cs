using System;

namespace Epic.OnlineServices.Connect
{
	// Token: 0x020005C6 RID: 1478
	public struct AuthExpirationCallbackInfo : ICallbackInfo
	{
		// Token: 0x17000B2C RID: 2860
		// (get) Token: 0x06002661 RID: 9825 RVA: 0x000386D4 File Offset: 0x000368D4
		// (set) Token: 0x06002662 RID: 9826 RVA: 0x000386DC File Offset: 0x000368DC
		public object ClientData { get; set; }

		// Token: 0x17000B2D RID: 2861
		// (get) Token: 0x06002663 RID: 9827 RVA: 0x000386E5 File Offset: 0x000368E5
		// (set) Token: 0x06002664 RID: 9828 RVA: 0x000386ED File Offset: 0x000368ED
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x06002665 RID: 9829 RVA: 0x000386F8 File Offset: 0x000368F8
		public Result? GetResultCode()
		{
			return null;
		}

		// Token: 0x06002666 RID: 9830 RVA: 0x00038713 File Offset: 0x00036913
		internal void Set(ref AuthExpirationCallbackInfoInternal other)
		{
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
		}
	}
}
