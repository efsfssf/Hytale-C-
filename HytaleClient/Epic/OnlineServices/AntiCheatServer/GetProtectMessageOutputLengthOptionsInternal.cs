using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatServer
{
	// Token: 0x02000681 RID: 1665
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct GetProtectMessageOutputLengthOptionsInternal : ISettable<GetProtectMessageOutputLengthOptions>, IDisposable
	{
		// Token: 0x17000CAC RID: 3244
		// (set) Token: 0x06002B4B RID: 11083 RVA: 0x0003FE95 File Offset: 0x0003E095
		public uint DataLengthBytes
		{
			set
			{
				this.m_DataLengthBytes = value;
			}
		}

		// Token: 0x06002B4C RID: 11084 RVA: 0x0003FE9F File Offset: 0x0003E09F
		public void Set(ref GetProtectMessageOutputLengthOptions other)
		{
			this.m_ApiVersion = 1;
			this.DataLengthBytes = other.DataLengthBytes;
		}

		// Token: 0x06002B4D RID: 11085 RVA: 0x0003FEB8 File Offset: 0x0003E0B8
		public void Set(ref GetProtectMessageOutputLengthOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.DataLengthBytes = other.Value.DataLengthBytes;
			}
		}

		// Token: 0x06002B4E RID: 11086 RVA: 0x0003FEEE File Offset: 0x0003E0EE
		public void Dispose()
		{
		}

		// Token: 0x04001290 RID: 4752
		private int m_ApiVersion;

		// Token: 0x04001291 RID: 4753
		private uint m_DataLengthBytes;
	}
}
