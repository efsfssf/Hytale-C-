using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x02000444 RID: 1092
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct RejectInviteCallbackInfoInternal : ICallbackInfoInternal, IGettable<RejectInviteCallbackInfo>, ISettable<RejectInviteCallbackInfo>, IDisposable
	{
		// Token: 0x170007FF RID: 2047
		// (get) Token: 0x06001CAA RID: 7338 RVA: 0x00029DE0 File Offset: 0x00027FE0
		// (set) Token: 0x06001CAB RID: 7339 RVA: 0x00029DF8 File Offset: 0x00027FF8
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

		// Token: 0x17000800 RID: 2048
		// (get) Token: 0x06001CAC RID: 7340 RVA: 0x00029E04 File Offset: 0x00028004
		// (set) Token: 0x06001CAD RID: 7341 RVA: 0x00029E25 File Offset: 0x00028025
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

		// Token: 0x17000801 RID: 2049
		// (get) Token: 0x06001CAE RID: 7342 RVA: 0x00029E38 File Offset: 0x00028038
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x17000802 RID: 2050
		// (get) Token: 0x06001CAF RID: 7343 RVA: 0x00029E50 File Offset: 0x00028050
		// (set) Token: 0x06001CB0 RID: 7344 RVA: 0x00029E71 File Offset: 0x00028071
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

		// Token: 0x06001CB1 RID: 7345 RVA: 0x00029E81 File Offset: 0x00028081
		public void Set(ref RejectInviteCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.InviteId = other.InviteId;
		}

		// Token: 0x06001CB2 RID: 7346 RVA: 0x00029EAC File Offset: 0x000280AC
		public void Set(ref RejectInviteCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ResultCode = other.Value.ResultCode;
				this.ClientData = other.Value.ClientData;
				this.InviteId = other.Value.InviteId;
			}
		}

		// Token: 0x06001CB3 RID: 7347 RVA: 0x00029F05 File Offset: 0x00028105
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_InviteId);
		}

		// Token: 0x06001CB4 RID: 7348 RVA: 0x00029F20 File Offset: 0x00028120
		public void Get(out RejectInviteCallbackInfo output)
		{
			output = default(RejectInviteCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x04000C80 RID: 3200
		private Result m_ResultCode;

		// Token: 0x04000C81 RID: 3201
		private IntPtr m_ClientData;

		// Token: 0x04000C82 RID: 3202
		private IntPtr m_InviteId;
	}
}
