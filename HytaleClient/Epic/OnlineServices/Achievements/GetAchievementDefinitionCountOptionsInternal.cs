using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Achievements
{
	// Token: 0x0200073D RID: 1853
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct GetAchievementDefinitionCountOptionsInternal : ISettable<GetAchievementDefinitionCountOptions>, IDisposable
	{
		// Token: 0x0600302D RID: 12333 RVA: 0x00047BAC File Offset: 0x00045DAC
		public void Set(ref GetAchievementDefinitionCountOptions other)
		{
			this.m_ApiVersion = 1;
		}

		// Token: 0x0600302E RID: 12334 RVA: 0x00047BB8 File Offset: 0x00045DB8
		public void Set(ref GetAchievementDefinitionCountOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
			}
		}

		// Token: 0x0600302F RID: 12335 RVA: 0x00047BD9 File Offset: 0x00045DD9
		public void Dispose()
		{
		}

		// Token: 0x04001594 RID: 5524
		private int m_ApiVersion;
	}
}
