using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.P2P
{
	// Token: 0x02000770 RID: 1904
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct AddNotifyPeerConnectionClosedOptionsInternal : ISettable<AddNotifyPeerConnectionClosedOptions>, IDisposable
	{
		// Token: 0x17000EF7 RID: 3831
		// (set) Token: 0x0600318F RID: 12687 RVA: 0x0004A2E6 File Offset: 0x000484E6
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x17000EF8 RID: 3832
		// (set) Token: 0x06003190 RID: 12688 RVA: 0x0004A2F6 File Offset: 0x000484F6
		public SocketId? SocketId
		{
			set
			{
				Helper.Set<SocketId, SocketIdInternal>(ref value, ref this.m_SocketId);
			}
		}

		// Token: 0x06003191 RID: 12689 RVA: 0x0004A307 File Offset: 0x00048507
		public void Set(ref AddNotifyPeerConnectionClosedOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
			this.SocketId = other.SocketId;
		}

		// Token: 0x06003192 RID: 12690 RVA: 0x0004A32C File Offset: 0x0004852C
		public void Set(ref AddNotifyPeerConnectionClosedOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
				this.SocketId = other.Value.SocketId;
			}
		}

		// Token: 0x06003193 RID: 12691 RVA: 0x0004A377 File Offset: 0x00048577
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_SocketId);
		}

		// Token: 0x04001636 RID: 5686
		private int m_ApiVersion;

		// Token: 0x04001637 RID: 5687
		private IntPtr m_LocalUserId;

		// Token: 0x04001638 RID: 5688
		private IntPtr m_SocketId;
	}
}
