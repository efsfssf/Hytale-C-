using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.CustomInvites
{
	// Token: 0x020005AE RID: 1454
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct RejectRequestToJoinCallbackInfoInternal : ICallbackInfoInternal, IGettable<RejectRequestToJoinCallbackInfo>, ISettable<RejectRequestToJoinCallbackInfo>, IDisposable
	{
		// Token: 0x17000AE8 RID: 2792
		// (get) Token: 0x060025B3 RID: 9651 RVA: 0x00037540 File Offset: 0x00035740
		// (set) Token: 0x060025B4 RID: 9652 RVA: 0x00037558 File Offset: 0x00035758
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

		// Token: 0x17000AE9 RID: 2793
		// (get) Token: 0x060025B5 RID: 9653 RVA: 0x00037564 File Offset: 0x00035764
		// (set) Token: 0x060025B6 RID: 9654 RVA: 0x00037585 File Offset: 0x00035785
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

		// Token: 0x17000AEA RID: 2794
		// (get) Token: 0x060025B7 RID: 9655 RVA: 0x00037598 File Offset: 0x00035798
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x17000AEB RID: 2795
		// (get) Token: 0x060025B8 RID: 9656 RVA: 0x000375B0 File Offset: 0x000357B0
		// (set) Token: 0x060025B9 RID: 9657 RVA: 0x000375D1 File Offset: 0x000357D1
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

		// Token: 0x17000AEC RID: 2796
		// (get) Token: 0x060025BA RID: 9658 RVA: 0x000375E4 File Offset: 0x000357E4
		// (set) Token: 0x060025BB RID: 9659 RVA: 0x00037605 File Offset: 0x00035805
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

		// Token: 0x060025BC RID: 9660 RVA: 0x00037615 File Offset: 0x00035815
		public void Set(ref RejectRequestToJoinCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.TargetUserId = other.TargetUserId;
		}

		// Token: 0x060025BD RID: 9661 RVA: 0x0003764C File Offset: 0x0003584C
		public void Set(ref RejectRequestToJoinCallbackInfo? other)
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

		// Token: 0x060025BE RID: 9662 RVA: 0x000376BA File Offset: 0x000358BA
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_TargetUserId);
		}

		// Token: 0x060025BF RID: 9663 RVA: 0x000376E1 File Offset: 0x000358E1
		public void Get(out RejectRequestToJoinCallbackInfo output)
		{
			output = default(RejectRequestToJoinCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x04001052 RID: 4178
		private Result m_ResultCode;

		// Token: 0x04001053 RID: 4179
		private IntPtr m_ClientData;

		// Token: 0x04001054 RID: 4180
		private IntPtr m_LocalUserId;

		// Token: 0x04001055 RID: 4181
		private IntPtr m_TargetUserId;
	}
}
