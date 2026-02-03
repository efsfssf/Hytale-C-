using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.TitleStorage
{
	// Token: 0x020000B6 RID: 182
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct QueryFileListOptionsInternal : ISettable<QueryFileListOptions>, IDisposable
	{
		// Token: 0x17000132 RID: 306
		// (set) Token: 0x060006CD RID: 1741 RVA: 0x00009975 File Offset: 0x00007B75
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x17000133 RID: 307
		// (set) Token: 0x060006CE RID: 1742 RVA: 0x00009985 File Offset: 0x00007B85
		public Utf8String[] ListOfTags
		{
			set
			{
				Helper.Set<Utf8String>(value, ref this.m_ListOfTags, out this.m_ListOfTagsCount);
			}
		}

		// Token: 0x060006CF RID: 1743 RVA: 0x0000999B File Offset: 0x00007B9B
		public void Set(ref QueryFileListOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
			this.ListOfTags = other.ListOfTags;
		}

		// Token: 0x060006D0 RID: 1744 RVA: 0x000099C0 File Offset: 0x00007BC0
		public void Set(ref QueryFileListOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
				this.ListOfTags = other.Value.ListOfTags;
			}
		}

		// Token: 0x060006D1 RID: 1745 RVA: 0x00009A0B File Offset: 0x00007C0B
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_ListOfTags);
		}

		// Token: 0x0400033B RID: 827
		private int m_ApiVersion;

		// Token: 0x0400033C RID: 828
		private IntPtr m_LocalUserId;

		// Token: 0x0400033D RID: 829
		private IntPtr m_ListOfTags;

		// Token: 0x0400033E RID: 830
		private uint m_ListOfTagsCount;
	}
}
