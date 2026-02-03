using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x02000172 RID: 370
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct SessionModificationSetPermissionLevelOptionsInternal : ISettable<SessionModificationSetPermissionLevelOptions>, IDisposable
	{
		// Token: 0x17000281 RID: 641
		// (set) Token: 0x06000AFB RID: 2811 RVA: 0x0000F946 File Offset: 0x0000DB46
		public OnlineSessionPermissionLevel PermissionLevel
		{
			set
			{
				this.m_PermissionLevel = value;
			}
		}

		// Token: 0x06000AFC RID: 2812 RVA: 0x0000F950 File Offset: 0x0000DB50
		public void Set(ref SessionModificationSetPermissionLevelOptions other)
		{
			this.m_ApiVersion = 1;
			this.PermissionLevel = other.PermissionLevel;
		}

		// Token: 0x06000AFD RID: 2813 RVA: 0x0000F968 File Offset: 0x0000DB68
		public void Set(ref SessionModificationSetPermissionLevelOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.PermissionLevel = other.Value.PermissionLevel;
			}
		}

		// Token: 0x06000AFE RID: 2814 RVA: 0x0000F99E File Offset: 0x0000DB9E
		public void Dispose()
		{
		}

		// Token: 0x04000502 RID: 1282
		private int m_ApiVersion;

		// Token: 0x04000503 RID: 1283
		private OnlineSessionPermissionLevel m_PermissionLevel;
	}
}
