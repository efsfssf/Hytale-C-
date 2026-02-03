using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.CustomInvites
{
	// Token: 0x020005B9 RID: 1465
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct SendCustomInviteOptionsInternal : ISettable<SendCustomInviteOptions>, IDisposable
	{
		// Token: 0x17000B0C RID: 2828
		// (set) Token: 0x0600260E RID: 9742 RVA: 0x00037E79 File Offset: 0x00036079
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x17000B0D RID: 2829
		// (set) Token: 0x0600260F RID: 9743 RVA: 0x00037E89 File Offset: 0x00036089
		public ProductUserId[] TargetUserIds
		{
			set
			{
				Helper.Set<ProductUserId>(value, ref this.m_TargetUserIds, out this.m_TargetUserIdsCount);
			}
		}

		// Token: 0x06002610 RID: 9744 RVA: 0x00037E9F File Offset: 0x0003609F
		public void Set(ref SendCustomInviteOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
			this.TargetUserIds = other.TargetUserIds;
		}

		// Token: 0x06002611 RID: 9745 RVA: 0x00037EC4 File Offset: 0x000360C4
		public void Set(ref SendCustomInviteOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
				this.TargetUserIds = other.Value.TargetUserIds;
			}
		}

		// Token: 0x06002612 RID: 9746 RVA: 0x00037F0F File Offset: 0x0003610F
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_TargetUserIds);
		}

		// Token: 0x04001077 RID: 4215
		private int m_ApiVersion;

		// Token: 0x04001078 RID: 4216
		private IntPtr m_LocalUserId;

		// Token: 0x04001079 RID: 4217
		private IntPtr m_TargetUserIds;

		// Token: 0x0400107A RID: 4218
		private uint m_TargetUserIdsCount;
	}
}
