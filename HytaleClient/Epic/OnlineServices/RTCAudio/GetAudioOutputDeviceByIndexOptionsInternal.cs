using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio
{
	// Token: 0x0200021B RID: 539
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct GetAudioOutputDeviceByIndexOptionsInternal : ISettable<GetAudioOutputDeviceByIndexOptions>, IDisposable
	{
		// Token: 0x17000400 RID: 1024
		// (set) Token: 0x06000F7D RID: 3965 RVA: 0x00016C4D File Offset: 0x00014E4D
		public uint DeviceInfoIndex
		{
			set
			{
				this.m_DeviceInfoIndex = value;
			}
		}

		// Token: 0x06000F7E RID: 3966 RVA: 0x00016C57 File Offset: 0x00014E57
		public void Set(ref GetAudioOutputDeviceByIndexOptions other)
		{
			this.m_ApiVersion = 1;
			this.DeviceInfoIndex = other.DeviceInfoIndex;
		}

		// Token: 0x06000F7F RID: 3967 RVA: 0x00016C70 File Offset: 0x00014E70
		public void Set(ref GetAudioOutputDeviceByIndexOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.DeviceInfoIndex = other.Value.DeviceInfoIndex;
			}
		}

		// Token: 0x06000F80 RID: 3968 RVA: 0x00016CA6 File Offset: 0x00014EA6
		public void Dispose()
		{
		}

		// Token: 0x040006F8 RID: 1784
		private int m_ApiVersion;

		// Token: 0x040006F9 RID: 1785
		private uint m_DeviceInfoIndex;
	}
}
