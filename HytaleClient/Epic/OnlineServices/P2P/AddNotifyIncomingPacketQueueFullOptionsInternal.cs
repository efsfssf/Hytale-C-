using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.P2P
{
	// Token: 0x0200076E RID: 1902
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct AddNotifyIncomingPacketQueueFullOptionsInternal : ISettable<AddNotifyIncomingPacketQueueFullOptions>, IDisposable
	{
		// Token: 0x06003188 RID: 12680 RVA: 0x0004A293 File Offset: 0x00048493
		public void Set(ref AddNotifyIncomingPacketQueueFullOptions other)
		{
			this.m_ApiVersion = 1;
		}

		// Token: 0x06003189 RID: 12681 RVA: 0x0004A2A0 File Offset: 0x000484A0
		public void Set(ref AddNotifyIncomingPacketQueueFullOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
			}
		}

		// Token: 0x0600318A RID: 12682 RVA: 0x0004A2C1 File Offset: 0x000484C1
		public void Dispose()
		{
		}

		// Token: 0x04001633 RID: 5683
		private int m_ApiVersion;
	}
}
