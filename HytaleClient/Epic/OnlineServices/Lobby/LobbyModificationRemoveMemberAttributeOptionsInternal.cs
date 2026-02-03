using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x020003E3 RID: 995
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct LobbyModificationRemoveMemberAttributeOptionsInternal : ISettable<LobbyModificationRemoveMemberAttributeOptions>, IDisposable
	{
		// Token: 0x170007B5 RID: 1973
		// (set) Token: 0x06001B08 RID: 6920 RVA: 0x000289D6 File Offset: 0x00026BD6
		public Utf8String Key
		{
			set
			{
				Helper.Set(value, ref this.m_Key);
			}
		}

		// Token: 0x06001B09 RID: 6921 RVA: 0x000289E6 File Offset: 0x00026BE6
		public void Set(ref LobbyModificationRemoveMemberAttributeOptions other)
		{
			this.m_ApiVersion = 1;
			this.Key = other.Key;
		}

		// Token: 0x06001B0A RID: 6922 RVA: 0x00028A00 File Offset: 0x00026C00
		public void Set(ref LobbyModificationRemoveMemberAttributeOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.Key = other.Value.Key;
			}
		}

		// Token: 0x06001B0B RID: 6923 RVA: 0x00028A36 File Offset: 0x00026C36
		public void Dispose()
		{
			Helper.Dispose(ref this.m_Key);
		}

		// Token: 0x04000C18 RID: 3096
		private int m_ApiVersion;

		// Token: 0x04000C19 RID: 3097
		private IntPtr m_Key;
	}
}
