using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.P2P
{
	// Token: 0x0200076C RID: 1900
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct AcceptConnectionOptionsInternal : ISettable<AcceptConnectionOptions>, IDisposable
	{
		// Token: 0x17000EF2 RID: 3826
		// (set) Token: 0x06003182 RID: 12674 RVA: 0x0004A1A9 File Offset: 0x000483A9
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x17000EF3 RID: 3827
		// (set) Token: 0x06003183 RID: 12675 RVA: 0x0004A1B9 File Offset: 0x000483B9
		public ProductUserId RemoteUserId
		{
			set
			{
				Helper.Set(value, ref this.m_RemoteUserId);
			}
		}

		// Token: 0x17000EF4 RID: 3828
		// (set) Token: 0x06003184 RID: 12676 RVA: 0x0004A1C9 File Offset: 0x000483C9
		public SocketId? SocketId
		{
			set
			{
				Helper.Set<SocketId, SocketIdInternal>(ref value, ref this.m_SocketId);
			}
		}

		// Token: 0x06003185 RID: 12677 RVA: 0x0004A1DA File Offset: 0x000483DA
		public void Set(ref AcceptConnectionOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
			this.RemoteUserId = other.RemoteUserId;
			this.SocketId = other.SocketId;
		}

		// Token: 0x06003186 RID: 12678 RVA: 0x0004A20C File Offset: 0x0004840C
		public void Set(ref AcceptConnectionOptions? other)
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

		// Token: 0x06003187 RID: 12679 RVA: 0x0004A26C File Offset: 0x0004846C
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_RemoteUserId);
			Helper.Dispose(ref this.m_SocketId);
		}

		// Token: 0x0400162F RID: 5679
		private int m_ApiVersion;

		// Token: 0x04001630 RID: 5680
		private IntPtr m_LocalUserId;

		// Token: 0x04001631 RID: 5681
		private IntPtr m_RemoteUserId;

		// Token: 0x04001632 RID: 5682
		private IntPtr m_SocketId;
	}
}
