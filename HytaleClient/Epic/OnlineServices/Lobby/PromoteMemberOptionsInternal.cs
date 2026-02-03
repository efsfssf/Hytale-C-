using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x0200043E RID: 1086
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct PromoteMemberOptionsInternal : ISettable<PromoteMemberOptions>, IDisposable
	{
		// Token: 0x170007F0 RID: 2032
		// (set) Token: 0x06001C83 RID: 7299 RVA: 0x00029A2D File Offset: 0x00027C2D
		public Utf8String LobbyId
		{
			set
			{
				Helper.Set(value, ref this.m_LobbyId);
			}
		}

		// Token: 0x170007F1 RID: 2033
		// (set) Token: 0x06001C84 RID: 7300 RVA: 0x00029A3D File Offset: 0x00027C3D
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x170007F2 RID: 2034
		// (set) Token: 0x06001C85 RID: 7301 RVA: 0x00029A4D File Offset: 0x00027C4D
		public ProductUserId TargetUserId
		{
			set
			{
				Helper.Set(value, ref this.m_TargetUserId);
			}
		}

		// Token: 0x06001C86 RID: 7302 RVA: 0x00029A5D File Offset: 0x00027C5D
		public void Set(ref PromoteMemberOptions other)
		{
			this.m_ApiVersion = 1;
			this.LobbyId = other.LobbyId;
			this.LocalUserId = other.LocalUserId;
			this.TargetUserId = other.TargetUserId;
		}

		// Token: 0x06001C87 RID: 7303 RVA: 0x00029A90 File Offset: 0x00027C90
		public void Set(ref PromoteMemberOptions? other)
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

		// Token: 0x06001C88 RID: 7304 RVA: 0x00029AF0 File Offset: 0x00027CF0
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LobbyId);
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_TargetUserId);
		}

		// Token: 0x04000C70 RID: 3184
		private int m_ApiVersion;

		// Token: 0x04000C71 RID: 3185
		private IntPtr m_LobbyId;

		// Token: 0x04000C72 RID: 3186
		private IntPtr m_LocalUserId;

		// Token: 0x04000C73 RID: 3187
		private IntPtr m_TargetUserId;
	}
}
