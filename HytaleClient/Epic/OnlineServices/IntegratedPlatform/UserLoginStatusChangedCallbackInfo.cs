using System;

namespace Epic.OnlineServices.IntegratedPlatform
{
	// Token: 0x020004CB RID: 1227
	public struct UserLoginStatusChangedCallbackInfo : ICallbackInfo
	{
		// Token: 0x17000904 RID: 2308
		// (get) Token: 0x06001FD6 RID: 8150 RVA: 0x0002E953 File Offset: 0x0002CB53
		// (set) Token: 0x06001FD7 RID: 8151 RVA: 0x0002E95B File Offset: 0x0002CB5B
		public object ClientData { get; set; }

		// Token: 0x17000905 RID: 2309
		// (get) Token: 0x06001FD8 RID: 8152 RVA: 0x0002E964 File Offset: 0x0002CB64
		// (set) Token: 0x06001FD9 RID: 8153 RVA: 0x0002E96C File Offset: 0x0002CB6C
		public Utf8String PlatformType { get; set; }

		// Token: 0x17000906 RID: 2310
		// (get) Token: 0x06001FDA RID: 8154 RVA: 0x0002E975 File Offset: 0x0002CB75
		// (set) Token: 0x06001FDB RID: 8155 RVA: 0x0002E97D File Offset: 0x0002CB7D
		public Utf8String LocalPlatformUserId { get; set; }

		// Token: 0x17000907 RID: 2311
		// (get) Token: 0x06001FDC RID: 8156 RVA: 0x0002E986 File Offset: 0x0002CB86
		// (set) Token: 0x06001FDD RID: 8157 RVA: 0x0002E98E File Offset: 0x0002CB8E
		public EpicAccountId AccountId { get; set; }

		// Token: 0x17000908 RID: 2312
		// (get) Token: 0x06001FDE RID: 8158 RVA: 0x0002E997 File Offset: 0x0002CB97
		// (set) Token: 0x06001FDF RID: 8159 RVA: 0x0002E99F File Offset: 0x0002CB9F
		public ProductUserId ProductUserId { get; set; }

		// Token: 0x17000909 RID: 2313
		// (get) Token: 0x06001FE0 RID: 8160 RVA: 0x0002E9A8 File Offset: 0x0002CBA8
		// (set) Token: 0x06001FE1 RID: 8161 RVA: 0x0002E9B0 File Offset: 0x0002CBB0
		public LoginStatus PreviousLoginStatus { get; set; }

		// Token: 0x1700090A RID: 2314
		// (get) Token: 0x06001FE2 RID: 8162 RVA: 0x0002E9B9 File Offset: 0x0002CBB9
		// (set) Token: 0x06001FE3 RID: 8163 RVA: 0x0002E9C1 File Offset: 0x0002CBC1
		public LoginStatus CurrentLoginStatus { get; set; }

		// Token: 0x06001FE4 RID: 8164 RVA: 0x0002E9CC File Offset: 0x0002CBCC
		public Result? GetResultCode()
		{
			return null;
		}

		// Token: 0x06001FE5 RID: 8165 RVA: 0x0002E9E8 File Offset: 0x0002CBE8
		internal void Set(ref UserLoginStatusChangedCallbackInfoInternal other)
		{
			this.ClientData = other.ClientData;
			this.PlatformType = other.PlatformType;
			this.LocalPlatformUserId = other.LocalPlatformUserId;
			this.AccountId = other.AccountId;
			this.ProductUserId = other.ProductUserId;
			this.PreviousLoginStatus = other.PreviousLoginStatus;
			this.CurrentLoginStatus = other.CurrentLoginStatus;
		}
	}
}
