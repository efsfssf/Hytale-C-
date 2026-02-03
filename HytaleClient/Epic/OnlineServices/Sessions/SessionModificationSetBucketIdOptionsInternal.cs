using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x02000168 RID: 360
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct SessionModificationSetBucketIdOptionsInternal : ISettable<SessionModificationSetBucketIdOptions>, IDisposable
	{
		// Token: 0x17000277 RID: 631
		// (set) Token: 0x06000ADD RID: 2781 RVA: 0x0000F6F2 File Offset: 0x0000D8F2
		public Utf8String BucketId
		{
			set
			{
				Helper.Set(value, ref this.m_BucketId);
			}
		}

		// Token: 0x06000ADE RID: 2782 RVA: 0x0000F702 File Offset: 0x0000D902
		public void Set(ref SessionModificationSetBucketIdOptions other)
		{
			this.m_ApiVersion = 1;
			this.BucketId = other.BucketId;
		}

		// Token: 0x06000ADF RID: 2783 RVA: 0x0000F71C File Offset: 0x0000D91C
		public void Set(ref SessionModificationSetBucketIdOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.BucketId = other.Value.BucketId;
			}
		}

		// Token: 0x06000AE0 RID: 2784 RVA: 0x0000F752 File Offset: 0x0000D952
		public void Dispose()
		{
			Helper.Dispose(ref this.m_BucketId);
		}

		// Token: 0x040004F3 RID: 1267
		private int m_ApiVersion;

		// Token: 0x040004F4 RID: 1268
		private IntPtr m_BucketId;
	}
}
