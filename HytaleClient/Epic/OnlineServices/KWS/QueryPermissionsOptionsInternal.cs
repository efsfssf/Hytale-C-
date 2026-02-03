using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.KWS
{
	// Token: 0x020004A8 RID: 1192
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct QueryPermissionsOptionsInternal : ISettable<QueryPermissionsOptions>, IDisposable
	{
		// Token: 0x170008CF RID: 2255
		// (set) Token: 0x06001F27 RID: 7975 RVA: 0x0002D982 File Offset: 0x0002BB82
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x06001F28 RID: 7976 RVA: 0x0002D992 File Offset: 0x0002BB92
		public void Set(ref QueryPermissionsOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
		}

		// Token: 0x06001F29 RID: 7977 RVA: 0x0002D9AC File Offset: 0x0002BBAC
		public void Set(ref QueryPermissionsOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
			}
		}

		// Token: 0x06001F2A RID: 7978 RVA: 0x0002D9E2 File Offset: 0x0002BBE2
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
		}

		// Token: 0x04000D87 RID: 3463
		private int m_ApiVersion;

		// Token: 0x04000D88 RID: 3464
		private IntPtr m_LocalUserId;
	}
}
