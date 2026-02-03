using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom
{
	// Token: 0x02000515 RID: 1301
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct CopyItemImageInfoByIndexOptionsInternal : ISettable<CopyItemImageInfoByIndexOptions>, IDisposable
	{
		// Token: 0x170009DE RID: 2526
		// (set) Token: 0x06002224 RID: 8740 RVA: 0x000321D1 File Offset: 0x000303D1
		public EpicAccountId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x170009DF RID: 2527
		// (set) Token: 0x06002225 RID: 8741 RVA: 0x000321E1 File Offset: 0x000303E1
		public Utf8String ItemId
		{
			set
			{
				Helper.Set(value, ref this.m_ItemId);
			}
		}

		// Token: 0x170009E0 RID: 2528
		// (set) Token: 0x06002226 RID: 8742 RVA: 0x000321F1 File Offset: 0x000303F1
		public uint ImageInfoIndex
		{
			set
			{
				this.m_ImageInfoIndex = value;
			}
		}

		// Token: 0x06002227 RID: 8743 RVA: 0x000321FB File Offset: 0x000303FB
		public void Set(ref CopyItemImageInfoByIndexOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
			this.ItemId = other.ItemId;
			this.ImageInfoIndex = other.ImageInfoIndex;
		}

		// Token: 0x06002228 RID: 8744 RVA: 0x0003222C File Offset: 0x0003042C
		public void Set(ref CopyItemImageInfoByIndexOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
				this.ItemId = other.Value.ItemId;
				this.ImageInfoIndex = other.Value.ImageInfoIndex;
			}
		}

		// Token: 0x06002229 RID: 8745 RVA: 0x0003228C File Offset: 0x0003048C
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_ItemId);
		}

		// Token: 0x04000ED9 RID: 3801
		private int m_ApiVersion;

		// Token: 0x04000EDA RID: 3802
		private IntPtr m_LocalUserId;

		// Token: 0x04000EDB RID: 3803
		private IntPtr m_ItemId;

		// Token: 0x04000EDC RID: 3804
		private uint m_ImageInfoIndex;
	}
}
