using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Achievements
{
	// Token: 0x02000733 RID: 1843
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct CopyPlayerAchievementByIndexOptionsInternal : ISettable<CopyPlayerAchievementByIndexOptions>, IDisposable
	{
		// Token: 0x17000E56 RID: 3670
		// (set) Token: 0x06002FB7 RID: 12215 RVA: 0x00046DEE File Offset: 0x00044FEE
		public ProductUserId TargetUserId
		{
			set
			{
				Helper.Set(value, ref this.m_TargetUserId);
			}
		}

		// Token: 0x17000E57 RID: 3671
		// (set) Token: 0x06002FB8 RID: 12216 RVA: 0x00046DFE File Offset: 0x00044FFE
		public uint AchievementIndex
		{
			set
			{
				this.m_AchievementIndex = value;
			}
		}

		// Token: 0x17000E58 RID: 3672
		// (set) Token: 0x06002FB9 RID: 12217 RVA: 0x00046E08 File Offset: 0x00045008
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x06002FBA RID: 12218 RVA: 0x00046E18 File Offset: 0x00045018
		public void Set(ref CopyPlayerAchievementByIndexOptions other)
		{
			this.m_ApiVersion = 2;
			this.TargetUserId = other.TargetUserId;
			this.AchievementIndex = other.AchievementIndex;
			this.LocalUserId = other.LocalUserId;
		}

		// Token: 0x06002FBB RID: 12219 RVA: 0x00046E4C File Offset: 0x0004504C
		public void Set(ref CopyPlayerAchievementByIndexOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 2;
				this.TargetUserId = other.Value.TargetUserId;
				this.AchievementIndex = other.Value.AchievementIndex;
				this.LocalUserId = other.Value.LocalUserId;
			}
		}

		// Token: 0x06002FBC RID: 12220 RVA: 0x00046EAC File Offset: 0x000450AC
		public void Dispose()
		{
			Helper.Dispose(ref this.m_TargetUserId);
			Helper.Dispose(ref this.m_LocalUserId);
		}

		// Token: 0x04001558 RID: 5464
		private int m_ApiVersion;

		// Token: 0x04001559 RID: 5465
		private IntPtr m_TargetUserId;

		// Token: 0x0400155A RID: 5466
		private uint m_AchievementIndex;

		// Token: 0x0400155B RID: 5467
		private IntPtr m_LocalUserId;
	}
}
