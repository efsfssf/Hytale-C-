using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.P2P
{
	// Token: 0x02000772 RID: 1906
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct AddNotifyPeerConnectionEstablishedOptionsInternal : ISettable<AddNotifyPeerConnectionEstablishedOptions>, IDisposable
	{
		// Token: 0x17000EFB RID: 3835
		// (set) Token: 0x06003198 RID: 12696 RVA: 0x0004A3B4 File Offset: 0x000485B4
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x17000EFC RID: 3836
		// (set) Token: 0x06003199 RID: 12697 RVA: 0x0004A3C4 File Offset: 0x000485C4
		public SocketId? SocketId
		{
			set
			{
				Helper.Set<SocketId, SocketIdInternal>(ref value, ref this.m_SocketId);
			}
		}

		// Token: 0x0600319A RID: 12698 RVA: 0x0004A3D5 File Offset: 0x000485D5
		public void Set(ref AddNotifyPeerConnectionEstablishedOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
			this.SocketId = other.SocketId;
		}

		// Token: 0x0600319B RID: 12699 RVA: 0x0004A3FC File Offset: 0x000485FC
		public void Set(ref AddNotifyPeerConnectionEstablishedOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
				this.SocketId = other.Value.SocketId;
			}
		}

		// Token: 0x0600319C RID: 12700 RVA: 0x0004A447 File Offset: 0x00048647
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_SocketId);
		}

		// Token: 0x0400163B RID: 5691
		private int m_ApiVersion;

		// Token: 0x0400163C RID: 5692
		private IntPtr m_LocalUserId;

		// Token: 0x0400163D RID: 5693
		private IntPtr m_SocketId;
	}
}
