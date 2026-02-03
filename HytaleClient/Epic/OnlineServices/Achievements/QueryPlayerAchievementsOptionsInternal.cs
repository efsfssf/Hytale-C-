using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Achievements
{
	// Token: 0x0200075D RID: 1885
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct QueryPlayerAchievementsOptionsInternal : ISettable<QueryPlayerAchievementsOptions>, IDisposable
	{
		// Token: 0x17000ED4 RID: 3796
		// (set) Token: 0x06003111 RID: 12561 RVA: 0x00048E29 File Offset: 0x00047029
		public ProductUserId TargetUserId
		{
			set
			{
				Helper.Set(value, ref this.m_TargetUserId);
			}
		}

		// Token: 0x17000ED5 RID: 3797
		// (set) Token: 0x06003112 RID: 12562 RVA: 0x00048E39 File Offset: 0x00047039
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x06003113 RID: 12563 RVA: 0x00048E49 File Offset: 0x00047049
		public void Set(ref QueryPlayerAchievementsOptions other)
		{
			this.m_ApiVersion = 2;
			this.TargetUserId = other.TargetUserId;
			this.LocalUserId = other.LocalUserId;
		}

		// Token: 0x06003114 RID: 12564 RVA: 0x00048E70 File Offset: 0x00047070
		public void Set(ref QueryPlayerAchievementsOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 2;
				this.TargetUserId = other.Value.TargetUserId;
				this.LocalUserId = other.Value.LocalUserId;
			}
		}

		// Token: 0x06003115 RID: 12565 RVA: 0x00048EBB File Offset: 0x000470BB
		public void Dispose()
		{
			Helper.Dispose(ref this.m_TargetUserId);
			Helper.Dispose(ref this.m_LocalUserId);
		}

		// Token: 0x040015E1 RID: 5601
		private int m_ApiVersion;

		// Token: 0x040015E2 RID: 5602
		private IntPtr m_TargetUserId;

		// Token: 0x040015E3 RID: 5603
		private IntPtr m_LocalUserId;
	}
}
