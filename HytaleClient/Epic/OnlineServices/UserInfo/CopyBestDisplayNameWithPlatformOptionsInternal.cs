using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UserInfo
{
	// Token: 0x02000029 RID: 41
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct CopyBestDisplayNameWithPlatformOptionsInternal : ISettable<CopyBestDisplayNameWithPlatformOptions>, IDisposable
	{
		// Token: 0x17000023 RID: 35
		// (set) Token: 0x06000378 RID: 888 RVA: 0x00004EDD File Offset: 0x000030DD
		public EpicAccountId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x17000024 RID: 36
		// (set) Token: 0x06000379 RID: 889 RVA: 0x00004EED File Offset: 0x000030ED
		public EpicAccountId TargetUserId
		{
			set
			{
				Helper.Set(value, ref this.m_TargetUserId);
			}
		}

		// Token: 0x17000025 RID: 37
		// (set) Token: 0x0600037A RID: 890 RVA: 0x00004EFD File Offset: 0x000030FD
		public uint TargetPlatformType
		{
			set
			{
				this.m_TargetPlatformType = value;
			}
		}

		// Token: 0x0600037B RID: 891 RVA: 0x00004F07 File Offset: 0x00003107
		public void Set(ref CopyBestDisplayNameWithPlatformOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
			this.TargetUserId = other.TargetUserId;
			this.TargetPlatformType = other.TargetPlatformType;
		}

		// Token: 0x0600037C RID: 892 RVA: 0x00004F38 File Offset: 0x00003138
		public void Set(ref CopyBestDisplayNameWithPlatformOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
				this.TargetUserId = other.Value.TargetUserId;
				this.TargetPlatformType = other.Value.TargetPlatformType;
			}
		}

		// Token: 0x0600037D RID: 893 RVA: 0x00004F98 File Offset: 0x00003198
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_TargetUserId);
		}

		// Token: 0x0400015F RID: 351
		private int m_ApiVersion;

		// Token: 0x04000160 RID: 352
		private IntPtr m_LocalUserId;

		// Token: 0x04000161 RID: 353
		private IntPtr m_TargetUserId;

		// Token: 0x04000162 RID: 354
		private uint m_TargetPlatformType;
	}
}
