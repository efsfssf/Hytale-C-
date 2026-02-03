using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.TitleStorage
{
	// Token: 0x0200009C RID: 156
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct DeleteCacheCallbackInfoInternal : ICallbackInfoInternal, IGettable<DeleteCacheCallbackInfo>, ISettable<DeleteCacheCallbackInfo>, IDisposable
	{
		// Token: 0x17000105 RID: 261
		// (get) Token: 0x06000628 RID: 1576 RVA: 0x00008E30 File Offset: 0x00007030
		// (set) Token: 0x06000629 RID: 1577 RVA: 0x00008E48 File Offset: 0x00007048
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

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x0600062A RID: 1578 RVA: 0x00008E54 File Offset: 0x00007054
		// (set) Token: 0x0600062B RID: 1579 RVA: 0x00008E75 File Offset: 0x00007075
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

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x0600062C RID: 1580 RVA: 0x00008E88 File Offset: 0x00007088
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x0600062D RID: 1581 RVA: 0x00008EA0 File Offset: 0x000070A0
		// (set) Token: 0x0600062E RID: 1582 RVA: 0x00008EC1 File Offset: 0x000070C1
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

		// Token: 0x0600062F RID: 1583 RVA: 0x00008ED1 File Offset: 0x000070D1
		public void Set(ref DeleteCacheCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
		}

		// Token: 0x06000630 RID: 1584 RVA: 0x00008EFC File Offset: 0x000070FC
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

		// Token: 0x06000631 RID: 1585 RVA: 0x00008F55 File Offset: 0x00007155
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LocalUserId);
		}

		// Token: 0x06000632 RID: 1586 RVA: 0x00008F70 File Offset: 0x00007170
		public void Get(out DeleteCacheCallbackInfo output)
		{
			output = default(DeleteCacheCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x0400030F RID: 783
		private Result m_ResultCode;

		// Token: 0x04000310 RID: 784
		private IntPtr m_ClientData;

		// Token: 0x04000311 RID: 785
		private IntPtr m_LocalUserId;
	}
}
