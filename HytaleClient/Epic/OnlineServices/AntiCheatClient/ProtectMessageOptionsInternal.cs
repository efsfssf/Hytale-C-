using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatClient
{
	// Token: 0x020006F5 RID: 1781
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct ProtectMessageOptionsInternal : ISettable<ProtectMessageOptions>, IDisposable
	{
		// Token: 0x17000DA1 RID: 3489
		// (set) Token: 0x06002DE0 RID: 11744 RVA: 0x00043B63 File Offset: 0x00041D63
		public ArraySegment<byte> Data
		{
			set
			{
				Helper.Set(value, ref this.m_Data, out this.m_DataLengthBytes);
			}
		}

		// Token: 0x17000DA2 RID: 3490
		// (set) Token: 0x06002DE1 RID: 11745 RVA: 0x00043B79 File Offset: 0x00041D79
		public uint OutBufferSizeBytes
		{
			set
			{
				this.m_OutBufferSizeBytes = value;
			}
		}

		// Token: 0x06002DE2 RID: 11746 RVA: 0x00043B83 File Offset: 0x00041D83
		public void Set(ref ProtectMessageOptions other)
		{
			this.m_ApiVersion = 1;
			this.Data = other.Data;
			this.OutBufferSizeBytes = other.OutBufferSizeBytes;
		}

		// Token: 0x06002DE3 RID: 11747 RVA: 0x00043BA8 File Offset: 0x00041DA8
		public void Set(ref ProtectMessageOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.Data = other.Value.Data;
				this.OutBufferSizeBytes = other.Value.OutBufferSizeBytes;
			}
		}

		// Token: 0x06002DE4 RID: 11748 RVA: 0x00043BF3 File Offset: 0x00041DF3
		public void Dispose()
		{
			Helper.Dispose(ref this.m_Data);
		}

		// Token: 0x04001439 RID: 5177
		private int m_ApiVersion;

		// Token: 0x0400143A RID: 5178
		private uint m_DataLengthBytes;

		// Token: 0x0400143B RID: 5179
		private IntPtr m_Data;

		// Token: 0x0400143C RID: 5180
		private uint m_OutBufferSizeBytes;
	}
}
