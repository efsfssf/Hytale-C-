using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x0200038E RID: 910
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct GetInviteIdByIndexOptionsInternal : ISettable<GetInviteIdByIndexOptions>, IDisposable
	{
		// Token: 0x170006CA RID: 1738
		// (set) Token: 0x06001866 RID: 6246 RVA: 0x00023B07 File Offset: 0x00021D07
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x170006CB RID: 1739
		// (set) Token: 0x06001867 RID: 6247 RVA: 0x00023B17 File Offset: 0x00021D17
		public uint Index
		{
			set
			{
				this.m_Index = value;
			}
		}

		// Token: 0x06001868 RID: 6248 RVA: 0x00023B21 File Offset: 0x00021D21
		public void Set(ref GetInviteIdByIndexOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
			this.Index = other.Index;
		}

		// Token: 0x06001869 RID: 6249 RVA: 0x00023B48 File Offset: 0x00021D48
		public void Set(ref GetInviteIdByIndexOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
				this.Index = other.Value.Index;
			}
		}

		// Token: 0x0600186A RID: 6250 RVA: 0x00023B93 File Offset: 0x00021D93
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
		}

		// Token: 0x04000ACD RID: 2765
		private int m_ApiVersion;

		// Token: 0x04000ACE RID: 2766
		private IntPtr m_LocalUserId;

		// Token: 0x04000ACF RID: 2767
		private uint m_Index;
	}
}
