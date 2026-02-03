using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x020003D1 RID: 977
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct LobbyInviteAcceptedCallbackInfoInternal : ICallbackInfoInternal, IGettable<LobbyInviteAcceptedCallbackInfo>, ISettable<LobbyInviteAcceptedCallbackInfo>, IDisposable
	{
		// Token: 0x17000780 RID: 1920
		// (get) Token: 0x06001A77 RID: 6775 RVA: 0x000279AC File Offset: 0x00025BAC
		// (set) Token: 0x06001A78 RID: 6776 RVA: 0x000279CD File Offset: 0x00025BCD
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

		// Token: 0x17000781 RID: 1921
		// (get) Token: 0x06001A79 RID: 6777 RVA: 0x000279E0 File Offset: 0x00025BE0
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x17000782 RID: 1922
		// (get) Token: 0x06001A7A RID: 6778 RVA: 0x000279F8 File Offset: 0x00025BF8
		// (set) Token: 0x06001A7B RID: 6779 RVA: 0x00027A19 File Offset: 0x00025C19
		public Utf8String InviteId
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_InviteId, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_InviteId);
			}
		}

		// Token: 0x17000783 RID: 1923
		// (get) Token: 0x06001A7C RID: 6780 RVA: 0x00027A2C File Offset: 0x00025C2C
		// (set) Token: 0x06001A7D RID: 6781 RVA: 0x00027A4D File Offset: 0x00025C4D
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

		// Token: 0x17000784 RID: 1924
		// (get) Token: 0x06001A7E RID: 6782 RVA: 0x00027A60 File Offset: 0x00025C60
		// (set) Token: 0x06001A7F RID: 6783 RVA: 0x00027A81 File Offset: 0x00025C81
		public ProductUserId TargetUserId
		{
			get
			{
				ProductUserId result;
				Helper.Get<ProductUserId>(this.m_TargetUserId, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_TargetUserId);
			}
		}

		// Token: 0x17000785 RID: 1925
		// (get) Token: 0x06001A80 RID: 6784 RVA: 0x00027A94 File Offset: 0x00025C94
		// (set) Token: 0x06001A81 RID: 6785 RVA: 0x00027AB5 File Offset: 0x00025CB5
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

		// Token: 0x06001A82 RID: 6786 RVA: 0x00027AC8 File Offset: 0x00025CC8
		public void Set(ref LobbyInviteAcceptedCallbackInfo other)
		{
			this.ClientData = other.ClientData;
			this.InviteId = other.InviteId;
			this.LocalUserId = other.LocalUserId;
			this.TargetUserId = other.TargetUserId;
			this.LobbyId = other.LobbyId;
		}

		// Token: 0x06001A83 RID: 6787 RVA: 0x00027B18 File Offset: 0x00025D18
		public void Set(ref LobbyInviteAcceptedCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ClientData = other.Value.ClientData;
				this.InviteId = other.Value.InviteId;
				this.LocalUserId = other.Value.LocalUserId;
				this.TargetUserId = other.Value.TargetUserId;
				this.LobbyId = other.Value.LobbyId;
			}
		}

		// Token: 0x06001A84 RID: 6788 RVA: 0x00027B9B File Offset: 0x00025D9B
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_InviteId);
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_TargetUserId);
			Helper.Dispose(ref this.m_LobbyId);
		}

		// Token: 0x06001A85 RID: 6789 RVA: 0x00027BDA File Offset: 0x00025DDA
		public void Get(out LobbyInviteAcceptedCallbackInfo output)
		{
			output = default(LobbyInviteAcceptedCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x04000BD3 RID: 3027
		private IntPtr m_ClientData;

		// Token: 0x04000BD4 RID: 3028
		private IntPtr m_InviteId;

		// Token: 0x04000BD5 RID: 3029
		private IntPtr m_LocalUserId;

		// Token: 0x04000BD6 RID: 3030
		private IntPtr m_TargetUserId;

		// Token: 0x04000BD7 RID: 3031
		private IntPtr m_LobbyId;
	}
}
