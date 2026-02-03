using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x020003DF RID: 991
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct LobbyModificationAddMemberAttributeOptionsInternal : ISettable<LobbyModificationAddMemberAttributeOptions>, IDisposable
	{
		// Token: 0x170007B0 RID: 1968
		// (set) Token: 0x06001AFB RID: 6907 RVA: 0x000288AC File Offset: 0x00026AAC
		public AttributeData? Attribute
		{
			set
			{
				Helper.Set<AttributeData, AttributeDataInternal>(ref value, ref this.m_Attribute);
			}
		}

		// Token: 0x170007B1 RID: 1969
		// (set) Token: 0x06001AFC RID: 6908 RVA: 0x000288BD File Offset: 0x00026ABD
		public LobbyAttributeVisibility Visibility
		{
			set
			{
				this.m_Visibility = value;
			}
		}

		// Token: 0x06001AFD RID: 6909 RVA: 0x000288C7 File Offset: 0x00026AC7
		public void Set(ref LobbyModificationAddMemberAttributeOptions other)
		{
			this.m_ApiVersion = 2;
			this.Attribute = other.Attribute;
			this.Visibility = other.Visibility;
		}

		// Token: 0x06001AFE RID: 6910 RVA: 0x000288EC File Offset: 0x00026AEC
		public void Set(ref LobbyModificationAddMemberAttributeOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 2;
				this.Attribute = other.Value.Attribute;
				this.Visibility = other.Value.Visibility;
			}
		}

		// Token: 0x06001AFF RID: 6911 RVA: 0x00028937 File Offset: 0x00026B37
		public void Dispose()
		{
			Helper.Dispose(ref this.m_Attribute);
		}

		// Token: 0x04000C11 RID: 3089
		private int m_ApiVersion;

		// Token: 0x04000C12 RID: 3090
		private IntPtr m_Attribute;

		// Token: 0x04000C13 RID: 3091
		private LobbyAttributeVisibility m_Visibility;
	}
}
