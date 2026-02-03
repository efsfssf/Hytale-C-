using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sanctions
{
	// Token: 0x02000198 RID: 408
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct CopyPlayerSanctionByIndexOptionsInternal : ISettable<CopyPlayerSanctionByIndexOptions>, IDisposable
	{
		// Token: 0x170002BA RID: 698
		// (set) Token: 0x06000BE1 RID: 3041 RVA: 0x000115DF File Offset: 0x0000F7DF
		public ProductUserId TargetUserId
		{
			set
			{
				Helper.Set(value, ref this.m_TargetUserId);
			}
		}

		// Token: 0x170002BB RID: 699
		// (set) Token: 0x06000BE2 RID: 3042 RVA: 0x000115EF File Offset: 0x0000F7EF
		public uint SanctionIndex
		{
			set
			{
				this.m_SanctionIndex = value;
			}
		}

		// Token: 0x06000BE3 RID: 3043 RVA: 0x000115F9 File Offset: 0x0000F7F9
		public void Set(ref CopyPlayerSanctionByIndexOptions other)
		{
			this.m_ApiVersion = 1;
			this.TargetUserId = other.TargetUserId;
			this.SanctionIndex = other.SanctionIndex;
		}

		// Token: 0x06000BE4 RID: 3044 RVA: 0x00011620 File Offset: 0x0000F820
		public void Set(ref CopyPlayerSanctionByIndexOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.TargetUserId = other.Value.TargetUserId;
				this.SanctionIndex = other.Value.SanctionIndex;
			}
		}

		// Token: 0x06000BE5 RID: 3045 RVA: 0x0001166B File Offset: 0x0000F86B
		public void Dispose()
		{
			Helper.Dispose(ref this.m_TargetUserId);
		}

		// Token: 0x04000573 RID: 1395
		private int m_ApiVersion;

		// Token: 0x04000574 RID: 1396
		private IntPtr m_TargetUserId;

		// Token: 0x04000575 RID: 1397
		private uint m_SanctionIndex;
	}
}
