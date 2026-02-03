using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.CustomInvites
{
	// Token: 0x020005A4 RID: 1444
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct OnRequestToJoinRejectedCallbackInfoInternal : ICallbackInfoInternal, IGettable<OnRequestToJoinRejectedCallbackInfo>, ISettable<OnRequestToJoinRejectedCallbackInfo>, IDisposable
	{
		// Token: 0x17000AE0 RID: 2784
		// (get) Token: 0x0600257E RID: 9598 RVA: 0x00037338 File Offset: 0x00035538
		// (set) Token: 0x0600257F RID: 9599 RVA: 0x00037359 File Offset: 0x00035559
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

		// Token: 0x17000AE1 RID: 2785
		// (get) Token: 0x06002580 RID: 9600 RVA: 0x0003736C File Offset: 0x0003556C
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x17000AE2 RID: 2786
		// (get) Token: 0x06002581 RID: 9601 RVA: 0x00037384 File Offset: 0x00035584
		// (set) Token: 0x06002582 RID: 9602 RVA: 0x000373A5 File Offset: 0x000355A5
		public ProductUserId TargetUserId
		{
			get
			{
				ProductUserId result;
				Helper.Get<ProductUserId>(this.m_TargetUserId, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_TargetUserId);
			}
		}

		// Token: 0x17000AE3 RID: 2787
		// (get) Token: 0x06002583 RID: 9603 RVA: 0x000373B8 File Offset: 0x000355B8
		// (set) Token: 0x06002584 RID: 9604 RVA: 0x000373D9 File Offset: 0x000355D9
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

		// Token: 0x06002585 RID: 9605 RVA: 0x000373E9 File Offset: 0x000355E9
		public void Set(ref OnRequestToJoinRejectedCallbackInfo other)
		{
			this.ClientData = other.ClientData;
			this.TargetUserId = other.TargetUserId;
			this.LocalUserId = other.LocalUserId;
		}

		// Token: 0x06002586 RID: 9606 RVA: 0x00037414 File Offset: 0x00035614
		public void Set(ref OnRequestToJoinRejectedCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ClientData = other.Value.ClientData;
				this.TargetUserId = other.Value.TargetUserId;
				this.LocalUserId = other.Value.LocalUserId;
			}
		}

		// Token: 0x06002587 RID: 9607 RVA: 0x0003746D File Offset: 0x0003566D
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_TargetUserId);
			Helper.Dispose(ref this.m_LocalUserId);
		}

		// Token: 0x06002588 RID: 9608 RVA: 0x00037494 File Offset: 0x00035694
		public void Get(out OnRequestToJoinRejectedCallbackInfo output)
		{
			output = default(OnRequestToJoinRejectedCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x0400104B RID: 4171
		private IntPtr m_ClientData;

		// Token: 0x0400104C RID: 4172
		private IntPtr m_TargetUserId;

		// Token: 0x0400104D RID: 4173
		private IntPtr m_LocalUserId;
	}
}
