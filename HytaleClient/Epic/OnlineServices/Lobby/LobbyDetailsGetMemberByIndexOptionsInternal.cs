using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x020003C8 RID: 968
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct LobbyDetailsGetMemberByIndexOptionsInternal : ISettable<LobbyDetailsGetMemberByIndexOptions>, IDisposable
	{
		// Token: 0x1700075A RID: 1882
		// (set) Token: 0x060019D2 RID: 6610 RVA: 0x00025EDA File Offset: 0x000240DA
		public uint MemberIndex
		{
			set
			{
				this.m_MemberIndex = value;
			}
		}

		// Token: 0x060019D3 RID: 6611 RVA: 0x00025EE4 File Offset: 0x000240E4
		public void Set(ref LobbyDetailsGetMemberByIndexOptions other)
		{
			this.m_ApiVersion = 1;
			this.MemberIndex = other.MemberIndex;
		}

		// Token: 0x060019D4 RID: 6612 RVA: 0x00025EFC File Offset: 0x000240FC
		public void Set(ref LobbyDetailsGetMemberByIndexOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.MemberIndex = other.Value.MemberIndex;
			}
		}

		// Token: 0x060019D5 RID: 6613 RVA: 0x00025F32 File Offset: 0x00024132
		public void Dispose()
		{
		}

		// Token: 0x04000B77 RID: 2935
		private int m_ApiVersion;

		// Token: 0x04000B78 RID: 2936
		private uint m_MemberIndex;
	}
}
