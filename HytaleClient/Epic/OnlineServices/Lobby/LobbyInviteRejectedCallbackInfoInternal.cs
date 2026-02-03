using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x020003D5 RID: 981
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct LobbyInviteRejectedCallbackInfoInternal : ICallbackInfoInternal, IGettable<LobbyInviteRejectedCallbackInfo>, ISettable<LobbyInviteRejectedCallbackInfo>, IDisposable
	{
		// Token: 0x17000794 RID: 1940
		// (get) Token: 0x06001AA9 RID: 6825 RVA: 0x00027F14 File Offset: 0x00026114
		// (set) Token: 0x06001AAA RID: 6826 RVA: 0x00027F35 File Offset: 0x00026135
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

		// Token: 0x17000795 RID: 1941
		// (get) Token: 0x06001AAB RID: 6827 RVA: 0x00027F48 File Offset: 0x00026148
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x17000796 RID: 1942
		// (get) Token: 0x06001AAC RID: 6828 RVA: 0x00027F60 File Offset: 0x00026160
		// (set) Token: 0x06001AAD RID: 6829 RVA: 0x00027F81 File Offset: 0x00026181
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

		// Token: 0x17000797 RID: 1943
		// (get) Token: 0x06001AAE RID: 6830 RVA: 0x00027F94 File Offset: 0x00026194
		// (set) Token: 0x06001AAF RID: 6831 RVA: 0x00027FB5 File Offset: 0x000261B5
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

		// Token: 0x17000798 RID: 1944
		// (get) Token: 0x06001AB0 RID: 6832 RVA: 0x00027FC8 File Offset: 0x000261C8
		// (set) Token: 0x06001AB1 RID: 6833 RVA: 0x00027FE9 File Offset: 0x000261E9
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

		// Token: 0x17000799 RID: 1945
		// (get) Token: 0x06001AB2 RID: 6834 RVA: 0x00027FFC File Offset: 0x000261FC
		// (set) Token: 0x06001AB3 RID: 6835 RVA: 0x0002801D File Offset: 0x0002621D
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

		// Token: 0x06001AB4 RID: 6836 RVA: 0x00028030 File Offset: 0x00026230
		public void Set(ref LobbyInviteRejectedCallbackInfo other)
		{
			this.ClientData = other.ClientData;
			this.InviteId = other.InviteId;
			this.LocalUserId = other.LocalUserId;
			this.TargetUserId = other.TargetUserId;
			this.LobbyId = other.LobbyId;
		}

		// Token: 0x06001AB5 RID: 6837 RVA: 0x00028080 File Offset: 0x00026280
		public void Set(ref LobbyInviteRejectedCallbackInfo? other)
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

		// Token: 0x06001AB6 RID: 6838 RVA: 0x00028103 File Offset: 0x00026303
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_InviteId);
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_TargetUserId);
			Helper.Dispose(ref this.m_LobbyId);
		}

		// Token: 0x06001AB7 RID: 6839 RVA: 0x00028142 File Offset: 0x00026342
		public void Get(out LobbyInviteRejectedCallbackInfo output)
		{
			output = default(LobbyInviteRejectedCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x04000BE5 RID: 3045
		private IntPtr m_ClientData;

		// Token: 0x04000BE6 RID: 3046
		private IntPtr m_InviteId;

		// Token: 0x04000BE7 RID: 3047
		private IntPtr m_LocalUserId;

		// Token: 0x04000BE8 RID: 3048
		private IntPtr m_TargetUserId;

		// Token: 0x04000BE9 RID: 3049
		private IntPtr m_LobbyId;
	}
}
