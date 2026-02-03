using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.P2P
{
	// Token: 0x02000767 RID: 1895
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct ReceivePacketOptions
	{
		// Token: 0x17000EE4 RID: 3812
		// (get) Token: 0x06003161 RID: 12641 RVA: 0x00049BA7 File Offset: 0x00047DA7
		// (set) Token: 0x06003162 RID: 12642 RVA: 0x00049BAF File Offset: 0x00047DAF
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x17000EE5 RID: 3813
		// (get) Token: 0x06003163 RID: 12643 RVA: 0x00049BB8 File Offset: 0x00047DB8
		// (set) Token: 0x06003164 RID: 12644 RVA: 0x00049BC0 File Offset: 0x00047DC0
		public uint MaxDataSizeBytes { get; set; }

		// Token: 0x17000EE6 RID: 3814
		// (get) Token: 0x06003165 RID: 12645 RVA: 0x00049BCC File Offset: 0x00047DCC
		// (set) Token: 0x06003166 RID: 12646 RVA: 0x00049C08 File Offset: 0x00047E08
		public byte? RequestedChannel
		{
			get
			{
				bool flag = this.m_RequestedChannel == null;
				byte? result;
				if (flag)
				{
					result = null;
				}
				else
				{
					result = new byte?(this.m_RequestedChannel[0]);
				}
				return result;
			}
			set
			{
				bool flag = value != null;
				if (flag)
				{
					bool flag2 = this.m_RequestedChannel == null;
					if (flag2)
					{
						this.m_RequestedChannel = new byte[1];
					}
					this.m_RequestedChannel[0] = value.Value;
				}
				else
				{
					this.m_RequestedChannel = null;
				}
			}
		}

		// Token: 0x04001612 RID: 5650
		internal byte[] m_RequestedChannel;
	}
}
