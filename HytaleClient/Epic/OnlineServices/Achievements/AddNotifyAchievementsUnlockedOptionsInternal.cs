using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Achievements
{
	// Token: 0x02000725 RID: 1829
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct AddNotifyAchievementsUnlockedOptionsInternal : ISettable<AddNotifyAchievementsUnlockedOptions>, IDisposable
	{
		// Token: 0x06002F87 RID: 12167 RVA: 0x00046A67 File Offset: 0x00044C67
		public void Set(ref AddNotifyAchievementsUnlockedOptions other)
		{
			this.m_ApiVersion = 1;
		}

		// Token: 0x06002F88 RID: 12168 RVA: 0x00046A74 File Offset: 0x00044C74
		public void Set(ref AddNotifyAchievementsUnlockedOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
			}
		}

		// Token: 0x06002F89 RID: 12169 RVA: 0x00046A95 File Offset: 0x00044C95
		public void Dispose()
		{
		}

		// Token: 0x04001540 RID: 5440
		private int m_ApiVersion;
	}
}
