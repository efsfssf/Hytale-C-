using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatCommon
{
	// Token: 0x020006B4 RID: 1716
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct LogPlayerTakeDamageOptionsInternal : ISettable<LogPlayerTakeDamageOptions>, IDisposable
	{
		// Token: 0x17000D17 RID: 3351
		// (set) Token: 0x06002C53 RID: 11347 RVA: 0x00041572 File Offset: 0x0003F772
		public IntPtr VictimPlayerHandle
		{
			set
			{
				this.m_VictimPlayerHandle = value;
			}
		}

		// Token: 0x17000D18 RID: 3352
		// (set) Token: 0x06002C54 RID: 11348 RVA: 0x0004157C File Offset: 0x0003F77C
		public Vec3f? VictimPlayerPosition
		{
			set
			{
				Helper.Set<Vec3f, Vec3fInternal>(ref value, ref this.m_VictimPlayerPosition);
			}
		}

		// Token: 0x17000D19 RID: 3353
		// (set) Token: 0x06002C55 RID: 11349 RVA: 0x0004158D File Offset: 0x0003F78D
		public Quat? VictimPlayerViewRotation
		{
			set
			{
				Helper.Set<Quat, QuatInternal>(ref value, ref this.m_VictimPlayerViewRotation);
			}
		}

		// Token: 0x17000D1A RID: 3354
		// (set) Token: 0x06002C56 RID: 11350 RVA: 0x0004159E File Offset: 0x0003F79E
		public IntPtr AttackerPlayerHandle
		{
			set
			{
				this.m_AttackerPlayerHandle = value;
			}
		}

		// Token: 0x17000D1B RID: 3355
		// (set) Token: 0x06002C57 RID: 11351 RVA: 0x000415A8 File Offset: 0x0003F7A8
		public Vec3f? AttackerPlayerPosition
		{
			set
			{
				Helper.Set<Vec3f, Vec3fInternal>(ref value, ref this.m_AttackerPlayerPosition);
			}
		}

		// Token: 0x17000D1C RID: 3356
		// (set) Token: 0x06002C58 RID: 11352 RVA: 0x000415B9 File Offset: 0x0003F7B9
		public Quat? AttackerPlayerViewRotation
		{
			set
			{
				Helper.Set<Quat, QuatInternal>(ref value, ref this.m_AttackerPlayerViewRotation);
			}
		}

		// Token: 0x17000D1D RID: 3357
		// (set) Token: 0x06002C59 RID: 11353 RVA: 0x000415CA File Offset: 0x0003F7CA
		public bool IsHitscanAttack
		{
			set
			{
				Helper.Set(value, ref this.m_IsHitscanAttack);
			}
		}

		// Token: 0x17000D1E RID: 3358
		// (set) Token: 0x06002C5A RID: 11354 RVA: 0x000415DA File Offset: 0x0003F7DA
		public bool HasLineOfSight
		{
			set
			{
				Helper.Set(value, ref this.m_HasLineOfSight);
			}
		}

		// Token: 0x17000D1F RID: 3359
		// (set) Token: 0x06002C5B RID: 11355 RVA: 0x000415EA File Offset: 0x0003F7EA
		public bool IsCriticalHit
		{
			set
			{
				Helper.Set(value, ref this.m_IsCriticalHit);
			}
		}

		// Token: 0x17000D20 RID: 3360
		// (set) Token: 0x06002C5C RID: 11356 RVA: 0x000415FA File Offset: 0x0003F7FA
		public uint HitBoneId_DEPRECATED
		{
			set
			{
				this.m_HitBoneId_DEPRECATED = value;
			}
		}

		// Token: 0x17000D21 RID: 3361
		// (set) Token: 0x06002C5D RID: 11357 RVA: 0x00041604 File Offset: 0x0003F804
		public float DamageTaken
		{
			set
			{
				this.m_DamageTaken = value;
			}
		}

		// Token: 0x17000D22 RID: 3362
		// (set) Token: 0x06002C5E RID: 11358 RVA: 0x0004160E File Offset: 0x0003F80E
		public float HealthRemaining
		{
			set
			{
				this.m_HealthRemaining = value;
			}
		}

		// Token: 0x17000D23 RID: 3363
		// (set) Token: 0x06002C5F RID: 11359 RVA: 0x00041618 File Offset: 0x0003F818
		public AntiCheatCommonPlayerTakeDamageSource DamageSource
		{
			set
			{
				this.m_DamageSource = value;
			}
		}

		// Token: 0x17000D24 RID: 3364
		// (set) Token: 0x06002C60 RID: 11360 RVA: 0x00041622 File Offset: 0x0003F822
		public AntiCheatCommonPlayerTakeDamageType DamageType
		{
			set
			{
				this.m_DamageType = value;
			}
		}

		// Token: 0x17000D25 RID: 3365
		// (set) Token: 0x06002C61 RID: 11361 RVA: 0x0004162C File Offset: 0x0003F82C
		public AntiCheatCommonPlayerTakeDamageResult DamageResult
		{
			set
			{
				this.m_DamageResult = value;
			}
		}

		// Token: 0x17000D26 RID: 3366
		// (set) Token: 0x06002C62 RID: 11362 RVA: 0x00041636 File Offset: 0x0003F836
		public LogPlayerUseWeaponData? PlayerUseWeaponData
		{
			set
			{
				Helper.Set<LogPlayerUseWeaponData, LogPlayerUseWeaponDataInternal>(ref value, ref this.m_PlayerUseWeaponData);
			}
		}

		// Token: 0x17000D27 RID: 3367
		// (set) Token: 0x06002C63 RID: 11363 RVA: 0x00041647 File Offset: 0x0003F847
		public uint TimeSincePlayerUseWeaponMs
		{
			set
			{
				this.m_TimeSincePlayerUseWeaponMs = value;
			}
		}

		// Token: 0x17000D28 RID: 3368
		// (set) Token: 0x06002C64 RID: 11364 RVA: 0x00041651 File Offset: 0x0003F851
		public Vec3f? DamagePosition
		{
			set
			{
				Helper.Set<Vec3f, Vec3fInternal>(ref value, ref this.m_DamagePosition);
			}
		}

		// Token: 0x17000D29 RID: 3369
		// (set) Token: 0x06002C65 RID: 11365 RVA: 0x00041662 File Offset: 0x0003F862
		public Vec3f? AttackerPlayerViewPosition
		{
			set
			{
				Helper.Set<Vec3f, Vec3fInternal>(ref value, ref this.m_AttackerPlayerViewPosition);
			}
		}

		// Token: 0x06002C66 RID: 11366 RVA: 0x00041674 File Offset: 0x0003F874
		public void Set(ref LogPlayerTakeDamageOptions other)
		{
			this.m_ApiVersion = 4;
			this.VictimPlayerHandle = other.VictimPlayerHandle;
			this.VictimPlayerPosition = other.VictimPlayerPosition;
			this.VictimPlayerViewRotation = other.VictimPlayerViewRotation;
			this.AttackerPlayerHandle = other.AttackerPlayerHandle;
			this.AttackerPlayerPosition = other.AttackerPlayerPosition;
			this.AttackerPlayerViewRotation = other.AttackerPlayerViewRotation;
			this.IsHitscanAttack = other.IsHitscanAttack;
			this.HasLineOfSight = other.HasLineOfSight;
			this.IsCriticalHit = other.IsCriticalHit;
			this.HitBoneId_DEPRECATED = other.HitBoneId_DEPRECATED;
			this.DamageTaken = other.DamageTaken;
			this.HealthRemaining = other.HealthRemaining;
			this.DamageSource = other.DamageSource;
			this.DamageType = other.DamageType;
			this.DamageResult = other.DamageResult;
			this.PlayerUseWeaponData = other.PlayerUseWeaponData;
			this.TimeSincePlayerUseWeaponMs = other.TimeSincePlayerUseWeaponMs;
			this.DamagePosition = other.DamagePosition;
			this.AttackerPlayerViewPosition = other.AttackerPlayerViewPosition;
		}

		// Token: 0x06002C67 RID: 11367 RVA: 0x00041780 File Offset: 0x0003F980
		public void Set(ref LogPlayerTakeDamageOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 4;
				this.VictimPlayerHandle = other.Value.VictimPlayerHandle;
				this.VictimPlayerPosition = other.Value.VictimPlayerPosition;
				this.VictimPlayerViewRotation = other.Value.VictimPlayerViewRotation;
				this.AttackerPlayerHandle = other.Value.AttackerPlayerHandle;
				this.AttackerPlayerPosition = other.Value.AttackerPlayerPosition;
				this.AttackerPlayerViewRotation = other.Value.AttackerPlayerViewRotation;
				this.IsHitscanAttack = other.Value.IsHitscanAttack;
				this.HasLineOfSight = other.Value.HasLineOfSight;
				this.IsCriticalHit = other.Value.IsCriticalHit;
				this.HitBoneId_DEPRECATED = other.Value.HitBoneId_DEPRECATED;
				this.DamageTaken = other.Value.DamageTaken;
				this.HealthRemaining = other.Value.HealthRemaining;
				this.DamageSource = other.Value.DamageSource;
				this.DamageType = other.Value.DamageType;
				this.DamageResult = other.Value.DamageResult;
				this.PlayerUseWeaponData = other.Value.PlayerUseWeaponData;
				this.TimeSincePlayerUseWeaponMs = other.Value.TimeSincePlayerUseWeaponMs;
				this.DamagePosition = other.Value.DamagePosition;
				this.AttackerPlayerViewPosition = other.Value.AttackerPlayerViewPosition;
			}
		}

		// Token: 0x06002C68 RID: 11368 RVA: 0x00041934 File Offset: 0x0003FB34
		public void Dispose()
		{
			Helper.Dispose(ref this.m_VictimPlayerHandle);
			Helper.Dispose(ref this.m_VictimPlayerPosition);
			Helper.Dispose(ref this.m_VictimPlayerViewRotation);
			Helper.Dispose(ref this.m_AttackerPlayerHandle);
			Helper.Dispose(ref this.m_AttackerPlayerPosition);
			Helper.Dispose(ref this.m_AttackerPlayerViewRotation);
			Helper.Dispose(ref this.m_PlayerUseWeaponData);
			Helper.Dispose(ref this.m_DamagePosition);
			Helper.Dispose(ref this.m_AttackerPlayerViewPosition);
		}

		// Token: 0x04001375 RID: 4981
		private int m_ApiVersion;

		// Token: 0x04001376 RID: 4982
		private IntPtr m_VictimPlayerHandle;

		// Token: 0x04001377 RID: 4983
		private IntPtr m_VictimPlayerPosition;

		// Token: 0x04001378 RID: 4984
		private IntPtr m_VictimPlayerViewRotation;

		// Token: 0x04001379 RID: 4985
		private IntPtr m_AttackerPlayerHandle;

		// Token: 0x0400137A RID: 4986
		private IntPtr m_AttackerPlayerPosition;

		// Token: 0x0400137B RID: 4987
		private IntPtr m_AttackerPlayerViewRotation;

		// Token: 0x0400137C RID: 4988
		private int m_IsHitscanAttack;

		// Token: 0x0400137D RID: 4989
		private int m_HasLineOfSight;

		// Token: 0x0400137E RID: 4990
		private int m_IsCriticalHit;

		// Token: 0x0400137F RID: 4991
		private uint m_HitBoneId_DEPRECATED;

		// Token: 0x04001380 RID: 4992
		private float m_DamageTaken;

		// Token: 0x04001381 RID: 4993
		private float m_HealthRemaining;

		// Token: 0x04001382 RID: 4994
		private AntiCheatCommonPlayerTakeDamageSource m_DamageSource;

		// Token: 0x04001383 RID: 4995
		private AntiCheatCommonPlayerTakeDamageType m_DamageType;

		// Token: 0x04001384 RID: 4996
		private AntiCheatCommonPlayerTakeDamageResult m_DamageResult;

		// Token: 0x04001385 RID: 4997
		private IntPtr m_PlayerUseWeaponData;

		// Token: 0x04001386 RID: 4998
		private uint m_TimeSincePlayerUseWeaponMs;

		// Token: 0x04001387 RID: 4999
		private IntPtr m_DamagePosition;

		// Token: 0x04001388 RID: 5000
		private IntPtr m_AttackerPlayerViewPosition;
	}
}
