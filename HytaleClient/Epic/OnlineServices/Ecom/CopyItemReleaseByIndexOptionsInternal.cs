using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom
{
	// Token: 0x02000517 RID: 1303
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct CopyItemReleaseByIndexOptionsInternal : ISettable<CopyItemReleaseByIndexOptions>, IDisposable
	{
		// Token: 0x170009E4 RID: 2532
		// (set) Token: 0x06002230 RID: 8752 RVA: 0x000322DA File Offset: 0x000304DA
		public EpicAccountId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x170009E5 RID: 2533
		// (set) Token: 0x06002231 RID: 8753 RVA: 0x000322EA File Offset: 0x000304EA
		public Utf8String ItemId
		{
			set
			{
				Helper.Set(value, ref this.m_ItemId);
			}
		}

		// Token: 0x170009E6 RID: 2534
		// (set) Token: 0x06002232 RID: 8754 RVA: 0x000322FA File Offset: 0x000304FA
		public uint ReleaseIndex
		{
			set
			{
				this.m_ReleaseIndex = value;
			}
		}

		// Token: 0x06002233 RID: 8755 RVA: 0x00032304 File Offset: 0x00030504
		public void Set(ref CopyItemReleaseByIndexOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
			this.ItemId = other.ItemId;
			this.ReleaseIndex = other.ReleaseIndex;
		}

		// Token: 0x06002234 RID: 8756 RVA: 0x00032338 File Offset: 0x00030538
		public void Set(ref CopyItemReleaseByIndexOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
				this.ItemId = other.Value.ItemId;
				this.ReleaseIndex = other.Value.ReleaseIndex;
			}
		}

		// Token: 0x06002235 RID: 8757 RVA: 0x00032398 File Offset: 0x00030598
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_ItemId);
		}

		// Token: 0x04000EE0 RID: 3808
		private int m_ApiVersion;

		// Token: 0x04000EE1 RID: 3809
		private IntPtr m_LocalUserId;

		// Token: 0x04000EE2 RID: 3810
		private IntPtr m_ItemId;

		// Token: 0x04000EE3 RID: 3811
		private uint m_ReleaseIndex;
	}
}
