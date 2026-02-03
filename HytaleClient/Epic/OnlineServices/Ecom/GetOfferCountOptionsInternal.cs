using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom
{
	// Token: 0x02000535 RID: 1333
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct GetOfferCountOptionsInternal : ISettable<GetOfferCountOptions>, IDisposable
	{
		// Token: 0x17000A24 RID: 2596
		// (set) Token: 0x060022E9 RID: 8937 RVA: 0x00033B2E File Offset: 0x00031D2E
		public EpicAccountId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x060022EA RID: 8938 RVA: 0x00033B3E File Offset: 0x00031D3E
		public void Set(ref GetOfferCountOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
		}

		// Token: 0x060022EB RID: 8939 RVA: 0x00033B58 File Offset: 0x00031D58
		public void Set(ref GetOfferCountOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
			}
		}

		// Token: 0x060022EC RID: 8940 RVA: 0x00033B8E File Offset: 0x00031D8E
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
		}

		// Token: 0x04000F65 RID: 3941
		private int m_ApiVersion;

		// Token: 0x04000F66 RID: 3942
		private IntPtr m_LocalUserId;
	}
}
