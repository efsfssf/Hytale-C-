using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Achievements
{
	// Token: 0x02000741 RID: 1857
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct GetUnlockedAchievementCountOptionsInternal : ISettable<GetUnlockedAchievementCountOptions>, IDisposable
	{
		// Token: 0x17000E8E RID: 3726
		// (set) Token: 0x06003038 RID: 12344 RVA: 0x00047C6A File Offset: 0x00045E6A
		public ProductUserId UserId
		{
			set
			{
				Helper.Set(value, ref this.m_UserId);
			}
		}

		// Token: 0x06003039 RID: 12345 RVA: 0x00047C7A File Offset: 0x00045E7A
		public void Set(ref GetUnlockedAchievementCountOptions other)
		{
			this.m_ApiVersion = 1;
			this.UserId = other.UserId;
		}

		// Token: 0x0600303A RID: 12346 RVA: 0x00047C94 File Offset: 0x00045E94
		public void Set(ref GetUnlockedAchievementCountOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.UserId = other.Value.UserId;
			}
		}

		// Token: 0x0600303B RID: 12347 RVA: 0x00047CCA File Offset: 0x00045ECA
		public void Dispose()
		{
			Helper.Dispose(ref this.m_UserId);
		}

		// Token: 0x04001599 RID: 5529
		private int m_ApiVersion;

		// Token: 0x0400159A RID: 5530
		private IntPtr m_UserId;
	}
}
