using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatServer
{
	// Token: 0x02000691 RID: 1681
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct UnprotectMessageOptionsInternal : ISettable<UnprotectMessageOptions>, IDisposable
	{
		// Token: 0x17000CCC RID: 3276
		// (set) Token: 0x06002BA3 RID: 11171 RVA: 0x000403E5 File Offset: 0x0003E5E5
		public IntPtr ClientHandle
		{
			set
			{
				this.m_ClientHandle = value;
			}
		}

		// Token: 0x17000CCD RID: 3277
		// (set) Token: 0x06002BA4 RID: 11172 RVA: 0x000403EF File Offset: 0x0003E5EF
		public ArraySegment<byte> Data
		{
			set
			{
				Helper.Set(value, ref this.m_Data, out this.m_DataLengthBytes);
			}
		}

		// Token: 0x17000CCE RID: 3278
		// (set) Token: 0x06002BA5 RID: 11173 RVA: 0x00040405 File Offset: 0x0003E605
		public uint OutBufferSizeBytes
		{
			set
			{
				this.m_OutBufferSizeBytes = value;
			}
		}

		// Token: 0x06002BA6 RID: 11174 RVA: 0x0004040F File Offset: 0x0003E60F
		public void Set(ref UnprotectMessageOptions other)
		{
			this.m_ApiVersion = 1;
			this.ClientHandle = other.ClientHandle;
			this.Data = other.Data;
			this.OutBufferSizeBytes = other.OutBufferSizeBytes;
		}

		// Token: 0x06002BA7 RID: 11175 RVA: 0x00040440 File Offset: 0x0003E640
		public void Set(ref UnprotectMessageOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.ClientHandle = other.Value.ClientHandle;
				this.Data = other.Value.Data;
				this.OutBufferSizeBytes = other.Value.OutBufferSizeBytes;
			}
		}

		// Token: 0x06002BA8 RID: 11176 RVA: 0x000404A0 File Offset: 0x0003E6A0
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientHandle);
			Helper.Dispose(ref this.m_Data);
		}

		// Token: 0x040012B7 RID: 4791
		private int m_ApiVersion;

		// Token: 0x040012B8 RID: 4792
		private IntPtr m_ClientHandle;

		// Token: 0x040012B9 RID: 4793
		private uint m_DataLengthBytes;

		// Token: 0x040012BA RID: 4794
		private IntPtr m_Data;

		// Token: 0x040012BB RID: 4795
		private uint m_OutBufferSizeBytes;
	}
}
