using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.CustomInvites
{
	// Token: 0x0200059E RID: 1438
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct OnRequestToJoinAcceptedCallbackInfoInternal : ICallbackInfoInternal, IGettable<OnRequestToJoinAcceptedCallbackInfo>, ISettable<OnRequestToJoinAcceptedCallbackInfo>, IDisposable
	{
		// Token: 0x17000AD9 RID: 2777
		// (get) Token: 0x0600255B RID: 9563 RVA: 0x0003714C File Offset: 0x0003534C
		// (set) Token: 0x0600255C RID: 9564 RVA: 0x0003716D File Offset: 0x0003536D
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

		// Token: 0x17000ADA RID: 2778
		// (get) Token: 0x0600255D RID: 9565 RVA: 0x00037180 File Offset: 0x00035380
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x17000ADB RID: 2779
		// (get) Token: 0x0600255E RID: 9566 RVA: 0x00037198 File Offset: 0x00035398
		// (set) Token: 0x0600255F RID: 9567 RVA: 0x000371B9 File Offset: 0x000353B9
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

		// Token: 0x17000ADC RID: 2780
		// (get) Token: 0x06002560 RID: 9568 RVA: 0x000371CC File Offset: 0x000353CC
		// (set) Token: 0x06002561 RID: 9569 RVA: 0x000371ED File Offset: 0x000353ED
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

		// Token: 0x06002562 RID: 9570 RVA: 0x000371FD File Offset: 0x000353FD
		public void Set(ref OnRequestToJoinAcceptedCallbackInfo other)
		{
			this.ClientData = other.ClientData;
			this.TargetUserId = other.TargetUserId;
			this.LocalUserId = other.LocalUserId;
		}

		// Token: 0x06002563 RID: 9571 RVA: 0x00037228 File Offset: 0x00035428
		public void Set(ref OnRequestToJoinAcceptedCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ClientData = other.Value.ClientData;
				this.TargetUserId = other.Value.TargetUserId;
				this.LocalUserId = other.Value.LocalUserId;
			}
		}

		// Token: 0x06002564 RID: 9572 RVA: 0x00037281 File Offset: 0x00035481
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_TargetUserId);
			Helper.Dispose(ref this.m_LocalUserId);
		}

		// Token: 0x06002565 RID: 9573 RVA: 0x000372A8 File Offset: 0x000354A8
		public void Get(out OnRequestToJoinAcceptedCallbackInfo output)
		{
			output = default(OnRequestToJoinAcceptedCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x04001045 RID: 4165
		private IntPtr m_ClientData;

		// Token: 0x04001046 RID: 4166
		private IntPtr m_TargetUserId;

		// Token: 0x04001047 RID: 4167
		private IntPtr m_LocalUserId;
	}
}
