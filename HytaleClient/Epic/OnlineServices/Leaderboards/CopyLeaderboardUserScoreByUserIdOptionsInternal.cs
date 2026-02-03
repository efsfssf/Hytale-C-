using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Leaderboards
{
	// Token: 0x02000460 RID: 1120
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct CopyLeaderboardUserScoreByUserIdOptionsInternal : ISettable<CopyLeaderboardUserScoreByUserIdOptions>, IDisposable
	{
		// Token: 0x17000847 RID: 2119
		// (set) Token: 0x06001D5E RID: 7518 RVA: 0x0002AEFC File Offset: 0x000290FC
		public ProductUserId UserId
		{
			set
			{
				Helper.Set(value, ref this.m_UserId);
			}
		}

		// Token: 0x17000848 RID: 2120
		// (set) Token: 0x06001D5F RID: 7519 RVA: 0x0002AF0C File Offset: 0x0002910C
		public Utf8String StatName
		{
			set
			{
				Helper.Set(value, ref this.m_StatName);
			}
		}

		// Token: 0x06001D60 RID: 7520 RVA: 0x0002AF1C File Offset: 0x0002911C
		public void Set(ref CopyLeaderboardUserScoreByUserIdOptions other)
		{
			this.m_ApiVersion = 1;
			this.UserId = other.UserId;
			this.StatName = other.StatName;
		}

		// Token: 0x06001D61 RID: 7521 RVA: 0x0002AF40 File Offset: 0x00029140
		public void Set(ref CopyLeaderboardUserScoreByUserIdOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.UserId = other.Value.UserId;
				this.StatName = other.Value.StatName;
			}
		}

		// Token: 0x06001D62 RID: 7522 RVA: 0x0002AF8B File Offset: 0x0002918B
		public void Dispose()
		{
			Helper.Dispose(ref this.m_UserId);
			Helper.Dispose(ref this.m_StatName);
		}

		// Token: 0x04000CCC RID: 3276
		private int m_ApiVersion;

		// Token: 0x04000CCD RID: 3277
		private IntPtr m_UserId;

		// Token: 0x04000CCE RID: 3278
		private IntPtr m_StatName;
	}
}
