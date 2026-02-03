using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Achievements
{
	// Token: 0x02000727 RID: 1831
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct AddNotifyAchievementsUnlockedV2OptionsInternal : ISettable<AddNotifyAchievementsUnlockedV2Options>, IDisposable
	{
		// Token: 0x06002F8A RID: 12170 RVA: 0x00046A98 File Offset: 0x00044C98
		public void Set(ref AddNotifyAchievementsUnlockedV2Options other)
		{
			this.m_ApiVersion = 2;
		}

		// Token: 0x06002F8B RID: 12171 RVA: 0x00046AA4 File Offset: 0x00044CA4
		public void Set(ref AddNotifyAchievementsUnlockedV2Options? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 2;
			}
		}

		// Token: 0x06002F8C RID: 12172 RVA: 0x00046AC5 File Offset: 0x00044CC5
		public void Dispose()
		{
		}

		// Token: 0x04001541 RID: 5441
		private int m_ApiVersion;
	}
}
