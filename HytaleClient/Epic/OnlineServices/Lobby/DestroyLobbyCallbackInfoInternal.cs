using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x02000386 RID: 902
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct DestroyLobbyCallbackInfoInternal : ICallbackInfoInternal, IGettable<DestroyLobbyCallbackInfo>, ISettable<DestroyLobbyCallbackInfo>, IDisposable
	{
		// Token: 0x170006BA RID: 1722
		// (get) Token: 0x0600183F RID: 6207 RVA: 0x0002377C File Offset: 0x0002197C
		// (set) Token: 0x06001840 RID: 6208 RVA: 0x00023794 File Offset: 0x00021994
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

		// Token: 0x170006BB RID: 1723
		// (get) Token: 0x06001841 RID: 6209 RVA: 0x000237A0 File Offset: 0x000219A0
		// (set) Token: 0x06001842 RID: 6210 RVA: 0x000237C1 File Offset: 0x000219C1
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

		// Token: 0x170006BC RID: 1724
		// (get) Token: 0x06001843 RID: 6211 RVA: 0x000237D4 File Offset: 0x000219D4
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x170006BD RID: 1725
		// (get) Token: 0x06001844 RID: 6212 RVA: 0x000237EC File Offset: 0x000219EC
		// (set) Token: 0x06001845 RID: 6213 RVA: 0x0002380D File Offset: 0x00021A0D
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

		// Token: 0x06001846 RID: 6214 RVA: 0x0002381D File Offset: 0x00021A1D
		public void Set(ref DestroyLobbyCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LobbyId = other.LobbyId;
		}

		// Token: 0x06001847 RID: 6215 RVA: 0x00023848 File Offset: 0x00021A48
		public void Set(ref DestroyLobbyCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ResultCode = other.Value.ResultCode;
				this.ClientData = other.Value.ClientData;
				this.LobbyId = other.Value.LobbyId;
			}
		}

		// Token: 0x06001848 RID: 6216 RVA: 0x000238A1 File Offset: 0x00021AA1
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LobbyId);
		}

		// Token: 0x06001849 RID: 6217 RVA: 0x000238BC File Offset: 0x00021ABC
		public void Get(out DestroyLobbyCallbackInfo output)
		{
			output = default(DestroyLobbyCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x04000ABB RID: 2747
		private Result m_ResultCode;

		// Token: 0x04000ABC RID: 2748
		private IntPtr m_ClientData;

		// Token: 0x04000ABD RID: 2749
		private IntPtr m_LobbyId;
	}
}
