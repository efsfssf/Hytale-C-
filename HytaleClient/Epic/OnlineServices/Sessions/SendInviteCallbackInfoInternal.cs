using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x02000145 RID: 325
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct SendInviteCallbackInfoInternal : ICallbackInfoInternal, IGettable<SendInviteCallbackInfo>, ISettable<SendInviteCallbackInfo>, IDisposable
	{
		// Token: 0x17000217 RID: 535
		// (get) Token: 0x060009D5 RID: 2517 RVA: 0x0000DA30 File Offset: 0x0000BC30
		// (set) Token: 0x060009D6 RID: 2518 RVA: 0x0000DA48 File Offset: 0x0000BC48
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

		// Token: 0x17000218 RID: 536
		// (get) Token: 0x060009D7 RID: 2519 RVA: 0x0000DA54 File Offset: 0x0000BC54
		// (set) Token: 0x060009D8 RID: 2520 RVA: 0x0000DA75 File Offset: 0x0000BC75
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

		// Token: 0x17000219 RID: 537
		// (get) Token: 0x060009D9 RID: 2521 RVA: 0x0000DA88 File Offset: 0x0000BC88
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x060009DA RID: 2522 RVA: 0x0000DAA0 File Offset: 0x0000BCA0
		public void Set(ref SendInviteCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
		}

		// Token: 0x060009DB RID: 2523 RVA: 0x0000DAC0 File Offset: 0x0000BCC0
		public void Set(ref SendInviteCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ResultCode = other.Value.ResultCode;
				this.ClientData = other.Value.ClientData;
			}
		}

		// Token: 0x060009DC RID: 2524 RVA: 0x0000DB04 File Offset: 0x0000BD04
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
		}

		// Token: 0x060009DD RID: 2525 RVA: 0x0000DB13 File Offset: 0x0000BD13
		public void Get(out SendInviteCallbackInfo output)
		{
			output = default(SendInviteCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x04000474 RID: 1140
		private Result m_ResultCode;

		// Token: 0x04000475 RID: 1141
		private IntPtr m_ClientData;
	}
}
