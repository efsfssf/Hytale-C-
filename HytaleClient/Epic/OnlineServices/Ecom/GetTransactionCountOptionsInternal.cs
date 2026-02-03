using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom
{
	// Token: 0x0200053B RID: 1339
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct GetTransactionCountOptionsInternal : ISettable<GetTransactionCountOptions>, IDisposable
	{
		// Token: 0x17000A2E RID: 2606
		// (set) Token: 0x06002301 RID: 8961 RVA: 0x00033D47 File Offset: 0x00031F47
		public EpicAccountId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x06002302 RID: 8962 RVA: 0x00033D57 File Offset: 0x00031F57
		public void Set(ref GetTransactionCountOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
		}

		// Token: 0x06002303 RID: 8963 RVA: 0x00033D70 File Offset: 0x00031F70
		public void Set(ref GetTransactionCountOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
			}
		}

		// Token: 0x06002304 RID: 8964 RVA: 0x00033DA6 File Offset: 0x00031FA6
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
		}

		// Token: 0x04000F72 RID: 3954
		private int m_ApiVersion;

		// Token: 0x04000F73 RID: 3955
		private IntPtr m_LocalUserId;
	}
}
