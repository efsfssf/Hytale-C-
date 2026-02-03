using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x02000440 RID: 1088
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct QueryInvitesCallbackInfoInternal : ICallbackInfoInternal, IGettable<QueryInvitesCallbackInfo>, ISettable<QueryInvitesCallbackInfo>, IDisposable
	{
		// Token: 0x170007F6 RID: 2038
		// (get) Token: 0x06001C91 RID: 7313 RVA: 0x00029B94 File Offset: 0x00027D94
		// (set) Token: 0x06001C92 RID: 7314 RVA: 0x00029BAC File Offset: 0x00027DAC
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

		// Token: 0x170007F7 RID: 2039
		// (get) Token: 0x06001C93 RID: 7315 RVA: 0x00029BB8 File Offset: 0x00027DB8
		// (set) Token: 0x06001C94 RID: 7316 RVA: 0x00029BD9 File Offset: 0x00027DD9
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

		// Token: 0x170007F8 RID: 2040
		// (get) Token: 0x06001C95 RID: 7317 RVA: 0x00029BEC File Offset: 0x00027DEC
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x170007F9 RID: 2041
		// (get) Token: 0x06001C96 RID: 7318 RVA: 0x00029C04 File Offset: 0x00027E04
		// (set) Token: 0x06001C97 RID: 7319 RVA: 0x00029C25 File Offset: 0x00027E25
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

		// Token: 0x06001C98 RID: 7320 RVA: 0x00029C35 File Offset: 0x00027E35
		public void Set(ref QueryInvitesCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
		}

		// Token: 0x06001C99 RID: 7321 RVA: 0x00029C60 File Offset: 0x00027E60
		public void Set(ref QueryInvitesCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ResultCode = other.Value.ResultCode;
				this.ClientData = other.Value.ClientData;
				this.LocalUserId = other.Value.LocalUserId;
			}
		}

		// Token: 0x06001C9A RID: 7322 RVA: 0x00029CB9 File Offset: 0x00027EB9
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LocalUserId);
		}

		// Token: 0x06001C9B RID: 7323 RVA: 0x00029CD4 File Offset: 0x00027ED4
		public void Get(out QueryInvitesCallbackInfo output)
		{
			output = default(QueryInvitesCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x04000C77 RID: 3191
		private Result m_ResultCode;

		// Token: 0x04000C78 RID: 3192
		private IntPtr m_ClientData;

		// Token: 0x04000C79 RID: 3193
		private IntPtr m_LocalUserId;
	}
}
