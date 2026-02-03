using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatServer
{
	// Token: 0x02000689 RID: 1673
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct ProtectMessageOptionsInternal : ISettable<ProtectMessageOptions>, IDisposable
	{
		// Token: 0x17000CB0 RID: 3248
		// (set) Token: 0x06002B6D RID: 11117 RVA: 0x0003FF24 File Offset: 0x0003E124
		public IntPtr ClientHandle
		{
			set
			{
				this.m_ClientHandle = value;
			}
		}

		// Token: 0x17000CB1 RID: 3249
		// (set) Token: 0x06002B6E RID: 11118 RVA: 0x0003FF2E File Offset: 0x0003E12E
		public ArraySegment<byte> Data
		{
			set
			{
				Helper.Set(value, ref this.m_Data, out this.m_DataLengthBytes);
			}
		}

		// Token: 0x17000CB2 RID: 3250
		// (set) Token: 0x06002B6F RID: 11119 RVA: 0x0003FF44 File Offset: 0x0003E144
		public uint OutBufferSizeBytes
		{
			set
			{
				this.m_OutBufferSizeBytes = value;
			}
		}

		// Token: 0x06002B70 RID: 11120 RVA: 0x0003FF4E File Offset: 0x0003E14E
		public void Set(ref ProtectMessageOptions other)
		{
			this.m_ApiVersion = 1;
			this.ClientHandle = other.ClientHandle;
			this.Data = other.Data;
			this.OutBufferSizeBytes = other.OutBufferSizeBytes;
		}

		// Token: 0x06002B71 RID: 11121 RVA: 0x0003FF80 File Offset: 0x0003E180
		public void Set(ref ProtectMessageOptions? other)
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

		// Token: 0x06002B72 RID: 11122 RVA: 0x0003FFE0 File Offset: 0x0003E1E0
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientHandle);
			Helper.Dispose(ref this.m_Data);
		}

		// Token: 0x04001295 RID: 4757
		private int m_ApiVersion;

		// Token: 0x04001296 RID: 4758
		private IntPtr m_ClientHandle;

		// Token: 0x04001297 RID: 4759
		private uint m_DataLengthBytes;

		// Token: 0x04001298 RID: 4760
		private IntPtr m_Data;

		// Token: 0x04001299 RID: 4761
		private uint m_OutBufferSizeBytes;
	}
}
