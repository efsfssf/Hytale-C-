using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Reports
{
	// Token: 0x020002A2 RID: 674
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct SendPlayerBehaviorReportCompleteCallbackInfoInternal : ICallbackInfoInternal, IGettable<SendPlayerBehaviorReportCompleteCallbackInfo>, ISettable<SendPlayerBehaviorReportCompleteCallbackInfo>, IDisposable
	{
		// Token: 0x170004FA RID: 1274
		// (get) Token: 0x060012D3 RID: 4819 RVA: 0x0001B6E4 File Offset: 0x000198E4
		// (set) Token: 0x060012D4 RID: 4820 RVA: 0x0001B6FC File Offset: 0x000198FC
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

		// Token: 0x170004FB RID: 1275
		// (get) Token: 0x060012D5 RID: 4821 RVA: 0x0001B708 File Offset: 0x00019908
		// (set) Token: 0x060012D6 RID: 4822 RVA: 0x0001B729 File Offset: 0x00019929
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

		// Token: 0x170004FC RID: 1276
		// (get) Token: 0x060012D7 RID: 4823 RVA: 0x0001B73C File Offset: 0x0001993C
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x060012D8 RID: 4824 RVA: 0x0001B754 File Offset: 0x00019954
		public void Set(ref SendPlayerBehaviorReportCompleteCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
		}

		// Token: 0x060012D9 RID: 4825 RVA: 0x0001B774 File Offset: 0x00019974
		public void Set(ref SendPlayerBehaviorReportCompleteCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ResultCode = other.Value.ResultCode;
				this.ClientData = other.Value.ClientData;
			}
		}

		// Token: 0x060012DA RID: 4826 RVA: 0x0001B7B8 File Offset: 0x000199B8
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
		}

		// Token: 0x060012DB RID: 4827 RVA: 0x0001B7C7 File Offset: 0x000199C7
		public void Get(out SendPlayerBehaviorReportCompleteCallbackInfo output)
		{
			output = default(SendPlayerBehaviorReportCompleteCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x04000845 RID: 2117
		private Result m_ResultCode;

		// Token: 0x04000846 RID: 2118
		private IntPtr m_ClientData;
	}
}
