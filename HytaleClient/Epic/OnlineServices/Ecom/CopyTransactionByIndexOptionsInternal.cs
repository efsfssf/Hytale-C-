using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom
{
	// Token: 0x02000525 RID: 1317
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct CopyTransactionByIndexOptionsInternal : ISettable<CopyTransactionByIndexOptions>, IDisposable
	{
		// Token: 0x17000A05 RID: 2565
		// (set) Token: 0x06002276 RID: 8822 RVA: 0x000328FC File Offset: 0x00030AFC
		public EpicAccountId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x17000A06 RID: 2566
		// (set) Token: 0x06002277 RID: 8823 RVA: 0x0003290C File Offset: 0x00030B0C
		public uint TransactionIndex
		{
			set
			{
				this.m_TransactionIndex = value;
			}
		}

		// Token: 0x06002278 RID: 8824 RVA: 0x00032916 File Offset: 0x00030B16
		public void Set(ref CopyTransactionByIndexOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
			this.TransactionIndex = other.TransactionIndex;
		}

		// Token: 0x06002279 RID: 8825 RVA: 0x0003293C File Offset: 0x00030B3C
		public void Set(ref CopyTransactionByIndexOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
				this.TransactionIndex = other.Value.TransactionIndex;
			}
		}

		// Token: 0x0600227A RID: 8826 RVA: 0x00032987 File Offset: 0x00030B87
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
		}

		// Token: 0x04000F08 RID: 3848
		private int m_ApiVersion;

		// Token: 0x04000F09 RID: 3849
		private IntPtr m_LocalUserId;

		// Token: 0x04000F0A RID: 3850
		private uint m_TransactionIndex;
	}
}
