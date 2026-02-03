using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.PlayerDataStorage
{
	// Token: 0x02000318 RID: 792
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct QueryFileCallbackInfoInternal : ICallbackInfoInternal, IGettable<QueryFileCallbackInfo>, ISettable<QueryFileCallbackInfo>, IDisposable
	{
		// Token: 0x170005C0 RID: 1472
		// (get) Token: 0x0600158C RID: 5516 RVA: 0x0001F444 File Offset: 0x0001D644
		// (set) Token: 0x0600158D RID: 5517 RVA: 0x0001F45C File Offset: 0x0001D65C
		public Result ResultCode
		{
			get
			{
				return this.m_ResultCode;
			}
			set
			{
				this.m_ResultCode = value;
			}
		}

		// Token: 0x170005C1 RID: 1473
		// (get) Token: 0x0600158E RID: 5518 RVA: 0x0001F468 File Offset: 0x0001D668
		// (set) Token: 0x0600158F RID: 5519 RVA: 0x0001F489 File Offset: 0x0001D689
		public object ClientData
		{
			get
			{
				object result;
				Helper.Get(this.m_ClientData, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_ClientData);
			}
		}

		// Token: 0x170005C2 RID: 1474
		// (get) Token: 0x06001590 RID: 5520 RVA: 0x0001F49C File Offset: 0x0001D69C
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x170005C3 RID: 1475
		// (get) Token: 0x06001591 RID: 5521 RVA: 0x0001F4B4 File Offset: 0x0001D6B4
		// (set) Token: 0x06001592 RID: 5522 RVA: 0x0001F4D5 File Offset: 0x0001D6D5
		public ProductUserId LocalUserId
		{
			get
			{
				ProductUserId result;
				Helper.Get<ProductUserId>(this.m_LocalUserId, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x06001593 RID: 5523 RVA: 0x0001F4E5 File Offset: 0x0001D6E5
		public void Set(ref QueryFileCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
		}

		// Token: 0x06001594 RID: 5524 RVA: 0x0001F510 File Offset: 0x0001D710
		public void Set(ref QueryFileCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ResultCode = other.Value.ResultCode;
				this.ClientData = other.Value.ClientData;
				this.LocalUserId = other.Value.LocalUserId;
			}
		}

		// Token: 0x06001595 RID: 5525 RVA: 0x0001F569 File Offset: 0x0001D769
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LocalUserId);
		}

		// Token: 0x06001596 RID: 5526 RVA: 0x0001F584 File Offset: 0x0001D784
		public void Get(out QueryFileCallbackInfo output)
		{
			output = default(QueryFileCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x0400095D RID: 2397
		private Result m_ResultCode;

		// Token: 0x0400095E RID: 2398
		private IntPtr m_ClientData;

		// Token: 0x0400095F RID: 2399
		private IntPtr m_LocalUserId;
	}
}
