using System;

namespace Epic.OnlineServices.Connect
{
	// Token: 0x02000625 RID: 1573
	public struct VerifyIdTokenCallbackInfo : ICallbackInfo
	{
		// Token: 0x17000BD4 RID: 3028
		// (get) Token: 0x060028A8 RID: 10408 RVA: 0x0003B99C File Offset: 0x00039B9C
		// (set) Token: 0x060028A9 RID: 10409 RVA: 0x0003B9A4 File Offset: 0x00039BA4
		public Result ResultCode { get; set; }

		// Token: 0x17000BD5 RID: 3029
		// (get) Token: 0x060028AA RID: 10410 RVA: 0x0003B9AD File Offset: 0x00039BAD
		// (set) Token: 0x060028AB RID: 10411 RVA: 0x0003B9B5 File Offset: 0x00039BB5
		public object ClientData { get; set; }

		// Token: 0x17000BD6 RID: 3030
		// (get) Token: 0x060028AC RID: 10412 RVA: 0x0003B9BE File Offset: 0x00039BBE
		// (set) Token: 0x060028AD RID: 10413 RVA: 0x0003B9C6 File Offset: 0x00039BC6
		public ProductUserId ProductUserId { get; set; }

		// Token: 0x17000BD7 RID: 3031
		// (get) Token: 0x060028AE RID: 10414 RVA: 0x0003B9CF File Offset: 0x00039BCF
		// (set) Token: 0x060028AF RID: 10415 RVA: 0x0003B9D7 File Offset: 0x00039BD7
		public bool IsAccountInfoPresent { get; set; }

		// Token: 0x17000BD8 RID: 3032
		// (get) Token: 0x060028B0 RID: 10416 RVA: 0x0003B9E0 File Offset: 0x00039BE0
		// (set) Token: 0x060028B1 RID: 10417 RVA: 0x0003B9E8 File Offset: 0x00039BE8
		public ExternalAccountType AccountIdType { get; set; }

		// Token: 0x17000BD9 RID: 3033
		// (get) Token: 0x060028B2 RID: 10418 RVA: 0x0003B9F1 File Offset: 0x00039BF1
		// (set) Token: 0x060028B3 RID: 10419 RVA: 0x0003B9F9 File Offset: 0x00039BF9
		public Utf8String AccountId { get; set; }

		// Token: 0x17000BDA RID: 3034
		// (get) Token: 0x060028B4 RID: 10420 RVA: 0x0003BA02 File Offset: 0x00039C02
		// (set) Token: 0x060028B5 RID: 10421 RVA: 0x0003BA0A File Offset: 0x00039C0A
		public Utf8String Platform { get; set; }

		// Token: 0x17000BDB RID: 3035
		// (get) Token: 0x060028B6 RID: 10422 RVA: 0x0003BA13 File Offset: 0x00039C13
		// (set) Token: 0x060028B7 RID: 10423 RVA: 0x0003BA1B File Offset: 0x00039C1B
		public Utf8String DeviceType { get; set; }

		// Token: 0x17000BDC RID: 3036
		// (get) Token: 0x060028B8 RID: 10424 RVA: 0x0003BA24 File Offset: 0x00039C24
		// (set) Token: 0x060028B9 RID: 10425 RVA: 0x0003BA2C File Offset: 0x00039C2C
		public Utf8String ClientId { get; set; }

		// Token: 0x17000BDD RID: 3037
		// (get) Token: 0x060028BA RID: 10426 RVA: 0x0003BA35 File Offset: 0x00039C35
		// (set) Token: 0x060028BB RID: 10427 RVA: 0x0003BA3D File Offset: 0x00039C3D
		public Utf8String ProductId { get; set; }

		// Token: 0x17000BDE RID: 3038
		// (get) Token: 0x060028BC RID: 10428 RVA: 0x0003BA46 File Offset: 0x00039C46
		// (set) Token: 0x060028BD RID: 10429 RVA: 0x0003BA4E File Offset: 0x00039C4E
		public Utf8String SandboxId { get; set; }

		// Token: 0x17000BDF RID: 3039
		// (get) Token: 0x060028BE RID: 10430 RVA: 0x0003BA57 File Offset: 0x00039C57
		// (set) Token: 0x060028BF RID: 10431 RVA: 0x0003BA5F File Offset: 0x00039C5F
		public Utf8String DeploymentId { get; set; }

		// Token: 0x060028C0 RID: 10432 RVA: 0x0003BA68 File Offset: 0x00039C68
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x060028C1 RID: 10433 RVA: 0x0003BA88 File Offset: 0x00039C88
		internal void Set(ref VerifyIdTokenCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.ProductUserId = other.ProductUserId;
			this.IsAccountInfoPresent = other.IsAccountInfoPresent;
			this.AccountIdType = other.AccountIdType;
			this.AccountId = other.AccountId;
			this.Platform = other.Platform;
			this.DeviceType = other.DeviceType;
			this.ClientId = other.ClientId;
			this.ProductId = other.ProductId;
			this.SandboxId = other.SandboxId;
			this.DeploymentId = other.DeploymentId;
		}
	}
}
