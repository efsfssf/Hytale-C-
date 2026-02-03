using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x02000177 RID: 375
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct SessionSearchFindCallbackInfoInternal : ICallbackInfoInternal, IGettable<SessionSearchFindCallbackInfo>, ISettable<SessionSearchFindCallbackInfo>, IDisposable
	{
		// Token: 0x17000286 RID: 646
		// (get) Token: 0x06000B17 RID: 2839 RVA: 0x0000FCCC File Offset: 0x0000DECC
		// (set) Token: 0x06000B18 RID: 2840 RVA: 0x0000FCE4 File Offset: 0x0000DEE4
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

		// Token: 0x17000287 RID: 647
		// (get) Token: 0x06000B19 RID: 2841 RVA: 0x0000FCF0 File Offset: 0x0000DEF0
		// (set) Token: 0x06000B1A RID: 2842 RVA: 0x0000FD11 File Offset: 0x0000DF11
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

		// Token: 0x17000288 RID: 648
		// (get) Token: 0x06000B1B RID: 2843 RVA: 0x0000FD24 File Offset: 0x0000DF24
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x06000B1C RID: 2844 RVA: 0x0000FD3C File Offset: 0x0000DF3C
		public void Set(ref SessionSearchFindCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
		}

		// Token: 0x06000B1D RID: 2845 RVA: 0x0000FD5C File Offset: 0x0000DF5C
		public void Set(ref SessionSearchFindCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ResultCode = other.Value.ResultCode;
				this.ClientData = other.Value.ClientData;
			}
		}

		// Token: 0x06000B1E RID: 2846 RVA: 0x0000FDA0 File Offset: 0x0000DFA0
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
		}

		// Token: 0x06000B1F RID: 2847 RVA: 0x0000FDAF File Offset: 0x0000DFAF
		public void Get(out SessionSearchFindCallbackInfo output)
		{
			output = default(SessionSearchFindCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x04000511 RID: 1297
		private Result m_ResultCode;

		// Token: 0x04000512 RID: 1298
		private IntPtr m_ClientData;
	}
}
