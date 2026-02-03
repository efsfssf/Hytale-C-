using System;

namespace Epic.OnlineServices.IntegratedPlatform
{
	// Token: 0x020004CD RID: 1229
	public struct UserPreLogoutCallbackInfo : ICallbackInfo
	{
		// Token: 0x17000913 RID: 2323
		// (get) Token: 0x06001FF9 RID: 8185 RVA: 0x0002ED25 File Offset: 0x0002CF25
		// (set) Token: 0x06001FFA RID: 8186 RVA: 0x0002ED2D File Offset: 0x0002CF2D
		public object ClientData { get; set; }

		// Token: 0x17000914 RID: 2324
		// (get) Token: 0x06001FFB RID: 8187 RVA: 0x0002ED36 File Offset: 0x0002CF36
		// (set) Token: 0x06001FFC RID: 8188 RVA: 0x0002ED3E File Offset: 0x0002CF3E
		public Utf8String PlatformType { get; set; }

		// Token: 0x17000915 RID: 2325
		// (get) Token: 0x06001FFD RID: 8189 RVA: 0x0002ED47 File Offset: 0x0002CF47
		// (set) Token: 0x06001FFE RID: 8190 RVA: 0x0002ED4F File Offset: 0x0002CF4F
		public Utf8String LocalPlatformUserId { get; set; }

		// Token: 0x17000916 RID: 2326
		// (get) Token: 0x06001FFF RID: 8191 RVA: 0x0002ED58 File Offset: 0x0002CF58
		// (set) Token: 0x06002000 RID: 8192 RVA: 0x0002ED60 File Offset: 0x0002CF60
		public EpicAccountId AccountId { get; set; }

		// Token: 0x17000917 RID: 2327
		// (get) Token: 0x06002001 RID: 8193 RVA: 0x0002ED69 File Offset: 0x0002CF69
		// (set) Token: 0x06002002 RID: 8194 RVA: 0x0002ED71 File Offset: 0x0002CF71
		public ProductUserId ProductUserId { get; set; }

		// Token: 0x06002003 RID: 8195 RVA: 0x0002ED7C File Offset: 0x0002CF7C
		public Result? GetResultCode()
		{
			return null;
		}

		// Token: 0x06002004 RID: 8196 RVA: 0x0002ED98 File Offset: 0x0002CF98
		internal void Set(ref UserPreLogoutCallbackInfoInternal other)
		{
			this.ClientData = other.ClientData;
			this.PlatformType = other.PlatformType;
			this.LocalPlatformUserId = other.LocalPlatformUserId;
			this.AccountId = other.AccountId;
			this.ProductUserId = other.ProductUserId;
		}
	}
}
