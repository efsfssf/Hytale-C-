using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatCommon
{
	// Token: 0x020006B8 RID: 1720
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct LogPlayerUseAbilityOptionsInternal : ISettable<LogPlayerUseAbilityOptions>, IDisposable
	{
		// Token: 0x17000D3C RID: 3388
		// (set) Token: 0x06002C89 RID: 11401 RVA: 0x00041C26 File Offset: 0x0003FE26
		public IntPtr PlayerHandle
		{
			set
			{
				this.m_PlayerHandle = value;
			}
		}

		// Token: 0x17000D3D RID: 3389
		// (set) Token: 0x06002C8A RID: 11402 RVA: 0x00041C30 File Offset: 0x0003FE30
		public uint AbilityId
		{
			set
			{
				this.m_AbilityId = value;
			}
		}

		// Token: 0x17000D3E RID: 3390
		// (set) Token: 0x06002C8B RID: 11403 RVA: 0x00041C3A File Offset: 0x0003FE3A
		public uint AbilityDurationMs
		{
			set
			{
				this.m_AbilityDurationMs = value;
			}
		}

		// Token: 0x17000D3F RID: 3391
		// (set) Token: 0x06002C8C RID: 11404 RVA: 0x00041C44 File Offset: 0x0003FE44
		public uint AbilityCooldownMs
		{
			set
			{
				this.m_AbilityCooldownMs = value;
			}
		}

		// Token: 0x06002C8D RID: 11405 RVA: 0x00041C4E File Offset: 0x0003FE4E
		public void Set(ref LogPlayerUseAbilityOptions other)
		{
			this.m_ApiVersion = 1;
			this.PlayerHandle = other.PlayerHandle;
			this.AbilityId = other.AbilityId;
			this.AbilityDurationMs = other.AbilityDurationMs;
			this.AbilityCooldownMs = other.AbilityCooldownMs;
		}

		// Token: 0x06002C8E RID: 11406 RVA: 0x00041C8C File Offset: 0x0003FE8C
		public void Set(ref LogPlayerUseAbilityOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.PlayerHandle = other.Value.PlayerHandle;
				this.AbilityId = other.Value.AbilityId;
				this.AbilityDurationMs = other.Value.AbilityDurationMs;
				this.AbilityCooldownMs = other.Value.AbilityCooldownMs;
			}
		}

		// Token: 0x06002C8F RID: 11407 RVA: 0x00041D01 File Offset: 0x0003FF01
		public void Dispose()
		{
			Helper.Dispose(ref this.m_PlayerHandle);
		}

		// Token: 0x0400139C RID: 5020
		private int m_ApiVersion;

		// Token: 0x0400139D RID: 5021
		private IntPtr m_PlayerHandle;

		// Token: 0x0400139E RID: 5022
		private uint m_AbilityId;

		// Token: 0x0400139F RID: 5023
		private uint m_AbilityDurationMs;

		// Token: 0x040013A0 RID: 5024
		private uint m_AbilityCooldownMs;
	}
}
