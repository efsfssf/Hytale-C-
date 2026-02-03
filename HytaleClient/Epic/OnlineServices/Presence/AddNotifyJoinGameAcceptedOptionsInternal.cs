using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Presence
{
	// Token: 0x020002B9 RID: 697
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct AddNotifyJoinGameAcceptedOptionsInternal : ISettable<AddNotifyJoinGameAcceptedOptions>, IDisposable
	{
		// Token: 0x06001351 RID: 4945 RVA: 0x0001C1CD File Offset: 0x0001A3CD
		public void Set(ref AddNotifyJoinGameAcceptedOptions other)
		{
			this.m_ApiVersion = 2;
		}

		// Token: 0x06001352 RID: 4946 RVA: 0x0001C1D8 File Offset: 0x0001A3D8
		public void Set(ref AddNotifyJoinGameAcceptedOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 2;
			}
		}

		// Token: 0x06001353 RID: 4947 RVA: 0x0001C1F9 File Offset: 0x0001A3F9
		public void Dispose()
		{
		}

		// Token: 0x04000877 RID: 2167
		private int m_ApiVersion;
	}
}
