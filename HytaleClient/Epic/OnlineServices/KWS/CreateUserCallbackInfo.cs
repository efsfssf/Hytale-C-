using System;

namespace Epic.OnlineServices.KWS
{
	// Token: 0x02000487 RID: 1159
	public struct CreateUserCallbackInfo : ICallbackInfo
	{
		// Token: 0x1700088E RID: 2190
		// (get) Token: 0x06001E44 RID: 7748 RVA: 0x0002C50A File Offset: 0x0002A70A
		// (set) Token: 0x06001E45 RID: 7749 RVA: 0x0002C512 File Offset: 0x0002A712
		public Result ResultCode { get; set; }

		// Token: 0x1700088F RID: 2191
		// (get) Token: 0x06001E46 RID: 7750 RVA: 0x0002C51B File Offset: 0x0002A71B
		// (set) Token: 0x06001E47 RID: 7751 RVA: 0x0002C523 File Offset: 0x0002A723
		public object ClientData { get; set; }

		// Token: 0x17000890 RID: 2192
		// (get) Token: 0x06001E48 RID: 7752 RVA: 0x0002C52C File Offset: 0x0002A72C
		// (set) Token: 0x06001E49 RID: 7753 RVA: 0x0002C534 File Offset: 0x0002A734
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x17000891 RID: 2193
		// (get) Token: 0x06001E4A RID: 7754 RVA: 0x0002C53D File Offset: 0x0002A73D
		// (set) Token: 0x06001E4B RID: 7755 RVA: 0x0002C545 File Offset: 0x0002A745
		public Utf8String KWSUserId { get; set; }

		// Token: 0x17000892 RID: 2194
		// (get) Token: 0x06001E4C RID: 7756 RVA: 0x0002C54E File Offset: 0x0002A74E
		// (set) Token: 0x06001E4D RID: 7757 RVA: 0x0002C556 File Offset: 0x0002A756
		public bool IsMinor { get; set; }

		// Token: 0x06001E4E RID: 7758 RVA: 0x0002C560 File Offset: 0x0002A760
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x06001E4F RID: 7759 RVA: 0x0002C580 File Offset: 0x0002A780
		internal void Set(ref CreateUserCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.KWSUserId = other.KWSUserId;
			this.IsMinor = other.IsMinor;
		}
	}
}
