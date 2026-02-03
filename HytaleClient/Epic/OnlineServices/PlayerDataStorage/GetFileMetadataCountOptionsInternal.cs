using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.PlayerDataStorage
{
	// Token: 0x02000300 RID: 768
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct GetFileMetadataCountOptionsInternal : ISettable<GetFileMetadataCountOptions>, IDisposable
	{
		// Token: 0x170005BC RID: 1468
		// (set) Token: 0x06001514 RID: 5396 RVA: 0x0001ECE5 File Offset: 0x0001CEE5
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x06001515 RID: 5397 RVA: 0x0001ECF5 File Offset: 0x0001CEF5
		public void Set(ref GetFileMetadataCountOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
		}

		// Token: 0x06001516 RID: 5398 RVA: 0x0001ED0C File Offset: 0x0001CF0C
		public void Set(ref GetFileMetadataCountOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
			}
		}

		// Token: 0x06001517 RID: 5399 RVA: 0x0001ED42 File Offset: 0x0001CF42
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
		}

		// Token: 0x04000941 RID: 2369
		private int m_ApiVersion;

		// Token: 0x04000942 RID: 2370
		private IntPtr m_LocalUserId;
	}
}
