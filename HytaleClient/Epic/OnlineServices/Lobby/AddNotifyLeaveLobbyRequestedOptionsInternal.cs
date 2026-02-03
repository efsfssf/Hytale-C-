using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x02000362 RID: 866
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct AddNotifyLeaveLobbyRequestedOptionsInternal : ISettable<AddNotifyLeaveLobbyRequestedOptions>, IDisposable
	{
		// Token: 0x06001782 RID: 6018 RVA: 0x000225C8 File Offset: 0x000207C8
		public void Set(ref AddNotifyLeaveLobbyRequestedOptions other)
		{
			this.m_ApiVersion = 1;
		}

		// Token: 0x06001783 RID: 6019 RVA: 0x000225D4 File Offset: 0x000207D4
		public void Set(ref AddNotifyLeaveLobbyRequestedOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
			}
		}

		// Token: 0x06001784 RID: 6020 RVA: 0x000225F5 File Offset: 0x000207F5
		public void Dispose()
		{
		}

		// Token: 0x04000A63 RID: 2659
		private int m_ApiVersion;
	}
}
