using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x02000394 RID: 916
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct HardMuteMemberOptionsInternal : ISettable<HardMuteMemberOptions>, IDisposable
	{
		// Token: 0x170006DD RID: 1757
		// (set) Token: 0x06001893 RID: 6291 RVA: 0x00023EFF File Offset: 0x000220FF
		public Utf8String LobbyId
		{
			set
			{
				Helper.Set(value, ref this.m_LobbyId);
			}
		}

		// Token: 0x170006DE RID: 1758
		// (set) Token: 0x06001894 RID: 6292 RVA: 0x00023F0F File Offset: 0x0002210F
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x170006DF RID: 1759
		// (set) Token: 0x06001895 RID: 6293 RVA: 0x00023F1F File Offset: 0x0002211F
		public ProductUserId TargetUserId
		{
			set
			{
				Helper.Set(value, ref this.m_TargetUserId);
			}
		}

		// Token: 0x170006E0 RID: 1760
		// (set) Token: 0x06001896 RID: 6294 RVA: 0x00023F2F File Offset: 0x0002212F
		public bool HardMute
		{
			set
			{
				Helper.Set(value, ref this.m_HardMute);
			}
		}

		// Token: 0x06001897 RID: 6295 RVA: 0x00023F3F File Offset: 0x0002213F
		public void Set(ref HardMuteMemberOptions other)
		{
			this.m_ApiVersion = 1;
			this.LobbyId = other.LobbyId;
			this.LocalUserId = other.LocalUserId;
			this.TargetUserId = other.TargetUserId;
			this.HardMute = other.HardMute;
		}

		// Token: 0x06001898 RID: 6296 RVA: 0x00023F80 File Offset: 0x00022180
		public void Set(ref HardMuteMemberOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LobbyId = other.Value.LobbyId;
				this.LocalUserId = other.Value.LocalUserId;
				this.TargetUserId = other.Value.TargetUserId;
				this.HardMute = other.Value.HardMute;
			}
		}

		// Token: 0x06001899 RID: 6297 RVA: 0x00023FF5 File Offset: 0x000221F5
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LobbyId);
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_TargetUserId);
		}

		// Token: 0x04000AE1 RID: 2785
		private int m_ApiVersion;

		// Token: 0x04000AE2 RID: 2786
		private IntPtr m_LobbyId;

		// Token: 0x04000AE3 RID: 2787
		private IntPtr m_LocalUserId;

		// Token: 0x04000AE4 RID: 2788
		private IntPtr m_TargetUserId;

		// Token: 0x04000AE5 RID: 2789
		private int m_HardMute;
	}
}
