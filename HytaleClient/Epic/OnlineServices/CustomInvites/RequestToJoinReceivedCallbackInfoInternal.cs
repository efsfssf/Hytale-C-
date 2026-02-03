using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.CustomInvites
{
	// Token: 0x020005B2 RID: 1458
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct RequestToJoinReceivedCallbackInfoInternal : ICallbackInfoInternal, IGettable<RequestToJoinReceivedCallbackInfo>, ISettable<RequestToJoinReceivedCallbackInfo>, IDisposable
	{
		// Token: 0x17000AF4 RID: 2804
		// (get) Token: 0x060025D1 RID: 9681 RVA: 0x00037840 File Offset: 0x00035A40
		// (set) Token: 0x060025D2 RID: 9682 RVA: 0x00037861 File Offset: 0x00035A61
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

		// Token: 0x17000AF5 RID: 2805
		// (get) Token: 0x060025D3 RID: 9683 RVA: 0x00037874 File Offset: 0x00035A74
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x17000AF6 RID: 2806
		// (get) Token: 0x060025D4 RID: 9684 RVA: 0x0003788C File Offset: 0x00035A8C
		// (set) Token: 0x060025D5 RID: 9685 RVA: 0x000378AD File Offset: 0x00035AAD
		public ProductUserId FromUserId
		{
			get
			{
				ProductUserId result;
				Helper.Get<ProductUserId>(this.m_FromUserId, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_FromUserId);
			}
		}

		// Token: 0x17000AF7 RID: 2807
		// (get) Token: 0x060025D6 RID: 9686 RVA: 0x000378C0 File Offset: 0x00035AC0
		// (set) Token: 0x060025D7 RID: 9687 RVA: 0x000378E1 File Offset: 0x00035AE1
		public ProductUserId ToUserId
		{
			get
			{
				ProductUserId result;
				Helper.Get<ProductUserId>(this.m_ToUserId, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_ToUserId);
			}
		}

		// Token: 0x060025D8 RID: 9688 RVA: 0x000378F1 File Offset: 0x00035AF1
		public void Set(ref RequestToJoinReceivedCallbackInfo other)
		{
			this.ClientData = other.ClientData;
			this.FromUserId = other.FromUserId;
			this.ToUserId = other.ToUserId;
		}

		// Token: 0x060025D9 RID: 9689 RVA: 0x0003791C File Offset: 0x00035B1C
		public void Set(ref RequestToJoinReceivedCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ClientData = other.Value.ClientData;
				this.FromUserId = other.Value.FromUserId;
				this.ToUserId = other.Value.ToUserId;
			}
		}

		// Token: 0x060025DA RID: 9690 RVA: 0x00037975 File Offset: 0x00035B75
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_FromUserId);
			Helper.Dispose(ref this.m_ToUserId);
		}

		// Token: 0x060025DB RID: 9691 RVA: 0x0003799C File Offset: 0x00035B9C
		public void Get(out RequestToJoinReceivedCallbackInfo output)
		{
			output = default(RequestToJoinReceivedCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x0400105E RID: 4190
		private IntPtr m_ClientData;

		// Token: 0x0400105F RID: 4191
		private IntPtr m_FromUserId;

		// Token: 0x04001060 RID: 4192
		private IntPtr m_ToUserId;
	}
}
