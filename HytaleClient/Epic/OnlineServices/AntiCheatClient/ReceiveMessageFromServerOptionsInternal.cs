using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatClient
{
	// Token: 0x020006F9 RID: 1785
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct ReceiveMessageFromServerOptionsInternal : ISettable<ReceiveMessageFromServerOptions>, IDisposable
	{
		// Token: 0x17000DA8 RID: 3496
		// (set) Token: 0x06002DF0 RID: 11760 RVA: 0x00043CDF File Offset: 0x00041EDF
		public ArraySegment<byte> Data
		{
			set
			{
				Helper.Set(value, ref this.m_Data, out this.m_DataLengthBytes);
			}
		}

		// Token: 0x06002DF1 RID: 11761 RVA: 0x00043CF5 File Offset: 0x00041EF5
		public void Set(ref ReceiveMessageFromServerOptions other)
		{
			this.m_ApiVersion = 1;
			this.Data = other.Data;
		}

		// Token: 0x06002DF2 RID: 11762 RVA: 0x00043D0C File Offset: 0x00041F0C
		public void Set(ref ReceiveMessageFromServerOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.Data = other.Value.Data;
			}
		}

		// Token: 0x06002DF3 RID: 11763 RVA: 0x00043D42 File Offset: 0x00041F42
		public void Dispose()
		{
			Helper.Dispose(ref this.m_Data);
		}

		// Token: 0x04001444 RID: 5188
		private int m_ApiVersion;

		// Token: 0x04001445 RID: 5189
		private uint m_DataLengthBytes;

		// Token: 0x04001446 RID: 5190
		private IntPtr m_Data;
	}
}
