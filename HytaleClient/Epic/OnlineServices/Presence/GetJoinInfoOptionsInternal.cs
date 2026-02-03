using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Presence
{
	// Token: 0x020002C3 RID: 707
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct GetJoinInfoOptionsInternal : ISettable<GetJoinInfoOptions>, IDisposable
	{
		// Token: 0x1700052F RID: 1327
		// (set) Token: 0x06001377 RID: 4983 RVA: 0x0001C4DE File Offset: 0x0001A6DE
		public EpicAccountId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x17000530 RID: 1328
		// (set) Token: 0x06001378 RID: 4984 RVA: 0x0001C4EE File Offset: 0x0001A6EE
		public EpicAccountId TargetUserId
		{
			set
			{
				Helper.Set(value, ref this.m_TargetUserId);
			}
		}

		// Token: 0x06001379 RID: 4985 RVA: 0x0001C4FE File Offset: 0x0001A6FE
		public void Set(ref GetJoinInfoOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
			this.TargetUserId = other.TargetUserId;
		}

		// Token: 0x0600137A RID: 4986 RVA: 0x0001C524 File Offset: 0x0001A724
		public void Set(ref GetJoinInfoOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
				this.TargetUserId = other.Value.TargetUserId;
			}
		}

		// Token: 0x0600137B RID: 4987 RVA: 0x0001C56F File Offset: 0x0001A76F
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_TargetUserId);
		}

		// Token: 0x04000888 RID: 2184
		private int m_ApiVersion;

		// Token: 0x04000889 RID: 2185
		private IntPtr m_LocalUserId;

		// Token: 0x0400088A RID: 2186
		private IntPtr m_TargetUserId;
	}
}
