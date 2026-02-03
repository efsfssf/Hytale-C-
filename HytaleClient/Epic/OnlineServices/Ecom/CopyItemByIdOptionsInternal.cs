using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom
{
	// Token: 0x02000513 RID: 1299
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct CopyItemByIdOptionsInternal : ISettable<CopyItemByIdOptions>, IDisposable
	{
		// Token: 0x170009D9 RID: 2521
		// (set) Token: 0x06002219 RID: 8729 RVA: 0x000320F1 File Offset: 0x000302F1
		public EpicAccountId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x170009DA RID: 2522
		// (set) Token: 0x0600221A RID: 8730 RVA: 0x00032101 File Offset: 0x00030301
		public Utf8String ItemId
		{
			set
			{
				Helper.Set(value, ref this.m_ItemId);
			}
		}

		// Token: 0x0600221B RID: 8731 RVA: 0x00032111 File Offset: 0x00030311
		public void Set(ref CopyItemByIdOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
			this.ItemId = other.ItemId;
		}

		// Token: 0x0600221C RID: 8732 RVA: 0x00032138 File Offset: 0x00030338
		public void Set(ref CopyItemByIdOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
				this.ItemId = other.Value.ItemId;
			}
		}

		// Token: 0x0600221D RID: 8733 RVA: 0x00032183 File Offset: 0x00030383
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_ItemId);
		}

		// Token: 0x04000ED3 RID: 3795
		private int m_ApiVersion;

		// Token: 0x04000ED4 RID: 3796
		private IntPtr m_LocalUserId;

		// Token: 0x04000ED5 RID: 3797
		private IntPtr m_ItemId;
	}
}
