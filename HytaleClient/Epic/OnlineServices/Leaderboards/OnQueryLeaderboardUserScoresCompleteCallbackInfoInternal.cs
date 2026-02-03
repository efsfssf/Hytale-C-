using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Leaderboards
{
	// Token: 0x0200047A RID: 1146
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct OnQueryLeaderboardUserScoresCompleteCallbackInfoInternal : ICallbackInfoInternal, IGettable<OnQueryLeaderboardUserScoresCompleteCallbackInfo>, ISettable<OnQueryLeaderboardUserScoresCompleteCallbackInfo>, IDisposable
	{
		// Token: 0x1700086F RID: 2159
		// (get) Token: 0x06001DFB RID: 7675 RVA: 0x0002BE74 File Offset: 0x0002A074
		// (set) Token: 0x06001DFC RID: 7676 RVA: 0x0002BE8C File Offset: 0x0002A08C
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

		// Token: 0x17000870 RID: 2160
		// (get) Token: 0x06001DFD RID: 7677 RVA: 0x0002BE98 File Offset: 0x0002A098
		// (set) Token: 0x06001DFE RID: 7678 RVA: 0x0002BEB9 File Offset: 0x0002A0B9
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

		// Token: 0x17000871 RID: 2161
		// (get) Token: 0x06001DFF RID: 7679 RVA: 0x0002BECC File Offset: 0x0002A0CC
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x06001E00 RID: 7680 RVA: 0x0002BEE4 File Offset: 0x0002A0E4
		public void Set(ref OnQueryLeaderboardUserScoresCompleteCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
		}

		// Token: 0x06001E01 RID: 7681 RVA: 0x0002BF04 File Offset: 0x0002A104
		public void Set(ref OnQueryLeaderboardUserScoresCompleteCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ResultCode = other.Value.ResultCode;
				this.ClientData = other.Value.ClientData;
			}
		}

		// Token: 0x06001E02 RID: 7682 RVA: 0x0002BF48 File Offset: 0x0002A148
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
		}

		// Token: 0x06001E03 RID: 7683 RVA: 0x0002BF57 File Offset: 0x0002A157
		public void Get(out OnQueryLeaderboardUserScoresCompleteCallbackInfo output)
		{
			output = default(OnQueryLeaderboardUserScoresCompleteCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x04000D0F RID: 3343
		private Result m_ResultCode;

		// Token: 0x04000D10 RID: 3344
		private IntPtr m_ClientData;
	}
}
