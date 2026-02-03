using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x020000E5 RID: 229
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct AddNotifyLeaveSessionRequestedOptionsInternal : ISettable<AddNotifyLeaveSessionRequestedOptions>, IDisposable
	{
		// Token: 0x06000801 RID: 2049 RVA: 0x0000BA40 File Offset: 0x00009C40
		public void Set(ref AddNotifyLeaveSessionRequestedOptions other)
		{
			this.m_ApiVersion = 1;
		}

		// Token: 0x06000802 RID: 2050 RVA: 0x0000BA4C File Offset: 0x00009C4C
		public void Set(ref AddNotifyLeaveSessionRequestedOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
			}
		}

		// Token: 0x06000803 RID: 2051 RVA: 0x0000BA6D File Offset: 0x00009C6D
		public void Dispose()
		{
		}

		// Token: 0x040003D7 RID: 983
		private int m_ApiVersion;
	}
}
