using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio
{
	// Token: 0x02000215 RID: 533
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct CopyOutputDeviceInformationByIndexOptionsInternal : ISettable<CopyOutputDeviceInformationByIndexOptions>, IDisposable
	{
		// Token: 0x170003FC RID: 1020
		// (set) Token: 0x06000F6E RID: 3950 RVA: 0x00016B46 File Offset: 0x00014D46
		public uint DeviceIndex
		{
			set
			{
				this.m_DeviceIndex = value;
			}
		}

		// Token: 0x06000F6F RID: 3951 RVA: 0x00016B50 File Offset: 0x00014D50
		public void Set(ref CopyOutputDeviceInformationByIndexOptions other)
		{
			this.m_ApiVersion = 1;
			this.DeviceIndex = other.DeviceIndex;
		}

		// Token: 0x06000F70 RID: 3952 RVA: 0x00016B68 File Offset: 0x00014D68
		public void Set(ref CopyOutputDeviceInformationByIndexOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.DeviceIndex = other.Value.DeviceIndex;
			}
		}

		// Token: 0x06000F71 RID: 3953 RVA: 0x00016B9E File Offset: 0x00014D9E
		public void Dispose()
		{
		}

		// Token: 0x040006F1 RID: 1777
		private int m_ApiVersion;

		// Token: 0x040006F2 RID: 1778
		private uint m_DeviceIndex;
	}
}
