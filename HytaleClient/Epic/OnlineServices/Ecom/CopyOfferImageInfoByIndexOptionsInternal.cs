using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom
{
	// Token: 0x0200051F RID: 1311
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct CopyOfferImageInfoByIndexOptionsInternal : ISettable<CopyOfferImageInfoByIndexOptions>, IDisposable
	{
		// Token: 0x170009F6 RID: 2550
		// (set) Token: 0x06002257 RID: 8791 RVA: 0x00032629 File Offset: 0x00030829
		public EpicAccountId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x170009F7 RID: 2551
		// (set) Token: 0x06002258 RID: 8792 RVA: 0x00032639 File Offset: 0x00030839
		public Utf8String OfferId
		{
			set
			{
				Helper.Set(value, ref this.m_OfferId);
			}
		}

		// Token: 0x170009F8 RID: 2552
		// (set) Token: 0x06002259 RID: 8793 RVA: 0x00032649 File Offset: 0x00030849
		public uint ImageInfoIndex
		{
			set
			{
				this.m_ImageInfoIndex = value;
			}
		}

		// Token: 0x0600225A RID: 8794 RVA: 0x00032653 File Offset: 0x00030853
		public void Set(ref CopyOfferImageInfoByIndexOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
			this.OfferId = other.OfferId;
			this.ImageInfoIndex = other.ImageInfoIndex;
		}

		// Token: 0x0600225B RID: 8795 RVA: 0x00032684 File Offset: 0x00030884
		public void Set(ref CopyOfferImageInfoByIndexOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
				this.OfferId = other.Value.OfferId;
				this.ImageInfoIndex = other.Value.ImageInfoIndex;
			}
		}

		// Token: 0x0600225C RID: 8796 RVA: 0x000326E4 File Offset: 0x000308E4
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_OfferId);
		}

		// Token: 0x04000EF6 RID: 3830
		private int m_ApiVersion;

		// Token: 0x04000EF7 RID: 3831
		private IntPtr m_LocalUserId;

		// Token: 0x04000EF8 RID: 3832
		private IntPtr m_OfferId;

		// Token: 0x04000EF9 RID: 3833
		private uint m_ImageInfoIndex;
	}
}
