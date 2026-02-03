using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatClient
{
	// Token: 0x02000701 RID: 1793
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct UnregisterPeerOptionsInternal : ISettable<UnregisterPeerOptions>, IDisposable
	{
		// Token: 0x17000DBC RID: 3516
		// (set) Token: 0x06002E1A RID: 11802 RVA: 0x0004407B File Offset: 0x0004227B
		public IntPtr PeerHandle
		{
			set
			{
				this.m_PeerHandle = value;
			}
		}

		// Token: 0x06002E1B RID: 11803 RVA: 0x00044085 File Offset: 0x00042285
		public void Set(ref UnregisterPeerOptions other)
		{
			this.m_ApiVersion = 1;
			this.PeerHandle = other.PeerHandle;
		}

		// Token: 0x06002E1C RID: 11804 RVA: 0x0004409C File Offset: 0x0004229C
		public void Set(ref UnregisterPeerOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.PeerHandle = other.Value.PeerHandle;
			}
		}

		// Token: 0x06002E1D RID: 11805 RVA: 0x000440D2 File Offset: 0x000422D2
		public void Dispose()
		{
			Helper.Dispose(ref this.m_PeerHandle);
		}

		// Token: 0x0400145E RID: 5214
		private int m_ApiVersion;

		// Token: 0x0400145F RID: 5215
		private IntPtr m_PeerHandle;
	}
}
