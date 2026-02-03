using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x0200016C RID: 364
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct SessionModificationSetInvitesAllowedOptionsInternal : ISettable<SessionModificationSetInvitesAllowedOptions>, IDisposable
	{
		// Token: 0x1700027B RID: 635
		// (set) Token: 0x06000AE9 RID: 2793 RVA: 0x0000F7F2 File Offset: 0x0000D9F2
		public bool InvitesAllowed
		{
			set
			{
				Helper.Set(value, ref this.m_InvitesAllowed);
			}
		}

		// Token: 0x06000AEA RID: 2794 RVA: 0x0000F802 File Offset: 0x0000DA02
		public void Set(ref SessionModificationSetInvitesAllowedOptions other)
		{
			this.m_ApiVersion = 1;
			this.InvitesAllowed = other.InvitesAllowed;
		}

		// Token: 0x06000AEB RID: 2795 RVA: 0x0000F81C File Offset: 0x0000DA1C
		public void Set(ref SessionModificationSetInvitesAllowedOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.InvitesAllowed = other.Value.InvitesAllowed;
			}
		}

		// Token: 0x06000AEC RID: 2796 RVA: 0x0000F852 File Offset: 0x0000DA52
		public void Dispose()
		{
		}

		// Token: 0x040004F9 RID: 1273
		private int m_ApiVersion;

		// Token: 0x040004FA RID: 1274
		private int m_InvitesAllowed;
	}
}
