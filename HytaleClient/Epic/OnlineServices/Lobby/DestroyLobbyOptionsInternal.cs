using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x02000388 RID: 904
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct DestroyLobbyOptionsInternal : ISettable<DestroyLobbyOptions>, IDisposable
	{
		// Token: 0x170006C0 RID: 1728
		// (set) Token: 0x0600184E RID: 6222 RVA: 0x000238F0 File Offset: 0x00021AF0
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x170006C1 RID: 1729
		// (set) Token: 0x0600184F RID: 6223 RVA: 0x00023900 File Offset: 0x00021B00
		public Utf8String LobbyId
		{
			set
			{
				Helper.Set(value, ref this.m_LobbyId);
			}
		}

		// Token: 0x06001850 RID: 6224 RVA: 0x00023910 File Offset: 0x00021B10
		public void Set(ref DestroyLobbyOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
			this.LobbyId = other.LobbyId;
		}

		// Token: 0x06001851 RID: 6225 RVA: 0x00023934 File Offset: 0x00021B34
		public void Set(ref DestroyLobbyOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
				this.LobbyId = other.Value.LobbyId;
			}
		}

		// Token: 0x06001852 RID: 6226 RVA: 0x0002397F File Offset: 0x00021B7F
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_LobbyId);
		}

		// Token: 0x04000AC0 RID: 2752
		private int m_ApiVersion;

		// Token: 0x04000AC1 RID: 2753
		private IntPtr m_LocalUserId;

		// Token: 0x04000AC2 RID: 2754
		private IntPtr m_LobbyId;
	}
}
