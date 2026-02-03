using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Achievements
{
	// Token: 0x02000763 RID: 1891
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct UnlockedAchievementInternal : IGettable<UnlockedAchievement>, ISettable<UnlockedAchievement>, IDisposable
	{
		// Token: 0x17000EE0 RID: 3808
		// (get) Token: 0x06003131 RID: 12593 RVA: 0x00049118 File Offset: 0x00047318
		// (set) Token: 0x06003132 RID: 12594 RVA: 0x00049139 File Offset: 0x00047339
		public Utf8String AchievementId
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_AchievementId, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_AchievementId);
			}
		}

		// Token: 0x17000EE1 RID: 3809
		// (get) Token: 0x06003133 RID: 12595 RVA: 0x0004914C File Offset: 0x0004734C
		// (set) Token: 0x06003134 RID: 12596 RVA: 0x0004916D File Offset: 0x0004736D
		public DateTimeOffset? UnlockTime
		{
			get
			{
				DateTimeOffset? result;
				Helper.Get(this.m_UnlockTime, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_UnlockTime);
			}
		}

		// Token: 0x06003135 RID: 12597 RVA: 0x0004917D File Offset: 0x0004737D
		public void Set(ref UnlockedAchievement other)
		{
			this.m_ApiVersion = 1;
			this.AchievementId = other.AchievementId;
			this.UnlockTime = other.UnlockTime;
		}

		// Token: 0x06003136 RID: 12598 RVA: 0x000491A4 File Offset: 0x000473A4
		public void Set(ref UnlockedAchievement? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.AchievementId = other.Value.AchievementId;
				this.UnlockTime = other.Value.UnlockTime;
			}
		}

		// Token: 0x06003137 RID: 12599 RVA: 0x000491EF File Offset: 0x000473EF
		public void Dispose()
		{
			Helper.Dispose(ref this.m_AchievementId);
		}

		// Token: 0x06003138 RID: 12600 RVA: 0x000491FE File Offset: 0x000473FE
		public void Get(out UnlockedAchievement output)
		{
			output = default(UnlockedAchievement);
			output.Set(ref this);
		}

		// Token: 0x040015F1 RID: 5617
		private int m_ApiVersion;

		// Token: 0x040015F2 RID: 5618
		private IntPtr m_AchievementId;

		// Token: 0x040015F3 RID: 5619
		private long m_UnlockTime;
	}
}
