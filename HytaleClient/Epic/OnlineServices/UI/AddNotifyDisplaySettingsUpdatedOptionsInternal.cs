using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UI
{
	// Token: 0x02000050 RID: 80
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct AddNotifyDisplaySettingsUpdatedOptionsInternal : ISettable<AddNotifyDisplaySettingsUpdatedOptions>, IDisposable
	{
		// Token: 0x06000489 RID: 1161 RVA: 0x00006A76 File Offset: 0x00004C76
		public void Set(ref AddNotifyDisplaySettingsUpdatedOptions other)
		{
			this.m_ApiVersion = 1;
		}

		// Token: 0x0600048A RID: 1162 RVA: 0x00006A80 File Offset: 0x00004C80
		public void Set(ref AddNotifyDisplaySettingsUpdatedOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
			}
		}

		// Token: 0x0600048B RID: 1163 RVA: 0x00006AA1 File Offset: 0x00004CA1
		public void Dispose()
		{
		}

		// Token: 0x040001DC RID: 476
		private int m_ApiVersion;
	}
}
