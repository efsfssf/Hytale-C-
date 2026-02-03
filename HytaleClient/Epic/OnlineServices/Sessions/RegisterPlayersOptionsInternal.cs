using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x0200013F RID: 319
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct RegisterPlayersOptionsInternal : ISettable<RegisterPlayersOptions>, IDisposable
	{
		// Token: 0x1700020A RID: 522
		// (set) Token: 0x060009B2 RID: 2482 RVA: 0x0000D701 File Offset: 0x0000B901
		public Utf8String SessionName
		{
			set
			{
				Helper.Set(value, ref this.m_SessionName);
			}
		}

		// Token: 0x1700020B RID: 523
		// (set) Token: 0x060009B3 RID: 2483 RVA: 0x0000D711 File Offset: 0x0000B911
		public ProductUserId[] PlayersToRegister
		{
			set
			{
				Helper.Set<ProductUserId>(value, ref this.m_PlayersToRegister, out this.m_PlayersToRegisterCount);
			}
		}

		// Token: 0x060009B4 RID: 2484 RVA: 0x0000D727 File Offset: 0x0000B927
		public void Set(ref RegisterPlayersOptions other)
		{
			this.m_ApiVersion = 3;
			this.SessionName = other.SessionName;
			this.PlayersToRegister = other.PlayersToRegister;
		}

		// Token: 0x060009B5 RID: 2485 RVA: 0x0000D74C File Offset: 0x0000B94C
		public void Set(ref RegisterPlayersOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 3;
				this.SessionName = other.Value.SessionName;
				this.PlayersToRegister = other.Value.PlayersToRegister;
			}
		}

		// Token: 0x060009B6 RID: 2486 RVA: 0x0000D797 File Offset: 0x0000B997
		public void Dispose()
		{
			Helper.Dispose(ref this.m_SessionName);
			Helper.Dispose(ref this.m_PlayersToRegister);
		}

		// Token: 0x04000465 RID: 1125
		private int m_ApiVersion;

		// Token: 0x04000466 RID: 1126
		private IntPtr m_SessionName;

		// Token: 0x04000467 RID: 1127
		private IntPtr m_PlayersToRegister;

		// Token: 0x04000468 RID: 1128
		private uint m_PlayersToRegisterCount;
	}
}
