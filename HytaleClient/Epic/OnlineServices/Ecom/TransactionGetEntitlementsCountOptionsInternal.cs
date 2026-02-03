using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom
{
	// Token: 0x02000573 RID: 1395
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct TransactionGetEntitlementsCountOptionsInternal : ISettable<TransactionGetEntitlementsCountOptions>, IDisposable
	{
		// Token: 0x06002464 RID: 9316 RVA: 0x000359AD File Offset: 0x00033BAD
		public void Set(ref TransactionGetEntitlementsCountOptions other)
		{
			this.m_ApiVersion = 1;
		}

		// Token: 0x06002465 RID: 9317 RVA: 0x000359B8 File Offset: 0x00033BB8
		public void Set(ref TransactionGetEntitlementsCountOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
			}
		}

		// Token: 0x06002466 RID: 9318 RVA: 0x000359D9 File Offset: 0x00033BD9
		public void Dispose()
		{
		}

		// Token: 0x04000FF6 RID: 4086
		private int m_ApiVersion;
	}
}
