using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UI
{
	// Token: 0x02000058 RID: 88
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct GetToggleFriendsButtonOptionsInternal : ISettable<GetToggleFriendsButtonOptions>, IDisposable
	{
		// Token: 0x0600049B RID: 1179 RVA: 0x00006BD1 File Offset: 0x00004DD1
		public void Set(ref GetToggleFriendsButtonOptions other)
		{
			this.m_ApiVersion = 1;
		}

		// Token: 0x0600049C RID: 1180 RVA: 0x00006BDC File Offset: 0x00004DDC
		public void Set(ref GetToggleFriendsButtonOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
			}
		}

		// Token: 0x0600049D RID: 1181 RVA: 0x00006BFD File Offset: 0x00004DFD
		public void Dispose()
		{
		}

		// Token: 0x040001E4 RID: 484
		private int m_ApiVersion;
	}
}
