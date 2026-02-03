using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x0200037A RID: 890
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct CopyLobbyDetailsHandleByInviteIdOptionsInternal : ISettable<CopyLobbyDetailsHandleByInviteIdOptions>, IDisposable
	{
		// Token: 0x17000689 RID: 1673
		// (set) Token: 0x060017DB RID: 6107 RVA: 0x00022EBB File Offset: 0x000210BB
		public Utf8String InviteId
		{
			set
			{
				Helper.Set(value, ref this.m_InviteId);
			}
		}

		// Token: 0x060017DC RID: 6108 RVA: 0x00022ECB File Offset: 0x000210CB
		public void Set(ref CopyLobbyDetailsHandleByInviteIdOptions other)
		{
			this.m_ApiVersion = 1;
			this.InviteId = other.InviteId;
		}

		// Token: 0x060017DD RID: 6109 RVA: 0x00022EE4 File Offset: 0x000210E4
		public void Set(ref CopyLobbyDetailsHandleByInviteIdOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.InviteId = other.Value.InviteId;
			}
		}

		// Token: 0x060017DE RID: 6110 RVA: 0x00022F1A File Offset: 0x0002111A
		public void Dispose()
		{
			Helper.Dispose(ref this.m_InviteId);
		}

		// Token: 0x04000A85 RID: 2693
		private int m_ApiVersion;

		// Token: 0x04000A86 RID: 2694
		private IntPtr m_InviteId;
	}
}
