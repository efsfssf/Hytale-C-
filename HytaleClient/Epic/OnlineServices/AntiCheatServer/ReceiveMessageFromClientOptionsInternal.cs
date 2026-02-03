using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatServer
{
	// Token: 0x0200068B RID: 1675
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct ReceiveMessageFromClientOptionsInternal : ISettable<ReceiveMessageFromClientOptions>, IDisposable
	{
		// Token: 0x17000CB5 RID: 3253
		// (set) Token: 0x06002B77 RID: 11127 RVA: 0x0004001D File Offset: 0x0003E21D
		public IntPtr ClientHandle
		{
			set
			{
				this.m_ClientHandle = value;
			}
		}

		// Token: 0x17000CB6 RID: 3254
		// (set) Token: 0x06002B78 RID: 11128 RVA: 0x00040027 File Offset: 0x0003E227
		public ArraySegment<byte> Data
		{
			set
			{
				Helper.Set(value, ref this.m_Data, out this.m_DataLengthBytes);
			}
		}

		// Token: 0x06002B79 RID: 11129 RVA: 0x0004003D File Offset: 0x0003E23D
		public void Set(ref ReceiveMessageFromClientOptions other)
		{
			this.m_ApiVersion = 1;
			this.ClientHandle = other.ClientHandle;
			this.Data = other.Data;
		}

		// Token: 0x06002B7A RID: 11130 RVA: 0x00040064 File Offset: 0x0003E264
		public void Set(ref ReceiveMessageFromClientOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.ClientHandle = other.Value.ClientHandle;
				this.Data = other.Value.Data;
			}
		}

		// Token: 0x06002B7B RID: 11131 RVA: 0x000400AF File Offset: 0x0003E2AF
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientHandle);
			Helper.Dispose(ref this.m_Data);
		}

		// Token: 0x0400129C RID: 4764
		private int m_ApiVersion;

		// Token: 0x0400129D RID: 4765
		private IntPtr m_ClientHandle;

		// Token: 0x0400129E RID: 4766
		private uint m_DataLengthBytes;

		// Token: 0x0400129F RID: 4767
		private IntPtr m_Data;
	}
}
