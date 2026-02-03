using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio
{
	// Token: 0x0200021D RID: 541
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct GetAudioOutputDevicesCountOptionsInternal : ISettable<GetAudioOutputDevicesCountOptions>, IDisposable
	{
		// Token: 0x06000F81 RID: 3969 RVA: 0x00016CA9 File Offset: 0x00014EA9
		public void Set(ref GetAudioOutputDevicesCountOptions other)
		{
			this.m_ApiVersion = 1;
		}

		// Token: 0x06000F82 RID: 3970 RVA: 0x00016CB4 File Offset: 0x00014EB4
		public void Set(ref GetAudioOutputDevicesCountOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
			}
		}

		// Token: 0x06000F83 RID: 3971 RVA: 0x00016CD5 File Offset: 0x00014ED5
		public void Dispose()
		{
		}

		// Token: 0x040006FA RID: 1786
		private int m_ApiVersion;
	}
}
