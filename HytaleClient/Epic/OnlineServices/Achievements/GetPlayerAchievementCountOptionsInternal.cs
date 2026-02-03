using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Achievements
{
	// Token: 0x0200073F RID: 1855
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct GetPlayerAchievementCountOptionsInternal : ISettable<GetPlayerAchievementCountOptions>, IDisposable
	{
		// Token: 0x17000E8C RID: 3724
		// (set) Token: 0x06003032 RID: 12338 RVA: 0x00047BED File Offset: 0x00045DED
		public ProductUserId UserId
		{
			set
			{
				Helper.Set(value, ref this.m_UserId);
			}
		}

		// Token: 0x06003033 RID: 12339 RVA: 0x00047BFD File Offset: 0x00045DFD
		public void Set(ref GetPlayerAchievementCountOptions other)
		{
			this.m_ApiVersion = 1;
			this.UserId = other.UserId;
		}

		// Token: 0x06003034 RID: 12340 RVA: 0x00047C14 File Offset: 0x00045E14
		public void Set(ref GetPlayerAchievementCountOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.UserId = other.Value.UserId;
			}
		}

		// Token: 0x06003035 RID: 12341 RVA: 0x00047C4A File Offset: 0x00045E4A
		public void Dispose()
		{
			Helper.Dispose(ref this.m_UserId);
		}

		// Token: 0x04001596 RID: 5526
		private int m_ApiVersion;

		// Token: 0x04001597 RID: 5527
		private IntPtr m_UserId;
	}
}
