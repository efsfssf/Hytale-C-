using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom
{
	// Token: 0x0200052F RID: 1327
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct GetItemImageInfoCountOptionsInternal : ISettable<GetItemImageInfoCountOptions>, IDisposable
	{
		// Token: 0x17000A1B RID: 2587
		// (set) Token: 0x060022D3 RID: 8915 RVA: 0x00033927 File Offset: 0x00031B27
		public EpicAccountId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x17000A1C RID: 2588
		// (set) Token: 0x060022D4 RID: 8916 RVA: 0x00033937 File Offset: 0x00031B37
		public Utf8String ItemId
		{
			set
			{
				Helper.Set(value, ref this.m_ItemId);
			}
		}

		// Token: 0x060022D5 RID: 8917 RVA: 0x00033947 File Offset: 0x00031B47
		public void Set(ref GetItemImageInfoCountOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
			this.ItemId = other.ItemId;
		}

		// Token: 0x060022D6 RID: 8918 RVA: 0x0003396C File Offset: 0x00031B6C
		public void Set(ref GetItemImageInfoCountOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
				this.ItemId = other.Value.ItemId;
			}
		}

		// Token: 0x060022D7 RID: 8919 RVA: 0x000339B7 File Offset: 0x00031BB7
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_ItemId);
		}

		// Token: 0x04000F59 RID: 3929
		private int m_ApiVersion;

		// Token: 0x04000F5A RID: 3930
		private IntPtr m_LocalUserId;

		// Token: 0x04000F5B RID: 3931
		private IntPtr m_ItemId;
	}
}
