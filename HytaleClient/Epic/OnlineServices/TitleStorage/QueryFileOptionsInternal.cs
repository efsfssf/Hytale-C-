using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.TitleStorage
{
	// Token: 0x020000B8 RID: 184
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct QueryFileOptionsInternal : ISettable<QueryFileOptions>, IDisposable
	{
		// Token: 0x17000136 RID: 310
		// (set) Token: 0x060006D6 RID: 1750 RVA: 0x00009A48 File Offset: 0x00007C48
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x17000137 RID: 311
		// (set) Token: 0x060006D7 RID: 1751 RVA: 0x00009A58 File Offset: 0x00007C58
		public Utf8String Filename
		{
			set
			{
				Helper.Set(value, ref this.m_Filename);
			}
		}

		// Token: 0x060006D8 RID: 1752 RVA: 0x00009A68 File Offset: 0x00007C68
		public void Set(ref QueryFileOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
			this.Filename = other.Filename;
		}

		// Token: 0x060006D9 RID: 1753 RVA: 0x00009A8C File Offset: 0x00007C8C
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

		// Token: 0x060006DA RID: 1754 RVA: 0x00009AD7 File Offset: 0x00007CD7
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_Filename);
		}

		// Token: 0x04000341 RID: 833
		private int m_ApiVersion;

		// Token: 0x04000342 RID: 834
		private IntPtr m_LocalUserId;

		// Token: 0x04000343 RID: 835
		private IntPtr m_Filename;
	}
}
