using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x02000448 RID: 1096
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct RTCRoomConnectionChangedCallbackInfoInternal : ICallbackInfoInternal, IGettable<RTCRoomConnectionChangedCallbackInfo>, ISettable<RTCRoomConnectionChangedCallbackInfo>, IDisposable
	{
		// Token: 0x1700080C RID: 2060
		// (get) Token: 0x06001CCA RID: 7370 RVA: 0x0002A0C0 File Offset: 0x000282C0
		// (set) Token: 0x06001CCB RID: 7371 RVA: 0x0002A0E1 File Offset: 0x000282E1
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

		// Token: 0x1700080D RID: 2061
		// (get) Token: 0x06001CCC RID: 7372 RVA: 0x0002A0F4 File Offset: 0x000282F4
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x1700080E RID: 2062
		// (get) Token: 0x06001CCD RID: 7373 RVA: 0x0002A10C File Offset: 0x0002830C
		// (set) Token: 0x06001CCE RID: 7374 RVA: 0x0002A12D File Offset: 0x0002832D
		public Utf8String LobbyId
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_LobbyId, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_LobbyId);
			}
		}

		// Token: 0x1700080F RID: 2063
		// (get) Token: 0x06001CCF RID: 7375 RVA: 0x0002A140 File Offset: 0x00028340
		// (set) Token: 0x06001CD0 RID: 7376 RVA: 0x0002A161 File Offset: 0x00028361
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

		// Token: 0x17000810 RID: 2064
		// (get) Token: 0x06001CD1 RID: 7377 RVA: 0x0002A174 File Offset: 0x00028374
		// (set) Token: 0x06001CD2 RID: 7378 RVA: 0x0002A195 File Offset: 0x00028395
		public bool IsConnected
		{
			get
			{
				bool result;
				Helper.Get(this.m_IsConnected, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_IsConnected);
			}
		}

		// Token: 0x17000811 RID: 2065
		// (get) Token: 0x06001CD3 RID: 7379 RVA: 0x0002A1A8 File Offset: 0x000283A8
		// (set) Token: 0x06001CD4 RID: 7380 RVA: 0x0002A1C0 File Offset: 0x000283C0
		public Result DisconnectReason
		{
			get
			{
				return this.m_DisconnectReason;
			}
			set
			{
				this.m_DisconnectReason = value;
			}
		}

		// Token: 0x06001CD5 RID: 7381 RVA: 0x0002A1CC File Offset: 0x000283CC
		public void Set(ref RTCRoomConnectionChangedCallbackInfo other)
		{
			this.ClientData = other.ClientData;
			this.LobbyId = other.LobbyId;
			this.LocalUserId = other.LocalUserId;
			this.IsConnected = other.IsConnected;
			this.DisconnectReason = other.DisconnectReason;
		}

		// Token: 0x06001CD6 RID: 7382 RVA: 0x0002A21C File Offset: 0x0002841C
		public void Set(ref RTCRoomConnectionChangedCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ClientData = other.Value.ClientData;
				this.LobbyId = other.Value.LobbyId;
				this.LocalUserId = other.Value.LocalUserId;
				this.IsConnected = other.Value.IsConnected;
				this.DisconnectReason = other.Value.DisconnectReason;
			}
		}

		// Token: 0x06001CD7 RID: 7383 RVA: 0x0002A29F File Offset: 0x0002849F
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LobbyId);
			Helper.Dispose(ref this.m_LocalUserId);
		}

		// Token: 0x06001CD8 RID: 7384 RVA: 0x0002A2C6 File Offset: 0x000284C6
		public void Get(out RTCRoomConnectionChangedCallbackInfo output)
		{
			output = default(RTCRoomConnectionChangedCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x04000C8D RID: 3213
		private IntPtr m_ClientData;

		// Token: 0x04000C8E RID: 3214
		private IntPtr m_LobbyId;

		// Token: 0x04000C8F RID: 3215
		private IntPtr m_LocalUserId;

		// Token: 0x04000C90 RID: 3216
		private int m_IsConnected;

		// Token: 0x04000C91 RID: 3217
		private Result m_DisconnectReason;
	}
}
