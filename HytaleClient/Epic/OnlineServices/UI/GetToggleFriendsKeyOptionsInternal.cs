using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UI
{
	// Token: 0x0200005A RID: 90
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct GetToggleFriendsKeyOptionsInternal : ISettable<GetToggleFriendsKeyOptions>, IDisposable
	{
		// Token: 0x0600049E RID: 1182 RVA: 0x00006C00 File Offset: 0x00004E00
		public void Set(ref GetToggleFriendsKeyOptions other)
		{
			this.m_ApiVersion = 1;
		}

		// Token: 0x0600049F RID: 1183 RVA: 0x00006C0C File Offset: 0x00004E0C
		public void Set(ref GetToggleFriendsKeyOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
			}
		}

		// Token: 0x060004A0 RID: 1184 RVA: 0x00006C2D File Offset: 0x00004E2D
		public void Dispose()
		{
		}

		// Token: 0x040001E5 RID: 485
		private int m_ApiVersion;
	}
}
