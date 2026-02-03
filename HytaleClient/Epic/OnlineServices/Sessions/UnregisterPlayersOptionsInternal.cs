using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x02000190 RID: 400
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct UnregisterPlayersOptionsInternal : ISettable<UnregisterPlayersOptions>, IDisposable
	{
		// Token: 0x170002A9 RID: 681
		// (set) Token: 0x06000BB5 RID: 2997 RVA: 0x000111C0 File Offset: 0x0000F3C0
		public Utf8String SessionName
		{
			set
			{
				Helper.Set(value, ref this.m_SessionName);
			}
		}

		// Token: 0x170002AA RID: 682
		// (set) Token: 0x06000BB6 RID: 2998 RVA: 0x000111D0 File Offset: 0x0000F3D0
		public ProductUserId[] PlayersToUnregister
		{
			set
			{
				Helper.Set<ProductUserId>(value, ref this.m_PlayersToUnregister, out this.m_PlayersToUnregisterCount);
			}
		}

		// Token: 0x06000BB7 RID: 2999 RVA: 0x000111E6 File Offset: 0x0000F3E6
		public void Set(ref UnregisterPlayersOptions other)
		{
			this.m_ApiVersion = 2;
			this.SessionName = other.SessionName;
			this.PlayersToUnregister = other.PlayersToUnregister;
		}

		// Token: 0x06000BB8 RID: 3000 RVA: 0x0001120C File Offset: 0x0000F40C
		public void Set(ref UnregisterPlayersOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 2;
				this.SessionName = other.Value.SessionName;
				this.PlayersToUnregister = other.Value.PlayersToUnregister;
			}
		}

		// Token: 0x06000BB9 RID: 3001 RVA: 0x00011257 File Offset: 0x0000F457
		public void Dispose()
		{
			Helper.Dispose(ref this.m_SessionName);
			Helper.Dispose(ref this.m_PlayersToUnregister);
		}

		// Token: 0x0400055F RID: 1375
		private int m_ApiVersion;

		// Token: 0x04000560 RID: 1376
		private IntPtr m_SessionName;

		// Token: 0x04000561 RID: 1377
		private IntPtr m_PlayersToUnregister;

		// Token: 0x04000562 RID: 1378
		private uint m_PlayersToUnregisterCount;
	}
}
