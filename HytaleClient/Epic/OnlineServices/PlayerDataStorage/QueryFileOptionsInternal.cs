using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.PlayerDataStorage
{
	// Token: 0x0200031E RID: 798
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct QueryFileOptionsInternal : ISettable<QueryFileOptions>, IDisposable
	{
		// Token: 0x170005D1 RID: 1489
		// (set) Token: 0x060015B8 RID: 5560 RVA: 0x0001F86B File Offset: 0x0001DA6B
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x170005D2 RID: 1490
		// (set) Token: 0x060015B9 RID: 5561 RVA: 0x0001F87B File Offset: 0x0001DA7B
		public Utf8String Filename
		{
			set
			{
				Helper.Set(value, ref this.m_Filename);
			}
		}

		// Token: 0x060015BA RID: 5562 RVA: 0x0001F88B File Offset: 0x0001DA8B
		public void Set(ref QueryFileOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
			this.Filename = other.Filename;
		}

		// Token: 0x060015BB RID: 5563 RVA: 0x0001F8B0 File Offset: 0x0001DAB0
		public void Set(ref QueryFileOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
				this.Filename = other.Value.Filename;
			}
		}

		// Token: 0x060015BC RID: 5564 RVA: 0x0001F8FB File Offset: 0x0001DAFB
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_Filename);
		}

		// Token: 0x0400096D RID: 2413
		private int m_ApiVersion;

		// Token: 0x0400096E RID: 2414
		private IntPtr m_LocalUserId;

		// Token: 0x0400096F RID: 2415
		private IntPtr m_Filename;
	}
}
