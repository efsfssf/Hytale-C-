using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.TitleStorage
{
	// Token: 0x0200009A RID: 154
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct CopyFileMetadataByFilenameOptionsInternal : ISettable<CopyFileMetadataByFilenameOptions>, IDisposable
	{
		// Token: 0x17000100 RID: 256
		// (set) Token: 0x0600061B RID: 1563 RVA: 0x00008D08 File Offset: 0x00006F08
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x17000101 RID: 257
		// (set) Token: 0x0600061C RID: 1564 RVA: 0x00008D18 File Offset: 0x00006F18
		public Utf8String Filename
		{
			set
			{
				Helper.Set(value, ref this.m_Filename);
			}
		}

		// Token: 0x0600061D RID: 1565 RVA: 0x00008D28 File Offset: 0x00006F28
		public void Set(ref CopyFileMetadataByFilenameOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
			this.Filename = other.Filename;
		}

		// Token: 0x0600061E RID: 1566 RVA: 0x00008D4C File Offset: 0x00006F4C
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

		// Token: 0x0600061F RID: 1567 RVA: 0x00008D97 File Offset: 0x00006F97
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_Filename);
		}

		// Token: 0x04000309 RID: 777
		private int m_ApiVersion;

		// Token: 0x0400030A RID: 778
		private IntPtr m_LocalUserId;

		// Token: 0x0400030B RID: 779
		private IntPtr m_Filename;
	}
}
