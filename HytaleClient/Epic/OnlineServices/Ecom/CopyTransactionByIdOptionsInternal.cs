using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom
{
	// Token: 0x02000523 RID: 1315
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct CopyTransactionByIdOptionsInternal : ISettable<CopyTransactionByIdOptions>, IDisposable
	{
		// Token: 0x17000A01 RID: 2561
		// (set) Token: 0x0600226D RID: 8813 RVA: 0x0003282D File Offset: 0x00030A2D
		public EpicAccountId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x17000A02 RID: 2562
		// (set) Token: 0x0600226E RID: 8814 RVA: 0x0003283D File Offset: 0x00030A3D
		public Utf8String TransactionId
		{
			set
			{
				Helper.Set(value, ref this.m_TransactionId);
			}
		}

		// Token: 0x0600226F RID: 8815 RVA: 0x0003284D File Offset: 0x00030A4D
		public void Set(ref CopyTransactionByIdOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
			this.TransactionId = other.TransactionId;
		}

		// Token: 0x06002270 RID: 8816 RVA: 0x00032874 File Offset: 0x00030A74
		public void Set(ref CopyTransactionByIdOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
				this.TransactionId = other.Value.TransactionId;
			}
		}

		// Token: 0x06002271 RID: 8817 RVA: 0x000328BF File Offset: 0x00030ABF
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_TransactionId);
		}

		// Token: 0x04000F03 RID: 3843
		private int m_ApiVersion;

		// Token: 0x04000F04 RID: 3844
		private IntPtr m_LocalUserId;

		// Token: 0x04000F05 RID: 3845
		private IntPtr m_TransactionId;
	}
}
