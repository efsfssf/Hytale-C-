using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.KWS
{
	// Token: 0x020004AA RID: 1194
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct RequestPermissionsCallbackInfoInternal : ICallbackInfoInternal, IGettable<RequestPermissionsCallbackInfo>, ISettable<RequestPermissionsCallbackInfo>, IDisposable
	{
		// Token: 0x170008D3 RID: 2259
		// (get) Token: 0x06001F33 RID: 7987 RVA: 0x0002DA6C File Offset: 0x0002BC6C
		// (set) Token: 0x06001F34 RID: 7988 RVA: 0x0002DA84 File Offset: 0x0002BC84
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

		// Token: 0x170008D4 RID: 2260
		// (get) Token: 0x06001F35 RID: 7989 RVA: 0x0002DA90 File Offset: 0x0002BC90
		// (set) Token: 0x06001F36 RID: 7990 RVA: 0x0002DAB1 File Offset: 0x0002BCB1
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

		// Token: 0x170008D5 RID: 2261
		// (get) Token: 0x06001F37 RID: 7991 RVA: 0x0002DAC4 File Offset: 0x0002BCC4
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x170008D6 RID: 2262
		// (get) Token: 0x06001F38 RID: 7992 RVA: 0x0002DADC File Offset: 0x0002BCDC
		// (set) Token: 0x06001F39 RID: 7993 RVA: 0x0002DAFD File Offset: 0x0002BCFD
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

		// Token: 0x06001F3A RID: 7994 RVA: 0x0002DB0D File Offset: 0x0002BD0D
		public void Set(ref RequestPermissionsCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
		}

		// Token: 0x06001F3B RID: 7995 RVA: 0x0002DB38 File Offset: 0x0002BD38
		public void Set(ref RequestPermissionsCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ResultCode = other.Value.ResultCode;
				this.ClientData = other.Value.ClientData;
				this.LocalUserId = other.Value.LocalUserId;
			}
		}

		// Token: 0x06001F3C RID: 7996 RVA: 0x0002DB91 File Offset: 0x0002BD91
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LocalUserId);
		}

		// Token: 0x06001F3D RID: 7997 RVA: 0x0002DBAC File Offset: 0x0002BDAC
		public void Get(out RequestPermissionsCallbackInfo output)
		{
			output = default(RequestPermissionsCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x04000D8C RID: 3468
		private Result m_ResultCode;

		// Token: 0x04000D8D RID: 3469
		private IntPtr m_ClientData;

		// Token: 0x04000D8E RID: 3470
		private IntPtr m_LocalUserId;
	}
}
