using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio
{
	// Token: 0x0200021F RID: 543
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct GetInputDevicesCountOptionsInternal : ISettable<GetInputDevicesCountOptions>, IDisposable
	{
		// Token: 0x06000F84 RID: 3972 RVA: 0x00016CD8 File Offset: 0x00014ED8
		public void Set(ref GetInputDevicesCountOptions other)
		{
			this.m_ApiVersion = 1;
		}

		// Token: 0x06000F85 RID: 3973 RVA: 0x00016CE4 File Offset: 0x00014EE4
		public void Set(ref GetInputDevicesCountOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
			}
		}

		// Token: 0x06000F86 RID: 3974 RVA: 0x00016D05 File Offset: 0x00014F05
		public void Dispose()
		{
		}

		// Token: 0x040006FB RID: 1787
		private int m_ApiVersion;
	}
}
