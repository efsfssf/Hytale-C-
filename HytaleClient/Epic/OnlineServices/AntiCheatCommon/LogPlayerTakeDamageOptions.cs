using System;

namespace Epic.OnlineServices.AntiCheatCommon
{
	// Token: 0x020006B3 RID: 1715
	public struct LogPlayerTakeDamageOptions
	{
		// Token: 0x17000D04 RID: 3332
		// (get) Token: 0x06002C2D RID: 11309 RVA: 0x0004142F File Offset: 0x0003F62F
		// (set) Token: 0x06002C2E RID: 11310 RVA: 0x00041437 File Offset: 0x0003F637
		public IntPtr VictimPlayerHandle { get; set; }

		// Token: 0x17000D05 RID: 3333
		// (get) Token: 0x06002C2F RID: 11311 RVA: 0x00041440 File Offset: 0x0003F640
		// (set) Token: 0x06002C30 RID: 11312 RVA: 0x00041448 File Offset: 0x0003F648
		public Vec3f? VictimPlayerPosition { get; set; }

		// Token: 0x17000D06 RID: 3334
		// (get) Token: 0x06002C31 RID: 11313 RVA: 0x00041451 File Offset: 0x0003F651
		// (set) Token: 0x06002C32 RID: 11314 RVA: 0x00041459 File Offset: 0x0003F659
		public Quat? VictimPlayerViewRotation { get; set; }

		// Token: 0x17000D07 RID: 3335
		// (get) Token: 0x06002C33 RID: 11315 RVA: 0x00041462 File Offset: 0x0003F662
		// (set) Token: 0x06002C34 RID: 11316 RVA: 0x0004146A File Offset: 0x0003F66A
		public IntPtr AttackerPlayerHandle { get; set; }

		// Token: 0x17000D08 RID: 3336
		// (get) Token: 0x06002C35 RID: 11317 RVA: 0x00041473 File Offset: 0x0003F673
		// (set) Token: 0x06002C36 RID: 11318 RVA: 0x0004147B File Offset: 0x0003F67B
		public Vec3f? AttackerPlayerPosition { get; set; }

		// Token: 0x17000D09 RID: 3337
		// (get) Token: 0x06002C37 RID: 11319 RVA: 0x00041484 File Offset: 0x0003F684
		// (set) Token: 0x06002C38 RID: 11320 RVA: 0x0004148C File Offset: 0x0003F68C
		public Quat? AttackerPlayerViewRotation { get; set; }

		// Token: 0x17000D0A RID: 3338
		// (get) Token: 0x06002C39 RID: 11321 RVA: 0x00041495 File Offset: 0x0003F695
		// (set) Token: 0x06002C3A RID: 11322 RVA: 0x0004149D File Offset: 0x0003F69D
		public bool IsHitscanAttack { get; set; }

		// Token: 0x17000D0B RID: 3339
		// (get) Token: 0x06002C3B RID: 11323 RVA: 0x000414A6 File Offset: 0x0003F6A6
		// (set) Token: 0x06002C3C RID: 11324 RVA: 0x000414AE File Offset: 0x0003F6AE
		public bool HasLineOfSight { get; set; }

		// Token: 0x17000D0C RID: 3340
		// (get) Token: 0x06002C3D RID: 11325 RVA: 0x000414B7 File Offset: 0x0003F6B7
		// (set) Token: 0x06002C3E RID: 11326 RVA: 0x000414BF File Offset: 0x0003F6BF
		public bool IsCriticalHit { get; set; }

		// Token: 0x17000D0D RID: 3341
		// (get) Token: 0x06002C3F RID: 11327 RVA: 0x000414C8 File Offset: 0x0003F6C8
		// (set) Token: 0x06002C40 RID: 11328 RVA: 0x000414D0 File Offset: 0x0003F6D0
		internal uint HitBoneId_DEPRECATED { get; set; }

		// Token: 0x17000D0E RID: 3342
		// (get) Token: 0x06002C41 RID: 11329 RVA: 0x000414D9 File Offset: 0x0003F6D9
		// (set) Token: 0x06002C42 RID: 11330 RVA: 0x000414E1 File Offset: 0x0003F6E1
		public float DamageTaken { get; set; }

		// Token: 0x17000D0F RID: 3343
		// (get) Token: 0x06002C43 RID: 11331 RVA: 0x000414EA File Offset: 0x0003F6EA
		// (set) Token: 0x06002C44 RID: 11332 RVA: 0x000414F2 File Offset: 0x0003F6F2
		public float HealthRemaining { get; set; }

		// Token: 0x17000D10 RID: 3344
		// (get) Token: 0x06002C45 RID: 11333 RVA: 0x000414FB File Offset: 0x0003F6FB
		// (set) Token: 0x06002C46 RID: 11334 RVA: 0x00041503 File Offset: 0x0003F703
		public AntiCheatCommonPlayerTakeDamageSource DamageSource { get; set; }

		// Token: 0x17000D11 RID: 3345
		// (get) Token: 0x06002C47 RID: 11335 RVA: 0x0004150C File Offset: 0x0003F70C
		// (set) Token: 0x06002C48 RID: 11336 RVA: 0x00041514 File Offset: 0x0003F714
		public AntiCheatCommonPlayerTakeDamageType DamageType { get; set; }

		// Token: 0x17000D12 RID: 3346
		// (get) Token: 0x06002C49 RID: 11337 RVA: 0x0004151D File Offset: 0x0003F71D
		// (set) Token: 0x06002C4A RID: 11338 RVA: 0x00041525 File Offset: 0x0003F725
		public AntiCheatCommonPlayerTakeDamageResult DamageResult { get; set; }

		// Token: 0x17000D13 RID: 3347
		// (get) Token: 0x06002C4B RID: 11339 RVA: 0x0004152E File Offset: 0x0003F72E
		// (set) Token: 0x06002C4C RID: 11340 RVA: 0x00041536 File Offset: 0x0003F736
		public LogPlayerUseWeaponData? PlayerUseWeaponData { get; set; }

		// Token: 0x17000D14 RID: 3348
		// (get) Token: 0x06002C4D RID: 11341 RVA: 0x0004153F File Offset: 0x0003F73F
		// (set) Token: 0x06002C4E RID: 11342 RVA: 0x00041547 File Offset: 0x0003F747
		public uint TimeSincePlayerUseWeaponMs { get; set; }

		// Token: 0x17000D15 RID: 3349
		// (get) Token: 0x06002C4F RID: 11343 RVA: 0x00041550 File Offset: 0x0003F750
		// (set) Token: 0x06002C50 RID: 11344 RVA: 0x00041558 File Offset: 0x0003F758
		public Vec3f? DamagePosition { get; set; }

		// Token: 0x17000D16 RID: 3350
		// (get) Token: 0x06002C51 RID: 11345 RVA: 0x00041561 File Offset: 0x0003F761
		// (set) Token: 0x06002C52 RID: 11346 RVA: 0x00041569 File Offset: 0x0003F769
		public Vec3f? AttackerPlayerViewPosition { get; set; }
	}
}
