using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.P2P
{
	// Token: 0x02000798 RID: 1944
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct OnPeerConnectionInterruptedInfoInternal : ICallbackInfoInternal, IGettable<OnPeerConnectionInterruptedInfo>, ISettable<OnPeerConnectionInterruptedInfo>, IDisposable
	{
		// Token: 0x17000F3C RID: 3900
		// (get) Token: 0x0600325B RID: 12891 RVA: 0x0004B32C File Offset: 0x0004952C
		// (set) Token: 0x0600325C RID: 12892 RVA: 0x0004B34D File Offset: 0x0004954D
		public object ClientData
		{
			get
			{
				object result;
				Helper.Get(this.m_ClientData, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_ClientData);
			}
		}

		// Token: 0x17000F3D RID: 3901
		// (get) Token: 0x0600325D RID: 12893 RVA: 0x0004B360 File Offset: 0x00049560
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x17000F3E RID: 3902
		// (get) Token: 0x0600325E RID: 12894 RVA: 0x0004B378 File Offset: 0x00049578
		// (set) Token: 0x0600325F RID: 12895 RVA: 0x0004B399 File Offset: 0x00049599
		public ProductUserId LocalUserId
		{
			get
			{
				ProductUserId result;
				Helper.Get<ProductUserId>(this.m_LocalUserId, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x17000F3F RID: 3903
		// (get) Token: 0x06003260 RID: 12896 RVA: 0x0004B3AC File Offset: 0x000495AC
		// (set) Token: 0x06003261 RID: 12897 RVA: 0x0004B3CD File Offset: 0x000495CD
		public ProductUserId RemoteUserId
		{
			get
			{
				ProductUserId result;
				Helper.Get<ProductUserId>(this.m_RemoteUserId, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_RemoteUserId);
			}
		}

		// Token: 0x17000F40 RID: 3904
		// (get) Token: 0x06003262 RID: 12898 RVA: 0x0004B3E0 File Offset: 0x000495E0
		// (set) Token: 0x06003263 RID: 12899 RVA: 0x0004B401 File Offset: 0x00049601
		public SocketId? SocketId
		{
			get
			{
				SocketId? result;
				Helper.Get<SocketIdInternal, SocketId>(this.m_SocketId, out result);
				return result;
			}
			set
			{
				Helper.Set<SocketId, SocketIdInternal>(ref value, ref this.m_SocketId);
			}
		}

		// Token: 0x06003264 RID: 12900 RVA: 0x0004B412 File Offset: 0x00049612
		public void Set(ref OnPeerConnectionInterruptedInfo other)
		{
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.RemoteUserId = other.RemoteUserId;
			this.SocketId = other.SocketId;
		}

		// Token: 0x06003265 RID: 12901 RVA: 0x0004B44C File Offset: 0x0004964C
		public void Set(ref OnPeerConnectionInterruptedInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ClientData = other.Value.ClientData;
				this.LocalUserId = other.Value.LocalUserId;
				this.RemoteUserId = other.Value.RemoteUserId;
				this.SocketId = other.Value.SocketId;
			}
		}

		// Token: 0x06003266 RID: 12902 RVA: 0x0004B4BA File Offset: 0x000496BA
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_RemoteUserId);
			Helper.Dispose(ref this.m_SocketId);
		}

		// Token: 0x06003267 RID: 12903 RVA: 0x0004B4ED File Offset: 0x000496ED
		public void Get(out OnPeerConnectionInterruptedInfo output)
		{
			output = default(OnPeerConnectionInterruptedInfo);
			output.Set(ref this);
		}

		// Token: 0x0400169C RID: 5788
		private IntPtr m_ClientData;

		// Token: 0x0400169D RID: 5789
		private IntPtr m_LocalUserId;

		// Token: 0x0400169E RID: 5790
		private IntPtr m_RemoteUserId;

		// Token: 0x0400169F RID: 5791
		private IntPtr m_SocketId;
	}
}
