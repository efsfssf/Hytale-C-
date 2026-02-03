using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.P2P
{
	// Token: 0x020007A9 RID: 1961
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct SetPacketQueueSizeOptionsInternal : ISettable<SetPacketQueueSizeOptions>, IDisposable
	{
		// Token: 0x17000F69 RID: 3945
		// (set) Token: 0x060032DA RID: 13018 RVA: 0x0004BD1A File Offset: 0x00049F1A
		public ulong IncomingPacketQueueMaxSizeBytes
		{
			set
			{
				this.m_IncomingPacketQueueMaxSizeBytes = value;
			}
		}

		// Token: 0x17000F6A RID: 3946
		// (set) Token: 0x060032DB RID: 13019 RVA: 0x0004BD24 File Offset: 0x00049F24
		public ulong OutgoingPacketQueueMaxSizeBytes
		{
			set
			{
				this.m_OutgoingPacketQueueMaxSizeBytes = value;
			}
		}

		// Token: 0x060032DC RID: 13020 RVA: 0x0004BD2E File Offset: 0x00049F2E
		public void Set(ref SetPacketQueueSizeOptions other)
		{
			this.m_ApiVersion = 1;
			this.IncomingPacketQueueMaxSizeBytes = other.IncomingPacketQueueMaxSizeBytes;
			this.OutgoingPacketQueueMaxSizeBytes = other.OutgoingPacketQueueMaxSizeBytes;
		}

		// Token: 0x060032DD RID: 13021 RVA: 0x0004BD54 File Offset: 0x00049F54
		public void Set(ref SetPacketQueueSizeOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.IncomingPacketQueueMaxSizeBytes = other.Value.IncomingPacketQueueMaxSizeBytes;
				this.OutgoingPacketQueueMaxSizeBytes = other.Value.OutgoingPacketQueueMaxSizeBytes;
			}
		}

		// Token: 0x060032DE RID: 13022 RVA: 0x0004BD9F File Offset: 0x00049F9F
		public void Dispose()
		{
		}

		// Token: 0x040016CF RID: 5839
		private int m_ApiVersion;

		// Token: 0x040016D0 RID: 5840
		private ulong m_IncomingPacketQueueMaxSizeBytes;

		// Token: 0x040016D1 RID: 5841
		private ulong m_OutgoingPacketQueueMaxSizeBytes;
	}
}
