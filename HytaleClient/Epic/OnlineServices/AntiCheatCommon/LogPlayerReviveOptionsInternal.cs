using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatCommon
{
	// Token: 0x020006B0 RID: 1712
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct LogPlayerReviveOptionsInternal : ISettable<LogPlayerReviveOptions>, IDisposable
	{
		// Token: 0x17000CFC RID: 3324
		// (set) Token: 0x06002C1C RID: 11292 RVA: 0x0004129F File Offset: 0x0003F49F
		public IntPtr RevivedPlayerHandle
		{
			set
			{
				this.m_RevivedPlayerHandle = value;
			}
		}

		// Token: 0x17000CFD RID: 3325
		// (set) Token: 0x06002C1D RID: 11293 RVA: 0x000412A9 File Offset: 0x0003F4A9
		public IntPtr ReviverPlayerHandle
		{
			set
			{
				this.m_ReviverPlayerHandle = value;
			}
		}

		// Token: 0x06002C1E RID: 11294 RVA: 0x000412B3 File Offset: 0x0003F4B3
		public void Set(ref LogPlayerReviveOptions other)
		{
			this.m_ApiVersion = 1;
			this.RevivedPlayerHandle = other.RevivedPlayerHandle;
			this.ReviverPlayerHandle = other.ReviverPlayerHandle;
		}

		// Token: 0x06002C1F RID: 11295 RVA: 0x000412D8 File Offset: 0x0003F4D8
		public void Set(ref LogPlayerReviveOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.RevivedPlayerHandle = other.Value.RevivedPlayerHandle;
				this.ReviverPlayerHandle = other.Value.ReviverPlayerHandle;
			}
		}

		// Token: 0x06002C20 RID: 11296 RVA: 0x00041323 File Offset: 0x0003F523
		public void Dispose()
		{
			Helper.Dispose(ref this.m_RevivedPlayerHandle);
			Helper.Dispose(ref this.m_ReviverPlayerHandle);
		}

		// Token: 0x04001358 RID: 4952
		private int m_ApiVersion;

		// Token: 0x04001359 RID: 4953
		private IntPtr m_RevivedPlayerHandle;

		// Token: 0x0400135A RID: 4954
		private IntPtr m_ReviverPlayerHandle;
	}
}
