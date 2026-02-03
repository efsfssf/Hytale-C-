using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UI
{
	// Token: 0x0200005E RID: 94
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct HideFriendsOptionsInternal : ISettable<HideFriendsOptions>, IDisposable
	{
		// Token: 0x17000093 RID: 147
		// (set) Token: 0x060004B6 RID: 1206 RVA: 0x00006E0F File Offset: 0x0000500F
		public EpicAccountId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x060004B7 RID: 1207 RVA: 0x00006E1F File Offset: 0x0000501F
		public void Set(ref HideFriendsOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
		}

		// Token: 0x060004B8 RID: 1208 RVA: 0x00006E38 File Offset: 0x00005038
		public void Set(ref HideFriendsOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
			}
		}

		// Token: 0x060004B9 RID: 1209 RVA: 0x00006E6E File Offset: 0x0000506E
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
		}

		// Token: 0x040001ED RID: 493
		private int m_ApiVersion;

		// Token: 0x040001EE RID: 494
		private IntPtr m_LocalUserId;
	}
}
