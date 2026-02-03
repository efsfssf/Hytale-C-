using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x02000151 RID: 337
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct SessionDetailsCopySessionAttributeByIndexOptionsInternal : ISettable<SessionDetailsCopySessionAttributeByIndexOptions>, IDisposable
	{
		// Token: 0x17000232 RID: 562
		// (set) Token: 0x06000A22 RID: 2594 RVA: 0x0000E295 File Offset: 0x0000C495
		public uint AttrIndex
		{
			set
			{
				this.m_AttrIndex = value;
			}
		}

		// Token: 0x06000A23 RID: 2595 RVA: 0x0000E29F File Offset: 0x0000C49F
		public void Set(ref SessionDetailsCopySessionAttributeByIndexOptions other)
		{
			this.m_ApiVersion = 1;
			this.AttrIndex = other.AttrIndex;
		}

		// Token: 0x06000A24 RID: 2596 RVA: 0x0000E2B8 File Offset: 0x0000C4B8
		public void Set(ref SessionDetailsCopySessionAttributeByIndexOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.AttrIndex = other.Value.AttrIndex;
			}
		}

		// Token: 0x06000A25 RID: 2597 RVA: 0x0000E2EE File Offset: 0x0000C4EE
		public void Dispose()
		{
		}

		// Token: 0x0400049A RID: 1178
		private int m_ApiVersion;

		// Token: 0x0400049B RID: 1179
		private uint m_AttrIndex;
	}
}
