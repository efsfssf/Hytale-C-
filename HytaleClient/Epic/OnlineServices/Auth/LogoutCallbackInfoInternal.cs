using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Auth
{
	// Token: 0x0200064A RID: 1610
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct LogoutCallbackInfoInternal : ICallbackInfoInternal, IGettable<LogoutCallbackInfo>, ISettable<LogoutCallbackInfo>, IDisposable
	{
		// Token: 0x17000C3A RID: 3130
		// (get) Token: 0x060029C7 RID: 10695 RVA: 0x0003DA4C File Offset: 0x0003BC4C
		// (set) Token: 0x060029C8 RID: 10696 RVA: 0x0003DA64 File Offset: 0x0003BC64
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

		// Token: 0x17000C3B RID: 3131
		// (get) Token: 0x060029C9 RID: 10697 RVA: 0x0003DA70 File Offset: 0x0003BC70
		// (set) Token: 0x060029CA RID: 10698 RVA: 0x0003DA91 File Offset: 0x0003BC91
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

		// Token: 0x17000C3C RID: 3132
		// (get) Token: 0x060029CB RID: 10699 RVA: 0x0003DAA4 File Offset: 0x0003BCA4
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x17000C3D RID: 3133
		// (get) Token: 0x060029CC RID: 10700 RVA: 0x0003DABC File Offset: 0x0003BCBC
		// (set) Token: 0x060029CD RID: 10701 RVA: 0x0003DADD File Offset: 0x0003BCDD
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

		// Token: 0x060029CE RID: 10702 RVA: 0x0003DAED File Offset: 0x0003BCED
		public void Set(ref LogoutCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
		}

		// Token: 0x060029CF RID: 10703 RVA: 0x0003DB18 File Offset: 0x0003BD18
		public void Set(ref LogoutCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ResultCode = other.Value.ResultCode;
				this.ClientData = other.Value.ClientData;
				this.LocalUserId = other.Value.LocalUserId;
			}
		}

		// Token: 0x060029D0 RID: 10704 RVA: 0x0003DB71 File Offset: 0x0003BD71
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LocalUserId);
		}

		// Token: 0x060029D1 RID: 10705 RVA: 0x0003DB8C File Offset: 0x0003BD8C
		public void Get(out LogoutCallbackInfo output)
		{
			output = default(LogoutCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x04001204 RID: 4612
		private Result m_ResultCode;

		// Token: 0x04001205 RID: 4613
		private IntPtr m_ClientData;

		// Token: 0x04001206 RID: 4614
		private IntPtr m_LocalUserId;
	}
}
