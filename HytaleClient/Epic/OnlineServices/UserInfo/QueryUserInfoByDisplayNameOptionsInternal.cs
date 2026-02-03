using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UserInfo
{
	// Token: 0x02000041 RID: 65
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct QueryUserInfoByDisplayNameOptionsInternal : ISettable<QueryUserInfoByDisplayNameOptions>, IDisposable
	{
		// Token: 0x17000055 RID: 85
		// (set) Token: 0x06000403 RID: 1027 RVA: 0x000059E6 File Offset: 0x00003BE6
		public EpicAccountId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x17000056 RID: 86
		// (set) Token: 0x06000404 RID: 1028 RVA: 0x000059F6 File Offset: 0x00003BF6
		public Utf8String DisplayName
		{
			set
			{
				Helper.Set(value, ref this.m_DisplayName);
			}
		}

		// Token: 0x06000405 RID: 1029 RVA: 0x00005A06 File Offset: 0x00003C06
		public void Set(ref QueryUserInfoByDisplayNameOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
			this.DisplayName = other.DisplayName;
		}

		// Token: 0x06000406 RID: 1030 RVA: 0x00005A2C File Offset: 0x00003C2C
		public void Set(ref QueryUserInfoByDisplayNameOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
				this.DisplayName = other.Value.DisplayName;
			}
		}

		// Token: 0x06000407 RID: 1031 RVA: 0x00005A77 File Offset: 0x00003C77
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_DisplayName);
		}

		// Token: 0x04000198 RID: 408
		private int m_ApiVersion;

		// Token: 0x04000199 RID: 409
		private IntPtr m_LocalUserId;

		// Token: 0x0400019A RID: 410
		private IntPtr m_DisplayName;
	}
}
