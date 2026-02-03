using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.IntegratedPlatform
{
	// Token: 0x020004B8 RID: 1208
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct FinalizeDeferredUserLogoutOptionsInternal : ISettable<FinalizeDeferredUserLogoutOptions>, IDisposable
	{
		// Token: 0x170008E9 RID: 2281
		// (set) Token: 0x06001F72 RID: 8050 RVA: 0x0002DFEF File Offset: 0x0002C1EF
		public Utf8String PlatformType
		{
			set
			{
				Helper.Set(value, ref this.m_PlatformType);
			}
		}

		// Token: 0x170008EA RID: 2282
		// (set) Token: 0x06001F73 RID: 8051 RVA: 0x0002DFFF File Offset: 0x0002C1FF
		public Utf8String LocalPlatformUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalPlatformUserId);
			}
		}

		// Token: 0x170008EB RID: 2283
		// (set) Token: 0x06001F74 RID: 8052 RVA: 0x0002E00F File Offset: 0x0002C20F
		public LoginStatus ExpectedLoginStatus
		{
			set
			{
				this.m_ExpectedLoginStatus = value;
			}
		}

		// Token: 0x06001F75 RID: 8053 RVA: 0x0002E019 File Offset: 0x0002C219
		public void Set(ref FinalizeDeferredUserLogoutOptions other)
		{
			this.m_ApiVersion = 1;
			this.PlatformType = other.PlatformType;
			this.LocalPlatformUserId = other.LocalPlatformUserId;
			this.ExpectedLoginStatus = other.ExpectedLoginStatus;
		}

		// Token: 0x06001F76 RID: 8054 RVA: 0x0002E04C File Offset: 0x0002C24C
		public void Set(ref FinalizeDeferredUserLogoutOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.PlatformType = other.Value.PlatformType;
				this.LocalPlatformUserId = other.Value.LocalPlatformUserId;
				this.ExpectedLoginStatus = other.Value.ExpectedLoginStatus;
			}
		}

		// Token: 0x06001F77 RID: 8055 RVA: 0x0002E0AC File Offset: 0x0002C2AC
		public void Dispose()
		{
			Helper.Dispose(ref this.m_PlatformType);
			Helper.Dispose(ref this.m_LocalPlatformUserId);
		}

		// Token: 0x04000DA6 RID: 3494
		private int m_ApiVersion;

		// Token: 0x04000DA7 RID: 3495
		private IntPtr m_PlatformType;

		// Token: 0x04000DA8 RID: 3496
		private IntPtr m_LocalPlatformUserId;

		// Token: 0x04000DA9 RID: 3497
		private LoginStatus m_ExpectedLoginStatus;
	}
}
