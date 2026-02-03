using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Connect
{
	// Token: 0x02000614 RID: 1556
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct QueryExternalAccountMappingsCallbackInfoInternal : ICallbackInfoInternal, IGettable<QueryExternalAccountMappingsCallbackInfo>, ISettable<QueryExternalAccountMappingsCallbackInfo>, IDisposable
	{
		// Token: 0x17000BA3 RID: 2979
		// (get) Token: 0x0600282D RID: 10285 RVA: 0x0003ADDC File Offset: 0x00038FDC
		// (set) Token: 0x0600282E RID: 10286 RVA: 0x0003ADF4 File Offset: 0x00038FF4
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

		// Token: 0x17000BA4 RID: 2980
		// (get) Token: 0x0600282F RID: 10287 RVA: 0x0003AE00 File Offset: 0x00039000
		// (set) Token: 0x06002830 RID: 10288 RVA: 0x0003AE21 File Offset: 0x00039021
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

		// Token: 0x17000BA5 RID: 2981
		// (get) Token: 0x06002831 RID: 10289 RVA: 0x0003AE34 File Offset: 0x00039034
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x17000BA6 RID: 2982
		// (get) Token: 0x06002832 RID: 10290 RVA: 0x0003AE4C File Offset: 0x0003904C
		// (set) Token: 0x06002833 RID: 10291 RVA: 0x0003AE6D File Offset: 0x0003906D
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

		// Token: 0x06002834 RID: 10292 RVA: 0x0003AE7D File Offset: 0x0003907D
		public void Set(ref QueryExternalAccountMappingsCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
		}

		// Token: 0x06002835 RID: 10293 RVA: 0x0003AEA8 File Offset: 0x000390A8
		public void Set(ref QueryExternalAccountMappingsCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ResultCode = other.Value.ResultCode;
				this.ClientData = other.Value.ClientData;
				this.LocalUserId = other.Value.LocalUserId;
			}
		}

		// Token: 0x06002836 RID: 10294 RVA: 0x0003AF01 File Offset: 0x00039101
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LocalUserId);
		}

		// Token: 0x06002837 RID: 10295 RVA: 0x0003AF1C File Offset: 0x0003911C
		public void Get(out QueryExternalAccountMappingsCallbackInfo output)
		{
			output = default(QueryExternalAccountMappingsCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x0400113B RID: 4411
		private Result m_ResultCode;

		// Token: 0x0400113C RID: 4412
		private IntPtr m_ClientData;

		// Token: 0x0400113D RID: 4413
		private IntPtr m_LocalUserId;
	}
}
