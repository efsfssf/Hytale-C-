using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Leaderboards
{
	// Token: 0x02000472 RID: 1138
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct OnQueryLeaderboardDefinitionsCompleteCallbackInfoInternal : ICallbackInfoInternal, IGettable<OnQueryLeaderboardDefinitionsCompleteCallbackInfo>, ISettable<OnQueryLeaderboardDefinitionsCompleteCallbackInfo>, IDisposable
	{
		// Token: 0x17000863 RID: 2147
		// (get) Token: 0x06001DC9 RID: 7625 RVA: 0x0002BB54 File Offset: 0x00029D54
		// (set) Token: 0x06001DCA RID: 7626 RVA: 0x0002BB6C File Offset: 0x00029D6C
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

		// Token: 0x17000864 RID: 2148
		// (get) Token: 0x06001DCB RID: 7627 RVA: 0x0002BB78 File Offset: 0x00029D78
		// (set) Token: 0x06001DCC RID: 7628 RVA: 0x0002BB99 File Offset: 0x00029D99
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

		// Token: 0x17000865 RID: 2149
		// (get) Token: 0x06001DCD RID: 7629 RVA: 0x0002BBAC File Offset: 0x00029DAC
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x06001DCE RID: 7630 RVA: 0x0002BBC4 File Offset: 0x00029DC4
		public void Set(ref OnQueryLeaderboardDefinitionsCompleteCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
		}

		// Token: 0x06001DCF RID: 7631 RVA: 0x0002BBE4 File Offset: 0x00029DE4
		public void Set(ref OnQueryLeaderboardDefinitionsCompleteCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ResultCode = other.Value.ResultCode;
				this.ClientData = other.Value.ClientData;
			}
		}

		// Token: 0x06001DD0 RID: 7632 RVA: 0x0002BC28 File Offset: 0x00029E28
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
		}

		// Token: 0x06001DD1 RID: 7633 RVA: 0x0002BC37 File Offset: 0x00029E37
		public void Get(out OnQueryLeaderboardDefinitionsCompleteCallbackInfo output)
		{
			output = default(OnQueryLeaderboardDefinitionsCompleteCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x04000D05 RID: 3333
		private Result m_ResultCode;

		// Token: 0x04000D06 RID: 3334
		private IntPtr m_ClientData;
	}
}
