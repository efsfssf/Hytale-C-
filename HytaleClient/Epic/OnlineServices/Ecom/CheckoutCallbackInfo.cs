using System;

namespace Epic.OnlineServices.Ecom
{
	// Token: 0x02000505 RID: 1285
	public struct CheckoutCallbackInfo : ICallbackInfo
	{
		// Token: 0x170009B6 RID: 2486
		// (get) Token: 0x060021C8 RID: 8648 RVA: 0x000319CD File Offset: 0x0002FBCD
		// (set) Token: 0x060021C9 RID: 8649 RVA: 0x000319D5 File Offset: 0x0002FBD5
		public Result ResultCode { get; set; }

		// Token: 0x170009B7 RID: 2487
		// (get) Token: 0x060021CA RID: 8650 RVA: 0x000319DE File Offset: 0x0002FBDE
		// (set) Token: 0x060021CB RID: 8651 RVA: 0x000319E6 File Offset: 0x0002FBE6
		public object ClientData { get; set; }

		// Token: 0x170009B8 RID: 2488
		// (get) Token: 0x060021CC RID: 8652 RVA: 0x000319EF File Offset: 0x0002FBEF
		// (set) Token: 0x060021CD RID: 8653 RVA: 0x000319F7 File Offset: 0x0002FBF7
		public EpicAccountId LocalUserId { get; set; }

		// Token: 0x170009B9 RID: 2489
		// (get) Token: 0x060021CE RID: 8654 RVA: 0x00031A00 File Offset: 0x0002FC00
		// (set) Token: 0x060021CF RID: 8655 RVA: 0x00031A08 File Offset: 0x0002FC08
		public Utf8String TransactionId { get; set; }

		// Token: 0x060021D0 RID: 8656 RVA: 0x00031A14 File Offset: 0x0002FC14
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x060021D1 RID: 8657 RVA: 0x00031A31 File Offset: 0x0002FC31
		internal void Set(ref CheckoutCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.TransactionId = other.TransactionId;
		}
	}
}
