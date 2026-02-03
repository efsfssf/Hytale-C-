using System;

namespace Epic.OnlineServices.CustomInvites
{
	// Token: 0x02000574 RID: 1396
	public struct AcceptRequestToJoinCallbackInfo : ICallbackInfo
	{
		// Token: 0x17000AA0 RID: 2720
		// (get) Token: 0x06002467 RID: 9319 RVA: 0x000359DC File Offset: 0x00033BDC
		// (set) Token: 0x06002468 RID: 9320 RVA: 0x000359E4 File Offset: 0x00033BE4
		public Result ResultCode { get; set; }

		// Token: 0x17000AA1 RID: 2721
		// (get) Token: 0x06002469 RID: 9321 RVA: 0x000359ED File Offset: 0x00033BED
		// (set) Token: 0x0600246A RID: 9322 RVA: 0x000359F5 File Offset: 0x00033BF5
		public object ClientData { get; set; }

		// Token: 0x17000AA2 RID: 2722
		// (get) Token: 0x0600246B RID: 9323 RVA: 0x000359FE File Offset: 0x00033BFE
		// (set) Token: 0x0600246C RID: 9324 RVA: 0x00035A06 File Offset: 0x00033C06
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x17000AA3 RID: 2723
		// (get) Token: 0x0600246D RID: 9325 RVA: 0x00035A0F File Offset: 0x00033C0F
		// (set) Token: 0x0600246E RID: 9326 RVA: 0x00035A17 File Offset: 0x00033C17
		public ProductUserId TargetUserId { get; set; }

		// Token: 0x0600246F RID: 9327 RVA: 0x00035A20 File Offset: 0x00033C20
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x06002470 RID: 9328 RVA: 0x00035A3D File Offset: 0x00033C3D
		internal void Set(ref AcceptRequestToJoinCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.TargetUserId = other.TargetUserId;
		}
	}
}
