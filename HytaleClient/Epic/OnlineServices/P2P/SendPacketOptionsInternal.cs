using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.P2P
{
	// Token: 0x02000769 RID: 1897
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct SendPacketOptionsInternal : ISettable<SendPacketOptions>, IDisposable
	{
		// Token: 0x17000EE7 RID: 3815
		// (set) Token: 0x06003169 RID: 12649 RVA: 0x00049CD5 File Offset: 0x00047ED5
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x17000EE8 RID: 3816
		// (set) Token: 0x0600316A RID: 12650 RVA: 0x00049CE5 File Offset: 0x00047EE5
		public ProductUserId RemoteUserId
		{
			set
			{
				Helper.Set(value, ref this.m_RemoteUserId);
			}
		}

		// Token: 0x17000EE9 RID: 3817
		// (set) Token: 0x0600316B RID: 12651 RVA: 0x00049CF5 File Offset: 0x00047EF5
		public byte Channel
		{
			set
			{
				this.m_Channel = value;
			}
		}

		// Token: 0x17000EEA RID: 3818
		// (set) Token: 0x0600316C RID: 12652 RVA: 0x00049CFF File Offset: 0x00047EFF
		public ArraySegment<byte> Data
		{
			set
			{
				Helper.Set(value, ref this.m_Data, out this.m_DataLengthBytes);
			}
		}

		// Token: 0x17000EEB RID: 3819
		// (set) Token: 0x0600316D RID: 12653 RVA: 0x00049D15 File Offset: 0x00047F15
		public bool AllowDelayedDelivery
		{
			set
			{
				Helper.Set(value, ref this.m_AllowDelayedDelivery);
			}
		}

		// Token: 0x17000EEC RID: 3820
		// (set) Token: 0x0600316E RID: 12654 RVA: 0x00049D25 File Offset: 0x00047F25
		public PacketReliability Reliability
		{
			set
			{
				this.m_Reliability = value;
			}
		}

		// Token: 0x17000EED RID: 3821
		// (set) Token: 0x0600316F RID: 12655 RVA: 0x00049D2F File Offset: 0x00047F2F
		public bool DisableAutoAcceptConnection
		{
			set
			{
				Helper.Set(value, ref this.m_DisableAutoAcceptConnection);
			}
		}

		// Token: 0x06003170 RID: 12656 RVA: 0x00049D40 File Offset: 0x00047F40
		public void Set(ref SendPacketOptions other)
		{
			this.m_ApiVersion = 3;
			this.LocalUserId = other.LocalUserId;
			this.RemoteUserId = other.RemoteUserId;
			this.m_SocketId = IntPtr.Zero;
			bool flag = other.SocketId != null;
			if (flag)
			{
				this.m_SocketId = Helper.AddPinnedBuffer(other.SocketId.Value.m_AllBytes);
			}
			this.Channel = other.Channel;
			this.Data = other.Data;
			this.AllowDelayedDelivery = other.AllowDelayedDelivery;
			this.Reliability = other.Reliability;
			this.DisableAutoAcceptConnection = other.DisableAutoAcceptConnection;
		}

		// Token: 0x06003171 RID: 12657 RVA: 0x00049DF0 File Offset: 0x00047FF0
		public void Set(ref SendPacketOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 3;
				this.LocalUserId = other.Value.LocalUserId;
				this.RemoteUserId = other.Value.RemoteUserId;
				this.m_SocketId = IntPtr.Zero;
				bool flag2 = other.Value.SocketId != null;
				if (flag2)
				{
					this.m_SocketId = Helper.AddPinnedBuffer(other.Value.SocketId.Value.m_AllBytes);
				}
				this.Channel = other.Value.Channel;
				this.Data = other.Value.Data;
				this.AllowDelayedDelivery = other.Value.AllowDelayedDelivery;
				this.Reliability = other.Value.Reliability;
				this.DisableAutoAcceptConnection = other.Value.DisableAutoAcceptConnection;
			}
		}

		// Token: 0x06003172 RID: 12658 RVA: 0x00049EF4 File Offset: 0x000480F4
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_RemoteUserId);
			Helper.Dispose(ref this.m_SocketId);
			Helper.Dispose(ref this.m_Data);
		}

		// Token: 0x04001619 RID: 5657
		private int m_ApiVersion;

		// Token: 0x0400161A RID: 5658
		private IntPtr m_LocalUserId;

		// Token: 0x0400161B RID: 5659
		private IntPtr m_RemoteUserId;

		// Token: 0x0400161C RID: 5660
		internal IntPtr m_SocketId;

		// Token: 0x0400161D RID: 5661
		private byte m_Channel;

		// Token: 0x0400161E RID: 5662
		private uint m_DataLengthBytes;

		// Token: 0x0400161F RID: 5663
		private IntPtr m_Data;

		// Token: 0x04001620 RID: 5664
		private int m_AllowDelayedDelivery;

		// Token: 0x04001621 RID: 5665
		private PacketReliability m_Reliability;

		// Token: 0x04001622 RID: 5666
		private int m_DisableAutoAcceptConnection;
	}
}
