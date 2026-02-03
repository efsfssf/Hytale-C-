using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.CustomInvites
{
	// Token: 0x020005BD RID: 1469
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct SendRequestToJoinCallbackInfoInternal : ICallbackInfoInternal, IGettable<SendRequestToJoinCallbackInfo>, ISettable<SendRequestToJoinCallbackInfo>, IDisposable
	{
		// Token: 0x17000B1F RID: 2847
		// (get) Token: 0x0600263C RID: 9788 RVA: 0x00038328 File Offset: 0x00036528
		// (set) Token: 0x0600263D RID: 9789 RVA: 0x00038340 File Offset: 0x00036540
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

		// Token: 0x17000B20 RID: 2848
		// (get) Token: 0x0600263E RID: 9790 RVA: 0x0003834C File Offset: 0x0003654C
		// (set) Token: 0x0600263F RID: 9791 RVA: 0x0003836D File Offset: 0x0003656D
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

		// Token: 0x17000B21 RID: 2849
		// (get) Token: 0x06002640 RID: 9792 RVA: 0x00038380 File Offset: 0x00036580
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x17000B22 RID: 2850
		// (get) Token: 0x06002641 RID: 9793 RVA: 0x00038398 File Offset: 0x00036598
		// (set) Token: 0x06002642 RID: 9794 RVA: 0x000383B9 File Offset: 0x000365B9
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

		// Token: 0x17000B23 RID: 2851
		// (get) Token: 0x06002643 RID: 9795 RVA: 0x000383CC File Offset: 0x000365CC
		// (set) Token: 0x06002644 RID: 9796 RVA: 0x000383ED File Offset: 0x000365ED
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

		// Token: 0x06002645 RID: 9797 RVA: 0x000383FD File Offset: 0x000365FD
		public void Set(ref SendRequestToJoinCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.TargetUserId = other.TargetUserId;
		}

		// Token: 0x06002646 RID: 9798 RVA: 0x00038434 File Offset: 0x00036634
		public void Set(ref SendRequestToJoinCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ResultCode = other.Value.ResultCode;
				this.ClientData = other.Value.ClientData;
				this.LocalUserId = other.Value.LocalUserId;
				this.TargetUserId = other.Value.TargetUserId;
			}
		}

		// Token: 0x06002647 RID: 9799 RVA: 0x000384A2 File Offset: 0x000366A2
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_TargetUserId);
		}

		// Token: 0x06002648 RID: 9800 RVA: 0x000384C9 File Offset: 0x000366C9
		public void Get(out SendRequestToJoinCallbackInfo output)
		{
			output = default(SendRequestToJoinCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x0400108B RID: 4235
		private Result m_ResultCode;

		// Token: 0x0400108C RID: 4236
		private IntPtr m_ClientData;

		// Token: 0x0400108D RID: 4237
		private IntPtr m_LocalUserId;

		// Token: 0x0400108E RID: 4238
		private IntPtr m_TargetUserId;
	}
}
