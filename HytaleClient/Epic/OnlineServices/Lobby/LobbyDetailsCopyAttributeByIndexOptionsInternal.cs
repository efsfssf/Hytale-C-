using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x020003B6 RID: 950
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct LobbyDetailsCopyAttributeByIndexOptionsInternal : ISettable<LobbyDetailsCopyAttributeByIndexOptions>, IDisposable
	{
		// Token: 0x1700074A RID: 1866
		// (set) Token: 0x0600199F RID: 6559 RVA: 0x00025ADE File Offset: 0x00023CDE
		public uint AttrIndex
		{
			set
			{
				this.m_AttrIndex = value;
			}
		}

		// Token: 0x060019A0 RID: 6560 RVA: 0x00025AE8 File Offset: 0x00023CE8
		public void Set(ref LobbyDetailsCopyAttributeByIndexOptions other)
		{
			this.m_ApiVersion = 1;
			this.AttrIndex = other.AttrIndex;
		}

		// Token: 0x060019A1 RID: 6561 RVA: 0x00025B00 File Offset: 0x00023D00
		public void Set(ref LobbyDetailsCopyAttributeByIndexOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.AttrIndex = other.Value.AttrIndex;
			}
		}

		// Token: 0x060019A2 RID: 6562 RVA: 0x00025B36 File Offset: 0x00023D36
		public void Dispose()
		{
		}

		// Token: 0x04000B5E RID: 2910
		private int m_ApiVersion;

		// Token: 0x04000B5F RID: 2911
		private uint m_AttrIndex;
	}
}
