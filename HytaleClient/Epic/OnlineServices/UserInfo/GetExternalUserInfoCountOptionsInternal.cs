using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UserInfo
{
	// Token: 0x02000035 RID: 53
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct GetExternalUserInfoCountOptionsInternal : ISettable<GetExternalUserInfoCountOptions>, IDisposable
	{
		// Token: 0x17000046 RID: 70
		// (set) Token: 0x060003C4 RID: 964 RVA: 0x00005600 File Offset: 0x00003800
		public EpicAccountId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x17000047 RID: 71
		// (set) Token: 0x060003C5 RID: 965 RVA: 0x00005610 File Offset: 0x00003810
		public EpicAccountId TargetUserId
		{
			set
			{
				Helper.Set(value, ref this.m_TargetUserId);
			}
		}

		// Token: 0x060003C6 RID: 966 RVA: 0x00005620 File Offset: 0x00003820
		public void Set(ref GetExternalUserInfoCountOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
			this.TargetUserId = other.TargetUserId;
		}

		// Token: 0x060003C7 RID: 967 RVA: 0x00005644 File Offset: 0x00003844
		public void Set(ref GetExternalUserInfoCountOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
				this.TargetUserId = other.Value.TargetUserId;
			}
		}

		// Token: 0x060003C8 RID: 968 RVA: 0x0000568F File Offset: 0x0000388F
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_TargetUserId);
		}

		// Token: 0x04000188 RID: 392
		private int m_ApiVersion;

		// Token: 0x04000189 RID: 393
		private IntPtr m_LocalUserId;

		// Token: 0x0400018A RID: 394
		private IntPtr m_TargetUserId;
	}
}
