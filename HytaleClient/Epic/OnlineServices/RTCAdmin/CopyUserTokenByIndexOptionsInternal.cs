using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAdmin
{
	// Token: 0x02000285 RID: 645
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct CopyUserTokenByIndexOptionsInternal : ISettable<CopyUserTokenByIndexOptions>, IDisposable
	{
		// Token: 0x170004C5 RID: 1221
		// (set) Token: 0x06001223 RID: 4643 RVA: 0x0001A7D1 File Offset: 0x000189D1
		public uint UserTokenIndex
		{
			set
			{
				this.m_UserTokenIndex = value;
			}
		}

		// Token: 0x170004C6 RID: 1222
		// (set) Token: 0x06001224 RID: 4644 RVA: 0x0001A7DB File Offset: 0x000189DB
		public uint QueryId
		{
			set
			{
				this.m_QueryId = value;
			}
		}

		// Token: 0x06001225 RID: 4645 RVA: 0x0001A7E5 File Offset: 0x000189E5
		public void Set(ref CopyUserTokenByIndexOptions other)
		{
			this.m_ApiVersion = 2;
			this.UserTokenIndex = other.UserTokenIndex;
			this.QueryId = other.QueryId;
		}

		// Token: 0x06001226 RID: 4646 RVA: 0x0001A80C File Offset: 0x00018A0C
		public void Set(ref CopyUserTokenByIndexOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 2;
				this.UserTokenIndex = other.Value.UserTokenIndex;
				this.QueryId = other.Value.QueryId;
			}
		}

		// Token: 0x06001227 RID: 4647 RVA: 0x0001A857 File Offset: 0x00018A57
		public void Dispose()
		{
		}

		// Token: 0x040007FA RID: 2042
		private int m_ApiVersion;

		// Token: 0x040007FB RID: 2043
		private uint m_UserTokenIndex;

		// Token: 0x040007FC RID: 2044
		private uint m_QueryId;
	}
}
