using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.P2P
{
	// Token: 0x020007A0 RID: 1952
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct OnRemoteConnectionClosedInfoInternal : ICallbackInfoInternal, IGettable<OnRemoteConnectionClosedInfo>, ISettable<OnRemoteConnectionClosedInfo>, IDisposable
	{
		// Token: 0x17000F4D RID: 3917
		// (get) Token: 0x06003297 RID: 12951 RVA: 0x0004B774 File Offset: 0x00049974
		// (set) Token: 0x06003298 RID: 12952 RVA: 0x0004B795 File Offset: 0x00049995
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

		// Token: 0x17000F4E RID: 3918
		// (get) Token: 0x06003299 RID: 12953 RVA: 0x0004B7A8 File Offset: 0x000499A8
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x17000F4F RID: 3919
		// (get) Token: 0x0600329A RID: 12954 RVA: 0x0004B7C0 File Offset: 0x000499C0
		// (set) Token: 0x0600329B RID: 12955 RVA: 0x0004B7E1 File Offset: 0x000499E1
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

		// Token: 0x17000F50 RID: 3920
		// (get) Token: 0x0600329C RID: 12956 RVA: 0x0004B7F4 File Offset: 0x000499F4
		// (set) Token: 0x0600329D RID: 12957 RVA: 0x0004B815 File Offset: 0x00049A15
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

		// Token: 0x17000F51 RID: 3921
		// (get) Token: 0x0600329E RID: 12958 RVA: 0x0004B828 File Offset: 0x00049A28
		// (set) Token: 0x0600329F RID: 12959 RVA: 0x0004B849 File Offset: 0x00049A49
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

		// Token: 0x17000F52 RID: 3922
		// (get) Token: 0x060032A0 RID: 12960 RVA: 0x0004B85C File Offset: 0x00049A5C
		// (set) Token: 0x060032A1 RID: 12961 RVA: 0x0004B874 File Offset: 0x00049A74
		public ConnectionClosedReason Reason
		{
			get
			{
				return this.m_Reason;
			}
			set
			{
				this.m_Reason = value;
			}
		}

		// Token: 0x060032A2 RID: 12962 RVA: 0x0004B880 File Offset: 0x00049A80
		public void Set(ref OnRemoteConnectionClosedInfo other)
		{
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.RemoteUserId = other.RemoteUserId;
			this.SocketId = other.SocketId;
			this.Reason = other.Reason;
		}

		// Token: 0x060032A3 RID: 12963 RVA: 0x0004B8D0 File Offset: 0x00049AD0
		public void Set(ref OnRemoteConnectionClosedInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ClientData = other.Value.ClientData;
				this.LocalUserId = other.Value.LocalUserId;
				this.RemoteUserId = other.Value.RemoteUserId;
				this.SocketId = other.Value.SocketId;
				this.Reason = other.Value.Reason;
			}
		}

		// Token: 0x060032A4 RID: 12964 RVA: 0x0004B953 File Offset: 0x00049B53
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_RemoteUserId);
			Helper.Dispose(ref this.m_SocketId);
		}

		// Token: 0x060032A5 RID: 12965 RVA: 0x0004B986 File Offset: 0x00049B86
		public void Get(out OnRemoteConnectionClosedInfo output)
		{
			output = default(OnRemoteConnectionClosedInfo);
			output.Set(ref this);
		}

		// Token: 0x040016AB RID: 5803
		private IntPtr m_ClientData;

		// Token: 0x040016AC RID: 5804
		private IntPtr m_LocalUserId;

		// Token: 0x040016AD RID: 5805
		private IntPtr m_RemoteUserId;

		// Token: 0x040016AE RID: 5806
		private IntPtr m_SocketId;

		// Token: 0x040016AF RID: 5807
		private ConnectionClosedReason m_Reason;
	}
}
