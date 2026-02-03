using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x02000396 RID: 918
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct IsRTCRoomConnectedOptionsInternal : ISettable<IsRTCRoomConnectedOptions>, IDisposable
	{
		// Token: 0x170006E3 RID: 1763
		// (set) Token: 0x0600189E RID: 6302 RVA: 0x0002403E File Offset: 0x0002223E
		public Utf8String LobbyId
		{
			set
			{
				Helper.Set(value, ref this.m_LobbyId);
			}
		}

		// Token: 0x170006E4 RID: 1764
		// (set) Token: 0x0600189F RID: 6303 RVA: 0x0002404E File Offset: 0x0002224E
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x060018A0 RID: 6304 RVA: 0x0002405E File Offset: 0x0002225E
		public void Set(ref IsRTCRoomConnectedOptions other)
		{
			this.m_ApiVersion = 1;
			this.LobbyId = other.LobbyId;
			this.LocalUserId = other.LocalUserId;
		}

		// Token: 0x060018A1 RID: 6305 RVA: 0x00024084 File Offset: 0x00022284
		public void Set(ref IsRTCRoomConnectedOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LobbyId = other.Value.LobbyId;
				this.LocalUserId = other.Value.LocalUserId;
			}
		}

		// Token: 0x060018A2 RID: 6306 RVA: 0x000240CF File Offset: 0x000222CF
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LobbyId);
			Helper.Dispose(ref this.m_LocalUserId);
		}

		// Token: 0x04000AE8 RID: 2792
		private int m_ApiVersion;

		// Token: 0x04000AE9 RID: 2793
		private IntPtr m_LobbyId;

		// Token: 0x04000AEA RID: 2794
		private IntPtr m_LocalUserId;
	}
}
