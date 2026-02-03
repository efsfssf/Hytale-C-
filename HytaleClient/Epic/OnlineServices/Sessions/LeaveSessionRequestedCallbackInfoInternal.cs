using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x02000115 RID: 277
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct LeaveSessionRequestedCallbackInfoInternal : ICallbackInfoInternal, IGettable<LeaveSessionRequestedCallbackInfo>, ISettable<LeaveSessionRequestedCallbackInfo>, IDisposable
	{
		// Token: 0x170001F2 RID: 498
		// (get) Token: 0x060008F3 RID: 2291 RVA: 0x0000D0BC File Offset: 0x0000B2BC
		// (set) Token: 0x060008F4 RID: 2292 RVA: 0x0000D0DD File Offset: 0x0000B2DD
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

		// Token: 0x170001F3 RID: 499
		// (get) Token: 0x060008F5 RID: 2293 RVA: 0x0000D0F0 File Offset: 0x0000B2F0
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x170001F4 RID: 500
		// (get) Token: 0x060008F6 RID: 2294 RVA: 0x0000D108 File Offset: 0x0000B308
		// (set) Token: 0x060008F7 RID: 2295 RVA: 0x0000D129 File Offset: 0x0000B329
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

		// Token: 0x170001F5 RID: 501
		// (get) Token: 0x060008F8 RID: 2296 RVA: 0x0000D13C File Offset: 0x0000B33C
		// (set) Token: 0x060008F9 RID: 2297 RVA: 0x0000D15D File Offset: 0x0000B35D
		public Utf8String SessionName
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_SessionName, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_SessionName);
			}
		}

		// Token: 0x060008FA RID: 2298 RVA: 0x0000D16D File Offset: 0x0000B36D
		public void Set(ref LeaveSessionRequestedCallbackInfo other)
		{
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.SessionName = other.SessionName;
		}

		// Token: 0x060008FB RID: 2299 RVA: 0x0000D198 File Offset: 0x0000B398
		public void Set(ref LeaveSessionRequestedCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ClientData = other.Value.ClientData;
				this.LocalUserId = other.Value.LocalUserId;
				this.SessionName = other.Value.SessionName;
			}
		}

		// Token: 0x060008FC RID: 2300 RVA: 0x0000D1F1 File Offset: 0x0000B3F1
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_SessionName);
		}

		// Token: 0x060008FD RID: 2301 RVA: 0x0000D218 File Offset: 0x0000B418
		public void Get(out LeaveSessionRequestedCallbackInfo output)
		{
			output = default(LeaveSessionRequestedCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x04000440 RID: 1088
		private IntPtr m_ClientData;

		// Token: 0x04000441 RID: 1089
		private IntPtr m_LocalUserId;

		// Token: 0x04000442 RID: 1090
		private IntPtr m_SessionName;
	}
}
