using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Connect
{
	// Token: 0x02000618 RID: 1560
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct QueryProductUserIdMappingsCallbackInfoInternal : ICallbackInfoInternal, IGettable<QueryProductUserIdMappingsCallbackInfo>, ISettable<QueryProductUserIdMappingsCallbackInfo>, IDisposable
	{
		// Token: 0x17000BB0 RID: 2992
		// (get) Token: 0x0600284C RID: 10316 RVA: 0x0003B0BC File Offset: 0x000392BC
		// (set) Token: 0x0600284D RID: 10317 RVA: 0x0003B0D4 File Offset: 0x000392D4
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

		// Token: 0x17000BB1 RID: 2993
		// (get) Token: 0x0600284E RID: 10318 RVA: 0x0003B0E0 File Offset: 0x000392E0
		// (set) Token: 0x0600284F RID: 10319 RVA: 0x0003B101 File Offset: 0x00039301
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

		// Token: 0x17000BB2 RID: 2994
		// (get) Token: 0x06002850 RID: 10320 RVA: 0x0003B114 File Offset: 0x00039314
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x17000BB3 RID: 2995
		// (get) Token: 0x06002851 RID: 10321 RVA: 0x0003B12C File Offset: 0x0003932C
		// (set) Token: 0x06002852 RID: 10322 RVA: 0x0003B14D File Offset: 0x0003934D
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

		// Token: 0x06002853 RID: 10323 RVA: 0x0003B15D File Offset: 0x0003935D
		public void Set(ref QueryProductUserIdMappingsCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
		}

		// Token: 0x06002854 RID: 10324 RVA: 0x0003B188 File Offset: 0x00039388
		public void Set(ref QueryProductUserIdMappingsCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ResultCode = other.Value.ResultCode;
				this.ClientData = other.Value.ClientData;
				this.LocalUserId = other.Value.LocalUserId;
			}
		}

		// Token: 0x06002855 RID: 10325 RVA: 0x0003B1E1 File Offset: 0x000393E1
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LocalUserId);
		}

		// Token: 0x06002856 RID: 10326 RVA: 0x0003B1FC File Offset: 0x000393FC
		public void Get(out QueryProductUserIdMappingsCallbackInfo output)
		{
			output = default(QueryProductUserIdMappingsCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x04001149 RID: 4425
		private Result m_ResultCode;

		// Token: 0x0400114A RID: 4426
		private IntPtr m_ClientData;

		// Token: 0x0400114B RID: 4427
		private IntPtr m_LocalUserId;
	}
}
