using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UserInfo
{
	// Token: 0x0200002F RID: 47
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct CopyExternalUserInfoByIndexOptionsInternal : ISettable<CopyExternalUserInfoByIndexOptions>, IDisposable
	{
		// Token: 0x17000035 RID: 53
		// (set) Token: 0x0600039C RID: 924 RVA: 0x0000520E File Offset: 0x0000340E
		public EpicAccountId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x17000036 RID: 54
		// (set) Token: 0x0600039D RID: 925 RVA: 0x0000521E File Offset: 0x0000341E
		public EpicAccountId TargetUserId
		{
			set
			{
				Helper.Set(value, ref this.m_TargetUserId);
			}
		}

		// Token: 0x17000037 RID: 55
		// (set) Token: 0x0600039E RID: 926 RVA: 0x0000522E File Offset: 0x0000342E
		public uint Index
		{
			set
			{
				this.m_Index = value;
			}
		}

		// Token: 0x0600039F RID: 927 RVA: 0x00005238 File Offset: 0x00003438
		public void Set(ref CopyExternalUserInfoByIndexOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
			this.TargetUserId = other.TargetUserId;
			this.Index = other.Index;
		}

		// Token: 0x060003A0 RID: 928 RVA: 0x0000526C File Offset: 0x0000346C
		public void Set(ref CopyExternalUserInfoByIndexOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
				this.TargetUserId = other.Value.TargetUserId;
				this.Index = other.Value.Index;
			}
		}

		// Token: 0x060003A1 RID: 929 RVA: 0x000052CC File Offset: 0x000034CC
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_TargetUserId);
		}

		// Token: 0x04000174 RID: 372
		private int m_ApiVersion;

		// Token: 0x04000175 RID: 373
		private IntPtr m_LocalUserId;

		// Token: 0x04000176 RID: 374
		private IntPtr m_TargetUserId;

		// Token: 0x04000177 RID: 375
		private uint m_Index;
	}
}
