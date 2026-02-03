using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x02000380 RID: 896
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct CreateLobbyCallbackInfoInternal : ICallbackInfoInternal, IGettable<CreateLobbyCallbackInfo>, ISettable<CreateLobbyCallbackInfo>, IDisposable
	{
		// Token: 0x17000693 RID: 1683
		// (get) Token: 0x060017F6 RID: 6134 RVA: 0x000230E0 File Offset: 0x000212E0
		// (set) Token: 0x060017F7 RID: 6135 RVA: 0x000230F8 File Offset: 0x000212F8
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

		// Token: 0x17000694 RID: 1684
		// (get) Token: 0x060017F8 RID: 6136 RVA: 0x00023104 File Offset: 0x00021304
		// (set) Token: 0x060017F9 RID: 6137 RVA: 0x00023125 File Offset: 0x00021325
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

		// Token: 0x17000695 RID: 1685
		// (get) Token: 0x060017FA RID: 6138 RVA: 0x00023138 File Offset: 0x00021338
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x17000696 RID: 1686
		// (get) Token: 0x060017FB RID: 6139 RVA: 0x00023150 File Offset: 0x00021350
		// (set) Token: 0x060017FC RID: 6140 RVA: 0x00023171 File Offset: 0x00021371
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

		// Token: 0x060017FD RID: 6141 RVA: 0x00023181 File Offset: 0x00021381
		public void Set(ref CreateLobbyCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LobbyId = other.LobbyId;
		}

		// Token: 0x060017FE RID: 6142 RVA: 0x000231AC File Offset: 0x000213AC
		public void Set(ref CreateLobbyCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ResultCode = other.Value.ResultCode;
				this.ClientData = other.Value.ClientData;
				this.LobbyId = other.Value.LobbyId;
			}
		}

		// Token: 0x060017FF RID: 6143 RVA: 0x00023205 File Offset: 0x00021405
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LobbyId);
		}

		// Token: 0x06001800 RID: 6144 RVA: 0x00023220 File Offset: 0x00021420
		public void Get(out CreateLobbyCallbackInfo output)
		{
			output = default(CreateLobbyCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x04000A92 RID: 2706
		private Result m_ResultCode;

		// Token: 0x04000A93 RID: 2707
		private IntPtr m_ClientData;

		// Token: 0x04000A94 RID: 2708
		private IntPtr m_LobbyId;
	}
}
