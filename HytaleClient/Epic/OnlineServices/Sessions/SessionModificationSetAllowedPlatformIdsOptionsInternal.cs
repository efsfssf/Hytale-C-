using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x02000166 RID: 358
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct SessionModificationSetAllowedPlatformIdsOptionsInternal : ISettable<SessionModificationSetAllowedPlatformIdsOptions>, IDisposable
	{
		// Token: 0x17000275 RID: 629
		// (set) Token: 0x06000AD7 RID: 2775 RVA: 0x0000F66E File Offset: 0x0000D86E
		public uint[] AllowedPlatformIds
		{
			set
			{
				Helper.Set<uint>(value, ref this.m_AllowedPlatformIds, out this.m_AllowedPlatformIdsCount);
			}
		}

		// Token: 0x06000AD8 RID: 2776 RVA: 0x0000F684 File Offset: 0x0000D884
		public void Set(ref SessionModificationSetAllowedPlatformIdsOptions other)
		{
			this.m_ApiVersion = 1;
			this.AllowedPlatformIds = other.AllowedPlatformIds;
		}

		// Token: 0x06000AD9 RID: 2777 RVA: 0x0000F69C File Offset: 0x0000D89C
		public void Set(ref SessionModificationSetAllowedPlatformIdsOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.AllowedPlatformIds = other.Value.AllowedPlatformIds;
			}
		}

		// Token: 0x06000ADA RID: 2778 RVA: 0x0000F6D2 File Offset: 0x0000D8D2
		public void Dispose()
		{
			Helper.Dispose(ref this.m_AllowedPlatformIds);
		}

		// Token: 0x040004EF RID: 1263
		private int m_ApiVersion;

		// Token: 0x040004F0 RID: 1264
		private IntPtr m_AllowedPlatformIds;

		// Token: 0x040004F1 RID: 1265
		private uint m_AllowedPlatformIdsCount;
	}
}
