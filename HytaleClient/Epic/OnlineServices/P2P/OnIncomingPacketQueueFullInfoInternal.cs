using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.P2P
{
	// Token: 0x02000790 RID: 1936
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct OnIncomingPacketQueueFullInfoInternal : ICallbackInfoInternal, IGettable<OnIncomingPacketQueueFullInfo>, ISettable<OnIncomingPacketQueueFullInfo>, IDisposable
	{
		// Token: 0x17000F24 RID: 3876
		// (get) Token: 0x06003211 RID: 12817 RVA: 0x0004AD14 File Offset: 0x00048F14
		// (set) Token: 0x06003212 RID: 12818 RVA: 0x0004AD35 File Offset: 0x00048F35
		public object ClientData
		{
			get
			{
				object result;
				Helper.Get(this.m_ClientData, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_ClientData);
			}
		}

		// Token: 0x17000F25 RID: 3877
		// (get) Token: 0x06003213 RID: 12819 RVA: 0x0004AD48 File Offset: 0x00048F48
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x17000F26 RID: 3878
		// (get) Token: 0x06003214 RID: 12820 RVA: 0x0004AD60 File Offset: 0x00048F60
		// (set) Token: 0x06003215 RID: 12821 RVA: 0x0004AD78 File Offset: 0x00048F78
		public ulong PacketQueueMaxSizeBytes
		{
			get
			{
				return this.m_PacketQueueMaxSizeBytes;
			}
			set
			{
				this.m_PacketQueueMaxSizeBytes = value;
			}
		}

		// Token: 0x17000F27 RID: 3879
		// (get) Token: 0x06003216 RID: 12822 RVA: 0x0004AD84 File Offset: 0x00048F84
		// (set) Token: 0x06003217 RID: 12823 RVA: 0x0004AD9C File Offset: 0x00048F9C
		public ulong PacketQueueCurrentSizeBytes
		{
			get
			{
				return this.m_PacketQueueCurrentSizeBytes;
			}
			set
			{
				this.m_PacketQueueCurrentSizeBytes = value;
			}
		}

		// Token: 0x17000F28 RID: 3880
		// (get) Token: 0x06003218 RID: 12824 RVA: 0x0004ADA8 File Offset: 0x00048FA8
		// (set) Token: 0x06003219 RID: 12825 RVA: 0x0004ADC9 File Offset: 0x00048FC9
		public ProductUserId OverflowPacketLocalUserId
		{
			get
			{
				ProductUserId result;
				Helper.Get<ProductUserId>(this.m_OverflowPacketLocalUserId, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_OverflowPacketLocalUserId);
			}
		}

		// Token: 0x17000F29 RID: 3881
		// (get) Token: 0x0600321A RID: 12826 RVA: 0x0004ADDC File Offset: 0x00048FDC
		// (set) Token: 0x0600321B RID: 12827 RVA: 0x0004ADF4 File Offset: 0x00048FF4
		public byte OverflowPacketChannel
		{
			get
			{
				return this.m_OverflowPacketChannel;
			}
			set
			{
				this.m_OverflowPacketChannel = value;
			}
		}

		// Token: 0x17000F2A RID: 3882
		// (get) Token: 0x0600321C RID: 12828 RVA: 0x0004AE00 File Offset: 0x00049000
		// (set) Token: 0x0600321D RID: 12829 RVA: 0x0004AE18 File Offset: 0x00049018
		public uint OverflowPacketSizeBytes
		{
			get
			{
				return this.m_OverflowPacketSizeBytes;
			}
			set
			{
				this.m_OverflowPacketSizeBytes = value;
			}
		}

		// Token: 0x0600321E RID: 12830 RVA: 0x0004AE24 File Offset: 0x00049024
		public void Set(ref OnIncomingPacketQueueFullInfo other)
		{
			this.ClientData = other.ClientData;
			this.PacketQueueMaxSizeBytes = other.PacketQueueMaxSizeBytes;
			this.PacketQueueCurrentSizeBytes = other.PacketQueueCurrentSizeBytes;
			this.OverflowPacketLocalUserId = other.OverflowPacketLocalUserId;
			this.OverflowPacketChannel = other.OverflowPacketChannel;
			this.OverflowPacketSizeBytes = other.OverflowPacketSizeBytes;
		}

		// Token: 0x0600321F RID: 12831 RVA: 0x0004AE80 File Offset: 0x00049080
		public void Set(ref OnIncomingPacketQueueFullInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ClientData = other.Value.ClientData;
				this.PacketQueueMaxSizeBytes = other.Value.PacketQueueMaxSizeBytes;
				this.PacketQueueCurrentSizeBytes = other.Value.PacketQueueCurrentSizeBytes;
				this.OverflowPacketLocalUserId = other.Value.OverflowPacketLocalUserId;
				this.OverflowPacketChannel = other.Value.OverflowPacketChannel;
				this.OverflowPacketSizeBytes = other.Value.OverflowPacketSizeBytes;
			}
		}

		// Token: 0x06003220 RID: 12832 RVA: 0x0004AF1B File Offset: 0x0004911B
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_OverflowPacketLocalUserId);
		}

		// Token: 0x06003221 RID: 12833 RVA: 0x0004AF36 File Offset: 0x00049136
		public void Get(out OnIncomingPacketQueueFullInfo output)
		{
			output = default(OnIncomingPacketQueueFullInfo);
			output.Set(ref this);
		}

		// Token: 0x04001686 RID: 5766
		private IntPtr m_ClientData;

		// Token: 0x04001687 RID: 5767
		private ulong m_PacketQueueMaxSizeBytes;

		// Token: 0x04001688 RID: 5768
		private ulong m_PacketQueueCurrentSizeBytes;

		// Token: 0x04001689 RID: 5769
		private IntPtr m_OverflowPacketLocalUserId;

		// Token: 0x0400168A RID: 5770
		private byte m_OverflowPacketChannel;

		// Token: 0x0400168B RID: 5771
		private uint m_OverflowPacketSizeBytes;
	}
}
