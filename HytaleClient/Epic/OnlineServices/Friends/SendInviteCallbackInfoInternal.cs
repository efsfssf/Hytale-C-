using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Friends
{
	// Token: 0x020004FC RID: 1276
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct SendInviteCallbackInfoInternal : ICallbackInfoInternal, IGettable<SendInviteCallbackInfo>, ISettable<SendInviteCallbackInfo>, IDisposable
	{
		// Token: 0x17000969 RID: 2409
		// (get) Token: 0x0600211B RID: 8475 RVA: 0x000306C4 File Offset: 0x0002E8C4
		// (set) Token: 0x0600211C RID: 8476 RVA: 0x000306DC File Offset: 0x0002E8DC
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

		// Token: 0x1700096A RID: 2410
		// (get) Token: 0x0600211D RID: 8477 RVA: 0x000306E8 File Offset: 0x0002E8E8
		// (set) Token: 0x0600211E RID: 8478 RVA: 0x00030709 File Offset: 0x0002E909
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

		// Token: 0x1700096B RID: 2411
		// (get) Token: 0x0600211F RID: 8479 RVA: 0x0003071C File Offset: 0x0002E91C
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x1700096C RID: 2412
		// (get) Token: 0x06002120 RID: 8480 RVA: 0x00030734 File Offset: 0x0002E934
		// (set) Token: 0x06002121 RID: 8481 RVA: 0x00030755 File Offset: 0x0002E955
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

		// Token: 0x1700096D RID: 2413
		// (get) Token: 0x06002122 RID: 8482 RVA: 0x00030768 File Offset: 0x0002E968
		// (set) Token: 0x06002123 RID: 8483 RVA: 0x00030789 File Offset: 0x0002E989
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

		// Token: 0x06002124 RID: 8484 RVA: 0x00030799 File Offset: 0x0002E999
		public void Set(ref SendInviteCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.TargetUserId = other.TargetUserId;
		}

		// Token: 0x06002125 RID: 8485 RVA: 0x000307D0 File Offset: 0x0002E9D0
		public void Set(ref SendInviteCallbackInfo? other)
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

		// Token: 0x06002126 RID: 8486 RVA: 0x0003083E File Offset: 0x0002EA3E
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_TargetUserId);
		}

		// Token: 0x06002127 RID: 8487 RVA: 0x00030865 File Offset: 0x0002EA65
		public void Get(out SendInviteCallbackInfo output)
		{
			output = default(SendInviteCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x04000E55 RID: 3669
		private Result m_ResultCode;

		// Token: 0x04000E56 RID: 3670
		private IntPtr m_ClientData;

		// Token: 0x04000E57 RID: 3671
		private IntPtr m_LocalUserId;

		// Token: 0x04000E58 RID: 3672
		private IntPtr m_TargetUserId;
	}
}
