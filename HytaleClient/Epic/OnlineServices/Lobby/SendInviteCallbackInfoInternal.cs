using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x0200044A RID: 1098
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct SendInviteCallbackInfoInternal : ICallbackInfoInternal, IGettable<SendInviteCallbackInfo>, ISettable<SendInviteCallbackInfo>, IDisposable
	{
		// Token: 0x17000815 RID: 2069
		// (get) Token: 0x06001CE1 RID: 7393 RVA: 0x0002A354 File Offset: 0x00028554
		// (set) Token: 0x06001CE2 RID: 7394 RVA: 0x0002A36C File Offset: 0x0002856C
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

		// Token: 0x17000816 RID: 2070
		// (get) Token: 0x06001CE3 RID: 7395 RVA: 0x0002A378 File Offset: 0x00028578
		// (set) Token: 0x06001CE4 RID: 7396 RVA: 0x0002A399 File Offset: 0x00028599
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

		// Token: 0x17000817 RID: 2071
		// (get) Token: 0x06001CE5 RID: 7397 RVA: 0x0002A3AC File Offset: 0x000285AC
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x17000818 RID: 2072
		// (get) Token: 0x06001CE6 RID: 7398 RVA: 0x0002A3C4 File Offset: 0x000285C4
		// (set) Token: 0x06001CE7 RID: 7399 RVA: 0x0002A3E5 File Offset: 0x000285E5
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

		// Token: 0x06001CE8 RID: 7400 RVA: 0x0002A3F5 File Offset: 0x000285F5
		public void Set(ref SendInviteCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LobbyId = other.LobbyId;
		}

		// Token: 0x06001CE9 RID: 7401 RVA: 0x0002A420 File Offset: 0x00028620
		public void Set(ref SendInviteCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ResultCode = other.Value.ResultCode;
				this.ClientData = other.Value.ClientData;
				this.LobbyId = other.Value.LobbyId;
			}
		}

		// Token: 0x06001CEA RID: 7402 RVA: 0x0002A479 File Offset: 0x00028679
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LobbyId);
		}

		// Token: 0x06001CEB RID: 7403 RVA: 0x0002A494 File Offset: 0x00028694
		public void Get(out SendInviteCallbackInfo output)
		{
			output = default(SendInviteCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x04000C95 RID: 3221
		private Result m_ResultCode;

		// Token: 0x04000C96 RID: 3222
		private IntPtr m_ClientData;

		// Token: 0x04000C97 RID: 3223
		private IntPtr m_LobbyId;
	}
}
