using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Achievements
{
	// Token: 0x02000731 RID: 1841
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct CopyPlayerAchievementByAchievementIdOptionsInternal : ISettable<CopyPlayerAchievementByAchievementIdOptions>, IDisposable
	{
		// Token: 0x17000E50 RID: 3664
		// (set) Token: 0x06002FAB RID: 12203 RVA: 0x00046CD0 File Offset: 0x00044ED0
		public ProductUserId TargetUserId
		{
			set
			{
				Helper.Set(value, ref this.m_TargetUserId);
			}
		}

		// Token: 0x17000E51 RID: 3665
		// (set) Token: 0x06002FAC RID: 12204 RVA: 0x00046CE0 File Offset: 0x00044EE0
		public Utf8String AchievementId
		{
			set
			{
				Helper.Set(value, ref this.m_AchievementId);
			}
		}

		// Token: 0x17000E52 RID: 3666
		// (set) Token: 0x06002FAD RID: 12205 RVA: 0x00046CF0 File Offset: 0x00044EF0
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x06002FAE RID: 12206 RVA: 0x00046D00 File Offset: 0x00044F00
		public void Set(ref CopyPlayerAchievementByAchievementIdOptions other)
		{
			this.m_ApiVersion = 2;
			this.TargetUserId = other.TargetUserId;
			this.AchievementId = other.AchievementId;
			this.LocalUserId = other.LocalUserId;
		}

		// Token: 0x06002FAF RID: 12207 RVA: 0x00046D34 File Offset: 0x00044F34
		public void Set(ref CopyPlayerAchievementByAchievementIdOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 2;
				this.TargetUserId = other.Value.TargetUserId;
				this.AchievementId = other.Value.AchievementId;
				this.LocalUserId = other.Value.LocalUserId;
			}
		}

		// Token: 0x06002FB0 RID: 12208 RVA: 0x00046D94 File Offset: 0x00044F94
		public void Dispose()
		{
			Helper.Dispose(ref this.m_TargetUserId);
			Helper.Dispose(ref this.m_AchievementId);
			Helper.Dispose(ref this.m_LocalUserId);
		}

		// Token: 0x04001551 RID: 5457
		private int m_ApiVersion;

		// Token: 0x04001552 RID: 5458
		private IntPtr m_TargetUserId;

		// Token: 0x04001553 RID: 5459
		private IntPtr m_AchievementId;

		// Token: 0x04001554 RID: 5460
		private IntPtr m_LocalUserId;
	}
}
