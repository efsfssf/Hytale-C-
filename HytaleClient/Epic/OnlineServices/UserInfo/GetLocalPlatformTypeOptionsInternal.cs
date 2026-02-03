using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UserInfo
{
	// Token: 0x02000037 RID: 55
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct GetLocalPlatformTypeOptionsInternal : ISettable<GetLocalPlatformTypeOptions>, IDisposable
	{
		// Token: 0x060003C9 RID: 969 RVA: 0x000056AA File Offset: 0x000038AA
		public void Set(ref GetLocalPlatformTypeOptions other)
		{
			this.m_ApiVersion = 1;
		}

		// Token: 0x060003CA RID: 970 RVA: 0x000056B4 File Offset: 0x000038B4
		public void Set(ref GetLocalPlatformTypeOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
			}
		}

		// Token: 0x060003CB RID: 971 RVA: 0x000056D5 File Offset: 0x000038D5
		public void Dispose()
		{
		}

		// Token: 0x0400018B RID: 395
		private int m_ApiVersion;
	}
}
