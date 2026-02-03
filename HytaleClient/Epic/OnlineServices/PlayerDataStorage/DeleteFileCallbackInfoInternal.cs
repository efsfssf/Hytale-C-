using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.PlayerDataStorage
{
	// Token: 0x020002F4 RID: 756
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct DeleteFileCallbackInfoInternal : ICallbackInfoInternal, IGettable<DeleteFileCallbackInfo>, ISettable<DeleteFileCallbackInfo>, IDisposable
	{
		// Token: 0x17000591 RID: 1425
		// (get) Token: 0x060014AB RID: 5291 RVA: 0x0001E268 File Offset: 0x0001C468
		// (set) Token: 0x060014AC RID: 5292 RVA: 0x0001E280 File Offset: 0x0001C480
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

		// Token: 0x17000592 RID: 1426
		// (get) Token: 0x060014AD RID: 5293 RVA: 0x0001E28C File Offset: 0x0001C48C
		// (set) Token: 0x060014AE RID: 5294 RVA: 0x0001E2AD File Offset: 0x0001C4AD
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

		// Token: 0x17000593 RID: 1427
		// (get) Token: 0x060014AF RID: 5295 RVA: 0x0001E2C0 File Offset: 0x0001C4C0
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x17000594 RID: 1428
		// (get) Token: 0x060014B0 RID: 5296 RVA: 0x0001E2D8 File Offset: 0x0001C4D8
		// (set) Token: 0x060014B1 RID: 5297 RVA: 0x0001E2F9 File Offset: 0x0001C4F9
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

		// Token: 0x060014B2 RID: 5298 RVA: 0x0001E309 File Offset: 0x0001C509
		public void Set(ref DeleteFileCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
		}

		// Token: 0x060014B3 RID: 5299 RVA: 0x0001E334 File Offset: 0x0001C534
		public void Set(ref DeleteFileCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ResultCode = other.Value.ResultCode;
				this.ClientData = other.Value.ClientData;
				this.LocalUserId = other.Value.LocalUserId;
			}
		}

		// Token: 0x060014B4 RID: 5300 RVA: 0x0001E38D File Offset: 0x0001C58D
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LocalUserId);
		}

		// Token: 0x060014B5 RID: 5301 RVA: 0x0001E3A8 File Offset: 0x0001C5A8
		public void Get(out DeleteFileCallbackInfo output)
		{
			output = default(DeleteFileCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x04000916 RID: 2326
		private Result m_ResultCode;

		// Token: 0x04000917 RID: 2327
		private IntPtr m_ClientData;

		// Token: 0x04000918 RID: 2328
		private IntPtr m_LocalUserId;
	}
}
