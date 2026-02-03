using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x02000404 RID: 1028
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct LobbySearchSetTargetUserIdOptionsInternal : ISettable<LobbySearchSetTargetUserIdOptions>, IDisposable
	{
		// Token: 0x170007D6 RID: 2006
		// (set) Token: 0x06001B7C RID: 7036 RVA: 0x000293D7 File Offset: 0x000275D7
		public ProductUserId TargetUserId
		{
			set
			{
				Helper.Set(value, ref this.m_TargetUserId);
			}
		}

		// Token: 0x06001B7D RID: 7037 RVA: 0x000293E7 File Offset: 0x000275E7
		public void Set(ref LobbySearchSetTargetUserIdOptions other)
		{
			this.m_ApiVersion = 1;
			this.TargetUserId = other.TargetUserId;
		}

		// Token: 0x06001B7E RID: 7038 RVA: 0x00029400 File Offset: 0x00027600
		public void Set(ref LobbySearchSetTargetUserIdOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.TargetUserId = other.Value.TargetUserId;
			}
		}

		// Token: 0x06001B7F RID: 7039 RVA: 0x00029436 File Offset: 0x00027636
		public void Dispose()
		{
			Helper.Dispose(ref this.m_TargetUserId);
		}

		// Token: 0x04000C55 RID: 3157
		private int m_ApiVersion;

		// Token: 0x04000C56 RID: 3158
		private IntPtr m_TargetUserId;
	}
}
