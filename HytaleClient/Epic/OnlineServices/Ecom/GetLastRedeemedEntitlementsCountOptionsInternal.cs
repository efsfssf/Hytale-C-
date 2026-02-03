using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom
{
	// Token: 0x02000533 RID: 1331
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct GetLastRedeemedEntitlementsCountOptionsInternal : ISettable<GetLastRedeemedEntitlementsCountOptions>, IDisposable
	{
		// Token: 0x17000A22 RID: 2594
		// (set) Token: 0x060022E3 RID: 8931 RVA: 0x00033AAF File Offset: 0x00031CAF
		public EpicAccountId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x060022E4 RID: 8932 RVA: 0x00033ABF File Offset: 0x00031CBF
		public void Set(ref GetLastRedeemedEntitlementsCountOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
		}

		// Token: 0x060022E5 RID: 8933 RVA: 0x00033AD8 File Offset: 0x00031CD8
		public void Set(ref GetLastRedeemedEntitlementsCountOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
			}
		}

		// Token: 0x060022E6 RID: 8934 RVA: 0x00033B0E File Offset: 0x00031D0E
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
		}

		// Token: 0x04000F62 RID: 3938
		private int m_ApiVersion;

		// Token: 0x04000F63 RID: 3939
		private IntPtr m_LocalUserId;
	}
}
