using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x02000162 RID: 354
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct SessionModificationAddAttributeOptionsInternal : ISettable<SessionModificationAddAttributeOptions>, IDisposable
	{
		// Token: 0x17000270 RID: 624
		// (set) Token: 0x06000ACA RID: 2762 RVA: 0x0000F544 File Offset: 0x0000D744
		public AttributeData? SessionAttribute
		{
			set
			{
				Helper.Set<AttributeData, AttributeDataInternal>(ref value, ref this.m_SessionAttribute);
			}
		}

		// Token: 0x17000271 RID: 625
		// (set) Token: 0x06000ACB RID: 2763 RVA: 0x0000F555 File Offset: 0x0000D755
		public SessionAttributeAdvertisementType AdvertisementType
		{
			set
			{
				this.m_AdvertisementType = value;
			}
		}

		// Token: 0x06000ACC RID: 2764 RVA: 0x0000F55F File Offset: 0x0000D75F
		public void Set(ref SessionModificationAddAttributeOptions other)
		{
			this.m_ApiVersion = 2;
			this.SessionAttribute = other.SessionAttribute;
			this.AdvertisementType = other.AdvertisementType;
		}

		// Token: 0x06000ACD RID: 2765 RVA: 0x0000F584 File Offset: 0x0000D784
		public void Set(ref SessionModificationAddAttributeOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 2;
				this.SessionAttribute = other.Value.SessionAttribute;
				this.AdvertisementType = other.Value.AdvertisementType;
			}
		}

		// Token: 0x06000ACE RID: 2766 RVA: 0x0000F5CF File Offset: 0x0000D7CF
		public void Dispose()
		{
			Helper.Dispose(ref this.m_SessionAttribute);
		}

		// Token: 0x040004E8 RID: 1256
		private int m_ApiVersion;

		// Token: 0x040004E9 RID: 1257
		private IntPtr m_SessionAttribute;

		// Token: 0x040004EA RID: 1258
		private SessionAttributeAdvertisementType m_AdvertisementType;
	}
}
