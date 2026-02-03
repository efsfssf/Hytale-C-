using System;

namespace Epic.OnlineServices.RTCAdmin
{
	// Token: 0x0200029B RID: 667
	public struct UserToken
	{
		// Token: 0x170004F4 RID: 1268
		// (get) Token: 0x060012B4 RID: 4788 RVA: 0x0001B4A3 File Offset: 0x000196A3
		// (set) Token: 0x060012B5 RID: 4789 RVA: 0x0001B4AB File Offset: 0x000196AB
		public ProductUserId ProductUserId { get; set; }

		// Token: 0x170004F5 RID: 1269
		// (get) Token: 0x060012B6 RID: 4790 RVA: 0x0001B4B4 File Offset: 0x000196B4
		// (set) Token: 0x060012B7 RID: 4791 RVA: 0x0001B4BC File Offset: 0x000196BC
		public Utf8String Token { get; set; }

		// Token: 0x060012B8 RID: 4792 RVA: 0x0001B4C5 File Offset: 0x000196C5
		internal void Set(ref UserTokenInternal other)
		{
			this.ProductUserId = other.ProductUserId;
			this.Token = other.Token;
		}
	}
}
