using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom
{
	// Token: 0x0200051D RID: 1309
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct CopyOfferByIndexOptionsInternal : ISettable<CopyOfferByIndexOptions>, IDisposable
	{
		// Token: 0x170009F1 RID: 2545
		// (set) Token: 0x0600224C RID: 8780 RVA: 0x0003255C File Offset: 0x0003075C
		public EpicAccountId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x170009F2 RID: 2546
		// (set) Token: 0x0600224D RID: 8781 RVA: 0x0003256C File Offset: 0x0003076C
		public uint OfferIndex
		{
			set
			{
				this.m_OfferIndex = value;
			}
		}

		// Token: 0x0600224E RID: 8782 RVA: 0x00032576 File Offset: 0x00030776
		public void Set(ref CopyOfferByIndexOptions other)
		{
			this.m_ApiVersion = 3;
			this.LocalUserId = other.LocalUserId;
			this.OfferIndex = other.OfferIndex;
		}

		// Token: 0x0600224F RID: 8783 RVA: 0x0003259C File Offset: 0x0003079C
		public void Set(ref CopyOfferByIndexOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 3;
				this.LocalUserId = other.Value.LocalUserId;
				this.OfferIndex = other.Value.OfferIndex;
			}
		}

		// Token: 0x06002250 RID: 8784 RVA: 0x000325E7 File Offset: 0x000307E7
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
		}

		// Token: 0x04000EF0 RID: 3824
		private int m_ApiVersion;

		// Token: 0x04000EF1 RID: 3825
		private IntPtr m_LocalUserId;

		// Token: 0x04000EF2 RID: 3826
		private uint m_OfferIndex;
	}
}
