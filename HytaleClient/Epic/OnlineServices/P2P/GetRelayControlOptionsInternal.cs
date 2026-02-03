using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.P2P
{
	// Token: 0x02000786 RID: 1926
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct GetRelayControlOptionsInternal : ISettable<GetRelayControlOptions>, IDisposable
	{
		// Token: 0x060031D9 RID: 12761 RVA: 0x0004A998 File Offset: 0x00048B98
		public void Set(ref GetRelayControlOptions other)
		{
			this.m_ApiVersion = 1;
		}

		// Token: 0x060031DA RID: 12762 RVA: 0x0004A9A4 File Offset: 0x00048BA4
		public void Set(ref GetRelayControlOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
			}
		}

		// Token: 0x060031DB RID: 12763 RVA: 0x0004A9C5 File Offset: 0x00048BC5
		public void Dispose()
		{
		}

		// Token: 0x0400166E RID: 5742
		private int m_ApiVersion;
	}
}
