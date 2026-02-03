using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.CustomInvites
{
	// Token: 0x0200057D RID: 1405
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct AddNotifyCustomInviteRejectedOptionsInternal : ISettable<AddNotifyCustomInviteRejectedOptions>, IDisposable
	{
		// Token: 0x0600248D RID: 9357 RVA: 0x00035D54 File Offset: 0x00033F54
		public void Set(ref AddNotifyCustomInviteRejectedOptions other)
		{
			this.m_ApiVersion = 1;
		}

		// Token: 0x0600248E RID: 9358 RVA: 0x00035D60 File Offset: 0x00033F60
		public void Set(ref AddNotifyCustomInviteRejectedOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
			}
		}

		// Token: 0x0600248F RID: 9359 RVA: 0x00035D81 File Offset: 0x00033F81
		public void Dispose()
		{
		}

		// Token: 0x04001006 RID: 4102
		private int m_ApiVersion;
	}
}
