using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.P2P
{
	// Token: 0x020007A2 RID: 1954
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct PacketQueueInfoInternal : IGettable<PacketQueueInfo>, ISettable<PacketQueueInfo>, IDisposable
	{
		// Token: 0x17000F59 RID: 3929
		// (get) Token: 0x060032B3 RID: 12979 RVA: 0x0004BA5C File Offset: 0x00049C5C
		// (set) Token: 0x060032B4 RID: 12980 RVA: 0x0004BA74 File Offset: 0x00049C74
		public ulong IncomingPacketQueueMaxSizeBytes
		{
			get
			{
				return this.m_IncomingPacketQueueMaxSizeBytes;
			}
			set
			{
				this.m_IncomingPacketQueueMaxSizeBytes = value;
			}
		}

		// Token: 0x17000F5A RID: 3930
		// (get) Token: 0x060032B5 RID: 12981 RVA: 0x0004BA80 File Offset: 0x00049C80
		// (set) Token: 0x060032B6 RID: 12982 RVA: 0x0004BA98 File Offset: 0x00049C98
		public ulong IncomingPacketQueueCurrentSizeBytes
		{
			get
			{
				return this.m_IncomingPacketQueueCurrentSizeBytes;
			}
			set
			{
				this.m_IncomingPacketQueueCurrentSizeBytes = value;
			}
		}

		// Token: 0x17000F5B RID: 3931
		// (get) Token: 0x060032B7 RID: 12983 RVA: 0x0004BAA4 File Offset: 0x00049CA4
		// (set) Token: 0x060032B8 RID: 12984 RVA: 0x0004BABC File Offset: 0x00049CBC
		public ulong IncomingPacketQueueCurrentPacketCount
		{
			get
			{
				return this.m_IncomingPacketQueueCurrentPacketCount;
			}
			set
			{
				this.m_IncomingPacketQueueCurrentPacketCount = value;
			}
		}

		// Token: 0x17000F5C RID: 3932
		// (get) Token: 0x060032B9 RID: 12985 RVA: 0x0004BAC8 File Offset: 0x00049CC8
		// (set) Token: 0x060032BA RID: 12986 RVA: 0x0004BAE0 File Offset: 0x00049CE0
		public ulong OutgoingPacketQueueMaxSizeBytes
		{
			get
			{
				return this.m_OutgoingPacketQueueMaxSizeBytes;
			}
			set
			{
				this.m_OutgoingPacketQueueMaxSizeBytes = value;
			}
		}

		// Token: 0x17000F5D RID: 3933
		// (get) Token: 0x060032BB RID: 12987 RVA: 0x0004BAEC File Offset: 0x00049CEC
		// (set) Token: 0x060032BC RID: 12988 RVA: 0x0004BB04 File Offset: 0x00049D04
		public ulong OutgoingPacketQueueCurrentSizeBytes
		{
			get
			{
				return this.m_OutgoingPacketQueueCurrentSizeBytes;
			}
			set
			{
				this.m_OutgoingPacketQueueCurrentSizeBytes = value;
			}
		}

		// Token: 0x17000F5E RID: 3934
		// (get) Token: 0x060032BD RID: 12989 RVA: 0x0004BB10 File Offset: 0x00049D10
		// (set) Token: 0x060032BE RID: 12990 RVA: 0x0004BB28 File Offset: 0x00049D28
		public ulong OutgoingPacketQueueCurrentPacketCount
		{
			get
			{
				return this.m_OutgoingPacketQueueCurrentPacketCount;
			}
			set
			{
				this.m_OutgoingPacketQueueCurrentPacketCount = value;
			}
		}

		// Token: 0x060032BF RID: 12991 RVA: 0x0004BB34 File Offset: 0x00049D34
		public void Set(ref PacketQueueInfo other)
		{
			this.IncomingPacketQueueMaxSizeBytes = other.IncomingPacketQueueMaxSizeBytes;
			this.IncomingPacketQueueCurrentSizeBytes = other.IncomingPacketQueueCurrentSizeBytes;
			this.IncomingPacketQueueCurrentPacketCount = other.IncomingPacketQueueCurrentPacketCount;
			this.OutgoingPacketQueueMaxSizeBytes = other.OutgoingPacketQueueMaxSizeBytes;
			this.OutgoingPacketQueueCurrentSizeBytes = other.OutgoingPacketQueueCurrentSizeBytes;
			this.OutgoingPacketQueueCurrentPacketCount = other.OutgoingPacketQueueCurrentPacketCount;
		}

		// Token: 0x060032C0 RID: 12992 RVA: 0x0004BB90 File Offset: 0x00049D90
		public void Set(ref PacketQueueInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.IncomingPacketQueueMaxSizeBytes = other.Value.IncomingPacketQueueMaxSizeBytes;
				this.IncomingPacketQueueCurrentSizeBytes = other.Value.IncomingPacketQueueCurrentSizeBytes;
				this.IncomingPacketQueueCurrentPacketCount = other.Value.IncomingPacketQueueCurrentPacketCount;
				this.OutgoingPacketQueueMaxSizeBytes = other.Value.OutgoingPacketQueueMaxSizeBytes;
				this.OutgoingPacketQueueCurrentSizeBytes = other.Value.OutgoingPacketQueueCurrentSizeBytes;
				this.OutgoingPacketQueueCurrentPacketCount = other.Value.OutgoingPacketQueueCurrentPacketCount;
			}
		}

		// Token: 0x060032C1 RID: 12993 RVA: 0x0004BC2B File Offset: 0x00049E2B
		public void Dispose()
		{
		}

		// Token: 0x060032C2 RID: 12994 RVA: 0x0004BC2E File Offset: 0x00049E2E
		public void Get(out PacketQueueInfo output)
		{
			output = default(PacketQueueInfo);
			output.Set(ref this);
		}

		// Token: 0x040016B6 RID: 5814
		private ulong m_IncomingPacketQueueMaxSizeBytes;

		// Token: 0x040016B7 RID: 5815
		private ulong m_IncomingPacketQueueCurrentSizeBytes;

		// Token: 0x040016B8 RID: 5816
		private ulong m_IncomingPacketQueueCurrentPacketCount;

		// Token: 0x040016B9 RID: 5817
		private ulong m_OutgoingPacketQueueMaxSizeBytes;

		// Token: 0x040016BA RID: 5818
		private ulong m_OutgoingPacketQueueCurrentSizeBytes;

		// Token: 0x040016BB RID: 5819
		private ulong m_OutgoingPacketQueueCurrentPacketCount;
	}
}
