using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.KWS
{
	// Token: 0x020004A4 RID: 1188
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct QueryAgeGateOptionsInternal : ISettable<QueryAgeGateOptions>, IDisposable
	{
		// Token: 0x06001EFF RID: 7935 RVA: 0x0002D55B File Offset: 0x0002B75B
		public void Set(ref QueryAgeGateOptions other)
		{
			this.m_ApiVersion = 1;
		}

		// Token: 0x06001F00 RID: 7936 RVA: 0x0002D568 File Offset: 0x0002B768
		public void Set(ref QueryAgeGateOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
			}
		}

		// Token: 0x06001F01 RID: 7937 RVA: 0x0002D589 File Offset: 0x0002B789
		public void Dispose()
		{
		}

		// Token: 0x04000D77 RID: 3447
		private int m_ApiVersion;
	}
}
