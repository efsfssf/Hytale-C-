using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio
{
	// Token: 0x020001FB RID: 507
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct AddNotifyAudioDevicesChangedOptionsInternal : ISettable<AddNotifyAudioDevicesChangedOptions>, IDisposable
	{
		// Token: 0x06000EAA RID: 3754 RVA: 0x00015832 File Offset: 0x00013A32
		public void Set(ref AddNotifyAudioDevicesChangedOptions other)
		{
			this.m_ApiVersion = 1;
		}

		// Token: 0x06000EAB RID: 3755 RVA: 0x0001583C File Offset: 0x00013A3C
		public void Set(ref AddNotifyAudioDevicesChangedOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
			}
		}

		// Token: 0x06000EAC RID: 3756 RVA: 0x0001585D File Offset: 0x00013A5D
		public void Dispose()
		{
		}

		// Token: 0x040006A3 RID: 1699
		private int m_ApiVersion;
	}
}
