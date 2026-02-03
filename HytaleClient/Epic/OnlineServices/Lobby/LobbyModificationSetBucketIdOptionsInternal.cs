using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x020003E7 RID: 999
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct LobbyModificationSetBucketIdOptionsInternal : ISettable<LobbyModificationSetBucketIdOptions>, IDisposable
	{
		// Token: 0x170007B9 RID: 1977
		// (set) Token: 0x06001B14 RID: 6932 RVA: 0x00028ADA File Offset: 0x00026CDA
		public Utf8String BucketId
		{
			set
			{
				Helper.Set(value, ref this.m_BucketId);
			}
		}

		// Token: 0x06001B15 RID: 6933 RVA: 0x00028AEA File Offset: 0x00026CEA
		public void Set(ref LobbyModificationSetBucketIdOptions other)
		{
			this.m_ApiVersion = 1;
			this.BucketId = other.BucketId;
		}

		// Token: 0x06001B16 RID: 6934 RVA: 0x00028B04 File Offset: 0x00026D04
		public void Set(ref LobbyModificationSetBucketIdOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.BucketId = other.Value.BucketId;
			}
		}

		// Token: 0x06001B17 RID: 6935 RVA: 0x00028B3A File Offset: 0x00026D3A
		public void Dispose()
		{
			Helper.Dispose(ref this.m_BucketId);
		}

		// Token: 0x04000C1F RID: 3103
		private int m_ApiVersion;

		// Token: 0x04000C20 RID: 3104
		private IntPtr m_BucketId;
	}
}
