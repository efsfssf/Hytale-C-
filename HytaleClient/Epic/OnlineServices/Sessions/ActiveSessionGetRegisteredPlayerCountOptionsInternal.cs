using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x020000DF RID: 223
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct ActiveSessionGetRegisteredPlayerCountOptionsInternal : ISettable<ActiveSessionGetRegisteredPlayerCountOptions>, IDisposable
	{
		// Token: 0x060007E6 RID: 2022 RVA: 0x0000B7BD File Offset: 0x000099BD
		public void Set(ref ActiveSessionGetRegisteredPlayerCountOptions other)
		{
			this.m_ApiVersion = 1;
		}

		// Token: 0x060007E7 RID: 2023 RVA: 0x0000B7C8 File Offset: 0x000099C8
		public void Set(ref ActiveSessionGetRegisteredPlayerCountOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
			}
		}

		// Token: 0x060007E8 RID: 2024 RVA: 0x0000B7E9 File Offset: 0x000099E9
		public void Dispose()
		{
		}

		// Token: 0x040003CC RID: 972
		private int m_ApiVersion;
	}
}
