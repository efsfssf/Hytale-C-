using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.P2P
{
	// Token: 0x0200077C RID: 1916
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct CloseConnectionsOptionsInternal : ISettable<CloseConnectionsOptions>, IDisposable
	{
		// Token: 0x17000F13 RID: 3859
		// (set) Token: 0x060031CB RID: 12747 RVA: 0x0004A85D File Offset: 0x00048A5D
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x17000F14 RID: 3860
		// (set) Token: 0x060031CC RID: 12748 RVA: 0x0004A86D File Offset: 0x00048A6D
		public SocketId? SocketId
		{
			set
			{
				Helper.Set<SocketId, SocketIdInternal>(ref value, ref this.m_SocketId);
			}
		}

		// Token: 0x060031CD RID: 12749 RVA: 0x0004A87E File Offset: 0x00048A7E
		public void Set(ref CloseConnectionsOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
			this.SocketId = other.SocketId;
		}

		// Token: 0x060031CE RID: 12750 RVA: 0x0004A8A4 File Offset: 0x00048AA4
		public void Set(ref CloseConnectionsOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
				this.SocketId = other.Value.SocketId;
			}
		}

		// Token: 0x060031CF RID: 12751 RVA: 0x0004A8EF File Offset: 0x00048AEF
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_SocketId);
		}

		// Token: 0x04001658 RID: 5720
		private int m_ApiVersion;

		// Token: 0x04001659 RID: 5721
		private IntPtr m_LocalUserId;

		// Token: 0x0400165A RID: 5722
		private IntPtr m_SocketId;
	}
}
