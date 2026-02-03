using System;

namespace Epic.OnlineServices.Auth
{
	// Token: 0x02000636 RID: 1590
	public struct DeletePersistentAuthCallbackInfo : ICallbackInfo
	{
		// Token: 0x17000BFD RID: 3069
		// (get) Token: 0x06002930 RID: 10544 RVA: 0x0003CB93 File Offset: 0x0003AD93
		// (set) Token: 0x06002931 RID: 10545 RVA: 0x0003CB9B File Offset: 0x0003AD9B
		public Result ResultCode { get; set; }

		// Token: 0x17000BFE RID: 3070
		// (get) Token: 0x06002932 RID: 10546 RVA: 0x0003CBA4 File Offset: 0x0003ADA4
		// (set) Token: 0x06002933 RID: 10547 RVA: 0x0003CBAC File Offset: 0x0003ADAC
		public object ClientData { get; set; }

		// Token: 0x06002934 RID: 10548 RVA: 0x0003CBB8 File Offset: 0x0003ADB8
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x06002935 RID: 10549 RVA: 0x0003CBD5 File Offset: 0x0003ADD5
		internal void Set(ref DeletePersistentAuthCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
		}
	}
}
