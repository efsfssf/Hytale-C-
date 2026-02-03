using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x020000F9 RID: 249
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct CopySessionHandleForPresenceOptionsInternal : ISettable<CopySessionHandleForPresenceOptions>, IDisposable
	{
		// Token: 0x170001AE RID: 430
		// (set) Token: 0x0600084D RID: 2125 RVA: 0x0000C21A File Offset: 0x0000A41A
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x0600084E RID: 2126 RVA: 0x0000C22A File Offset: 0x0000A42A
		public void Set(ref CopySessionHandleForPresenceOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
		}

		// Token: 0x0600084F RID: 2127 RVA: 0x0000C244 File Offset: 0x0000A444
		public void Set(ref CopySessionHandleForPresenceOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
			}
		}

		// Token: 0x06000850 RID: 2128 RVA: 0x0000C27A File Offset: 0x0000A47A
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
		}

		// Token: 0x040003F5 RID: 1013
		private int m_ApiVersion;

		// Token: 0x040003F6 RID: 1014
		private IntPtr m_LocalUserId;
	}
}
