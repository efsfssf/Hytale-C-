using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Leaderboards
{
	// Token: 0x0200045C RID: 1116
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct CopyLeaderboardRecordByUserIdOptionsInternal : ISettable<CopyLeaderboardRecordByUserIdOptions>, IDisposable
	{
		// Token: 0x17000840 RID: 2112
		// (set) Token: 0x06001D4D RID: 7501 RVA: 0x0002ADAE File Offset: 0x00028FAE
		public ProductUserId UserId
		{
			set
			{
				Helper.Set(value, ref this.m_UserId);
			}
		}

		// Token: 0x06001D4E RID: 7502 RVA: 0x0002ADBE File Offset: 0x00028FBE
		public void Set(ref CopyLeaderboardRecordByUserIdOptions other)
		{
			this.m_ApiVersion = 2;
			this.UserId = other.UserId;
		}

		// Token: 0x06001D4F RID: 7503 RVA: 0x0002ADD8 File Offset: 0x00028FD8
		public void Set(ref CopyLeaderboardRecordByUserIdOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 2;
				this.UserId = other.Value.UserId;
			}
		}

		// Token: 0x06001D50 RID: 7504 RVA: 0x0002AE0E File Offset: 0x0002900E
		public void Dispose()
		{
			Helper.Dispose(ref this.m_UserId);
		}

		// Token: 0x04000CC3 RID: 3267
		private int m_ApiVersion;

		// Token: 0x04000CC4 RID: 3268
		private IntPtr m_UserId;
	}
}
