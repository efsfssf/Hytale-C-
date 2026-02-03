using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Achievements
{
	// Token: 0x02000745 RID: 1861
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct OnAchievementsUnlockedCallbackInfoInternal : ICallbackInfoInternal, IGettable<OnAchievementsUnlockedCallbackInfo>, ISettable<OnAchievementsUnlockedCallbackInfo>, IDisposable
	{
		// Token: 0x17000E92 RID: 3730
		// (get) Token: 0x0600304C RID: 12364 RVA: 0x00047D54 File Offset: 0x00045F54
		// (set) Token: 0x0600304D RID: 12365 RVA: 0x00047D75 File Offset: 0x00045F75
		public object ClientData
		{
			get
			{
				object result;
				Helper.Get(this.m_ClientData, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_ClientData);
			}
		}

		// Token: 0x17000E93 RID: 3731
		// (get) Token: 0x0600304E RID: 12366 RVA: 0x00047D88 File Offset: 0x00045F88
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x17000E94 RID: 3732
		// (get) Token: 0x0600304F RID: 12367 RVA: 0x00047DA0 File Offset: 0x00045FA0
		// (set) Token: 0x06003050 RID: 12368 RVA: 0x00047DC1 File Offset: 0x00045FC1
		public ProductUserId UserId
		{
			get
			{
				ProductUserId result;
				Helper.Get<ProductUserId>(this.m_UserId, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_UserId);
			}
		}

		// Token: 0x17000E95 RID: 3733
		// (get) Token: 0x06003051 RID: 12369 RVA: 0x00047DD4 File Offset: 0x00045FD4
		// (set) Token: 0x06003052 RID: 12370 RVA: 0x00047DFC File Offset: 0x00045FFC
		public Utf8String[] AchievementIds
		{
			get
			{
				Utf8String[] result;
				Helper.Get<Utf8String>(this.m_AchievementIds, out result, this.m_AchievementsCount, true);
				return result;
			}
			set
			{
				Helper.Set<Utf8String>(value, ref this.m_AchievementIds, true, out this.m_AchievementsCount);
			}
		}

		// Token: 0x06003053 RID: 12371 RVA: 0x00047E13 File Offset: 0x00046013
		public void Set(ref OnAchievementsUnlockedCallbackInfo other)
		{
			this.ClientData = other.ClientData;
			this.UserId = other.UserId;
			this.AchievementIds = other.AchievementIds;
		}

		// Token: 0x06003054 RID: 12372 RVA: 0x00047E40 File Offset: 0x00046040
		public void Set(ref OnAchievementsUnlockedCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ClientData = other.Value.ClientData;
				this.UserId = other.Value.UserId;
				this.AchievementIds = other.Value.AchievementIds;
			}
		}

		// Token: 0x06003055 RID: 12373 RVA: 0x00047E99 File Offset: 0x00046099
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_UserId);
			Helper.Dispose(ref this.m_AchievementIds);
		}

		// Token: 0x06003056 RID: 12374 RVA: 0x00047EC0 File Offset: 0x000460C0
		public void Get(out OnAchievementsUnlockedCallbackInfo output)
		{
			output = default(OnAchievementsUnlockedCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x0400159E RID: 5534
		private IntPtr m_ClientData;

		// Token: 0x0400159F RID: 5535
		private IntPtr m_UserId;

		// Token: 0x040015A0 RID: 5536
		private uint m_AchievementsCount;

		// Token: 0x040015A1 RID: 5537
		private IntPtr m_AchievementIds;
	}
}
