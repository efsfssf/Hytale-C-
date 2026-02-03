using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x0200044E RID: 1102
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct SendLobbyNativeInviteRequestedCallbackInfoInternal : ICallbackInfoInternal, IGettable<SendLobbyNativeInviteRequestedCallbackInfo>, ISettable<SendLobbyNativeInviteRequestedCallbackInfo>, IDisposable
	{
		// Token: 0x17000825 RID: 2085
		// (get) Token: 0x06001D06 RID: 7430 RVA: 0x0002A6A4 File Offset: 0x000288A4
		// (set) Token: 0x06001D07 RID: 7431 RVA: 0x0002A6C5 File Offset: 0x000288C5
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

		// Token: 0x17000826 RID: 2086
		// (get) Token: 0x06001D08 RID: 7432 RVA: 0x0002A6D8 File Offset: 0x000288D8
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x17000827 RID: 2087
		// (get) Token: 0x06001D09 RID: 7433 RVA: 0x0002A6F0 File Offset: 0x000288F0
		// (set) Token: 0x06001D0A RID: 7434 RVA: 0x0002A708 File Offset: 0x00028908
		public ulong UiEventId
		{
			get
			{
				return this.m_UiEventId;
			}
			set
			{
				this.m_UiEventId = value;
			}
		}

		// Token: 0x17000828 RID: 2088
		// (get) Token: 0x06001D0B RID: 7435 RVA: 0x0002A714 File Offset: 0x00028914
		// (set) Token: 0x06001D0C RID: 7436 RVA: 0x0002A735 File Offset: 0x00028935
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

		// Token: 0x17000829 RID: 2089
		// (get) Token: 0x06001D0D RID: 7437 RVA: 0x0002A748 File Offset: 0x00028948
		// (set) Token: 0x06001D0E RID: 7438 RVA: 0x0002A769 File Offset: 0x00028969
		public Utf8String TargetNativeAccountType
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_TargetNativeAccountType, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_TargetNativeAccountType);
			}
		}

		// Token: 0x1700082A RID: 2090
		// (get) Token: 0x06001D0F RID: 7439 RVA: 0x0002A77C File Offset: 0x0002897C
		// (set) Token: 0x06001D10 RID: 7440 RVA: 0x0002A79D File Offset: 0x0002899D
		public Utf8String TargetUserNativeAccountId
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_TargetUserNativeAccountId, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_TargetUserNativeAccountId);
			}
		}

		// Token: 0x1700082B RID: 2091
		// (get) Token: 0x06001D11 RID: 7441 RVA: 0x0002A7B0 File Offset: 0x000289B0
		// (set) Token: 0x06001D12 RID: 7442 RVA: 0x0002A7D1 File Offset: 0x000289D1
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

		// Token: 0x06001D13 RID: 7443 RVA: 0x0002A7E4 File Offset: 0x000289E4
		public void Set(ref SendLobbyNativeInviteRequestedCallbackInfo other)
		{
			this.ClientData = other.ClientData;
			this.UiEventId = other.UiEventId;
			this.LocalUserId = other.LocalUserId;
			this.TargetNativeAccountType = other.TargetNativeAccountType;
			this.TargetUserNativeAccountId = other.TargetUserNativeAccountId;
			this.LobbyId = other.LobbyId;
		}

		// Token: 0x06001D14 RID: 7444 RVA: 0x0002A840 File Offset: 0x00028A40
		public void Set(ref SendLobbyNativeInviteRequestedCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ClientData = other.Value.ClientData;
				this.UiEventId = other.Value.UiEventId;
				this.LocalUserId = other.Value.LocalUserId;
				this.TargetNativeAccountType = other.Value.TargetNativeAccountType;
				this.TargetUserNativeAccountId = other.Value.TargetUserNativeAccountId;
				this.LobbyId = other.Value.LobbyId;
			}
		}

		// Token: 0x06001D15 RID: 7445 RVA: 0x0002A8DB File Offset: 0x00028ADB
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_TargetNativeAccountType);
			Helper.Dispose(ref this.m_TargetUserNativeAccountId);
			Helper.Dispose(ref this.m_LobbyId);
		}

		// Token: 0x06001D16 RID: 7446 RVA: 0x0002A91A File Offset: 0x00028B1A
		public void Get(out SendLobbyNativeInviteRequestedCallbackInfo output)
		{
			output = default(SendLobbyNativeInviteRequestedCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x04000CA5 RID: 3237
		private IntPtr m_ClientData;

		// Token: 0x04000CA6 RID: 3238
		private ulong m_UiEventId;

		// Token: 0x04000CA7 RID: 3239
		private IntPtr m_LocalUserId;

		// Token: 0x04000CA8 RID: 3240
		private IntPtr m_TargetNativeAccountType;

		// Token: 0x04000CA9 RID: 3241
		private IntPtr m_TargetUserNativeAccountId;

		// Token: 0x04000CAA RID: 3242
		private IntPtr m_LobbyId;
	}
}
