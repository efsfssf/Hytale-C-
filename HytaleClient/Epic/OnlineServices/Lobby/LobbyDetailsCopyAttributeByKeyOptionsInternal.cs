using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x020003B8 RID: 952
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct LobbyDetailsCopyAttributeByKeyOptionsInternal : ISettable<LobbyDetailsCopyAttributeByKeyOptions>, IDisposable
	{
		// Token: 0x1700074C RID: 1868
		// (set) Token: 0x060019A5 RID: 6565 RVA: 0x00025B4A File Offset: 0x00023D4A
		public Utf8String AttrKey
		{
			set
			{
				Helper.Set(value, ref this.m_AttrKey);
			}
		}

		// Token: 0x060019A6 RID: 6566 RVA: 0x00025B5A File Offset: 0x00023D5A
		public void Set(ref LobbyDetailsCopyAttributeByKeyOptions other)
		{
			this.m_ApiVersion = 1;
			this.AttrKey = other.AttrKey;
		}

		// Token: 0x060019A7 RID: 6567 RVA: 0x00025B74 File Offset: 0x00023D74
		public void Set(ref LobbyDetailsCopyAttributeByKeyOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.AttrKey = other.Value.AttrKey;
			}
		}

		// Token: 0x060019A8 RID: 6568 RVA: 0x00025BAA File Offset: 0x00023DAA
		public void Dispose()
		{
			Helper.Dispose(ref this.m_AttrKey);
		}

		// Token: 0x04000B61 RID: 2913
		private int m_ApiVersion;

		// Token: 0x04000B62 RID: 2914
		private IntPtr m_AttrKey;
	}
}
