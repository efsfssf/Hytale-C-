using System;

namespace Epic.OnlineServices.AntiCheatCommon
{
	// Token: 0x020006B9 RID: 1721
	public struct LogPlayerUseWeaponData
	{
		// Token: 0x17000D40 RID: 3392
		// (get) Token: 0x06002C90 RID: 11408 RVA: 0x00041D10 File Offset: 0x0003FF10
		// (set) Token: 0x06002C91 RID: 11409 RVA: 0x00041D18 File Offset: 0x0003FF18
		public IntPtr PlayerHandle { get; set; }

		// Token: 0x17000D41 RID: 3393
		// (get) Token: 0x06002C92 RID: 11410 RVA: 0x00041D21 File Offset: 0x0003FF21
		// (set) Token: 0x06002C93 RID: 11411 RVA: 0x00041D29 File Offset: 0x0003FF29
		public Vec3f? PlayerPosition { get; set; }

		// Token: 0x17000D42 RID: 3394
		// (get) Token: 0x06002C94 RID: 11412 RVA: 0x00041D32 File Offset: 0x0003FF32
		// (set) Token: 0x06002C95 RID: 11413 RVA: 0x00041D3A File Offset: 0x0003FF3A
		public Quat? PlayerViewRotation { get; set; }

		// Token: 0x17000D43 RID: 3395
		// (get) Token: 0x06002C96 RID: 11414 RVA: 0x00041D43 File Offset: 0x0003FF43
		// (set) Token: 0x06002C97 RID: 11415 RVA: 0x00041D4B File Offset: 0x0003FF4B
		public bool IsPlayerViewZoomed { get; set; }

		// Token: 0x17000D44 RID: 3396
		// (get) Token: 0x06002C98 RID: 11416 RVA: 0x00041D54 File Offset: 0x0003FF54
		// (set) Token: 0x06002C99 RID: 11417 RVA: 0x00041D5C File Offset: 0x0003FF5C
		public bool IsMeleeAttack { get; set; }

		// Token: 0x17000D45 RID: 3397
		// (get) Token: 0x06002C9A RID: 11418 RVA: 0x00041D65 File Offset: 0x0003FF65
		// (set) Token: 0x06002C9B RID: 11419 RVA: 0x00041D6D File Offset: 0x0003FF6D
		public Utf8String WeaponName { get; set; }

		// Token: 0x06002C9C RID: 11420 RVA: 0x00041D78 File Offset: 0x0003FF78
		internal void Set(ref LogPlayerUseWeaponDataInternal other)
		{
			this.PlayerHandle = other.PlayerHandle;
			this.PlayerPosition = other.PlayerPosition;
			this.PlayerViewRotation = other.PlayerViewRotation;
			this.IsPlayerViewZoomed = other.IsPlayerViewZoomed;
			this.IsMeleeAttack = other.IsMeleeAttack;
			this.WeaponName = other.WeaponName;
		}
	}
}
