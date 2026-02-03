using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x020003C0 RID: 960
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct LobbyDetailsCopyMemberInfoOptionsInternal : ISettable<LobbyDetailsCopyMemberInfoOptions>, IDisposable
	{
		// Token: 0x17000756 RID: 1878
		// (set) Token: 0x060019C0 RID: 6592 RVA: 0x00025D7F File Offset: 0x00023F7F
		public ProductUserId TargetUserId
		{
			set
			{
				Helper.Set(value, ref this.m_TargetUserId);
			}
		}

		// Token: 0x060019C1 RID: 6593 RVA: 0x00025D8F File Offset: 0x00023F8F
		public void Set(ref LobbyDetailsCopyMemberInfoOptions other)
		{
			this.m_ApiVersion = 1;
			this.TargetUserId = other.TargetUserId;
		}

		// Token: 0x060019C2 RID: 6594 RVA: 0x00025DA8 File Offset: 0x00023FA8
		public void Set(ref LobbyDetailsCopyMemberInfoOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.TargetUserId = other.Value.TargetUserId;
			}
		}

		// Token: 0x060019C3 RID: 6595 RVA: 0x00025DDE File Offset: 0x00023FDE
		public void Dispose()
		{
			Helper.Dispose(ref this.m_TargetUserId);
		}

		// Token: 0x04000B6F RID: 2927
		private int m_ApiVersion;

		// Token: 0x04000B70 RID: 2928
		private IntPtr m_TargetUserId;
	}
}
