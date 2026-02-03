using System;

namespace Epic.OnlineServices.Connect
{
	// Token: 0x020005E1 RID: 1505
	public struct ExternalAccountInfo
	{
		// Token: 0x17000B5A RID: 2906
		// (get) Token: 0x0600270D RID: 9997 RVA: 0x00039C98 File Offset: 0x00037E98
		// (set) Token: 0x0600270E RID: 9998 RVA: 0x00039CA0 File Offset: 0x00037EA0
		public ProductUserId ProductUserId { get; set; }

		// Token: 0x17000B5B RID: 2907
		// (get) Token: 0x0600270F RID: 9999 RVA: 0x00039CA9 File Offset: 0x00037EA9
		// (set) Token: 0x06002710 RID: 10000 RVA: 0x00039CB1 File Offset: 0x00037EB1
		public Utf8String DisplayName { get; set; }

		// Token: 0x17000B5C RID: 2908
		// (get) Token: 0x06002711 RID: 10001 RVA: 0x00039CBA File Offset: 0x00037EBA
		// (set) Token: 0x06002712 RID: 10002 RVA: 0x00039CC2 File Offset: 0x00037EC2
		public Utf8String AccountId { get; set; }

		// Token: 0x17000B5D RID: 2909
		// (get) Token: 0x06002713 RID: 10003 RVA: 0x00039CCB File Offset: 0x00037ECB
		// (set) Token: 0x06002714 RID: 10004 RVA: 0x00039CD3 File Offset: 0x00037ED3
		public ExternalAccountType AccountIdType { get; set; }

		// Token: 0x17000B5E RID: 2910
		// (get) Token: 0x06002715 RID: 10005 RVA: 0x00039CDC File Offset: 0x00037EDC
		// (set) Token: 0x06002716 RID: 10006 RVA: 0x00039CE4 File Offset: 0x00037EE4
		public DateTimeOffset? LastLoginTime { get; set; }

		// Token: 0x06002717 RID: 10007 RVA: 0x00039CF0 File Offset: 0x00037EF0
		internal void Set(ref ExternalAccountInfoInternal other)
		{
			this.ProductUserId = other.ProductUserId;
			this.DisplayName = other.DisplayName;
			this.AccountId = other.AccountId;
			this.AccountIdType = other.AccountIdType;
			this.LastLoginTime = other.LastLoginTime;
		}
	}
}
