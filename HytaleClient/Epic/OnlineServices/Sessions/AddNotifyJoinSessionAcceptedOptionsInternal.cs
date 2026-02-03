using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x020000E3 RID: 227
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct AddNotifyJoinSessionAcceptedOptionsInternal : ISettable<AddNotifyJoinSessionAcceptedOptions>, IDisposable
	{
		// Token: 0x060007FE RID: 2046 RVA: 0x0000BA12 File Offset: 0x00009C12
		public void Set(ref AddNotifyJoinSessionAcceptedOptions other)
		{
			this.m_ApiVersion = 1;
		}

		// Token: 0x060007FF RID: 2047 RVA: 0x0000BA1C File Offset: 0x00009C1C
		public void Set(ref AddNotifyJoinSessionAcceptedOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
			}
		}

		// Token: 0x06000800 RID: 2048 RVA: 0x0000BA3D File Offset: 0x00009C3D
		public void Dispose()
		{
		}

		// Token: 0x040003D6 RID: 982
		private int m_ApiVersion;
	}
}
