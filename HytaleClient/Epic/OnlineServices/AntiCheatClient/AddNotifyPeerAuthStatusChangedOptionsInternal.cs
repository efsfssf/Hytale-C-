using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatClient
{
	// Token: 0x020006DA RID: 1754
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct AddNotifyPeerAuthStatusChangedOptionsInternal : ISettable<AddNotifyPeerAuthStatusChangedOptions>, IDisposable
	{
		// Token: 0x06002D5A RID: 11610 RVA: 0x00042F58 File Offset: 0x00041158
		public void Set(ref AddNotifyPeerAuthStatusChangedOptions other)
		{
			this.m_ApiVersion = 1;
		}

		// Token: 0x06002D5B RID: 11611 RVA: 0x00042F64 File Offset: 0x00041164
		public void Set(ref AddNotifyPeerAuthStatusChangedOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
			}
		}

		// Token: 0x06002D5C RID: 11612 RVA: 0x00042F85 File Offset: 0x00041185
		public void Dispose()
		{
		}

		// Token: 0x040013F4 RID: 5108
		private int m_ApiVersion;
	}
}
