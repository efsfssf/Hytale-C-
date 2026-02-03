using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom
{
	// Token: 0x02000539 RID: 1337
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct GetOfferItemCountOptionsInternal : ISettable<GetOfferItemCountOptions>, IDisposable
	{
		// Token: 0x17000A2B RID: 2603
		// (set) Token: 0x060022FA RID: 8954 RVA: 0x00033C8C File Offset: 0x00031E8C
		public EpicAccountId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x17000A2C RID: 2604
		// (set) Token: 0x060022FB RID: 8955 RVA: 0x00033C9C File Offset: 0x00031E9C
		public Utf8String OfferId
		{
			set
			{
				Helper.Set(value, ref this.m_OfferId);
			}
		}

		// Token: 0x060022FC RID: 8956 RVA: 0x00033CAC File Offset: 0x00031EAC
		public void Set(ref GetOfferItemCountOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
			this.OfferId = other.OfferId;
		}

		// Token: 0x060022FD RID: 8957 RVA: 0x00033CD0 File Offset: 0x00031ED0
		public void Set(ref GetOfferItemCountOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
				this.OfferId = other.Value.OfferId;
			}
		}

		// Token: 0x060022FE RID: 8958 RVA: 0x00033D1B File Offset: 0x00031F1B
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_OfferId);
		}

		// Token: 0x04000F6E RID: 3950
		private int m_ApiVersion;

		// Token: 0x04000F6F RID: 3951
		private IntPtr m_LocalUserId;

		// Token: 0x04000F70 RID: 3952
		private IntPtr m_OfferId;
	}
}
