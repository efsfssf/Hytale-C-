using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom
{
	// Token: 0x02000506 RID: 1286
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct CheckoutCallbackInfoInternal : ICallbackInfoInternal, IGettable<CheckoutCallbackInfo>, ISettable<CheckoutCallbackInfo>, IDisposable
	{
		// Token: 0x170009BA RID: 2490
		// (get) Token: 0x060021D2 RID: 8658 RVA: 0x00031A68 File Offset: 0x0002FC68
		// (set) Token: 0x060021D3 RID: 8659 RVA: 0x00031A80 File Offset: 0x0002FC80
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

		// Token: 0x170009BB RID: 2491
		// (get) Token: 0x060021D4 RID: 8660 RVA: 0x00031A8C File Offset: 0x0002FC8C
		// (set) Token: 0x060021D5 RID: 8661 RVA: 0x00031AAD File Offset: 0x0002FCAD
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

		// Token: 0x170009BC RID: 2492
		// (get) Token: 0x060021D6 RID: 8662 RVA: 0x00031AC0 File Offset: 0x0002FCC0
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x170009BD RID: 2493
		// (get) Token: 0x060021D7 RID: 8663 RVA: 0x00031AD8 File Offset: 0x0002FCD8
		// (set) Token: 0x060021D8 RID: 8664 RVA: 0x00031AF9 File Offset: 0x0002FCF9
		public EpicAccountId LocalUserId
		{
			get
			{
				EpicAccountId result;
				Helper.Get<EpicAccountId>(this.m_LocalUserId, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x170009BE RID: 2494
		// (get) Token: 0x060021D9 RID: 8665 RVA: 0x00031B0C File Offset: 0x0002FD0C
		// (set) Token: 0x060021DA RID: 8666 RVA: 0x00031B2D File Offset: 0x0002FD2D
		public Utf8String TransactionId
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_TransactionId, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_TransactionId);
			}
		}

		// Token: 0x060021DB RID: 8667 RVA: 0x00031B3D File Offset: 0x0002FD3D
		public void Set(ref CheckoutCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.TransactionId = other.TransactionId;
		}

		// Token: 0x060021DC RID: 8668 RVA: 0x00031B74 File Offset: 0x0002FD74
		public void Set(ref CheckoutCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ResultCode = other.Value.ResultCode;
				this.ClientData = other.Value.ClientData;
				this.LocalUserId = other.Value.LocalUserId;
				this.TransactionId = other.Value.TransactionId;
			}
		}

		// Token: 0x060021DD RID: 8669 RVA: 0x00031BE2 File Offset: 0x0002FDE2
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_TransactionId);
		}

		// Token: 0x060021DE RID: 8670 RVA: 0x00031C09 File Offset: 0x0002FE09
		public void Get(out CheckoutCallbackInfo output)
		{
			output = default(CheckoutCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x04000EAB RID: 3755
		private Result m_ResultCode;

		// Token: 0x04000EAC RID: 3756
		private IntPtr m_ClientData;

		// Token: 0x04000EAD RID: 3757
		private IntPtr m_LocalUserId;

		// Token: 0x04000EAE RID: 3758
		private IntPtr m_TransactionId;
	}
}
