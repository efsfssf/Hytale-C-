using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sanctions
{
	// Token: 0x020001A0 RID: 416
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct GetPlayerSanctionCountOptionsInternal : ISettable<GetPlayerSanctionCountOptions>, IDisposable
	{
		// Token: 0x170002CA RID: 714
		// (set) Token: 0x06000C0F RID: 3087 RVA: 0x00011964 File Offset: 0x0000FB64
		public ProductUserId TargetUserId
		{
			set
			{
				Helper.Set(value, ref this.m_TargetUserId);
			}
		}

		// Token: 0x06000C10 RID: 3088 RVA: 0x00011974 File Offset: 0x0000FB74
		public void Set(ref GetPlayerSanctionCountOptions other)
		{
			this.m_ApiVersion = 1;
			this.TargetUserId = other.TargetUserId;
		}

		// Token: 0x06000C11 RID: 3089 RVA: 0x0001198C File Offset: 0x0000FB8C
		public void Set(ref GetPlayerSanctionCountOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.TargetUserId = other.Value.TargetUserId;
			}
		}

		// Token: 0x06000C12 RID: 3090 RVA: 0x000119C2 File Offset: 0x0000FBC2
		public void Dispose()
		{
			Helper.Dispose(ref this.m_TargetUserId);
		}

		// Token: 0x04000584 RID: 1412
		private int m_ApiVersion;

		// Token: 0x04000585 RID: 1413
		private IntPtr m_TargetUserId;
	}
}
