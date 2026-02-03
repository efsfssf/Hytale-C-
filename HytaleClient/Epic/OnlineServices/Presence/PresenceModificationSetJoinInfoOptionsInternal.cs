using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Presence
{
	// Token: 0x020002DB RID: 731
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct PresenceModificationSetJoinInfoOptionsInternal : ISettable<PresenceModificationSetJoinInfoOptions>, IDisposable
	{
		// Token: 0x17000560 RID: 1376
		// (set) Token: 0x06001424 RID: 5156 RVA: 0x0001D702 File Offset: 0x0001B902
		public Utf8String JoinInfo
		{
			set
			{
				Helper.Set(value, ref this.m_JoinInfo);
			}
		}

		// Token: 0x06001425 RID: 5157 RVA: 0x0001D712 File Offset: 0x0001B912
		public void Set(ref PresenceModificationSetJoinInfoOptions other)
		{
			this.m_ApiVersion = 1;
			this.JoinInfo = other.JoinInfo;
		}

		// Token: 0x06001426 RID: 5158 RVA: 0x0001D72C File Offset: 0x0001B92C
		public void Set(ref PresenceModificationSetJoinInfoOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.JoinInfo = other.Value.JoinInfo;
			}
		}

		// Token: 0x06001427 RID: 5159 RVA: 0x0001D762 File Offset: 0x0001B962
		public void Dispose()
		{
			Helper.Dispose(ref this.m_JoinInfo);
		}

		// Token: 0x040008DA RID: 2266
		private int m_ApiVersion;

		// Token: 0x040008DB RID: 2267
		private IntPtr m_JoinInfo;
	}
}
