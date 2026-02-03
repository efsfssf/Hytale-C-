using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.P2P
{
	// Token: 0x02000780 RID: 1920
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct GetNATTypeOptionsInternal : ISettable<GetNATTypeOptions>, IDisposable
	{
		// Token: 0x060031D0 RID: 12752 RVA: 0x0004A90A File Offset: 0x00048B0A
		public void Set(ref GetNATTypeOptions other)
		{
			this.m_ApiVersion = 1;
		}

		// Token: 0x060031D1 RID: 12753 RVA: 0x0004A914 File Offset: 0x00048B14
		public void Set(ref GetNATTypeOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
			}
		}

		// Token: 0x060031D2 RID: 12754 RVA: 0x0004A935 File Offset: 0x00048B35
		public void Dispose()
		{
		}

		// Token: 0x0400166B RID: 5739
		private int m_ApiVersion;
	}
}
