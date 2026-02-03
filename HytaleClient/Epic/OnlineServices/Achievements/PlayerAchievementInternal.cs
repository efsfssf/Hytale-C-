using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Achievements
{
	// Token: 0x02000757 RID: 1879
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct PlayerAchievementInternal : IGettable<PlayerAchievement>, ISettable<PlayerAchievement>, IDisposable
	{
		// Token: 0x17000EBE RID: 3774
		// (get) Token: 0x060030DC RID: 12508 RVA: 0x00048808 File Offset: 0x00046A08
		// (set) Token: 0x060030DD RID: 12509 RVA: 0x00048829 File Offset: 0x00046A29
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

		// Token: 0x17000EBF RID: 3775
		// (get) Token: 0x060030DE RID: 12510 RVA: 0x0004883C File Offset: 0x00046A3C
		// (set) Token: 0x060030DF RID: 12511 RVA: 0x00048854 File Offset: 0x00046A54
		public double Progress
		{
			get
			{
				return this.m_Progress;
			}
			set
			{
				this.m_Progress = value;
			}
		}

		// Token: 0x17000EC0 RID: 3776
		// (get) Token: 0x060030E0 RID: 12512 RVA: 0x00048860 File Offset: 0x00046A60
		// (set) Token: 0x060030E1 RID: 12513 RVA: 0x00048881 File Offset: 0x00046A81
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

		// Token: 0x17000EC1 RID: 3777
		// (get) Token: 0x060030E2 RID: 12514 RVA: 0x00048894 File Offset: 0x00046A94
		// (set) Token: 0x060030E3 RID: 12515 RVA: 0x000488BB File Offset: 0x00046ABB
		public PlayerStatInfo[] StatInfo
		{
			get
			{
				PlayerStatInfo[] result;
				Helper.Get<PlayerStatInfoInternal, PlayerStatInfo>(this.m_StatInfo, out result, this.m_StatInfoCount);
				return result;
			}
			set
			{
				Helper.Set<PlayerStatInfo, PlayerStatInfoInternal>(ref value, ref this.m_StatInfo, out this.m_StatInfoCount);
			}
		}

		// Token: 0x17000EC2 RID: 3778
		// (get) Token: 0x060030E4 RID: 12516 RVA: 0x000488D4 File Offset: 0x00046AD4
		// (set) Token: 0x060030E5 RID: 12517 RVA: 0x000488F5 File Offset: 0x00046AF5
		public Utf8String DisplayName
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_DisplayName, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_DisplayName);
			}
		}

		// Token: 0x17000EC3 RID: 3779
		// (get) Token: 0x060030E6 RID: 12518 RVA: 0x00048908 File Offset: 0x00046B08
		// (set) Token: 0x060030E7 RID: 12519 RVA: 0x00048929 File Offset: 0x00046B29
		public Utf8String Description
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_Description, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_Description);
			}
		}

		// Token: 0x17000EC4 RID: 3780
		// (get) Token: 0x060030E8 RID: 12520 RVA: 0x0004893C File Offset: 0x00046B3C
		// (set) Token: 0x060030E9 RID: 12521 RVA: 0x0004895D File Offset: 0x00046B5D
		public Utf8String IconURL
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_IconURL, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_IconURL);
			}
		}

		// Token: 0x17000EC5 RID: 3781
		// (get) Token: 0x060030EA RID: 12522 RVA: 0x00048970 File Offset: 0x00046B70
		// (set) Token: 0x060030EB RID: 12523 RVA: 0x00048991 File Offset: 0x00046B91
		public Utf8String FlavorText
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_FlavorText, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_FlavorText);
			}
		}

		// Token: 0x060030EC RID: 12524 RVA: 0x000489A4 File Offset: 0x00046BA4
		public void Set(ref PlayerAchievement other)
		{
			this.m_ApiVersion = 2;
			this.AchievementId = other.AchievementId;
			this.Progress = other.Progress;
			this.UnlockTime = other.UnlockTime;
			this.StatInfo = other.StatInfo;
			this.DisplayName = other.DisplayName;
			this.Description = other.Description;
			this.IconURL = other.IconURL;
			this.FlavorText = other.FlavorText;
		}

		// Token: 0x060030ED RID: 12525 RVA: 0x00048A24 File Offset: 0x00046C24
		public void Set(ref PlayerAchievement? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 2;
				this.AchievementId = other.Value.AchievementId;
				this.Progress = other.Value.Progress;
				this.UnlockTime = other.Value.UnlockTime;
				this.StatInfo = other.Value.StatInfo;
				this.DisplayName = other.Value.DisplayName;
				this.Description = other.Value.Description;
				this.IconURL = other.Value.IconURL;
				this.FlavorText = other.Value.FlavorText;
			}
		}

		// Token: 0x060030EE RID: 12526 RVA: 0x00048AF0 File Offset: 0x00046CF0
		public void Dispose()
		{
			Helper.Dispose(ref this.m_AchievementId);
			Helper.Dispose(ref this.m_StatInfo);
			Helper.Dispose(ref this.m_DisplayName);
			Helper.Dispose(ref this.m_Description);
			Helper.Dispose(ref this.m_IconURL);
			Helper.Dispose(ref this.m_FlavorText);
		}

		// Token: 0x060030EF RID: 12527 RVA: 0x00048B46 File Offset: 0x00046D46
		public void Get(out PlayerAchievement output)
		{
			output = default(PlayerAchievement);
			output.Set(ref this);
		}

		// Token: 0x040015C6 RID: 5574
		private int m_ApiVersion;

		// Token: 0x040015C7 RID: 5575
		private IntPtr m_AchievementId;

		// Token: 0x040015C8 RID: 5576
		private double m_Progress;

		// Token: 0x040015C9 RID: 5577
		private long m_UnlockTime;

		// Token: 0x040015CA RID: 5578
		private int m_StatInfoCount;

		// Token: 0x040015CB RID: 5579
		private IntPtr m_StatInfo;

		// Token: 0x040015CC RID: 5580
		private IntPtr m_DisplayName;

		// Token: 0x040015CD RID: 5581
		private IntPtr m_Description;

		// Token: 0x040015CE RID: 5582
		private IntPtr m_IconURL;

		// Token: 0x040015CF RID: 5583
		private IntPtr m_FlavorText;
	}
}
