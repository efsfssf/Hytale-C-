using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.P2P
{
	// Token: 0x02000776 RID: 1910
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct AddNotifyPeerConnectionRequestOptionsInternal : ISettable<AddNotifyPeerConnectionRequestOptions>, IDisposable
	{
		// Token: 0x17000F03 RID: 3843
		// (set) Token: 0x060031AA RID: 12714 RVA: 0x0004A554 File Offset: 0x00048754
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x17000F04 RID: 3844
		// (set) Token: 0x060031AB RID: 12715 RVA: 0x0004A564 File Offset: 0x00048764
		public SocketId? SocketId
		{
			set
			{
				Helper.Set<SocketId, SocketIdInternal>(ref value, ref this.m_SocketId);
			}
		}

		// Token: 0x060031AC RID: 12716 RVA: 0x0004A575 File Offset: 0x00048775
		public void Set(ref AddNotifyPeerConnectionRequestOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
			this.SocketId = other.SocketId;
		}

		// Token: 0x060031AD RID: 12717 RVA: 0x0004A59C File Offset: 0x0004879C
		public void Set(ref AddNotifyPeerConnectionRequestOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
				this.SocketId = other.Value.SocketId;
			}
		}

		// Token: 0x060031AE RID: 12718 RVA: 0x0004A5E7 File Offset: 0x000487E7
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_SocketId);
		}

		// Token: 0x04001645 RID: 5701
		private int m_ApiVersion;

		// Token: 0x04001646 RID: 5702
		private IntPtr m_LocalUserId;

		// Token: 0x04001647 RID: 5703
		private IntPtr m_SocketId;
	}
}
