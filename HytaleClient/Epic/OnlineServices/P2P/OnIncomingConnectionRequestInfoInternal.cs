using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.P2P
{
	// Token: 0x0200078C RID: 1932
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct OnIncomingConnectionRequestInfoInternal : ICallbackInfoInternal, IGettable<OnIncomingConnectionRequestInfo>, ISettable<OnIncomingConnectionRequestInfo>, IDisposable
	{
		// Token: 0x17000F19 RID: 3865
		// (get) Token: 0x060031EE RID: 12782 RVA: 0x0004AA60 File Offset: 0x00048C60
		// (set) Token: 0x060031EF RID: 12783 RVA: 0x0004AA81 File Offset: 0x00048C81
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

		// Token: 0x17000F1A RID: 3866
		// (get) Token: 0x060031F0 RID: 12784 RVA: 0x0004AA94 File Offset: 0x00048C94
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x17000F1B RID: 3867
		// (get) Token: 0x060031F1 RID: 12785 RVA: 0x0004AAAC File Offset: 0x00048CAC
		// (set) Token: 0x060031F2 RID: 12786 RVA: 0x0004AACD File Offset: 0x00048CCD
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

		// Token: 0x17000F1C RID: 3868
		// (get) Token: 0x060031F3 RID: 12787 RVA: 0x0004AAE0 File Offset: 0x00048CE0
		// (set) Token: 0x060031F4 RID: 12788 RVA: 0x0004AB01 File Offset: 0x00048D01
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

		// Token: 0x17000F1D RID: 3869
		// (get) Token: 0x060031F5 RID: 12789 RVA: 0x0004AB14 File Offset: 0x00048D14
		// (set) Token: 0x060031F6 RID: 12790 RVA: 0x0004AB35 File Offset: 0x00048D35
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

		// Token: 0x060031F7 RID: 12791 RVA: 0x0004AB46 File Offset: 0x00048D46
		public void Set(ref OnIncomingConnectionRequestInfo other)
		{
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.RemoteUserId = other.RemoteUserId;
			this.SocketId = other.SocketId;
		}

		// Token: 0x060031F8 RID: 12792 RVA: 0x0004AB80 File Offset: 0x00048D80
		public void Set(ref OnIncomingConnectionRequestInfo? other)
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

		// Token: 0x060031F9 RID: 12793 RVA: 0x0004ABEE File Offset: 0x00048DEE
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_RemoteUserId);
			Helper.Dispose(ref this.m_SocketId);
		}

		// Token: 0x060031FA RID: 12794 RVA: 0x0004AC21 File Offset: 0x00048E21
		public void Get(out OnIncomingConnectionRequestInfo output)
		{
			output = default(OnIncomingConnectionRequestInfo);
			output.Set(ref this);
		}

		// Token: 0x0400167C RID: 5756
		private IntPtr m_ClientData;

		// Token: 0x0400167D RID: 5757
		private IntPtr m_LocalUserId;

		// Token: 0x0400167E RID: 5758
		private IntPtr m_RemoteUserId;

		// Token: 0x0400167F RID: 5759
		private IntPtr m_SocketId;
	}
}
