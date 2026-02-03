using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio
{
	// Token: 0x02000213 RID: 531
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct CopyInputDeviceInformationByIndexOptionsInternal : ISettable<CopyInputDeviceInformationByIndexOptions>, IDisposable
	{
		// Token: 0x170003FA RID: 1018
		// (set) Token: 0x06000F68 RID: 3944 RVA: 0x00016AD8 File Offset: 0x00014CD8
		public uint DeviceIndex
		{
			set
			{
				this.m_DeviceIndex = value;
			}
		}

		// Token: 0x06000F69 RID: 3945 RVA: 0x00016AE2 File Offset: 0x00014CE2
		public void Set(ref CopyInputDeviceInformationByIndexOptions other)
		{
			this.m_ApiVersion = 1;
			this.DeviceIndex = other.DeviceIndex;
		}

		// Token: 0x06000F6A RID: 3946 RVA: 0x00016AFC File Offset: 0x00014CFC
		public void Set(ref CopyInputDeviceInformationByIndexOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.DeviceIndex = other.Value.DeviceIndex;
			}
		}

		// Token: 0x06000F6B RID: 3947 RVA: 0x00016B32 File Offset: 0x00014D32
		public void Dispose()
		{
		}

		// Token: 0x040006EE RID: 1774
		private int m_ApiVersion;

		// Token: 0x040006EF RID: 1775
		private uint m_DeviceIndex;
	}
}
