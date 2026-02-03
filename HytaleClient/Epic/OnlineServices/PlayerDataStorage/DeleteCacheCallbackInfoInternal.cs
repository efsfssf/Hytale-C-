using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.PlayerDataStorage
{
	// Token: 0x020002F0 RID: 752
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct DeleteCacheCallbackInfoInternal : ICallbackInfoInternal, IGettable<DeleteCacheCallbackInfo>, ISettable<DeleteCacheCallbackInfo>, IDisposable
	{
		// Token: 0x17000588 RID: 1416
		// (get) Token: 0x06001492 RID: 5266 RVA: 0x0001E01C File Offset: 0x0001C21C
		// (set) Token: 0x06001493 RID: 5267 RVA: 0x0001E034 File Offset: 0x0001C234
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

		// Token: 0x17000589 RID: 1417
		// (get) Token: 0x06001494 RID: 5268 RVA: 0x0001E040 File Offset: 0x0001C240
		// (set) Token: 0x06001495 RID: 5269 RVA: 0x0001E061 File Offset: 0x0001C261
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

		// Token: 0x1700058A RID: 1418
		// (get) Token: 0x06001496 RID: 5270 RVA: 0x0001E074 File Offset: 0x0001C274
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x1700058B RID: 1419
		// (get) Token: 0x06001497 RID: 5271 RVA: 0x0001E08C File Offset: 0x0001C28C
		// (set) Token: 0x06001498 RID: 5272 RVA: 0x0001E0AD File Offset: 0x0001C2AD
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

		// Token: 0x06001499 RID: 5273 RVA: 0x0001E0BD File Offset: 0x0001C2BD
		public void Set(ref DeleteCacheCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
		}

		// Token: 0x0600149A RID: 5274 RVA: 0x0001E0E8 File Offset: 0x0001C2E8
		public void Set(ref DeleteCacheCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ResultCode = other.Value.ResultCode;
				this.ClientData = other.Value.ClientData;
				this.LocalUserId = other.Value.LocalUserId;
			}
		}

		// Token: 0x0600149B RID: 5275 RVA: 0x0001E141 File Offset: 0x0001C341
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LocalUserId);
		}

		// Token: 0x0600149C RID: 5276 RVA: 0x0001E15C File Offset: 0x0001C35C
		public void Get(out DeleteCacheCallbackInfo output)
		{
			output = default(DeleteCacheCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x0400090D RID: 2317
		private Result m_ResultCode;

		// Token: 0x0400090E RID: 2318
		private IntPtr m_ClientData;

		// Token: 0x0400090F RID: 2319
		private IntPtr m_LocalUserId;
	}
}
