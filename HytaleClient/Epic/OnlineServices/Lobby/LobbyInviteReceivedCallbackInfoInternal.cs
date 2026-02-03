using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x020003D3 RID: 979
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct LobbyInviteReceivedCallbackInfoInternal : ICallbackInfoInternal, IGettable<LobbyInviteReceivedCallbackInfo>, ISettable<LobbyInviteReceivedCallbackInfo>, IDisposable
	{
		// Token: 0x1700078A RID: 1930
		// (get) Token: 0x06001A90 RID: 6800 RVA: 0x00027C84 File Offset: 0x00025E84
		// (set) Token: 0x06001A91 RID: 6801 RVA: 0x00027CA5 File Offset: 0x00025EA5
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

		// Token: 0x1700078B RID: 1931
		// (get) Token: 0x06001A92 RID: 6802 RVA: 0x00027CB8 File Offset: 0x00025EB8
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x1700078C RID: 1932
		// (get) Token: 0x06001A93 RID: 6803 RVA: 0x00027CD0 File Offset: 0x00025ED0
		// (set) Token: 0x06001A94 RID: 6804 RVA: 0x00027CF1 File Offset: 0x00025EF1
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

		// Token: 0x1700078D RID: 1933
		// (get) Token: 0x06001A95 RID: 6805 RVA: 0x00027D04 File Offset: 0x00025F04
		// (set) Token: 0x06001A96 RID: 6806 RVA: 0x00027D25 File Offset: 0x00025F25
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

		// Token: 0x1700078E RID: 1934
		// (get) Token: 0x06001A97 RID: 6807 RVA: 0x00027D38 File Offset: 0x00025F38
		// (set) Token: 0x06001A98 RID: 6808 RVA: 0x00027D59 File Offset: 0x00025F59
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

		// Token: 0x06001A99 RID: 6809 RVA: 0x00027D69 File Offset: 0x00025F69
		public void Set(ref LobbyInviteReceivedCallbackInfo other)
		{
			this.ClientData = other.ClientData;
			this.InviteId = other.InviteId;
			this.LocalUserId = other.LocalUserId;
			this.TargetUserId = other.TargetUserId;
		}

		// Token: 0x06001A9A RID: 6810 RVA: 0x00027DA0 File Offset: 0x00025FA0
		public void Set(ref LobbyInviteReceivedCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ClientData = other.Value.ClientData;
				this.InviteId = other.Value.InviteId;
				this.LocalUserId = other.Value.LocalUserId;
				this.TargetUserId = other.Value.TargetUserId;
			}
		}

		// Token: 0x06001A9B RID: 6811 RVA: 0x00027E0E File Offset: 0x0002600E
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_InviteId);
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_TargetUserId);
		}

		// Token: 0x06001A9C RID: 6812 RVA: 0x00027E41 File Offset: 0x00026041
		public void Get(out LobbyInviteReceivedCallbackInfo output)
		{
			output = default(LobbyInviteReceivedCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x04000BDC RID: 3036
		private IntPtr m_ClientData;

		// Token: 0x04000BDD RID: 3037
		private IntPtr m_InviteId;

		// Token: 0x04000BDE RID: 3038
		private IntPtr m_LocalUserId;

		// Token: 0x04000BDF RID: 3039
		private IntPtr m_TargetUserId;
	}
}
