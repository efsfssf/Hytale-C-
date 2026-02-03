using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.P2P
{
	// Token: 0x02000778 RID: 1912
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct ClearPacketQueueOptionsInternal : ISettable<ClearPacketQueueOptions>, IDisposable
	{
		// Token: 0x17000F08 RID: 3848
		// (set) Token: 0x060031B5 RID: 12725 RVA: 0x0004A635 File Offset: 0x00048835
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x17000F09 RID: 3849
		// (set) Token: 0x060031B6 RID: 12726 RVA: 0x0004A645 File Offset: 0x00048845
		public ProductUserId RemoteUserId
		{
			set
			{
				Helper.Set(value, ref this.m_RemoteUserId);
			}
		}

		// Token: 0x17000F0A RID: 3850
		// (set) Token: 0x060031B7 RID: 12727 RVA: 0x0004A655 File Offset: 0x00048855
		public SocketId? SocketId
		{
			set
			{
				Helper.Set<SocketId, SocketIdInternal>(ref value, ref this.m_SocketId);
			}
		}

		// Token: 0x060031B8 RID: 12728 RVA: 0x0004A666 File Offset: 0x00048866
		public void Set(ref ClearPacketQueueOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
			this.RemoteUserId = other.RemoteUserId;
			this.SocketId = other.SocketId;
		}

		// Token: 0x060031B9 RID: 12729 RVA: 0x0004A698 File Offset: 0x00048898
		public void Set(ref ClearPacketQueueOptions? other)
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

		// Token: 0x060031BA RID: 12730 RVA: 0x0004A6F8 File Offset: 0x000488F8
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_RemoteUserId);
			Helper.Dispose(ref this.m_SocketId);
		}

		// Token: 0x0400164B RID: 5707
		private int m_ApiVersion;

		// Token: 0x0400164C RID: 5708
		private IntPtr m_LocalUserId;

		// Token: 0x0400164D RID: 5709
		private IntPtr m_RemoteUserId;

		// Token: 0x0400164E RID: 5710
		private IntPtr m_SocketId;
	}
}
