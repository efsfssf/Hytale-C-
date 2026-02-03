using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x020003BC RID: 956
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct LobbyDetailsCopyMemberAttributeByIndexOptionsInternal : ISettable<LobbyDetailsCopyMemberAttributeByIndexOptions>, IDisposable
	{
		// Token: 0x1700074F RID: 1871
		// (set) Token: 0x060019B0 RID: 6576 RVA: 0x00025C0A File Offset: 0x00023E0A
		public ProductUserId TargetUserId
		{
			set
			{
				Helper.Set(value, ref this.m_TargetUserId);
			}
		}

		// Token: 0x17000750 RID: 1872
		// (set) Token: 0x060019B1 RID: 6577 RVA: 0x00025C1A File Offset: 0x00023E1A
		public uint AttrIndex
		{
			set
			{
				this.m_AttrIndex = value;
			}
		}

		// Token: 0x060019B2 RID: 6578 RVA: 0x00025C24 File Offset: 0x00023E24
		public void Set(ref LobbyDetailsCopyMemberAttributeByIndexOptions other)
		{
			this.m_ApiVersion = 1;
			this.TargetUserId = other.TargetUserId;
			this.AttrIndex = other.AttrIndex;
		}

		// Token: 0x060019B3 RID: 6579 RVA: 0x00025C48 File Offset: 0x00023E48
		public void Set(ref LobbyDetailsCopyMemberAttributeByIndexOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.TargetUserId = other.Value.TargetUserId;
				this.AttrIndex = other.Value.AttrIndex;
			}
		}

		// Token: 0x060019B4 RID: 6580 RVA: 0x00025C93 File Offset: 0x00023E93
		public void Dispose()
		{
			Helper.Dispose(ref this.m_TargetUserId);
		}

		// Token: 0x04000B66 RID: 2918
		private int m_ApiVersion;

		// Token: 0x04000B67 RID: 2919
		private IntPtr m_TargetUserId;

		// Token: 0x04000B68 RID: 2920
		private uint m_AttrIndex;
	}
}
