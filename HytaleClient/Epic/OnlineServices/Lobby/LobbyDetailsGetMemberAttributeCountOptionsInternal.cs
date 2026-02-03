using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x020003C6 RID: 966
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct LobbyDetailsGetMemberAttributeCountOptionsInternal : ISettable<LobbyDetailsGetMemberAttributeCountOptions>, IDisposable
	{
		// Token: 0x17000758 RID: 1880
		// (set) Token: 0x060019CC RID: 6604 RVA: 0x00025E5D File Offset: 0x0002405D
		public ProductUserId TargetUserId
		{
			set
			{
				Helper.Set(value, ref this.m_TargetUserId);
			}
		}

		// Token: 0x060019CD RID: 6605 RVA: 0x00025E6D File Offset: 0x0002406D
		public void Set(ref LobbyDetailsGetMemberAttributeCountOptions other)
		{
			this.m_ApiVersion = 1;
			this.TargetUserId = other.TargetUserId;
		}

		// Token: 0x060019CE RID: 6606 RVA: 0x00025E84 File Offset: 0x00024084
		public void Set(ref LobbyDetailsGetMemberAttributeCountOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.TargetUserId = other.Value.TargetUserId;
			}
		}

		// Token: 0x060019CF RID: 6607 RVA: 0x00025EBA File Offset: 0x000240BA
		public void Dispose()
		{
			Helper.Dispose(ref this.m_TargetUserId);
		}

		// Token: 0x04000B74 RID: 2932
		private int m_ApiVersion;

		// Token: 0x04000B75 RID: 2933
		private IntPtr m_TargetUserId;
	}
}
