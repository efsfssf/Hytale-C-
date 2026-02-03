using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x0200038A RID: 906
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct GetConnectStringOptionsInternal : ISettable<GetConnectStringOptions>, IDisposable
	{
		// Token: 0x170006C4 RID: 1732
		// (set) Token: 0x06001857 RID: 6231 RVA: 0x000239BC File Offset: 0x00021BBC
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x170006C5 RID: 1733
		// (set) Token: 0x06001858 RID: 6232 RVA: 0x000239CC File Offset: 0x00021BCC
		public Utf8String LobbyId
		{
			set
			{
				Helper.Set(value, ref this.m_LobbyId);
			}
		}

		// Token: 0x06001859 RID: 6233 RVA: 0x000239DC File Offset: 0x00021BDC
		public void Set(ref GetConnectStringOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
			this.LobbyId = other.LobbyId;
		}

		// Token: 0x0600185A RID: 6234 RVA: 0x00023A00 File Offset: 0x00021C00
		public void Set(ref GetConnectStringOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
				this.LobbyId = other.Value.LobbyId;
			}
		}

		// Token: 0x0600185B RID: 6235 RVA: 0x00023A4B File Offset: 0x00021C4B
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_LobbyId);
		}

		// Token: 0x04000AC5 RID: 2757
		private int m_ApiVersion;

		// Token: 0x04000AC6 RID: 2758
		private IntPtr m_LocalUserId;

		// Token: 0x04000AC7 RID: 2759
		private IntPtr m_LobbyId;
	}
}
