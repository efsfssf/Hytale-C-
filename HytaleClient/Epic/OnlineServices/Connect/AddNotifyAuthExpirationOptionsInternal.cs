using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Connect
{
	// Token: 0x020005C3 RID: 1475
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct AddNotifyAuthExpirationOptionsInternal : ISettable<AddNotifyAuthExpirationOptions>, IDisposable
	{
		// Token: 0x0600265B RID: 9819 RVA: 0x00038676 File Offset: 0x00036876
		public void Set(ref AddNotifyAuthExpirationOptions other)
		{
			this.m_ApiVersion = 1;
		}

		// Token: 0x0600265C RID: 9820 RVA: 0x00038680 File Offset: 0x00036880
		public void Set(ref AddNotifyAuthExpirationOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
			}
		}

		// Token: 0x0600265D RID: 9821 RVA: 0x000386A1 File Offset: 0x000368A1
		public void Dispose()
		{
		}

		// Token: 0x04001099 RID: 4249
		private int m_ApiVersion;
	}
}
