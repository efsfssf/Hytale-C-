using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.P2P
{
	// Token: 0x02000794 RID: 1940
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct OnPeerConnectionEstablishedInfoInternal : ICallbackInfoInternal, IGettable<OnPeerConnectionEstablishedInfo>, ISettable<OnPeerConnectionEstablishedInfo>, IDisposable
	{
		// Token: 0x17000F31 RID: 3889
		// (get) Token: 0x06003238 RID: 12856 RVA: 0x0004B028 File Offset: 0x00049228
		// (set) Token: 0x06003239 RID: 12857 RVA: 0x0004B049 File Offset: 0x00049249
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

		// Token: 0x17000F32 RID: 3890
		// (get) Token: 0x0600323A RID: 12858 RVA: 0x0004B05C File Offset: 0x0004925C
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x17000F33 RID: 3891
		// (get) Token: 0x0600323B RID: 12859 RVA: 0x0004B074 File Offset: 0x00049274
		// (set) Token: 0x0600323C RID: 12860 RVA: 0x0004B095 File Offset: 0x00049295
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

		// Token: 0x17000F34 RID: 3892
		// (get) Token: 0x0600323D RID: 12861 RVA: 0x0004B0A8 File Offset: 0x000492A8
		// (set) Token: 0x0600323E RID: 12862 RVA: 0x0004B0C9 File Offset: 0x000492C9
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

		// Token: 0x17000F35 RID: 3893
		// (get) Token: 0x0600323F RID: 12863 RVA: 0x0004B0DC File Offset: 0x000492DC
		// (set) Token: 0x06003240 RID: 12864 RVA: 0x0004B0FD File Offset: 0x000492FD
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

		// Token: 0x17000F36 RID: 3894
		// (get) Token: 0x06003241 RID: 12865 RVA: 0x0004B110 File Offset: 0x00049310
		// (set) Token: 0x06003242 RID: 12866 RVA: 0x0004B128 File Offset: 0x00049328
		public ConnectionEstablishedType ConnectionType
		{
			get
			{
				return this.m_ConnectionType;
			}
			set
			{
				this.m_ConnectionType = value;
			}
		}

		// Token: 0x17000F37 RID: 3895
		// (get) Token: 0x06003243 RID: 12867 RVA: 0x0004B134 File Offset: 0x00049334
		// (set) Token: 0x06003244 RID: 12868 RVA: 0x0004B14C File Offset: 0x0004934C
		public NetworkConnectionType NetworkType
		{
			get
			{
				return this.m_NetworkType;
			}
			set
			{
				this.m_NetworkType = value;
			}
		}

		// Token: 0x06003245 RID: 12869 RVA: 0x0004B158 File Offset: 0x00049358
		public void Set(ref OnPeerConnectionEstablishedInfo other)
		{
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.RemoteUserId = other.RemoteUserId;
			this.SocketId = other.SocketId;
			this.ConnectionType = other.ConnectionType;
			this.NetworkType = other.NetworkType;
		}

		// Token: 0x06003246 RID: 12870 RVA: 0x0004B1B4 File Offset: 0x000493B4
		public void Set(ref OnPeerConnectionEstablishedInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ClientData = other.Value.ClientData;
				this.LocalUserId = other.Value.LocalUserId;
				this.RemoteUserId = other.Value.RemoteUserId;
				this.SocketId = other.Value.SocketId;
				this.ConnectionType = other.Value.ConnectionType;
				this.NetworkType = other.Value.NetworkType;
			}
		}

		// Token: 0x06003247 RID: 12871 RVA: 0x0004B24F File Offset: 0x0004944F
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_RemoteUserId);
			Helper.Dispose(ref this.m_SocketId);
		}

		// Token: 0x06003248 RID: 12872 RVA: 0x0004B282 File Offset: 0x00049482
		public void Get(out OnPeerConnectionEstablishedInfo output)
		{
			output = default(OnPeerConnectionEstablishedInfo);
			output.Set(ref this);
		}

		// Token: 0x04001692 RID: 5778
		private IntPtr m_ClientData;

		// Token: 0x04001693 RID: 5779
		private IntPtr m_LocalUserId;

		// Token: 0x04001694 RID: 5780
		private IntPtr m_RemoteUserId;

		// Token: 0x04001695 RID: 5781
		private IntPtr m_SocketId;

		// Token: 0x04001696 RID: 5782
		private ConnectionEstablishedType m_ConnectionType;

		// Token: 0x04001697 RID: 5783
		private NetworkConnectionType m_NetworkType;
	}
}
