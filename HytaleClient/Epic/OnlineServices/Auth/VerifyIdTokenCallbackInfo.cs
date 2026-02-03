using System;

namespace Epic.OnlineServices.Auth
{
	// Token: 0x02000665 RID: 1637
	public struct VerifyIdTokenCallbackInfo : ICallbackInfo
	{
		// Token: 0x17000C69 RID: 3177
		// (get) Token: 0x06002A7A RID: 10874 RVA: 0x0003E674 File Offset: 0x0003C874
		// (set) Token: 0x06002A7B RID: 10875 RVA: 0x0003E67C File Offset: 0x0003C87C
		public Result ResultCode { get; set; }

		// Token: 0x17000C6A RID: 3178
		// (get) Token: 0x06002A7C RID: 10876 RVA: 0x0003E685 File Offset: 0x0003C885
		// (set) Token: 0x06002A7D RID: 10877 RVA: 0x0003E68D File Offset: 0x0003C88D
		public object ClientData { get; set; }

		// Token: 0x17000C6B RID: 3179
		// (get) Token: 0x06002A7E RID: 10878 RVA: 0x0003E696 File Offset: 0x0003C896
		// (set) Token: 0x06002A7F RID: 10879 RVA: 0x0003E69E File Offset: 0x0003C89E
		public Utf8String ApplicationId { get; set; }

		// Token: 0x17000C6C RID: 3180
		// (get) Token: 0x06002A80 RID: 10880 RVA: 0x0003E6A7 File Offset: 0x0003C8A7
		// (set) Token: 0x06002A81 RID: 10881 RVA: 0x0003E6AF File Offset: 0x0003C8AF
		public Utf8String ClientId { get; set; }

		// Token: 0x17000C6D RID: 3181
		// (get) Token: 0x06002A82 RID: 10882 RVA: 0x0003E6B8 File Offset: 0x0003C8B8
		// (set) Token: 0x06002A83 RID: 10883 RVA: 0x0003E6C0 File Offset: 0x0003C8C0
		public Utf8String ProductId { get; set; }

		// Token: 0x17000C6E RID: 3182
		// (get) Token: 0x06002A84 RID: 10884 RVA: 0x0003E6C9 File Offset: 0x0003C8C9
		// (set) Token: 0x06002A85 RID: 10885 RVA: 0x0003E6D1 File Offset: 0x0003C8D1
		public Utf8String SandboxId { get; set; }

		// Token: 0x17000C6F RID: 3183
		// (get) Token: 0x06002A86 RID: 10886 RVA: 0x0003E6DA File Offset: 0x0003C8DA
		// (set) Token: 0x06002A87 RID: 10887 RVA: 0x0003E6E2 File Offset: 0x0003C8E2
		public Utf8String DeploymentId { get; set; }

		// Token: 0x17000C70 RID: 3184
		// (get) Token: 0x06002A88 RID: 10888 RVA: 0x0003E6EB File Offset: 0x0003C8EB
		// (set) Token: 0x06002A89 RID: 10889 RVA: 0x0003E6F3 File Offset: 0x0003C8F3
		public Utf8String DisplayName { get; set; }

		// Token: 0x17000C71 RID: 3185
		// (get) Token: 0x06002A8A RID: 10890 RVA: 0x0003E6FC File Offset: 0x0003C8FC
		// (set) Token: 0x06002A8B RID: 10891 RVA: 0x0003E704 File Offset: 0x0003C904
		public bool IsExternalAccountInfoPresent { get; set; }

		// Token: 0x17000C72 RID: 3186
		// (get) Token: 0x06002A8C RID: 10892 RVA: 0x0003E70D File Offset: 0x0003C90D
		// (set) Token: 0x06002A8D RID: 10893 RVA: 0x0003E715 File Offset: 0x0003C915
		public ExternalAccountType ExternalAccountIdType { get; set; }

		// Token: 0x17000C73 RID: 3187
		// (get) Token: 0x06002A8E RID: 10894 RVA: 0x0003E71E File Offset: 0x0003C91E
		// (set) Token: 0x06002A8F RID: 10895 RVA: 0x0003E726 File Offset: 0x0003C926
		public Utf8String ExternalAccountId { get; set; }

		// Token: 0x17000C74 RID: 3188
		// (get) Token: 0x06002A90 RID: 10896 RVA: 0x0003E72F File Offset: 0x0003C92F
		// (set) Token: 0x06002A91 RID: 10897 RVA: 0x0003E737 File Offset: 0x0003C937
		public Utf8String ExternalAccountDisplayName { get; set; }

		// Token: 0x17000C75 RID: 3189
		// (get) Token: 0x06002A92 RID: 10898 RVA: 0x0003E740 File Offset: 0x0003C940
		// (set) Token: 0x06002A93 RID: 10899 RVA: 0x0003E748 File Offset: 0x0003C948
		public Utf8String Platform { get; set; }

		// Token: 0x06002A94 RID: 10900 RVA: 0x0003E754 File Offset: 0x0003C954
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x06002A95 RID: 10901 RVA: 0x0003E774 File Offset: 0x0003C974
		internal void Set(ref VerifyIdTokenCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.ApplicationId = other.ApplicationId;
			this.ClientId = other.ClientId;
			this.ProductId = other.ProductId;
			this.SandboxId = other.SandboxId;
			this.DeploymentId = other.DeploymentId;
			this.DisplayName = other.DisplayName;
			this.IsExternalAccountInfoPresent = other.IsExternalAccountInfoPresent;
			this.ExternalAccountIdType = other.ExternalAccountIdType;
			this.ExternalAccountId = other.ExternalAccountId;
			this.ExternalAccountDisplayName = other.ExternalAccountDisplayName;
			this.Platform = other.Platform;
		}
	}
}
