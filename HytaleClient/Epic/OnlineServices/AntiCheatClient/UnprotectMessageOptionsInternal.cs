using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatClient
{
	// Token: 0x020006FF RID: 1791
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct UnprotectMessageOptionsInternal : ISettable<UnprotectMessageOptions>, IDisposable
	{
		// Token: 0x17000DB9 RID: 3513
		// (set) Token: 0x06002E13 RID: 11795 RVA: 0x00043FCA File Offset: 0x000421CA
		public ArraySegment<byte> Data
		{
			set
			{
				Helper.Set(value, ref this.m_Data, out this.m_DataLengthBytes);
			}
		}

		// Token: 0x17000DBA RID: 3514
		// (set) Token: 0x06002E14 RID: 11796 RVA: 0x00043FE0 File Offset: 0x000421E0
		public uint OutBufferSizeBytes
		{
			set
			{
				this.m_OutBufferSizeBytes = value;
			}
		}

		// Token: 0x06002E15 RID: 11797 RVA: 0x00043FEA File Offset: 0x000421EA
		public void Set(ref UnprotectMessageOptions other)
		{
			this.m_ApiVersion = 1;
			this.Data = other.Data;
			this.OutBufferSizeBytes = other.OutBufferSizeBytes;
		}

		// Token: 0x06002E16 RID: 11798 RVA: 0x00044010 File Offset: 0x00042210
		public void Set(ref UnprotectMessageOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.Data = other.Value.Data;
				this.OutBufferSizeBytes = other.Value.OutBufferSizeBytes;
			}
		}

		// Token: 0x06002E17 RID: 11799 RVA: 0x0004405B File Offset: 0x0004225B
		public void Dispose()
		{
			Helper.Dispose(ref this.m_Data);
		}

		// Token: 0x04001459 RID: 5209
		private int m_ApiVersion;

		// Token: 0x0400145A RID: 5210
		private uint m_DataLengthBytes;

		// Token: 0x0400145B RID: 5211
		private IntPtr m_Data;

		// Token: 0x0400145C RID: 5212
		private uint m_OutBufferSizeBytes;
	}
}
