using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.P2P
{
	// Token: 0x02000782 RID: 1922
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct GetPacketQueueInfoOptionsInternal : ISettable<GetPacketQueueInfoOptions>, IDisposable
	{
		// Token: 0x060031D3 RID: 12755 RVA: 0x0004A938 File Offset: 0x00048B38
		public void Set(ref GetPacketQueueInfoOptions other)
		{
			this.m_ApiVersion = 1;
		}

		// Token: 0x060031D4 RID: 12756 RVA: 0x0004A944 File Offset: 0x00048B44
		public void Set(ref GetPacketQueueInfoOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
			}
		}

		// Token: 0x060031D5 RID: 12757 RVA: 0x0004A965 File Offset: 0x00048B65
		public void Dispose()
		{
		}

		// Token: 0x0400166C RID: 5740
		private int m_ApiVersion;
	}
}
