using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.TitleStorage
{
	// Token: 0x020000A4 RID: 164
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct GetFileMetadataCountOptionsInternal : ISettable<GetFileMetadataCountOptions>, IDisposable
	{
		// Token: 0x1700011F RID: 287
		// (set) Token: 0x0600066B RID: 1643 RVA: 0x000094E5 File Offset: 0x000076E5
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x0600066C RID: 1644 RVA: 0x000094F5 File Offset: 0x000076F5
		public void Set(ref GetFileMetadataCountOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
		}

		// Token: 0x0600066D RID: 1645 RVA: 0x0000950C File Offset: 0x0000770C
		public void Set(ref GetFileMetadataCountOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
			}
		}

		// Token: 0x0600066E RID: 1646 RVA: 0x00009542 File Offset: 0x00007742
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
		}

		// Token: 0x04000329 RID: 809
		private int m_ApiVersion;

		// Token: 0x0400032A RID: 810
		private IntPtr m_LocalUserId;
	}
}
