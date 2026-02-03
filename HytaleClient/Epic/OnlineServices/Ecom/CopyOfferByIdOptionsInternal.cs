using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom
{
	// Token: 0x0200051B RID: 1307
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct CopyOfferByIdOptionsInternal : ISettable<CopyOfferByIdOptions>, IDisposable
	{
		// Token: 0x170009ED RID: 2541
		// (set) Token: 0x06002243 RID: 8771 RVA: 0x00032490 File Offset: 0x00030690
		public EpicAccountId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x170009EE RID: 2542
		// (set) Token: 0x06002244 RID: 8772 RVA: 0x000324A0 File Offset: 0x000306A0
		public Utf8String OfferId
		{
			set
			{
				Helper.Set(value, ref this.m_OfferId);
			}
		}

		// Token: 0x06002245 RID: 8773 RVA: 0x000324B0 File Offset: 0x000306B0
		public void Set(ref CopyOfferByIdOptions other)
		{
			this.m_ApiVersion = 3;
			this.LocalUserId = other.LocalUserId;
			this.OfferId = other.OfferId;
		}

		// Token: 0x06002246 RID: 8774 RVA: 0x000324D4 File Offset: 0x000306D4
		public void Set(ref CopyOfferByIdOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 3;
				this.LocalUserId = other.Value.LocalUserId;
				this.OfferId = other.Value.OfferId;
			}
		}

		// Token: 0x06002247 RID: 8775 RVA: 0x0003251F File Offset: 0x0003071F
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_OfferId);
		}

		// Token: 0x04000EEB RID: 3819
		private int m_ApiVersion;

		// Token: 0x04000EEC RID: 3820
		private IntPtr m_LocalUserId;

		// Token: 0x04000EED RID: 3821
		private IntPtr m_OfferId;
	}
}
