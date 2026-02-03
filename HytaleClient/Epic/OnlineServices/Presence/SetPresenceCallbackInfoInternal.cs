using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Presence
{
	// Token: 0x020002E5 RID: 741
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct SetPresenceCallbackInfoInternal : ICallbackInfoInternal, IGettable<SetPresenceCallbackInfo>, ISettable<SetPresenceCallbackInfo>, IDisposable
	{
		// Token: 0x17000575 RID: 1397
		// (get) Token: 0x0600145C RID: 5212 RVA: 0x0001DBF8 File Offset: 0x0001BDF8
		// (set) Token: 0x0600145D RID: 5213 RVA: 0x0001DC10 File Offset: 0x0001BE10
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

		// Token: 0x17000576 RID: 1398
		// (get) Token: 0x0600145E RID: 5214 RVA: 0x0001DC1C File Offset: 0x0001BE1C
		// (set) Token: 0x0600145F RID: 5215 RVA: 0x0001DC3D File Offset: 0x0001BE3D
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

		// Token: 0x17000577 RID: 1399
		// (get) Token: 0x06001460 RID: 5216 RVA: 0x0001DC50 File Offset: 0x0001BE50
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x17000578 RID: 1400
		// (get) Token: 0x06001461 RID: 5217 RVA: 0x0001DC68 File Offset: 0x0001BE68
		// (set) Token: 0x06001462 RID: 5218 RVA: 0x0001DC89 File Offset: 0x0001BE89
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

		// Token: 0x06001463 RID: 5219 RVA: 0x0001DC99 File Offset: 0x0001BE99
		public void Set(ref SetPresenceCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
		}

		// Token: 0x06001464 RID: 5220 RVA: 0x0001DCC4 File Offset: 0x0001BEC4
		public void Set(ref SetPresenceCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ResultCode = other.Value.ResultCode;
				this.ClientData = other.Value.ClientData;
				this.LocalUserId = other.Value.LocalUserId;
			}
		}

		// Token: 0x06001465 RID: 5221 RVA: 0x0001DD1D File Offset: 0x0001BF1D
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LocalUserId);
		}

		// Token: 0x06001466 RID: 5222 RVA: 0x0001DD38 File Offset: 0x0001BF38
		public void Get(out SetPresenceCallbackInfo output)
		{
			output = default(SetPresenceCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x040008F2 RID: 2290
		private Result m_ResultCode;

		// Token: 0x040008F3 RID: 2291
		private IntPtr m_ClientData;

		// Token: 0x040008F4 RID: 2292
		private IntPtr m_LocalUserId;
	}
}
