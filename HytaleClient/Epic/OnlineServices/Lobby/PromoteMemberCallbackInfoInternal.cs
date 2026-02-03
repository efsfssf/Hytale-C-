using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x0200043C RID: 1084
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct PromoteMemberCallbackInfoInternal : ICallbackInfoInternal, IGettable<PromoteMemberCallbackInfo>, ISettable<PromoteMemberCallbackInfo>, IDisposable
	{
		// Token: 0x170007E9 RID: 2025
		// (get) Token: 0x06001C72 RID: 7282 RVA: 0x000298A8 File Offset: 0x00027AA8
		// (set) Token: 0x06001C73 RID: 7283 RVA: 0x000298C0 File Offset: 0x00027AC0
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

		// Token: 0x170007EA RID: 2026
		// (get) Token: 0x06001C74 RID: 7284 RVA: 0x000298CC File Offset: 0x00027ACC
		// (set) Token: 0x06001C75 RID: 7285 RVA: 0x000298ED File Offset: 0x00027AED
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

		// Token: 0x170007EB RID: 2027
		// (get) Token: 0x06001C76 RID: 7286 RVA: 0x00029900 File Offset: 0x00027B00
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x170007EC RID: 2028
		// (get) Token: 0x06001C77 RID: 7287 RVA: 0x00029918 File Offset: 0x00027B18
		// (set) Token: 0x06001C78 RID: 7288 RVA: 0x00029939 File Offset: 0x00027B39
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

		// Token: 0x06001C79 RID: 7289 RVA: 0x00029949 File Offset: 0x00027B49
		public void Set(ref PromoteMemberCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LobbyId = other.LobbyId;
		}

		// Token: 0x06001C7A RID: 7290 RVA: 0x00029974 File Offset: 0x00027B74
		public void Set(ref PromoteMemberCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ResultCode = other.Value.ResultCode;
				this.ClientData = other.Value.ClientData;
				this.LobbyId = other.Value.LobbyId;
			}
		}

		// Token: 0x06001C7B RID: 7291 RVA: 0x000299CD File Offset: 0x00027BCD
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LobbyId);
		}

		// Token: 0x06001C7C RID: 7292 RVA: 0x000299E8 File Offset: 0x00027BE8
		public void Get(out PromoteMemberCallbackInfo output)
		{
			output = default(PromoteMemberCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x04000C6A RID: 3178
		private Result m_ResultCode;

		// Token: 0x04000C6B RID: 3179
		private IntPtr m_ClientData;

		// Token: 0x04000C6C RID: 3180
		private IntPtr m_LobbyId;
	}
}
