using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.KWS
{
	// Token: 0x0200048E RID: 1166
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct GetPermissionsCountOptionsInternal : ISettable<GetPermissionsCountOptions>, IDisposable
	{
		// Token: 0x170008A4 RID: 2212
		// (set) Token: 0x06001E76 RID: 7798 RVA: 0x0002C9E3 File Offset: 0x0002ABE3
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x06001E77 RID: 7799 RVA: 0x0002C9F3 File Offset: 0x0002ABF3
		public void Set(ref GetPermissionsCountOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
		}

		// Token: 0x06001E78 RID: 7800 RVA: 0x0002CA0C File Offset: 0x0002AC0C
		public void Set(ref GetPermissionsCountOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
			}
		}

		// Token: 0x06001E79 RID: 7801 RVA: 0x0002CA42 File Offset: 0x0002AC42
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
		}

		// Token: 0x04000D4C RID: 3404
		private int m_ApiVersion;

		// Token: 0x04000D4D RID: 3405
		private IntPtr m_LocalUserId;
	}
}
