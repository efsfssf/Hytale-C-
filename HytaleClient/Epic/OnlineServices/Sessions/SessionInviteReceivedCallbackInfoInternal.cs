using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x0200015D RID: 349
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct SessionInviteReceivedCallbackInfoInternal : ICallbackInfoInternal, IGettable<SessionInviteReceivedCallbackInfo>, ISettable<SessionInviteReceivedCallbackInfo>, IDisposable
	{
		// Token: 0x1700025E RID: 606
		// (get) Token: 0x06000A92 RID: 2706 RVA: 0x0000EE10 File Offset: 0x0000D010
		// (set) Token: 0x06000A93 RID: 2707 RVA: 0x0000EE31 File Offset: 0x0000D031
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

		// Token: 0x1700025F RID: 607
		// (get) Token: 0x06000A94 RID: 2708 RVA: 0x0000EE44 File Offset: 0x0000D044
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x17000260 RID: 608
		// (get) Token: 0x06000A95 RID: 2709 RVA: 0x0000EE5C File Offset: 0x0000D05C
		// (set) Token: 0x06000A96 RID: 2710 RVA: 0x0000EE7D File Offset: 0x0000D07D
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

		// Token: 0x17000261 RID: 609
		// (get) Token: 0x06000A97 RID: 2711 RVA: 0x0000EE90 File Offset: 0x0000D090
		// (set) Token: 0x06000A98 RID: 2712 RVA: 0x0000EEB1 File Offset: 0x0000D0B1
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

		// Token: 0x17000262 RID: 610
		// (get) Token: 0x06000A99 RID: 2713 RVA: 0x0000EEC4 File Offset: 0x0000D0C4
		// (set) Token: 0x06000A9A RID: 2714 RVA: 0x0000EEE5 File Offset: 0x0000D0E5
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

		// Token: 0x06000A9B RID: 2715 RVA: 0x0000EEF5 File Offset: 0x0000D0F5
		public void Set(ref SessionInviteReceivedCallbackInfo other)
		{
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.TargetUserId = other.TargetUserId;
			this.InviteId = other.InviteId;
		}

		// Token: 0x06000A9C RID: 2716 RVA: 0x0000EF2C File Offset: 0x0000D12C
		public void Set(ref SessionInviteReceivedCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ClientData = other.Value.ClientData;
				this.LocalUserId = other.Value.LocalUserId;
				this.TargetUserId = other.Value.TargetUserId;
				this.InviteId = other.Value.InviteId;
			}
		}

		// Token: 0x06000A9D RID: 2717 RVA: 0x0000EF9A File Offset: 0x0000D19A
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_TargetUserId);
			Helper.Dispose(ref this.m_InviteId);
		}

		// Token: 0x06000A9E RID: 2718 RVA: 0x0000EFCD File Offset: 0x0000D1CD
		public void Get(out SessionInviteReceivedCallbackInfo output)
		{
			output = default(SessionInviteReceivedCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x040004CB RID: 1227
		private IntPtr m_ClientData;

		// Token: 0x040004CC RID: 1228
		private IntPtr m_LocalUserId;

		// Token: 0x040004CD RID: 1229
		private IntPtr m_TargetUserId;

		// Token: 0x040004CE RID: 1230
		private IntPtr m_InviteId;
	}
}
