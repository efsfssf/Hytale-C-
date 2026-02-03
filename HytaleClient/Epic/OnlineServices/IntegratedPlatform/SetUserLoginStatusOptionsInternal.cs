using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.IntegratedPlatform
{
	// Token: 0x020004C6 RID: 1222
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct SetUserLoginStatusOptionsInternal : ISettable<SetUserLoginStatusOptions>, IDisposable
	{
		// Token: 0x170008F7 RID: 2295
		// (set) Token: 0x06001FB4 RID: 8116 RVA: 0x0002E5C0 File Offset: 0x0002C7C0
		public Utf8String PlatformType
		{
			set
			{
				Helper.Set(value, ref this.m_PlatformType);
			}
		}

		// Token: 0x170008F8 RID: 2296
		// (set) Token: 0x06001FB5 RID: 8117 RVA: 0x0002E5D0 File Offset: 0x0002C7D0
		public Utf8String LocalPlatformUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalPlatformUserId);
			}
		}

		// Token: 0x170008F9 RID: 2297
		// (set) Token: 0x06001FB6 RID: 8118 RVA: 0x0002E5E0 File Offset: 0x0002C7E0
		public LoginStatus CurrentLoginStatus
		{
			set
			{
				this.m_CurrentLoginStatus = value;
			}
		}

		// Token: 0x06001FB7 RID: 8119 RVA: 0x0002E5EA File Offset: 0x0002C7EA
		public void Set(ref SetUserLoginStatusOptions other)
		{
			this.m_ApiVersion = 1;
			this.PlatformType = other.PlatformType;
			this.LocalPlatformUserId = other.LocalPlatformUserId;
			this.CurrentLoginStatus = other.CurrentLoginStatus;
		}

		// Token: 0x06001FB8 RID: 8120 RVA: 0x0002E61C File Offset: 0x0002C81C
		public void Set(ref SetUserLoginStatusOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.PlatformType = other.Value.PlatformType;
				this.LocalPlatformUserId = other.Value.LocalPlatformUserId;
				this.CurrentLoginStatus = other.Value.CurrentLoginStatus;
			}
		}

		// Token: 0x06001FB9 RID: 8121 RVA: 0x0002E67C File Offset: 0x0002C87C
		public void Dispose()
		{
			Helper.Dispose(ref this.m_PlatformType);
			Helper.Dispose(ref this.m_LocalPlatformUserId);
		}

		// Token: 0x04000DCD RID: 3533
		private int m_ApiVersion;

		// Token: 0x04000DCE RID: 3534
		private IntPtr m_PlatformType;

		// Token: 0x04000DCF RID: 3535
		private IntPtr m_LocalPlatformUserId;

		// Token: 0x04000DD0 RID: 3536
		private LoginStatus m_CurrentLoginStatus;
	}
}
