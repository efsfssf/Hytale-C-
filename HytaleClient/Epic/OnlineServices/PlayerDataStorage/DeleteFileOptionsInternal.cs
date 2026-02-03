using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.PlayerDataStorage
{
	// Token: 0x020002F6 RID: 758
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct DeleteFileOptionsInternal : ISettable<DeleteFileOptions>, IDisposable
	{
		// Token: 0x17000597 RID: 1431
		// (set) Token: 0x060014BA RID: 5306 RVA: 0x0001E3DC File Offset: 0x0001C5DC
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x17000598 RID: 1432
		// (set) Token: 0x060014BB RID: 5307 RVA: 0x0001E3EC File Offset: 0x0001C5EC
		public Utf8String Filename
		{
			set
			{
				Helper.Set(value, ref this.m_Filename);
			}
		}

		// Token: 0x060014BC RID: 5308 RVA: 0x0001E3FC File Offset: 0x0001C5FC
		public void Set(ref DeleteFileOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
			this.Filename = other.Filename;
		}

		// Token: 0x060014BD RID: 5309 RVA: 0x0001E420 File Offset: 0x0001C620
		public void Set(ref DeleteFileOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
				this.Filename = other.Value.Filename;
			}
		}

		// Token: 0x060014BE RID: 5310 RVA: 0x0001E46B File Offset: 0x0001C66B
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_Filename);
		}

		// Token: 0x0400091B RID: 2331
		private int m_ApiVersion;

		// Token: 0x0400091C RID: 2332
		private IntPtr m_LocalUserId;

		// Token: 0x0400091D RID: 2333
		private IntPtr m_Filename;
	}
}
