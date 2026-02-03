using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Achievements
{
	// Token: 0x02000749 RID: 1865
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct OnAchievementsUnlockedCallbackV2InfoInternal : ICallbackInfoInternal, IGettable<OnAchievementsUnlockedCallbackV2Info>, ISettable<OnAchievementsUnlockedCallbackV2Info>, IDisposable
	{
		// Token: 0x17000E9A RID: 3738
		// (get) Token: 0x06003069 RID: 12393 RVA: 0x00047F6C File Offset: 0x0004616C
		// (set) Token: 0x0600306A RID: 12394 RVA: 0x00047F8D File Offset: 0x0004618D
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

		// Token: 0x17000E9B RID: 3739
		// (get) Token: 0x0600306B RID: 12395 RVA: 0x00047FA0 File Offset: 0x000461A0
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x17000E9C RID: 3740
		// (get) Token: 0x0600306C RID: 12396 RVA: 0x00047FB8 File Offset: 0x000461B8
		// (set) Token: 0x0600306D RID: 12397 RVA: 0x00047FD9 File Offset: 0x000461D9
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

		// Token: 0x17000E9D RID: 3741
		// (get) Token: 0x0600306E RID: 12398 RVA: 0x00047FEC File Offset: 0x000461EC
		// (set) Token: 0x0600306F RID: 12399 RVA: 0x0004800D File Offset: 0x0004620D
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

		// Token: 0x17000E9E RID: 3742
		// (get) Token: 0x06003070 RID: 12400 RVA: 0x00048020 File Offset: 0x00046220
		// (set) Token: 0x06003071 RID: 12401 RVA: 0x00048041 File Offset: 0x00046241
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

		// Token: 0x06003072 RID: 12402 RVA: 0x00048051 File Offset: 0x00046251
		public void Set(ref OnAchievementsUnlockedCallbackV2Info other)
		{
			this.ClientData = other.ClientData;
			this.UserId = other.UserId;
			this.AchievementId = other.AchievementId;
			this.UnlockTime = other.UnlockTime;
		}

		// Token: 0x06003073 RID: 12403 RVA: 0x00048088 File Offset: 0x00046288
		public void Set(ref OnAchievementsUnlockedCallbackV2Info? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ClientData = other.Value.ClientData;
				this.UserId = other.Value.UserId;
				this.AchievementId = other.Value.AchievementId;
				this.UnlockTime = other.Value.UnlockTime;
			}
		}

		// Token: 0x06003074 RID: 12404 RVA: 0x000480F6 File Offset: 0x000462F6
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_UserId);
			Helper.Dispose(ref this.m_AchievementId);
		}

		// Token: 0x06003075 RID: 12405 RVA: 0x0004811D File Offset: 0x0004631D
		public void Get(out OnAchievementsUnlockedCallbackV2Info output)
		{
			output = default(OnAchievementsUnlockedCallbackV2Info);
			output.Set(ref this);
		}

		// Token: 0x040015A6 RID: 5542
		private IntPtr m_ClientData;

		// Token: 0x040015A7 RID: 5543
		private IntPtr m_UserId;

		// Token: 0x040015A8 RID: 5544
		private IntPtr m_AchievementId;

		// Token: 0x040015A9 RID: 5545
		private long m_UnlockTime;
	}
}
