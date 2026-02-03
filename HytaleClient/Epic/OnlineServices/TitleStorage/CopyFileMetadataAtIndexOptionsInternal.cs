using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.TitleStorage
{
	// Token: 0x02000098 RID: 152
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct CopyFileMetadataAtIndexOptionsInternal : ISettable<CopyFileMetadataAtIndexOptions>, IDisposable
	{
		// Token: 0x170000FC RID: 252
		// (set) Token: 0x06000612 RID: 1554 RVA: 0x00008C4B File Offset: 0x00006E4B
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x170000FD RID: 253
		// (set) Token: 0x06000613 RID: 1555 RVA: 0x00008C5B File Offset: 0x00006E5B
		public uint Index
		{
			set
			{
				this.m_Index = value;
			}
		}

		// Token: 0x06000614 RID: 1556 RVA: 0x00008C65 File Offset: 0x00006E65
		public void Set(ref CopyFileMetadataAtIndexOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
			this.Index = other.Index;
		}

		// Token: 0x06000615 RID: 1557 RVA: 0x00008C8C File Offset: 0x00006E8C
		public void Set(ref CopyFileMetadataAtIndexOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
				this.Index = other.Value.Index;
			}
		}

		// Token: 0x06000616 RID: 1558 RVA: 0x00008CD7 File Offset: 0x00006ED7
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
		}

		// Token: 0x04000304 RID: 772
		private int m_ApiVersion;

		// Token: 0x04000305 RID: 773
		private IntPtr m_LocalUserId;

		// Token: 0x04000306 RID: 774
		private uint m_Index;
	}
}
