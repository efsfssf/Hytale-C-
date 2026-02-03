using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Achievements
{
	// Token: 0x0200075B RID: 1883
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct QueryDefinitionsOptionsInternal : ISettable<QueryDefinitionsOptions>, IDisposable
	{
		// Token: 0x17000ECF RID: 3791
		// (set) Token: 0x06003107 RID: 12551 RVA: 0x00048D18 File Offset: 0x00046F18
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x17000ED0 RID: 3792
		// (set) Token: 0x06003108 RID: 12552 RVA: 0x00048D28 File Offset: 0x00046F28
		public EpicAccountId EpicUserId_DEPRECATED
		{
			set
			{
				Helper.Set(value, ref this.m_EpicUserId_DEPRECATED);
			}
		}

		// Token: 0x17000ED1 RID: 3793
		// (set) Token: 0x06003109 RID: 12553 RVA: 0x00048D38 File Offset: 0x00046F38
		public Utf8String[] HiddenAchievementIds_DEPRECATED
		{
			set
			{
				Helper.Set<Utf8String>(value, ref this.m_HiddenAchievementIds_DEPRECATED, true, out this.m_HiddenAchievementsCount_DEPRECATED);
			}
		}

		// Token: 0x0600310A RID: 12554 RVA: 0x00048D4F File Offset: 0x00046F4F
		public void Set(ref QueryDefinitionsOptions other)
		{
			this.m_ApiVersion = 3;
			this.LocalUserId = other.LocalUserId;
			this.EpicUserId_DEPRECATED = other.EpicUserId_DEPRECATED;
			this.HiddenAchievementIds_DEPRECATED = other.HiddenAchievementIds_DEPRECATED;
		}

		// Token: 0x0600310B RID: 12555 RVA: 0x00048D80 File Offset: 0x00046F80
		public void Set(ref QueryDefinitionsOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 3;
				this.LocalUserId = other.Value.LocalUserId;
				this.EpicUserId_DEPRECATED = other.Value.EpicUserId_DEPRECATED;
				this.HiddenAchievementIds_DEPRECATED = other.Value.HiddenAchievementIds_DEPRECATED;
			}
		}

		// Token: 0x0600310C RID: 12556 RVA: 0x00048DE0 File Offset: 0x00046FE0
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_EpicUserId_DEPRECATED);
			Helper.Dispose(ref this.m_HiddenAchievementIds_DEPRECATED);
		}

		// Token: 0x040015DA RID: 5594
		private int m_ApiVersion;

		// Token: 0x040015DB RID: 5595
		private IntPtr m_LocalUserId;

		// Token: 0x040015DC RID: 5596
		private IntPtr m_EpicUserId_DEPRECATED;

		// Token: 0x040015DD RID: 5597
		private IntPtr m_HiddenAchievementIds_DEPRECATED;

		// Token: 0x040015DE RID: 5598
		private uint m_HiddenAchievementsCount_DEPRECATED;
	}
}
