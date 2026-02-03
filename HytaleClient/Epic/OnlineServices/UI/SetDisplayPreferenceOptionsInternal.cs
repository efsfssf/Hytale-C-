using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UI
{
	// Token: 0x02000083 RID: 131
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct SetDisplayPreferenceOptionsInternal : ISettable<SetDisplayPreferenceOptions>, IDisposable
	{
		// Token: 0x170000D7 RID: 215
		// (set) Token: 0x0600058F RID: 1423 RVA: 0x00007C64 File Offset: 0x00005E64
		public NotificationLocation NotificationLocation
		{
			set
			{
				this.m_NotificationLocation = value;
			}
		}

		// Token: 0x06000590 RID: 1424 RVA: 0x00007C6E File Offset: 0x00005E6E
		public void Set(ref SetDisplayPreferenceOptions other)
		{
			this.m_ApiVersion = 1;
			this.NotificationLocation = other.NotificationLocation;
		}

		// Token: 0x06000591 RID: 1425 RVA: 0x00007C88 File Offset: 0x00005E88
		public void Set(ref SetDisplayPreferenceOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.NotificationLocation = other.Value.NotificationLocation;
			}
		}

		// Token: 0x06000592 RID: 1426 RVA: 0x00007CBE File Offset: 0x00005EBE
		public void Dispose()
		{
		}

		// Token: 0x040002BA RID: 698
		private int m_ApiVersion;

		// Token: 0x040002BB RID: 699
		private NotificationLocation m_NotificationLocation;
	}
}
