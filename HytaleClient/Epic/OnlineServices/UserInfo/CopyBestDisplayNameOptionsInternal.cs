using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UserInfo
{
	// Token: 0x02000027 RID: 39
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct CopyBestDisplayNameOptionsInternal : ISettable<CopyBestDisplayNameOptions>, IDisposable
	{
		// Token: 0x1700001E RID: 30
		// (set) Token: 0x0600036D RID: 877 RVA: 0x00004DFD File Offset: 0x00002FFD
		public EpicAccountId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x1700001F RID: 31
		// (set) Token: 0x0600036E RID: 878 RVA: 0x00004E0D File Offset: 0x0000300D
		public EpicAccountId TargetUserId
		{
			set
			{
				Helper.Set(value, ref this.m_TargetUserId);
			}
		}

		// Token: 0x0600036F RID: 879 RVA: 0x00004E1D File Offset: 0x0000301D
		public void Set(ref CopyBestDisplayNameOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
			this.TargetUserId = other.TargetUserId;
		}

		// Token: 0x06000370 RID: 880 RVA: 0x00004E44 File Offset: 0x00003044
		public void Set(ref CopyBestDisplayNameOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
				this.TargetUserId = other.Value.TargetUserId;
			}
		}

		// Token: 0x06000371 RID: 881 RVA: 0x00004E8F File Offset: 0x0000308F
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_TargetUserId);
		}

		// Token: 0x04000159 RID: 345
		private int m_ApiVersion;

		// Token: 0x0400015A RID: 346
		private IntPtr m_LocalUserId;

		// Token: 0x0400015B RID: 347
		private IntPtr m_TargetUserId;
	}
}
