using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x020000E7 RID: 231
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct AddNotifySendSessionNativeInviteRequestedOptionsInternal : ISettable<AddNotifySendSessionNativeInviteRequestedOptions>, IDisposable
	{
		// Token: 0x06000804 RID: 2052 RVA: 0x0000BA70 File Offset: 0x00009C70
		public void Set(ref AddNotifySendSessionNativeInviteRequestedOptions other)
		{
			this.m_ApiVersion = 1;
		}

		// Token: 0x06000805 RID: 2053 RVA: 0x0000BA7C File Offset: 0x00009C7C
		public void Set(ref AddNotifySendSessionNativeInviteRequestedOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
			}
		}

		// Token: 0x06000806 RID: 2054 RVA: 0x0000BA9D File Offset: 0x00009C9D
		public void Dispose()
		{
		}

		// Token: 0x040003D8 RID: 984
		private int m_ApiVersion;
	}
}
