using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x020003AC RID: 940
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct LeaveLobbyOptionsInternal : ISettable<LeaveLobbyOptions>, IDisposable
	{
		// Token: 0x17000735 RID: 1845
		// (set) Token: 0x0600195B RID: 6491 RVA: 0x000251FC File Offset: 0x000233FC
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x17000736 RID: 1846
		// (set) Token: 0x0600195C RID: 6492 RVA: 0x0002520C File Offset: 0x0002340C
		public Utf8String LobbyId
		{
			set
			{
				Helper.Set(value, ref this.m_LobbyId);
			}
		}

		// Token: 0x0600195D RID: 6493 RVA: 0x0002521C File Offset: 0x0002341C
		public void Set(ref LeaveLobbyOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
			this.LobbyId = other.LobbyId;
		}

		// Token: 0x0600195E RID: 6494 RVA: 0x00025240 File Offset: 0x00023440
		public void Set(ref LeaveLobbyOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
				this.LobbyId = other.Value.LobbyId;
			}
		}

		// Token: 0x0600195F RID: 6495 RVA: 0x0002528B File Offset: 0x0002348B
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_LobbyId);
		}

		// Token: 0x04000B39 RID: 2873
		private int m_ApiVersion;

		// Token: 0x04000B3A RID: 2874
		private IntPtr m_LocalUserId;

		// Token: 0x04000B3B RID: 2875
		private IntPtr m_LobbyId;
	}
}
