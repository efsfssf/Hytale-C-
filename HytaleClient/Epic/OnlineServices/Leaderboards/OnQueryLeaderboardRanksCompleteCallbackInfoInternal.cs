using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Leaderboards
{
	// Token: 0x02000476 RID: 1142
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct OnQueryLeaderboardRanksCompleteCallbackInfoInternal : ICallbackInfoInternal, IGettable<OnQueryLeaderboardRanksCompleteCallbackInfo>, ISettable<OnQueryLeaderboardRanksCompleteCallbackInfo>, IDisposable
	{
		// Token: 0x17000869 RID: 2153
		// (get) Token: 0x06001DE2 RID: 7650 RVA: 0x0002BCC4 File Offset: 0x00029EC4
		// (set) Token: 0x06001DE3 RID: 7651 RVA: 0x0002BCDC File Offset: 0x00029EDC
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

		// Token: 0x1700086A RID: 2154
		// (get) Token: 0x06001DE4 RID: 7652 RVA: 0x0002BCE8 File Offset: 0x00029EE8
		// (set) Token: 0x06001DE5 RID: 7653 RVA: 0x0002BD09 File Offset: 0x00029F09
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

		// Token: 0x1700086B RID: 2155
		// (get) Token: 0x06001DE6 RID: 7654 RVA: 0x0002BD1C File Offset: 0x00029F1C
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x1700086C RID: 2156
		// (get) Token: 0x06001DE7 RID: 7655 RVA: 0x0002BD34 File Offset: 0x00029F34
		// (set) Token: 0x06001DE8 RID: 7656 RVA: 0x0002BD55 File Offset: 0x00029F55
		public Utf8String LeaderboardId
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_LeaderboardId, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_LeaderboardId);
			}
		}

		// Token: 0x06001DE9 RID: 7657 RVA: 0x0002BD65 File Offset: 0x00029F65
		public void Set(ref OnQueryLeaderboardRanksCompleteCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LeaderboardId = other.LeaderboardId;
		}

		// Token: 0x06001DEA RID: 7658 RVA: 0x0002BD90 File Offset: 0x00029F90
		public void Set(ref OnQueryLeaderboardRanksCompleteCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ResultCode = other.Value.ResultCode;
				this.ClientData = other.Value.ClientData;
				this.LeaderboardId = other.Value.LeaderboardId;
			}
		}

		// Token: 0x06001DEB RID: 7659 RVA: 0x0002BDE9 File Offset: 0x00029FE9
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LeaderboardId);
		}

		// Token: 0x06001DEC RID: 7660 RVA: 0x0002BE04 File Offset: 0x0002A004
		public void Get(out OnQueryLeaderboardRanksCompleteCallbackInfo output)
		{
			output = default(OnQueryLeaderboardRanksCompleteCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x04000D0A RID: 3338
		private Result m_ResultCode;

		// Token: 0x04000D0B RID: 3339
		private IntPtr m_ClientData;

		// Token: 0x04000D0C RID: 3340
		private IntPtr m_LeaderboardId;
	}
}
