using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatClient
{
	// Token: 0x020006D4 RID: 1748
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct AddNotifyMessageToPeerOptionsInternal : ISettable<AddNotifyMessageToPeerOptions>, IDisposable
	{
		// Token: 0x06002D51 RID: 11601 RVA: 0x00042EC8 File Offset: 0x000410C8
		public void Set(ref AddNotifyMessageToPeerOptions other)
		{
			this.m_ApiVersion = 1;
		}

		// Token: 0x06002D52 RID: 11602 RVA: 0x00042ED4 File Offset: 0x000410D4
		public void Set(ref AddNotifyMessageToPeerOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
			}
		}

		// Token: 0x06002D53 RID: 11603 RVA: 0x00042EF5 File Offset: 0x000410F5
		public void Dispose()
		{
		}

		// Token: 0x040013F1 RID: 5105
		private int m_ApiVersion;
	}
}
