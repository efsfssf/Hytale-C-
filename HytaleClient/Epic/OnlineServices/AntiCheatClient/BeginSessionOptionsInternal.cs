using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatClient
{
	// Token: 0x020006DF RID: 1759
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct BeginSessionOptionsInternal : ISettable<BeginSessionOptions>, IDisposable
	{
		// Token: 0x17000D8D RID: 3469
		// (set) Token: 0x06002D7E RID: 11646 RVA: 0x00043661 File Offset: 0x00041861
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x17000D8E RID: 3470
		// (set) Token: 0x06002D7F RID: 11647 RVA: 0x00043671 File Offset: 0x00041871
		public AntiCheatClientMode Mode
		{
			set
			{
				this.m_Mode = value;
			}
		}

		// Token: 0x06002D80 RID: 11648 RVA: 0x0004367B File Offset: 0x0004187B
		public void Set(ref BeginSessionOptions other)
		{
			this.m_ApiVersion = 3;
			this.LocalUserId = other.LocalUserId;
			this.Mode = other.Mode;
		}

		// Token: 0x06002D81 RID: 11649 RVA: 0x000436A0 File Offset: 0x000418A0
		public void Set(ref BeginSessionOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 3;
				this.LocalUserId = other.Value.LocalUserId;
				this.Mode = other.Value.Mode;
			}
		}

		// Token: 0x06002D82 RID: 11650 RVA: 0x000436EB File Offset: 0x000418EB
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
		}

		// Token: 0x04001422 RID: 5154
		private int m_ApiVersion;

		// Token: 0x04001423 RID: 5155
		private IntPtr m_LocalUserId;

		// Token: 0x04001424 RID: 5156
		private AntiCheatClientMode m_Mode;
	}
}
