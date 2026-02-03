using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x020003A8 RID: 936
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct KickMemberOptionsInternal : ISettable<KickMemberOptions>, IDisposable
	{
		// Token: 0x17000729 RID: 1833
		// (set) Token: 0x0600193E RID: 6462 RVA: 0x00024F21 File Offset: 0x00023121
		public Utf8String LobbyId
		{
			set
			{
				Helper.Set(value, ref this.m_LobbyId);
			}
		}

		// Token: 0x1700072A RID: 1834
		// (set) Token: 0x0600193F RID: 6463 RVA: 0x00024F31 File Offset: 0x00023131
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x1700072B RID: 1835
		// (set) Token: 0x06001940 RID: 6464 RVA: 0x00024F41 File Offset: 0x00023141
		public ProductUserId TargetUserId
		{
			set
			{
				Helper.Set(value, ref this.m_TargetUserId);
			}
		}

		// Token: 0x06001941 RID: 6465 RVA: 0x00024F51 File Offset: 0x00023151
		public void Set(ref KickMemberOptions other)
		{
			this.m_ApiVersion = 1;
			this.LobbyId = other.LobbyId;
			this.LocalUserId = other.LocalUserId;
			this.TargetUserId = other.TargetUserId;
		}

		// Token: 0x06001942 RID: 6466 RVA: 0x00024F84 File Offset: 0x00023184
		public void Set(ref KickMemberOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LobbyId = other.Value.LobbyId;
				this.LocalUserId = other.Value.LocalUserId;
				this.TargetUserId = other.Value.TargetUserId;
			}
		}

		// Token: 0x06001943 RID: 6467 RVA: 0x00024FE4 File Offset: 0x000231E4
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LobbyId);
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_TargetUserId);
		}

		// Token: 0x04000B2D RID: 2861
		private int m_ApiVersion;

		// Token: 0x04000B2E RID: 2862
		private IntPtr m_LobbyId;

		// Token: 0x04000B2F RID: 2863
		private IntPtr m_LocalUserId;

		// Token: 0x04000B30 RID: 2864
		private IntPtr m_TargetUserId;
	}
}
