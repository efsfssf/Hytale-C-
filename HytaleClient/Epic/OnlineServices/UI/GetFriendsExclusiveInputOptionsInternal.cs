using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UI
{
	// Token: 0x02000054 RID: 84
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct GetFriendsExclusiveInputOptionsInternal : ISettable<GetFriendsExclusiveInputOptions>, IDisposable
	{
		// Token: 0x17000088 RID: 136
		// (set) Token: 0x06000491 RID: 1169 RVA: 0x00006AE5 File Offset: 0x00004CE5
		public EpicAccountId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x06000492 RID: 1170 RVA: 0x00006AF5 File Offset: 0x00004CF5
		public void Set(ref GetFriendsExclusiveInputOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
		}

		// Token: 0x06000493 RID: 1171 RVA: 0x00006B0C File Offset: 0x00004D0C
		public void Set(ref GetFriendsExclusiveInputOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
			}
		}

		// Token: 0x06000494 RID: 1172 RVA: 0x00006B42 File Offset: 0x00004D42
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
		}

		// Token: 0x040001DF RID: 479
		private int m_ApiVersion;

		// Token: 0x040001E0 RID: 480
		private IntPtr m_LocalUserId;
	}
}
