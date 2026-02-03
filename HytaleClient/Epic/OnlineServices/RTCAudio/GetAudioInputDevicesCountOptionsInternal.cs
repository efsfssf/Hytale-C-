using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio
{
	// Token: 0x02000219 RID: 537
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct GetAudioInputDevicesCountOptionsInternal : ISettable<GetAudioInputDevicesCountOptions>, IDisposable
	{
		// Token: 0x06000F78 RID: 3960 RVA: 0x00016C0D File Offset: 0x00014E0D
		public void Set(ref GetAudioInputDevicesCountOptions other)
		{
			this.m_ApiVersion = 1;
		}

		// Token: 0x06000F79 RID: 3961 RVA: 0x00016C18 File Offset: 0x00014E18
		public void Set(ref GetAudioInputDevicesCountOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
			}
		}

		// Token: 0x06000F7A RID: 3962 RVA: 0x00016C39 File Offset: 0x00014E39
		public void Dispose()
		{
		}

		// Token: 0x040006F6 RID: 1782
		private int m_ApiVersion;
	}
}
