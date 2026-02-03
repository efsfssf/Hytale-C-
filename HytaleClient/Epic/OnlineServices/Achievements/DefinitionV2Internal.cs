using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Achievements
{
	// Token: 0x0200073B RID: 1851
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct DefinitionV2Internal : IGettable<DefinitionV2>, ISettable<DefinitionV2>, IDisposable
	{
		// Token: 0x17000E81 RID: 3713
		// (get) Token: 0x06003015 RID: 12309 RVA: 0x0004777C File Offset: 0x0004597C
		// (set) Token: 0x06003016 RID: 12310 RVA: 0x0004779D File Offset: 0x0004599D
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

		// Token: 0x17000E82 RID: 3714
		// (get) Token: 0x06003017 RID: 12311 RVA: 0x000477B0 File Offset: 0x000459B0
		// (set) Token: 0x06003018 RID: 12312 RVA: 0x000477D1 File Offset: 0x000459D1
		public Utf8String UnlockedDisplayName
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_UnlockedDisplayName, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_UnlockedDisplayName);
			}
		}

		// Token: 0x17000E83 RID: 3715
		// (get) Token: 0x06003019 RID: 12313 RVA: 0x000477E4 File Offset: 0x000459E4
		// (set) Token: 0x0600301A RID: 12314 RVA: 0x00047805 File Offset: 0x00045A05
		public Utf8String UnlockedDescription
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_UnlockedDescription, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_UnlockedDescription);
			}
		}

		// Token: 0x17000E84 RID: 3716
		// (get) Token: 0x0600301B RID: 12315 RVA: 0x00047818 File Offset: 0x00045A18
		// (set) Token: 0x0600301C RID: 12316 RVA: 0x00047839 File Offset: 0x00045A39
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

		// Token: 0x17000E85 RID: 3717
		// (get) Token: 0x0600301D RID: 12317 RVA: 0x0004784C File Offset: 0x00045A4C
		// (set) Token: 0x0600301E RID: 12318 RVA: 0x0004786D File Offset: 0x00045A6D
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

		// Token: 0x17000E86 RID: 3718
		// (get) Token: 0x0600301F RID: 12319 RVA: 0x00047880 File Offset: 0x00045A80
		// (set) Token: 0x06003020 RID: 12320 RVA: 0x000478A1 File Offset: 0x00045AA1
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

		// Token: 0x17000E87 RID: 3719
		// (get) Token: 0x06003021 RID: 12321 RVA: 0x000478B4 File Offset: 0x00045AB4
		// (set) Token: 0x06003022 RID: 12322 RVA: 0x000478D5 File Offset: 0x00045AD5
		public Utf8String UnlockedIconURL
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_UnlockedIconURL, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_UnlockedIconURL);
			}
		}

		// Token: 0x17000E88 RID: 3720
		// (get) Token: 0x06003023 RID: 12323 RVA: 0x000478E8 File Offset: 0x00045AE8
		// (set) Token: 0x06003024 RID: 12324 RVA: 0x00047909 File Offset: 0x00045B09
		public Utf8String LockedIconURL
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_LockedIconURL, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_LockedIconURL);
			}
		}

		// Token: 0x17000E89 RID: 3721
		// (get) Token: 0x06003025 RID: 12325 RVA: 0x0004791C File Offset: 0x00045B1C
		// (set) Token: 0x06003026 RID: 12326 RVA: 0x0004793D File Offset: 0x00045B3D
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

		// Token: 0x17000E8A RID: 3722
		// (get) Token: 0x06003027 RID: 12327 RVA: 0x00047950 File Offset: 0x00045B50
		// (set) Token: 0x06003028 RID: 12328 RVA: 0x00047977 File Offset: 0x00045B77
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

		// Token: 0x06003029 RID: 12329 RVA: 0x00047990 File Offset: 0x00045B90
		public void Set(ref DefinitionV2 other)
		{
			this.m_ApiVersion = 2;
			this.AchievementId = other.AchievementId;
			this.UnlockedDisplayName = other.UnlockedDisplayName;
			this.UnlockedDescription = other.UnlockedDescription;
			this.LockedDisplayName = other.LockedDisplayName;
			this.LockedDescription = other.LockedDescription;
			this.FlavorText = other.FlavorText;
			this.UnlockedIconURL = other.UnlockedIconURL;
			this.LockedIconURL = other.LockedIconURL;
			this.IsHidden = other.IsHidden;
			this.StatThresholds = other.StatThresholds;
		}

		// Token: 0x0600302A RID: 12330 RVA: 0x00047A28 File Offset: 0x00045C28
		public void Set(ref DefinitionV2? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 2;
				this.AchievementId = other.Value.AchievementId;
				this.UnlockedDisplayName = other.Value.UnlockedDisplayName;
				this.UnlockedDescription = other.Value.UnlockedDescription;
				this.LockedDisplayName = other.Value.LockedDisplayName;
				this.LockedDescription = other.Value.LockedDescription;
				this.FlavorText = other.Value.FlavorText;
				this.UnlockedIconURL = other.Value.UnlockedIconURL;
				this.LockedIconURL = other.Value.LockedIconURL;
				this.IsHidden = other.Value.IsHidden;
				this.StatThresholds = other.Value.StatThresholds;
			}
		}

		// Token: 0x0600302B RID: 12331 RVA: 0x00047B20 File Offset: 0x00045D20
		public void Dispose()
		{
			Helper.Dispose(ref this.m_AchievementId);
			Helper.Dispose(ref this.m_UnlockedDisplayName);
			Helper.Dispose(ref this.m_UnlockedDescription);
			Helper.Dispose(ref this.m_LockedDisplayName);
			Helper.Dispose(ref this.m_LockedDescription);
			Helper.Dispose(ref this.m_FlavorText);
			Helper.Dispose(ref this.m_UnlockedIconURL);
			Helper.Dispose(ref this.m_LockedIconURL);
			Helper.Dispose(ref this.m_StatThresholds);
		}

		// Token: 0x0600302C RID: 12332 RVA: 0x00047B9A File Offset: 0x00045D9A
		public void Get(out DefinitionV2 output)
		{
			output = default(DefinitionV2);
			output.Set(ref this);
		}

		// Token: 0x04001588 RID: 5512
		private int m_ApiVersion;

		// Token: 0x04001589 RID: 5513
		private IntPtr m_AchievementId;

		// Token: 0x0400158A RID: 5514
		private IntPtr m_UnlockedDisplayName;

		// Token: 0x0400158B RID: 5515
		private IntPtr m_UnlockedDescription;

		// Token: 0x0400158C RID: 5516
		private IntPtr m_LockedDisplayName;

		// Token: 0x0400158D RID: 5517
		private IntPtr m_LockedDescription;

		// Token: 0x0400158E RID: 5518
		private IntPtr m_FlavorText;

		// Token: 0x0400158F RID: 5519
		private IntPtr m_UnlockedIconURL;

		// Token: 0x04001590 RID: 5520
		private IntPtr m_LockedIconURL;

		// Token: 0x04001591 RID: 5521
		private int m_IsHidden;

		// Token: 0x04001592 RID: 5522
		private uint m_StatThresholdsCount;

		// Token: 0x04001593 RID: 5523
		private IntPtr m_StatThresholds;
	}
}
