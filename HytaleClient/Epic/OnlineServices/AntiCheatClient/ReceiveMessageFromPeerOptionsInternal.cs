using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatClient
{
	// Token: 0x020006F7 RID: 1783
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct ReceiveMessageFromPeerOptionsInternal : ISettable<ReceiveMessageFromPeerOptions>, IDisposable
	{
		// Token: 0x17000DA5 RID: 3493
		// (set) Token: 0x06002DE9 RID: 11753 RVA: 0x00043C24 File Offset: 0x00041E24
		public IntPtr PeerHandle
		{
			set
			{
				this.m_PeerHandle = value;
			}
		}

		// Token: 0x17000DA6 RID: 3494
		// (set) Token: 0x06002DEA RID: 11754 RVA: 0x00043C2E File Offset: 0x00041E2E
		public ArraySegment<byte> Data
		{
			set
			{
				Helper.Set(value, ref this.m_Data, out this.m_DataLengthBytes);
			}
		}

		// Token: 0x06002DEB RID: 11755 RVA: 0x00043C44 File Offset: 0x00041E44
		public void Set(ref ReceiveMessageFromPeerOptions other)
		{
			this.m_ApiVersion = 1;
			this.PeerHandle = other.PeerHandle;
			this.Data = other.Data;
		}

		// Token: 0x06002DEC RID: 11756 RVA: 0x00043C68 File Offset: 0x00041E68
		public void Set(ref ReceiveMessageFromPeerOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.PeerHandle = other.Value.PeerHandle;
				this.Data = other.Value.Data;
			}
		}

		// Token: 0x06002DED RID: 11757 RVA: 0x00043CB3 File Offset: 0x00041EB3
		public void Dispose()
		{
			Helper.Dispose(ref this.m_PeerHandle);
			Helper.Dispose(ref this.m_Data);
		}

		// Token: 0x0400143F RID: 5183
		private int m_ApiVersion;

		// Token: 0x04001440 RID: 5184
		private IntPtr m_PeerHandle;

		// Token: 0x04001441 RID: 5185
		private uint m_DataLengthBytes;

		// Token: 0x04001442 RID: 5186
		private IntPtr m_Data;
	}
}
