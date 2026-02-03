using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatClient
{
	// Token: 0x020006D2 RID: 1746
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct AddNotifyClientIntegrityViolatedOptionsInternal : ISettable<AddNotifyClientIntegrityViolatedOptions>, IDisposable
	{
		// Token: 0x06002D4E RID: 11598 RVA: 0x00042E99 File Offset: 0x00041099
		public void Set(ref AddNotifyClientIntegrityViolatedOptions other)
		{
			this.m_ApiVersion = 1;
		}

		// Token: 0x06002D4F RID: 11599 RVA: 0x00042EA4 File Offset: 0x000410A4
		public void Set(ref AddNotifyClientIntegrityViolatedOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
			}
		}

		// Token: 0x06002D50 RID: 11600 RVA: 0x00042EC5 File Offset: 0x000410C5
		public void Dispose()
		{
		}

		// Token: 0x040013F0 RID: 5104
		private int m_ApiVersion;
	}
}
