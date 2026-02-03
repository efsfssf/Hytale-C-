using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x02000450 RID: 1104
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct UpdateLobbyCallbackInfoInternal : ICallbackInfoInternal, IGettable<UpdateLobbyCallbackInfo>, ISettable<UpdateLobbyCallbackInfo>, IDisposable
	{
		// Token: 0x1700082F RID: 2095
		// (get) Token: 0x06001D1F RID: 7455 RVA: 0x0002A9A8 File Offset: 0x00028BA8
		// (set) Token: 0x06001D20 RID: 7456 RVA: 0x0002A9C0 File Offset: 0x00028BC0
		public Result ResultCode
		{
			get
			{
				return this.m_ResultCode;
			}
			set
			{
				this.m_ResultCode = value;
			}
		}

		// Token: 0x17000830 RID: 2096
		// (get) Token: 0x06001D21 RID: 7457 RVA: 0x0002A9CC File Offset: 0x00028BCC
		// (set) Token: 0x06001D22 RID: 7458 RVA: 0x0002A9ED File Offset: 0x00028BED
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

		// Token: 0x17000831 RID: 2097
		// (get) Token: 0x06001D23 RID: 7459 RVA: 0x0002AA00 File Offset: 0x00028C00
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x17000832 RID: 2098
		// (get) Token: 0x06001D24 RID: 7460 RVA: 0x0002AA18 File Offset: 0x00028C18
		// (set) Token: 0x06001D25 RID: 7461 RVA: 0x0002AA39 File Offset: 0x00028C39
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

		// Token: 0x06001D26 RID: 7462 RVA: 0x0002AA49 File Offset: 0x00028C49
		public void Set(ref UpdateLobbyCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LobbyId = other.LobbyId;
		}

		// Token: 0x06001D27 RID: 7463 RVA: 0x0002AA74 File Offset: 0x00028C74
		public void Set(ref UpdateLobbyCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ResultCode = other.Value.ResultCode;
				this.ClientData = other.Value.ClientData;
				this.LobbyId = other.Value.LobbyId;
			}
		}

		// Token: 0x06001D28 RID: 7464 RVA: 0x0002AACD File Offset: 0x00028CCD
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LobbyId);
		}

		// Token: 0x06001D29 RID: 7465 RVA: 0x0002AAE8 File Offset: 0x00028CE8
		public void Get(out UpdateLobbyCallbackInfo output)
		{
			output = default(UpdateLobbyCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x04000CAE RID: 3246
		private Result m_ResultCode;

		// Token: 0x04000CAF RID: 3247
		private IntPtr m_ClientData;

		// Token: 0x04000CB0 RID: 3248
		private IntPtr m_LobbyId;
	}
}
