using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.P2P
{
	// Token: 0x0200077A RID: 1914
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct CloseConnectionOptionsInternal : ISettable<CloseConnectionOptions>, IDisposable
	{
		// Token: 0x17000F0E RID: 3854
		// (set) Token: 0x060031C1 RID: 12737 RVA: 0x0004A752 File Offset: 0x00048952
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x17000F0F RID: 3855
		// (set) Token: 0x060031C2 RID: 12738 RVA: 0x0004A762 File Offset: 0x00048962
		public ProductUserId RemoteUserId
		{
			set
			{
				Helper.Set(value, ref this.m_RemoteUserId);
			}
		}

		// Token: 0x17000F10 RID: 3856
		// (set) Token: 0x060031C3 RID: 12739 RVA: 0x0004A772 File Offset: 0x00048972
		public SocketId? SocketId
		{
			set
			{
				Helper.Set<SocketId, SocketIdInternal>(ref value, ref this.m_SocketId);
			}
		}

		// Token: 0x060031C4 RID: 12740 RVA: 0x0004A783 File Offset: 0x00048983
		public void Set(ref CloseConnectionOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
			this.RemoteUserId = other.RemoteUserId;
			this.SocketId = other.SocketId;
		}

		// Token: 0x060031C5 RID: 12741 RVA: 0x0004A7B4 File Offset: 0x000489B4
		public void Set(ref CloseConnectionOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
				this.RemoteUserId = other.Value.RemoteUserId;
				this.SocketId = other.Value.SocketId;
			}
		}

		// Token: 0x060031C6 RID: 12742 RVA: 0x0004A814 File Offset: 0x00048A14
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_RemoteUserId);
			Helper.Dispose(ref this.m_SocketId);
		}

		// Token: 0x04001652 RID: 5714
		private int m_ApiVersion;

		// Token: 0x04001653 RID: 5715
		private IntPtr m_LocalUserId;

		// Token: 0x04001654 RID: 5716
		private IntPtr m_RemoteUserId;

		// Token: 0x04001655 RID: 5717
		private IntPtr m_SocketId;
	}
}
