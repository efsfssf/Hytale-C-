using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatClient
{
	// Token: 0x020006D0 RID: 1744
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct AddExternalIntegrityCatalogOptionsInternal : ISettable<AddExternalIntegrityCatalogOptions>, IDisposable
	{
		// Token: 0x17000D8A RID: 3466
		// (set) Token: 0x06002D4A RID: 11594 RVA: 0x00042E2B File Offset: 0x0004102B
		public Utf8String PathToBinFile
		{
			set
			{
				Helper.Set(value, ref this.m_PathToBinFile);
			}
		}

		// Token: 0x06002D4B RID: 11595 RVA: 0x00042E3B File Offset: 0x0004103B
		public void Set(ref AddExternalIntegrityCatalogOptions other)
		{
			this.m_ApiVersion = 1;
			this.PathToBinFile = other.PathToBinFile;
		}

		// Token: 0x06002D4C RID: 11596 RVA: 0x00042E54 File Offset: 0x00041054
		public void Set(ref AddExternalIntegrityCatalogOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.PathToBinFile = other.Value.PathToBinFile;
			}
		}

		// Token: 0x06002D4D RID: 11597 RVA: 0x00042E8A File Offset: 0x0004108A
		public void Dispose()
		{
			Helper.Dispose(ref this.m_PathToBinFile);
		}

		// Token: 0x040013EE RID: 5102
		private int m_ApiVersion;

		// Token: 0x040013EF RID: 5103
		private IntPtr m_PathToBinFile;
	}
}
