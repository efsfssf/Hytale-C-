using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio
{
	// Token: 0x02000217 RID: 535
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct GetAudioInputDeviceByIndexOptionsInternal : ISettable<GetAudioInputDeviceByIndexOptions>, IDisposable
	{
		// Token: 0x170003FE RID: 1022
		// (set) Token: 0x06000F74 RID: 3956 RVA: 0x00016BB2 File Offset: 0x00014DB2
		public uint DeviceInfoIndex
		{
			set
			{
				this.m_DeviceInfoIndex = value;
			}
		}

		// Token: 0x06000F75 RID: 3957 RVA: 0x00016BBC File Offset: 0x00014DBC
		public void Set(ref GetAudioInputDeviceByIndexOptions other)
		{
			this.m_ApiVersion = 1;
			this.DeviceInfoIndex = other.DeviceInfoIndex;
		}

		// Token: 0x06000F76 RID: 3958 RVA: 0x00016BD4 File Offset: 0x00014DD4
		public void Set(ref GetAudioInputDeviceByIndexOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.DeviceInfoIndex = other.Value.DeviceInfoIndex;
			}
		}

		// Token: 0x06000F77 RID: 3959 RVA: 0x00016C0A File Offset: 0x00014E0A
		public void Dispose()
		{
		}

		// Token: 0x040006F4 RID: 1780
		private int m_ApiVersion;

		// Token: 0x040006F5 RID: 1781
		private uint m_DeviceInfoIndex;
	}
}
