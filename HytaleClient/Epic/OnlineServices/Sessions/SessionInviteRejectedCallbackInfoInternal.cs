using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x0200015F RID: 351
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct SessionInviteRejectedCallbackInfoInternal : ICallbackInfoInternal, IGettable<SessionInviteRejectedCallbackInfo>, ISettable<SessionInviteRejectedCallbackInfo>, IDisposable
	{
		// Token: 0x17000268 RID: 616
		// (get) Token: 0x06000AAB RID: 2731 RVA: 0x0000F0A0 File Offset: 0x0000D2A0
		// (set) Token: 0x06000AAC RID: 2732 RVA: 0x0000F0C1 File Offset: 0x0000D2C1
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

		// Token: 0x17000269 RID: 617
		// (get) Token: 0x06000AAD RID: 2733 RVA: 0x0000F0D4 File Offset: 0x0000D2D4
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x1700026A RID: 618
		// (get) Token: 0x06000AAE RID: 2734 RVA: 0x0000F0EC File Offset: 0x0000D2EC
		// (set) Token: 0x06000AAF RID: 2735 RVA: 0x0000F10D File Offset: 0x0000D30D
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

		// Token: 0x1700026B RID: 619
		// (get) Token: 0x06000AB0 RID: 2736 RVA: 0x0000F120 File Offset: 0x0000D320
		// (set) Token: 0x06000AB1 RID: 2737 RVA: 0x0000F141 File Offset: 0x0000D341
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

		// Token: 0x1700026C RID: 620
		// (get) Token: 0x06000AB2 RID: 2738 RVA: 0x0000F154 File Offset: 0x0000D354
		// (set) Token: 0x06000AB3 RID: 2739 RVA: 0x0000F175 File Offset: 0x0000D375
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

		// Token: 0x1700026D RID: 621
		// (get) Token: 0x06000AB4 RID: 2740 RVA: 0x0000F188 File Offset: 0x0000D388
		// (set) Token: 0x06000AB5 RID: 2741 RVA: 0x0000F1A9 File Offset: 0x0000D3A9
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

		// Token: 0x06000AB6 RID: 2742 RVA: 0x0000F1BC File Offset: 0x0000D3BC
		public void Set(ref SessionInviteRejectedCallbackInfo other)
		{
			this.ClientData = other.ClientData;
			this.InviteId = other.InviteId;
			this.LocalUserId = other.LocalUserId;
			this.TargetUserId = other.TargetUserId;
			this.SessionId = other.SessionId;
		}

		// Token: 0x06000AB7 RID: 2743 RVA: 0x0000F20C File Offset: 0x0000D40C
		public void Set(ref SessionInviteRejectedCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ClientData = other.Value.ClientData;
				this.InviteId = other.Value.InviteId;
				this.LocalUserId = other.Value.LocalUserId;
				this.TargetUserId = other.Value.TargetUserId;
				this.SessionId = other.Value.SessionId;
			}
		}

		// Token: 0x06000AB8 RID: 2744 RVA: 0x0000F28F File Offset: 0x0000D48F
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_InviteId);
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_TargetUserId);
			Helper.Dispose(ref this.m_SessionId);
		}

		// Token: 0x06000AB9 RID: 2745 RVA: 0x0000F2CE File Offset: 0x0000D4CE
		public void Get(out SessionInviteRejectedCallbackInfo output)
		{
			output = default(SessionInviteRejectedCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x040004D4 RID: 1236
		private IntPtr m_ClientData;

		// Token: 0x040004D5 RID: 1237
		private IntPtr m_InviteId;

		// Token: 0x040004D6 RID: 1238
		private IntPtr m_LocalUserId;

		// Token: 0x040004D7 RID: 1239
		private IntPtr m_TargetUserId;

		// Token: 0x040004D8 RID: 1240
		private IntPtr m_SessionId;
	}
}
