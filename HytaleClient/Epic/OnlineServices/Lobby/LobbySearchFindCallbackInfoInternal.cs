using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x020003F4 RID: 1012
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct LobbySearchFindCallbackInfoInternal : ICallbackInfoInternal, IGettable<LobbySearchFindCallbackInfo>, ISettable<LobbySearchFindCallbackInfo>, IDisposable
	{
		// Token: 0x170007C4 RID: 1988
		// (get) Token: 0x06001B42 RID: 6978 RVA: 0x00028FC0 File Offset: 0x000271C0
		// (set) Token: 0x06001B43 RID: 6979 RVA: 0x00028FD8 File Offset: 0x000271D8
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

		// Token: 0x170007C5 RID: 1989
		// (get) Token: 0x06001B44 RID: 6980 RVA: 0x00028FE4 File Offset: 0x000271E4
		// (set) Token: 0x06001B45 RID: 6981 RVA: 0x00029005 File Offset: 0x00027205
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

		// Token: 0x170007C6 RID: 1990
		// (get) Token: 0x06001B46 RID: 6982 RVA: 0x00029018 File Offset: 0x00027218
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x06001B47 RID: 6983 RVA: 0x00029030 File Offset: 0x00027230
		public void Set(ref LobbySearchFindCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
		}

		// Token: 0x06001B48 RID: 6984 RVA: 0x00029050 File Offset: 0x00027250
		public void Set(ref LobbySearchFindCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ResultCode = other.Value.ResultCode;
				this.ClientData = other.Value.ClientData;
			}
		}

		// Token: 0x06001B49 RID: 6985 RVA: 0x00029094 File Offset: 0x00027294
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
		}

		// Token: 0x06001B4A RID: 6986 RVA: 0x000290A3 File Offset: 0x000272A3
		public void Get(out LobbySearchFindCallbackInfo output)
		{
			output = default(LobbySearchFindCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x04000C3E RID: 3134
		private Result m_ResultCode;

		// Token: 0x04000C3F RID: 3135
		private IntPtr m_ClientData;
	}
}
