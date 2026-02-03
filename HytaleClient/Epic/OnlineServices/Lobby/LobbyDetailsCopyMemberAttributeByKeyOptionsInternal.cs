using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x020003BE RID: 958
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct LobbyDetailsCopyMemberAttributeByKeyOptionsInternal : ISettable<LobbyDetailsCopyMemberAttributeByKeyOptions>, IDisposable
	{
		// Token: 0x17000753 RID: 1875
		// (set) Token: 0x060019B9 RID: 6585 RVA: 0x00025CC4 File Offset: 0x00023EC4
		public ProductUserId TargetUserId
		{
			set
			{
				Helper.Set(value, ref this.m_TargetUserId);
			}
		}

		// Token: 0x17000754 RID: 1876
		// (set) Token: 0x060019BA RID: 6586 RVA: 0x00025CD4 File Offset: 0x00023ED4
		public Utf8String AttrKey
		{
			set
			{
				Helper.Set(value, ref this.m_AttrKey);
			}
		}

		// Token: 0x060019BB RID: 6587 RVA: 0x00025CE4 File Offset: 0x00023EE4
		public void Set(ref LobbyDetailsCopyMemberAttributeByKeyOptions other)
		{
			this.m_ApiVersion = 1;
			this.TargetUserId = other.TargetUserId;
			this.AttrKey = other.AttrKey;
		}

		// Token: 0x060019BC RID: 6588 RVA: 0x00025D08 File Offset: 0x00023F08
		public void Set(ref LobbyDetailsCopyMemberAttributeByKeyOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.TargetUserId = other.Value.TargetUserId;
				this.AttrKey = other.Value.AttrKey;
			}
		}

		// Token: 0x060019BD RID: 6589 RVA: 0x00025D53 File Offset: 0x00023F53
		public void Dispose()
		{
			Helper.Dispose(ref this.m_TargetUserId);
			Helper.Dispose(ref this.m_AttrKey);
		}

		// Token: 0x04000B6B RID: 2923
		private int m_ApiVersion;

		// Token: 0x04000B6C RID: 2924
		private IntPtr m_TargetUserId;

		// Token: 0x04000B6D RID: 2925
		private IntPtr m_AttrKey;
	}
}
