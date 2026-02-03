using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sanctions
{
	// Token: 0x020001A6 RID: 422
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct QueryActivePlayerSanctionsCallbackInfoInternal : ICallbackInfoInternal, IGettable<QueryActivePlayerSanctionsCallbackInfo>, ISettable<QueryActivePlayerSanctionsCallbackInfo>, IDisposable
	{
		// Token: 0x170002D7 RID: 727
		// (get) Token: 0x06000C3A RID: 3130 RVA: 0x00011C74 File Offset: 0x0000FE74
		// (set) Token: 0x06000C3B RID: 3131 RVA: 0x00011C8C File Offset: 0x0000FE8C
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

		// Token: 0x170002D8 RID: 728
		// (get) Token: 0x06000C3C RID: 3132 RVA: 0x00011C98 File Offset: 0x0000FE98
		// (set) Token: 0x06000C3D RID: 3133 RVA: 0x00011CB9 File Offset: 0x0000FEB9
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

		// Token: 0x170002D9 RID: 729
		// (get) Token: 0x06000C3E RID: 3134 RVA: 0x00011CCC File Offset: 0x0000FECC
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x170002DA RID: 730
		// (get) Token: 0x06000C3F RID: 3135 RVA: 0x00011CE4 File Offset: 0x0000FEE4
		// (set) Token: 0x06000C40 RID: 3136 RVA: 0x00011D05 File Offset: 0x0000FF05
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

		// Token: 0x170002DB RID: 731
		// (get) Token: 0x06000C41 RID: 3137 RVA: 0x00011D18 File Offset: 0x0000FF18
		// (set) Token: 0x06000C42 RID: 3138 RVA: 0x00011D39 File Offset: 0x0000FF39
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

		// Token: 0x06000C43 RID: 3139 RVA: 0x00011D49 File Offset: 0x0000FF49
		public void Set(ref QueryActivePlayerSanctionsCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.TargetUserId = other.TargetUserId;
			this.LocalUserId = other.LocalUserId;
		}

		// Token: 0x06000C44 RID: 3140 RVA: 0x00011D80 File Offset: 0x0000FF80
		public void Set(ref QueryActivePlayerSanctionsCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ResultCode = other.Value.ResultCode;
				this.ClientData = other.Value.ClientData;
				this.TargetUserId = other.Value.TargetUserId;
				this.LocalUserId = other.Value.LocalUserId;
			}
		}

		// Token: 0x06000C45 RID: 3141 RVA: 0x00011DEE File Offset: 0x0000FFEE
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_TargetUserId);
			Helper.Dispose(ref this.m_LocalUserId);
		}

		// Token: 0x06000C46 RID: 3142 RVA: 0x00011E15 File Offset: 0x00010015
		public void Get(out QueryActivePlayerSanctionsCallbackInfo output)
		{
			output = default(QueryActivePlayerSanctionsCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x04000593 RID: 1427
		private Result m_ResultCode;

		// Token: 0x04000594 RID: 1428
		private IntPtr m_ClientData;

		// Token: 0x04000595 RID: 1429
		private IntPtr m_TargetUserId;

		// Token: 0x04000596 RID: 1430
		private IntPtr m_LocalUserId;
	}
}
