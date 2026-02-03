using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatClient
{
	// Token: 0x020006FD RID: 1789
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct Reserved01OptionsInternal : ISettable<Reserved01Options>, IDisposable
	{
		// Token: 0x06002E0C RID: 11788 RVA: 0x00043F7A File Offset: 0x0004217A
		public void Set(ref Reserved01Options other)
		{
			this.m_ApiVersion = 1;
		}

		// Token: 0x06002E0D RID: 11789 RVA: 0x00043F84 File Offset: 0x00042184
		public void Set(ref Reserved01Options? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
			}
		}

		// Token: 0x06002E0E RID: 11790 RVA: 0x00043FA5 File Offset: 0x000421A5
		public void Dispose()
		{
		}

		// Token: 0x04001456 RID: 5206
		private int m_ApiVersion;
	}
}
