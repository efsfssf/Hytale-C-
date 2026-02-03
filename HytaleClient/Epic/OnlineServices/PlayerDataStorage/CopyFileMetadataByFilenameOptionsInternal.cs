using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.PlayerDataStorage
{
	// Token: 0x020002EE RID: 750
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct CopyFileMetadataByFilenameOptionsInternal : ISettable<CopyFileMetadataByFilenameOptions>, IDisposable
	{
		// Token: 0x17000583 RID: 1411
		// (set) Token: 0x06001485 RID: 5253 RVA: 0x0001DEF4 File Offset: 0x0001C0F4
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x17000584 RID: 1412
		// (set) Token: 0x06001486 RID: 5254 RVA: 0x0001DF04 File Offset: 0x0001C104
		public Utf8String Filename
		{
			set
			{
				Helper.Set(value, ref this.m_Filename);
			}
		}

		// Token: 0x06001487 RID: 5255 RVA: 0x0001DF14 File Offset: 0x0001C114
		public void Set(ref CopyFileMetadataByFilenameOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
			this.Filename = other.Filename;
		}

		// Token: 0x06001488 RID: 5256 RVA: 0x0001DF38 File Offset: 0x0001C138
		public void Set(ref CopyFileMetadataByFilenameOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
				this.Filename = other.Value.Filename;
			}
		}

		// Token: 0x06001489 RID: 5257 RVA: 0x0001DF83 File Offset: 0x0001C183
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_Filename);
		}

		// Token: 0x04000907 RID: 2311
		private int m_ApiVersion;

		// Token: 0x04000908 RID: 2312
		private IntPtr m_LocalUserId;

		// Token: 0x04000909 RID: 2313
		private IntPtr m_Filename;
	}
}
