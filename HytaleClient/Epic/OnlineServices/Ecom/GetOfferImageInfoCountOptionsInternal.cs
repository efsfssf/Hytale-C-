using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom
{
	// Token: 0x02000537 RID: 1335
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct GetOfferImageInfoCountOptionsInternal : ISettable<GetOfferImageInfoCountOptions>, IDisposable
	{
		// Token: 0x17000A27 RID: 2599
		// (set) Token: 0x060022F1 RID: 8945 RVA: 0x00033BBF File Offset: 0x00031DBF
		public EpicAccountId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x17000A28 RID: 2600
		// (set) Token: 0x060022F2 RID: 8946 RVA: 0x00033BCF File Offset: 0x00031DCF
		public Utf8String OfferId
		{
			set
			{
				Helper.Set(value, ref this.m_OfferId);
			}
		}

		// Token: 0x060022F3 RID: 8947 RVA: 0x00033BDF File Offset: 0x00031DDF
		public void Set(ref GetOfferImageInfoCountOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
			this.OfferId = other.OfferId;
		}

		// Token: 0x060022F4 RID: 8948 RVA: 0x00033C04 File Offset: 0x00031E04
		public void Set(ref GetOfferImageInfoCountOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
				this.OfferId = other.Value.OfferId;
			}
		}

		// Token: 0x060022F5 RID: 8949 RVA: 0x00033C4F File Offset: 0x00031E4F
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_OfferId);
		}

		// Token: 0x04000F69 RID: 3945
		private int m_ApiVersion;

		// Token: 0x04000F6A RID: 3946
		private IntPtr m_LocalUserId;

		// Token: 0x04000F6B RID: 3947
		private IntPtr m_OfferId;
	}
}
