using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.P2P
{
	// Token: 0x02000765 RID: 1893
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct GetNextReceivedPacketSizeOptionsInternal : ISettable<GetNextReceivedPacketSizeOptions>, IDisposable
	{
		// Token: 0x0600313D RID: 12605 RVA: 0x000492B0 File Offset: 0x000474B0
		public void Set(ref GetNextReceivedPacketSizeOptions other)
		{
			this.m_ApiVersion = 2;
			this.m_LocalUserId = other.LocalUserId.InnerHandle;
			this.m_RequestedChannel = IntPtr.Zero;
			bool flag = other.RequestedChannel != null;
			if (flag)
			{
				this.m_RequestedChannel = Helper.AddPinnedBuffer(other.m_RequestedChannel);
			}
		}

		// Token: 0x0600313E RID: 12606 RVA: 0x00049308 File Offset: 0x00047508
		public void Set(ref GetNextReceivedPacketSizeOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 2;
				this.m_LocalUserId = other.Value.LocalUserId.InnerHandle;
				this.m_RequestedChannel = IntPtr.Zero;
				bool flag2 = other.Value.RequestedChannel != null;
				if (flag2)
				{
					this.m_RequestedChannel = Helper.AddPinnedBuffer(other.Value.m_RequestedChannel);
				}
			}
		}

		// Token: 0x0600313F RID: 12607 RVA: 0x0004937F File Offset: 0x0004757F
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_RequestedChannel);
		}

		// Token: 0x040015F6 RID: 5622
		private int m_ApiVersion;

		// Token: 0x040015F7 RID: 5623
		private IntPtr m_LocalUserId;

		// Token: 0x040015F8 RID: 5624
		private IntPtr m_RequestedChannel;
	}
}
