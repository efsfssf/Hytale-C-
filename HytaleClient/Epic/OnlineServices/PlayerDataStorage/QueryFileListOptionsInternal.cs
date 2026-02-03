using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.PlayerDataStorage
{
	// Token: 0x0200031C RID: 796
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct QueryFileListOptionsInternal : ISettable<QueryFileListOptions>, IDisposable
	{
		// Token: 0x170005CE RID: 1486
		// (set) Token: 0x060015B0 RID: 5552 RVA: 0x0001F7DC File Offset: 0x0001D9DC
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x060015B1 RID: 5553 RVA: 0x0001F7EC File Offset: 0x0001D9EC
		public void Set(ref QueryFileListOptions other)
		{
			this.m_ApiVersion = 2;
			this.LocalUserId = other.LocalUserId;
		}

		// Token: 0x060015B2 RID: 5554 RVA: 0x0001F804 File Offset: 0x0001DA04
		public void Set(ref QueryFileListOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 2;
				this.LocalUserId = other.Value.LocalUserId;
			}
		}

		// Token: 0x060015B3 RID: 5555 RVA: 0x0001F83A File Offset: 0x0001DA3A
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
		}

		// Token: 0x04000969 RID: 2409
		private int m_ApiVersion;

		// Token: 0x0400096A RID: 2410
		private IntPtr m_LocalUserId;
	}
}
