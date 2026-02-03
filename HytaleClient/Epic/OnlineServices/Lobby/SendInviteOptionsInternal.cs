using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x0200044C RID: 1100
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct SendInviteOptionsInternal : ISettable<SendInviteOptions>, IDisposable
	{
		// Token: 0x1700081C RID: 2076
		// (set) Token: 0x06001CF2 RID: 7410 RVA: 0x0002A4D9 File Offset: 0x000286D9
		public Utf8String LobbyId
		{
			set
			{
				Helper.Set(value, ref this.m_LobbyId);
			}
		}

		// Token: 0x1700081D RID: 2077
		// (set) Token: 0x06001CF3 RID: 7411 RVA: 0x0002A4E9 File Offset: 0x000286E9
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x1700081E RID: 2078
		// (set) Token: 0x06001CF4 RID: 7412 RVA: 0x0002A4F9 File Offset: 0x000286F9
		public ProductUserId TargetUserId
		{
			set
			{
				Helper.Set(value, ref this.m_TargetUserId);
			}
		}

		// Token: 0x06001CF5 RID: 7413 RVA: 0x0002A509 File Offset: 0x00028709
		public void Set(ref SendInviteOptions other)
		{
			this.m_ApiVersion = 1;
			this.LobbyId = other.LobbyId;
			this.LocalUserId = other.LocalUserId;
			this.TargetUserId = other.TargetUserId;
		}

		// Token: 0x06001CF6 RID: 7414 RVA: 0x0002A53C File Offset: 0x0002873C
		public void Set(ref SendInviteOptions? other)
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

		// Token: 0x06001CF7 RID: 7415 RVA: 0x0002A59C File Offset: 0x0002879C
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LobbyId);
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_TargetUserId);
		}

		// Token: 0x04000C9B RID: 3227
		private int m_ApiVersion;

		// Token: 0x04000C9C RID: 3228
		private IntPtr m_LobbyId;

		// Token: 0x04000C9D RID: 3229
		private IntPtr m_LocalUserId;

		// Token: 0x04000C9E RID: 3230
		private IntPtr m_TargetUserId;
	}
}
