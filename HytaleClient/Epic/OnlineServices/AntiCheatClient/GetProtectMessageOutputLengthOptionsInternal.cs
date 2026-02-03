using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatClient
{
	// Token: 0x020006E3 RID: 1763
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct GetProtectMessageOutputLengthOptionsInternal : ISettable<GetProtectMessageOutputLengthOptions>, IDisposable
	{
		// Token: 0x17000D90 RID: 3472
		// (set) Token: 0x06002D88 RID: 11656 RVA: 0x00043739 File Offset: 0x00041939
		public uint DataLengthBytes
		{
			set
			{
				this.m_DataLengthBytes = value;
			}
		}

		// Token: 0x06002D89 RID: 11657 RVA: 0x00043743 File Offset: 0x00041943
		public void Set(ref GetProtectMessageOutputLengthOptions other)
		{
			this.m_ApiVersion = 1;
			this.DataLengthBytes = other.DataLengthBytes;
		}

		// Token: 0x06002D8A RID: 11658 RVA: 0x0004375C File Offset: 0x0004195C
		public void Set(ref GetProtectMessageOutputLengthOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.DataLengthBytes = other.Value.DataLengthBytes;
			}
		}

		// Token: 0x06002D8B RID: 11659 RVA: 0x00043792 File Offset: 0x00041992
		public void Dispose()
		{
		}

		// Token: 0x04001427 RID: 5159
		private int m_ApiVersion;

		// Token: 0x04001428 RID: 5160
		private uint m_DataLengthBytes;
	}
}
