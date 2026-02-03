using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom
{
	// Token: 0x02000521 RID: 1313
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct CopyOfferItemByIndexOptionsInternal : ISettable<CopyOfferItemByIndexOptions>, IDisposable
	{
		// Token: 0x170009FC RID: 2556
		// (set) Token: 0x06002263 RID: 8803 RVA: 0x00032732 File Offset: 0x00030932
		public EpicAccountId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x170009FD RID: 2557
		// (set) Token: 0x06002264 RID: 8804 RVA: 0x00032742 File Offset: 0x00030942
		public Utf8String OfferId
		{
			set
			{
				Helper.Set(value, ref this.m_OfferId);
			}
		}

		// Token: 0x170009FE RID: 2558
		// (set) Token: 0x06002265 RID: 8805 RVA: 0x00032752 File Offset: 0x00030952
		public uint ItemIndex
		{
			set
			{
				this.m_ItemIndex = value;
			}
		}

		// Token: 0x06002266 RID: 8806 RVA: 0x0003275C File Offset: 0x0003095C
		public void Set(ref CopyOfferItemByIndexOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
			this.OfferId = other.OfferId;
			this.ItemIndex = other.ItemIndex;
		}

		// Token: 0x06002267 RID: 8807 RVA: 0x00032790 File Offset: 0x00030990
		public void Set(ref CopyOfferItemByIndexOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
				this.OfferId = other.Value.OfferId;
				this.ItemIndex = other.Value.ItemIndex;
			}
		}

		// Token: 0x06002268 RID: 8808 RVA: 0x000327F0 File Offset: 0x000309F0
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_OfferId);
		}

		// Token: 0x04000EFD RID: 3837
		private int m_ApiVersion;

		// Token: 0x04000EFE RID: 3838
		private IntPtr m_LocalUserId;

		// Token: 0x04000EFF RID: 3839
		private IntPtr m_OfferId;

		// Token: 0x04000F00 RID: 3840
		private uint m_ItemIndex;
	}
}
