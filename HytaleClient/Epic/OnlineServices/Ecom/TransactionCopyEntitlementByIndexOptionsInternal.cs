using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom
{
	// Token: 0x02000571 RID: 1393
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct TransactionCopyEntitlementByIndexOptionsInternal : ISettable<TransactionCopyEntitlementByIndexOptions>, IDisposable
	{
		// Token: 0x17000A9F RID: 2719
		// (set) Token: 0x06002460 RID: 9312 RVA: 0x00035950 File Offset: 0x00033B50
		public uint EntitlementIndex
		{
			set
			{
				this.m_EntitlementIndex = value;
			}
		}

		// Token: 0x06002461 RID: 9313 RVA: 0x0003595A File Offset: 0x00033B5A
		public void Set(ref TransactionCopyEntitlementByIndexOptions other)
		{
			this.m_ApiVersion = 1;
			this.EntitlementIndex = other.EntitlementIndex;
		}

		// Token: 0x06002462 RID: 9314 RVA: 0x00035974 File Offset: 0x00033B74
		public void Set(ref TransactionCopyEntitlementByIndexOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.EntitlementIndex = other.Value.EntitlementIndex;
			}
		}

		// Token: 0x06002463 RID: 9315 RVA: 0x000359AA File Offset: 0x00033BAA
		public void Dispose()
		{
		}

		// Token: 0x04000FF4 RID: 4084
		private int m_ApiVersion;

		// Token: 0x04000FF5 RID: 4085
		private uint m_EntitlementIndex;
	}
}
