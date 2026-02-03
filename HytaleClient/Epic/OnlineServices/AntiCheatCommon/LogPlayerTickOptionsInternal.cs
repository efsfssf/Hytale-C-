using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatCommon
{
	// Token: 0x020006B6 RID: 1718
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct LogPlayerTickOptionsInternal : ISettable<LogPlayerTickOptions>, IDisposable
	{
		// Token: 0x17000D31 RID: 3377
		// (set) Token: 0x06002C77 RID: 11383 RVA: 0x00041A25 File Offset: 0x0003FC25
		public IntPtr PlayerHandle
		{
			set
			{
				this.m_PlayerHandle = value;
			}
		}

		// Token: 0x17000D32 RID: 3378
		// (set) Token: 0x06002C78 RID: 11384 RVA: 0x00041A2F File Offset: 0x0003FC2F
		public Vec3f? PlayerPosition
		{
			set
			{
				Helper.Set<Vec3f, Vec3fInternal>(ref value, ref this.m_PlayerPosition);
			}
		}

		// Token: 0x17000D33 RID: 3379
		// (set) Token: 0x06002C79 RID: 11385 RVA: 0x00041A40 File Offset: 0x0003FC40
		public Quat? PlayerViewRotation
		{
			set
			{
				Helper.Set<Quat, QuatInternal>(ref value, ref this.m_PlayerViewRotation);
			}
		}

		// Token: 0x17000D34 RID: 3380
		// (set) Token: 0x06002C7A RID: 11386 RVA: 0x00041A51 File Offset: 0x0003FC51
		public bool IsPlayerViewZoomed
		{
			set
			{
				Helper.Set(value, ref this.m_IsPlayerViewZoomed);
			}
		}

		// Token: 0x17000D35 RID: 3381
		// (set) Token: 0x06002C7B RID: 11387 RVA: 0x00041A61 File Offset: 0x0003FC61
		public float PlayerHealth
		{
			set
			{
				this.m_PlayerHealth = value;
			}
		}

		// Token: 0x17000D36 RID: 3382
		// (set) Token: 0x06002C7C RID: 11388 RVA: 0x00041A6B File Offset: 0x0003FC6B
		public AntiCheatCommonPlayerMovementState PlayerMovementState
		{
			set
			{
				this.m_PlayerMovementState = value;
			}
		}

		// Token: 0x17000D37 RID: 3383
		// (set) Token: 0x06002C7D RID: 11389 RVA: 0x00041A75 File Offset: 0x0003FC75
		public Vec3f? PlayerViewPosition
		{
			set
			{
				Helper.Set<Vec3f, Vec3fInternal>(ref value, ref this.m_PlayerViewPosition);
			}
		}

		// Token: 0x06002C7E RID: 11390 RVA: 0x00041A88 File Offset: 0x0003FC88
		public void Set(ref LogPlayerTickOptions other)
		{
			this.m_ApiVersion = 3;
			this.PlayerHandle = other.PlayerHandle;
			this.PlayerPosition = other.PlayerPosition;
			this.PlayerViewRotation = other.PlayerViewRotation;
			this.IsPlayerViewZoomed = other.IsPlayerViewZoomed;
			this.PlayerHealth = other.PlayerHealth;
			this.PlayerMovementState = other.PlayerMovementState;
			this.PlayerViewPosition = other.PlayerViewPosition;
		}

		// Token: 0x06002C7F RID: 11391 RVA: 0x00041AF8 File Offset: 0x0003FCF8
		public void Set(ref LogPlayerTickOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 3;
				this.PlayerHandle = other.Value.PlayerHandle;
				this.PlayerPosition = other.Value.PlayerPosition;
				this.PlayerViewRotation = other.Value.PlayerViewRotation;
				this.IsPlayerViewZoomed = other.Value.IsPlayerViewZoomed;
				this.PlayerHealth = other.Value.PlayerHealth;
				this.PlayerMovementState = other.Value.PlayerMovementState;
				this.PlayerViewPosition = other.Value.PlayerViewPosition;
			}
		}

		// Token: 0x06002C80 RID: 11392 RVA: 0x00041BAF File Offset: 0x0003FDAF
		public void Dispose()
		{
			Helper.Dispose(ref this.m_PlayerHandle);
			Helper.Dispose(ref this.m_PlayerPosition);
			Helper.Dispose(ref this.m_PlayerViewRotation);
			Helper.Dispose(ref this.m_PlayerViewPosition);
		}

		// Token: 0x04001390 RID: 5008
		private int m_ApiVersion;

		// Token: 0x04001391 RID: 5009
		private IntPtr m_PlayerHandle;

		// Token: 0x04001392 RID: 5010
		private IntPtr m_PlayerPosition;

		// Token: 0x04001393 RID: 5011
		private IntPtr m_PlayerViewRotation;

		// Token: 0x04001394 RID: 5012
		private int m_IsPlayerViewZoomed;

		// Token: 0x04001395 RID: 5013
		private float m_PlayerHealth;

		// Token: 0x04001396 RID: 5014
		private AntiCheatCommonPlayerMovementState m_PlayerMovementState;

		// Token: 0x04001397 RID: 5015
		private IntPtr m_PlayerViewPosition;
	}
}
