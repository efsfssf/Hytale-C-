using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.P2P
{
	// Token: 0x02000774 RID: 1908
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct AddNotifyPeerConnectionInterruptedOptionsInternal : ISettable<AddNotifyPeerConnectionInterruptedOptions>, IDisposable
	{
		// Token: 0x17000EFF RID: 3839
		// (set) Token: 0x060031A1 RID: 12705 RVA: 0x0004A484 File Offset: 0x00048684
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x17000F00 RID: 3840
		// (set) Token: 0x060031A2 RID: 12706 RVA: 0x0004A494 File Offset: 0x00048694
		public SocketId? SocketId
		{
			set
			{
				Helper.Set<SocketId, SocketIdInternal>(ref value, ref this.m_SocketId);
			}
		}

		// Token: 0x060031A3 RID: 12707 RVA: 0x0004A4A5 File Offset: 0x000486A5
		public void Set(ref AddNotifyPeerConnectionInterruptedOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
			this.SocketId = other.SocketId;
		}

		// Token: 0x060031A4 RID: 12708 RVA: 0x0004A4CC File Offset: 0x000486CC
		public void Set(ref AddNotifyPeerConnectionInterruptedOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
				this.SocketId = other.Value.SocketId;
			}
		}

		// Token: 0x060031A5 RID: 12709 RVA: 0x0004A517 File Offset: 0x00048717
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_SocketId);
		}

		// Token: 0x04001640 RID: 5696
		private int m_ApiVersion;

		// Token: 0x04001641 RID: 5697
		private IntPtr m_LocalUserId;

		// Token: 0x04001642 RID: 5698
		private IntPtr m_SocketId;
	}
}
