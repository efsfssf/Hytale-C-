using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x020003DD RID: 989
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct LobbyModificationAddAttributeOptionsInternal : ISettable<LobbyModificationAddAttributeOptions>, IDisposable
	{
		// Token: 0x170007AC RID: 1964
		// (set) Token: 0x06001AF2 RID: 6898 RVA: 0x000287F0 File Offset: 0x000269F0
		public AttributeData? Attribute
		{
			set
			{
				Helper.Set<AttributeData, AttributeDataInternal>(ref value, ref this.m_Attribute);
			}
		}

		// Token: 0x170007AD RID: 1965
		// (set) Token: 0x06001AF3 RID: 6899 RVA: 0x00028801 File Offset: 0x00026A01
		public LobbyAttributeVisibility Visibility
		{
			set
			{
				this.m_Visibility = value;
			}
		}

		// Token: 0x06001AF4 RID: 6900 RVA: 0x0002880B File Offset: 0x00026A0B
		public void Set(ref LobbyModificationAddAttributeOptions other)
		{
			this.m_ApiVersion = 2;
			this.Attribute = other.Attribute;
			this.Visibility = other.Visibility;
		}

		// Token: 0x06001AF5 RID: 6901 RVA: 0x00028830 File Offset: 0x00026A30
		public void Set(ref LobbyModificationAddAttributeOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 2;
				this.Attribute = other.Value.Attribute;
				this.Visibility = other.Value.Visibility;
			}
		}

		// Token: 0x06001AF6 RID: 6902 RVA: 0x0002887B File Offset: 0x00026A7B
		public void Dispose()
		{
			Helper.Dispose(ref this.m_Attribute);
		}

		// Token: 0x04000C0C RID: 3084
		private int m_ApiVersion;

		// Token: 0x04000C0D RID: 3085
		private IntPtr m_Attribute;

		// Token: 0x04000C0E RID: 3086
		private LobbyAttributeVisibility m_Visibility;
	}
}
