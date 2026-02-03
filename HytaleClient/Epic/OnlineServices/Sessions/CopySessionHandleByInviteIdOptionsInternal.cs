using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x020000F5 RID: 245
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct CopySessionHandleByInviteIdOptionsInternal : ISettable<CopySessionHandleByInviteIdOptions>, IDisposable
	{
		// Token: 0x170001AA RID: 426
		// (set) Token: 0x06000841 RID: 2113 RVA: 0x0000C12E File Offset: 0x0000A32E
		public Utf8String InviteId
		{
			set
			{
				Helper.Set(value, ref this.m_InviteId);
			}
		}

		// Token: 0x06000842 RID: 2114 RVA: 0x0000C13E File Offset: 0x0000A33E
		public void Set(ref CopySessionHandleByInviteIdOptions other)
		{
			this.m_ApiVersion = 1;
			this.InviteId = other.InviteId;
		}

		// Token: 0x06000843 RID: 2115 RVA: 0x0000C158 File Offset: 0x0000A358
		public void Set(ref CopySessionHandleByInviteIdOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.InviteId = other.Value.InviteId;
			}
		}

		// Token: 0x06000844 RID: 2116 RVA: 0x0000C18E File Offset: 0x0000A38E
		public void Dispose()
		{
			Helper.Dispose(ref this.m_InviteId);
		}

		// Token: 0x040003EF RID: 1007
		private int m_ApiVersion;

		// Token: 0x040003F0 RID: 1008
		private IntPtr m_InviteId;
	}
}
