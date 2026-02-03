using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.PlayerDataStorage
{
	// Token: 0x020002EC RID: 748
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct CopyFileMetadataAtIndexOptionsInternal : ISettable<CopyFileMetadataAtIndexOptions>, IDisposable
	{
		// Token: 0x1700057F RID: 1407
		// (set) Token: 0x0600147C RID: 5244 RVA: 0x0001DE38 File Offset: 0x0001C038
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x17000580 RID: 1408
		// (set) Token: 0x0600147D RID: 5245 RVA: 0x0001DE48 File Offset: 0x0001C048
		public uint Index
		{
			set
			{
				this.m_Index = value;
			}
		}

		// Token: 0x0600147E RID: 5246 RVA: 0x0001DE52 File Offset: 0x0001C052
		public void Set(ref CopyFileMetadataAtIndexOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
			this.Index = other.Index;
		}

		// Token: 0x0600147F RID: 5247 RVA: 0x0001DE78 File Offset: 0x0001C078
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

		// Token: 0x06001480 RID: 5248 RVA: 0x0001DEC3 File Offset: 0x0001C0C3
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
		}

		// Token: 0x04000902 RID: 2306
		private int m_ApiVersion;

		// Token: 0x04000903 RID: 2307
		private IntPtr m_LocalUserId;

		// Token: 0x04000904 RID: 2308
		private uint m_Index;
	}
}
