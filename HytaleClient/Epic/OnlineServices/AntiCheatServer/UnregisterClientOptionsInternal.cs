using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatServer
{
	// Token: 0x02000693 RID: 1683
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct UnregisterClientOptionsInternal : ISettable<UnregisterClientOptions>, IDisposable
	{
		// Token: 0x17000CD0 RID: 3280
		// (set) Token: 0x06002BAB RID: 11179 RVA: 0x000404CC File Offset: 0x0003E6CC
		public IntPtr ClientHandle
		{
			set
			{
				this.m_ClientHandle = value;
			}
		}

		// Token: 0x06002BAC RID: 11180 RVA: 0x000404D6 File Offset: 0x0003E6D6
		public void Set(ref UnregisterClientOptions other)
		{
			this.m_ApiVersion = 1;
			this.ClientHandle = other.ClientHandle;
		}

		// Token: 0x06002BAD RID: 11181 RVA: 0x000404F0 File Offset: 0x0003E6F0
		public void Set(ref UnregisterClientOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.ClientHandle = other.Value.ClientHandle;
			}
		}

		// Token: 0x06002BAE RID: 11182 RVA: 0x00040526 File Offset: 0x0003E726
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientHandle);
		}

		// Token: 0x040012BD RID: 4797
		private int m_ApiVersion;

		// Token: 0x040012BE RID: 4798
		private IntPtr m_ClientHandle;
	}
}
