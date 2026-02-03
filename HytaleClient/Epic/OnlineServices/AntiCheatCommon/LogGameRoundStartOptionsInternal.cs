using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatCommon
{
	// Token: 0x020006AC RID: 1708
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct LogGameRoundStartOptionsInternal : ISettable<LogGameRoundStartOptions>, IDisposable
	{
		// Token: 0x17000CF3 RID: 3315
		// (set) Token: 0x06002C0A RID: 11274 RVA: 0x000410B6 File Offset: 0x0003F2B6
		public Utf8String SessionIdentifier
		{
			set
			{
				Helper.Set(value, ref this.m_SessionIdentifier);
			}
		}

		// Token: 0x17000CF4 RID: 3316
		// (set) Token: 0x06002C0B RID: 11275 RVA: 0x000410C6 File Offset: 0x0003F2C6
		public Utf8String LevelName
		{
			set
			{
				Helper.Set(value, ref this.m_LevelName);
			}
		}

		// Token: 0x17000CF5 RID: 3317
		// (set) Token: 0x06002C0C RID: 11276 RVA: 0x000410D6 File Offset: 0x0003F2D6
		public Utf8String ModeName
		{
			set
			{
				Helper.Set(value, ref this.m_ModeName);
			}
		}

		// Token: 0x17000CF6 RID: 3318
		// (set) Token: 0x06002C0D RID: 11277 RVA: 0x000410E6 File Offset: 0x0003F2E6
		public uint RoundTimeSeconds
		{
			set
			{
				this.m_RoundTimeSeconds = value;
			}
		}

		// Token: 0x17000CF7 RID: 3319
		// (set) Token: 0x06002C0E RID: 11278 RVA: 0x000410F0 File Offset: 0x0003F2F0
		public AntiCheatCommonGameRoundCompetitionType CompetitionType
		{
			set
			{
				this.m_CompetitionType = value;
			}
		}

		// Token: 0x06002C0F RID: 11279 RVA: 0x000410FC File Offset: 0x0003F2FC
		public void Set(ref LogGameRoundStartOptions other)
		{
			this.m_ApiVersion = 2;
			this.SessionIdentifier = other.SessionIdentifier;
			this.LevelName = other.LevelName;
			this.ModeName = other.ModeName;
			this.RoundTimeSeconds = other.RoundTimeSeconds;
			this.CompetitionType = other.CompetitionType;
		}

		// Token: 0x06002C10 RID: 11280 RVA: 0x00041154 File Offset: 0x0003F354
		public void Set(ref LogGameRoundStartOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 2;
				this.SessionIdentifier = other.Value.SessionIdentifier;
				this.LevelName = other.Value.LevelName;
				this.ModeName = other.Value.ModeName;
				this.RoundTimeSeconds = other.Value.RoundTimeSeconds;
				this.CompetitionType = other.Value.CompetitionType;
			}
		}

		// Token: 0x06002C11 RID: 11281 RVA: 0x000411DE File Offset: 0x0003F3DE
		public void Dispose()
		{
			Helper.Dispose(ref this.m_SessionIdentifier);
			Helper.Dispose(ref this.m_LevelName);
			Helper.Dispose(ref this.m_ModeName);
		}

		// Token: 0x0400134D RID: 4941
		private int m_ApiVersion;

		// Token: 0x0400134E RID: 4942
		private IntPtr m_SessionIdentifier;

		// Token: 0x0400134F RID: 4943
		private IntPtr m_LevelName;

		// Token: 0x04001350 RID: 4944
		private IntPtr m_ModeName;

		// Token: 0x04001351 RID: 4945
		private uint m_RoundTimeSeconds;

		// Token: 0x04001352 RID: 4946
		private AntiCheatCommonGameRoundCompetitionType m_CompetitionType;
	}
}
