using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom
{
	// Token: 0x02000531 RID: 1329
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct GetItemReleaseCountOptionsInternal : ISettable<GetItemReleaseCountOptions>, IDisposable
	{
		// Token: 0x17000A1F RID: 2591
		// (set) Token: 0x060022DC RID: 8924 RVA: 0x000339F4 File Offset: 0x00031BF4
		public EpicAccountId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x17000A20 RID: 2592
		// (set) Token: 0x060022DD RID: 8925 RVA: 0x00033A04 File Offset: 0x00031C04
		public Utf8String ItemId
		{
			set
			{
				Helper.Set(value, ref this.m_ItemId);
			}
		}

		// Token: 0x060022DE RID: 8926 RVA: 0x00033A14 File Offset: 0x00031C14
		public void Set(ref GetItemReleaseCountOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
			this.ItemId = other.ItemId;
		}

		// Token: 0x060022DF RID: 8927 RVA: 0x00033A38 File Offset: 0x00031C38
		public void Set(ref GetItemReleaseCountOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
				this.ItemId = other.Value.ItemId;
			}
		}

		// Token: 0x060022E0 RID: 8928 RVA: 0x00033A83 File Offset: 0x00031C83
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_ItemId);
		}

		// Token: 0x04000F5E RID: 3934
		private int m_ApiVersion;

		// Token: 0x04000F5F RID: 3935
		private IntPtr m_LocalUserId;

		// Token: 0x04000F60 RID: 3936
		private IntPtr m_ItemId;
	}
}
