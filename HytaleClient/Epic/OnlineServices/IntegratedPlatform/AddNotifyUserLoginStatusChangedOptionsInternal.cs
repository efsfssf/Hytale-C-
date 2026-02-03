using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.IntegratedPlatform
{
	// Token: 0x020004B2 RID: 1202
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct AddNotifyUserLoginStatusChangedOptionsInternal : ISettable<AddNotifyUserLoginStatusChangedOptions>, IDisposable
	{
		// Token: 0x06001F63 RID: 8035 RVA: 0x0002DF2E File Offset: 0x0002C12E
		public void Set(ref AddNotifyUserLoginStatusChangedOptions other)
		{
			this.m_ApiVersion = 1;
		}

		// Token: 0x06001F64 RID: 8036 RVA: 0x0002DF38 File Offset: 0x0002C138
		public void Set(ref AddNotifyUserLoginStatusChangedOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
			}
		}

		// Token: 0x06001F65 RID: 8037 RVA: 0x0002DF59 File Offset: 0x0002C159
		public void Dispose()
		{
		}

		// Token: 0x04000DA0 RID: 3488
		private int m_ApiVersion;
	}
}
