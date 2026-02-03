using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x02000406 RID: 1030
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct LobbyUpdateReceivedCallbackInfoInternal : ICallbackInfoInternal, IGettable<LobbyUpdateReceivedCallbackInfo>, ISettable<LobbyUpdateReceivedCallbackInfo>, IDisposable
	{
		// Token: 0x170007D9 RID: 2009
		// (get) Token: 0x06001B86 RID: 7046 RVA: 0x000294A0 File Offset: 0x000276A0
		// (set) Token: 0x06001B87 RID: 7047 RVA: 0x000294C1 File Offset: 0x000276C1
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

		// Token: 0x170007DA RID: 2010
		// (get) Token: 0x06001B88 RID: 7048 RVA: 0x000294D4 File Offset: 0x000276D4
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x170007DB RID: 2011
		// (get) Token: 0x06001B89 RID: 7049 RVA: 0x000294EC File Offset: 0x000276EC
		// (set) Token: 0x06001B8A RID: 7050 RVA: 0x0002950D File Offset: 0x0002770D
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

		// Token: 0x06001B8B RID: 7051 RVA: 0x0002951D File Offset: 0x0002771D
		public void Set(ref LobbyUpdateReceivedCallbackInfo other)
		{
			this.ClientData = other.ClientData;
			this.LobbyId = other.LobbyId;
		}

		// Token: 0x06001B8C RID: 7052 RVA: 0x0002953C File Offset: 0x0002773C
		public void Set(ref LobbyUpdateReceivedCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ClientData = other.Value.ClientData;
				this.LobbyId = other.Value.LobbyId;
			}
		}

		// Token: 0x06001B8D RID: 7053 RVA: 0x00029580 File Offset: 0x00027780
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LobbyId);
		}

		// Token: 0x06001B8E RID: 7054 RVA: 0x0002959B File Offset: 0x0002779B
		public void Get(out LobbyUpdateReceivedCallbackInfo output)
		{
			output = default(LobbyUpdateReceivedCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x04000C59 RID: 3161
		private IntPtr m_ClientData;

		// Token: 0x04000C5A RID: 3162
		private IntPtr m_LobbyId;
	}
}
