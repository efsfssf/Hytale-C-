using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x020003E1 RID: 993
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct LobbyModificationRemoveAttributeOptionsInternal : ISettable<LobbyModificationRemoveAttributeOptions>, IDisposable
	{
		// Token: 0x170007B3 RID: 1971
		// (set) Token: 0x06001B02 RID: 6914 RVA: 0x00028957 File Offset: 0x00026B57
		public Utf8String Key
		{
			set
			{
				Helper.Set(value, ref this.m_Key);
			}
		}

		// Token: 0x06001B03 RID: 6915 RVA: 0x00028967 File Offset: 0x00026B67
		public void Set(ref LobbyModificationRemoveAttributeOptions other)
		{
			this.m_ApiVersion = 1;
			this.Key = other.Key;
		}

		// Token: 0x06001B04 RID: 6916 RVA: 0x00028980 File Offset: 0x00026B80
		public void Set(ref LobbyModificationRemoveAttributeOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.Key = other.Value.Key;
			}
		}

		// Token: 0x06001B05 RID: 6917 RVA: 0x000289B6 File Offset: 0x00026BB6
		public void Dispose()
		{
			Helper.Dispose(ref this.m_Key);
		}

		// Token: 0x04000C15 RID: 3093
		private int m_ApiVersion;

		// Token: 0x04000C16 RID: 3094
		private IntPtr m_Key;
	}
}
