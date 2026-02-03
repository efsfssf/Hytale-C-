using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x02000139 RID: 313
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct QueryInvitesCallbackInfoInternal : ICallbackInfoInternal, IGettable<QueryInvitesCallbackInfo>, ISettable<QueryInvitesCallbackInfo>, IDisposable
	{
		// Token: 0x170001F9 RID: 505
		// (get) Token: 0x06000986 RID: 2438 RVA: 0x0000D2A8 File Offset: 0x0000B4A8
		// (set) Token: 0x06000987 RID: 2439 RVA: 0x0000D2C0 File Offset: 0x0000B4C0
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

		// Token: 0x170001FA RID: 506
		// (get) Token: 0x06000988 RID: 2440 RVA: 0x0000D2CC File Offset: 0x0000B4CC
		// (set) Token: 0x06000989 RID: 2441 RVA: 0x0000D2ED File Offset: 0x0000B4ED
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

		// Token: 0x170001FB RID: 507
		// (get) Token: 0x0600098A RID: 2442 RVA: 0x0000D300 File Offset: 0x0000B500
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x170001FC RID: 508
		// (get) Token: 0x0600098B RID: 2443 RVA: 0x0000D318 File Offset: 0x0000B518
		// (set) Token: 0x0600098C RID: 2444 RVA: 0x0000D339 File Offset: 0x0000B539
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

		// Token: 0x0600098D RID: 2445 RVA: 0x0000D349 File Offset: 0x0000B549
		public void Set(ref QueryInvitesCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
		}

		// Token: 0x0600098E RID: 2446 RVA: 0x0000D374 File Offset: 0x0000B574
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

		// Token: 0x0600098F RID: 2447 RVA: 0x0000D3CD File Offset: 0x0000B5CD
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LocalUserId);
		}

		// Token: 0x06000990 RID: 2448 RVA: 0x0000D3E8 File Offset: 0x0000B5E8
		public void Get(out QueryInvitesCallbackInfo output)
		{
			output = default(QueryInvitesCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x04000453 RID: 1107
		private Result m_ResultCode;

		// Token: 0x04000454 RID: 1108
		private IntPtr m_ClientData;

		// Token: 0x04000455 RID: 1109
		private IntPtr m_LocalUserId;
	}
}
