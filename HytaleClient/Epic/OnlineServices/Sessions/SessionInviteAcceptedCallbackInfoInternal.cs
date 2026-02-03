using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x0200015B RID: 347
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct SessionInviteAcceptedCallbackInfoInternal : ICallbackInfoInternal, IGettable<SessionInviteAcceptedCallbackInfo>, ISettable<SessionInviteAcceptedCallbackInfo>, IDisposable
	{
		// Token: 0x17000254 RID: 596
		// (get) Token: 0x06000A79 RID: 2681 RVA: 0x0000EB38 File Offset: 0x0000CD38
		// (set) Token: 0x06000A7A RID: 2682 RVA: 0x0000EB59 File Offset: 0x0000CD59
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

		// Token: 0x17000255 RID: 597
		// (get) Token: 0x06000A7B RID: 2683 RVA: 0x0000EB6C File Offset: 0x0000CD6C
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x17000256 RID: 598
		// (get) Token: 0x06000A7C RID: 2684 RVA: 0x0000EB84 File Offset: 0x0000CD84
		// (set) Token: 0x06000A7D RID: 2685 RVA: 0x0000EBA5 File Offset: 0x0000CDA5
		public Utf8String SessionId
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_SessionId, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_SessionId);
			}
		}

		// Token: 0x17000257 RID: 599
		// (get) Token: 0x06000A7E RID: 2686 RVA: 0x0000EBB8 File Offset: 0x0000CDB8
		// (set) Token: 0x06000A7F RID: 2687 RVA: 0x0000EBD9 File Offset: 0x0000CDD9
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

		// Token: 0x17000258 RID: 600
		// (get) Token: 0x06000A80 RID: 2688 RVA: 0x0000EBEC File Offset: 0x0000CDEC
		// (set) Token: 0x06000A81 RID: 2689 RVA: 0x0000EC0D File Offset: 0x0000CE0D
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

		// Token: 0x17000259 RID: 601
		// (get) Token: 0x06000A82 RID: 2690 RVA: 0x0000EC20 File Offset: 0x0000CE20
		// (set) Token: 0x06000A83 RID: 2691 RVA: 0x0000EC41 File Offset: 0x0000CE41
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

		// Token: 0x06000A84 RID: 2692 RVA: 0x0000EC54 File Offset: 0x0000CE54
		public void Set(ref SessionInviteAcceptedCallbackInfo other)
		{
			this.ClientData = other.ClientData;
			this.SessionId = other.SessionId;
			this.LocalUserId = other.LocalUserId;
			this.TargetUserId = other.TargetUserId;
			this.InviteId = other.InviteId;
		}

		// Token: 0x06000A85 RID: 2693 RVA: 0x0000ECA4 File Offset: 0x0000CEA4
		public void Set(ref SessionInviteAcceptedCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ClientData = other.Value.ClientData;
				this.SessionId = other.Value.SessionId;
				this.LocalUserId = other.Value.LocalUserId;
				this.TargetUserId = other.Value.TargetUserId;
				this.InviteId = other.Value.InviteId;
			}
		}

		// Token: 0x06000A86 RID: 2694 RVA: 0x0000ED27 File Offset: 0x0000CF27
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_SessionId);
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_TargetUserId);
			Helper.Dispose(ref this.m_InviteId);
		}

		// Token: 0x06000A87 RID: 2695 RVA: 0x0000ED66 File Offset: 0x0000CF66
		public void Get(out SessionInviteAcceptedCallbackInfo output)
		{
			output = default(SessionInviteAcceptedCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x040004C2 RID: 1218
		private IntPtr m_ClientData;

		// Token: 0x040004C3 RID: 1219
		private IntPtr m_SessionId;

		// Token: 0x040004C4 RID: 1220
		private IntPtr m_LocalUserId;

		// Token: 0x040004C5 RID: 1221
		private IntPtr m_TargetUserId;

		// Token: 0x040004C6 RID: 1222
		private IntPtr m_InviteId;
	}
}
