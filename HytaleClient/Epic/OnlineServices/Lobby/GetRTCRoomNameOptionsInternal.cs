using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x02000390 RID: 912
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct GetRTCRoomNameOptionsInternal : ISettable<GetRTCRoomNameOptions>, IDisposable
	{
		// Token: 0x170006CE RID: 1742
		// (set) Token: 0x0600186F RID: 6255 RVA: 0x00023BC4 File Offset: 0x00021DC4
		public Utf8String LobbyId
		{
			set
			{
				Helper.Set(value, ref this.m_LobbyId);
			}
		}

		// Token: 0x170006CF RID: 1743
		// (set) Token: 0x06001870 RID: 6256 RVA: 0x00023BD4 File Offset: 0x00021DD4
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x06001871 RID: 6257 RVA: 0x00023BE4 File Offset: 0x00021DE4
		public void Set(ref GetRTCRoomNameOptions other)
		{
			this.m_ApiVersion = 1;
			this.LobbyId = other.LobbyId;
			this.LocalUserId = other.LocalUserId;
		}

		// Token: 0x06001872 RID: 6258 RVA: 0x00023C08 File Offset: 0x00021E08
		public void Set(ref GetRTCRoomNameOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LobbyId = other.Value.LobbyId;
				this.LocalUserId = other.Value.LocalUserId;
			}
		}

		// Token: 0x06001873 RID: 6259 RVA: 0x00023C53 File Offset: 0x00021E53
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LobbyId);
			Helper.Dispose(ref this.m_LocalUserId);
		}

		// Token: 0x04000AD2 RID: 2770
		private int m_ApiVersion;

		// Token: 0x04000AD3 RID: 2771
		private IntPtr m_LobbyId;

		// Token: 0x04000AD4 RID: 2772
		private IntPtr m_LocalUserId;
	}
}
