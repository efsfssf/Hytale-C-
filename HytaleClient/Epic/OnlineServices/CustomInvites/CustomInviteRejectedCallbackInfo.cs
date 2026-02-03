using System;

namespace Epic.OnlineServices.CustomInvites
{
	// Token: 0x02000588 RID: 1416
	public struct CustomInviteRejectedCallbackInfo : ICallbackInfo
	{
		// Token: 0x17000AAD RID: 2733
		// (get) Token: 0x0600249F RID: 9375 RVA: 0x00035E74 File Offset: 0x00034074
		// (set) Token: 0x060024A0 RID: 9376 RVA: 0x00035E7C File Offset: 0x0003407C
		public object ClientData { get; set; }

		// Token: 0x17000AAE RID: 2734
		// (get) Token: 0x060024A1 RID: 9377 RVA: 0x00035E85 File Offset: 0x00034085
		// (set) Token: 0x060024A2 RID: 9378 RVA: 0x00035E8D File Offset: 0x0003408D
		public ProductUserId TargetUserId { get; set; }

		// Token: 0x17000AAF RID: 2735
		// (get) Token: 0x060024A3 RID: 9379 RVA: 0x00035E96 File Offset: 0x00034096
		// (set) Token: 0x060024A4 RID: 9380 RVA: 0x00035E9E File Offset: 0x0003409E
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x17000AB0 RID: 2736
		// (get) Token: 0x060024A5 RID: 9381 RVA: 0x00035EA7 File Offset: 0x000340A7
		// (set) Token: 0x060024A6 RID: 9382 RVA: 0x00035EAF File Offset: 0x000340AF
		public Utf8String CustomInviteId { get; set; }

		// Token: 0x17000AB1 RID: 2737
		// (get) Token: 0x060024A7 RID: 9383 RVA: 0x00035EB8 File Offset: 0x000340B8
		// (set) Token: 0x060024A8 RID: 9384 RVA: 0x00035EC0 File Offset: 0x000340C0
		public Utf8String Payload { get; set; }

		// Token: 0x060024A9 RID: 9385 RVA: 0x00035ECC File Offset: 0x000340CC
		public Result? GetResultCode()
		{
			return null;
		}

		// Token: 0x060024AA RID: 9386 RVA: 0x00035EE8 File Offset: 0x000340E8
		internal void Set(ref CustomInviteRejectedCallbackInfoInternal other)
		{
			this.ClientData = other.ClientData;
			this.TargetUserId = other.TargetUserId;
			this.LocalUserId = other.LocalUserId;
			this.CustomInviteId = other.CustomInviteId;
			this.Payload = other.Payload;
		}
	}
}
