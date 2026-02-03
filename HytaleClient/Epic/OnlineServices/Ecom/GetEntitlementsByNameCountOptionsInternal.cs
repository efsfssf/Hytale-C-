using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom
{
	// Token: 0x0200052B RID: 1323
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct GetEntitlementsByNameCountOptionsInternal : ISettable<GetEntitlementsByNameCountOptions>, IDisposable
	{
		// Token: 0x17000A15 RID: 2581
		// (set) Token: 0x060022C4 RID: 8900 RVA: 0x000337D9 File Offset: 0x000319D9
		public EpicAccountId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x17000A16 RID: 2582
		// (set) Token: 0x060022C5 RID: 8901 RVA: 0x000337E9 File Offset: 0x000319E9
		public Utf8String EntitlementName
		{
			set
			{
				Helper.Set(value, ref this.m_EntitlementName);
			}
		}

		// Token: 0x060022C6 RID: 8902 RVA: 0x000337F9 File Offset: 0x000319F9
		public void Set(ref GetEntitlementsByNameCountOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
			this.EntitlementName = other.EntitlementName;
		}

		// Token: 0x060022C7 RID: 8903 RVA: 0x00033820 File Offset: 0x00031A20
		public void Set(ref GetEntitlementsByNameCountOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
				this.EntitlementName = other.Value.EntitlementName;
			}
		}

		// Token: 0x060022C8 RID: 8904 RVA: 0x0003386B File Offset: 0x00031A6B
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_EntitlementName);
		}

		// Token: 0x04000F51 RID: 3921
		private int m_ApiVersion;

		// Token: 0x04000F52 RID: 3922
		private IntPtr m_LocalUserId;

		// Token: 0x04000F53 RID: 3923
		private IntPtr m_EntitlementName;
	}
}
