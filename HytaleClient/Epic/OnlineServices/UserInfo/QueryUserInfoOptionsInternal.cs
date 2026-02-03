using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UserInfo
{
	// Token: 0x02000049 RID: 73
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct QueryUserInfoOptionsInternal : ISettable<QueryUserInfoOptions>, IDisposable
	{
		// Token: 0x17000075 RID: 117
		// (set) Token: 0x0600044E RID: 1102 RVA: 0x00006159 File Offset: 0x00004359
		public EpicAccountId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x17000076 RID: 118
		// (set) Token: 0x0600044F RID: 1103 RVA: 0x00006169 File Offset: 0x00004369
		public EpicAccountId TargetUserId
		{
			set
			{
				Helper.Set(value, ref this.m_TargetUserId);
			}
		}

		// Token: 0x06000450 RID: 1104 RVA: 0x00006179 File Offset: 0x00004379
		public void Set(ref QueryUserInfoOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
			this.TargetUserId = other.TargetUserId;
		}

		// Token: 0x06000451 RID: 1105 RVA: 0x000061A0 File Offset: 0x000043A0
		public void Set(ref QueryUserInfoOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
				this.TargetUserId = other.Value.TargetUserId;
			}
		}

		// Token: 0x06000452 RID: 1106 RVA: 0x000061EB File Offset: 0x000043EB
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_TargetUserId);
		}

		// Token: 0x040001B8 RID: 440
		private int m_ApiVersion;

		// Token: 0x040001B9 RID: 441
		private IntPtr m_LocalUserId;

		// Token: 0x040001BA RID: 442
		private IntPtr m_TargetUserId;
	}
}
