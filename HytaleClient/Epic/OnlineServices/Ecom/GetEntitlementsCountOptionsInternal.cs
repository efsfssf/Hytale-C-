using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom
{
	// Token: 0x0200052D RID: 1325
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct GetEntitlementsCountOptionsInternal : ISettable<GetEntitlementsCountOptions>, IDisposable
	{
		// Token: 0x17000A18 RID: 2584
		// (set) Token: 0x060022CB RID: 8907 RVA: 0x00033897 File Offset: 0x00031A97
		public EpicAccountId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x060022CC RID: 8908 RVA: 0x000338A7 File Offset: 0x00031AA7
		public void Set(ref GetEntitlementsCountOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
		}

		// Token: 0x060022CD RID: 8909 RVA: 0x000338C0 File Offset: 0x00031AC0
		public void Set(ref GetEntitlementsCountOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
			}
		}

		// Token: 0x060022CE RID: 8910 RVA: 0x000338F6 File Offset: 0x00031AF6
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
		}

		// Token: 0x04000F55 RID: 3925
		private int m_ApiVersion;

		// Token: 0x04000F56 RID: 3926
		private IntPtr m_LocalUserId;
	}
}
