using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatCommon
{
	// Token: 0x020006CC RID: 1740
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct SetGameSessionIdOptionsInternal : ISettable<SetGameSessionIdOptions>, IDisposable
	{
		// Token: 0x17000D82 RID: 3458
		// (set) Token: 0x06002D33 RID: 11571 RVA: 0x00042C4C File Offset: 0x00040E4C
		public Utf8String GameSessionId
		{
			set
			{
				Helper.Set(value, ref this.m_GameSessionId);
			}
		}

		// Token: 0x06002D34 RID: 11572 RVA: 0x00042C5C File Offset: 0x00040E5C
		public void Set(ref SetGameSessionIdOptions other)
		{
			this.m_ApiVersion = 1;
			this.GameSessionId = other.GameSessionId;
		}

		// Token: 0x06002D35 RID: 11573 RVA: 0x00042C74 File Offset: 0x00040E74
		public void Set(ref SetGameSessionIdOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.GameSessionId = other.Value.GameSessionId;
			}
		}

		// Token: 0x06002D36 RID: 11574 RVA: 0x00042CAA File Offset: 0x00040EAA
		public void Dispose()
		{
			Helper.Dispose(ref this.m_GameSessionId);
		}

		// Token: 0x040013E5 RID: 5093
		private int m_ApiVersion;

		// Token: 0x040013E6 RID: 5094
		private IntPtr m_GameSessionId;
	}
}
