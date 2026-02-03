using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Achievements
{
	// Token: 0x02000739 RID: 1849
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct DefinitionInternal : IGettable<Definition>, ISettable<Definition>, IDisposable
	{
		// Token: 0x17000E6C RID: 3692
		// (get) Token: 0x06002FE6 RID: 12262 RVA: 0x000471B0 File Offset: 0x000453B0
		// (set) Token: 0x06002FE7 RID: 12263 RVA: 0x000471D1 File Offset: 0x000453D1
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

		// Token: 0x17000E6D RID: 3693
		// (get) Token: 0x06002FE8 RID: 12264 RVA: 0x000471E4 File Offset: 0x000453E4
		// (set) Token: 0x06002FE9 RID: 12265 RVA: 0x00047205 File Offset: 0x00045405
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

		// Token: 0x17000E6E RID: 3694
		// (get) Token: 0x06002FEA RID: 12266 RVA: 0x00047218 File Offset: 0x00045418
		// (set) Token: 0x06002FEB RID: 12267 RVA: 0x00047239 File Offset: 0x00045439
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

		// Token: 0x17000E6F RID: 3695
		// (get) Token: 0x06002FEC RID: 12268 RVA: 0x0004724C File Offset: 0x0004544C
		// (set) Token: 0x06002FED RID: 12269 RVA: 0x0004726D File Offset: 0x0004546D
		public Utf8String LockedDisplayName
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_LockedDisplayName, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_LockedDisplayName);
			}
		}

		// Token: 0x17000E70 RID: 3696
		// (get) Token: 0x06002FEE RID: 12270 RVA: 0x00047280 File Offset: 0x00045480
		// (set) Token: 0x06002FEF RID: 12271 RVA: 0x000472A1 File Offset: 0x000454A1
		public Utf8String LockedDescription
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_LockedDescription, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_LockedDescription);
			}
		}

		// Token: 0x17000E71 RID: 3697
		// (get) Token: 0x06002FF0 RID: 12272 RVA: 0x000472B4 File Offset: 0x000454B4
		// (set) Token: 0x06002FF1 RID: 12273 RVA: 0x000472D5 File Offset: 0x000454D5
		public Utf8String HiddenDescription
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_HiddenDescription, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_HiddenDescription);
			}
		}

		// Token: 0x17000E72 RID: 3698
		// (get) Token: 0x06002FF2 RID: 12274 RVA: 0x000472E8 File Offset: 0x000454E8
		// (set) Token: 0x06002FF3 RID: 12275 RVA: 0x00047309 File Offset: 0x00045509
		public Utf8String CompletionDescription
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_CompletionDescription, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_CompletionDescription);
			}
		}

		// Token: 0x17000E73 RID: 3699
		// (get) Token: 0x06002FF4 RID: 12276 RVA: 0x0004731C File Offset: 0x0004551C
		// (set) Token: 0x06002FF5 RID: 12277 RVA: 0x0004733D File Offset: 0x0004553D
		public Utf8String UnlockedIconId
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_UnlockedIconId, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_UnlockedIconId);
			}
		}

		// Token: 0x17000E74 RID: 3700
		// (get) Token: 0x06002FF6 RID: 12278 RVA: 0x00047350 File Offset: 0x00045550
		// (set) Token: 0x06002FF7 RID: 12279 RVA: 0x00047371 File Offset: 0x00045571
		public Utf8String LockedIconId
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_LockedIconId, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_LockedIconId);
			}
		}

		// Token: 0x17000E75 RID: 3701
		// (get) Token: 0x06002FF8 RID: 12280 RVA: 0x00047384 File Offset: 0x00045584
		// (set) Token: 0x06002FF9 RID: 12281 RVA: 0x000473A5 File Offset: 0x000455A5
		public bool IsHidden
		{
			get
			{
				bool result;
				Helper.Get(this.m_IsHidden, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_IsHidden);
			}
		}

		// Token: 0x17000E76 RID: 3702
		// (get) Token: 0x06002FFA RID: 12282 RVA: 0x000473B8 File Offset: 0x000455B8
		// (set) Token: 0x06002FFB RID: 12283 RVA: 0x000473DF File Offset: 0x000455DF
		public StatThresholds[] StatThresholds
		{
			get
			{
				StatThresholds[] result;
				Helper.Get<StatThresholdsInternal, StatThresholds>(this.m_StatThresholds, out result, this.m_StatThresholdsCount);
				return result;
			}
			set
			{
				Helper.Set<StatThresholds, StatThresholdsInternal>(ref value, ref this.m_StatThresholds, out this.m_StatThresholdsCount);
			}
		}

		// Token: 0x06002FFC RID: 12284 RVA: 0x000473F8 File Offset: 0x000455F8
		public void Set(ref Definition other)
		{
			this.m_ApiVersion = 1;
			this.AchievementId = other.AchievementId;
			this.DisplayName = other.DisplayName;
			this.Description = other.Description;
			this.LockedDisplayName = other.LockedDisplayName;
			this.LockedDescription = other.LockedDescription;
			this.HiddenDescription = other.HiddenDescription;
			this.CompletionDescription = other.CompletionDescription;
			this.UnlockedIconId = other.UnlockedIconId;
			this.LockedIconId = other.LockedIconId;
			this.IsHidden = other.IsHidden;
			this.StatThresholds = other.StatThresholds;
		}

		// Token: 0x06002FFD RID: 12285 RVA: 0x0004749C File Offset: 0x0004569C
		public void Set(ref Definition? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.AchievementId = other.Value.AchievementId;
				this.DisplayName = other.Value.DisplayName;
				this.Description = other.Value.Description;
				this.LockedDisplayName = other.Value.LockedDisplayName;
				this.LockedDescription = other.Value.LockedDescription;
				this.HiddenDescription = other.Value.HiddenDescription;
				this.CompletionDescription = other.Value.CompletionDescription;
				this.UnlockedIconId = other.Value.UnlockedIconId;
				this.LockedIconId = other.Value.LockedIconId;
				this.IsHidden = other.Value.IsHidden;
				this.StatThresholds = other.Value.StatThresholds;
			}
		}

		// Token: 0x06002FFE RID: 12286 RVA: 0x000475A8 File Offset: 0x000457A8
		public void Dispose()
		{
			Helper.Dispose(ref this.m_AchievementId);
			Helper.Dispose(ref this.m_DisplayName);
			Helper.Dispose(ref this.m_Description);
			Helper.Dispose(ref this.m_LockedDisplayName);
			Helper.Dispose(ref this.m_LockedDescription);
			Helper.Dispose(ref this.m_HiddenDescription);
			Helper.Dispose(ref this.m_CompletionDescription);
			Helper.Dispose(ref this.m_UnlockedIconId);
			Helper.Dispose(ref this.m_LockedIconId);
			Helper.Dispose(ref this.m_StatThresholds);
		}

		// Token: 0x06002FFF RID: 12287 RVA: 0x0004762E File Offset: 0x0004582E
		public void Get(out Definition output)
		{
			output = default(Definition);
			output.Set(ref this);
		}

		// Token: 0x04001571 RID: 5489
		private int m_ApiVersion;

		// Token: 0x04001572 RID: 5490
		private IntPtr m_AchievementId;

		// Token: 0x04001573 RID: 5491
		private IntPtr m_DisplayName;

		// Token: 0x04001574 RID: 5492
		private IntPtr m_Description;

		// Token: 0x04001575 RID: 5493
		private IntPtr m_LockedDisplayName;

		// Token: 0x04001576 RID: 5494
		private IntPtr m_LockedDescription;

		// Token: 0x04001577 RID: 5495
		private IntPtr m_HiddenDescription;

		// Token: 0x04001578 RID: 5496
		private IntPtr m_CompletionDescription;

		// Token: 0x04001579 RID: 5497
		private IntPtr m_UnlockedIconId;

		// Token: 0x0400157A RID: 5498
		private IntPtr m_LockedIconId;

		// Token: 0x0400157B RID: 5499
		private int m_IsHidden;

		// Token: 0x0400157C RID: 5500
		private int m_StatThresholdsCount;

		// Token: 0x0400157D RID: 5501
		private IntPtr m_StatThresholds;
	}
}
