using System;

namespace Epic.OnlineServices.P2P
{
	// Token: 0x02000764 RID: 1892
	public struct GetNextReceivedPacketSizeOptions
	{
		// Token: 0x17000EE2 RID: 3810
		// (get) Token: 0x06003139 RID: 12601 RVA: 0x00049210 File Offset: 0x00047410
		// (set) Token: 0x0600313A RID: 12602 RVA: 0x00049218 File Offset: 0x00047418
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x17000EE3 RID: 3811
		// (get) Token: 0x0600313B RID: 12603 RVA: 0x00049224 File Offset: 0x00047424
		// (set) Token: 0x0600313C RID: 12604 RVA: 0x00049260 File Offset: 0x00047460
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

		// Token: 0x040015F4 RID: 5620
		internal byte[] m_RequestedChannel;
	}
}
