using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.P2P
{
	// Token: 0x02000768 RID: 1896
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct ReceivePacketOptionsInternal : IDisposable
	{
		// Token: 0x06003167 RID: 12647 RVA: 0x00049C58 File Offset: 0x00047E58
		public ReceivePacketOptionsInternal(ref ReceivePacketOptions other)
		{
			this.m_ApiVersion = 2;
			this.m_RequestedChannel = IntPtr.Zero;
			bool flag = other.RequestedChannel != null;
			if (flag)
			{
				this.m_RequestedChannel = Helper.AddPinnedBuffer(other.m_RequestedChannel);
			}
			this.m_LocalUserId = other.LocalUserId.InnerHandle;
			this.m_MaxDataSizeBytes = other.MaxDataSizeBytes;
		}

		// Token: 0x06003168 RID: 12648 RVA: 0x00049CBA File Offset: 0x00047EBA
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_RequestedChannel);
		}

		// Token: 0x04001615 RID: 5653
		private int m_ApiVersion;

		// Token: 0x04001616 RID: 5654
		private IntPtr m_LocalUserId;

		// Token: 0x04001617 RID: 5655
		private uint m_MaxDataSizeBytes;

		// Token: 0x04001618 RID: 5656
		public IntPtr m_RequestedChannel;
	}
}
