using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.KWS
{
	// Token: 0x02000486 RID: 1158
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct CopyPermissionByIndexOptionsInternal : ISettable<CopyPermissionByIndexOptions>, IDisposable
	{
		// Token: 0x1700088C RID: 2188
		// (set) Token: 0x06001E3F RID: 7743 RVA: 0x0002C472 File Offset: 0x0002A672
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x1700088D RID: 2189
		// (set) Token: 0x06001E40 RID: 7744 RVA: 0x0002C482 File Offset: 0x0002A682
		public uint Index
		{
			set
			{
				this.m_Index = value;
			}
		}

		// Token: 0x06001E41 RID: 7745 RVA: 0x0002C48C File Offset: 0x0002A68C
		public void Set(ref CopyPermissionByIndexOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
			this.Index = other.Index;
		}

		// Token: 0x06001E42 RID: 7746 RVA: 0x0002C4B0 File Offset: 0x0002A6B0
		public void Set(ref CopyPermissionByIndexOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
				this.Index = other.Value.Index;
			}
		}

		// Token: 0x06001E43 RID: 7747 RVA: 0x0002C4FB File Offset: 0x0002A6FB
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
		}

		// Token: 0x04000D32 RID: 3378
		private int m_ApiVersion;

		// Token: 0x04000D33 RID: 3379
		private IntPtr m_LocalUserId;

		// Token: 0x04000D34 RID: 3380
		private uint m_Index;
	}
}
