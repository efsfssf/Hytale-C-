using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x02000141 RID: 321
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct RejectInviteCallbackInfoInternal : ICallbackInfoInternal, IGettable<RejectInviteCallbackInfo>, ISettable<RejectInviteCallbackInfo>, IDisposable
	{
		// Token: 0x1700020E RID: 526
		// (get) Token: 0x060009BD RID: 2493 RVA: 0x0000D810 File Offset: 0x0000BA10
		// (set) Token: 0x060009BE RID: 2494 RVA: 0x0000D828 File Offset: 0x0000BA28
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

		// Token: 0x1700020F RID: 527
		// (get) Token: 0x060009BF RID: 2495 RVA: 0x0000D834 File Offset: 0x0000BA34
		// (set) Token: 0x060009C0 RID: 2496 RVA: 0x0000D855 File Offset: 0x0000BA55
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

		// Token: 0x17000210 RID: 528
		// (get) Token: 0x060009C1 RID: 2497 RVA: 0x0000D868 File Offset: 0x0000BA68
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x060009C2 RID: 2498 RVA: 0x0000D880 File Offset: 0x0000BA80
		public void Set(ref RejectInviteCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
		}

		// Token: 0x060009C3 RID: 2499 RVA: 0x0000D8A0 File Offset: 0x0000BAA0
		public void Set(ref RejectInviteCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ResultCode = other.Value.ResultCode;
				this.ClientData = other.Value.ClientData;
			}
		}

		// Token: 0x060009C4 RID: 2500 RVA: 0x0000D8E4 File Offset: 0x0000BAE4
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
		}

		// Token: 0x060009C5 RID: 2501 RVA: 0x0000D8F3 File Offset: 0x0000BAF3
		public void Get(out RejectInviteCallbackInfo output)
		{
			output = default(RejectInviteCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x0400046B RID: 1131
		private Result m_ResultCode;

		// Token: 0x0400046C RID: 1132
		private IntPtr m_ClientData;
	}
}
