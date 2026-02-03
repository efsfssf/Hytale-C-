using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Auth
{
	// Token: 0x02000639 RID: 1593
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct DeletePersistentAuthOptionsInternal : ISettable<DeletePersistentAuthOptions>, IDisposable
	{
		// Token: 0x17000C03 RID: 3075
		// (set) Token: 0x06002941 RID: 10561 RVA: 0x0003CCFA File Offset: 0x0003AEFA
		public Utf8String RefreshToken
		{
			set
			{
				Helper.Set(value, ref this.m_RefreshToken);
			}
		}

		// Token: 0x06002942 RID: 10562 RVA: 0x0003CD0A File Offset: 0x0003AF0A
		public void Set(ref DeletePersistentAuthOptions other)
		{
			this.m_ApiVersion = 2;
			this.RefreshToken = other.RefreshToken;
		}

		// Token: 0x06002943 RID: 10563 RVA: 0x0003CD24 File Offset: 0x0003AF24
		public void Set(ref DeletePersistentAuthOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 2;
				this.RefreshToken = other.Value.RefreshToken;
			}
		}

		// Token: 0x06002944 RID: 10564 RVA: 0x0003CD5A File Offset: 0x0003AF5A
		public void Dispose()
		{
			Helper.Dispose(ref this.m_RefreshToken);
		}

		// Token: 0x040011BD RID: 4541
		private int m_ApiVersion;

		// Token: 0x040011BE RID: 4542
		private IntPtr m_RefreshToken;
	}
}
