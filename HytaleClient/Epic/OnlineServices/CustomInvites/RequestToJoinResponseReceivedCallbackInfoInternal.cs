using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.CustomInvites
{
	// Token: 0x020005B5 RID: 1461
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct RequestToJoinResponseReceivedCallbackInfoInternal : ICallbackInfoInternal, IGettable<RequestToJoinResponseReceivedCallbackInfo>, ISettable<RequestToJoinResponseReceivedCallbackInfo>, IDisposable
	{
		// Token: 0x17000AFC RID: 2812
		// (get) Token: 0x060025E6 RID: 9702 RVA: 0x00037A48 File Offset: 0x00035C48
		// (set) Token: 0x060025E7 RID: 9703 RVA: 0x00037A69 File Offset: 0x00035C69
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

		// Token: 0x17000AFD RID: 2813
		// (get) Token: 0x060025E8 RID: 9704 RVA: 0x00037A7C File Offset: 0x00035C7C
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x17000AFE RID: 2814
		// (get) Token: 0x060025E9 RID: 9705 RVA: 0x00037A94 File Offset: 0x00035C94
		// (set) Token: 0x060025EA RID: 9706 RVA: 0x00037AB5 File Offset: 0x00035CB5
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

		// Token: 0x17000AFF RID: 2815
		// (get) Token: 0x060025EB RID: 9707 RVA: 0x00037AC8 File Offset: 0x00035CC8
		// (set) Token: 0x060025EC RID: 9708 RVA: 0x00037AE9 File Offset: 0x00035CE9
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

		// Token: 0x17000B00 RID: 2816
		// (get) Token: 0x060025ED RID: 9709 RVA: 0x00037AFC File Offset: 0x00035CFC
		// (set) Token: 0x060025EE RID: 9710 RVA: 0x00037B14 File Offset: 0x00035D14
		public RequestToJoinResponse Response
		{
			get
			{
				return this.m_Response;
			}
			set
			{
				this.m_Response = value;
			}
		}

		// Token: 0x060025EF RID: 9711 RVA: 0x00037B1E File Offset: 0x00035D1E
		public void Set(ref RequestToJoinResponseReceivedCallbackInfo other)
		{
			this.ClientData = other.ClientData;
			this.FromUserId = other.FromUserId;
			this.ToUserId = other.ToUserId;
			this.Response = other.Response;
		}

		// Token: 0x060025F0 RID: 9712 RVA: 0x00037B58 File Offset: 0x00035D58
		public void Set(ref RequestToJoinResponseReceivedCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ClientData = other.Value.ClientData;
				this.FromUserId = other.Value.FromUserId;
				this.ToUserId = other.Value.ToUserId;
				this.Response = other.Value.Response;
			}
		}

		// Token: 0x060025F1 RID: 9713 RVA: 0x00037BC6 File Offset: 0x00035DC6
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_FromUserId);
			Helper.Dispose(ref this.m_ToUserId);
		}

		// Token: 0x060025F2 RID: 9714 RVA: 0x00037BED File Offset: 0x00035DED
		public void Get(out RequestToJoinResponseReceivedCallbackInfo output)
		{
			output = default(RequestToJoinResponseReceivedCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x04001068 RID: 4200
		private IntPtr m_ClientData;

		// Token: 0x04001069 RID: 4201
		private IntPtr m_FromUserId;

		// Token: 0x0400106A RID: 4202
		private IntPtr m_ToUserId;

		// Token: 0x0400106B RID: 4203
		private RequestToJoinResponse m_Response;
	}
}
