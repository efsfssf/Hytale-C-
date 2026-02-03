using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Friends
{
	// Token: 0x020004F8 RID: 1272
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct RejectInviteCallbackInfoInternal : ICallbackInfoInternal, IGettable<RejectInviteCallbackInfo>, ISettable<RejectInviteCallbackInfo>, IDisposable
	{
		// Token: 0x1700095C RID: 2396
		// (get) Token: 0x060020FB RID: 8443 RVA: 0x000303A8 File Offset: 0x0002E5A8
		// (set) Token: 0x060020FC RID: 8444 RVA: 0x000303C0 File Offset: 0x0002E5C0
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

		// Token: 0x1700095D RID: 2397
		// (get) Token: 0x060020FD RID: 8445 RVA: 0x000303CC File Offset: 0x0002E5CC
		// (set) Token: 0x060020FE RID: 8446 RVA: 0x000303ED File Offset: 0x0002E5ED
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

		// Token: 0x1700095E RID: 2398
		// (get) Token: 0x060020FF RID: 8447 RVA: 0x00030400 File Offset: 0x0002E600
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x1700095F RID: 2399
		// (get) Token: 0x06002100 RID: 8448 RVA: 0x00030418 File Offset: 0x0002E618
		// (set) Token: 0x06002101 RID: 8449 RVA: 0x00030439 File Offset: 0x0002E639
		public EpicAccountId LocalUserId
		{
			get
			{
				EpicAccountId result;
				Helper.Get<EpicAccountId>(this.m_LocalUserId, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x17000960 RID: 2400
		// (get) Token: 0x06002102 RID: 8450 RVA: 0x0003044C File Offset: 0x0002E64C
		// (set) Token: 0x06002103 RID: 8451 RVA: 0x0003046D File Offset: 0x0002E66D
		public EpicAccountId TargetUserId
		{
			get
			{
				EpicAccountId result;
				Helper.Get<EpicAccountId>(this.m_TargetUserId, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_TargetUserId);
			}
		}

		// Token: 0x06002104 RID: 8452 RVA: 0x0003047D File Offset: 0x0002E67D
		public void Set(ref RejectInviteCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.TargetUserId = other.TargetUserId;
		}

		// Token: 0x06002105 RID: 8453 RVA: 0x000304B4 File Offset: 0x0002E6B4
		public void Set(ref RejectInviteCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ResultCode = other.Value.ResultCode;
				this.ClientData = other.Value.ClientData;
				this.LocalUserId = other.Value.LocalUserId;
				this.TargetUserId = other.Value.TargetUserId;
			}
		}

		// Token: 0x06002106 RID: 8454 RVA: 0x00030522 File Offset: 0x0002E722
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_TargetUserId);
		}

		// Token: 0x06002107 RID: 8455 RVA: 0x00030549 File Offset: 0x0002E749
		public void Get(out RejectInviteCallbackInfo output)
		{
			output = default(RejectInviteCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x04000E48 RID: 3656
		private Result m_ResultCode;

		// Token: 0x04000E49 RID: 3657
		private IntPtr m_ClientData;

		// Token: 0x04000E4A RID: 3658
		private IntPtr m_LocalUserId;

		// Token: 0x04000E4B RID: 3659
		private IntPtr m_TargetUserId;
	}
}
